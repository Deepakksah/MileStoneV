#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.BackupRestore;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Exceptions;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.ServerAPI;
using VideoOS.Common.Interfaces;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.EventServer.Communication.Rest;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.Platform;
using VideoOS.Platform.EventsData;
using VideoOS.Platform.UI;
using VideoOS.Rules;
using VideoOS.Rules.Adapters;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.CommandHandler;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Licensing;
using VideoOS.Server.Log;
using VideoOS.Server.MIP;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Server.RuleActions;
using VideoOS.Server.ServiceChannel;
using VideoOS.Server.Tasks;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Network;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Telemetry.Configuration;
using VideoOS.Vms.Utilities.Scheduler;
using VideoOS.Vms.Utilities.SystemSettings;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Server;

public sealed class Server : IServerServiceSupport, IServer, IRuleActionDataProvider, ISecretServerRuleActionDataProvider, IDiscoveryEngine, IServerCommandAPI, IServerEnterpriseServerConnector, IServerManagement
{
	private static ILogger logger = Telemetry.GetILogger("Server");

	private static ILogger perfLogger = Telemetry.GetILogger("Performance");

	private static readonly IServerEnvironment ServerEnvironment = new ServerEnvironment();

	private SecurityToken _serverSecurityToken;

	private EvidenceLockUpdateScheduler _evidenceLockUpdateScheduler;

	private Timer _GCTimer;

	private Timer _licensecheckTimer;

	private Timer _MemoryLoggerTimer;

	private long _PeakPrivateMemorySize;

	private Process _CurrentProcess;

	private int runningRequests;

	private object runningRequestsLock = new object();

	private int runningAttachs;

	private object runningAttachsLock = new object();

	private ServiceHost _serverWebService;

	private IServiceContainer _serviceContainer;

	private static Server _serverInstance;

	private static object _serverLock = new object();

	private RuleEngine _ruleEngine;

	private readonly List<IBackgroundService> _backgroundServices = new List<IBackgroundService>();

	private DeviceDataDistributionManager deviceDataDistributionManager;

	private volatile bool _closing;

	private const long MaxTotalMetadataChars = 50000000L;

	private readonly IRestResourceConverter _restResourceConverter = new RestResourceConverter();

	public ServerTokenHandler TokenHandler { get; private set; }

	public ISqlConnectionManager SqlConnectionManager { get; private set; }

	public IServiceDiscoveryEngine DiscoveryEngine { get; private set; }

	public ISymmetricEncryption CorporateEncryptionSymmetric { get; private set; }

	public static Server Instance
	{
		get
		{
			if (_serverInstance == null)
			{
				lock (_serverLock)
				{
					if (_serverInstance == null)
					{
						_serverInstance = new Server();
					}
				}
			}
			return _serverInstance;
		}
	}

	public IDataComponentFactory DataComponentFactory { get; private set; }

	public ISqlLogFactory SqlLogFactory { get; private set; }

	public Data Data { get; private set; }

	public Guid HomeSiteId { get; private set; }

	public EventServerCommunicationManager MIPMessageManager { get; private set; }

	public IEventConsumer EVSEventConsumer => MIPMessageManager;

	public EventCollector EventCollector { get; private set; }

	public MessageManager MessageManager { get; private set; }

	public IRecorderCommManager RecorderCommManager { get; private set; }

	public ProxyResponseManager ProxyResponseManager { get; private set; }

	public NotificationManager NotificationManager { get; private set; }

	public ILicenseManager LicenseManager { get; private set; }

	public IIdpAdminClient IdpAdminClient { get; private set; }

	public LicenseFeatureChecker LicenseFeatureChecker { get; private set; }

	public ServiceChannelManager ServiceChannelManager { get; private set; }

	public SnmpManager SNMPManager { get; private set; }

	public Scheduler Scheduler { get; private set; }

	public WindowsIdentity ProcessIdentity { get; private set; }

	public ServerConfigurationLookup ConfigurationLookup { get; private set; }

	public ITokenCache ServerTokenCache { get; private set; }

	public ITokenCache ManagementServerTokenCache { get; private set; }

	public ITokenCache IdpManagementTokenCache { get; private set; }

	public ITokenCache IdpEventLogTokenCache { get; private set; }

	public ITokenCache FailoverServerTokenCache { get; private set; }

	public ITokenCache ServerLogsTokenCache { get; private set; }

	public VideoOS.Server.CommandHandler.IVmoClientFactory VmoClientFactory { get; }

	public IRuleEngine RuleEngine => _ruleEngine;

	public ILogger Logger => logger;

	public bool IsClosing => _closing;

	private IServerFileStorageCleanupService ServerFileStoragePersistedCleanupService { get; set; }

	private IServerFileStorageCleanupService ServerFileStorageTemporaryCleanupService { get; set; }

	public bool IsInitialized { get; private set; }

	public IServerFileStorage ServerFileStorage { get; private set; }

	public IServerStorageManager ServerStorageManager { get; private set; }

	public IFileUploadSynchronizer FileUploadSynchronizer { get; private set; }

	public ILocalizedLoggerFactory LocalizedLoggerFactory { get; private set; }

	public ILocalizedLogger AuditAndSystemLogger { get; private set; }

	public Version Version { get; private set; }

	Server ISecretServerRuleActionDataProvider.Server
	{
		[DebuggerStepThrough]
		get
		{
			return this;
		}
	}

	IRuleEngine IRuleActionDataProvider.RuleEngine
	{
		[DebuggerStepThrough]
		get
		{
			return RuleEngine;
		}
	}

	LicenseException IServerManagement.LastAutomaticActivationError => (LicenseException)LicenseManager.LastAutomaticActivationError;

	public event EventHandler<StatusMessageEventArgs> StatusMessage;

	public event EventHandler<ShutdownRequestedEventArgs> ShutdownRequested;

	public ICallContextInfo CreateServerServiceCallContextUserInfo()
	{
		string locationIPAddress = Dns.GetHostName();
		try
		{
			locationIPAddress = IPHelper.DnsGetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		return new BasicCallContextInfo(Instance.ProcessIdentity.User.ToString(), Instance.ProcessIdentity.Name, locationIPAddress);
	}

	public Server()
	{
		logger.LogInformation("Service starting");
		logger.LogInformation(" Version                     : {DisplayVersion}", AssemblyProductInfo.DisplayVersion);
		logger.LogInformation(" Number of processors        : {ProcessorCount}", Environment.ProcessorCount);
		logger.LogInformation(" Server GC enabled           : {IsServerGC}", GCSettings.IsServerGC);
		logger.LogInformation(" Platform                    : {Platform}-bit", IntPtr.Size * 8);
		Version version = AssemblyProductInfo.Version;
		Version = new Version(version.Major, version.Minor, 0, version.Revision);
		if (CheckDevelopmentRegistry.FakeHigherServerVersion)
		{
			Version = new Version(version.Major + 1, version.Minor, 0, version.Revision);
			logger.LogInformation("Faking server version: {Version}", Version.ToString());
		}
		ProcessIdentity = WindowsIdentity.GetCurrent();
		_evidenceLockUpdateScheduler = new EvidenceLockUpdateScheduler(ServerEnvironment);
		_GCTimer = new Timer(GCCollectTimerCallback, null, -1, -1);
		_licensecheckTimer = new Timer(LicenseCheckTimerCallback, null, -1, -1);
		_MemoryLoggerTimer = new Timer(MemoryLoggerTimerCallback, null, -1, -1);
		deviceDataDistributionManager = new DeviceDataDistributionManager(ServerEnvironment);
		Data = new Data();
		ConfigurationLookup = new ServerConfigurationLookup(this);
		DataComponentFactory = new DataComponentFactory();
		SqlLogFactory = new SqlLogFactory();
		string uriString = string.Format(CultureInfo.InvariantCulture, "http://localhost:{0}/ServerWebService", ServerConfig.Instance.ServerPort);
		_serverWebService = new ServiceHost(typeof(ServerWebService), new Uri(uriString));
		SNMPManager = new SnmpManager(!Socket.OSSupportsIPv4, ServerConfig.Instance.Snmp, logger);
		VmoClientFactory = new VmoClientsPool(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName, ServerConfig.Instance.VmoCommunicationConfiguration.Port, ServerConfig.Instance.VmoClientPoolKeepAlive, logger);
		RecorderCommManager = new RecorderCommManager(this);
		RecorderCommManager.CommunicationStateChanged += recorderCommManager_CommunicationStateChanged;
		MessageManager = new MessageManager(RecorderCommManager);
		ProxyResponseManager = new ProxyResponseManager();
		NotificationManager = new NotificationManager(this);
		_serviceContainer = ConfigureServices();
		LicenseManager.AutomaticActivationFailed += LicenseManager_AutomaticActivationFailedEventHandler;
		LicenseManager.RecordersNeedLicenseUpdate += LicenseManager_RecordersNeedLicenseUpdate;
		LicenseFeatureChecker = new LicenseFeatureChecker(LicenseManager);
		ServiceChannelManager = new ServiceChannelManager(logger, ServerConfig.Instance.VmoCommunicationConfiguration.Port);
		Scheduler = new Scheduler();
		_ruleEngine = new RuleEngine();
		_ruleEngine.StartEventQueue();
		_ruleEngine.RuleActionProcessing += ruleEngine_RuleActionProcessing;
		_ruleEngine.TriggerEventProcessing += ruleEngine_TriggerEventProcessing;
		DiscoveryEngine = new ServiceDiscoveryEngine(ServerConfig.Instance.WebApiConfig.ServiceDiscoveryCheckInterval, RecorderCommManager);
		_CurrentProcess = Process.GetCurrentProcess();
		ServerFileStoragePersistedCleanupService = new ServerFileStorageCleanupService(TimeSpan.FromSeconds(10.0), Logger);
		ServerFileStorageTemporaryCleanupService = new ServerFileStorageCleanupService(TimeSpan.FromSeconds(10.0), Logger);
		ServerFileStorage = new ServerFileStorage(new ServerFileAccess(), Logger, ServerConfig.FileStorageFolderPath, new ServerFileStorageQuota(ServerConfig.Instance.FileStorageQuotaMBLimit * 1024 * 1024), ServerFileStoragePersistedCleanupService, ServerFileStorageTemporaryCleanupService, ServerConfig.Instance.FileStoragePersistedEntryDuration, ServerConfig.Instance.FileStorageTemporaryEntryDuration);
		ServerStorageManager = new ServerStorageManager();
		FileUploadSynchronizer = new FileUploadSynchronizer(Logger, TimeSpan.FromMinutes(1.0));
	}

	private IServiceContainer ConfigureServices()
	{
		IServiceCollection serviceCollection = new ServiceCollection();
		serviceCollection.AddSingleton((ISecretProvider)new FileSecretProvider(OemResources.Instance.Server.CommonApplicationDataFolder, "idp.tmp", "idp.sec"));
		serviceCollection.AddSingleton<ISecretHandler, SecretHandler>();
		LicenseManagerCache licenseManagerCache = new LicenseManagerCache(this, this);
		serviceCollection.AddSingleton((ICachedLicenseInfo)licenseManagerCache);
		serviceCollection.AddSingleton((ILicenseManagerCache)licenseManagerCache);
		LicenseManager = new LicenseManager(Data, new LicenseManagerComponentsProvider(), ServerEnvironment, licenseManagerCache, licenseManagerCache);
		serviceCollection.AddSingleton(LicenseManager);
		serviceCollection.AddSingleton<IDataComponentFactory, DataComponentFactory>();
		serviceCollection.AddSingleton((ILogReceiver)new ServerLogReceiver(ServerConfig.Instance.LogConfiguration.Logs, ServerConfig.Instance.LogDataQueueManagerConfig.NumberOfLogsToRead));
		return ServiceContainer.CreateInstance(serviceCollection);
	}

	private void LicenseManager_RecordersNeedLicenseUpdate(object sender, EventArgs e)
	{
		ILogger iLogger = Telemetry.GetILogger("Licensing");
		iLogger?.LogInformation("LicenseManagerOnRecordersNeedLicenseUpdateEventHandler");
		foreach (KeyValuePair<Guid, byte[]> item in (sender as ILicenseManager).RecordersRequiringUpdate)
		{
			using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Instance.Data));
			License license = null;
			try
			{
				license = configurationManager.Licenses[item.Key];
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager.Licenses.Add(item.Key, item.Value);
			}
			else
			{
				license.DataAsBytes = item.Value;
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			if (changes != null)
			{
				ICallContextInfo callContextInfo = CreateServerServiceCallContextUserInfo();
				SystemConfigurationDto systemConfigurationDto = new SystemConfigurationDto(changes);
				UpdateSystemConfigurationCommand command = new UpdateSystemConfigurationCommand(systemConfigurationDto);
				IMessageManagerResponseCollector messageManagerResponseCollector = null;
				try
				{
					messageManagerResponseCollector = Instance.MessageManager.SendCommand(item.Key, command, callContextInfo);
				}
				finally
				{
					messageManagerResponseCollector?.Dispose();
				}
				iLogger?.LogInformation("LicenseManagerOnRecordersNeedLicenseUpdateEventHandler - Update command sent to {LicenseKey}", item.Key);
			}
		}
	}

	public string GetServerVersion()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		return VideoOS.Common.Integration.ServerAPI.ServerVersion.GetServerVersion(executingAssembly);
	}

	private void GCCollectTimerCallback(object state)
	{
		try
		{
			GC.Collect();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void MemoryLoggerTimerCallback(object state)
	{
		try
		{
			_CurrentProcess.Refresh();
			if (_CurrentProcess.PrivateMemorySize64 > _PeakPrivateMemorySize)
			{
				_PeakPrivateMemorySize = _CurrentProcess.PrivateMemorySize64;
			}
			string message = "Private memory: {2,7:N0} MB;  Peak: {3,7:N0} MB  - Virtual memory: {4,7:N0} MB;  Peak: {5,7:N0} MB   -   Working set: {0,7:N0} MB;  Peak: {1,7:N0} MB  - Paged memory: {6,7:N0} MB;  Peak: {7,7:N0} MB";
			perfLogger.LogInformation(message, (double)_CurrentProcess.WorkingSet64 / 1024.0 / 1024.0, (double)_CurrentProcess.PeakWorkingSet64 / 1024.0 / 1024.0, (double)_CurrentProcess.PrivateMemorySize64 / 1024.0 / 1024.0, (double)_PeakPrivateMemorySize / 1024.0 / 1024.0, (double)_CurrentProcess.VirtualMemorySize64 / 1024.0 / 1024.0, (double)_CurrentProcess.PeakVirtualMemorySize64 / 1024.0 / 1024.0, (double)_CurrentProcess.PagedMemorySize64 / 1024.0 / 1024.0, (double)_CurrentProcess.PeakPagedMemorySize64 / 1024.0 / 1024.0);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void recorderCommManager_CommunicationStateChanged(object sender, RecorderCommunicationStateEventArgs e)
	{
		LogEntryId? logEntryId = null;
		if (e.RecorderConnectionStatus == RecorderConnectionStatus.Connected && e.PreviousRecorderConnectionStatus != RecorderConnectionStatus.Connected)
		{
			logEntryId = LogEntryId.RecorderConnected;
		}
		else if (e.RecorderConnectionStatus != RecorderConnectionStatus.Connected && e.PreviousRecorderConnectionStatus == RecorderConnectionStatus.Connected)
		{
			logEntryId = LogEntryId.RecorderDisconnected;
		}
		if (logEntryId.HasValue && ServerLogManager.ShouldEntryBeLogged(LogManagerBase.GetLogEntry(logEntryId.Value, "")))
		{
			string text = string.Empty;
			try
			{
				text = ConfigurationDataManager.FindRecorderNameByRecorderId(e.RecorderId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (!string.IsNullOrEmpty(text))
			{
				ServerLogManager.Instance.AddLogEntry(logEntryId.Value, text);
			}
		}
	}

	public void Initialize()
	{
		_closing = false;
		IsInitialized = false;
		logger.LogInformation("Server initializing...");
		try
		{
			OnStatusMessage(Resources.StatusStartApplyConfiguration);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			OnStatusMessage(LogEntryId.InitializeServerApplyConfigurationFailed, ex2.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.InitializeServerApplyConfigurationFailed, EventLogEntryType.Error, ex2.Message);
			throw;
		}
		try
		{
			try
			{
				OnStatusMessage(Resources.StatusStartDatabase);
				WaitForSQLServer();
				Data.Initialize();
				TokenHandler = new ServerTokenHandler();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				OnStatusMessage(LogEntryId.InitializeDatabaseFailed, ex4.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeDatabaseFailed, EventLogEntryType.Error, ex4.Message);
				throw;
			}
			try
			{
				byte[] inArray = new KeyManager(SqlConnectionManager, Logger).LoadMasterKey();
				CorporateEncryptionSymmetric = new CorporateEncryptionSymmetric(Convert.ToBase64String(inArray));
				Logger.LogInformation("Master key has been loaded...");
			}
			catch (Exception ex5)
			{
				OnStatusMessage(LogEntryId.InitializeMasterEncryptionKeyFailed, ex5.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeMasterEncryptionKeyFailed, EventLogEntryType.Error, ex5.Message);
				throw;
			}
			try
			{
				OnStatusMessage(Resources.StatusInitializeCommunicationClients);
				ISecretHandler requiredService = _serviceContainer.GetRequiredService<ISecretHandler>();
				try
				{
					requiredService.EnsureUserEncryptedSecret();
				}
				catch (RemoveFileSecretException)
				{
					logger.LogWarning("Unable to remove secret file");
				}
				Uri authorizationServerUri = ServerEnvironment.ServerConfig.WebApiConfig.AuthorizationServerUri;
				string secret = new string(requiredService.ReadSecret());
				ServerTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "recorder", secret));
				ManagementServerTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "managementserver", secret));
				HomeSiteId = VideoOS.Server.DataComponents.ConfigurationObjects.Site.QueryHomeSite(_serviceContainer.GetRequiredService<IDataComponentFactory>()).Id;
				MIPMessageManager = new EventServerCommunicationManager(Logger, ManagementServerTokenCache);
				EventCollector = new EventCollector(MIPMessageManager, _ruleEngine, logger);
				EventCollector.Start();
				IdpManagementTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "idp_management", secret));
				IdpEventLogTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "idp_log_event", secret));
				FailoverServerTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "failover", secret));
				ServerLogsTokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, ServerEnvironment.ServerConfig.ClientRegistrationId.ToString(), "client_credentials"), "serverlogs", secret));
				ServerTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				ManagementServerTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				IdpManagementTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				IdpEventLogTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				FailoverServerTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				ServerLogsTokenCache.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				IdpAdminClient = new IdpAdminClient(IdpManagementTokenCache, authorizationServerUri);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex8)
			{
				OnStatusMessage(Resources.StatusInitializeCommunicationClientsFailed, ex8.Message);
				ServerLogManager.AddEventLogEntry(Resources.StatusInitializeCommunicationClientsFailed, EventLogEntryType.Error, ex8.Message);
				throw;
			}
			WaitForIdp();
			try
			{
				OnStatusMessage(Resources.StatusStartLogManager);
				string settingsFilePath = TelemetrySettingsConfigurator.GetSettingsFilePath(OemResources.Instance.App.CompanyName);
				TokenCacheDelegatingHandler tokenCacheDelegatinghandler = new TokenCacheDelegatingHandler((CancellationToken cancellationToken) => ServerLogsTokenCache.RefreshToken(cancellationToken));
				LocalizedLoggerFactory = Telemetry.GetILocalizedLoggerFactory("Server", () => new HttpClient((HttpMessageHandler)(object)tokenCacheDelegatinghandler), logger, settingsFilePath);
				AuditAndSystemLogger = LocalizedLoggerFactory.CreateLocalizedLogger("Server");
				ServerLogManager.Initialize(_serviceContainer.GetRequiredService<ILogReceiver>(), ServerConfig.Instance.LogConfiguration.Logs, AuditAndSystemLogger);
				using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Data));
				ServerLogManager.Instance.UpdateSystemSettings(configurationManager);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex10)
			{
				OnStatusMessage(LogEntryId.InitializeLogManagerFailed, ex10.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeLogManagerFailed, EventLogEntryType.Error, ex10.Message);
				throw;
			}
			try
			{
				OnStatusMessage(Resources.StatusStartExternalPluginConfig);
				ExternalPluginConfig.Initialize(this);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex12)
			{
				OnStatusMessage(LogEntryId.LoadExternalPluginConfigurationFailed, ex12.Message);
			}
			using (VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager2 = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Instance.Data)))
			{
				Keys.SiteSpecificPrivatePublicKey = configurationManager2.SystemSettings[SettingList.LicenseRsaKey].Value;
			}
			try
			{
				OnStatusMessage(Resources.StatusStartLicenseManager);
				lock (LicenseManager)
				{
					LicenseManager.Initialize();
				}
			}
			catch (LicenseException ex13)
			{
				OnStatusMessage(LogEntryId.LicenseInitializeFailed, string.Format(CultureInfo.InvariantCulture, Resources.InvalidTemporaryLicenseFile, ex13.ErrorNumber.ToString(CultureInfo.InvariantCulture)));
				ServerLogManager.AddEventLogEntry(LogEntryId.LicenseInitializeFailed, EventLogEntryType.Error, string.Format(CultureInfo.InvariantCulture, Resources.InvalidTemporaryLicenseFile, ex13.ErrorNumber.ToString(CultureInfo.InvariantCulture)));
				throw;
			}
			bool shouldRefreshLicenseCache = false;
			try
			{
				if (FindFirstFile(OemResources.Instance.Server.CommonApplicationDataFolder, "lic", out var fileName))
				{
					logger.LogInformation("New license file found. {LicensePath}", fileName);
					try
					{
						ApplyAndPersistLicenseFromFile(fileName);
						shouldRefreshLicenseCache = true;
					}
					catch (Exception ex14)
					{
						logger.LogWarning("Applying license from installer failed. Exception:\n{Exception}", ex14);
						throw;
					}
					finally
					{
						try
						{
							File.Delete(fileName);
						}
						catch (Exception ex15)
						{
							logger.LogError("Unable to remove license file after use, ex: {Exception}", ex15);
						}
					}
				}
				else
				{
					string empty = string.Empty;
					using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager3 = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Data));
					License license = null;
					License license2 = null;
					License license3 = null;
					try
					{
						license = configurationManager3.Licenses.ServerLicense;
					}
					catch (ConfigurationItemNotFoundException)
					{
						logger.LogWarning("The server license was not found in the database");
					}
					try
					{
						license2 = configurationManager3.Licenses.LicenseOwner;
					}
					catch (ConfigurationItemNotFoundException)
					{
						logger.LogWarning("The license owner was not found in the database");
					}
					try
					{
						license3 = configurationManager3.Licenses.SitesInformation;
					}
					catch (ConfigurationItemNotFoundException)
					{
						logger.LogWarning("The site information license was not found in the database");
					}
					if (license2 != null && license2.HasData)
					{
						UpdateLicenseOwner(license2.DataAsString);
					}
					if (license3 != null && license3.HasData)
					{
						UpdateSitesInformation(license3.DataAsString);
					}
					if (license != null && license.HasData)
					{
						UpdateLicense(license.DataAsBytes, LicenseUpdateOption.Initialize);
					}
					else if (FindFirstFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lic", out fileName))
					{
						ApplyAndPersistLicenseFromFile(fileName);
						shouldRefreshLicenseCache = true;
					}
					else
					{
						OnStatusMessage(LogEntryId.LicenseUnLicensedServer);
						ServerLogManager.AddEventLogEntry(LogEntryId.LicenseUnLicensedServer, EventLogEntryType.Warning);
					}
					try
					{
						if (LicenseManager.QueryAutomaticLicenseActivationEnabled() && LicenseManager.IsActivationRequired())
						{
							LicenseManager.QueueAutomaticActivation();
						}
					}
					catch (DatabaseException ex19)
					{
						Logger.LogWarning("AutomaticLicenseActivation: Database lookup failed while checking if automatic license activation is enabled  during startup; Exception: \n{Exception}", ex19);
					}
				}
			}
			catch (LicenseException ex20)
			{
				OnStatusMessage(LogEntryId.LicenseUpdateFailed, ex20.ErrorNumber.ToString(CultureInfo.InvariantCulture));
				ServerLogManager.AddEventLogEntry(LogEntryId.LicenseUpdateFailed, EventLogEntryType.Warning, ex20.ErrorNumber.ToString(CultureInfo.InvariantCulture));
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex22)
			{
				OnStatusMessage(LogEntryId.LicenseUpdateFailed, ex22.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.LicenseUpdateFailed, EventLogEntryType.Warning, ex22.Message);
			}
			try
			{
				CheckLicenseWithLogging();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				OnStatusMessage(Resources.StatusInitializeRuleProcessor);
				InitializeRuleEngine();
				string[] tableNames = new string[9]
				{
					ConfigurationDataSet.ReadOnlyInstance.EventTypes.TableName,
					ConfigurationDataSet.ReadOnlyInstance.DeviceGroups.TableName,
					ConfigurationDataSet.ReadOnlyInstance.DeviceGroupMembers.TableName,
					ConfigurationDataSet.ReadOnlyInstance.ServerGroup.TableName,
					ConfigurationDataSet.ReadOnlyInstance.ServerGroupMembership.TableName,
					ConfigurationDataSet.ReadOnlyInstance.TimeProfile.TableName,
					ConfigurationDataSet.ReadOnlyInstance.Rules.TableName,
					ConfigurationDataSet.ReadOnlyInstance.Recorders.TableName,
					ConfigurationDataSet.ReadOnlyInstance.RuleVariables.TableName
				};
				ConfigurationDataSet configurationDataSet = Data.LoadConfigurationDataSetTables(tableNames);
				_ruleEngine.SetConfiguration(new ConfigurationAdapter(configurationDataSet));
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex25)
			{
				OnStatusMessage(LogEntryId.InitializeServerRuleProcessorFailed, ex25.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeServerRuleProcessorFailed, EventLogEntryType.Error, ex25.Message);
			}
			WaitForVMOServerConnection(shouldRefreshLicenseCache);
			try
			{
				OnStatusMessage(Resources.StatusStartCommandProcessor);
				InitializeCommandProcessor();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex27)
			{
				OnStatusMessage(LogEntryId.InitializeServerCommandProcessorFailed, ex27.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeServerCommandProcessorFailed, EventLogEntryType.Error, ex27.Message);
				throw;
			}
			IdpLogHandler idpLogHandler = new IdpLogHandler(new VmoClient(new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName)
			{
				ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port
			}), ServerEnvironment);
			IdpLogRetriever item = new IdpLogRetriever(ServerConfig.Instance.WebApiConfig.AuthorizationServerUri, IdpEventLogTokenCache, _serviceContainer.GetRequiredService<ILogReceiver>(), _serviceContainer.GetRequiredService<IDataComponentFactory>(), ServerConfig.Instance.LogConfiguration, idpLogHandler, AuditAndSystemLogger);
			_backgroundServices.Add(item);
			try
			{
				_serverWebService.Open();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex29)
			{
				OnStatusMessage(LogEntryId.InitializeVmoServerObjectFailed, ex29.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeVmoServerObjectFailed, EventLogEntryType.Error, ex29.Message);
				throw;
			}
			StartSnmpServer();
			deviceDataDistributionManager.Start();
			try
			{
				OnStatusMessage(Resources.StatusStartRuleProcessor);
				_ruleEngine.Start();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex31)
			{
				OnStatusMessage(LogEntryId.InitializeServerRuleProcessorFailed, ex31.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeServerRuleProcessorFailed, EventLogEntryType.Error, ex31.Message);
			}
			try
			{
				OnStatusMessage(Resources.StatusStartRecorderCommunication);
				RecorderCommManager.Initialize(ServerConfig.Instance);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex33)
			{
				OnStatusMessage(LogEntryId.InitializeRecorderCommunicationFailed, ex33.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeRecorderCommunicationFailed, EventLogEntryType.Error, ex33.Message);
				throw;
			}
			try
			{
				OnStatusMessage(Resources.StatusStartTaskManager);
				ServerTaskManager.Instance.Start();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex35)
			{
				OnStatusMessage(LogEntryId.InitializeTaskManagerFailed, ex35.Message);
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeTaskManagerFailed, EventLogEntryType.Error, ex35.Message);
				throw;
			}
			try
			{
				ServerFileStorage.PurgeDataFolder();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex37)
			{
				Logger.LogError("Failed to purge the ServerFileStorage folder. Error: {ExceptionMessage}; Path: {Path}", ex37.Message, ServerFileStorage.RootDirectory);
			}
			TokenHandler.UpdateSecurityToken();
			_evidenceLockUpdateScheduler.TimeBetweenAllSizesUpdates = ServerConfig.Instance.EvidenceLockUpdateConfig.TimeBetweenUpdateAllSizes;
			_evidenceLockUpdateScheduler.TimeBetweenUknownSizeUpdates = ServerConfig.Instance.EvidenceLockUpdateConfig.TimeBetweenUpdateOfUnknownSizes;
			_evidenceLockUpdateScheduler.Start();
			if (ServerConfig.Instance.GarbageCollectInterval.Ticks > 0)
			{
				_GCTimer.Change(ServerConfig.Instance.GarbageCollectInterval, ServerConfig.Instance.GarbageCollectInterval);
			}
			if (ServerConfig.Instance.MemoryLogInterval.Ticks > 0)
			{
				_MemoryLoggerTimer.Change(ServerConfig.Instance.MemoryLogInterval, ServerConfig.Instance.MemoryLogInterval);
			}
			ServerLogManager.Instance.AddLogEntry(LogEntryId.ServerStarted);
			OnStatusMessage(LogEntryId.ServerStarted);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex39)
		{
			OnStatusMessage(LogEntryId.ServerFailed, ex39.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.ServerFailed, EventLogEntryType.Error, ex39.Message);
			throw;
		}
		try
		{
			if (ServerConfig.Instance.IsFailoverClusterMember && ServerConfig.Instance.ClusterLocalWebserverUri != null)
			{
				Task.Run(delegate
				{
					CheckAndFireMsFailoverStartEvent();
				}).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex41)
		{
			Logger.LogError("Failed to check Management server failover status during startup. Error: {ExceptionMessage}", ex41.Message);
		}
		Scheduler.ScheduleTask(new ScheduledCommand(new CleanupHardwareDefinitionsCommand(), Scheduler, TimeSpan.FromMinutes(10.0)), TimeSpan.FromMinutes(30.0));
		if (ServerConfig.Instance.EvidenceLockOldRecorderConfig.WaitTimeAfterStartup != Timeout.InfiniteTimeSpan)
		{
			Scheduler.ScheduleTask(new ScheduledCommand(new EvidenceLockOnOldRecorderCommand2()), ServerConfig.Instance.EvidenceLockOldRecorderConfig.WaitTimeAfterStartup);
		}
		foreach (IBackgroundService backgroundService in _backgroundServices)
		{
			if (!backgroundService.IsStarted)
			{
				backgroundService.Start().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
			}
		}
		logger.LogInformation("Server initialization done.");
		IsInitialized = true;
		ICachedLicenseInfo service = ServiceContainer.Instance.GetService<ICachedLicenseInfo>();
		service.ClearCachedLicenseInfo();
	}

	private static bool FindFirstFile(string directory, string fileExtension, out string fileName)
	{
		fileName = string.Empty;
		DirectoryInfo directoryInfo = new DirectoryInfo(directory);
		FileInfo[] files = directoryInfo.GetFiles("*." + fileExtension);
		if (files.Length != 0)
		{
			FileInfo fileInfo = files.FirstOrDefault((FileInfo f) => string.Equals(f.Extension, "." + fileExtension, StringComparison.OrdinalIgnoreCase));
			if (fileInfo != null)
			{
				fileName = fileInfo.FullName;
				return true;
			}
		}
		return false;
	}

	private static string GetClusterIp(string machineFqdn)
	{
		if (!ServerConfig.Instance.IsFailoverClusterMember)
		{
			return machineFqdn;
		}
		if (!IPAddress.TryParse(ServerConfig.Instance.VirtualAddress, out var address))
		{
			return machineFqdn;
		}
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
			{
				if (unicastAddress.Address.Equals(address))
				{
					return address.ToString();
				}
			}
		}
		return machineFqdn;
	}

	private void WaitForVMOServerConnection(bool shouldRefreshLicenseCache)
	{
		int num = -1;
		int num2 = 200;
		Exception ex = null;
		ServerConnection serverConnection = null;
		VmoClient vmoClient = null;
		try
		{
			serverConnection = new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName);
			serverConnection.ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port;
			vmoClient = new VmoClient(serverConnection);
			while (num != 0 && num2 > 0)
			{
				try
				{
					OnStatusMessage(Resources.StatusStartVmoClient);
					vmoClient.ServerConnection.Connect();
					vmoClient.ManagementServer.Refresh();
					if (shouldRefreshLicenseCache)
					{
						vmoClient.ManagementServer.RefreshLicenseCache();
					}
					SecurityNamespaceCache.Initialize(serverConnection);
					num = 0;
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					OnStatusMessage(LogEntryId.InitializeVmoClientFailed, ex3.Message);
					ex = ex3;
					Thread.Sleep(5000);
				}
				num2--;
			}
			if (num == 0)
			{
				logger.LogInformation("Connected to VMO. Management Server version {ServerVersion}", vmoClient.ServerConnection.ServerVersion);
				string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
				string hostName = Dns.GetHostName();
				string text = "";
				text = ((string.IsNullOrEmpty(domainName) || hostName.Contains(domainName)) ? hostName : (hostName + "." + domainName));
				text = GetClusterIp(text);
				Uri uri = UriHandling.CreateHttpUri(text, ServerConfig.Instance.VmoCommunicationConfiguration.Port);
				Uri serverUrl = UriHandling.CreateHttpsUri(text, ServerConfig.Instance.VmoCommunicationConfiguration.HttpsPort);
				Address address = new Address(uri, AddressType.Server);
				Address address2 = new Address(serverUrl, AddressType.Internal);
				List<Address> list = new List<Address>();
				bool flag = false;
				foreach (Address address3 in vmoClient.ManagementServer.Addresses)
				{
					if (address3.AddressType == AddressType.Server)
					{
						list.Add(address3);
					}
					else if (address3.ServerUrl.ToString().ToUpperInvariant() == uri.ToString().ToUpperInvariant())
					{
						list.Add(address3);
					}
					if (address3.AddressType == AddressType.Internal && address3.ServerUrl.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) && address3.ServerUrl.Port == ServerConfig.Instance.VmoCommunicationConfiguration.HttpsPort && address3.ServerUrl.Host.Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
				}
				foreach (Address item in list)
				{
					vmoClient.ManagementServer.Addresses.Remove(item);
				}
				vmoClient.ManagementServer.Addresses.Add(address);
				if (!flag)
				{
					vmoClient.ManagementServer.Addresses.Add(address2);
				}
				if (vmoClient.ManagementServer.Name.Length == 0)
				{
					vmoClient.ManagementServer.Name = Environment.MachineName;
				}
				vmoClient.ManagementServer.Update();
				vmoClient.ManagementServer.FullSiteSynchronization();
				if (vmoClient.ServerConnection.IsOpen)
				{
					vmoClient.ServerConnection.Disconnect();
				}
				return;
			}
			if (ex != null)
			{
				ServerLogManager.AddEventLogEntry(LogEntryId.InitializeVmoClientFailed, EventLogEntryType.Error, ex.Message);
			}
			throw new Exception("Timeout waiting for VMO Server.");
		}
		finally
		{
			serverConnection?.Dispose();
			vmoClient?.Dispose();
		}
	}

	private void WaitForIdp()
	{
		int num = -1;
		int num2 = 200;
		while (num != 0 && num2 > 0)
		{
			try
			{
				OnStatusMessage(Resources.StatusStartIdp);
				ServerTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				ManagementServerTokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				num = 0;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (IdpException ex2)
			{
				if (ex2.StatusCode != 503)
				{
					ServerLogManager.AddEventLogEntry(Resources.StatusErrorRegistrationIdpServer, EventLogEntryType.Error, ex2.Message);
					OnStatusMessage(Resources.StatusErrorRegistrationIdpServer);
					throw;
				}
				OnStatusMessage(Resources.StatusWaitForIdpServer);
				Thread.Sleep(5000);
			}
			catch (Exception ex3)
			{
				OnStatusMessage(Resources.StatusErrorRegistrationIdpServer);
				ServerLogManager.AddEventLogEntry(Resources.StatusErrorRegistrationIdpServer, EventLogEntryType.Error, ex3.Message);
				throw;
			}
			num2--;
		}
		if (num == 0)
		{
			logger.LogInformation("Connected to Authorization Server");
			return;
		}
		throw new Exception("Timeout waiting for Authorization Server.");
	}

	private void LicenseCheckTimerCallback(object state)
	{
		try
		{
			CheckLicenseWithLogging();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		ScheduleLicenseCheck();
	}

	private void ScheduleLicenseCheck()
	{
		try
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = utcNow.AddDays(1.0).Date.AddMinutes(2.0);
			lock (LicenseManager)
			{
				if (LicenseManager.HasLicense() && LicenseManager.LicenseExpires)
				{
					DateTime licenseExpiryDate = LicenseManager.LicenseExpiryDate;
					if (!(licenseExpiryDate < utcNow) && licenseExpiryDate < dateTime)
					{
						dateTime = licenseExpiryDate.AddSeconds(5.0);
					}
					else if (LicenseManager.NearExpiryAutomaticActivationNeeded(utcNow, licenseExpiryDate))
					{
						LicenseManager.QueueAutomaticActivation();
					}
				}
			}
			lock (_licensecheckTimer)
			{
				TimeSpan timeSpan = dateTime.Subtract(utcNow);
				_licensecheckTimer.Change((int)timeSpan.TotalMilliseconds, -1);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private void CheckLicenseWithLogging()
	{
		lock (LicenseManager)
		{
			try
			{
				LicenseManager.VerifyLicense();
				ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseValid);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				if (LicenseManager.IsLicenseExpired)
				{
					ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseExpired);
				}
				else
				{
					ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseInvalid);
				}
				throw;
			}
		}
	}

	private void ApplyAndPersistLicenseFromFile(string licensePath)
	{
		ICallContextInfo callContextInfo = CreateServerServiceCallContextUserInfo();
		byte[] licenseForStorage = File.ReadAllBytes(licensePath);
		ILicenseLoader licenseLoader = LicenseFactory.CreateLicenseLoader();
		byte[] array = licenseLoader.ConvertLicenseForStorage(licenseForStorage);
		UpdateLicense(array, LicenseUpdateOption.Initialize);
		Data.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Data));
			License license = null;
			try
			{
				license = configurationManager.Licenses.ServerLicense;
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager.Licenses.AddServerLicense(array);
			}
			else
			{
				license.DataAsBytes = array;
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
		}
		finally
		{
			Data.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private void WaitForSQLServer()
	{
		int num = -1;
		int num2 = 200;
		string text = "";
		string text2 = "";
		ISqlConnectionManager sqlConnectionManager = null;
		while (num != 0 && num2 > 0)
		{
			DatabaseException ex = null;
			try
			{
				IDatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.ServerService);
				sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, Logger);
				using DatabaseConnection databaseConnection = sqlConnectionManager.GetSqlConnection();
				text = databaseConnection.Database;
				using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand())
				{
					databaseCommand.CommandType = CommandType.Text;
					databaseCommand.CommandText = "SELECT DATABASEPROPERTY(@DatabaseName, 'IsInRecovery')";
					databaseCommand.AddParameter("@DatabaseName", SqlDbType.NVarChar, 128, databaseConnection.Database);
					using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SequentialAccess);
					if (databaseDataReader.Read())
					{
						num = databaseDataReader.GetInt32(0);
					}
				}
				if (num == 0)
				{
					using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand("SELECT Version FROM System"))
					{
						databaseCommand2.CommandType = CommandType.Text;
						text2 = (string)databaseCommand2.ExecuteScalar();
						if (string.IsNullOrEmpty(text2))
						{
							logger.LogInformation("Empty version number received from the database. Retrying...");
							num = -1;
						}
					}
					using DatabaseCommand databaseCommand3 = databaseConnection.CreateCommand("set_managementserver_hostname_setting");
					databaseCommand3.CommandType = CommandType.StoredProcedure;
					databaseCommand3.AddParameter("@settingName", SqlDbType.NVarChar, Enum.GetName(typeof(SettingList), SettingList.ManagementServerHostName));
					databaseCommand3.AddParameter("@hostName", SqlDbType.NVarChar, IPHelper.GetFullCumputerName());
					databaseCommand3.ExecuteNonQuery();
				}
			}
			catch (DatabaseException ex2)
			{
				ex = ex2;
			}
			if (num != 0)
			{
				if (num2 == 200)
				{
					OnStatusMessage(Resources.StatusWaitForSqlServer);
				}
				logger.LogWarning("{StatusWaitForSqlServer}", Resources.StatusWaitForSqlServer);
				if (ex != null)
				{
					logger.LogError("{ExceptionMessage}", ex.Message);
				}
				Thread.Sleep(TimeSpan.FromSeconds(3.0));
				num2--;
			}
		}
		if (num == 0)
		{
			SqlConnectionManager = sqlConnectionManager;
			logger.LogInformation("Connected to database {Database}. Database version {DatabaseVersion}", text, text2);
		}
		if (num2 == 0)
		{
			throw new TimeoutException("Timeout waiting for database.");
		}
	}

	public void RequestShutdown(string message)
	{
		this.ShutdownRequested?.Invoke(this, new ShutdownRequestedEventArgs(message));
	}

	public void Close(bool skipDatabaseLog)
	{
		_closing = true;
		logger.LogInformation("Service stopping");
		OnStatusMessage(Resources.StatusServerShutdown);
		if (!skipDatabaseLog)
		{
			ServerLogManager.Instance.AddLogEntry(LogEntryId.ServerStopped, true);
		}
		deviceDataDistributionManager?.Stop();
		deviceDataDistributionManager?.Dispose();
		deviceDataDistributionManager = null;
		MIPMessageManager?.Close();
		EvidenceLockUpdateScheduler evidenceLockUpdateScheduler = _evidenceLockUpdateScheduler;
		if (evidenceLockUpdateScheduler != null && evidenceLockUpdateScheduler.IsStarted)
		{
			_evidenceLockUpdateScheduler?.Stop();
		}
		_evidenceLockUpdateScheduler?.Dispose();
		_evidenceLockUpdateScheduler = null;
		_GCTimer?.Dispose();
		_GCTimer = null;
		_MemoryLoggerTimer?.Change(-1, -1);
		_MemoryLoggerTimer?.Dispose();
		_MemoryLoggerTimer = null;
		CloseAndDisposeLicenseManager();
		IdpAdminClient?.Dispose();
		IdpAdminClient = null;
		CloseCommandProcessor();
		CloseStatusRuleProcessor();
		EventCollector?.Stop();
		RecorderCommManager?.Close(ServerConfig.Instance);
		VmoClientFactory?.Dispose();
		CloseSnmpManager();
		CloseTokenHandler();
		DisposeServerTokenCache();
		CloseServerFileStorage();
		CloseServerLogManager();
		foreach (IBackgroundService backgroundService in _backgroundServices)
		{
			if (backgroundService.IsStarted)
			{
				backgroundService.Stop().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
			}
			if (backgroundService is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		_serviceContainer?.Dispose();
		OnStatusMessage(LogEntryId.ShutdownServerComplete);
		Thread.Sleep(1000);
		logger.LogInformation("Service stopped");
		LocalizedLoggerFactory?.Dispose();
		ServerLogsTokenCache?.Dispose();
	}

	private void CloseAndDisposeLicenseManager()
	{
		if (LicenseManager != null)
		{
			LicenseManager.AutomaticActivationFailed -= LicenseManager_AutomaticActivationFailedEventHandler;
			LicenseManager.RecordersNeedLicenseUpdate -= LicenseManager_RecordersNeedLicenseUpdate;
			LicenseManager.Dispose();
			LicenseManager = null;
		}
	}

	private void CloseCommandProcessor()
	{
		OnStatusMessage(Resources.StatusCommandProcessorShutdown);
		try
		{
			CommandProcessor.Close();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			OnStatusMessage(LogEntryId.ShutdownCommandProcessorFailed, ex2.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.ShutdownCommandProcessorFailed, EventLogEntryType.Error, ex2.Message);
		}
	}

	private void CloseStatusRuleProcessor()
	{
		OnStatusMessage(Resources.StatusRuleProcessorShutdown);
		try
		{
			_ruleEngine?.Stop();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			OnStatusMessage(LogEntryId.ShutdownRuleProcessorFailed, ex2.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.ShutdownRuleProcessorFailed, EventLogEntryType.Error, ex2.Message);
		}
	}

	private void CloseSnmpManager()
	{
		OnStatusMessage(Resources.StatusStopSNMPManager);
		try
		{
			SNMPManager?.Stop();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			OnStatusMessage(LogEntryId.ShutdownServerSnmpManagerFailed, ex2.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.ShutdownServerSnmpManagerFailed, EventLogEntryType.Error, ex2.Message);
		}
	}

	private void CloseTokenHandler()
	{
		try
		{
			TokenHandler?.Close();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private void DisposeServerTokenCache()
	{
		try
		{
			ServerTokenCache?.Dispose();
			ManagementServerTokenCache?.Dispose();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private void CloseServerFileStorage()
	{
		try
		{
			ServerFileStoragePersistedCleanupService?.Dispose();
			ServerFileStorageTemporaryCleanupService?.Dispose();
			ServerFileStorage?.Dispose();
			FileUploadSynchronizer?.Dispose();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private static void CloseServerLogManager()
	{
		try
		{
			ServerLogManager.Instance.Close();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private void InitializeRuleEngine()
	{
		RegisterActionFactory(new DeviceStartRuleActionFactory(this));
		RegisterActionFactory(new DeviceStopRuleActionFactory(this));
		RegisterActionFactory(new DeviceStartStopFeedRuleActionFactory(this));
		RegisterActionFactory(new DeviceStartPluginRuleActionFactory(this));
		RegisterActionFactory(new DeviceStopPluginRuleActionFactory(this));
		RegisterActionFactory(new DeviceRecordingRuleActionFactory(this));
		RegisterActionFactory(new DeviceRecordingFramerateRuleActionFactory(this));
		RegisterActionFactory(new DeviceChangeSettingsRuleActionFactory(this));
		RegisterActionFactory(new DeviceFramerateRuleActionFactory(this));
		RegisterActionFactory(new LogServerEntryRuleActionFactory(this));
		RegisterActionFactory(new NotificationRuleActionFactory(this));
		RegisterActionFactory(new SetDeviceOutputRuleActionFactory(this));
		RegisterActionFactory(new SetBookmarkRuleActionFactory(this));
		RegisterActionFactory(new DeviceStartStopPatrollingRuleActionFactory(this));
		RegisterActionFactory(new DevicePauseResumePatrollingRuleActionFactory(this));
		RegisterActionFactory(new GotoPresetRuleActionFactory(this));
		RegisterActionFactory(new GotoDefaultPresetRuleActionFactory(this));
		RegisterActionFactory(new PlaySoundRuleActionFactory(this));
		RegisterActionFactory(new SetMatrixViewRuleActionFactory(this));
		RegisterActionFactory(new SendSNMPTrapRuleActionFactory(this));
		RegisterActionFactory(new ActivateUserDefinedEventOnRemoteSiteRuleActionFactory(this));
		RegisterActionFactory(new ActivateArchivingRuleActionFactory(this));
		RegisterActionFactory(new DeviceEdgeRetrievalRuleActionFactory(this));
		RegisterActionFactory(new DeviceEdgeRetrievalTimeOrMetadataRuleActionFactory(this));
		RegisterActionFactory(new DeviceRecordAllFramesActionFactory(this));
		RegisterActionFactory(new MIPRuleActionFactory(this));
		RegisterActionFactory(new MIPRuleAction2Factory(this));
		RegisterActionFactory(new ChangeHardwarePasswordRuleActionFactory(this));
		InitializeVideoWallRuleEngine();
		_ruleEngine.ActionPluginsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "RuleActions");
		_ruleEngine.DynamicRegisterRuleActions(this);
	}

	private void InitializeVideoWallRuleEngine()
	{
		RegisterActionFactory(new SetVideoWallPresetRuleActionFactory(this));
		RegisterActionFactory(new SetMonitorViewRuleActionFactory(this));
		RegisterActionFactory(new SetMonitorCamerasRuleActionFactory(this));
		RegisterActionFactory(new SetMonitorTextRuleActionFactory(this));
		RegisterActionFactory(new RemoveMonitorCamerasRuleActionFactory(this));
	}

	private void RegisterActionFactory(RuleActionFactoryBase ruleActionFactory)
	{
		_ruleEngine.RegisterActionFactory(ruleActionFactory);
	}

	private void ruleEngine_TriggerEventProcessing(object sender, TriggerEventEventArgs e)
	{
		if (RuleEngine.Logger == null || !RuleEngine.Logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug) || e.TriggerEvent.EventTypeId == SourceGuid.PeriodicTimerEvent)
		{
			return;
		}
		Data.ConfigurationDataSetCacheLock.EnterReadLock();
		try
		{
			SourceType sourceType = ConfigurationLookup.FindEventSourceTypeFromEventTypeID(e.TriggerEvent.EventTypeId);
			string text = ConfigurationLookup.FindSourceDisplayName(e.TriggerEvent.SourceId, sourceType);
			string text2 = ConfigurationLookup.FindEventDisplayNameFromEventTypeID(e.TriggerEvent.EventTypeId, Resources.EventNameUnknown);
			RuleEngine.Logger.LogDebug("Event triggered. Event name: {EventName}, Source: {SourceDisplayName}", text2, text);
		}
		finally
		{
			Data.ConfigurationDataSetCacheLock.ExitReadLock();
		}
	}

	public object InternalGetNamedObject(string objectName)
	{
		if (!(objectName == "Server"))
		{
			if (objectName == "RuleEngine")
			{
				return RuleEngine;
			}
			return null;
		}
		return this;
	}

	private void ruleEngine_RuleActionProcessing(object sender, RuleActionProcessingEventArgs e)
	{
		if (e.RuleAction.HandlesAllActionTargets || e.TriggerEvent.IsRemoteEvent)
		{
			return;
		}
		bool flag = !e.TriggerEvent.IsGlobal;
		RuleActionExecutionRequest ruleActionExecutionRequest = null;
		foreach (ActionTarget expandedTarget in e.ExpandedTargets)
		{
			if (expandedTarget.TargetType == ActionTargetType.Device && flag)
			{
				if (ruleActionExecutionRequest == null)
				{
					ruleActionExecutionRequest = new RuleActionExecutionRequest(e.Rule.ID, e.RuleAction.InstanceId, e.TriggerEvent, new ActionTargetCollection());
				}
				ruleActionExecutionRequest.ExpandedTargets.Add(expandedTarget);
			}
		}
		if (ruleActionExecutionRequest == null)
		{
			return;
		}
		try
		{
			ExecuteRuleActionsCommand command = new ExecuteRuleActionsCommand(Guid.Empty, new RuleActionExecutionRequest[1] { ruleActionExecutionRequest });
			ICallContextInfo state = Instance.CreateServerServiceCallContextUserInfo();
			CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			logger.LogWarning("Failed to process ExecuteRuleActionsCommand from the RuleProcessed event handler. Error: {ExceptionMessage}", ex2.Message);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.FailedToDistributeRuleActions, e.Rule.Name, ex2.Message);
		}
		foreach (ActionTarget expandedTarget2 in ruleActionExecutionRequest.ExpandedTargets)
		{
			e.ExpandedTargets.Remove(expandedTarget2);
		}
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType(), response.GetType());
	}

	public bool HasDeviceEvidenceLock(string deviceId)
	{
		return ConfigurationLookup.DeviceHasEvidenceLock(deviceId);
	}

	public void EventTriggerNoSecurity(Guid eventId, ICallContextInfo currentCallContextInfo)
	{
		try
		{
			TriggerEvent triggerEvent = new TriggerEvent(SourceGuid.External, eventId);
			Instance.RuleEngine.AddTriggerEvent(triggerEvent);
			RecorderCommManager.TriggerCustomEventAsync(triggerEvent, currentCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
			try
			{
				VideoOS.Server.DataComponents.ConfigurationObjects.UserDefinedEvent userDefinedEvent = VideoOS.Server.DataComponents.ConfigurationObjects.UserDefinedEvent.QueryUserDefinedEvent(eventId);
				string name = userDefinedEvent.Name;
				ServerLogManager.Instance.ServerLogEntryAuditLogger.AuditUserDefinedEventActivated(eventId, name, currentCallContextInfo.UserDomainAccountName, currentCallContextInfo.LocationIPAddress);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void EventTriggerWithMetaDataNoSecurity(Guid eventId, bool isDeviceEvent, Guid sourceId, EventMetaDataCollection metadata, ICallContextInfo currentCallContextInfo)
	{
		EventTracer.Log.TraceMethodStart("EventTriggerWithMetaDataNoSecurity");
		TriggerEvent triggerEvent = new TriggerEvent(sourceId, eventId, metadata)
		{
			HasBeenProcessedByEventServer = (sourceId == SourceGuid.External)
		};
		Instance.RuleEngine.AddTriggerEvent(triggerEvent);
		if (isDeviceEvent)
		{
			Guid serviceId = ConfigurationDataManager.FindActiveRecorderServiceIdByDeviceId(sourceId);
			RecorderCommManager.TriggerCustomEventAsync(serviceId, triggerEvent, currentCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
		}
		else
		{
			RecorderCommManager.TriggerCustomEventAsync(triggerEvent, currentCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
		}
		EventTracer.Log.TraceMethodStop("EventTriggerWithMetaDataNoSecurity");
	}

	public void TriggerBuiltInEventForDevice(Guid eventId, Guid deviceId, DateTime time, ICallContextInfo currentCallContextInfo)
	{
		try
		{
			TriggerEvent triggerEvent = new TriggerEvent(deviceId, eventId, time);
			Guid serviceId = ConfigurationDataManager.FindActiveRecorderServiceIdByDeviceId(deviceId);
			RecorderCommManager.TriggerCustomEventAsync(serviceId, triggerEvent, currentCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	public void UpdateSystemConfiguration(ConfigurationDataSet configurationChanges, ICallContextInfo callContextInfo)
	{
		try
		{
			LicenseFeatureChecker.CheckConfigurationDataSet(configurationChanges);
			if (configurationChanges != null)
			{
				Data.ConfigurationDataSetCacheLock.EnterWriteLock();
				try
				{
					Data.WriteConfigurationDataSetChangesToDatabase(configurationChanges, callContextInfo);
					return;
				}
				finally
				{
					Data.ConfigurationDataSetCacheLock.ExitWriteLock();
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void UpdateSecurityConfiguration(SecurityManagerDataTransferObject securityConfigurationDto)
	{
		try
		{
			LicenseFeatureChecker.CheckSecurityDataSet(securityConfigurationDto.DataSet);
			Data.WriteSecurityDataSetChangesToDatabase(securityConfigurationDto.DataSet);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (ServerServiceException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public CompleteConfigurationDto GetCompleteConfiguration(Guid recorderId, Guid recorderServiceId)
	{
		SystemConfigurationDto systemConfigurationDto = ConfigurationManagerAssembler.CreateSystemConfigurationWithVersionDto(recorderId, recorderServiceId, this);
		SecurityConfigurationDto securityConfigurationDto = SecurityManagerAssembler.CreateDataSetWithVersionDto(recorderId, this);
		SecurityTokenCollectionDto securityTokensDto = SecurityTokenCollectionDto.Create(TokenHandler.GetAllTokens());
		return new CompleteConfigurationDto(ServerResponseStatusDto.ResponseReady, systemConfigurationDto, securityConfigurationDto, securityTokensDto);
	}

	public CompleteConfigurationDto GetCompleteConfigurationDtoLegacy(Guid recorderServiceId, bool configDto, bool securityDto, bool tokensDto)
	{
		if (!RequestGetCompleteConfigurationWork())
		{
			Random random = new Random();
			Thread.Sleep(random.Next(ServerConfig.Instance.RecorderCommunicationConfiguration.ConnectWaitMin, ServerConfig.Instance.RecorderCommunicationConfiguration.ConnectWaitMax));
			return new CompleteConfigurationDto(ServerResponseStatusDto.ServerTooBusy, null, null, null);
		}
		try
		{
			return GetCompleteConfigurationDto(recorderServiceId, configDto, securityDto, tokensDto);
		}
		finally
		{
			ReleaseGetCompleteConfigurationWork();
		}
	}

	public CompleteConfigurationDto GetCompleteConfigurationDto(Guid recorderServiceId, bool configDto, bool securityDto, bool tokensDto)
	{
		try
		{
			SystemConfigurationDto systemConfigurationDto = null;
			SecurityConfigurationDto securityConfigurationDto = null;
			SecurityTokenCollectionDto securityTokenCollectionDto = null;
			Guid recorderId = RecorderCommManager.GetRecorderId(recorderServiceId);
			if (configDto)
			{
				systemConfigurationDto = ConfigurationManagerAssembler.CreateSystemConfigurationWithVersionDto(recorderId, recorderServiceId, this);
			}
			if (securityDto)
			{
				securityConfigurationDto = SecurityManagerAssembler.CreateDataSetWithVersionDto(recorderId, this);
			}
			if (tokensDto)
			{
				securityTokenCollectionDto = SecurityTokenCollectionDto.Create(TokenHandler.GetAllTokens());
			}
			ConfigurationVersionDto currentConfigurationVersions;
			try
			{
				currentConfigurationVersions = RecorderCommManager.GetCurrentConfigurationVersions(recorderServiceId);
				if (currentConfigurationVersions == null)
				{
					return new CompleteConfigurationDto(ServerResponseStatusDto.ReconnectRequired, null, null, null);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (RecorderNotFoundException)
			{
				return new CompleteConfigurationDto(ServerResponseStatusDto.ReconnectRequired, null, null, null);
			}
			if (systemConfigurationDto != null)
			{
				systemConfigurationDto.LastUpdate = currentConfigurationVersions.ConfigVersion;
				systemConfigurationDto.NextUpdate = -1L;
			}
			if (securityConfigurationDto != null)
			{
				securityConfigurationDto.LastUpdate = currentConfigurationVersions.SecurityVersion;
				securityConfigurationDto.NextUpdate = -1L;
			}
			if (securityTokenCollectionDto != null)
			{
				securityTokenCollectionDto.LastUpdate = currentConfigurationVersions.TokenVersion;
				securityTokenCollectionDto.NextUpdate = -1L;
			}
			if (configDto && securityDto && tokensDto)
			{
				OnStatusMessage(Resources.StatusSendCompleteConfiguration, recorderId);
			}
			return new CompleteConfigurationDto(ServerResponseStatusDto.ResponseReady, systemConfigurationDto, securityConfigurationDto, securityTokenCollectionDto);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			throw HandleException(ex4);
		}
	}

	public bool RequestGetCompleteConfigurationWork()
	{
		lock (runningRequestsLock)
		{
			if (runningRequests >= RecorderCommManager.MaxSimultaniousRecorderConfigRequests)
			{
				return false;
			}
			runningRequests++;
		}
		return true;
	}

	public void ReleaseGetCompleteConfigurationWork()
	{
		lock (runningRequestsLock)
		{
			runningRequests--;
		}
	}

	public ServerResponseStatusDto SetRecorderSpecificConfiguration(Guid serviceId, RecorderSpecificConfigurationDto recorderSpecificConfiguration, bool forceUpdate, ICallContextInfo callContextInfo)
	{
		Logger.LogDebug("SetRecorderSpecificConfiguration. Service id: {ServiceId}", serviceId);
		lock (runningRequestsLock)
		{
			if (!forceUpdate && runningRequests >= RecorderCommManager.MaxSimultaniousRecorderConfigRequests)
			{
				return ServerResponseStatusDto.ServerTooBusy;
			}
			runningRequests++;
		}
		try
		{
			if (recorderSpecificConfiguration.SystemConfigurationDto.DataSet != null)
			{
				Data.ConfigurationDataSetCacheLock.EnterWriteLock();
				try
				{
					Data.WriteConfigurationDataSetChangesToDatabase(recorderSpecificConfiguration.SystemConfigurationDto.DataSet, callContextInfo);
				}
				finally
				{
					Data.ConfigurationDataSetCacheLock.ExitWriteLock();
				}
			}
			if (recorderSpecificConfiguration.AdditionalConfiguration != null)
			{
				KeyValueDto[] additionalConfiguration = recorderSpecificConfiguration.AdditionalConfiguration;
				foreach (KeyValueDto keyValueDto in additionalConfiguration)
				{
					string key = keyValueDto.Key;
					if (key == "EventTypes")
					{
						UpdateEventTypes(keyValueDto.Value, callContextInfo);
					}
				}
			}
			Logger.LogDebug("SetRecorderSpecificConfiguration done. Service id: {ServiceId}", serviceId);
			lock (runningRequestsLock)
			{
				runningRequests--;
			}
			return ServerResponseStatusDto.ResponseReady;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogWarning("SetRecorderSpecificConfiguration exception. Service id: {ServiceId}, Exception:\r\n{Exception}", serviceId, ex2);
			lock (runningRequestsLock)
			{
				runningRequests--;
			}
			throw HandleException(ex2);
		}
	}

	public static void UpdateRecorderStatusData(Guid recorderId, Guid serviceId, string type, string data)
	{
		if (!(type == "ArchiveInfo"))
		{
			if (type == "RepairInfo")
			{
				TaskHandling.UpdateReparingTasks(recorderId, serviceId, data);
			}
			else
			{
				ConfigurationDataManager.UpdateRecorderStatusData(recorderId, type, data);
			}
		}
		else
		{
			TaskHandling.UpdateArchivingTasks(recorderId, serviceId, data);
		}
	}

	public void UpdateEventTypes(string eventDefinitions, ICallContextInfo callContextInfo)
	{
		if (string.IsNullOrEmpty(eventDefinitions))
		{
			return;
		}
		try
		{
			Data.ConfigurationDataSetCacheLock.EnterWriteLock();
			try
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Data));
				ConfigurationDataSet.UpdateEventTypes(configurationManager, eventDefinitions);
				ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
				if (changes == null)
				{
					return;
				}
				Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
				RecorderConfigData[] array = RecorderConfigData.QueryRecorders(DataComponentFactory);
				RecorderConfigData[] array2 = array;
				foreach (RecorderConfigData recorderConfigData in array2)
				{
					if (!recorderConfigData.Enabled)
					{
						continue;
					}
					Guid[] array3 = RecorderCommManager.QueryRecorderServiceIds(recorderConfigData.Id);
					foreach (Guid serviceId in array3)
					{
						try
						{
							RecorderCommManager.UpdateSystemConfigurationAsync(serviceId, new SystemConfigurationDto(changes), callContextInfo);
						}
						catch (RecorderNotFoundException)
						{
						}
						catch (NotSupportedByLegacyRecorderException)
						{
							ServerLogManager.Instance.AddLogEntry(LogEntryId.LegacyRecorderNoEventUpdates, recorderConfigData.Name);
						}
					}
				}
			}
			finally
			{
				Data.ConfigurationDataSetCacheLock.ExitWriteLock();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			logger.LogError("Server.UpdateEventTypes exception: {Exception}", ex4);
		}
	}

	public void UpdateRuleConfigurationFromDiffGram(ConfigurationDataSet configurationDataSetDiff, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		_ruleEngine.UpdateConfigurationFromDiffGram(new DiffGramConfigurationAdapter(configurationDataSetDiff, responseSink, callContextInfo));
	}

	public SecurityTokenCollectionDto GetSecurityTokensDto()
	{
		Trace.WriteLine("GetSecurityTokensDto");
		try
		{
			return SecurityTokenCollectionDto.Create(TokenHandler.GetAllTokens());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public SecurityTokenWithServerTimeDto2 GetSecurityTokenDtoWithManagementClientToken(string token)
	{
		Trace.WriteLine("GetSecurityTokenDtoWithManagementClientToken");
		try
		{
			return SecurityTokenWithServerTimeDto2.Create(TokenHandler.GetToken(token));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public SecurityTokenWithServerTimeDto GetSecurityTokenDto(string token)
	{
		Trace.WriteLine("GetSecurityTokenDto");
		try
		{
			return SecurityTokenWithServerTimeDto.Create(TokenHandler.GetToken(token));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public SecurityToken GetLocalSecurityToken(int timeoutMinutes)
	{
		if (_serverSecurityToken == null || _serverSecurityToken.IsExpired || _serverSecurityToken.IsCloseToExpire)
		{
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(Instance.ProcessIdentity.User.Value);
			_serverSecurityToken = TokenHandler.GetNewToken(securityIdentifier.Value, new TimeSpan(0, timeoutMinutes, 0));
		}
		return _serverSecurityToken;
	}

	public void ProcessPendingTasks(string recordingServerId)
	{
		if (string.IsNullOrEmpty(recordingServerId) || recordingServerId.Equals("909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			ServerTaskManager.Instance.CheckForTasksAvailable();
		}
		else
		{
			RecorderCommManager.ProcessPendingTasks(new Guid(recordingServerId));
		}
	}

	public bool IsRecorderOnline(Guid recordingServerId)
	{
		return RecorderCommManager.IsRecorderOnline(recordingServerId);
	}

	public ServerTaskDto[] GetRecorderTasks(Guid serviceId, long sequenceNumber)
	{
		return TaskHandling.GetPendingTasks(serviceId, sequenceNumber);
	}

	public static ServerTaskDto[] GetRecorderTasksWithPendingStop(Guid serviceId)
	{
		return TaskHandling.GetTasksWithPendingStop(serviceId);
	}

	public void RemoveRecorderTask(long taskId)
	{
		TaskHandling.RemoveTask(taskId);
	}

	public string UpdateRecorderTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		return TaskHandling.UpdateTaskProgress(taskId, percentComplete, details, status, endTime);
	}

	public static string StopRecorderTask(long taskId)
	{
		return TaskHandling.StopTask(taskId);
	}

	public static Guid[] FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid recorderId, Guid[] trackIds)
	{
		Guid[] result = new Guid[0];
		try
		{
			Guid[] tracksKnown = ConfigurationDataManager.QueryTracksWithPossibleMediaOnRecorder(recorderId);
			result = trackIds.Where((Guid t) => !tracksKnown.Contains(t)).ToArray();
		}
		catch (DatabaseException ex)
		{
			logger.LogError("Error using SQL database: {Exception}", ex);
		}
		return result;
	}

	public void PromoteNewEncryptionPassword(Guid recordingStorageId)
	{
		try
		{
			ConfigurationDataManager.PromoteNewEncryptionPassword(recordingStorageId);
			ProcessPendingTasks(ConfigurationDataManager.FindRecorderIdByRecordingStorageId(recordingStorageId).ToString());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public IDictionary<Guid, int> UpdateDriverNumbers(IDictionary<Guid, int> toUpdate)
	{
		try
		{
			IDictionary<Guid, int> dictionary = null;
			using (IDriverNumberComponent driverNumberComponent = DataComponentFactory.CreateDriverNumberComponent())
			{
				dictionary = driverNumberComponent.UpdateDriverNumbers(toUpdate);
			}
			if (dictionary != null && dictionary.Any())
			{
				ConfigurationDataSet ds = ConstructDatasetWithDriverNumbers(dictionary);
				UpdateConfigurationManagerCommand command = new UpdateConfigurationManagerCommand(new ConfigurationManagerDataTransferObject(ds));
				ICallContextInfo state = Instance.CreateServerServiceCallContextUserInfo();
				CommandProcessor.ProcessAsync(command, state, UpdateConfigurationManagerCommandCallback);
			}
			return dictionary;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	private static ConfigurationDataSet ConstructDatasetWithDriverNumbers(IDictionary<Guid, int> result)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		foreach (KeyValuePair<Guid, int> item in result)
		{
			ConfigurationDataSet.DriverNumbersRow driverNumbersRow = configurationDataSet.DriverNumbers.AddDriverNumbersRow(item.Key);
			driverNumbersRow.DriverNumber = item.Value;
		}
		return configurationDataSet;
	}

	private void UpdateConfigurationManagerCommandCallback(CommandBase command, ResponseBase response)
	{
		if (command is UpdateConfigurationManagerCommand updateConfigurationManagerCommand)
		{
			updateConfigurationManagerCommand.ConfigurationManagerDto.DataSet.Dispose();
		}
	}

	private Identity GetIdentityFromTokenEx(string token)
	{
		TokenStatus tokenStatus;
		SecurityToken token2 = TokenHandler.GetToken(token, out tokenStatus);
		return tokenStatus switch
		{
			TokenStatus.Expired => throw new ServerApiException(Resources.IServerApiExpiredToken), 
			TokenStatus.Invalid => throw new ServerApiException(Resources.IServerApiInvalidToken), 
			_ => token2.Identity, 
		};
	}

	private void InitializeCommandProcessor()
	{
		CommandProcessor.Logger = logger;
		CommandProcessor.Register(typeof(UpdateConfigurationManagerCommand), new UpdateConfigurationManagerCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(UpdateSecurityManagerCommand), new UpdateSecurityManagerCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(AnalyzeUpdateSecurityManagerCommand), new AnalyzeUpdateSecurityManagerCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(SendNotificationCommand), new SendNotificationCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(SendMultiNotificationCommand), new SendMultiNotificationCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(RecorderStatusCommand), new RecorderStatusCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(UpdateHardwareReadOnlySettingsCommand), new UpdateHardwareReadOnlySettingsCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(UpdateHardwareDriverCommand), new UpdateHardwareDriverCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(SetRecorderShutdownStateCommand), new SetRecorderShutdownStateCommandHandler(), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(ConfigureRulesCommand), new ConfigureRulesCommandHandler(_ruleEngine), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ExecuteRuleActionsCommand), new ExecuteRuleActionsCommandHandler(ServerEnvironment, this), ExecutorType.Queued);
		CommandProcessor.Register(typeof(DatabaseOperationCommand), new DatabaseOperationCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(UpdateRecorderStatusCommand), new UpdateRecorderStatusCommandHandler(this), ExecutorType.Queued);
		CommandProcessor.Register(typeof(HardwareScanCommand), new HardwareScanCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(GetAbsolutePtzPositionCommand), new GetAbsolutePtzPositionCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(PtzReserveSessionCommand), new PtzReserveSessionCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(PtzReleaseSessionCommand), new PtzReleaseSessionCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(PtzStopSessionCommand), new PtzStopSessionCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(HardwareSetupInformationCommand), new HardwareSetupInformationCommandHandler(ServerEnvironment, this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(SynchronizeDevicePresetsCommand), new SynchronizeDevicePresetsCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(SetMatrixViewCommand), new SetMatrixViewCommandHandler(this), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(TriggerEventAsGlobalCommand), new TriggerEventAsGlobalCommandHandler(this), ExecutorType.Queued);
		CommandProcessor.Register(typeof(VideoWallCommand), new VideoWallCommandHandler(VmoClientFactory), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ActivateUserDefinedEventOnRemoteSiteCommand), new ActivateUserDefinedEventOnRemoteSiteCommandHandler(), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ReportMediaStatusCommand), new ReportMediaStatusCommandHandler(), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(AuditLogCommand), new AuditLogCommandHandler(ServerEnvironment), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(ReplaceHardwareCommand), new ReplaceHardwareCommandHandler(ServerEnvironment), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(ServerFileSystemOperationCommand), new ServerFileSystemOperationCommandHandler(), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(AddBookmarkCommand), new AddBookmarkCommandHandler(VmoClientFactory), ExecutorType.ThreadPool);
		CommandProcessor.Register(typeof(ActivateArchivingCommand), new ActivateArchivingCommandHandler(), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(CleanupHardwareDefinitionsCommand), new CleanupHardwareDefinitionsCommandHandler(), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(BackupConfigCommand), new BackupConfigCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(EdgeStorageRetrievalCommand), new EdgeTaskCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ReportMediaCommittedToDatabaseCommand), new ReportMediaCommittedToDatabaseCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(ReportManualRecordingStateCommand), new ReportManualRecordingStateCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(EdgeJobUpdatedCommand), new EdgeJobUpdatedCommandHandler(this), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(MIPActionExecuteCommand), new MIPActionExecuteCommandHandler(this), ExecutorType.Queued);
		CommandProcessor.Register(typeof(MIPAction2ExecuteCommand), new MIPAction2ExecuteCommandHandler(this), ExecutorType.Queued);
		CommandProcessor.Register(typeof(DeleteConfigurationItemCommand), new DeleteConfigurationItemCommandHandler(ServerEnvironment, new ConfigurationDataSetLoader(Data)), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(SetHardwareNetworkSettingsCommand), new SetHardwareNetworkSettingsCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(GetHardwareNetworkSettingsCommand), new GetHardwareNetworkSettingsCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(PingDeviceCommand), new PingDeviceCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(GetHardwareUsersCommand), new GetHardwareUsersCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(DeleteHardwareUserCommand), new DeleteHardwareUserCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(AddHardwareUserCommand), new AddHardwareUserCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(UpdateEvidenceLockSizesCommand), new UpdateEvidenceLockSizesCommandHandler(ServerEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(EvidenceLockOnOldRecorderCommand2), new EvidenceLockOnOldRecorderCommandHandler(ServerEnvironment), ExecutorType.Thread);
		CommandProcessor.Register(typeof(SetupInitialHardwareUserCommand), new SetupInitialHardwareUserCommandHandler(ServerEnvironment), ExecutorType.Queued);
		CommandProcessor.Register(typeof(FirmwareUpgradeCommand), new FirmwareUpgradeCommandHandler(ServerEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(UploadFileChunkCommand), new UploadFileChunkCommandHandler(ServerEnvironment, ServerLogManager.Instance), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(FileUploadToRecorderCommand), new FileUploadToRecorderCommandHandler(ServerEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(CheckExternalRecorderRegistrationCommand), new CheckExternalRecorderRegistrationCommandHandler(ServerEnvironment), ExecutorType.AdvancedThreadPool);
		CommandProcessor.Register(typeof(CheckMsFailoverStatusCommand), new CheckMsFailoverStatusCommandHandler(ServerEnvironment, new DPAPIProtectionProvider()), ExecutorType.AdvancedThreadPool);
	}

	private Exception HandleException(Exception ex)
	{
		Trace.WriteLine("HandleException - " + ex.ToString());
		logger.LogError("Unhandled exception on server interface: {Exception}", ex);
		if (ex is DBConcurrencyException)
		{
			return new DatabaseConcurrencyException(Resources.IServerSqlConcurrencyError, ex);
		}
		if (ex is DatabaseException ex2)
		{
			if (ex2.Number == 10054)
			{
				ServerLogManager.Instance.AddLogEntry(LogEntryId.DatabaseConnectionLost);
			}
			switch (ex2.Number)
			{
			case 547:
				return new DatabaseConcurrencyException(Resources.IServerSqlConcurrencyError, ex);
			case 2601:
			case 2627:
				return new ServerServiceException(Resources.IServerSqlUniqueIndexOrConstraintViolation, ex);
			case 65000:
				return new ServerServiceException(DatabaseResources.RegisterSecurityObjectFailed, ex2.Number, ex);
			case 65001:
				return new ServerServiceException(DatabaseResources.UnregisterSecurityObjectFailed, ex2.Number, ex);
			default:
				return new ServerServiceException(Resources.IServerSqlError, ex);
			}
		}
		if (ex is ServerServiceException)
		{
			return ex;
		}
		return new ServerServiceException(Resources.IServerInternalError, ex);
	}

	public void LogInternal(LogEntry logEntry)
	{
		try
		{
			ServerLogManager.Instance.AddLogEntry(logEntry);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void LogInternal(LogEntry[] logEntries)
	{
		try
		{
			ServerLogManager.Instance.AddLogEntries(logEntries);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void HandleRecorderConnectedLegacy(Guid recorderId, ICallContextInfo callContextInfo)
	{
		if (!RequestAttachRecorderWork())
		{
			Random random = new Random();
			Thread.Sleep(random.Next(ServerConfig.Instance.RecorderCommunicationConfiguration.ConnectWaitMin, ServerConfig.Instance.RecorderCommunicationConfiguration.ConnectWaitMax));
			throw new TimeoutException("Unable to obtain the configuration lock within the specified time.");
		}
		try
		{
			HandleRecorder(recorderId, callContextInfo);
		}
		finally
		{
			ReleaseAttachRecorderWork();
		}
	}

	public void HandleRecorder(Guid recorderId, ICallContextInfo callContextInfo)
	{
		byte[] array = RequestRecorderLicense(recorderId);
		using VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Data));
		Data.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			License license = null;
			try
			{
				license = configurationManager.Licenses[recorderId];
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager.Licenses.Add(recorderId, array);
			}
			else
			{
				license.DataAsBytes = array;
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
		}
		finally
		{
			Data.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	public bool RequestAttachRecorderWork()
	{
		lock (runningAttachsLock)
		{
			if (runningAttachs >= RecorderCommManager.MaxSimultaniousRecorderConfigRequests)
			{
				return false;
			}
			runningAttachs++;
		}
		return true;
	}

	public void ReleaseAttachRecorderWork()
	{
		lock (runningAttachsLock)
		{
			runningAttachs--;
		}
	}

	public byte[] RequestRecorderLicense(Guid recorderId)
	{
		lock (LicenseManager)
		{
			return LicenseManager.GetRecorderLicense(recorderId);
		}
	}

	public void UpdateLicenseOwner(string ownerLicense)
	{
		lock (LicenseManager)
		{
			LicenseManager.LicenseOwnerXml = ownerLicense;
		}
	}

	public void UpdateSitesInformation(string sitesInfo)
	{
		lock (LicenseManager)
		{
			LicenseManager.SitesInformationXml = sitesInfo;
		}
	}

	public void UpdateLicense(byte[] licenseBytes, LicenseUpdateOption updateOption)
	{
		try
		{
			lock (LicenseManager)
			{
				ILicenseLoader licenseLoader = LicenseFactory.CreateLicenseLoader();
				ILicenseDocument newLicense = licenseLoader.CreateLicenseDocument(licenseBytes);
				LicenseManager.UpdateLicense(newLicense, updateOption);
			}
			OnStatusMessage(Resources.StatusLicenseUpdated);
		}
		catch (LicenseException ex)
		{
			OnStatusMessage(LogEntryId.LicenseUpdateFailed, ex.ErrorNumber.ToString(CultureInfo.InvariantCulture));
			ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseUpdateFailed, ex.ErrorNumber.ToString(CultureInfo.InvariantCulture));
			throw;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			OnStatusMessage(LogEntryId.LicenseUpdateFailed, ex3.Message);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseUpdateFailed, ex3.Message);
			throw;
		}
		finally
		{
			ScheduleLicenseCheck();
		}
	}

	public void OnStatusMessage(string message)
	{
		if (this.StatusMessage != null && !string.IsNullOrEmpty(message))
		{
			logger.LogInformation("{StatusMessage}", message);
			this.StatusMessage(this, new StatusMessageEventArgs(DateTime.UtcNow, message));
		}
	}

	public void OnStatusMessage(string messageFormat, params object[] args)
	{
		string message = string.Format(CultureInfo.InvariantCulture, messageFormat, args);
		OnStatusMessage(message);
	}

	public void OnStatusMessage(LogEntryId logEntryId, params string[] arguments)
	{
		string logDescription = LogManagerBase.GetLogDescription(logEntryId, arguments);
		if (this.StatusMessage != null && !string.IsNullOrEmpty(logDescription))
		{
			logger.LogInformation("{Description}", logDescription);
			this.StatusMessage(this, new StatusMessageEventArgs(DateTime.UtcNow, logDescription));
		}
	}

	private void StartSnmpServer()
	{
		try
		{
			OnStatusMessage(Resources.StatusStartSNMPManager);
			if (SNMPManager != null)
			{
				SNMPManager.Port = ServerConfig.Instance.Snmp.RemotingPort;
				SNMPManager.Start();
			}
			OnStatusMessage(Resources.StatusSNMPManagerStarted);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (SnmpServiceNotStartedException)
		{
			ServerInfoCollector serverInfoCollector = new ServerInfoCollector();
			LogEntrySystem logEntry = new LogEntrySystem(LogEntryId.ServerSnmpServiceNotStarted, serverInfoCollector.ManagementServerId(), SourceType.Server, serverInfoCollector.ManagementServerName(), string.Empty);
			ServerLogManager.Instance.AddLogEntry(logEntry);
		}
		catch (SnmpServiceNotInstalledException)
		{
			ServerInfoCollector serverInfoCollector2 = new ServerInfoCollector();
			LogEntrySystem logEntry2 = new LogEntrySystem(LogEntryId.ServerSnmpServiceNotInstalled, serverInfoCollector2.ManagementServerId(), SourceType.Server, serverInfoCollector2.ManagementServerName(), string.Empty);
			ServerLogManager.Instance.AddLogEntry(logEntry2);
		}
		catch (Exception ex4)
		{
			OnStatusMessage(LogEntryId.InitializeServerSnmpManagerFailed, ex4.Message);
			ServerLogManager.AddEventLogEntry(LogEntryId.InitializeServerSnmpManagerFailed, EventLogEntryType.Error, ex4.Message);
			throw;
		}
	}

	public void UpdateLicenseThroughUpdateConfigurationManagerCommand(byte[] encryptedLicense, string ownerXml, string sitesInformation)
	{
		bool flag = encryptedLicense != null && encryptedLicense.Length != 0;
		ILicenseDocument licenseDocument = (flag ? LoadLicenseDocument() : null);
		if (flag)
		{
			ConfigurationDataManager.CheckConfigurationAgainstLicense(licenseDocument);
		}
		ILicenseLoader licenseLoader = LicenseFactory.CreateLicenseLoader();
		byte[] license = licenseLoader.ConvertLicenseForStorage(encryptedLicense);
		using (VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager = new VideoOS.Common.Integration.Configuration.ConfigurationManager(new ConfigurationDataSetLoader(Instance.Data)))
		{
			if (flag)
			{
				UpdateDatabaseTypeInConfigurationManager(licenseDocument, configurationManager);
				UpdateLicenseInConfigurationManager(license, configurationManager);
			}
			if (ownerXml != null)
			{
				UpdateOwnerInformationInConfigurationManager(ownerXml, configurationManager);
			}
			if (sitesInformation != null)
			{
				UpdateSitesInformationInConfigurationManager(sitesInformation, configurationManager);
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			if (changes != null)
			{
				ConfigurationManagerDataTransferObject configurationManagerDto = new ConfigurationManagerDataTransferObject(changes);
				UpdateConfigurationManagerCommand updateConfigurationManagerCommand = new UpdateConfigurationManagerCommand(configurationManagerDto);
				updateConfigurationManagerCommand.PersistChanges = true;
				ICallContextInfo state = CreateServerServiceCallContextUserInfo();
				CommandProcessor.ProcessSync(updateConfigurationManagerCommand, state, new NullResponseSink());
			}
		}
		ILicenseDocument LoadLicenseDocument()
		{
			ILicenseLoader licenseLoader2 = LicenseFactory.CreateLicenseLoader();
			return licenseLoader2.ImportLicenseFile(encryptedLicense);
		}
	}

	private static void UpdateLicenseInConfigurationManager(byte[] license, VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager)
	{
		License license2 = null;
		try
		{
			license2 = configurationManager.Licenses.ServerLicense;
		}
		catch (ConfigurationItemNotFoundException)
		{
		}
		if (license2 == null)
		{
			configurationManager.Licenses.AddServerLicense(license);
			return;
		}
		license2.DataAsBytes = license;
		license2.LastModified = DateTime.UtcNow;
	}

	private static void UpdateDatabaseTypeInConfigurationManager(ILicenseDocument document, VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager)
	{
		CorporateProductType corporateProductType;
		try
		{
			corporateProductType = (CorporateProductType)Enum.Parse(typeof(CorporateProductType), document.ProductSku, ignoreCase: true);
		}
		catch (ArgumentException)
		{
			throw new LicenseException("Product SKU not valid", LicenseErrorNumber.InvalidLicenseFile);
		}
		catch (OverflowException)
		{
			throw new LicenseException("Product SKU not valid", LicenseErrorNumber.InvalidLicenseFile);
		}
		if (!Enum.IsDefined(typeof(CorporateProductType), corporateProductType))
		{
			throw new LicenseException("Product SKU not valid", LicenseErrorNumber.InvalidLicenseFile);
		}
	}

	private static void UpdateSitesInformationInConfigurationManager(string sitesInformation, VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager)
	{
		License license = null;
		try
		{
			license = configurationManager.Licenses.SitesInformation;
		}
		catch (ConfigurationItemNotFoundException)
		{
		}
		if (license == null)
		{
			configurationManager.Licenses.AddSitesInformation(sitesInformation);
			return;
		}
		license.DataAsString = sitesInformation;
		license.LastModified = DateTime.UtcNow;
	}

	private static void UpdateOwnerInformationInConfigurationManager(string ownerXml, VideoOS.Common.Integration.Configuration.ConfigurationManager configurationManager)
	{
		License license = null;
		try
		{
			license = configurationManager.Licenses.LicenseOwner;
		}
		catch (ConfigurationItemNotFoundException)
		{
		}
		if (license == null)
		{
			configurationManager.Licenses.AddLicenseOwner(ownerXml);
			return;
		}
		if (!string.IsNullOrEmpty(ownerXml) && license.HasData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(ownerXml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Customer/EditOwnerLink");
			if (xmlNode == null || string.IsNullOrEmpty(xmlNode.InnerXml))
			{
				XmlDocument xmlDocument2 = new XmlDocument();
				xmlDocument2.LoadXml(license.DataAsString);
				XmlNode xmlNode2 = xmlDocument2.SelectSingleNode("Customer/EditOwnerLink");
				string text = ((xmlNode2 != null) ? xmlNode2.InnerXml : string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					XmlElement xmlElement = xmlDocument.CreateElement("EditOwnerLink");
					xmlElement.InnerXml = text;
					XmlNode xmlNode3 = xmlDocument.SelectSingleNode("Customer");
					if (xmlNode3 != null)
					{
						xmlNode3.AppendChild(xmlElement);
						ownerXml = xmlDocument.OuterXml;
					}
				}
			}
		}
		license.DataAsString = ownerXml;
		license.LastModified = DateTime.UtcNow;
	}

	private void LicenseManager_AutomaticActivationFailedEventHandler(object sender, EventArgs e)
	{
		RuleEngine.AddTriggerEvent(new TriggerEvent(SourceGuid.Server, BuiltInEventTypes.EventIDs.AutomaticActivationFailed));
	}

	private void CheckAndFireMsFailoverStartEvent()
	{
		try
		{
			ICallContextInfo state = CreateServerServiceCallContextUserInfo();
			CheckMsFailoverStatusCommand command = new CheckMsFailoverStatusCommand(shouldInvokeNodeStartEvent: true);
			IResponseCollector responseCollector = ServerEnvironment.LocalCommandProcessor.ProcessCommandAsync(command, state);
			if (responseCollector.GetNextResponse(TimeSpan.FromSeconds(30.0)) is MsFailoverStatusResponse { InvokeFailoverStartedEvent: not false })
			{
				ServerInfoCollector serverInfoCollector = new ServerInfoCollector();
				Thread.Sleep(TimeSpan.FromSeconds(160.0));
				RuleEngine.AddTriggerEvent(new TriggerEvent(serverInfoCollector.ManagementServerId(), BuiltInEventTypes.EventIDs.ManagementServerFailoverNodeStarted));
				LogEntrySystem logEntry = new LogEntrySystem(LogEntryId.MsFailoverNodeStartedEventFired, serverInfoCollector.ManagementServerId(), SourceType.Server, serverInfoCollector.ManagementServerName(), string.Empty);
				ServerLogManager.Instance.AddLogEntry(logEntry);
			}
		}
		catch (OutOfMemoryException ex)
		{
			Logger.LogError("(CheckAndFireMsFailoverStartEvent) Error during execution of background task: {ExceptionMessage}", ex.Message);
		}
		catch (TimeoutException ex2)
		{
			Logger.LogError("(CheckAndFireMsFailoverStartEvent) Error during execution of background task: {ExceptionMessage}", ex2.Message);
		}
		catch (ArgumentNullException ex3)
		{
			Logger.LogError("(CheckAndFireMsFailoverStartEvent) Error during execution of background task: {ExceptionMessage}", ex3.Message);
		}
	}

	UserInfo IServerCommandAPI.GetUserInfo(string token)
	{
		try
		{
			UserInfo userInfo = new UserInfo();
			try
			{
				Identity identityFromTokenEx = GetIdentityFromTokenEx(token);
				userInfo.name = identityFromTokenEx.DomainAccountName;
				userInfo.sid = identityFromTokenEx.Sid;
			}
			catch (ServerApiException)
			{
			}
			return userInfo;
		}
		catch (Exception)
		{
			throw;
		}
	}

	Identity IServerCommandAPI.GetUserIdentity(string token)
	{
		try
		{
			Identity result = new Identity();
			try
			{
				result = GetIdentityFromTokenEx(token);
			}
			catch (ServerApiException)
			{
			}
			return result;
		}
		catch (Exception)
		{
			throw;
		}
	}

	void IServerCommandAPI.MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			SetMatrixViewCommand command = new SetMatrixViewCommand(matrixMonitorId, new Guid[1] { deviceId });
			CommandProcessor.ProcessAsync(command, basicCallContextInfo, new NullResponseSink());
		}
		catch (Exception)
		{
			throw;
		}
	}

	void IServerCommandAPI.EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo)
	{
		if (basicCallContextInfo == null)
		{
			throw new ArgumentNullException("basicCallContextInfo");
		}
		try
		{
			VideoOS.Server.DataComponents.ConfigurationObjects.UserDefinedEvent userDefinedEvent = null;
			try
			{
				userDefinedEvent = VideoOS.Server.DataComponents.ConfigurationObjects.UserDefinedEvent.QueryUserDefinedEvent(eventId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (userDefinedEvent == null)
			{
				return;
			}
			TriggerEvent triggerEvent = null;
			if (metadata != null)
			{
				EventMetaDataCollection eventMetaDataCollection = new EventMetaDataCollection();
				long num = 0L;
				foreach (KeyValue keyValue in metadata)
				{
					eventMetaDataCollection.SetMetadataAsString(keyValue.Key, keyValue.Value);
					num += keyValue.Key.Length + keyValue.Value.Length;
				}
				if (num < 50000000)
				{
					triggerEvent = new TriggerEvent(SourceGuid.External, eventId, eventMetaDataCollection);
				}
				else
				{
					logger.LogError("ServerIServerApi.EventTriggerWithMetadata(), total chars in meta data reached upper limit of: {MaxTotalMetadataChars}", 50000000L);
				}
			}
			else
			{
				triggerEvent = new TriggerEvent(SourceGuid.External, eventId);
			}
			if (triggerEvent != null)
			{
				RuleEngine.AddTriggerEvent(triggerEvent);
				RecorderCommManager.TriggerCustomEventAsync(triggerEvent, basicCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
				try
				{
					ServerLogManager.Instance.ServerLogEntryAuditLogger.AuditUserDefinedEventActivated(eventId, userDefinedEvent.Name, basicCallContextInfo.UserDomainAccountName, basicCallContextInfo.LocationIPAddress);
					return;
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
					return;
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	DictionaryEntry[] IServerCommandAPI.GetVmoServicesList()
	{
		System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		ServiceModelSectionGroup sectionGroup = ServiceModelSectionGroup.GetSectionGroup(config);
		List<DictionaryEntry> list = new List<DictionaryEntry>();
		foreach (ServiceElement service in sectionGroup.Services.Services)
		{
			string name = service.Name;
			string serviceAddress = GetServiceAddress(service);
			if (!string.IsNullOrEmpty(serviceAddress))
			{
				list.Add(new DictionaryEntry(name, serviceAddress));
			}
		}
		return list.ToArray();
	}

	string IServerCommandAPI.GetVmoServerName()
	{
		return ServerConfig.Instance.VmoCommunicationConfiguration.ServerName;
	}

	int IServerCommandAPI.GetVmoServerPort()
	{
		return ServerConfig.Instance.VmoCommunicationConfiguration.Port;
	}

	int IServerCommandAPI.GetVmoServerSecurePort()
	{
		return ServerConfig.Instance.VmoCommunicationConfiguration.HttpsPort;
	}

	private static string GetServiceAddress(ServiceElement svc)
	{
		string text = null;
		foreach (ServiceEndpointElement endpoint in svc.Endpoints)
		{
			if (endpoint.Binding.ToUpperInvariant().Equals("wsHttpBinding", StringComparison.OrdinalIgnoreCase) && endpoint.Address.IsAbsoluteUri && !string.IsNullOrEmpty(endpoint.Address.AbsoluteUri))
			{
				text = endpoint.Address.AbsoluteUri;
				break;
			}
		}
		if (text == null)
		{
			foreach (BaseAddressElement baseAddress in svc.Host.BaseAddresses)
			{
				if (Uri.TryCreate(baseAddress.BaseAddress, UriKind.Absolute, out var result) && result.Scheme == Uri.UriSchemeHttp)
				{
					text = result.AbsoluteUri;
					break;
				}
			}
		}
		return text;
	}

	void IServerEnterpriseServerConnector.ValidateToken(string token, string userHostAddress, out string userName, out string password)
	{
		Identity identityFromTokenEx = GetIdentityFromTokenEx(token);
		userName = identityFromTokenEx.DomainAccountName;
		password = "";
	}

	void IServerManagement.SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		try
		{
			command.InvocationInfo = proxyId;
			CommandProcessor.ProcessAsync(command, basicCallContextInfo, ProxyResponseManager);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	ResponseBase[] IServerManagement.GetResponses(Guid proxyId, TimeSpan timeout)
	{
		try
		{
			return ProxyResponseManager.GetResponses(proxyId, timeout);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	void IServerManagement.ReleaseGetResponses(Guid proxyId)
	{
		try
		{
			ProxyResponseManager.ReleaseGetResponses(proxyId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	SecurityManagerDataTransferObject IServerManagement.GetSecurityConfigurationDtoByRole(Guid roleId)
	{
		try
		{
			return SecurityManagerAssembler.CreateDtoByRole(roleId, this);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	SecurityManagerDataTransferObject IServerManagement.GetSecurityConfigurationDtoByTables(string[] tableNames)
	{
		try
		{
			return SecurityManagerAssembler.CreateDtoByTabels(tableNames, this);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void RefreshIdentityCache()
	{
		IdentityCache.ClearMemoryCache();
	}

	public void ServerRegistrationChanged()
	{
		MIPMessageManager.Close();
	}

	public void MipItemPositionChanged(string fqid, string newPosition)
	{
		FQID fQID = new FQID(fqid);
		PositioningInformation positioningInformation = new PositioningInformation(newPosition, 0.0, 0.0, 0.0);
		MIPMessageManager.SentEventToConsumer(new PostFreeFormEventBody
		{
			Source = fQID.ObjectId.ToString(),
			Type = BuiltInEventTypes.EventIDs.MipItemPositionChanged,
			Data = new MipItemPositionChangedEventData
			{
				Position = (positioningInformation.IsPointDefined ? new MipItemPositionChangedEventDataPositionInfo
				{
					Latitude = positioningInformation.Latitude,
					Longitude = positioningInformation.Longitude
				} : null)
			},
			Time = DateTimeOffset.UtcNow
		});
	}

	public void MipItemChanged(string fqid, int changeType)
	{
		FQID fQID = new FQID(fqid);
		if (!Util.IsBuiltInKind(fQID.Kind) || !(fQID.Kind != Kind.TransactSource))
		{
			MIPMessageManager.SentEventToConsumer(new PostFreeFormEventBody
			{
				Data = new SystemConfigurationChangedEventData
				{
					Action = changeType switch
					{
						2 => "deleted", 
						0 => "created", 
						_ => "updated", 
					},
					RelatedItem = new SystemConfigurationChangedEventDataRelatedItem
					{
						RelatedItemRestResource = _restResourceConverter.GetRestResource(fQID).ToString(),
						Name = fQID.ObjectIdString
					}
				},
				Source = HomeSiteId.ToString(),
				Type = BuiltInEventTypes.EventIDs.SystemConfigurationChanged,
				Time = DateTimeOffset.UtcNow
			});
		}
	}

	public void ResourceItemChanged(string resourceType, string id, string objectDisplayName, EventAction changeType)
	{
		MIPMessageManager.SentEventToConsumer(new PostFreeFormEventBody
		{
			Data = new SystemConfigurationChangedEventData
			{
				Action = changeType switch
				{
					EventAction.Deleted => "deleted", 
					EventAction.Added => "created", 
					_ => "updated", 
				},
				RelatedItem = new SystemConfigurationChangedEventDataRelatedItem
				{
					RelatedItemRestResource = (string.IsNullOrEmpty(id) ? resourceType : $"{resourceType}/{id}"),
					Name = objectDisplayName
				}
			},
			Source = HomeSiteId.ToString(),
			Type = BuiltInEventTypes.EventIDs.SystemConfigurationChanged,
			Time = DateTimeOffset.UtcNow
		});
	}

	public void LogServerChanged()
	{
		LogDataAccessor.Initialize();
	}

	AviCodec64[] IServerManagement.GetInstalledCodecs()
	{
		try
		{
			VmoSecurityManager.IsAdministrator(WcfCallContextInfo.Current.UserSid);
			return MailProcessor.GetInstalledCodecs();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	string IServerManagement.SendMail(MailGeneratorData mailGeneratorData)
	{
		try
		{
			Instance.NotificationManager.SendTestEmailNotification(mailGeneratorData);
			return string.Empty;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			return (ex2.InnerException != null) ? string.Format(CultureInfo.CurrentCulture, "{0} ({1})", ex2.Message, ex2.InnerException.Message) : ex2.Message;
		}
	}

	void IServerManagement.FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			Guid guid = Guid.Empty;
			try
			{
				guid = ConfigurationDataManager.FindUserDefinedEventTypeIdByName(eventName);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (guid != Guid.Empty)
			{
				TriggerEvent triggerEvent = new TriggerEvent(SourceGuid.External, guid);
				RuleEngine.AddTriggerEvent(triggerEvent.Clone());
				RecorderCommManager.TriggerCustomEventAsync(triggerEvent, basicCallContextInfo, RuleEngine.IsUsedInRule(triggerEvent));
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	void IServerManagement.Log(LogEntry logEntry)
	{
		try
		{
			ServerLogManager.Instance.AddLogEntry(logEntry);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	void IServerManagement.Log(LogEntry[] logEntries)
	{
		try
		{
			ServerLogManager.Instance.AddLogEntries(logEntries);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	LogQueryResult IServerManagement.QueryLog(LogQuery logQuery)
	{
		try
		{
			return ServerLogManager.QueryLog(logQuery);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	byte[] IServerManagement.QueryLogExport(LogQuery logQuery, int codePage, CultureInfo cultureInfo)
	{
		try
		{
			return ServerLogManager.QueryLogExport(logQuery, codePage, cultureInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	LogEntryField[] IServerManagement.QueryLogFilter(LogFilter logFilter)
	{
		try
		{
			return ServerLogManager.QueryLogFilter(logFilter);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	byte[] IServerManagement.GetLicenseRequestFromConfiguration()
	{
		try
		{
			lock (LicenseManager)
			{
				return LicenseManager.CreateServerLicenseRequest(ServerConfig.Instance.IncludeMacAddressInLicenseRequest);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	void IServerManagement.UpdateLicense(byte[] encryptedLicense, string ownerXml, string sitesInformation)
	{
		UpdateLicenseThroughUpdateConfigurationManagerCommand(encryptedLicense, ownerXml, sitesInformation);
	}

	void IServerManagement.QueueAutomaticActivation()
	{
		try
		{
			LicenseManager.QueueAutomaticActivation();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	LicenseInformation IServerManagement.GetLicenseInformationCache()
	{
		LicensingEventTracer.Logger.TraceMethodStart("GetLicenseInformationCache");
		try
		{
			LicenseInfoXmlWriter licenseInfoXmlWriter = new LicenseInfoXmlWriter(LicenseManager);
			return licenseInfoXmlWriter.CreateLicenseInformationCache(Data);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
		finally
		{
			LicensingEventTracer.Logger.TraceMethodStop("GetLicenseInformationCache");
		}
	}

	string IServerManagement.GetLicenseInformation()
	{
		LicensingEventTracer.Logger.TraceMethodStart("GetLicenseInformation");
		try
		{
			LicenseInfoXmlWriter licenseInfoXmlWriter = new LicenseInfoXmlWriter(LicenseManager);
			ICachedLicenseInfo requiredService = ServiceContainer.Instance.GetRequiredService<ICachedLicenseInfo>();
			return licenseInfoXmlWriter.CreateLicenseInformationXml(requiredService.CachedLicenseInfo, Data);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
		finally
		{
			LicensingEventTracer.Logger.TraceMethodStop("GetLicenseInformation");
		}
	}

	string IServerManagement.GetSystemInformationXml()
	{
		try
		{
			return LicenseManager.SystemInformationXml;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	string IServerManagement.GetNavigationXml()
	{
		string navigationXml = ConfigurationDataManager.GetNavigationXml();
		if (navigationXml.Length == 0)
		{
			throw new NavigationXmlApplicationException("Navigation XML does not exists in system settings.");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(navigationXml);
		RemoveNodeIfUnlicensed(xmlDocument, "AxisOneClick", "AxisOneClick");
		RemoveNodeIfUnlicensed(xmlDocument, "EvidenceLock", "EvidenceLock");
		RemoveNodeIfUnlicensed(xmlDocument, "RemoteConnectedServices", "RemoteConnectServers");
		RemoveNodeIfUnlicensed(xmlDocument, "RecordingServerFailover", "FailoverServers");
		RemoveNodeIfUnlicensed(xmlDocument, "SmartClientProfiles", "ClientProfiles");
		RemoveNodeIfUnlicensed(xmlDocument, "ManagementClientProfiles", "ManagementClientProfiles");
		RemoveNodeIfUnlicensed(xmlDocument, "ConfigurationReports", "ConfigReports");
		RemoveNodeIfUnlicensed(xmlDocument, "SystemMonitor", "SystemMonitor");
		RemoveNodeIfUnlicensed(xmlDocument, "SystemMonitor", "SystemMonitorThreshold");
		if (!LicenseManager.IsFeatureLicensed("SearchCategories") || !IsAnyMetadataCategoryLicensed())
		{
			RemoveNode(xmlDocument, "MetadataUse");
		}
		RemoveNodeIfNoChildren(xmlDocument, "RemoteConnectServers");
		if (!VideoWallConfigured())
		{
			DateTime expiryDate;
			if (!LicenseManager.IsFeatureLicensed("SmartWall"))
			{
				RemoveNode(xmlDocument, "VideoWall");
			}
			else if (LicenseManager.TryGetFeatureExpiryDate("SmartWall", out expiryDate) && DateTime.UtcNow.Date > expiryDate.Date)
			{
				SetNodeUnlicensed(xmlDocument, "VideoWall");
			}
		}
		return xmlDocument.OuterXml;
	}

	private static void RemoveNode(XmlDocument navigationXmlDoc, string nodeId)
	{
		XmlNode xmlNode = navigationXmlDoc.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "//entry[@ID='{0}']", nodeId));
		xmlNode?.ParentNode.RemoveChild(xmlNode);
	}

	private void RemoveNodeIfUnlicensed(XmlDocument navigationXmlDoc, string featureName, string nodeId)
	{
		if (!LicenseManager.IsFeatureLicensed(featureName))
		{
			RemoveNode(navigationXmlDoc, nodeId);
		}
	}

	private static void RemoveNodeIfNoChildren(XmlDocument navigationXmlDoc, string nodeId)
	{
		XmlNode xmlNode = navigationXmlDoc.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "//entry[@ID='{0}']", nodeId));
		if (xmlNode != null && !xmlNode.ChildNodes.Cast<XmlNode>().Any((XmlNode n) => string.Equals(n.Name, "entry", StringComparison.Ordinal)))
		{
			xmlNode.ParentNode.RemoveChild(xmlNode);
		}
	}

	private static void SetNodeUnlicensed(XmlDocument navigationXmlDoc, string nodeId)
	{
		XmlNode xmlNode = navigationXmlDoc.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "//entry[@ID='{0}']", nodeId));
		if (xmlNode != null)
		{
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("data");
			xmlNode2.InnerText = "Unlicensed.html";
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("launch/classname");
			xmlNode3.InnerText = "VideoOS.Administration.AddIn.UserControlHelp";
		}
	}

	private bool IsAnyMetadataCategoryLicensed()
	{
		string text = "1";
		string[] array = new string[3] { "7C4765C6-BD67-45A1-9E0B-84313207379C", "DE799EAA-7564-4882-8E86-B0FA85919559", "4E22B171-B878-4751-8184-DD87B2DD65F1" };
		string[] array2 = array;
		foreach (string featureParameterName in array2)
		{
			try
			{
				string featureParameterValue = LicenseManager.GetFeatureParameterValue("SearchCategories", featureParameterName);
				if (featureParameterValue == text)
				{
					return true;
				}
			}
			catch (ArgumentException)
			{
			}
		}
		return false;
	}

	private static bool VideoWallConfigured()
	{
		ServerConnection serverConnection = null;
		VmoClient vmoClient = null;
		try
		{
			serverConnection = new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName);
			serverConnection.ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port;
			vmoClient = new VmoClient(serverConnection);
			return vmoClient.ManagementServer.VideoWalls.Count > 0;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Instance.Logger.LogWarning("Failed to get video walls. Error: {ExceptionMessage}", ex2.Message);
		}
		finally
		{
			serverConnection?.Dispose();
			vmoClient?.Dispose();
		}
		return false;
	}

	GetFileResult IServerManagement.ReadFile(string fileName, long index, long count)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		string[] array = fileName.Split('\\');
		if (array.Length != 2)
		{
			throw new FileNotFoundException();
		}
		if (string.Compare(array[0], BackupRestoreContract.BackupFolderToken, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new FileNotFoundException();
		}
		string path = Path.Combine(ServerConfig.Instance.BackupShareFolderPath, ServerConfig.Instance.BackupShareFolderName, array[1]);
		int num = 0;
		byte[] array2 = new byte[count];
		long num2 = 100L;
		using (FileStream fileStream = File.OpenRead(path))
		{
			if (index < fileStream.Length)
			{
				num2 = index * 100 / fileStream.Length;
				fileStream.Seek(index, SeekOrigin.Begin);
				num = fileStream.Read(array2, 0, array2.Length);
			}
		}
		if (num >= array2.Length)
		{
			return new GetFileResult(array2, (int)num2);
		}
		byte[] array3 = new byte[num];
		Array.Copy(array2, array3, array3.Length);
		return new GetFileResult(array3, (int)num2);
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetLicenseByLicenseId(Guid licenseId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadLicenseByLicenseId(licenseId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareDefinitionByDigest(string digest)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareDefinitionByDigest(digest);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareByRecorderId(Guid recorderId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareByRecorderId(recorderId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareByHardwareId(Guid hardwareId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareByHardwareId(hardwareId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareByHardwareIds(Guid[] hardwareIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareByHardwareIds(hardwareIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareByHostedHardwareId(int hostedHardwareId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareByHostedHardwareId(hostedHardwareId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHardwareByDeviceGroupId(deviceGroupId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDevicesByHardwareId(Guid hardwareId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDevicesByHardwareId(hardwareId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDevicesByRecorderId(Guid recorderId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDevicesByRecorderId(recorderId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceByDeviceId(Guid deviceId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceByDeviceId(deviceId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDevicesByDeviceIds(Guid[] deviceIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDevicesByDeviceIds(deviceIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceGroupByDeviceGroupId(deviceGroupId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceGroupMembersByDeviceGroupId(deviceGroupId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceGroupMembersByDeviceId(Guid deviceId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceGroupMembersByDeviceId(deviceId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceRecorderPlugin(recorderPluginID, deviceId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceRecorderPlugins(recorderPluginID, deviceIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceRecorderPluginsByDeviceId(Guid deviceId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceRecorderPluginsByDeviceId(deviceId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDevicePrivacyMaskByDeviceIds(deviceIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceTracksByDeviceIds(Guid[] deviceIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceTracksByDeviceIds(deviceIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadDeviceGroupsByParentDeviceGroupId(parentDeviceGroupId);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHostedHardwareByIds(int[] hostedHardwareIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHostedHardwareByIds(hostedHardwareIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetHostingServersByIds(int[] hostingServerIds)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadHostingServersByIds(hostingServerIds);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	ConfigurationManagerDataTransferObject IServerManagement.GetConfigurationDataSetTables(string[] tableNames)
	{
		try
		{
			ConfigurationDataSet ds = Data.LoadConfigurationDataSetTables(tableNames);
			return new ConfigurationManagerDataTransferObject(ds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	Guid[] IServerManagement.QueryRolesForDefaultTimeProfile(Guid timeProfileId)
	{
		try
		{
			return TimeProfileRole.QueryRolesForDefaultTimeProfile(timeProfileId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	Guid[] IServerManagement.QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId)
	{
		try
		{
			return TimeProfileRole.QueryRolesForPermissionsWithTimeProfile(timeProfileId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public int[] QueryHostedHardwareIdsNotAdded()
	{
		try
		{
			return HostingInfo.QueryHostedHardwareIdsNotAdded();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}
}

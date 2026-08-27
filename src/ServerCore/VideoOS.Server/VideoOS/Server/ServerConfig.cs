using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Server.ConfigExtensions;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server;

public sealed class ServerConfig : IServerConfig, IDisposable
{
	private const string _primaryRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server\\Common";

	private static ServerConfig _instance;

	private LogConfig _logConfiguration;

	private RecorderCommunicationConfig _recorderCommunicationConfiguration;

	private VmoCommunicationConfig _vmoCommunicationConfiguration;

	private RulesConfig _rulesConfiguration;

	private WebApiConfig _webApiConfig;

	private readonly LogDataQueueManagerConfig _logDataQueueManagerConfig;

	private readonly EvidenceLockUpdateConfig _evidenceLockUpdateConfig;

	private readonly EvidenceLockOldRecorderConfig _evidenceLockOldRecorderConfig;

	private DateTime _lastReloadTaskStarted;

	private FileSystemWatcher _fileSystemWatcher;

	private bool disposedValue;

	public static ServerConfig Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ServerConfig();
			}
			return _instance;
		}
	}

	public int MaxCommandsToBulk { get; private set; }

	public TimeSpan VmoClientPoolKeepAlive { get; private set; }

	public int NotificationTokenTimeout { get; private set; }

	public int ServerPort { get; private set; }

	public int ProxyPort { get; private set; }

	public int TrayControllerPort { get; private set; }

	public TimeSpan GarbageCollectInterval { get; private set; }

	public TimeSpan MemoryLogInterval { get; private set; }

	public bool IncludeMacAddressInLicenseRequest { get; private set; }

	public int OnlineActivationRequestTimeout { get; private set; }

	public int AutomaticActivationDelaySec { get; private set; }

	public int AutomaticActivationRetryDelaySec { get; private set; }

	public int MoveHardwareSimultaneousTasks { get; private set; }

	public TimeSpan MoveHardwarePrepareMoveHardwareTimeout { get; private set; }

	public Uri LogServiceUrlDevOnly { get; private set; }

	public string BackupShareFolderName { get; private set; }

	public string BackupShareFolderPath { get; private set; }

	public ILogConfig LogConfiguration => _logConfiguration;

	public IRecorderCommunicationConfig RecorderCommunicationConfiguration => _recorderCommunicationConfiguration;

	public IVmoCommunicationConfig VmoCommunicationConfiguration => _vmoCommunicationConfiguration;

	public IWebApiConfig WebApiConfig => _webApiConfig;

	public IRulesConfig RulesConfiguration => _rulesConfiguration;

	public ILogDataQueueManagerConfig LogDataQueueManagerConfig => _logDataQueueManagerConfig;

	public IEvidenceLockUpdateConfig EvidenceLockUpdateConfig => _evidenceLockUpdateConfig;

	public IEvidenceLockOldRecorderConfig EvidenceLockOldRecorderConfig => _evidenceLockOldRecorderConfig;

	public Guid ClientRegistrationId { get; private set; }

	public SnmpConfig Snmp { get; private set; }

	public static string FileStorageFolderName => "FileStorage";

	public static string FileStorageFolderPath
	{
		get
		{
			string text = Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, FileStorageFolderName);
			Directory.CreateDirectory(text);
			return text;
		}
	}

	public uint FileStorageQuotaMBLimit { get; private set; }

	public TimeSpan FileStoragePersistedEntryDuration { get; private set; }

	public TimeSpan FileStorageTemporaryEntryDuration { get; private set; }

	public bool IsFailoverClusterMember { get; private set; }

	public Uri ClusterLocalWebserverUri { get; private set; }

	public string ClusterModuleName { get; private set; }

	public string VirtualAddress { get; set; }

	public string EncodedClusterPassword { get; private set; }

	public string OfflineFolder
	{
		get
		{
			string text = Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "Offline");
			Directory.CreateDirectory(text);
			return text;
		}
	}

	public string ConfigFilePath
	{
		get
		{
			string configFilePath = OemResources.Instance.Server.ConfigFilePath;
			string fileName = Path.GetFileName(configFilePath);
			string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
			if (File.Exists(text))
			{
				return text;
			}
			return configFilePath;
		}
	}

	public int SmartWallProcessDelay { get; public set; }

	public ServerConfig()
	{
		ClientRegistrationId = Guid.Empty;
		MaxCommandsToBulk = 1000;
		VmoClientPoolKeepAlive = TimeSpan.FromSeconds(300.0);
		NotificationTokenTimeout = 120;
		GarbageCollectInterval = TimeSpan.Zero;
		MemoryLogInterval = TimeSpan.Zero;
		IncludeMacAddressInLicenseRequest = true;
		OnlineActivationRequestTimeout = 360;
		AutomaticActivationDelaySec = 300;
		AutomaticActivationRetryDelaySec = 86400;
		_logConfiguration = new LogConfig();
		_recorderCommunicationConfiguration = new RecorderCommunicationConfig();
		_vmoCommunicationConfiguration = new VmoCommunicationConfig();
		_rulesConfiguration = new RulesConfig();
		_webApiConfig = new WebApiConfig();
		_logDataQueueManagerConfig = new LogDataQueueManagerConfig();
		_evidenceLockUpdateConfig = new EvidenceLockUpdateConfig();
		_evidenceLockOldRecorderConfig = new EvidenceLockOldRecorderConfig();
		Snmp = SnmpConfig.ServerSnmpConfig();
	}

	public void Initialize(int vmoPort)
	{
		_vmoCommunicationConfiguration.OverwritePort(vmoPort);
		Initialize();
	}

	public void Initialize()
	{
		try
		{
			InternalInitialize(ConfigFilePath);
			EnsureWatcherConfigured();
		}
		catch (ServerConfigInvalidException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new ServerConfigInvalidException("An unexpected exception occurred when reading the server configuration file.", inner);
		}
	}

	private void InternalInitialize(string configFilePath)
	{
		XmlDocument xmlDocument = null;
		XmlTextReader xmlTextReader = null;
		try
		{
			xmlDocument = new XmlDocument();
			xmlTextReader = new XmlTextReader(configFilePath);
			xmlDocument.Load(xmlTextReader);
			XmlNode xmlNode = xmlDocument["server"];
			string text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Server\\Common", "LogServiceUrl", null);
			if (!string.IsNullOrEmpty(text))
			{
				if (Uri.TryCreate(text, UriKind.Absolute, out var result))
				{
					LogServiceUrlDevOnly = result;
				}
				else
				{
					LogServiceUrlDevOnly = null;
				}
			}
			InitializeLogSettings(xmlNode["logsettings"]);
			InitializeMaxCommandsAsBulk(xmlNode["MaxCommandsToBulk"]);
			InitializeVmoClientPoolKeepAlive(xmlNode["VmoClientPoolKeepAlive"]);
			InitializeTokenTimeout(xmlNode["tokentimeout"]);
			InitializeServerPort(xmlNode["serverport"]);
			InitializeServerProxyCommunication(xmlNode["serverproxycommunication"]);
			InitializeTrayControllerCommunication(xmlNode["traycontrollercommunication"]);
			_recorderCommunicationConfiguration.Initialize(xmlNode["RecorderCommunication"]);
			_vmoCommunicationConfiguration.Initialize(xmlNode["VMOCommunication"]);
			_rulesConfiguration.Initialize(xmlNode["Rules"]);
			_webApiConfig.Initialize(xmlNode["WebApiConfig"]);
			_logDataQueueManagerConfig.Initialize(xmlNode["LogDataQueueManager"]);
			_evidenceLockUpdateConfig.Initialize(xmlNode["EvidenceLockUpdate"]);
			_evidenceLockOldRecorderConfig.Initialize(xmlNode["EvidenceLockOldRecorder"]);
			_logConfiguration.Initialize(xmlNode["IdpLog"]);
			InitializeOnlineActivationSettings(xmlNode["OnlineActivation"]);
			InternalInitializeMoveHardware(xmlNode["MoveHardware"]);
			InitializeGarbageCollectSettings(xmlNode["garbagecollectinterval"]);
			InitializeMemoryLogSettings(xmlNode["memoryloginterval"]);
			InitializeBackupSettings(xmlNode["backup"]);
			InitializeClientRegistrationId(xmlNode["ClientRegistrationId"]);
			InitializeSmartWallConfig(xmlNode["SmartWallProcessing"]);
			InitializeFileStorageSettings(xmlNode["FileStorage"]);
			InitializeFailoverSettings(xmlNode["Failover"]);
		}
		finally
		{
			if (xmlTextReader != null)
			{
				xmlTextReader.Close();
				xmlTextReader = null;
			}
		}
	}

	private void InitializeSmartWallConfig(XmlElement smartWallElement)
	{
		SmartWallProcessDelay = 1000;
		if (smartWallElement == null)
		{
			return;
		}
		XmlElement xmlElement = smartWallElement["ProcessingDelay"];
		if (xmlElement != null)
		{
			int result = -1;
			if (int.TryParse(xmlElement.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
			{
				SmartWallProcessDelay = result;
			}
		}
	}

	private void InitializeLogSettings(XmlElement logSettings)
	{
		if (logSettings == null)
		{
			throw new ServerConfigInvalidException("Log settings configuration missing.");
		}
		if (!logSettings.HasAttribute("cacheFile"))
		{
			throw new ServerConfigInvalidException("Log settings cache file setting missing.");
		}
		LogConfiguration.CacheFile = logSettings.Attributes["cacheFile"].InnerText;
		if (!Path.IsPathRooted(LogConfiguration.CacheFile))
		{
			LogConfiguration.CacheFile = Path.GetFullPath(Path.Combine(OfflineFolder, LogConfiguration.CacheFile));
		}
		InitializeLogSettingLoggers(logSettings);
	}

	private void InitializeLogSettingLoggers(XmlElement logSettings)
	{
		if (logSettings == null)
		{
			return;
		}
		XmlNodeList xmlNodeList = logSettings.SelectNodes("log");
		if (xmlNodeList == null)
		{
			return;
		}
		foreach (XmlNode item in logSettings.SelectNodes("log"))
		{
			string text = null;
			XmlAttribute xmlAttribute = item.Attributes["name"];
			if (xmlAttribute != null)
			{
				text = xmlAttribute.InnerText;
			}
			if (!string.Equals(text, "Alert", StringComparison.Ordinal))
			{
				string tableName = null;
				XmlNode xmlNode2 = item["tablename"];
				if (xmlNode2 != null)
				{
					tableName = xmlNode2.InnerText;
				}
				string dateTimeColumn = null;
				XmlNode xmlNode3 = item["datetimecolumn"];
				if (xmlNode3 != null)
				{
					dateTimeColumn = xmlNode3.InnerText;
				}
				int millisecondsBetweenDatabaseWrites = 0;
				XmlNode xmlNode4 = item["millisecondsbetweendatabasewrites"];
				if (xmlNode4 != null)
				{
					millisecondsBetweenDatabaseWrites = Convert.ToInt32(xmlNode4.InnerText, CultureInfo.InvariantCulture);
				}
				LogType logType = text switch
				{
					"System" => LogType.SystemLog, 
					"Audit" => LogType.AuditLog, 
					"Rule" => LogType.RuleLog, 
					_ => throw new ServerConfigInvalidException(FormattableString.Invariant(FormattableStringFactory.Create("Invalid log type ({0})", text ?? "<null>"))), 
				};
				LogDefinition value = new LogDefinition(logType, text, tableName, dateTimeColumn, millisecondsBetweenDatabaseWrites, enabled: true, extendedUserLogging: true, 60, 5);
				LogConfiguration.Logs[logType] = value;
			}
		}
	}

	private void InitializeMaxCommandsAsBulk(XmlElement maxCommandsToBulk)
	{
		if (maxCommandsToBulk != null)
		{
			MaxCommandsToBulk = Convert.ToInt32(maxCommandsToBulk.InnerText, CultureInfo.InvariantCulture);
			if (MaxCommandsToBulk <= 0)
			{
				throw new ServerConfigInvalidException("maximum commands that can be bulked cannot be zero or less");
			}
		}
	}

	private void InitializeVmoClientPoolKeepAlive(XmlElement vmoClientPoolKeepAlive)
	{
		if (vmoClientPoolKeepAlive != null && int.TryParse(vmoClientPoolKeepAlive.InnerText, out var result))
		{
			VmoClientPoolKeepAlive = TimeSpan.FromSeconds(result);
		}
	}

	private void InitializeTokenTimeout(XmlElement tokenTimeout)
	{
		if (tokenTimeout != null)
		{
			XmlNode xmlNode = tokenTimeout["notification"];
			if (xmlNode != null)
			{
				NotificationTokenTimeout = Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
			}
		}
	}

	private void InitializeServerPort(XmlElement serverPort)
	{
		if (serverPort == null)
		{
			throw new ServerConfigInvalidException("Server port missing.");
		}
		XmlNode xmlNode = serverPort["port"];
		if (xmlNode == null)
		{
			throw new ServerConfigInvalidException("Server port setting missing.");
		}
		ServerPort = Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
	}

	private void InitializeServerProxyCommunication(XmlElement serverProxyCommunication)
	{
		if (serverProxyCommunication == null)
		{
			throw new ServerConfigInvalidException("Server proxy communication missing.");
		}
		XmlNode xmlNode = serverProxyCommunication["port"];
		if (xmlNode == null)
		{
			throw new ServerConfigInvalidException("Server proxy communication port setting missing.");
		}
		ProxyPort = Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
	}

	private void InitializeTrayControllerCommunication(XmlElement serverProxyCommunication)
	{
		if (serverProxyCommunication == null)
		{
			throw new ServerConfigInvalidException("Tray controller communication missing.");
		}
		XmlNode xmlNode = serverProxyCommunication["port"];
		if (xmlNode == null)
		{
			throw new ServerConfigInvalidException("Tray controller communication port setting missing.");
		}
		TrayControllerPort = Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
	}

	private void InitializeOnlineActivationSettings(XmlElement onlineActivationNode)
	{
		IncludeMacAddressInLicenseRequest = true;
		OnlineActivationRequestTimeout = 360;
		AutomaticActivationDelaySec = 300;
		AutomaticActivationRetryDelaySec = 86400;
		if (onlineActivationNode != null && onlineActivationNode.HasAttribute("IncludeMACAddressInRequest"))
		{
			if (bool.TryParse(onlineActivationNode.Attributes["IncludeMACAddressInRequest"].Value, out var result))
			{
				IncludeMacAddressInLicenseRequest = result;
			}
			if (int.TryParse(onlineActivationNode.Attributes["RequestTimeout"].Value, out var result2))
			{
				OnlineActivationRequestTimeout = Math.Min(Math.Max(result2, 30), 600);
			}
			if (int.TryParse(onlineActivationNode.Attributes["ActivationDelaySec"].Value, out var result3))
			{
				AutomaticActivationDelaySec = Math.Max(result3, 1);
			}
			if (int.TryParse(onlineActivationNode.Attributes["ActivationRetryDelaySec"].Value, out var result4))
			{
				AutomaticActivationRetryDelaySec = Math.Max(result4, 1);
			}
		}
	}

	private void InternalInitializeMoveHardware(XmlElement moveHardwareNode)
	{
		MoveHardwareSimultaneousTasks = 8;
		MoveHardwarePrepareMoveHardwareTimeout = TimeSpan.FromSeconds(60.0);
		if (moveHardwareNode == null)
		{
			throw new ServerConfigInvalidException("Move hardware configuration missing.");
		}
		if (moveHardwareNode.ReadAttributeAsInt32("SimultaneousTasks", out var value))
		{
			MoveHardwareSimultaneousTasks = Math.Min(Math.Max(value, 1), 20);
		}
		if (moveHardwareNode.ReadAttributeAsInt32("PrepareMoveHardwareTimeoutSeconds", out var value2))
		{
			value2 = Math.Min(Math.Max(value2, 1), 600);
			MoveHardwarePrepareMoveHardwareTimeout = TimeSpan.FromSeconds(value2);
		}
	}

	private void InitializeGarbageCollectSettings(XmlElement garbageCollectionInterval)
	{
		if (garbageCollectionInterval != null)
		{
			GarbageCollectInterval = TimeSpan.FromMilliseconds(Convert.ToInt32(garbageCollectionInterval.InnerText, CultureInfo.CurrentCulture));
		}
	}

	private void InitializeMemoryLogSettings(XmlElement memoryLogInterval)
	{
		if (memoryLogInterval != null)
		{
			MemoryLogInterval = TimeSpan.FromMilliseconds(Convert.ToInt32(memoryLogInterval.InnerText, CultureInfo.CurrentCulture));
		}
	}

	private void InitializeBackupSettings(XmlElement backupNode)
	{
		if (backupNode != null)
		{
			XmlElement xmlElement = backupNode["sharename"];
			if (xmlElement != null && !string.IsNullOrEmpty(xmlElement.InnerText))
			{
				BackupShareFolderName = xmlElement.InnerText;
			}
			else
			{
				BackupShareFolderName = BackupResources.DefaultBackupFolderName;
			}
			XmlElement xmlElement2 = backupNode["folderpath"];
			if (xmlElement2 != null && !string.IsNullOrEmpty(xmlElement2.InnerText))
			{
				BackupShareFolderPath = xmlElement2.InnerText;
			}
			else
			{
				BackupShareFolderPath = BackupResources.DefaultBackupFolderPath;
			}
		}
		else
		{
			BackupShareFolderName = BackupResources.DefaultBackupFolderName;
			BackupShareFolderPath = BackupResources.DefaultBackupFolderPath;
		}
	}

	private void InitializeClientRegistrationId(XmlElement clientRegistrationId)
	{
		if (clientRegistrationId != null && Guid.TryParse(clientRegistrationId.InnerText, out var result))
		{
			ClientRegistrationId = result;
			return;
		}
		throw new ServerConfigInvalidException("Server configuration is invalid. Invalid client id");
	}

	private void InitializeFileStorageSettings(XmlElement fileStorageNode)
	{
		XmlElement xmlElement = fileStorageNode?["storageQuotaMBLimit"];
		if (xmlElement != null && !string.IsNullOrEmpty(xmlElement.InnerText) && uint.TryParse(xmlElement.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			FileStorageQuotaMBLimit = result;
		}
		else
		{
			FileStorageQuotaMBLimit = 1024u;
		}
		XmlElement xmlElement2 = fileStorageNode?["persistedEntryDurationInSeconds"];
		if (xmlElement2 != null && !string.IsNullOrEmpty(xmlElement2.InnerText) && uint.TryParse(xmlElement2.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
		{
			FileStoragePersistedEntryDuration = TimeSpan.FromSeconds(result2);
		}
		else
		{
			FileStoragePersistedEntryDuration = TimeSpan.FromMinutes(20.0);
		}
		XmlElement xmlElement3 = fileStorageNode?["temporaryEntryDurationInSeconds"];
		if (xmlElement3 != null && !string.IsNullOrEmpty(xmlElement3.InnerText) && uint.TryParse(xmlElement3.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result3))
		{
			FileStorageTemporaryEntryDuration = TimeSpan.FromSeconds(result3);
		}
		else
		{
			FileStorageTemporaryEntryDuration = TimeSpan.FromMinutes(5.0);
		}
	}

	private void InitializeFailoverSettings(XmlElement failoverNode)
	{
		if (failoverNode == null)
		{
			throw new ServerConfigInvalidException("Node 'failover' missing.");
		}
		ReadClusterMemberValue(failoverNode);
		ReadClusterLocalWebServerUriValue(failoverNode);
		ReadEncodedClusterPasswordStringValue(failoverNode);
		ReadClusterModuleName(failoverNode);
		ReadVirtualAddress(failoverNode);
	}

	private void ReadVirtualAddress(XmlElement failoverNode)
	{
		XmlElement xmlElement = failoverNode["VirtualAddress"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Node 'VirtualAddress' missing.");
		}
		if (!string.IsNullOrWhiteSpace(xmlElement.InnerText))
		{
			VirtualAddress = xmlElement.InnerText;
		}
	}

	private void ReadClusterModuleName(XmlElement failoverNode)
	{
		XmlElement xmlElement = failoverNode["ClusterModuleName"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Node 'ClusterModuleName' missing.");
		}
		if (!string.IsNullOrWhiteSpace(xmlElement.InnerText))
		{
			ClusterModuleName = xmlElement.InnerText;
		}
	}

	private void ReadClusterMemberValue(XmlElement failoverNode)
	{
		XmlElement xmlElement = failoverNode["ClusterMember"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Node 'ClusterMember' missing.");
		}
		if (bool.TryParse(xmlElement.InnerText, out var result))
		{
			IsFailoverClusterMember = result;
			return;
		}
		throw new ServerConfigInvalidException("Node 'ClusterMember' must contain a boolean value.");
	}

	private void ReadClusterLocalWebServerUriValue(XmlElement failoverNode)
	{
		XmlElement xmlElement = failoverNode["ClusterLocalWebserverUri"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Node 'ClusterLocalWebserverUri' missing.");
		}
		if (!string.IsNullOrWhiteSpace(xmlElement.InnerText))
		{
			if (!Uri.TryCreate(xmlElement.InnerText, UriKind.Absolute, out var result))
			{
				throw new ServerConfigInvalidException("Node 'ClusterLocalWebserverUri' must contain a URI value.");
			}
			ClusterLocalWebserverUri = result;
		}
	}

	private void ReadEncodedClusterPasswordStringValue(XmlElement failoverNode)
	{
		XmlElement xmlElement = failoverNode["FailoverCypher"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Node 'FailoverCypher' missing.");
		}
		if (!string.IsNullOrWhiteSpace(xmlElement.InnerText))
		{
			EncodedClusterPassword = xmlElement.InnerText;
		}
	}

	private void EnsureWatcherConfigured()
	{
		if (_fileSystemWatcher != null)
		{
			_fileSystemWatcher.Dispose();
			_fileSystemWatcher = null;
		}
		_lastReloadTaskStarted = DateTime.MinValue;
		string directoryName = Path.GetDirectoryName(ConfigFilePath);
		string fileName = Path.GetFileName(ConfigFilePath);
		_fileSystemWatcher = new FileSystemWatcher(directoryName, fileName);
		_fileSystemWatcher.IncludeSubdirectories = false;
		_fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
		_fileSystemWatcher.Changed += fileSystemWatcher_Changed;
		_fileSystemWatcher.EnableRaisingEvents = true;
	}

	private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
	{
		if (DateTime.UtcNow < _lastReloadTaskStarted.AddMilliseconds(700.0))
		{
			return;
		}
		_lastReloadTaskStarted = DateTime.UtcNow;
		ThreadPool.QueueUserWorkItem(delegate
		{
			Thread.Sleep(1000);
			try
			{
				Initialize();
			}
			catch (ServerConfigInvalidException ex)
			{
				Server.Instance.Logger.LogError("Failed to load settings on config file change. Exception: {Exception}", ex);
			}
		});
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && _fileSystemWatcher != null)
			{
				_fileSystemWatcher.Dispose();
				_fileSystemWatcher = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

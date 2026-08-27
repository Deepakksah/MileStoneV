using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.CertificateUtilities;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public sealed class ServerConfiguratorDataModel : ICertificateCmdModel, ICertificateQueryModel, IServerCertificateQueryModel, INonvolatileServerCertificateQueryModel, IClientRegistrationQueryModel, INonvolatileClientRegistrationQueryModel, IApplicationSettingsQueryModel, IDisposable
{
	private readonly Dictionary<Guid, ServerEndpointConfiguration> _serverEndpointsInstalled = new Dictionary<Guid, ServerEndpointConfiguration>();

	private Dictionary<Guid, ServerEndpointConfiguration> _serverEndpointsToBeInstalled = new Dictionary<Guid, ServerEndpointConfiguration>();

	private readonly Dictionary<Guid, ClientEndpointConfiguration> _clientEndpointsInstalled = new Dictionary<Guid, ClientEndpointConfiguration>();

	private Dictionary<Guid, ClientEndpointConfiguration> _clientEndpointsToBeInstalled = new Dictionary<Guid, ClientEndpointConfiguration>();

	private readonly Dictionary<Guid, ModifiableCertificateGroup> _certificateGroups = new Dictionary<Guid, ModifiableCertificateGroup>();

	private readonly Dictionary<Guid, SystemConfigurationElement> _configurationElementsInstalled = new Dictionary<Guid, SystemConfigurationElement>();

	private Dictionary<Guid, SystemConfigurationElement> _configurationElementsToBeInstalled = new Dictionary<Guid, SystemConfigurationElement>();

	private readonly VideoOS.ServerConfigurator.Interfaces.ILogger _logger;

	private VideoOS.ServerConfigurator.CertificateRegistration.ILogger _registrationLogger;

	private string _oldHostName;

	private readonly ICertificateProvider _certificateProvider;

	private readonly IDnsService _dnsService;

	private readonly Dictionary<bool, Uri> _managementServer = new Dictionary<bool, Uri>
	{
		{ false, null },
		{ true, null }
	};

	private readonly Dictionary<bool, Uri> _oldManagementServer = new Dictionary<bool, Uri>
	{
		{ false, null },
		{ true, null }
	};

	private readonly Dictionary<bool, Uri> _authorizationServer = new Dictionary<bool, Uri>
	{
		{ false, null },
		{ true, null }
	};

	private readonly IManagementServerHelper _managementServerHelper;

	private bool? _preferredSecureState;

	private const string IdpPath = "IDP";

	public IEnumerable<ServerEndpointConfiguration> ServerEndpointsInstalled => _serverEndpointsInstalled.Values;

	public IEnumerable<ServerEndpointConfiguration> ServerEndpointsToBeInstalled => _serverEndpointsToBeInstalled.Values;

	public IEnumerable<ClientEndpointConfiguration> ClientEndpointsInstalled => _clientEndpointsInstalled.Values;

	public IEnumerable<ClientEndpointConfiguration> ClientEndpointsToBeInstalled => _clientEndpointsToBeInstalled.Values;

	public IEnumerable<SystemConfigurationElement> ConfigurationElementsInstalled => _configurationElementsInstalled.Values;

	public IEnumerable<SystemConfigurationElement> ConfigurationElementsToBeInstalled => _configurationElementsToBeInstalled.Values;

	public IEnumerable<CertificateGroup> CertificateGroups => _certificateGroups.Values;

	public bool OverrideLocalManagementServer { get; private set; }

	private string LocalHost => _dnsService.GetHostEntry("localhost").HostName;

	public string HostName => LocalHost;

	public CultureInfo ApplicationUICulture { get; private set; }

	public event EventHandler<EventArgs> CertificateGroupsModified;

	public event EventHandler<EventArgs> ServerEndpointsModified;

	public event EventHandler<EventArgs> ClientEndpointsModified;

	public event EventHandler<EventArgs> ConfigurationElementsModified;

	public event EventHandler<EventArgs> ClientRegistrationServerModified;

	public ServerConfiguratorDataModel(VideoOS.ServerConfigurator.Interfaces.ILogger logger, ICertificateProvider certificateProvider, IDnsService dnsService)
		: this(logger, certificateProvider, dnsService, new ManagementServerHelper(logger, new HttpClientFactory()))
	{
	}

	public ServerConfiguratorDataModel(VideoOS.ServerConfigurator.Interfaces.ILogger logger, ICertificateProvider certificateProvider, IDnsService dnsService, IManagementServerHelper managementServerHelper)
	{
		_logger = logger;
		_registrationLogger = new LoggerWrapper(logger);
		_certificateProvider = certificateProvider;
		_dnsService = dnsService;
		_managementServerHelper = managementServerHelper;
	}

	public void AddServerEndpointInstalled(ServerEndpointConfiguration serverEndpoint)
	{
		if (serverEndpoint == null)
		{
			throw ArgumentNullExceptionAndLog("serverEndpoint", "AddServerEndpointInstalled");
		}
		LogInfo(FormattableString.Invariant($"id = {serverEndpoint.Id} ({serverEndpoint.DisplayName}"), "AddServerEndpointInstalled");
		if (!EnsureValidGroupIdAndCertificateRegistrar(serverEndpoint))
		{
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant($"The Server With ID = '{serverEndpoint.Id}' refers a Certificate Group that does not exist (Group with Id '{serverEndpoint.CertificateGroupId}')"), "AddServerEndpointInstalled");
		}
		if (_serverEndpointsInstalled.ContainsKey(serverEndpoint.Id))
		{
			LogWarning(LogString(FormattableString.Invariant($"Server Endpoint with Id {serverEndpoint.Id} already exists"), "AddServerEndpointInstalled"), "AddServerEndpointInstalled");
		}
		_serverEndpointsInstalled[serverEndpoint.Id] = serverEndpoint;
		_logger.LogInfo(FormattableString.Invariant($"Added server endpoint {serverEndpoint.Id}"));
		this.ServerEndpointsModified?.Invoke(this, new EventArgs());
	}

	private bool EnsureValidGroupIdAndCertificateRegistrar(ServerEndpointConfiguration serverEndpoint)
	{
		if (serverEndpoint.CertificateGroupId == Guid.Empty)
		{
			return serverEndpoint.CertificateRegistrar == null;
		}
		return _certificateGroups.ContainsKey(serverEndpoint.CertificateGroupId);
	}

	public void AddClientEndpointInstalled(ClientEndpointConfiguration clientEndpoint)
	{
		if (clientEndpoint == null)
		{
			throw ArgumentNullExceptionAndLog("clientEndpoint", "AddClientEndpointInstalled");
		}
		LogInfo(FormattableString.Invariant($"id = {clientEndpoint.Id} ({clientEndpoint.DisplayName}"), "AddClientEndpointInstalled");
		if (!EnsureValidGroupIdAndClientRegistrar(clientEndpoint))
		{
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant($"The Client Endpoint With ID = '{clientEndpoint.Id}' refers a Certificate Group that does not exist (Group with Id '{clientEndpoint.ServerGroupDependency}')"), "AddClientEndpointInstalled");
		}
		if (_clientEndpointsInstalled.ContainsKey(clientEndpoint.Id))
		{
			LogWarning(LogString(FormattableString.Invariant($"Client Endpoint with Id {clientEndpoint.Id} already exists"), "AddClientEndpointInstalled"), "AddClientEndpointInstalled");
		}
		_clientEndpointsInstalled[clientEndpoint.Id] = clientEndpoint;
		LogInfo(FormattableString.Invariant($"Added client endpoint {clientEndpoint.Id}"), "AddClientEndpointInstalled");
		this.ClientEndpointsModified?.Invoke(this, new EventArgs());
	}

	private bool EnsureValidGroupIdAndClientRegistrar(ClientEndpointConfiguration clientEndpoint)
	{
		if (!(clientEndpoint.ServerGroupDependency == Guid.Empty))
		{
			return _certificateGroups.ContainsKey(clientEndpoint.ServerGroupDependency);
		}
		return true;
	}

	public void AddConfigurationElementInstalled(SystemConfigurationElement configurationElement)
	{
		if (configurationElement == null)
		{
			throw ArgumentNullExceptionAndLog("configurationElement", "AddConfigurationElementInstalled");
		}
		LogInfo(FormattableString.Invariant($"id = {configurationElement.Id} ({configurationElement.DisplayName}"), "AddConfigurationElementInstalled");
		if (_configurationElementsInstalled.ContainsKey(configurationElement.Id))
		{
			LogWarning(LogString(FormattableString.Invariant($"ConfigurationElement Id {configurationElement.Id} already exists"), "AddConfigurationElementInstalled"), "AddConfigurationElementInstalled");
		}
		_configurationElementsInstalled[configurationElement.Id] = configurationElement;
		LogInfo(FormattableString.Invariant($"Added configuration element {configurationElement.Id}"), "AddConfigurationElementInstalled");
		this.ConfigurationElementsModified?.Invoke(this, new EventArgs());
	}

	public void SetServerEndpointsToBeInstalled(IEnumerable<ServerEndpointConfiguration> serverEndpoints)
	{
		IEnumerable<ServerEndpointConfiguration> source = serverEndpoints.Where((ServerEndpointConfiguration x) => !EnsureValidGroupIdAndCertificateRegistrar(x));
		if (source.Any())
		{
			ServerEndpointConfiguration serverEndpointConfiguration = source.First();
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant($"The Server With ID = '{serverEndpointConfiguration.Id}' refers a Certificate Group that does not exist (Group with Id '{serverEndpointConfiguration.CertificateGroupId}')"), "SetServerEndpointsToBeInstalled");
		}
		_serverEndpointsToBeInstalled = serverEndpoints.ToDictionary((ServerEndpointConfiguration x) => x.Id);
		UpdateServerGroupLocks(serverEndpoints);
		this.ServerEndpointsModified?.Invoke(this, new EventArgs());
	}

	public void SetClientEndpointsToBeInstalled(IEnumerable<ClientEndpointConfiguration> clientEndpoints)
	{
		IEnumerable<ClientEndpointConfiguration> source = clientEndpoints.Where((ClientEndpointConfiguration x) => !EnsureValidGroupIdAndClientRegistrar(x));
		if (source.Any())
		{
			ClientEndpointConfiguration clientEndpointConfiguration = source.First();
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant($"The Client Endpoint With ID = '{clientEndpointConfiguration.Id}' refers a Certificate Group that does not exist (Group with Id '{clientEndpointConfiguration.ServerGroupDependency}')"), "SetClientEndpointsToBeInstalled");
		}
		_clientEndpointsToBeInstalled = clientEndpoints.ToDictionary((ClientEndpointConfiguration x) => x.Id);
		this.ClientEndpointsModified?.Invoke(this, new EventArgs());
	}

	public void SetConfigurationElementsToBeInstalled(IEnumerable<SystemConfigurationElement> configurationElements)
	{
		_configurationElementsToBeInstalled = configurationElements.ToDictionary((SystemConfigurationElement x) => x.Id);
		this.ConfigurationElementsModified?.Invoke(this, new EventArgs());
	}

	public void RemoveServerEndpointInstalled(Guid id)
	{
		LogInfo(FormattableString.Invariant($"id = {id}"), "RemoveServerEndpointInstalled");
		if (_serverEndpointsInstalled.ContainsKey(id))
		{
			ServerEndpointConfiguration serverEndpoint = _serverEndpointsInstalled[id];
			_serverEndpointsInstalled.Remove(id);
			try
			{
				CertificateRegistration.Cleanup(serverEndpoint, _registrationLogger, _logger);
				ServiceRegistration.Cleanup(serverEndpoint, _registrationLogger, _logger);
				SetSqlServerHandler.Cleanup(serverEndpoint, _registrationLogger, _logger);
			}
			catch (Exception ex)
			{
				_logger?.LogError(FormattableString.Invariant($"Error occurred during ServerCleanup: {ex}"));
			}
			this.ServerEndpointsModified?.Invoke(this, new EventArgs());
		}
		else
		{
			LogInfo(FormattableString.Invariant($"The Server Endpoint With ID = {id} does not exist. This message should be expected only when upgrading from a non-server configurator installation"), "RemoveServerEndpointInstalled");
		}
	}

	public void RemoveClientEndpointInstalled(Guid id)
	{
		LogInfo(FormattableString.Invariant($"id = {id}"), "RemoveClientEndpointInstalled");
		if (_clientEndpointsInstalled.ContainsKey(id))
		{
			ClientEndpointConfiguration clientEndpoint = _clientEndpointsInstalled[id];
			_clientEndpointsInstalled.Remove(id);
			try
			{
				ClientRegistration.Cleanup(clientEndpoint, _registrationLogger, _logger);
				SetServerAddressHandler.Cleanup(clientEndpoint, _registrationLogger, _logger);
			}
			catch (Exception ex)
			{
				_logger?.LogError(FormattableString.Invariant($"Error occurred during ClientCLeanup: {ex}"));
			}
			LogInfo(LogString(FormattableString.Invariant($"Removed Client Endpoint {id} _clientEndpoints"), "RemoveClientEndpointInstalled"), "RemoveClientEndpointInstalled");
			this.ClientEndpointsModified?.Invoke(this, new EventArgs());
		}
		else
		{
			LogInfo(FormattableString.Invariant($"The Client Endpoint With ID = {id} does not exist. This message should be expected only when upgrading from a non-server configurator installation"), "RemoveClientEndpointInstalled");
		}
	}

	public void RemoveConfigurationElementInstalled(Guid id)
	{
		LogInfo(FormattableString.Invariant($"id = {id}"), "RemoveConfigurationElementInstalled");
		if (_configurationElementsInstalled.ContainsKey(id))
		{
			SystemConfigurationElement configurationElement = _configurationElementsInstalled[id];
			_configurationElementsInstalled.Remove(id);
			try
			{
				SystemConfigurationHandler.Cleanup(configurationElement, _registrationLogger, _logger);
				OverrideLocalManagementServer = false;
			}
			catch (Exception ex)
			{
				_logger?.LogError(FormattableString.Invariant($"Error occurred during System Configuration Cleanup: {ex}"));
			}
			LogInfo(LogString(FormattableString.Invariant($"Removed Configuration Element {id} _clientEndpoints"), "RemoveConfigurationElementInstalled"), "RemoveConfigurationElementInstalled");
			this.ConfigurationElementsModified?.Invoke(this, new EventArgs());
		}
		else
		{
			LogInfo(FormattableString.Invariant($"The Client Endpoint With ID = {id} does not exist. This message should be expected only when upgrading from a non-server configurator installation"), "RemoveConfigurationElementInstalled");
		}
	}

	public void SetLocationOnEndpointToBeInstalled(Guid endpointId, string location)
	{
		if (_clientEndpointsToBeInstalled.ContainsKey(endpointId))
		{
			_clientEndpointsToBeInstalled[endpointId].SetRegistrarFileLocation(location);
		}
		else if (_serverEndpointsToBeInstalled.ContainsKey(endpointId))
		{
			_serverEndpointsToBeInstalled[endpointId].SetRegistrarFileLocation(location);
		}
		else
		{
			if (!_configurationElementsToBeInstalled.ContainsKey(endpointId))
			{
				throw ExceptionAndLog<KeyNotFoundException>(FormattableString.Invariant($"The Endpoint With ID = {endpointId} does not exist"), "SetLocationOnEndpointToBeInstalled");
			}
			_configurationElementsToBeInstalled[endpointId].SetRegistrarFileLocation(location);
		}
		_logger.LogInfo(FormattableString.Invariant($"Endpoint Location of endd point with ID {endpointId} is set to {location}"));
	}

	public void SetGroupPresenterLocation(Guid certificateGroupId, string location)
	{
		if (_certificateGroups.ContainsKey(certificateGroupId))
		{
			_certificateGroups[certificateGroupId].SetPresenterFileLocation(location);
			_logger.LogInfo(FormattableString.Invariant($"Presenter Location of group with ID {certificateGroupId} is set to {location}"));
			return;
		}
		throw ExceptionAndLog<KeyNotFoundException>(FormattableString.Invariant($"The Group With ID = {certificateGroupId} does not exist"), "SetGroupPresenterLocation");
	}

	public ApplyStatus ApplyCertificatesForEndpointsToBeInstalled(IEnumerable<Guid> serverEndpointIds)
	{
		IEnumerable<ServerEndpointConfiguration> enumerable = serverEndpointIds.Select((Guid x) => _serverEndpointsToBeInstalled[x]);
		RegistrationResult registrationResult = new CertificateRegistration(enumerable, _certificateGroups, _registrationLogger, _logger, null).Register();
		if (registrationResult.ResultType.SuccessOrWarning())
		{
			foreach (ServerEndpointConfiguration item in enumerable)
			{
				_serverEndpointsInstalled[item.Id] = _serverEndpointsToBeInstalled[item.Id];
				_serverEndpointsToBeInstalled.Remove(item.Id);
			}
		}
		return registrationResult.GetResult<ApplyStatus>().Value;
	}

	public RegistrationResult ConfigureClientEndpointsToBeInstalled(IEnumerable<Guid> clientEndpointIds, bool offlineRegistration)
	{
		IEnumerable<ClientEndpointConfiguration> enumerable = clientEndpointIds.Select((Guid x) => _clientEndpointsToBeInstalled[x]);
		RegistrationResult registrationResult = null;
		if (offlineRegistration)
		{
			registrationResult = new RegistrationResult(new Dictionary<Type, object>
			{
				{
					typeof(RegisterClientStatus),
					RegisterClientStatus.Success
				},
				{
					typeof(SetServerAddressStatus),
					SetServerAddressStatus.Success
				}
			});
		}
		else
		{
			IInnerRegistration innerRegistration = new ClientRegistration(enumerable, null, GetAuthorizationServer(), _registrationLogger, _logger, null);
			registrationResult = ((IRegistration<ISetServerAddress>)new SetServerAddressHandler(enumerable, _registrationLogger, _logger, innerRegistration, GetManagementServer(secureState: false), GetManagementServer(secureState: true), _preferredSecureState == true, _certificateGroups.Values)).Register();
		}
		foreach (ClientEndpointConfiguration item in enumerable)
		{
			_clientEndpointsInstalled[item.Id] = _clientEndpointsToBeInstalled[item.Id];
			_clientEndpointsToBeInstalled.Remove(item.Id);
		}
		return registrationResult;
	}

	public SystemConfigurationStatus CommitConfigurationElementsToBeInstalled(IEnumerable<Guid> configurationElementIds)
	{
		if (configurationElementIds == null)
		{
			throw new ArgumentNullException("configurationElementIds");
		}
		foreach (Guid configurationElementId in configurationElementIds)
		{
			_configurationElementsInstalled[configurationElementId] = _configurationElementsToBeInstalled[configurationElementId];
			_configurationElementsToBeInstalled.Remove(configurationElementId);
		}
		return SystemConfigurationStatus.Success;
	}

	public RegistrationResult ApplyCertificatesAndRegisterAllEndpointsInstalled(NetworkCredential credentials)
	{
		return ApplyCertificatesAndRegisterEndpoints(_serverEndpointsInstalled.Values, credentials);
	}

	public RegistrationResult ApplyCertificatesAndRegisterAllEndpointsRelatedToGroup(Guid groupId, NetworkCredential credentials)
	{
		IEnumerable<ServerEndpointConfiguration> serverEndpoints = _serverEndpointsInstalled.Values.Where((ServerEndpointConfiguration x) => x.CertificateGroupId == groupId);
		return ApplyCertificatesAndRegisterEndpoints(serverEndpoints, credentials);
	}

	private RegistrationResult ApplyCertificatesAndRegisterEndpoints(IEnumerable<ServerEndpointConfiguration> serverEndpoints, NetworkCredential credentials)
	{
		RegistrationResult registrationResult = VerifySystemConsistency();
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			return registrationResult;
		}
		IInnerRegistration innerRegistration = null;
		innerRegistration = new CertificateAffectedClientRegistration(serverEndpoints, _clientEndpointsInstalled.Values, credentials, GetAuthorizationServer(), _registrationLogger, _logger, innerRegistration);
		innerRegistration = new SetServerAddressHandler(_clientEndpointsInstalled.Values, _registrationLogger, _logger, innerRegistration, GetManagementServer(secureState: false), GetManagementServer(secureState: true), _preferredSecureState == true, _certificateGroups.Values);
		innerRegistration = new ServiceRegistration(serverEndpoints, _registrationLogger, _logger, innerRegistration, credentials, GetServerToRegisterServiceTo(), GetUnsecureServerToRegisterServiceTo(), GetIdentityProvider(), GetServiceHost(), _certificateGroups.Values);
		innerRegistration = new SystemConfigurationHandler(_configurationElementsInstalled.Values.Where((SystemConfigurationElement x) => x.Purpose == ConfigurationPurpose.ChangeHostName), _registrationLogger, _logger, innerRegistration, GetOldManagementServer()?.Host, GetManagementServer(secureState: false), GetManagementServer(), GetAuthorizationServer(), credentials);
		return ((IRegistration<IApplyCertificate>)new CertificateRegistration(serverEndpoints, _certificateGroups, _registrationLogger, _logger, innerRegistration)).Register();
	}

	public RegistrationResult RegisterAllEndpointsInstalled(NetworkCredential credentials)
	{
		RegistrationResult registrationResult = VerifySystemConsistency();
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			return registrationResult;
		}
		IInnerRegistration innerRegistration = null;
		innerRegistration = new ClientRegistration(_clientEndpointsInstalled.Values, credentials, GetAuthorizationServer(), _registrationLogger, _logger, innerRegistration);
		innerRegistration = new SetServerAddressHandler(_clientEndpointsInstalled.Values, _registrationLogger, _logger, innerRegistration, GetManagementServer(secureState: false), GetManagementServer(secureState: true), _preferredSecureState == true, _certificateGroups.Values);
		innerRegistration = new SetSqlServerHandler(_serverEndpointsInstalled.Values, _registrationLogger, _logger, innerRegistration, new ConnectionStringParameters
		{
			Host = LocalHost
		});
		innerRegistration = new ServiceRegistration(_serverEndpointsInstalled.Values, _registrationLogger, _logger, innerRegistration, credentials, GetServerToRegisterServiceTo(), GetUnsecureServerToRegisterServiceTo(), GetIdentityProvider(), GetServiceHost(), _certificateGroups.Values);
		return ((IRegistration<ISystemConfiguration>)new SystemConfigurationHandler(_configurationElementsInstalled.Values.Where((SystemConfigurationElement x) => x.Purpose == ConfigurationPurpose.ChangeHostName), _registrationLogger, _logger, innerRegistration, GetOldManagementServer()?.Host, GetManagementServer(secureState: false), GetManagementServer(), GetAuthorizationServer(), credentials)).Register();
	}

	public RegistrationResult VerifySystemConsistency()
	{
		bool? secureState = GetSecureState();
		bool? flag;
		bool flag2;
		if (secureState.HasValue)
		{
			(flag, flag2) = IsManagementServerSecure();
		}
		else
		{
			flag = null;
			flag2 = false;
		}
		if (flag2)
		{
			return new RegistrationResult(ConsistencyStatus.ServerCertificateIsNotTrusted);
		}
		if (secureState.HasValue && flag.HasValue && secureState.Value != flag.Value)
		{
			return new RegistrationResult(ConsistencyStatus.ClientEndpointSecuritySchemeDoesNotMatchServerEndpoint);
		}
		return new RegistrationResult(ConsistencyStatus.Success);
	}

	public void SetManagementServer(Uri managementServer, bool secureState)
	{
		if (_oldManagementServer[secureState] != managementServer)
		{
			_oldManagementServer[secureState] = _managementServer[secureState];
		}
		_managementServer[secureState] = managementServer ?? throw new ArgumentNullException("managementServer");
		if (!IsAbsolutePathEmpty(managementServer))
		{
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant(FormattableStringFactory.Create("Path of {0} is {1}, this is not allowed, only empty path is allowed", "managementServer", managementServer.AbsolutePath)), "SetManagementServer");
		}
		SetAuthorizationServer(_managementServer[secureState]);
		_logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("ManagementServer for {0} communication is set to {1}, with Port set to {2}", secureState ? "Secure" : "Unsecure", _managementServer[secureState].AbsoluteUri, _managementServer[secureState].Port)));
	}

	public void SetManagementServer(Uri managementServer)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer");
		}
		if (!IsAbsolutePathEmpty(managementServer))
		{
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant(FormattableStringFactory.Create("Path of {0} is {1}, this is not allowed, only empty path is allowed", "managementServer", managementServer.AbsolutePath)), "SetManagementServer");
		}
		bool flag = managementServer.Scheme == Uri.UriSchemeHttps;
		SetManagementServer(managementServer, flag);
		Uri uri = new UriBuilder(_managementServer[!flag]?.Scheme ?? (flag ? Uri.UriSchemeHttp : Uri.UriSchemeHttps), portNumber: _managementServer[!flag]?.Port ?? (flag ? 80 : 443), host: managementServer.Host).Uri;
		SetManagementServer(uri, !flag);
	}

	public void SetPreferredSecureState(bool secure)
	{
		_preferredSecureState = secure;
	}

	public Uri GetManagementServer()
	{
		return GetManagementServer(GetSecureState() == true);
	}

	public Uri GetManagementServer(bool secureState)
	{
		if (UseLocalManagementServer())
		{
			if (_managementServer[secureState] == null)
			{
				return null;
			}
			int port = _managementServer[secureState].Port;
			string scheme = _managementServer[secureState].Scheme;
			string localHost = LocalHost;
			_managementServer[secureState] = new UriBuilder(scheme, localHost, port).Uri;
		}
		return _managementServer[secureState];
	}

	private string GetServiceHost()
	{
		if (OverrideLocalManagementServer)
		{
			return GetManagementServer().Host;
		}
		return LocalHost;
	}

	public Uri GetOldManagementServer()
	{
		return GetOldManagementServer(GetSecureState() == true);
	}

	public Uri GetOldManagementServer(bool secureState)
	{
		if (UseLocalManagementServer())
		{
			int portNumber = _oldManagementServer[secureState]?.Port ?? _managementServer[secureState].Port;
			string scheme = _oldManagementServer[secureState]?.Scheme ?? _managementServer[secureState].Scheme;
			string host = _oldHostName ?? LocalHost;
			_oldManagementServer[secureState] = new UriBuilder(scheme, host, portNumber).Uri;
		}
		return _oldManagementServer[secureState] ?? _managementServer[secureState];
	}

	public void SetAuthorizationServer(DnsEndPoint managementServer, bool secureState)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer");
		}
		if (secureState)
		{
			_authorizationServer[secureState] = new UriBuilder(Uri.UriSchemeHttps, managementServer.Host, managementServer.Port, "IDP").Uri;
			return;
		}
		_authorizationServer[false] = new UriBuilder(Uri.UriSchemeHttp, managementServer.Host, managementServer.Port, "IDP").Uri;
		_authorizationServer[true] = new UriBuilder(Uri.UriSchemeHttps, managementServer.Host, _managementServer[true]?.Port ?? 443, "IDP").Uri;
	}

	public void SetAuthorizationServer(Uri managementServer)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer");
		}
		_authorizationServer[managementServer.Scheme == Uri.UriSchemeHttps] = new UriBuilder(managementServer.Scheme, managementServer.Host, managementServer.Port, "IDP").Uri;
		_logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("AuthorizationServer for {0} communication is set to {1}", (managementServer.Scheme == Uri.UriSchemeHttps) ? "Secure" : "Unsecure", _authorizationServer[managementServer.Scheme == Uri.UriSchemeHttps].AbsoluteUri)));
	}

	public Uri GetAuthorizationServer()
	{
		return GetAuthorizationServer(GetSecureState() == true);
	}

	public Uri GetAuthorizationServer(bool secureState)
	{
		if (UseLocalManagementServer())
		{
			_authorizationServer[secureState] = new UriBuilder(GetManagementServer(secureState))
			{
				Path = _authorizationServer[secureState].AbsolutePath
			}.Uri;
		}
		return _authorizationServer[secureState];
	}

	private Uri GetUnsecureServerToRegisterServiceTo()
	{
		return GetManagementServer(secureState: false);
	}

	private Uri GetServerToRegisterServiceTo()
	{
		return GetManagementServer();
	}

	private Uri GetIdentityProvider()
	{
		return GetAuthorizationServer();
	}

	private static bool IsAbsolutePathEmpty(Uri clientRegistrationServer)
	{
		if (clientRegistrationServer.AbsolutePath.Equals("/", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	public bool IsValidThumbprint(string thumbprint, Guid certificateGroupId)
	{
		try
		{
			ModifiableCertificateGroup certificateGroup = GetCertificateGroup(certificateGroupId, "IsValidThumbprint");
			if (certificateGroup.ValidCertificates != null)
			{
				return certificateGroup.ValidCertificates.Contains(thumbprint);
			}
			return false;
		}
		catch (KeyNotFoundException)
		{
			return false;
		}
	}

	public bool? GetSecureState()
	{
		return GetServerGroupSecureState() ?? _preferredSecureState;
	}

	public bool? GetServerGroupSecureState()
	{
		IEnumerable<KeyValuePair<Guid, ClientEndpointConfiguration>> source = from x in _clientEndpointsInstalled.Concat(_clientEndpointsToBeInstalled)
			where x.Value.ServerGroupDependency != Guid.Empty
			select x;
		if (source.Any())
		{
			Guid serverGroup = source.First().Value.ServerGroupDependency;
			if (_certificateGroups.ContainsKey(serverGroup) && _serverEndpointsInstalled.Concat(_serverEndpointsToBeInstalled).Any((KeyValuePair<Guid, ServerEndpointConfiguration> x) => x.Value.CertificateGroupId == serverGroup))
			{
				return _certificateGroups[serverGroup].UseEncryption;
			}
		}
		return null;
	}

	public bool UseLocalManagementServer()
	{
		if (!OverrideLocalManagementServer)
		{
			return IsLocalManagementServerPresent();
		}
		return false;
	}

	public bool IsLocalManagementServerPresent()
	{
		return _configurationElementsInstalled.Values.Any((SystemConfigurationElement x) => x.SystemFeature == SystemFeature.IsManagementServer);
	}

	public (bool? secureState, bool certificateError) IsManagementServerSecure()
	{
		if (IsLocalManagementServerPresent())
		{
			return (secureState: null, certificateError: false);
		}
		return _managementServerHelper.IsSecure(GetManagementServer(), GetManagementServer(secureState: false));
	}

	public void SetOverrideLocalManagementServer(bool value)
	{
		if (value && !IsLocalManagementServerPresent())
		{
			throw new InvalidOperationException("You cannot override Local Management server, since there is no local management server");
		}
		OverrideLocalManagementServer = value;
	}

	public void SetPersistedHostName(string persistedHostName)
	{
		if (string.Compare(LocalHost, persistedHostName, StringComparison.OrdinalIgnoreCase) != 0)
		{
			_oldHostName = persistedHostName;
		}
	}

	public void AddCertificateGroup(CertificateGroup certificateGroup)
	{
		if (certificateGroup == null)
		{
			throw ArgumentNullExceptionAndLog("certificateGroup", "AddCertificateGroup");
		}
		if (_certificateGroups.ContainsKey(certificateGroup.Id))
		{
			_certificateGroups[certificateGroup.Id].Merge(certificateGroup);
			_logger.LogInfo(FormattableString.Invariant($"Merged instances of group {certificateGroup.Id}"));
		}
		else
		{
			ModifiableCertificateGroup value = (certificateGroup as ModifiableCertificateGroup) ?? new ModifiableCertificateGroup(certificateGroup);
			_certificateGroups[certificateGroup.Id] = value;
			_logger.LogInfo(FormattableString.Invariant($"Added group {certificateGroup.Id}"));
		}
		IEnumerable<Func<ICertificate, bool>> filters = FilterProvider.GetFilters<ICertificate>(certificateGroup);
		_certificateGroups[certificateGroup.Id].SetAvailableCertificates(_certificateProvider.GetValidCertificates(filters));
		if (certificateGroup.SelectedCertificate != null && certificateGroup.SelectedCertificate.X509Certificate == null)
		{
			ICertificate certificate = _certificateProvider.GetCertificate(certificateGroup.SelectedCertificate.Thumbprint);
			if (certificate != null)
			{
				_certificateGroups[certificateGroup.Id].SetSelectedCertificate(certificate);
			}
		}
		this.CertificateGroupsModified?.Invoke(this, new EventArgs());
	}

	public void RemoveCertificateGroup(Guid id)
	{
		if (!_certificateGroups.ContainsKey(id))
		{
			throw ExceptionAndLog<KeyNotFoundException>(FormattableString.Invariant($"The Certificate Group with Id = {id} does not exist."), "RemoveCertificateGroup");
		}
		IEnumerable<ServerEndpointConfiguration> first = _serverEndpointsInstalled.Values.Where((ServerEndpointConfiguration x) => x.CertificateGroupId == id);
		first = first.Concat(_serverEndpointsToBeInstalled.Values.Where((ServerEndpointConfiguration x) => x.CertificateGroupId == id));
		if (first.Any())
		{
			throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant(FormattableStringFactory.Create("The Certificate Group with Id = {0} cannot be removed, it is referred by these Servers: {1}", id, string.Join("; ", first.Select((ServerEndpointConfiguration x) => x.Id)))), "RemoveCertificateGroup");
		}
		_certificateGroups.Remove(id);
		this.CertificateGroupsModified?.Invoke(this, new EventArgs());
	}

	public void SetCertificate(string thumbprint, Guid certificateGroupId)
	{
		ModifiableCertificateGroup certificateGroup = GetCertificateGroup(certificateGroupId, "SetCertificate");
		ICertificate selectedCertificate;
		if (thumbprint != null)
		{
			IEnumerable<ICertificate> validCertificates = certificateGroup.ValidCertificates;
			if (validCertificates == null || !validCertificates.Contains(thumbprint))
			{
				throw ExceptionAndLog<InvalidOperationException>(FormattableString.Invariant($"The Certificate (Thumbprint = {thumbprint}) is not valid for the certificate group with Id = {certificateGroupId}"), "SetCertificate");
			}
			selectedCertificate = _certificateProvider.GetCertificate(thumbprint);
		}
		else
		{
			selectedCertificate = null;
		}
		certificateGroup.SetSelectedCertificate(selectedCertificate);
		this.CertificateGroupsModified?.Invoke(this, new EventArgs());
	}

	public void SetUseEncryption(bool newValue, Guid certificateGroupId)
	{
		GetCertificateGroup(certificateGroupId, "SetUseEncryption").SetUseEncryption(newValue);
		this.CertificateGroupsModified?.Invoke(this, new EventArgs());
		if (_clientEndpointsInstalled.Concat(_clientEndpointsToBeInstalled).Any((KeyValuePair<Guid, ClientEndpointConfiguration> c) => c.Value.ServerGroupDependency == certificateGroupId))
		{
			this.ClientRegistrationServerModified?.Invoke(this, new EventArgs());
		}
	}

	public void SetLockEncryption(bool encryptionValue, bool lockValue, Guid certificateGroupId)
	{
		GetCertificateGroup(certificateGroupId, "SetLockEncryption").SetLockEncryption(lockValue);
		SetUseEncryption(encryptionValue, certificateGroupId);
	}

	private ModifiableCertificateGroup GetCertificateGroup(Guid certificateGroupId, [CallerMemberName] string methodName = "")
	{
		if (!_certificateGroups.ContainsKey(certificateGroupId))
		{
			throw ExceptionAndLog<KeyNotFoundException>(FormattableString.Invariant($"The Certificate Group with Id = {certificateGroupId} does not exist."), methodName);
		}
		return _certificateGroups[certificateGroupId];
	}

	private void UpdateServerGroupLocks(IEnumerable<ServerEndpointConfiguration> serverEndpointsToBeInstalled)
	{
		foreach (ModifiableCertificateGroup certificateGroup in _certificateGroups.Values)
		{
			IEnumerable<Guid> first = from x in _serverEndpointsInstalled.Values
				where x.CertificateGroupId == certificateGroup.Id
				select x.Id;
			IEnumerable<Guid> second = from x in serverEndpointsToBeInstalled
				where x.CertificateGroupId == certificateGroup.Id
				select x.Id;
			bool flag = first.Except(second).Any();
			certificateGroup.SetLock(flag);
		}
	}

	private Exception ArgumentNullExceptionAndLog(string variableName, [CallerMemberName] string methodName = "")
	{
		return ExceptionAndLog<ArgumentNullException>(FormattableString.Invariant($"Method '{methodName}' called with '{variableName}' = null"), methodName);
	}

	private Exception ExceptionAndLog<T>(string errormessage, [CallerMemberName] string methodName = "") where T : Exception
	{
		_logger?.LogError(LogString(errormessage, methodName));
		Exception ex = null;
		try
		{
			return (T)Activator.CreateInstance(typeof(T), errormessage);
		}
		catch (Exception)
		{
			return new Exception(FormattableString.Invariant($"{typeof(T).Name}: {errormessage}"));
		}
	}

	private string LogString(string message, [CallerMemberName] string methodName = "")
	{
		return FormattableString.Invariant($"{GetType().Name}.{methodName}: {message}");
	}

	private void LogInfo(string message, [CallerMemberName] string methodName = "")
	{
		_logger?.LogInfo(LogString(message, methodName));
	}

	private void LogWarning(string message, [CallerMemberName] string methodName = "")
	{
		_logger?.LogWarning(LogString(message, methodName));
	}

	public void SetApplicationUICulture(CultureInfo cultureInfo)
	{
		ApplicationUICulture = cultureInfo;
	}

	public void Dispose()
	{
		_managementServerHelper?.Dispose();
	}
}

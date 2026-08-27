#define TRACE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Xml;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.SDK.Config.Shared;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config;

public class SystemInfo : ISystemInfo
{
	private enum populated
	{
		notattempted,
		failed,
		succeeded
	}

	private Uri _uri = new Uri("http://127.0.0.1");

	private populated _populated;

	private DateTime _lastPopulatedTry = DateTime.MinValue;

	private object _populateLock = new object();

	private ServerTypes _defaultServerType = ServerTypes.CORPORATE;

	public static long TokenRetryInterval = 18000L;

	public Guid InstanceId = Guid.NewGuid();

	private static List<Guid> _arcusSupport = new List<Guid>
	{
		Kind.Server,
		Kind.Camera,
		Kind.Folder
	};

	private object _retryTimerLock = new object();

	private Timer _connectRetryTimer;

	private Collection<IDisposable> _toBeDisposed = new Collection<IDisposable>();

	public string UserName { get; set; }

	public SecureString Password { get; set; }

	public string MacAddress { get; set; }

	public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

	public AuthenticationType AuthenticationType { get; public set; }

	public HttpStatusCode LastStatusCode { get; set; }

	public IConfigurationInfo ConfigurationInfo { get; private set; }

	public Exception LastException { get; set; }

	public string Url
	{
		get
		{
			string text = _uri.ToString();
			return text.Substring(0, text.Length - _uri.LocalPath.Length);
		}
		set
		{
			string text = value;
			if (!text.ToLower(CultureInfo.InvariantCulture).StartsWith("http"))
			{
				text = "http://" + text;
			}
			_uri = new Uri(text);
		}
	}

	public Uri Uri
	{
		get
		{
			string text = _uri.ToString();
			return new Uri(text.Substring(0, text.Length - _uri.LocalPath.Length));
		}
	}

	public string Hostname => _uri.DnsSafeHost;

	public int Port => _uri.Port;

	public bool IsConnected
	{
		get
		{
			if (Invalidated)
			{
				return false;
			}
			if (LoginServer == null)
			{
				return false;
			}
			return LoginServer.CorporateSystemInfo != null;
		}
	}

	public bool Invalidated { get; set; }

	public ServerTypes ServerType
	{
		get
		{
			if (LoginServer == null)
			{
				return _defaultServerType;
			}
			return LoginServer.ServerType;
		}
	}

	public bool SlavesPopulated { get; public set; }

	public InternalList<Guid> SlaveIds { get; private set; }

	public CameraCollection Cameras { get; private set; }

	public RecorderCollection Recorders { get; private set; }

	public MicrophoneCollection Microphones { get; private set; }

	public SpeakerCollection Speakers { get; private set; }

	public UserDefinedEventCollection Events { get; private set; }

	public OutputDeviceCollection OutputDevices { get; private set; }

	public MatrixMonitorCollection MatrixMonitors { get; private set; }

	public IDictionary<Guid, IMetadataDevice> MetadataDevices { get; private set; }

	public string Token
	{
		get
		{
			if (LoginServer != null && LoginServer.LoginInfo != null)
			{
				return LoginServer.LoginInfo.Token;
			}
			return "No-Token";
		}
	}

	public Guid Id => LoginSettings.Guid;

	public string Name { get; set; }

	public UserContext UserContext { get; }

	public NetworkCredential Credentials
	{
		get
		{
			if (LoginSettings.Uri == null || LoginSettings.CredentialCache == null)
			{
				return new NetworkCredential();
			}
			NetworkCredential credentialFromCache = GetCredentialFromCache(LoginSettings.CredentialCache, UriWithoutHttpScheme(LoginSettings.Uri), LoginSettings.Uri.Port, "Negotiate");
			if (credentialFromCache != null)
			{
				return credentialFromCache;
			}
			credentialFromCache = GetCredentialFromCache(LoginSettings.CredentialCache, UriWithoutHttpScheme(LoginSettings.Uri), LoginSettings.Uri.Port, "Basic");
			if (credentialFromCache != null)
			{
				return credentialFromCache;
			}
			credentialFromCache = GetCredentialFromCache(LoginSettings.CredentialCache, LoginSettings.Uri.Host, LoginSettings.Uri.Port, "Negotiate");
			if (credentialFromCache != null)
			{
				return credentialFromCache;
			}
			return GetCredentialFromCache(LoginSettings.CredentialCache, LoginSettings.Uri.Host, LoginSettings.Uri.Port, "Basic");
		}
	}

	public ServerId ServerId => new ServerId("XPCO", Hostname, Port, Id)
	{
		UserContext = UserContext
	};

	public FQID FQID => new FQID(ServerId, Guid.Empty, Id, FolderType.SystemDefined, Kind.Server);

	public LoginSettings LoginSettings { get; set; }

	public bool Populated => _populated == populated.succeeded;

	public bool PopulatedHasBeenTried => _populated != populated.notattempted;

	public bool MasterOnly { get; set; }

	public ILoginServer LoginServer { get; private set; }

	public DateTime LastHardwareUpdate { get; private set; } = DateTime.MinValue;

	public SystemInfo(LoginSettings loginSettings, UserContext userContext)
	{
		InitializeConfiguration();
		LoginSettings = loginSettings;
		Name = LoginSettings.Uri.Host;
		Url = loginSettings.Uri.ToString();
		LoginSettings.RequestNewTokenEvent += _loginSettings_RequestNewTokenEvent;
		UserContext = userContext;
		LoginServer = null;
	}

	private void _loginSettings_RequestNewTokenEvent(object sender, EventArgs e)
	{
		if (LoginServer != null)
		{
			LoginServer.RequestNewToken();
		}
	}

	private void InitializeConfiguration()
	{
		Recorders = new RecorderCollection(this);
		Cameras = new CameraCollection();
		Microphones = new MicrophoneCollection();
		Speakers = new SpeakerCollection();
		Events = new UserDefinedEventCollection();
		MatrixMonitors = new MatrixMonitorCollection();
		OutputDevices = new OutputDeviceCollection();
		foreach (IRecorder recorder in Recorders)
		{
			if (recorder.Cameras != null)
			{
				Cameras.AddRange(recorder.Cameras);
			}
			Microphones.AddRange(recorder.Microphones);
			Speakers.AddRange(recorder.Speakers);
			OutputDevices.AddRange(recorder.OutputDevices);
		}
		Events = new UserDefinedEventCollection(this);
		MatrixMonitors = new MatrixMonitorCollection(this);
		PopulateMetadataDevices();
		SlaveIds = new InternalList<Guid>();
		SlavesPopulated = false;
		ConfigurationInfo = null;
		if (_populated == populated.succeeded)
		{
			_populated = populated.notattempted;
		}
	}

	public bool ProductSupportOfKind(Guid kind)
	{
		return true;
	}

	void ISystemInfo.AddSingleCameraConfig(Guid recorderGuid, XmlDocument doc)
	{
		throw new NotImplementedException("Adding single configuration via XML is not implemented. ");
	}

	void ISystemInfo.AddSingleMicrophoneConfig(Guid recorderGuid, XmlDocument doc)
	{
		throw new NotImplementedException("Adding single configuration via XML is not implemented. ");
	}

	private static string UriWithoutHttpScheme(Uri uri)
	{
		if (uri != null)
		{
			return uri.DnsSafeHost + uri.AbsolutePath;
		}
		return "";
	}

	private static NetworkCredential GetCredentialFromCache(CredentialCache cache, string uri, int port, string authType)
	{
		NetworkCredential credential = cache.GetCredential(new UriBuilder(Uri.UriSchemeHttps, uri, port).Uri, authType);
		if (credential == null)
		{
			credential = cache.GetCredential(new UriBuilder(Uri.UriSchemeHttp, uri, port).Uri, authType);
		}
		return credential;
	}

	public void UpdateServerId(Guid id)
	{
		LoginSettings.Guid = id;
	}

	public string GetMIPServerType()
	{
		return "XPCO";
	}

	public bool Populate()
	{
		if (_populated == populated.failed && _lastPopulatedTry + TimeSpan.FromMinutes(1.0) < DateTime.UtcNow)
		{
			_populated = populated.notattempted;
		}
		if (_populated == populated.notattempted)
		{
			lock (_populateLock)
			{
				if (_populated == populated.notattempted)
				{
					try
					{
						if (ServerOfflineCache.IsServerOnline(_uri))
						{
							if (!IsLoggedIn())
							{
								ServerTrace.BeforeCall("SystemInfo", "Populate.Login", _uri.ToString() + ", auth:" + AuthenticationType);
								Login();
								ServerTrace.AfterCall("SystemInfo", "Populate.Login", _uri.ToString() + ", IsLoggedIn:" + IsLoggedIn());
								if (ServerOfflineCache.IsServerOffline(_uri))
								{
									_lastPopulatedTry = DateTime.UtcNow;
									return false;
								}
								if (!string.IsNullOrEmpty(UserContext.ClientType) && !SecurityAccess.IsClientAllowed(ServerId, UserContext.ClientType))
								{
									Logout();
									throw new ClientTypeNotAllowedMIPException("Client type not allowed");
								}
							}
							string[] obj = new string[5]
							{
								_uri.ToString(),
								", auth:",
								AuthenticationType.ToString(),
								", Id=",
								null
							};
							Guid instanceId = InstanceId;
							obj[4] = instanceId.ToString();
							ServerTrace.BeforeCall("SystemInfo", "Populate.PopulateFillAndSetPopulated", string.Concat(obj));
							PopulateFillAndSetPopulated();
							ServerTrace.AfterCall("SystemInfo", "Populate.PopulateFillAndSetPopulated", _uri.ToString() + ", auth:" + AuthenticationType);
						}
						else
						{
							ServerTrace.AfterCall("SystemInfo", "Populate - Ignored as server offline", _uri.ToString());
						}
					}
					catch (Exception lastException)
					{
						LastException = lastException;
						_populated = populated.failed;
					}
					_lastPopulatedTry = DateTime.UtcNow;
				}
			}
		}
		return _populated == populated.succeeded;
	}

	private void PopulateFillAndSetPopulated()
	{
		try
		{
			if (PopulateFillConfiguration())
			{
				_populated = populated.succeeded;
				EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LocalConfigurationChangedIndication, FQID));
			}
			else
			{
				_populated = populated.failed;
			}
		}
		catch (Exception lastException)
		{
			LastException = lastException;
			_populated = populated.failed;
		}
	}

	public bool Login()
	{
		LastStatusCode = HttpStatusCode.OK;
		if (LoginServer == null)
		{
			ConstructLoginServer();
		}
		if (LastStatusCode == HttpStatusCode.Unauthorized)
		{
			throw new InvalidCredentialsMIPException(_uri.DnsSafeHost);
		}
		if (LoginServer != null)
		{
			ServerTypeCache.ServerInfo serverInfo = ServerTypeCache.TryGet(_uri);
			ParallelHandler.RecheckServerOnlineState(_uri.ToString(), LoginSettings.CredentialCache, serverInfo?.IsOAuthServer ?? true);
			if (ServerOfflineCache.IsServerOnline(_uri))
			{
				string text = _uri.ToString();
				Guid instanceId = InstanceId;
				ServerTrace.BeforeCall("SystemInfo", "Login", text + ", Id=" + instanceId.ToString());
				try
				{
					LoginServer.Login(_uri);
				}
				catch (MIPException)
				{
					LoginServer.Logout();
					throw;
				}
				ServerTrace.AfterCall("SystemInfo", "Login", _uri.ToString());
			}
		}
		if (LastStatusCode == HttpStatusCode.Unauthorized)
		{
			throw new InvalidCredentialsMIPException(_uri.DnsSafeHost);
		}
		return IsLoggedIn();
	}

	public void SetConnectRetry(bool autoConnect)
	{
		lock (_retryTimerLock)
		{
			if (_connectRetryTimer == null)
			{
				_connectRetryTimer = new Timer(ConnectRetry_Callback);
			}
			_connectRetryTimer.Change(autoConnect ? TokenRetryInterval : (-1), -1L);
		}
	}

	private void ConnectRetry_Callback(object obj)
	{
		if (LoginServer == null)
		{
			try
			{
				ConstructLoginServer();
				if (LoginServer != null && LoginServer.Login(_uri) && _populated == populated.failed)
				{
					_populated = populated.notattempted;
				}
			}
			catch (Exception lastException)
			{
				LastException = lastException;
			}
		}
		else
		{
			try
			{
				if (!LoginServer.IsLoggedIn())
				{
					LoginServer.Login(_uri);
				}
				if (LoginServer.IsLoggedIn() && _populated != populated.succeeded)
				{
					Populate();
				}
			}
			catch (Exception)
			{
			}
		}
		lock (_retryTimerLock)
		{
			_connectRetryTimer?.Change(TokenRetryInterval, -1L);
		}
	}

	private void ConstructLoginServer()
	{
		ServerTrace.BeforeCall("SystemInfo", "ConstructLoginServer", _uri.ToString());
		ConstructLoginServer2();
		ServerTrace.AfterCall("SystemInfo", "ConstructLoginServer", _uri.ToString());
	}

	private void ConstructLoginServer2()
	{
		string originalString = _uri.OriginalString;
		bool flag = false;
		if (!ServerTypeCache.Contains(_uri))
		{
			if (ServerOfflineCache.IsServerOffline(_uri))
			{
				return;
			}
			ParallelHandler.FindBestAddress(LoginSettings.SecureOnly, new List<string> { _uri.ToString() }, LoginSettings.CredentialCache, out var unauthorized, LoginSettings.ServerType);
			if (unauthorized)
			{
				LastStatusCode = HttpStatusCode.Unauthorized;
				return;
			}
		}
		if (ServerTypeCache.Contains(_uri))
		{
			ServerTypeCache.Fill(LoginSettings);
			ServerTypeCache.ServerInfo serverInfo = ServerTypeCache.TryGet(_uri);
			Uri uri = new Uri(serverInfo.ServerAddress);
			if (serverInfo.IsOAuthServer && uri != _uri)
			{
				if (!LoginSettings.IsOAuthIdentity)
				{
					CredentialUtil.UpdateCredentialCacheUri(_uri, uri, LoginSettings.CredentialCache);
				}
				_uri = uri;
			}
			if (ServerOfflineCache.IsServerOnline(_uri))
			{
				flag = true;
			}
		}
		if (flag)
		{
			ServerTypeCache.ServerInfo serverInfo2 = ServerTypeCache.TryGet(_uri);
			if (serverInfo2 != null && serverInfo2.IsOAuthServer)
			{
				_uri = new UriBuilder(serverInfo2.ServerAddress).Uri;
				if (LoginSettings.IdentityTokenCache == null)
				{
					MipTokenCacheInternal identityTokenCache = new MipTokenCacheInternal(new Uri(_uri, "idp"), Credentials, LoginSettings.IsBasicUser);
					LoginSettings.IdentityTokenCache = identityTokenCache;
					EnvironmentManager.Instance.Log(error: false, "SystemInfo", $"Setting IdentityToken = value for instance={LoginSettings.InstanceGuid}");
				}
				if (string.Compare(LoginSettings.ServerProductInfo.ProductVersion, "24.2", ignoreCase: false, CultureInfo.InvariantCulture) >= 0)
				{
					LoginServer = new LoginServerRest(UserContext, this);
				}
				else
				{
					LoginServer = new LoginServerOauth(UserContext, this);
				}
			}
			else
			{
				LoginServer = new LoginServerSvc(UserContext, this);
			}
		}
		if (LoginServer != null)
		{
			ServerTypeCache.ServerIdentified(LoginSettings, originalString);
			return;
		}
		ServerTypeCache.ServerNotFound(LoginSettings);
		Trace.WriteLine("ConstructLoginServer: Unable to identify server type:" + _uri.DnsSafeHost);
		ServerOfflineCache.AddServer(_uri);
	}

	private bool PopulateFillConfiguration()
	{
		try
		{
			if (LoginServer != null && LoginServer.GetSystemInfoAndLogin())
			{
				if (Invalidated)
				{
					Invalidated = false;
				}
				FillConfiguration();
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = (LastException = ex);
			Trace.WriteLine(ex2.GetType()?.ToString() + ": " + ex2.Message + "\n" + ex2.StackTrace);
		}
		return IsConnected;
	}

	public bool IsLoggedIn()
	{
		if (LoginServer == null)
		{
			return false;
		}
		return LoginServer.IsLoggedIn();
	}

	public bool ServerConnectionCheck()
	{
		if (LoginServer == null)
		{
			ConstructLoginServer();
		}
		if (LoginServer == null)
		{
			return false;
		}
		if (!LoginServer.IsServerAvailable(_uri))
		{
			LastException = LoginServer.LastException;
			return false;
		}
		return true;
	}

	public bool Logout()
	{
		LastHardwareUpdate = DateTime.MinValue;
		Disconnect();
		InitializeConfiguration();
		return true;
	}

	public void Disconnect()
	{
		lock (_retryTimerLock)
		{
			if (_connectRetryTimer != null)
			{
				_connectRetryTimer.Dispose();
			}
			_connectRetryTimer = null;
		}
		if (LoginSettings != null)
		{
			LoginSettings.Token = "InvalidToken";
			LoginSettings.RequestNewTokenEvent -= _loginSettings_RequestNewTokenEvent;
		}
		if (LoginServer != null)
		{
			LoginServer.Logout();
			LoginServer = null;
		}
		lock (_toBeDisposed)
		{
			foreach (IDisposable item in _toBeDisposed)
			{
				item.Dispose();
			}
			_toBeDisposed.Clear();
		}
	}

	public void AddToDisposableObjects(IDisposable obj)
	{
		lock (_toBeDisposed)
		{
			_toBeDisposed.Add(obj);
		}
	}

	public Collection<IDisposable> DisposableObjects()
	{
		return _toBeDisposed;
	}

	private void FillConfiguration()
	{
		Cameras = new CameraCollection();
		Microphones = new MicrophoneCollection();
		Speakers = new SpeakerCollection();
		OutputDevices = new OutputDeviceCollection();
		if (LoginServer.ServerType != ServerTypes.CORPORATE)
		{
			return;
		}
		ConfigurationInfo = LoginServer.CorporateSystemInfo;
		Recorders = new RecorderCollection(this);
		foreach (IRecorder recorder in Recorders)
		{
			Cameras.AddRange(recorder.Cameras);
			Microphones.AddRange(recorder.Microphones);
			Speakers.AddRange(recorder.Speakers);
			OutputDevices.AddRange(recorder.OutputDevices);
		}
		Events = new UserDefinedEventCollection(this);
		MatrixMonitors = new MatrixMonitorCollection(this);
		PopulateMetadataDevices();
	}

	private void PopulateMetadataDevices()
	{
		if (Recorders == null)
		{
			MetadataDevices = new Dictionary<Guid, IMetadataDevice>();
			return;
		}
		MetadataDevices = Recorders.Where((IRecorder recorder) => recorder != null && recorder.MetadataDevices != null).SelectMany((IRecorder recorder) => recorder.MetadataDevices.Values).ToDictionary((IMetadataDevice device) => device.Guid, (IMetadataDevice device) => device);
	}

	public void RebuildDeviceCollections()
	{
		CameraCollection cameraCollection = new CameraCollection();
		MicrophoneCollection microphoneCollection = new MicrophoneCollection();
		SpeakerCollection speakerCollection = new SpeakerCollection();
		OutputDeviceCollection outputDeviceCollection = new OutputDeviceCollection();
		foreach (IRecorder recorder in Recorders)
		{
			cameraCollection.AddRange(recorder.Cameras);
			microphoneCollection.AddRange(recorder.Microphones);
			speakerCollection.AddRange(recorder.Speakers);
			outputDeviceCollection.AddRange(recorder.OutputDevices);
		}
		Cameras = cameraCollection;
		Microphones = microphoneCollection;
		Speakers = speakerCollection;
		OutputDevices = outputDeviceCollection;
		PopulateMetadataDevices();
		Events = new UserDefinedEventCollection(this);
		MatrixMonitors = new MatrixMonitorCollection(this);
	}

	public void RefreshRecorder(FQID recorderFqid)
	{
		try
		{
			if (LoginServer.ServerType != ServerTypes.CORPORATE)
			{
				return;
			}
			IRecorderInfo[] recorderInfos = LoginServer.GetRecorderInfos(new Guid[1] { recorderFqid.ServerId.Id });
			if (recorderInfos.Length != 1)
			{
				EnvironmentManager.Instance.Log(error: true, "RefreshRecorder", "Recorder identified by FQID could not be reloaded from server - ignored.  ServerType=" + recorderFqid.ServerId.ServerType);
				return;
			}
			if (Recorders.ContainsKey(recorderFqid.ServerId.Id))
			{
				if (Recorders[recorderFqid.ServerId.Id] is Recorder recorder)
				{
					recorder.UpdateRecorder(recorderInfos[0]);
					EnvironmentManager.Instance.Log(error: false, "RefreshRecorder", "Recorder configuration reloaded: " + recorder.HostName);
				}
			}
			else
			{
				Recorder recorder2 = new Recorder(this, recorderInfos[0]);
				Recorders.Add(recorder2);
				EnvironmentManager.Instance.Log(error: false, "RefreshRecorder", "Recorder configuration added: " + recorder2.HostName);
			}
			RebuildDeviceCollections();
			EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LocalConfigurationChangedIndication, FQID));
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SystemInfo", "RefreshRecorder", exception);
		}
	}

	public void RebuildConfiguration()
	{
		try
		{
			if (LoginServer.ServerType == ServerTypes.CORPORATE)
			{
				ConfigurationInfo = LoginServer.CorporateSystemInfo;
				IRecorderInfo[] recorders = ConfigurationInfo.Recorders;
				foreach (IRecorderInfo recorderInfo in recorders)
				{
					if (Recorders[recorderInfo.RecorderId] is Recorder recorder)
					{
						recorder.UpdateRecorder(recorderInfo);
					}
					else
					{
						Recorders.Add(new Recorder(this, recorderInfo));
					}
				}
			}
			RebuildDeviceCollections();
			EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LocalConfigurationChangedIndication, FQID));
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SystemInfo", "RebuildConfiguration", exception);
		}
	}

	public IChange[] QueryChanges(DateTime changesFrom)
	{
		if (LoginServer.ServerType == ServerTypes.CORPORATE)
		{
			return LoginServer.QueryChanges(changesFrom);
		}
		return null;
	}

	public IRecorderInfo QueryRecorderInfo(Guid recorderId)
	{
		return LoginServer.QueryRecorderInfo(recorderId);
	}

	public void UpdateLastHardwareUpdateIfNewer(DateTime newLastModified)
	{
		if (newLastModified > LastHardwareUpdate)
		{
			LastHardwareUpdate = newLastModified;
		}
	}

	public void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
		LoginServer.RegisterIntegration(instanceId, integrationId, integrationName, version, manufacturerName);
	}
}

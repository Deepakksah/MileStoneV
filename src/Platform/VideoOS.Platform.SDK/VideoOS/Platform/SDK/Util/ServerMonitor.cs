using System;
using System.Net;
using System.Security;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Util;

public class ServerMonitor : IDisposable
{
	private bool? _isConnected;

	private Thread _thread;

	private bool _stop;

	private UserContext _userContext;

	private object _syncObject = new object();

	public const int MinPollInterval = 5;

	public const int DefPollInterval = 15;

	private bool _isDisposed;

	private int _pollInterval = 15;

	public bool IsConnected
	{
		get
		{
			lock (_syncObject)
			{
				return _isConnected.HasValue && _isConnected.Value;
			}
		}
		public set
		{
			lock (_syncObject)
			{
				if (!_isConnected.HasValue || _isConnected.Value != value)
				{
					_isConnected = value;
					FireConnectionStateChanged((!value) ? ConnectStates.NotConnected : ConnectStates.Connected);
				}
			}
		}
	}

	public int PollInterval
	{
		get
		{
			return _pollInterval;
		}
		set
		{
			_pollInterval = Math.Max(value, 5);
		}
	}

	public Uri Uri { get; private set; }

	public Exception LastException
	{
		get
		{
			if (!IsConnected)
			{
				return ((SDKConfiguration)_userContext.Configuration).LastException(Uri);
			}
			return null;
		}
	}

	public bool Running => _thread != null;

	public ServerId ServerId => _userContext.Configuration.ServerFQID?.ServerId;

	public event ConnectionStateChangedEventHandler ConnectionStateChangedEvent;

	public ServerMonitor(Uri uri)
	{
		_userContext = new UserContext();
		_userContext.Configuration = new SDKConfiguration(_userContext);
		Uri = uri;
		if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.LoginNetworkCredential != null && EnvironmentManager.Instance.ApplicationLoggedOn)
		{
			NetworkCredential credential = EnvironmentManager.Instance.LoginNetworkCredential.GetCredential(Uri, EnvironmentManager.Instance.LoginAuthType);
			if (credential != null)
			{
				_userContext.CredentialCache.Add(Uri, EnvironmentManager.Instance.LoginAuthType, credential);
				LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(uri.DnsSafeHost);
				if (loginSettings != null && CredentialUtil.AreNetworkCredentialsEqual(loginSettings.NetworkCredential, credential))
				{
					_userContext.AddLoginSettings(loginSettings);
					((SDKConfiguration)_userContext.Configuration).AddConfiguration(_userContext.LoginSettings, masterOnly: true);
					return;
				}
			}
		}
		throw new InvalidCredentialsMIPException(uri.DnsSafeHost);
	}

	[Obsolete("This constructor is deprecated, please use ServerMonitor(bool, Uri, string, string, bool) instead.")]
	public ServerMonitor(Uri uri, string username, string password, bool adUser)
		: this(secureOnly: false, uri, username, password.ToSecureString(), adUser)
	{
	}

	public ServerMonitor(bool secureOnly, Uri uri, string username, string password, bool adUser)
		: this(secureOnly, uri, username, password.ToSecureString(), adUser)
	{
	}

	[Obsolete("This constructor is deprecated, please user ServerMonitor(bool, Uri, string, SecureString, bool) instead.")]
	public ServerMonitor(Uri uri, string username, SecureString password, bool adUser)
		: this(secureOnly: false, uri, username, password, adUser)
	{
	}

	public ServerMonitor(bool secureOnly, Uri uri, string username, SecureString password, bool adUser)
	{
		_userContext = new UserContext();
		_userContext.Configuration = new SDKConfiguration(_userContext);
		Uri = uri;
		if (adUser)
		{
			NetworkCredential cred = VideoOS.Platform.Login.Util.BuildNetworkCredential(username, password, "Negotiate");
			_userContext.CredentialCache.Add(Uri, "Negotiate", cred);
		}
		else
		{
			NetworkCredential cred2 = VideoOS.Platform.Login.Util.BuildNetworkCredential(username, password, "Basic");
			_userContext.CredentialCache.Add(Uri, "Basic", cred2);
		}
		LoginSettings loginSettings = new LoginSettings(Uri, _userContext.CredentialCache, Guid.Empty, Uri.DnsSafeHost, secureOnly);
		_userContext.AddLoginSettings(loginSettings);
		((SDKConfiguration)_userContext.Configuration).AddConfiguration(_userContext.LoginSettings, masterOnly: true);
	}

	public void Init()
	{
		try
		{
			if (_thread == null)
			{
				_stop = false;
				_thread = new Thread(Run);
				_thread.Name = "ServerMonitor:" + Uri;
				_thread.Start();
			}
		}
		catch (Exception innerException)
		{
			throw new MIPException("Unable to connect to EventServer.CommunicationService", innerException);
		}
	}

	public void Close()
	{
		Thread thread = null;
		lock (_syncObject)
		{
			if (_thread != null)
			{
				thread = _thread;
				_stop = true;
			}
		}
		thread?.Join(TimeSpan.FromSeconds(2.0));
		_thread = null;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			lock (_syncObject)
			{
				_stop = true;
			}
			_isDisposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void FireConnectionStateChanged(ConnectStates connectionState)
	{
		try
		{
			if (this.ConnectionStateChangedEvent != null)
			{
				this.ConnectionStateChangedEvent(this, new ConnectionStateChangedEventArgs(connectionState));
			}
		}
		catch (Exception)
		{
		}
	}

	private void Run()
	{
		try
		{
			EnvironmentManager.Instance.Log(error: false, "ServerMonitor background thread", "Now starting...", null);
			_isConnected = null;
			while (!_stop)
			{
				try
				{
					IsConnected = ((ISDKConfiguration)_userContext.Configuration).CheckServerConnection(Uri);
				}
				catch (Exception)
				{
					IsConnected = false;
				}
				for (int i = 0; i < _pollInterval; i++)
				{
					if (_stop)
					{
						break;
					}
					Thread.Sleep(1000);
				}
			}
			((ISDKConfiguration)_userContext.Configuration).LogoutAllServers();
			((ISDKConfiguration)_userContext.Configuration).RemoveAllServers();
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: false, "ServerMonitor background thread", "Exception:" + ex2.Message, null);
		}
		lock (_syncObject)
		{
			_thread = null;
		}
	}
}

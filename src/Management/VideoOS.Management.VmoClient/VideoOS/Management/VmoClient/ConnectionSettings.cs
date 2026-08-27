using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public class ConnectionSettings
{
	private string _serverName;

	private int _serverPort;

	private bool _connectAsUser;

	private string _connectAsUserName;

	private SecureString _connectAsUserPassword;

	private bool _isWindowsUser;

	private NetworkProtocol _networkProtocol;

	private bool _blockUpdates;

	private ITokenCache _tokenCache;

	private TimeSpan _timeout;

	public string ServerName
	{
		get
		{
			return _serverName;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			ThrowIfInvalidValue(value, "ServerName", allowEmpty: true);
			_serverName = value;
		}
	}

	public int ServerPort
	{
		get
		{
			return _serverPort;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			ThrowIfInvalidValue(value, 1, 65535, "ServerPort");
			_serverPort = value;
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			_timeout = value;
		}
	}

	public Uri ServerUri
	{
		get
		{
			if (TokenCache != null)
			{
				return new UriBuilder((NetworkProtocol == NetworkProtocol.Http) ? Uri.UriSchemeHttp : Uri.UriSchemeHttps, ServerName, ServerPort).Uri;
			}
			if (IsWindowsLogin)
			{
				return new UriBuilder(Uri.UriSchemeHttp, ServerName, ServerPort).Uri;
			}
			return new UriBuilder(Uri.UriSchemeHttps, ServerName, ServerPort).Uri;
		}
	}

	public bool IsWindowsLogin => _isWindowsUser;

	public ITokenCache TokenCache
	{
		get
		{
			return _tokenCache;
		}
		public set
		{
			_tokenCache = value;
			_isWindowsUser = false;
		}
	}

	public bool ConnectAsUser
	{
		get
		{
			return _connectAsUser;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			_connectAsUser = value;
		}
	}

	public string ConnectAsUserName
	{
		get
		{
			return _connectAsUserName;
		}
		public set
		{
			ThrowIfUpdatesAreBlocked();
			ThrowIfInvalidValue(value, "ConnectAsUserName");
			_connectAsUserName = value;
			string[] array = _connectAsUserName.Split('\\');
			if (array.Length == 2 && string.Equals(array[0], "[BASIC]", StringComparison.OrdinalIgnoreCase))
			{
				_isWindowsUser = false;
				NetworkProtocol = NetworkProtocol.Https;
			}
		}
	}

	public string ConnectAsUserPassword
	{
		get
		{
			string result = string.Empty;
			if (_connectAsUserPassword != null)
			{
				result = EncryptionUtil.DecryptSecureString(_connectAsUserPassword);
			}
			return result;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			ThrowIfInvalidValue(value, "ConnectAsUserPassword", allowEmpty: true);
			if (value != null && value.Length > 0)
			{
				_connectAsUserPassword = EncryptionUtil.EncryptString(value);
			}
			else
			{
				_connectAsUserPassword = null;
			}
		}
	}

	public NetworkProtocol NetworkProtocol
	{
		get
		{
			return _networkProtocol;
		}
		set
		{
			ThrowIfUpdatesAreBlocked();
			_networkProtocol = value;
		}
	}

	public bool BlockUpdates
	{
		get
		{
			return _blockUpdates;
		}
		set
		{
			_blockUpdates = value;
		}
	}

	public ConnectionSettings()
	{
		InitializeDefaultSettings();
	}

	public Binding GetBinding()
	{
		if (_tokenCache != null)
		{
			BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
			if (NetworkProtocol == NetworkProtocol.Https)
			{
				basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
			}
			else
			{
				basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
			}
			basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
			basicHttpBinding.MaxBufferPoolSize = 524288L;
			basicHttpBinding.MaxReceivedMessageSize = 2147483647L;
			basicHttpBinding.SendTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout);
			basicHttpBinding.CloseTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout);
			basicHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
			return basicHttpBinding;
		}
		if (_isWindowsUser)
		{
			return new WSHttpBinding
			{
				Security = 
				{
					Message = 
					{
						ClientCredentialType = MessageCredentialType.Windows
					}
				},
				MaxBufferPoolSize = 524288L,
				MaxReceivedMessageSize = 2147483647L,
				SendTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout),
				CloseTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout),
				ReaderQuotas = XmlDictionaryReaderQuotas.Max
			};
		}
		return new BasicHttpBinding
		{
			Security = 
			{
				Mode = BasicHttpSecurityMode.Transport,
				Transport = 
				{
					ClientCredentialType = HttpClientCredentialType.Basic
				}
			},
			SendTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout),
			CloseTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(5.0) : _timeout),
			MaxBufferPoolSize = 524288L,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}

	public NetworkCredential GetNetworkCredential()
	{
		if (TokenCache != null)
		{
			return null;
		}
		NetworkCredential networkCredential;
		if (ConnectAsUser)
		{
			networkCredential = new NetworkCredential();
			string[] array = ConnectAsUserName.Split('\\');
			if (array.Length == 2)
			{
				networkCredential.Domain = array[0];
				networkCredential.UserName = array[1];
			}
			else
			{
				networkCredential.UserName = ConnectAsUserName;
			}
			networkCredential.Password = ConnectAsUserPassword;
		}
		else
		{
			networkCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
		}
		return networkCredential;
	}

	public Uri GetServiceUri(string serviceAddressName, string serviceUriName, string uriType)
	{
		UriBuilder uriBuilder = new UriBuilder();
		if (uriType.Equals("Urn", StringComparison.OrdinalIgnoreCase))
		{
			return new Uri(string.Format(CultureInfo.InvariantCulture, "urn:videoos:management:server:{0}", serviceUriName));
		}
		if (uriType.Equals("Url", StringComparison.OrdinalIgnoreCase))
		{
			if (NetworkProtocol == NetworkProtocol.Http)
			{
				uriBuilder.Scheme = "http";
				uriBuilder.Port = 80;
			}
			else
			{
				uriBuilder.Scheme = "https";
				uriBuilder.Port = 443;
			}
			if (ServerPort != 0)
			{
				uriBuilder.Port = ServerPort;
			}
			else
			{
				ServerPort = uriBuilder.Port;
			}
			uriBuilder.Host = ServerName;
			uriBuilder.Path = "ManagementServer/" + serviceAddressName;
			return uriBuilder.Uri;
		}
		throw new ArgumentException("Invalid uri type specified");
	}

	private void InitializeDefaultSettings()
	{
		_serverName = "localhost";
		_serverPort = 0;
		_connectAsUser = false;
		_connectAsUserName = string.Empty;
		_connectAsUserPassword = null;
		_isWindowsUser = true;
		_networkProtocol = NetworkProtocol.Http;
		_timeout = TimeSpan.FromMinutes(1.0);
	}

	private void ThrowIfUpdatesAreBlocked()
	{
		if (BlockUpdates)
		{
			throw new PropertyReadOnlyException(Resources.ConnectionCannotBeChanged);
		}
	}

	private void ThrowIfInvalidValue(string str, string propertyName)
	{
		ThrowIfInvalidValue(str, propertyName, allowEmpty: false);
	}

	private void ThrowIfInvalidValue(string str, string propertyName, bool allowEmpty)
	{
		if (!IsValidString(str, allowEmpty))
		{
			throw new InvalidPropertyValueException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidPropertyValue, "null", propertyName, Resources.InvalidPropertyValueReasonString));
		}
	}

	private void ThrowIfInvalidValue(int n, int minValue, string propertyName)
	{
		ThrowIfInvalidValue(n, minValue, int.MaxValue, propertyName);
	}

	private void ThrowIfInvalidValue(int n, int minValue, int maxValue, string propertyName)
	{
		if (n < minValue)
		{
			throw new InvalidPropertyValueException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidPropertyValue, n.ToString(CultureInfo.InvariantCulture), propertyName, string.Format(CultureInfo.InvariantCulture, Resources.InvalidPropertyValueReasonIntBelowMin, minValue)));
		}
		if (n > maxValue)
		{
			throw new InvalidPropertyValueException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidPropertyValue, n.ToString(CultureInfo.InvariantCulture), propertyName, string.Format(CultureInfo.InvariantCulture, Resources.InvalidPropertyValueReasonIntAboveMax, maxValue)));
		}
	}

	private bool IsValidString(string str, bool allowEmpty)
	{
		if (str != null)
		{
			if (!allowEmpty)
			{
				return str.Length > 0;
			}
			return true;
		}
		return false;
	}
}

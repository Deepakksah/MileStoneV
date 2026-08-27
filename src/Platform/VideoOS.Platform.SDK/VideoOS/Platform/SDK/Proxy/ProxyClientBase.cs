using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Proxy;

public abstract class ProxyClientBase
{
	private Binding _binding;

	private LoginSettings _loginSettings;

	private bool _isInitialized;

	private ServerVersion _serverVersion;

	protected LoginSettings LoginSettings
	{
		[DebuggerStepThrough]
		get
		{
			return _loginSettings;
		}
	}

	public bool IsInitialized
	{
		[DebuggerStepThrough]
		get
		{
			return _isInitialized;
		}
		[DebuggerStepThrough]
		set
		{
			_isInitialized = value;
		}
	}

	protected ServerVersion ServerVersion => _serverVersion;

	public abstract ICommunicationObject CommunicationObject { get; }

	public abstract string ServiceName { get; }

	public abstract string ServiceNameOAuth { get; }

	public abstract Uri ServiceAddress { get; }

	private bool IsBasicUser => _loginSettings.IsBasicUser;

	private string CommandServicePath => "/ManagementServer/" + ServiceName;

	private string CommandServiceOAuthPath => "/ManagementServer/" + ServiceNameOAuth;

	public ProxyClientBase(LoginSettings loginSettings, Binding binding)
	{
		if (loginSettings.Uri.Scheme == Uri.UriSchemeHttp)
		{
			EnvironmentManager.Instance.Log(error: false, "ProxyClientBase", "Creating an HTTP client using uri: " + loginSettings.Uri.ToString());
		}
		_loginSettings = loginSettings;
		_binding = binding;
	}

	public abstract void Initialize();

	public void Initialize(ServerVersion serverVersion)
	{
		_serverVersion = serverVersion;
		Initialize();
	}

	protected Binding GetBinding()
	{
		return _binding;
	}

	protected EndpointAddress GetEndpointAddress()
	{
		return new EndpointAddress(ServiceAddress, new SpnEndpointIdentity(SpnFactory.GetSpn(GetServerAddress())));
	}

	protected Uri GetServerAddress()
	{
		Uri uri = _loginSettings.Uri;
		if (LoginSettings.IsOAuthConnection)
		{
			return new UriBuilder(uri.Scheme, uri.DnsSafeHost, uri.Port, CommandServiceOAuthPath).Uri;
		}
		return UriUtil.ConstructLegacyServiceUri(uri, IsBasicUser, CommandServicePath);
	}

	protected NetworkCredential GetClientCredentials()
	{
		return _loginSettings.NetworkCredential;
	}

	public void ConfigureEndpoint(ServiceEndpoint serviceEndpoint)
	{
		if (LoginSettings.IsOAuthConnection)
		{
			ManagementServerWcfHelper.ConfigureEndpoint(serviceEndpoint, LoginSettings.IdentityTokenCache);
		}
	}

	public void ConfigureCredentials(ClientCredentials clientCredentials)
	{
		if (!_loginSettings.IsOAuthIdentity)
		{
			if (_loginSettings.NetworkCredential.Domain != "[BASIC]" && !_loginSettings.IsBasicUser)
			{
				clientCredentials.Windows.ClientCredential = GetClientCredentials();
				return;
			}
			clientCredentials.UserName.UserName = _loginSettings.NetworkCredential.UserName;
			clientCredentials.UserName.Password = _loginSettings.NetworkCredential.Password;
			clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
		}
	}

	protected static Exception HandleException(Exception ex)
	{
		return ex;
	}

	public void Dispose()
	{
		try
		{
			CommunicationObject.Close();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			CommunicationObject.Abort();
		}
		IsInitialized = false;
	}
}

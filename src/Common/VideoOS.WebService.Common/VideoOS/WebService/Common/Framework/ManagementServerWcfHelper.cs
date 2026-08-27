using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.WebService.Common.Framework;

public class ManagementServerWcfHelper : ManagementServerWcfHelperBase
{
	public static Binding GetBinding(ServerAuthentication serverAuthentication)
	{
		return GetBinding(serverAuthentication, null, TimeSpan.FromMinutes(1.0));
	}

	public static Binding GetBinding(ServerAuthentication serverAuthentication, TimeSpan timeout)
	{
		return GetBinding(serverAuthentication, null, timeout);
	}

	public new static Binding GetBinding(ServerAuthentication serverAuthentication, string namespaceName)
	{
		return GetBinding(serverAuthentication, namespaceName, TimeSpan.FromMinutes(1.0));
	}

	public new static Binding GetBinding(ServerAuthentication serverAuthentication, string namespaceName, TimeSpan timeout)
	{
		Binding binding;
		switch (serverAuthentication)
		{
		case ServerAuthentication.LegacyWindowsCurrentUser:
		case ServerAuthentication.LegacyWindows:
			binding = GetWindowsSecurityBinding(timeout);
			break;
		case ServerAuthentication.LegacyBasic:
			binding = Bindings.GetBasicBinding(transport: true, withCredentials: true, timeout);
			break;
		default:
			binding = ManagementServerWcfHelperBase.GetBinding(serverAuthentication, namespaceName, timeout);
			if (binding == null)
			{
				throw new NotImplementedException();
			}
			break;
		}
		if (namespaceName != null)
		{
			binding.Namespace = namespaceName;
		}
		return binding;
	}

	private static Binding GetWindowsSecurityBinding(TimeSpan timeout)
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding();
		wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
		wSHttpBinding.MaxReceivedMessageSize = 2147483647L;
		wSHttpBinding.MaxBufferPoolSize = 2147483647L;
		wSHttpBinding.SendTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(10.0) : timeout);
		wSHttpBinding.CloseTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(10.0) : timeout);
		wSHttpBinding.BypassProxyOnLocal = false;
		wSHttpBinding.TransactionFlow = false;
		wSHttpBinding.MessageEncoding = WSMessageEncoding.Text;
		wSHttpBinding.TextEncoding = Encoding.UTF8;
		wSHttpBinding.UseDefaultWebProxy = true;
		wSHttpBinding.AllowCookies = false;
		wSHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		return wSHttpBinding;
	}

	public static void ConfigureCredentials(ServerAuthentication serverAuthentication, ClientCredentials clientCredentials, string userName, string password)
	{
		if (clientCredentials == null)
		{
			throw new ArgumentNullException("clientCredentials");
		}
		clientCredentials.SupportInteractive = true;
		if (serverAuthentication == ServerAuthentication.LegacyWindowsCurrentUser)
		{
			clientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			return;
		}
		bool flag = serverAuthentication == ServerAuthentication.LegacyWindows;
		bool flag2 = serverAuthentication == ServerAuthentication.LegacyBasic;
		if (!(flag2 || flag))
		{
			return;
		}
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		if (string.IsNullOrEmpty(userName))
		{
			throw new ArgumentException("userName");
		}
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		if (string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("password");
		}
		if (flag)
		{
			NetworkCredential networkCredential = new NetworkCredential();
			string[] array = userName.Split('\\');
			if (array.Length == 2)
			{
				networkCredential.Domain = array[0];
				networkCredential.UserName = array[1];
			}
			else
			{
				networkCredential.UserName = userName;
			}
			networkCredential.Password = password;
			clientCredentials.Windows.ClientCredential = networkCredential;
		}
		else if (flag2)
		{
			clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
			clientCredentials.UserName.UserName = userName;
			clientCredentials.UserName.Password = password;
		}
	}

	public new static void ConfigureEndpoint(ServerAuthentication serverAuthentication, ServiceEndpoint serviceEndpoint, ITokenCache tokenCache)
	{
		ManagementServerWcfHelperBase.ConfigureEndpoint(serverAuthentication, serviceEndpoint, tokenCache);
	}

	public new static EndpointAddress GetEndpointAddress(ServerAuthentication authenticationType, Uri serverUrl)
	{
		switch (authenticationType)
		{
		case ServerAuthentication.LegacyWindowsCurrentUser:
		case ServerAuthentication.LegacyWindows:
		{
			string spn = SpnFactory.GetSpn(serverUrl);
			return new EndpointAddress(serverUrl, new SpnEndpointIdentity(spn));
		}
		case ServerAuthentication.LegacyBasic:
			return new EndpointAddress(serverUrl.AbsoluteUri);
		default:
			return ManagementServerWcfHelperBase.GetEndpointAddress(authenticationType, serverUrl);
		}
	}
}

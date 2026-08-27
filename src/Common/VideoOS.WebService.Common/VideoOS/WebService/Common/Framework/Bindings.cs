using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace VideoOS.WebService.Common.Framework;

public class Bindings
{
	public static Binding GetBasicBinding(bool transport, bool withCredentials, TimeSpan timeout)
	{
		BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
		if (withCredentials)
		{
			basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
		}
		basicHttpBinding.Security.Mode = (transport ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
		basicHttpBinding.MaxReceivedMessageSize = 2147483647L;
		basicHttpBinding.MaxBufferPoolSize = 2147483647L;
		basicHttpBinding.SendTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(10.0) : timeout);
		basicHttpBinding.CloseTimeout = (Debugger.IsAttached ? TimeSpan.FromMinutes(10.0) : timeout);
		basicHttpBinding.BypassProxyOnLocal = false;
		basicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
		basicHttpBinding.TextEncoding = Encoding.UTF8;
		basicHttpBinding.UseDefaultWebProxy = true;
		basicHttpBinding.AllowCookies = false;
		basicHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		return basicHttpBinding;
	}
}

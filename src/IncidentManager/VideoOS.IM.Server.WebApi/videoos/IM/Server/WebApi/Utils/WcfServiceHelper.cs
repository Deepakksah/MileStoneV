using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using VideoOS.Platform.Util;

namespace VideoOS.IM.Server.WebApi.Utils;

public abstract class WcfServiceHelper
{
	protected static Binding GetOAuthBinding(bool isHttps)
	{
		return new BasicHttpBinding(isHttps ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None)
		{
			CloseTimeout = TimeSpan.FromSeconds(10.0),
			OpenTimeout = TimeSpan.FromSeconds(10.0),
			ReceiveTimeout = TimeSpan.FromSeconds(10.0),
			SendTimeout = TimeSpan.FromSeconds(10.0),
			BypassProxyOnLocal = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			ReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxDepth = 32,
				MaxStringContentLength = int.MaxValue,
				MaxArrayLength = int.MaxValue,
				MaxBytesPerRead = int.MaxValue,
				MaxNameTableCharCount = int.MaxValue
			}
		};
	}

	protected static EndpointAddress GetEndpointAddress(Uri uri)
	{
		return new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(uri)));
	}
}

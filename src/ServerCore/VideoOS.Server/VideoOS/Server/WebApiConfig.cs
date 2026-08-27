using System;
using System.Globalization;
using System.Xml;
using VideoOS.Common.Integration.WebApi;

namespace VideoOS.Server;

public sealed class WebApiConfig : IWebApiConfig
{
	public int Port { get; set; }

	public TimeSpan ServiceDiscoveryTimeout { get; set; }

	public TimeSpan ServiceDiscoveryCheckInterval { get; set; }

	public TimeSpan SleepBetweenRecorderReconnection { get; set; }

	public Uri AuthorizationServerUri { get; set; }

	public bool SecuredServer { get; set; }

	public string Scheme
	{
		get
		{
			if (!SecuredServer)
			{
				return Uri.UriSchemeHttp;
			}
			return Uri.UriSchemeHttps;
		}
	}

	public TimeSpan SleepBetweenGetCompleteConfigurationReconnection { get; set; }

	public WebApiConfig()
	{
		Port = 0;
		ServiceDiscoveryTimeout = TimeSpan.FromSeconds(10.0);
		ServiceDiscoveryCheckInterval = TimeSpan.FromSeconds(2.0);
		SleepBetweenRecorderReconnection = TimeSpan.FromSeconds(5.0);
		SleepBetweenGetCompleteConfigurationReconnection = TimeSpan.FromSeconds(5.0);
	}

	public void Initialize(XmlElement WebApiConfigNode)
	{
		if (WebApiConfigNode == null)
		{
			throw new ServerConfigInvalidException("Web Api configuration missing.");
		}
		XmlNode xmlNode = WebApiConfigNode["Port"];
		if (xmlNode != null)
		{
			Port = Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
			XmlNode xmlNode2 = WebApiConfigNode["ServiceDiscoveryTimeout"];
			if (xmlNode2 != null)
			{
				ServiceDiscoveryTimeout = TimeSpan.FromSeconds(Convert.ToInt32(xmlNode2.InnerText, CultureInfo.InvariantCulture));
				XmlNode xmlNode3 = WebApiConfigNode["ServiceDiscoveryCheckInterval"];
				if (xmlNode3 != null)
				{
					ServiceDiscoveryCheckInterval = TimeSpan.FromSeconds(Convert.ToInt32(xmlNode3.InnerText, CultureInfo.InvariantCulture));
					XmlNode xmlNode4 = WebApiConfigNode["SleepBetweenRecorderReconnection"];
					if (xmlNode4 != null)
					{
						SleepBetweenRecorderReconnection = TimeSpan.FromSeconds(Convert.ToInt32(xmlNode4.InnerText, CultureInfo.InvariantCulture));
						XmlNode xmlNode5 = WebApiConfigNode["SleepBetweenGetCompleteConfigurationReconnection"];
						if (xmlNode5 != null)
						{
							SleepBetweenGetCompleteConfigurationReconnection = TimeSpan.FromSeconds(Convert.ToInt32(xmlNode5.InnerText, CultureInfo.InvariantCulture));
							XmlNode xmlNode6 = WebApiConfigNode["AuthorizationServerUri"];
							if (xmlNode6 != null)
							{
								AuthorizationServerUri = new Uri(xmlNode6.InnerText);
								SecuredServer = SslConfigReader.IsHttpReservationSecure(Port);
								return;
							}
							throw new ServerConfigInvalidException("Authorization Server Uri missing.");
						}
						throw new ServerConfigInvalidException("Service discovery check interval missing.");
					}
					throw new ServerConfigInvalidException("Service discovery check interval missing.");
				}
				throw new ServerConfigInvalidException("Service discovery check interval missing.");
			}
			throw new ServerConfigInvalidException("Service discovery timeout missing.");
		}
		throw new ServerConfigInvalidException("Web Api port missing.");
	}
}

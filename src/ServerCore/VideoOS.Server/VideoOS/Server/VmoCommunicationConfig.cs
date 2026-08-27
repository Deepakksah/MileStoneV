using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Server;

public sealed class VmoCommunicationConfig : IVmoCommunicationConfig
{
	private bool portOverwritten;

	public string ServerName { get; private set; }

	public int Port { get; private set; }

	public int HttpsPort { get; private set; }

	public VmoCommunicationConfig()
	{
		ServerName = "localhost";
		Port = 80;
		HttpsPort = 443;
	}

	public void OverwritePort(int port)
	{
		Port = port;
		portOverwritten = true;
	}

	public void Initialize(XmlElement VMOCommunicationNode)
	{
		if (VMOCommunicationNode == null)
		{
			throw new ServerConfigInvalidException("VMO communication configuration missing.");
		}
		XmlNode xmlNode = VMOCommunicationNode["ServerName"];
		if (ServerName != null)
		{
			ServerName = xmlNode.InnerText;
			if (!portOverwritten)
			{
				XmlNode xmlNode2 = VMOCommunicationNode["Port"];
				if (xmlNode2 == null)
				{
					throw new ServerConfigInvalidException("VMO communication port missing.");
				}
				Port = Convert.ToInt32(xmlNode2.InnerText, CultureInfo.InvariantCulture);
				XmlNode xmlNode3 = VMOCommunicationNode["HttpsPort"];
				if (xmlNode3 == null)
				{
					throw new ServerConfigInvalidException("VMO communication https port missing.");
				}
				HttpsPort = Convert.ToInt32(xmlNode3.InnerText, CultureInfo.InvariantCulture);
			}
			return;
		}
		throw new ServerConfigInvalidException("VMO communication server name missing.");
	}
}

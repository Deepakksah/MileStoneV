using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace VideoOS.Server;

public sealed class RecorderCommunicationConfig : IRecorderCommunicationConfig
{
	public int MaxSimultaneousConfigRequests { get; private set; }

	public int ConnectWaitMin { get; private set; }

	public int ConnectWaitMax { get; private set; }

	public int ConnectionAliveTimeout { get; private set; }

	public RecorderCommunicationConfig()
	{
		MaxSimultaneousConfigRequests = 1;
		ConnectionAliveTimeout = 3000;
		ConnectWaitMin = 5000;
		ConnectWaitMax = 10000;
	}

	public void Initialize(XmlElement recorderCommunicationElement)
	{
		if (recorderCommunicationElement == null)
		{
			throw new ServerConfigInvalidException("Recorder communication missing.");
		}
		XmlElement xmlElement = recorderCommunicationElement["MaxSimultaniousConfigRequests"];
		if (xmlElement != null)
		{
			MaxSimultaneousConfigRequests = Convert.ToInt32(xmlElement.InnerText, CultureInfo.InvariantCulture);
		}
		InitializeConnectWait(recorderCommunicationElement);
		InitializeChannelSettings(recorderCommunicationElement);
	}

	private void InitializeConnectWait(XmlElement recorderCommunicationElement)
	{
		XmlElement xmlElement = recorderCommunicationElement["ConnectWait"];
		if (xmlElement != null)
		{
			XmlNode namedItem = xmlElement.Attributes.GetNamedItem("MinMS");
			if (namedItem != null && !string.IsNullOrEmpty(namedItem.Value))
			{
				ConnectWaitMin = Convert.ToInt32(namedItem.Value, CultureInfo.InvariantCulture);
			}
			XmlNode namedItem2 = xmlElement.Attributes.GetNamedItem("MaxMS");
			if (namedItem2 != null && !string.IsNullOrEmpty(namedItem2.Value))
			{
				ConnectWaitMax = Convert.ToInt32(namedItem2.Value, CultureInfo.InvariantCulture);
			}
		}
	}

	private void InitializeChannelSettings(XmlElement recorderCommunicationElement)
	{
		XmlElement xmlElement = recorderCommunicationElement["ChannelSettings"];
		if (xmlElement == null)
		{
			return;
		}
		foreach (XmlElement item in from XmlNode n in xmlElement.ChildNodes
			where n.NodeType == XmlNodeType.Element
			select n)
		{
			if (item.Name == "ConnectionAliveTimeOut")
			{
				ConnectionAliveTimeout = Convert.ToInt32(item.Value, CultureInfo.InvariantCulture);
			}
		}
	}
}

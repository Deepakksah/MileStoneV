using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("HostScanParameters")]
public class HostedScanParameters
{
	[XmlElement("Server")]
	public ServerCollection Servers { get; set; }

	public HostedScanParameters()
	{
		Servers = new ServerCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public string ToXmlRemovePasswords()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ToXml());
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("HostScanParameters/Server/Password");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				item.InnerText = string.Empty;
			}
		}
		xmlNodeList = xmlDocument.SelectNodes("HostScanParameters/Server/HardwareCollection/Hardware/@Password");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item2 in xmlNodeList)
			{
				item2.InnerText = string.Empty;
			}
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlDocument.WriteTo(xmlTextWriter);
		return stringWriter.ToString();
	}

	public static HostedScanParameters FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<HostedScanParameters>(xml);
	}
}

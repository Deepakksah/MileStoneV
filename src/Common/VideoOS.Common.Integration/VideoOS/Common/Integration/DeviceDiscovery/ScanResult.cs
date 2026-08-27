using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("ScanResult")]
public class ScanResult
{
	[XmlElement("Completed")]
	public bool Completed { get; set; }

	[XmlElement("Completion")]
	public int Progress { get; set; }

	[XmlElement("Phase1Completion")]
	public int Phase1Progress { get; set; }

	[XmlElement("Phase2Completion")]
	public int Phase2Progress { get; set; }

	[XmlElement("Phase3Completion")]
	public int Phase3Progress { get; set; }

	[XmlElement("Phase")]
	public ushort CurrentPhase { get; set; }

	[XmlElement("LastIpAddress")]
	public string LastAddress { get; set; }

	[XmlElement("LastPort")]
	public ushort LastPort { get; set; }

	[XmlElement("Discovery")]
	public DiscoveryCollection DiscoveredHardware { get; set; }

	[XmlElement("UnknownActiveAddress")]
	public Collection<string> UnknownActiveAddresses { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public string ToXmlRemovePasswords()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ToXml());
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("ScanResult/Discovery/Password");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				item.InnerText = string.Empty;
			}
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlDocument.WriteTo(xmlTextWriter);
		return stringWriter.ToString();
	}

	public static ScanResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<ScanResult>(xml);
	}
}

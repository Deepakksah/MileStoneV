using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("ScanParameters")]
public class ScanParameters
{
	[XmlElement("IpAddressInterval")]
	public AddressIntervalCollection AddressIntervals { get; set; }

	[XmlElement("Exclude")]
	public ExcludeCollection Exclude { get; set; }

	[XmlElement("Credential")]
	public CredentialCollection Credentials { get; set; }

	[XmlElement("TryDefaultCredentials")]
	[DefaultValue(true)]
	public bool TryDefaultCredentials { get; set; }

	[XmlElement("TimeOut")]
	[DefaultValue(4)]
	public int TimeoutSeconds { get; set; }

	[XmlElement("RunPhase1")]
	public bool RunPhase1 { get; set; }

	[XmlElement("RunPhase2")]
	public bool RunPhase2 { get; set; }

	[XmlElement("RunPhase3")]
	public bool RunPhase3 { get; set; }

	[XmlElement("PingResponseMandatory")]
	[DefaultValue(true)]
	public bool PingResponseMandatory { get; set; }

	[XmlElement("AllDriversFullIPScan")]
	[DefaultValue(false)]
	public bool AllDriversFullIPScan { get; set; }

	[XmlElement("DefaultPriority")]
	public int DefaultPriority { get; set; }

	[XmlElement("ScanPriority")]
	public ScanPriorityCollection ScanPriorities { get; set; }

	[XmlElement("ReturnUnknownActiveAddresses")]
	[DefaultValue(false)]
	public bool ReturnUnknownActiveAddresses { get; set; }

	[XmlElement("IPv6")]
	public bool IncludeIPv6 { get; set; }

	[XmlElement("Schemes")]
	public Schemes Schemes { get; set; } = new Schemes();

	public ScanParameters()
	{
		AddressIntervals = new AddressIntervalCollection();
		Exclude = new ExcludeCollection();
		Credentials = new CredentialCollection();
		TryDefaultCredentials = true;
		TimeoutSeconds = 4;
		RunPhase1 = false;
		RunPhase2 = false;
		RunPhase3 = false;
		PingResponseMandatory = true;
		AllDriversFullIPScan = false;
		DefaultPriority = 0;
		ScanPriorities = new ScanPriorityCollection();
		IncludeIPv6 = true;
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public string ToXmlRemovePasswords()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ToXml());
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("ScanParameters/Credential/Password");
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

	public static ScanParameters FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<ScanParameters>(xml);
	}
}

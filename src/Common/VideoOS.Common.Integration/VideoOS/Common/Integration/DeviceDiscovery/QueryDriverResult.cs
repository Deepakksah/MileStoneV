using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Result")]
public class QueryDriverResult
{
	[XmlElement("Completed")]
	public bool Completed { get; set; }

	[XmlElement("Drivers")]
	public InstalledDrivers Drivers { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static QueryDriverResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<QueryDriverResult>(xml);
	}
}

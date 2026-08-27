using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("ScanPriorities")]
public class ScanPriorities
{
	[XmlElement("ScanPriority")]
	public ScanPriorityCollection Items { get; set; }

	public ScanPriorities()
	{
		Items = new ScanPriorityCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static ScanPriorities FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<ScanPriorities>(xml);
	}

	public ScanPriorities Clone()
	{
		return FromXml(ToXml());
	}
}

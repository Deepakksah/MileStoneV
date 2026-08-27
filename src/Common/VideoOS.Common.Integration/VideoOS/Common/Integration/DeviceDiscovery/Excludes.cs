using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Excludes")]
public class Excludes
{
	[XmlElement("Exclude")]
	public ExcludeCollection Items { get; set; }

	public Excludes()
	{
		Items = new ExcludeCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static Excludes FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<Excludes>(xml);
	}

	public Excludes Clone()
	{
		return FromXml(ToXml());
	}
}

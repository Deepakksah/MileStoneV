using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Schemes")]
public class Schemes
{
	[XmlElement("Scheme")]
	public SchemeCollection Items { get; set; }

	public Schemes()
	{
		Items = new SchemeCollection();
	}

	public static Schemes FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<Schemes>(xml);
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}
}

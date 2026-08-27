using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Scheme")]
public class Scheme
{
	[XmlElement("SchemeParameter")]
	public SchemeParameterCollection Parameters { get; set; }

	[XmlElement("SchemeName")]
	public string Name { get; set; }

	[XmlElement("DriverNumber")]
	public int DriverNumber { get; set; }

	[XmlElement("DriverName")]
	public string DriverName { get; set; }

	public Scheme()
	{
		Parameters = new SchemeParameterCollection();
	}

	public static Scheme FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<Scheme>(xml);
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}
}

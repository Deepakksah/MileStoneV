using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Drivers")]
public class InstalledDrivers
{
	[XmlElement("Driver")]
	public InstalledDriverCollection Items { get; set; }

	public InstalledDrivers()
	{
		Items = new InstalledDriverCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static InstalledDrivers FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<InstalledDrivers>(xml);
	}
}

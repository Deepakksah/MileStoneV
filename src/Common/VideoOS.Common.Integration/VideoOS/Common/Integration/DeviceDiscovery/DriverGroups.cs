using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("DriverGroups")]
public class DriverGroups
{
	[XmlElement("DriverGroup")]
	public DriverGroup[] Items { get; set; }

	public DriverGroups()
	{
		Items = new DriverGroup[0];
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static DriverGroups FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<DriverGroups>(xml);
	}

	public static DriverGroups LoadXml(string fileName)
	{
		return XmlSerializationHelper.LoadXml<DriverGroups>(fileName);
	}

	public static void SaveXml(DriverGroups driverGroups, string fileName)
	{
		XmlSerializationHelper.SaveXml(driverGroups, fileName);
	}
}

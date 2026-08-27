using System;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DeviceGroupSelection
{
	[XmlElement("DeviceType")]
	public string DeviceType { get; set; }

	[XmlElement("Name")]
	public string Name { get; set; }

	[XmlElement("IDDeviceGroup")]
	public Guid IDDeviceGroup { get; set; }

	public DeviceGroupSelection()
	{
	}

	public DeviceGroupSelection(string deviceType, string name, Guid id)
	{
		DeviceType = deviceType;
		Name = name;
		IDDeviceGroup = id;
	}
}

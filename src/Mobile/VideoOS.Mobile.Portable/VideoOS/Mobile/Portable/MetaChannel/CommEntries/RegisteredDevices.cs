using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class RegisteredDevices
{
	public const string RegisteredDevicesName = "RegisteredDevices";

	[JsonProperty("RegisteredDevices")]
	[XmlElement(ElementName = "RegisteredDevices")]
	public List<RegisteredDevice> RegisteredDevicesList { get; set; }

	public RegisteredDevices()
	{
		InternalInit();
	}

	public RegisteredDevices(List<RegisteredDevice> devices)
	{
		RegisteredDevicesList = devices;
	}

	private void InternalInit()
	{
		RegisteredDevicesList = new List<RegisteredDevice>();
	}
}

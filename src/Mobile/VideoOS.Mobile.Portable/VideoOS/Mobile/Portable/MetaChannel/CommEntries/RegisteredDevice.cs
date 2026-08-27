using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class RegisteredDevice
{
	[JsonProperty("DeviceId")]
	[XmlAttribute(AttributeName = "DeviceId")]
	public Guid DeviceId { get; set; }

	[JsonProperty("DeviceName")]
	[XmlAttribute(AttributeName = "DeviceName")]
	public string DeviceName { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool DeviceEnabled
	{
		get
		{
			return DeviceState == DeviceState.Enabled;
		}
		set
		{
			DeviceState = (value ? DeviceState.Enabled : DeviceState.Disabled);
		}
	}

	[JsonProperty("Username")]
	[XmlAttribute(AttributeName = "Username")]
	public string Username { get; set; }

	[JsonProperty("DeviceState")]
	[XmlAttribute(AttributeName = "DeviceState")]
	public DeviceState DeviceState { get; set; }

	public RegisteredDevice()
	{
		InternalInit();
	}

	private void InternalInit()
	{
		DeviceId = Guid.Empty;
		DeviceName = "";
		Username = "";
		DeviceState = DeviceState.Disabled;
	}
}

using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class UpdateHardwareTaskDetailResult
{
	[XmlElement("DeviceId", DataType = "string")]
	public string InternalDeviceId { get; set; }

	[XmlIgnore]
	public Guid DeviceId
	{
		get
		{
			return new Guid(InternalDeviceId);
		}
		set
		{
			InternalDeviceId = value.ToString();
		}
	}

	[XmlElement("DeviceName", DataType = "string")]
	public string DeviceName { get; set; }

	[XmlElement("Change", DataType = "string")]
	public string Change { get; set; }

	[XmlElement("DeviceType", DataType = "string")]
	public string DeviceType { get; set; }

	public UpdateHardwareTaskDetailResult()
	{
	}

	public UpdateHardwareTaskDetailResult(Guid deviceId, string deviceName, string change, string deviceType)
	{
		DeviceId = deviceId;
		DeviceName = deviceName;
		Change = change;
		DeviceType = deviceType;
	}
}

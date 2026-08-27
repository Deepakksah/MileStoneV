using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Device")]
public class ReplaceHardwareTaskDevice
{
	[XmlAttribute("Id", DataType = "string")]
	public string InternalId { get; set; }

	[XmlIgnore]
	public Guid Id
	{
		get
		{
			return new Guid(InternalId);
		}
		set
		{
			InternalId = value.ToString();
		}
	}

	[XmlAttribute("Name", DataType = "string")]
	public string Name { get; set; }

	[XmlAttribute("DeviceType", DataType = "string")]
	public string DeviceType { get; set; }

	[XmlAttribute("SettingsChanged", DataType = "boolean")]
	public bool SettingsChanged { get; set; }

	[XmlArray("DeletedEvents")]
	[XmlArrayItem("Event")]
	public ReplaceHardwareTaskEventCollection DeletedEvents { get; set; }

	[XmlArray("DeletedStreams")]
	[XmlArrayItem("Stream")]
	public ReplaceHardwareTaskStreamCollection DeletedStreams { get; set; }

	[XmlArray("DeletedPtzPresets")]
	[XmlArrayItem("PtzPresets")]
	public ReplaceHardwareTaskPtzPresetCollection DeletedPtzPresets { get; set; }

	[XmlArray("DeletedPatrollingProfiles")]
	[XmlArrayItem("PatrollingProfile")]
	public ReplaceHardwareTaskPatrollingProfileCollection DeletedPatrollingProfiles { get; set; }

	public ReplaceHardwareTaskDevice()
	{
	}

	public ReplaceHardwareTaskDevice(Guid id, string deviceType, string name)
	{
		Id = id;
		Name = name;
		DeviceType = deviceType;
	}
}

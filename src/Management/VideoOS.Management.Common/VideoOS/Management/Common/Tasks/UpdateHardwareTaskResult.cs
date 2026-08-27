using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class UpdateHardwareTaskResult
{
	[XmlElement("HardwareId", DataType = "string")]
	public string InternalHardwareId { get; set; }

	[XmlIgnore]
	public Guid HardwareId
	{
		get
		{
			return new Guid(InternalHardwareId);
		}
		set
		{
			InternalHardwareId = value.ToString();
		}
	}

	[XmlElement("HardwareName", DataType = "string")]
	public string InternalHardwareName { get; set; }

	[XmlIgnore]
	public string HardwareName
	{
		get
		{
			return InternalHardwareName;
		}
		set
		{
			InternalHardwareName = value;
		}
	}

	[XmlElement("RecordingServerId", DataType = "string")]
	public string InternalRecordingServerId { get; set; }

	[XmlIgnore]
	public Guid RecordingServerId
	{
		get
		{
			return new Guid(InternalRecordingServerId);
		}
		set
		{
			InternalRecordingServerId = value.ToString();
		}
	}

	[XmlElement("HardwareEventsChanged", DataType = "boolean")]
	public bool HardwareEventsChanged { get; set; }

	[XmlArray("TaskDetails")]
	public UpdateHardwareTaskDetailResult[] TaskDetails { get; set; }

	[XmlElement("SessionDataId", DataType = "long")]
	public long SessionDataId { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static UpdateHardwareTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<UpdateHardwareTaskResult>(xml);
	}
}

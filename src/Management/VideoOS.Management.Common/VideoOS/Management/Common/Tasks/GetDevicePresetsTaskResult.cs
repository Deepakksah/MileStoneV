using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class GetDevicePresetsTaskResult
{
	[XmlElement("DeviceId", DataType = "string")]
	public string DeviceId { get; set; }

	[XmlElement("PresetXml", DataType = "string")]
	public string PresetXml { get; set; }

	[XmlElement("AffectedProfileIds", DataType = "string")]
	public string AffectedProfileIds { get; set; }

	[XmlElement("SessionDataId", DataType = "long")]
	public long SessionDataId { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static GetDevicePresetsTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<GetDevicePresetsTaskResult>(xml);
	}
}

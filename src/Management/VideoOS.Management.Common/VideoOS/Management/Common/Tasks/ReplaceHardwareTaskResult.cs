using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class ReplaceHardwareTaskResult
{
	[XmlElement("Hardware")]
	public ReplaceHardwareTaskDevice Hardware { get; set; }

	[XmlArray("AddedDevices")]
	[XmlArrayItem("Device")]
	public ReplaceHardwareTaskDeviceCollection AddedDevices { get; set; } = new ReplaceHardwareTaskDeviceCollection();

	[XmlArray("RemovedDevices")]
	[XmlArrayItem("Device")]
	public ReplaceHardwareTaskDeviceCollection DeletedDevices { get; set; } = new ReplaceHardwareTaskDeviceCollection();

	[XmlArray("MappedDevices")]
	[XmlArrayItem("Device")]
	public ReplaceHardwareTaskDeviceCollection MappedDevices { get; set; } = new ReplaceHardwareTaskDeviceCollection();

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static ReplaceHardwareTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<ReplaceHardwareTaskResult>(xml);
	}
}

using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class MoveHardwareTaskResult
{
	[XmlArray("Warnings")]
	[XmlArrayItem("Warning")]
	public TaskWarningCollection Warnings { get; set; }

	public MoveHardwareTaskResult()
	{
		Warnings = new TaskWarningCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static MoveHardwareTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<MoveHardwareTaskResult>(xml);
	}
}

using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("TaskError")]
public class TaskError
{
	[XmlElement("ErrorText", DataType = "string")]
	public string ErrorText { get; set; }

	[XmlElement("ErrorCode", DataType = "int")]
	public int ErrorCode { get; set; }

	[XmlElement("SubErrorCode", DataType = "int")]
	public int SubErrorCode { get; set; }

	[XmlElement("Details", DataType = "string")]
	public string Details { get; set; }

	[XmlArray("Arguments")]
	[XmlArrayItem("Argument")]
	public ArgumentCollection Arguments { get; set; }

	public TaskError()
	{
		Arguments = new ArgumentCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static TaskError FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<TaskError>(xml);
	}
}

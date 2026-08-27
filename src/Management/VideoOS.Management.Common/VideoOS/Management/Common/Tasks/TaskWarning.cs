using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("TaskWarning")]
[DebuggerDisplay("({Code}) {Message}")]
public class TaskWarning
{
	[XmlElement("Message", DataType = "string")]
	public string Message { get; set; }

	[XmlElement("Code", DataType = "int")]
	public int Code { get; set; }

	[XmlArray("Arguments")]
	[XmlArrayItem("Argument")]
	public ArgumentCollection Arguments { get; set; }

	public TaskWarning()
	{
		Arguments = new ArgumentCollection();
	}

	public TaskWarning(int code, string message)
	{
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		Code = code;
		Message = message;
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static TaskWarning FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<TaskWarning>(xml);
	}
}

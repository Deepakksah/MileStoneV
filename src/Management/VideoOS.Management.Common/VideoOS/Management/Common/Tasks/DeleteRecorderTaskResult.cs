using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class DeleteRecorderTaskResult
{
	[XmlElement("RecorderId", DataType = "string")]
	public string InternalRecorderId { get; set; }

	[XmlElement("RecorderName", DataType = "string")]
	public string RecorderName { get; set; }

	[XmlIgnore]
	public Guid RecorderId
	{
		get
		{
			return new Guid(InternalRecorderId);
		}
		set
		{
			InternalRecorderId = value.ToString();
		}
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static DeleteRecorderTaskResult FromXml(string xml)
	{
		if (xml != null && !string.IsNullOrEmpty(xml))
		{
			return XmlSerializationHelper.FromXml<DeleteRecorderTaskResult>(xml);
		}
		return new DeleteRecorderTaskResult();
	}
}

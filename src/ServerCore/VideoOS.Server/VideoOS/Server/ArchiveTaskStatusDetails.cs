using System;
using System.Xml.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Server;

[XmlRoot("TaskStatus")]
public class ArchiveTaskStatusDetails
{
	[XmlElement("AdjustedStartTime", DataType = "dateTime")]
	public DateTime AdjustedStartTime { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static ArchiveTaskStatusDetails FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<ArchiveTaskStatusDetails>(xml);
	}
}

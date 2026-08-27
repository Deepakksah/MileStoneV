using System.Xml.Serialization;
using VideoOS.LogServer.Communication.LogWriterServiceReference;

namespace VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;

[XmlRoot(ElementName = "entry")]
public class Entry
{
	[XmlAttribute(AttributeName = "object_type")]
	public string ObjectType { get; set; }

	[XmlAttribute(AttributeName = "severity")]
	public string Severity { get; set; }

	[XmlAttribute(AttributeName = "status")]
	public string Status { get; set; }

	[XmlAttribute(AttributeName = "uname")]
	public string UName { get; set; }

	[XmlText]
	public string Text { get; set; }

	public LogType LogType { get; set; }

	public string Category { get; set; }

	public string SourceType { get; set; }
}

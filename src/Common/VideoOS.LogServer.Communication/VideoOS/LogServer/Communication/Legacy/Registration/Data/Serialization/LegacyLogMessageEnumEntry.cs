using System.Xml.Serialization;

namespace VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;

[XmlRoot(ElementName = "enum")]
public class LegacyLogMessageEnumEntry
{
	[XmlAttribute(AttributeName = "uname")]
	public string Id { get; set; }

	[XmlAttribute(AttributeName = "value")]
	public string Value { get; set; }
}

using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;

[XmlRoot(ElementName = "userEnum")]
public class LegacyLogMessageUserEnumEntry
{
	[XmlElement(ElementName = "enum")]
	public Collection<LegacyLogMessageEnumEntry> LegacyLogMessageEnumEntries { get; } = new Collection<LegacyLogMessageEnumEntry>();

	[XmlAttribute(AttributeName = "uname")]
	public string Id { get; set; }
}

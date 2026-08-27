using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;

[XmlRoot(ElementName = "group")]
public class Group
{
	[XmlElement(ElementName = "entry")]
	public Collection<Entry> Entries { get; } = new Collection<Entry>();

	[XmlAttribute(AttributeName = "name")]
	public string Name { get; set; }

	[XmlAttribute(AttributeName = "category")]
	public string Category { get; set; }
}

using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ValueInfo
{
	public const string NameName = "Name";

	public const string ValueName = "Value";

	[JsonProperty("Name")]
	[XmlAttribute("Name")]
	public string Name { get; set; }

	[JsonProperty("Value")]
	[XmlAttribute("Value")]
	public string Value { get; set; }
}

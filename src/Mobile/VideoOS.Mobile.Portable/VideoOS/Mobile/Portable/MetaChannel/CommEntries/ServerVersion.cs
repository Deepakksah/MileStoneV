using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ServerVersion
{
	public const string MajorXmlName = "Major";

	public const string MinorXmlName = "Minor";

	public const string BuildXmlName = "Build";

	public const string RevisionXmlName = "Revision";

	public const string VersionStringXmlName = "VersionString";

	[JsonProperty("Major")]
	[XmlElement(ElementName = "Major")]
	public string Major { get; set; }

	[JsonProperty("Minor")]
	[XmlElement(ElementName = "Minor")]
	public string Minor { get; set; }

	[JsonProperty("Build")]
	[XmlElement(ElementName = "Build")]
	public string Build { get; set; }

	[JsonProperty("Revision")]
	[XmlElement(ElementName = "Revision")]
	public string Revision { get; set; }

	[JsonProperty("VersionString")]
	[XmlElement(ElementName = "VersionString")]
	public string VersionString { get; set; }

	public ServerVersion()
	{
	}

	public ServerVersion(string major, string minor, string build, string revision, string versionString)
	{
		Major = major;
		Minor = minor;
		Build = build;
		Revision = revision;
		VersionString = versionString;
	}
}

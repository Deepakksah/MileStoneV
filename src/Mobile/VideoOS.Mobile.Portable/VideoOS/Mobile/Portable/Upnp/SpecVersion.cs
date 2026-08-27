using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.Upnp;

public class SpecVersion
{
	private const int SpecVersionMajor = 1;

	private const int SpecVersionMinor = 0;

	[XmlElement("major")]
	public int Major { get; set; }

	[XmlElement("minor")]
	public int Minor { get; set; }

	public SpecVersion()
	{
		Major = 1;
		Minor = 0;
	}
}

using System.Xml.Serialization;

namespace VideoOS.Mobile.Users;

public class ProtectedObject
{
	[XmlAttribute("Data")]
	public byte[] Data { get; set; }

	[XmlAttribute("ProtectionType")]
	public ProtectionType ProtectionType { get; set; }
}

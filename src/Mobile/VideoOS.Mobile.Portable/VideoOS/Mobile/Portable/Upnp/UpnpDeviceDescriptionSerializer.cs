using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Upnp;

public class UpnpDeviceDescriptionSerializer : IUpnpDeviceDescriptionSerializer
{
	private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(UpnpDeviceDescription));

	public static UpnpDeviceDescriptionSerializer Instance { get; } = new UpnpDeviceDescriptionSerializer();

	private UpnpDeviceDescriptionSerializer()
	{
	}

	public string SerializeToString(UpnpDeviceDescription deviceDescription)
	{
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add("", "urn:schemas-upnp-org:device-1-0");
		using EncodedStringWriter encodedStringWriter = new EncodedStringWriter();
		_serializer.Serialize(encodedStringWriter, deviceDescription, xmlSerializerNamespaces);
		return encodedStringWriter.ToString();
	}

	public UpnpDeviceDescription Deserialize(string description)
	{
		using StringReader input = new StringReader(description);
		using XmlReader xmlReader = XmlReader.Create(input, SecurityXmlReaderSettings.Settings);
		return (UpnpDeviceDescription)_serializer.Deserialize(xmlReader);
	}
}

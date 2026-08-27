using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class SoapMessageSerializer
{
	private readonly string _schema;

	private const string SoapEnvelopeNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

	private const string SoapMessageFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body>{0}</s:Body></s:Envelope>";

	public SoapMessageSerializer(string schema)
	{
		_schema = schema;
	}

	public string Serialize<T>(T serializableObj)
	{
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add("u", _schema);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		string arg;
		using (StringWriter stringWriter = new StringWriter())
		{
			using CustomXmlTextWriter xmlWriter = new CustomXmlTextWriter(stringWriter);
			xmlSerializer.Serialize(xmlWriter, serializableObj, xmlSerializerNamespaces);
			arg = stringWriter.ToString();
		}
		return $"<?xml version=\"1.0\" encoding=\"utf-8\"?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body>{arg}</s:Body></s:Envelope>";
	}

	public T Desrialize<T>(Stream stream)
	{
		XDocument xDocument = XDocument.Load(stream);
		XNamespace xNamespace = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
		using XmlReader reader = xDocument.Descendants(xNamespace + "Body").First().FirstNode.CreateReader();
		using XmlReader xmlReader = XmlReader.Create(reader, SecurityXmlReaderSettings.Settings);
		return (T)new XmlSerializer(typeof(T), _schema).Deserialize(xmlReader);
	}
}

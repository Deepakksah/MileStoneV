using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class CalendarInfoEnvelope : IXmlSerializable
{
	private const string _nsSoapEnv = "http://schemas.xmlsoap.org/soap/envelope/";

	private const string _nsSoapEnc = "http://schemas.xmlsoap.org/soap/encoding/";

	public CalendarInfoBody Body { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void Serialize(XmlWriter writer)
	{
		writer.WriteStartDocument();
		writer.WriteStartElement("SOAP-ENV", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
		writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
		writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
		writer.WriteAttributeString("xmlns", "SOAP-ENC", null, "http://schemas.xmlsoap.org/soap/encoding/");
		writer.WriteAttributeString("xmlns", "SOAP-ENV", null, "http://schemas.xmlsoap.org/soap/envelope/");
		writer.WriteAttributeString("xmlns", "clr", null, "http://schemas.microsoft.com/soap/encoding/clr/1.0");
		writer.WriteAttributeString("SOAP-ENV", "encodingStyle", null, "http://schemas.xmlsoap.org/soap/encoding/");
		WriteXml(writer);
		writer.WriteEndElement();
		writer.WriteEndDocument();
	}

	public void ReadXml(XmlReader reader)
	{
		reader.ReadStartElement("Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
		reader.ReadStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
		Body = new CalendarInfoBody();
		Body.ReadXml(reader);
		reader.ReadEndElement();
		reader.ReadEndElement();
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("SOAP-ENV", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
		Body?.WriteXml(writer);
		writer.WriteEndElement();
	}
}

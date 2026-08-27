using System.IO;
using System.Xml;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class CustomXmlTextWriter : XmlTextWriter
{
	public CustomXmlTextWriter(StringWriter stream)
		: base(stream)
	{
	}

	public override void WriteStartDocument(bool standalone)
	{
	}

	public override void WriteStartDocument()
	{
	}

	public override void WriteEndElement()
	{
		WriteFullEndElement();
	}
}

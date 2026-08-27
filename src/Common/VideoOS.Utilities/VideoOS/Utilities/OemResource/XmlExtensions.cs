using System.Xml;
using System.Xml.Linq;

namespace VideoOS.Utilities.OemResource;

public static class XmlExtensions
{
	public static XmlDocument ToXmlDocument(this XDocument xDocument)
	{
		XmlDocument xmlDocument = new XmlDocument();
		using XmlReader reader = xDocument.CreateReader();
		xmlDocument.Load(reader);
		return xmlDocument;
	}
}

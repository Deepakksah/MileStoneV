using System.Xml;

namespace VideoOS.GatewayService.Utilities;

public class XmlHelper : IXmlHelper
{
	public XmlDocument Load(string path)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(path);
		return xmlDocument;
	}

	public void Save(XmlDocument document, string path)
	{
		document.Save(path);
	}
}

using System.Xml;

namespace VideoOS.GatewayService.Utilities;

public interface IXmlHelper
{
	XmlDocument Load(string path);

	void Save(XmlDocument document, string path);
}

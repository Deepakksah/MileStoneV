using System;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class HardwareScanExpressEncryption : IXmlTransformation
{
	public void Transform(XDocument doc, Func<string, byte[], string> method)
	{
		XElement xElement = doc.Root?.Element("credentialsListXml");
		if (xElement != null)
		{
			xElement.Value = method(xElement.Value, null);
		}
	}
}

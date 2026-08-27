using System;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class AddHardwareEncryption : IXmlTransformation
{
	public void Transform(XDocument doc, Func<string, byte[], string> method)
	{
		XElement xElement = doc.Root.Element("password");
		if (xElement != null)
		{
			xElement.Value = method(xElement.Value, null);
		}
	}
}

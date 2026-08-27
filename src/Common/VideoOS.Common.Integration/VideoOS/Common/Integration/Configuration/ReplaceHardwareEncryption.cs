using System;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class ReplaceHardwareEncryption : IXmlTransformation
{
	public void Transform(XDocument doc, Func<string, byte[], string> method)
	{
		XElement xElement = doc.Root.Element("password");
		if (xElement != null)
		{
			xElement.Value = method(xElement.Value, null);
		}
		XElement xElement2 = doc.Root.Element("customDriverData");
		if (xElement2 != null)
		{
			xElement2.Value = method(xElement2.Value, null);
		}
	}
}

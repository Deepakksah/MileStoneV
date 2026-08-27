using System;
using System.Xml;

namespace VideoOS.Common.Integration.MediaDatabase.PrivateConfigXmlExtensions;

public static class XmlExtensions
{
	public static XmlElement GetChildElement(this XmlElement xmlElement, string childElementName)
	{
		if (xmlElement == null)
		{
			throw new ArgumentNullException("xmlElement");
		}
		if (childElementName == null)
		{
			throw new ArgumentNullException("childElementName");
		}
		XmlElement xmlElement2 = xmlElement[childElementName];
		if (xmlElement2 == null)
		{
			throw new InvalidConfigurationXmlException(FormattableString.Invariant($"Element '{childElementName}' does not exist as a child element of '{xmlElement.Name}'."));
		}
		return xmlElement2;
	}
}

using System;
using System.Xml;

namespace VideoOS.Server.ConfigExtensions;

public static class XmlElementExtensions
{
	public static bool ReadAttributeAsInt32(this XmlElement element, string attributeName, out int value)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		if (attributeName == null)
		{
			throw new ArgumentNullException("attributeName");
		}
		value = 0;
		if (!element.HasAttribute(attributeName))
		{
			return false;
		}
		return int.TryParse(element.Attributes[attributeName].Value, out value);
	}
}

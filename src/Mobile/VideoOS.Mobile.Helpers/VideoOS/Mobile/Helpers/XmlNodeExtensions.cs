using System;
using System.Xml;

namespace VideoOS.Mobile.Helpers;

public static class XmlNodeExtensions
{
	public static string GetConfigEntry(this XmlNode configNode, string key)
	{
		foreach (XmlNode childNode in configNode.ChildNodes)
		{
			if (!(childNode.Name == "add"))
			{
				continue;
			}
			foreach (XmlAttribute attribute in childNode.Attributes)
			{
				if (attribute.Name == "key")
				{
					if (!attribute.Value.Equals(key, StringComparison.InvariantCultureIgnoreCase))
					{
						break;
					}
				}
				else if (attribute.Name == "value")
				{
					return attribute.Value;
				}
			}
		}
		return string.Empty;
	}

	public static void InsertКеyValue(this XmlNode node, string key, string value)
	{
		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (!(childNode.Name == "add"))
			{
				continue;
			}
			string text = null;
			foreach (XmlAttribute attribute in childNode.Attributes)
			{
				if (attribute.Name == "key")
				{
					text = ((string.Compare(attribute.Value, key, StringComparison.OrdinalIgnoreCase) != 0) ? null : value);
				}
				if (attribute.Name == "value" && text != null)
				{
					attribute.Value = text;
					break;
				}
			}
		}
	}
}

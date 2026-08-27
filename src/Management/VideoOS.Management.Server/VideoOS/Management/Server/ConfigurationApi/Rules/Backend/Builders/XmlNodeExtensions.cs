using System;
using System.Linq;
using System.Xml;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public static class XmlNodeExtensions
{
	public static bool HasAttribute<T>(this XmlNode node, string attributeName, out T attributeValue, bool capitalize = false)
	{
		attributeValue = default(T);
		XmlAttribute xmlAttribute = node.Attributes?[attributeName];
		if (xmlAttribute == null)
		{
			return false;
		}
		try
		{
			string value = xmlAttribute.Value;
			attributeValue = TypeHelper.FromString<T>(capitalize ? UppercaseFirst(value) : value, "Invalid {0}: '{1}'", Array.Empty<string>());
		}
		catch (ArgumentException)
		{
			return false;
		}
		return true;
	}

	public static bool HasAttributeWithValue(this XmlNode node, string attributeName, string expectedValue)
	{
		if (node.HasAttribute<string>(attributeName, out var attributeValue))
		{
			return expectedValue.Equals(attributeValue, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public static bool IsValid(this XmlNode node, string expectedName, bool expectedChild = false)
	{
		if (!string.Equals(node.Name, expectedName))
		{
			return false;
		}
		if (expectedChild && !node.HasChildNodes)
		{
			return false;
		}
		return true;
	}

	public static string[] GenerateArray(this XmlNodeList nodes)
	{
		if (nodes == null) return new string[0];
		return nodes.Cast<XmlNode>().Select(s => s.InnerText).ToArray();
	}

	public static XmlNode GetNodeByTagName(this XmlNode node, string id)
	{
		return node?.SelectSingleNode(".//" + id);
	}

	private static string UppercaseFirst(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		return str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
	}
}

using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.Integration.Extensions.XmlDocumentExtensions;

public static class XmlDocumentExtensionsCore
{
	private delegate bool TryParseMethod<T>(string str, out T result);

	private static readonly TryParseMethod<ushort> UShortParser = delegate(string str, out ushort result)
	{
		return ushort.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	};

	private static readonly TryParseMethod<int> IntParser = delegate(string str, out int result)
	{
		return int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	};

	private static readonly TryParseMethod<uint> UIntParser = delegate(string str, out uint result)
	{
		return uint.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	};

	private static readonly TryParseMethod<ulong> ULongParser = delegate(string str, out ulong result)
	{
		return ulong.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
	};

	private static readonly TryParseMethod<bool> BoolParser = bool.TryParse;

	private static readonly TryParseMethod<string> StringParser = delegate(string str, out string result)
	{
		result = str;
		return true;
	};

	public static bool ReadAttributeAsBoolean(this XmlElement xmlElement, string attributeName)
	{
		return ReadAttributeValue(xmlElement, attributeName, BoolParser);
	}

	public static string ReadAttributeAsString(this XmlElement xmlElement, string attributeName)
	{
		return ReadAttributeValue(xmlElement, attributeName, StringParser);
	}

	public static int ReadAttributeAsInt32(this XmlElement xmlElement, string attributeName)
	{
		return ReadAttributeValue(xmlElement, attributeName, IntParser);
	}

	public static int ReadAttributeAsInt32(this XmlElement xmlElement, string attributeName, int min, int max)
	{
		return ReadAttributeValueWithRangeCheck(xmlElement, attributeName, IntParser, min, max);
	}

	public static ulong ReadChildElementValueAsUInt64(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, ULongParser);
	}

	public static ulong ReadChildElementValueAsUInt64(this XmlElement xmlElement, string childElementName, ulong min, ulong max)
	{
		return ReadChildElementValueWithRangeCheck(xmlElement, childElementName, ULongParser, min, max);
	}

	public static int ReadChildElementValueAsInt32(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, IntParser);
	}

	public static int ReadChildElementValueAsInt32(this XmlElement xmlElement, string childElementName, int min, int max)
	{
		return ReadChildElementValueWithRangeCheck(xmlElement, childElementName, IntParser, min, max);
	}

	public static ushort ReadChildElementValueAsUInt16(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, UShortParser);
	}

	public static uint ReadChildElementValueAsUInt32(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, UIntParser);
	}

	public static uint ReadChildElementValueAsUInt32(this XmlElement xmlElement, string childElementName, uint min, uint max)
	{
		return ReadChildElementValueWithRangeCheck(xmlElement, childElementName, UIntParser, min, max);
	}

	public static ushort ReadChildElementValueAsUInt16(this XmlElement xmlElement, string childElementName, ushort min, ushort max)
	{
		return ReadChildElementValueWithRangeCheck(xmlElement, childElementName, UShortParser, min, max);
	}

	public static bool ReadChildElementValueAsBoolean(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, BoolParser);
	}

	public static string ReadChildElementValueAsString(this XmlElement xmlElement, string childElementName)
	{
		return ReadChildElementValue(xmlElement, childElementName, StringParser);
	}

	private static T ReadAttributeValue<T>(XmlElement xmlElement, string attributeName, TryParseMethod<T> parser)
	{
		if (xmlElement == null)
		{
			throw new ArgumentNullException("xmlElement");
		}
		if (attributeName == null)
		{
			throw new ArgumentNullException("attributeName");
		}
		if (!xmlElement.HasAttribute(attributeName))
		{
			throw new XmlException(FormattableString.Invariant($"The attribute '{attributeName}' is missing."));
		}
		string value = xmlElement.Attributes[attributeName].Value;
		if (!parser(value, out var result))
		{
			throw new XmlException(FormattableString.Invariant($"The attribute '{attributeName}' on element '{xmlElement.Name}' has an invalid value."));
		}
		return result;
	}

	private static T ReadAttributeValueWithRangeCheck<T>(XmlElement xmlElement, string attributeName, TryParseMethod<T> parser, T min, T max) where T : IComparable<T>
	{
		T result = ReadAttributeValue(xmlElement, attributeName, parser);
		if (result.CompareTo(min) < 0 || result.CompareTo(max) > 0)
		{
			throw new XmlException(FormattableString.Invariant($"The value of attribute '{attributeName}' under '{xmlElement.Name}' is out of range."));
		}
		return result;
	}

	private static T ReadChildElementValue<T>(XmlElement xmlElement, string childElementName, TryParseMethod<T> parser)
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
			throw new XmlException(FormattableString.Invariant($"Element '{childElementName}' does not exist as a child element of '{xmlElement.Name}'."));
		}
		if (parser(xmlElement2.InnerText, out var result))
		{
			return result;
		}
		throw new XmlException(FormattableString.Invariant($"The value of element '{childElementName}' under '{xmlElement.Name}' is invalid."));
	}

	private static T ReadChildElementValueWithRangeCheck<T>(XmlElement xmlElement, string childElementName, TryParseMethod<T> parser, T min, T max) where T : struct, IComparable<T>
	{
		T result = ReadChildElementValue(xmlElement, childElementName, parser);
		if (result.CompareTo(min) < 0 || result.CompareTo(max) > 0)
		{
			throw new XmlException(FormattableString.Invariant($"The value of element '{childElementName}' under '{xmlElement.Name}' is out of range."));
		}
		return result;
	}
}

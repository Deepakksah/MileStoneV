using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VideoOS.Utilities;

public static class Character
{
	private const string INVALID_XML_ENCODING_REGEX = "(?<![\\uD800-\\uDBFF])[\\uDC00-\\uDFFF]|[\\uD800-\\uDBFF](?![\\uDC00-\\uDFFF])|[\\x00-\\x08\\x0B\\x0C\\x0E-\\x1F\\x7F-\\x9F\\uFEFF\\uFFFE\\uFFFF]";

	private static Regex _invalidXMLChars = new Regex("(?<![\\uD800-\\uDBFF])[\\uDC00-\\uDFFF]|[\\uD800-\\uDBFF](?![\\uDC00-\\uDFFF])|[\\x00-\\x08\\x0B\\x0C\\x0E-\\x1F\\x7F-\\x9F\\uFEFF\\uFFFE\\uFFFF]", RegexOptions.Compiled);

	public static bool HasIllegalCharacters(string text)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		List<char> list = new List<char> { '\u007f' };
		for (int i = 0; i <= 31; i++)
		{
			list.Add((char)i);
		}
		if (text.IndexOfAny(list.ToArray()) >= 0)
		{
			return true;
		}
		return false;
	}

	public static string RemoveInvalidXmlChars(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return _invalidXMLChars.Replace(text, "");
	}
}

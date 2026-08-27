using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS;

public static class StringExtensions
{
	private static Regex re;

	private static Regex RegexInstance
	{
		get
		{
			if (re == null)
			{
				re = new Regex("^[A-Fa-f0-9]{32}$|^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$", RegexOptions.Compiled);
			}
			return re;
		}
	}

	public static bool IsGuid(this string input)
	{
		if (string.IsNullOrEmpty(input) || !RegexInstance.IsMatch(input))
		{
			return false;
		}
		return true;
	}

	public static bool TryParse(this string input, out Guid result)
	{
		result = Guid.Empty;
		if (string.IsNullOrEmpty(input) || !RegexInstance.IsMatch(input))
		{
			return false;
		}
		result = new Guid(input);
		return true;
	}

	public static Guid ToGuid(this string input)
	{
		return new Guid(input);
	}

	public static string TrimAndRemoveControlChars(this string input, bool removeWhiteSpaceControlChars)
	{
		return input.RemoveControlChars(removeWhiteSpaceControlChars).Trim();
	}

	public static string RemoveControlChars(this string input, bool removeWhiteSpaceControlChars)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (removeWhiteSpaceControlChars)
		{
			return new string(input.Where((char c) => !char.IsControl(c)).ToArray());
		}
		return new string(input.Where((char c) => !char.IsControl(c) || char.IsWhiteSpace(c)).ToArray());
	}
}

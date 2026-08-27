using System;
using System.Text.RegularExpressions;

namespace VideoOS.Common.Integration;

public static class IdpUriParser
{
	public static string FormatAuthority(Uri valueToParse)
	{
		if (valueToParse == null)
		{
			throw new ArgumentNullException("valueToParse");
		}
		return FormatAuthority(valueToParse, valueToParse.OriginalString);
	}

	public static string FormatAuthority(Uri valueToParse, string beforeValue)
	{
		if (valueToParse == null)
		{
			throw new ArgumentNullException("valueToParse");
		}
		if (beforeValue == null)
		{
			throw new ArgumentNullException("beforeValue");
		}
		if (!Uri.TryCreate(beforeValue, UriKind.Absolute, out var result))
		{
			throw new ArgumentException("Parameter must be an absolute Uri.", "beforeValue");
		}
		string input = TrimEnd(result.OriginalString, result.AbsolutePath);
		if (new Regex("((?::))(?:[0-9]+)$").Match(input).Success || !valueToParse.IsDefaultPort)
		{
			return FormattableString.Invariant($"{valueToParse.Scheme}{Uri.SchemeDelimiter}{valueToParse.Host}:{valueToParse.Port}{valueToParse.AbsolutePath}");
		}
		return FormattableString.Invariant($"{valueToParse.Scheme}{Uri.SchemeDelimiter}{valueToParse.Host}{valueToParse.AbsolutePath}");
	}

	private static string TrimEnd(string source, string value)
	{
		if (!source.EndsWith(value, StringComparison.Ordinal))
		{
			return source;
		}
		return source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
	}
}

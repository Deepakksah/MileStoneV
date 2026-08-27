using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VideoOS.Mobile.Helpers;

public static class CharactersSanitizer
{
	private const char DefaulltReplacementChar = '_';

	private static uint _maxNameLength = 255u;

	public const string IpExceptionChars = ".+";

	public const string CodecsExceptionChars = ";";

	public const string HostNameExceptionChars = ".";

	public const string ContentTypeExceptionChars = "/";

	public const string Base64EncodedAllowedChars = "'+/=";

	public const string VikingProtocolInvalidChars = "<>'";

	public const string DefaultInvalidChars = "\\/:*?!.\"<>|+";

	public static bool IsTrue(this string input)
	{
		return bool.TrueString.Equals(input, StringComparison.OrdinalIgnoreCase);
	}

	public static bool ToBool(this string input)
	{
		bool.TryParse(input, out var result);
		return result;
	}

	public static int ToInt32(this string input)
	{
		int.TryParse(input, out var result);
		return result;
	}

	public static ushort ToUInt16(this string input)
	{
		ushort.TryParse(input, out var result);
		return result;
	}

	public static string Sanitized(this string input, string exceptionChars = null, char replacementChar = '_')
	{
		return ReplaceChars(input, "\\/:*?!.\"<>|+", replacementChar, exceptionChars);
	}

	public static string SanitizedCustom(this string input, string invalidChars = null, char replacementChar = '_')
	{
		return ReplaceChars(input, invalidChars, replacementChar);
	}

	public static string SanitizeFolder(this string path)
	{
		if (path == null)
		{
			return null;
		}
		try
		{
			IEnumerable<char> enumerable = Path.GetInvalidPathChars().Concat(new char[2] { '<', '>' });
			foreach (char item in enumerable.Concat(enumerable))
			{
				if (path.Contains(item))
				{
					return null;
				}
			}
			Path.Combine(path);
			return path;
		}
		catch (ArgumentException)
		{
			return null;
		}
	}

	public static string SanitizeUri(this string input, UriKind kind = UriKind.Absolute)
	{
		try
		{
			new Uri(input, kind);
			return input;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static string ReplaceChars(string input, string invalidChars, char replaceChar, string exceptionChars = null)
	{
		if (string.IsNullOrEmpty(invalidChars) || string.IsNullOrEmpty(input))
		{
			return input;
		}
		List<char> list = new List<char>(invalidChars.ToCharArray());
		if (!string.IsNullOrEmpty(exceptionChars))
		{
			list.RemoveAll(((IEnumerable<char>)exceptionChars.ToCharArray()).Contains<char>);
		}
		foreach (char item in list)
		{
			input = input.Replace(item, replaceChar);
		}
		return input;
	}

	public static SanitizerError ValidateDefaultInvalidCharacters(string input, string exceptionChars = null, uint maxLength = 0u)
	{
		return ValidateCustomInvalidCharsAndLength(input, "\\/:*?!.\"<>|+", exceptionChars, maxLength);
	}

	public static SanitizerError ValidateCustomInvalidCharsAndLength(string input, string invalidChars = null, string exceptionChars = null, uint maxLength = 0u)
	{
		if (input == null)
		{
			return SanitizerError.InvalidInputString;
		}
		uint num = ((maxLength == 0) ? _maxNameLength : maxLength);
		if (input.Length > num)
		{
			return SanitizerError.StringTooLong;
		}
		IEnumerable<char> enumerable = invalidChars?.ToCharArray() ?? "\\/:*?!.\"<>|+".ToCharArray();
		if (!string.IsNullOrEmpty(exceptionChars))
		{
			enumerable = invalidChars?.ToCharArray().Except(exceptionChars.ToCharArray());
		}
		if (input.IndexOfAny(enumerable?.ToArray()) >= 0)
		{
			return SanitizerError.InvalidCharacters;
		}
		return SanitizerError.NoError;
	}

	public static SanitizerError ValidateStringLength(string input, uint maxLength)
	{
		input = input ?? throw new ArgumentNullException("input");
		if (maxLength != 0 && input.Length > maxLength)
		{
			return SanitizerError.StringTooLong;
		}
		return SanitizerError.NoError;
	}

	public static SanitizerError ValidateString(string input, string invalidChars, uint maxLength)
	{
		if (input == null)
		{
			return SanitizerError.InvalidInputString;
		}
		SanitizerError sanitizerError = ValidateStringLength(input, maxLength);
		if (sanitizerError == SanitizerError.NoError && !string.IsNullOrEmpty(invalidChars))
		{
			IEnumerable<char> source = invalidChars.ToCharArray();
			if (input.IndexOfAny(source.ToArray()) >= 0)
			{
				sanitizerError = SanitizerError.InvalidCharacters;
			}
		}
		return sanitizerError;
	}
}

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace VideoOS.Platform.UI.Extensions;

public static class StringExtensionMethods
{
	public static string ValueOrDefault(this string str, string defaultValue)
	{
		if (!string.IsNullOrWhiteSpace(str))
		{
			return str;
		}
		return defaultValue;
	}

	public static string ToUpperCustom(this string str, CultureInfo cultureInfo)
	{
		string[] source = new string[9] { "ar-SA", "zh-CN", "zh-TW", "he-IL", "hi-IN", "ja-JP", "ko-KR", "th-TH", "fa-IR" };
		string name = cultureInfo.Name;
		if (string.Equals(name, "el-GR", StringComparison.OrdinalIgnoreCase))
		{
			return RemoveAccents(str.ToUpper(cultureInfo));
		}
		if (source.Contains(name, StringComparer.OrdinalIgnoreCase))
		{
			return str.ToUpperInvariant();
		}
		return str.ToUpper(cultureInfo);
	}

	private static string RemoveAccents(string input)
	{
		return new string((from c in input.Normalize(NormalizationForm.FormD)
			where CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark
			select c).ToArray());
	}
}

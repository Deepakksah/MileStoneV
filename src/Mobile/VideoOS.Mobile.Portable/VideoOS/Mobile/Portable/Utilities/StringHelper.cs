using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoOS.Mobile.Portable.Utilities;

public static class StringHelper
{
	public static string ReplaceFirst(this string text, string oldValue, string newValue, StringComparison comparisonType)
	{
		int num = text.IndexOf(oldValue, comparisonType);
		if (num < 0)
		{
			return text;
		}
		return text.Substring(0, num) + newValue + text.Substring(num + oldValue.Length);
	}

	public static string ReplaceXssAttackChars(this string input)
	{
		return input.Replace('<', '_').Replace('>', '_').Replace('&', '_')
			.Replace('"', '_')
			.Replace(':', '_');
	}

	public static string ReplaceUrlReservedChars(this string input)
	{
		return input.Replace('#', '_').Replace('=', '_').Replace('?', '_')
			.Replace('\\', '_')
			.Replace('/', '_')
			.Replace('%', '_')
			.Replace('*', '_')
			.Replace('+', '_');
	}

	public static string ByteArrayToString(byte[] ba)
	{
		StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
		foreach (byte b in ba)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public static byte[] StringToByteArray(string hex)
	{
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}

	public static List<Guid> ToGuids(this string guids, char separator = ',')
	{
		List<Guid> result = new List<Guid>();
		if (!string.IsNullOrEmpty(guids))
		{
			Guid temp = Guid.Empty;
			result = (from _ in guids.Split(new char[1] { separator })
				where Guid.TryParse(_, out temp)
				select temp).ToList();
		}
		return result;
	}

	public static string GetBase64urlEncodedString(byte[] bytes)
	{
		return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_')
			.TrimEnd(new char[1] { '=' });
	}
}

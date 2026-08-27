using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Utilities.Formatting;

public static class StringByteConversionExtensions
{
	private const string VALID_BYTES_REGEX = "^[a-f0-9]*$";

	public static bool IsValidByteString(this string input)
	{
		if (input == null)
		{
			return false;
		}
		if (input.Length % 2 != 0)
		{
			return false;
		}
		return Regex.IsMatch(input, "^[a-f0-9]*$", RegexOptions.IgnoreCase);
	}

	public static byte[] ParseAsBytes(this string input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (input.Length % 2 != 0)
		{
			throw new ArgumentException("Input string must contain an equal number of characters", "input");
		}
		byte[] array = new byte[input.Length / 2];
		ParseStringAsBytes(input, array);
		return array;
	}

	private static void ParseStringAsBytes(string input, byte[] buffer)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (input.Length % 2 != 0)
		{
			throw new ArgumentException("Input string must contain an equal number of characters", "input");
		}
		if (buffer.Length < input.Length / 2)
		{
			throw new ArgumentException("Size of buffer too small.", "buffer");
		}
		for (int i = 0; i < input.Length; i += 2)
		{
			try
			{
				buffer[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
			}
			catch (FormatException innerException)
			{
				throw new ArgumentException("Input string contains invalid characters.", "input", innerException);
			}
		}
	}

	public static string AsString(this byte[] input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		return string.Join("", input.Select((byte b) => b.ToString("X2", CultureInfo.InvariantCulture)));
	}
}

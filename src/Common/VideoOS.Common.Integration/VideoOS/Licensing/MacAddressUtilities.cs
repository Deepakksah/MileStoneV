using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VideoOS.Licensing;

public static class MacAddressUtilities
{
	private static Stack<SHA256> _sha256Pool = new Stack<SHA256>();

	public static string EmptyMacAddress => "00:00:00:00:00:00";

	private static byte[] CreateMacAddressHash(string macAddress)
	{
		if (macAddress == null)
		{
			throw new ArgumentNullException("macAddress");
		}
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentException("MAC address cannot be an empty string", "macAddress");
		}
		if (Guid.TryParse(macAddress, out var result))
		{
			return ComputeHash(result.ToByteArray());
		}
		Match match = Regex.Match(macAddress, "(([0-9|a-f]{2}).?){6}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		if (!match.Success)
		{
			match = Regex.Match("00:00:00:00:00:00", "(([0-9|a-f]{2}).?){6}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		byte[] array = new byte[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = byte.Parse(match.Groups[2].Captures[i].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		return ComputeHash(array);
	}

	private static byte[] ComputeHash(byte[] bytes)
	{
		SHA256 sHA;
		lock (_sha256Pool)
		{
			sHA = ((_sha256Pool.Count <= 0) ? SHA256.Create() : _sha256Pool.Pop());
		}
		try
		{
			return sHA.ComputeHash(bytes);
		}
		finally
		{
			lock (_sha256Pool)
			{
				if (_sha256Pool.Count > 2)
				{
					sHA.Dispose();
				}
				else
				{
					_sha256Pool.Push(sHA);
				}
			}
		}
	}

	public static string CreateMacAddressHashString(string macAddress)
	{
		return Convert.ToBase64String(CreateMacAddressHash(macAddress));
	}

	public static bool IsEmptyMacAddress(string macAddress)
	{
		if (macAddress == null)
		{
			throw new ArgumentNullException("macAddress");
		}
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentException("MAC address cannot be an empty string", "macAddress");
		}
		if (Guid.TryParse(macAddress, out var result))
		{
			if (result == Guid.Empty)
			{
				return true;
			}
			return false;
		}
		if (!string.Equals(macAddress, EmptyMacAddress, StringComparison.Ordinal))
		{
			return string.Equals(macAddress, "000000000000", StringComparison.Ordinal);
		}
		return true;
	}
}

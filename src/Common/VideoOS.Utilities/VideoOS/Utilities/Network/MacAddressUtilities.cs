using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VideoOS.Utilities.Network;

public static class MacAddressUtilities
{
	private const string MAC_ADDRESS_REGEX = "^(([0-9|a-f]{2})[\\s\\.\\:\\-]?){6}$";

	private static byte[] CreateMacAddressHash(string macAddress)
	{
		if (macAddress == null)
		{
			throw new ArgumentNullException("macAddress");
		}
		Match match = Regex.Match(macAddress, "^(([0-9|a-f]{2})[\\s\\.\\:\\-]?){6}$", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			throw new ArgumentException("Mac address must have the format 'NN:NN:NN:NN:NN:NN' where NN is a hex number and ':' is an optional seperator character (any char).", "macAddress");
		}
		byte[] array = new byte[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = byte.Parse(match.Groups[2].Captures[i].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		using HashAlgorithm hashAlgorithm = new SHA256Managed();
		return hashAlgorithm.ComputeHash(array);
	}

	public static string CreateMacAddressHashString(string macAddress)
	{
		return Convert.ToBase64String(CreateMacAddressHash(macAddress));
	}
}

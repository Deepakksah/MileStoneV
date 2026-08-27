using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Licensing;

public static class SLC
{
	public enum Vendor
	{
		Milestone,
		Oem
	}

	private static Match MatchSlcToPattern(string slc)
	{
		if (slc == null)
		{
			throw new ArgumentNullException("slc");
		}
		string pattern = "([A-Z][0-9]{2})-([A-Z])[0-9]{2}-([0-9]{2})([0-9])-[0-9]{2}-[A-F0-9]{6}$";
		Match match = Regex.Match(slc, pattern);
		if (!match.Success)
		{
			throw new FormatInvalidException();
		}
		return match;
	}

	public static void Validate(string slc, string vendorCode, int versionMajor, int versionMinor)
	{
		Match match = MatchSlcToPattern(slc);
		if (!match.Groups[1].Value.Equals(vendorCode))
		{
			throw new FormatInvalidException();
		}
		if (!match.Groups[2].Value.Equals("C"))
		{
			throw new ProductCodeInvalidException();
		}
		if (!match.Groups[3].Value.Equals(versionMajor.ToString(CultureInfo.InvariantCulture)))
		{
			throw new ProductCodeInvalidException();
		}
		if (!match.Groups[4].Value.Equals(versionMinor.ToString(CultureInfo.InvariantCulture)))
		{
			throw new ProductCodeInvalidException();
		}
	}

	public static bool IsTrialLicense(string slc)
	{
		if (slc == null)
		{
			throw new ArgumentNullException("slc");
		}
		if (slc.Length < "X##–X##–###–##".Length)
		{
			return false;
		}
		string[] source = new string[5] { "02", "03", "04", "05", "09" };
		string value = slc.Substring(12, 2);
		return source.Contains(value);
	}

	public static Vendor GetVendor(string slc)
	{
		MatchSlcToPattern(slc);
		if (slc.Substring(0, 1) == "M")
		{
			return Vendor.Milestone;
		}
		return Vendor.Oem;
	}
}

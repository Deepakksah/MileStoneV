using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Management.Server;

public static class BuildInfo
{
	public static string Configuration = "Release";

	public static string ProductVersion => "25.3.17737.1";

	public static string Revision => "1";

	public static string Major => "25";

	public static string Minor => "3";

	public static string Build => "";

	public static string BuildDate => "2025.10.23 12:58:12";

	public static string GetVersionDisplayString()
	{
		Match match = Regex.Match(ProductVersion, "(?<Major>\\d*)\\.(?<Minor>\\d*)\\.(?<Server>[a-z]*)(?<Build>\\d*)\\.(?<Revision>\\d*)", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			return ProductVersion;
		}
		return CreateVersionDisplayValue(new Version(Convert.ToInt32(match.Groups["Major"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Minor"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Build"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Revision"].Value, CultureInfo.InvariantCulture)));
	}

	private static string CreateVersionDisplayValue(Version ver)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}{2}", ver.Major, ver.Minor, GetRevisionLetter(ver.Revision));
	}

	public static string GetRevisionLetter()
	{
		return GetRevisionLetter(Convert.ToInt32(Revision, CultureInfo.InvariantCulture));
	}

	private static string GetRevisionLetter(int revision)
	{
		if (revision >= 0 && revision <= 25)
		{
			return Convert.ToChar(revision + 96).ToString();
		}
		return string.Format(CultureInfo.InvariantCulture, " (Rev {0})", revision);
	}
}

using System;
using System.Globalization;

namespace VideoOS.Common.Integration.Extensions;

public static class VersionExtensions
{
	public static Version GetVersionWithoutBuild(this Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		return new Version(version.Major, version.Minor, 0, version.Revision);
	}

	public static string GetVersionDisplayValue(this Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}{2}", version.Major, version.Minor, GetRevisionLetter(version));
	}

	private static string GetRevisionLetter(Version ver)
	{
		if (ver.Revision >= 0 && ver.Revision <= 25)
		{
			return Convert.ToChar(ver.Revision + 96).ToString();
		}
		return string.Format(CultureInfo.InvariantCulture, " (Rev {0})", ver.Revision);
	}
}

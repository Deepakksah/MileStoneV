using System;
using System.Text.RegularExpressions;

namespace VideoOS.Installer.Common.Utilities;

public static class CommonHelper
{
	public static bool IsValidPath(string path)
	{
		if (!string.IsNullOrEmpty(path) && Regex.IsMatch(path, "^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$"))
		{
			return path.Length <= 100;
		}
		return false;
	}

	public static bool IsValidSqlServerPath(string path)
	{
		if (!string.IsNullOrEmpty(path) && Regex.IsMatch(path, "^(([a-zA-Z0-9])|(\\\\))([^\\\\]([^/:*?<>\"|]*)+)$"))
		{
			return path.Length <= 100;
		}
		return false;
	}

	public static bool IsWindows8Or2012OrBelow()
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Version.Major <= 6)
		{
			return oSVersion.Version.Minor <= 3;
		}
		return false;
	}
}

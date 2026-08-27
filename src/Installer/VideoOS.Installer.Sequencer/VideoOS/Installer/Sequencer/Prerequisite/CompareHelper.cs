using System;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public static class CompareHelper
{
	public static bool CompareVersions(Version curVersion, Version neededVersion, CompareType compareType)
	{
		bool result = true;
		if (curVersion == null)
		{
			return false;
		}
		switch (compareType)
		{
		case CompareType.ValueEqualTo:
		case CompareType.VersionEqualTo:
			if (curVersion != neededVersion)
			{
				result = false;
			}
			break;
		case CompareType.ValueGreaterThan:
		case CompareType.VersionGreaterThan:
			if (curVersion <= neededVersion)
			{
				result = false;
			}
			break;
		case CompareType.ValueGreaterThanOrEqualTo:
		case CompareType.VersionGreaterThanOrEqualTo:
			if (curVersion < neededVersion)
			{
				result = false;
			}
			break;
		case CompareType.ValueLessThan:
		case CompareType.VersionLessThan:
			if (curVersion >= neededVersion)
			{
				result = false;
			}
			break;
		case CompareType.ValueLessThanOrEqualTo:
		case CompareType.VersionLessThanOrEqualTo:
			if (curVersion > neededVersion)
			{
				result = false;
			}
			break;
		case CompareType.ValueNotEqualTo:
		case CompareType.VersionNotEqualTo:
			if (curVersion == neededVersion)
			{
				result = false;
			}
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	private static bool CompareStrings(string curVersion, string neededVersion, CompareType compareType)
	{
		bool result = true;
		switch (compareType)
		{
		case CompareType.ValueEqualTo:
		case CompareType.VersionEqualTo:
			if (string.Compare(curVersion, neededVersion, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				result = false;
			}
			break;
		case CompareType.ValueGreaterThan:
		case CompareType.VersionGreaterThan:
			if (string.Compare(curVersion, neededVersion, StringComparison.InvariantCultureIgnoreCase) <= 0)
			{
				result = false;
			}
			break;
		case CompareType.ValueGreaterThanOrEqualTo:
		case CompareType.VersionGreaterThanOrEqualTo:
			if (string.Compare(curVersion, neededVersion, StringComparison.InvariantCultureIgnoreCase) < 0)
			{
				result = false;
			}
			break;
		case CompareType.ValueLessThan:
		case CompareType.VersionLessThan:
			if (string.Compare(curVersion, neededVersion, StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				result = false;
			}
			break;
		case CompareType.ValueLessThanOrEqualTo:
		case CompareType.VersionLessThanOrEqualTo:
			if (string.Compare(curVersion, neededVersion, StringComparison.InvariantCultureIgnoreCase) > 0)
			{
				result = false;
			}
			break;
		case CompareType.ValueNotEqualTo:
		case CompareType.VersionNotEqualTo:
			if (curVersion == neededVersion)
			{
				result = false;
			}
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public static bool ComparePlatform(PlatformType platform, string value, CompareType compareType)
	{
		string text = platform.ToString();
		return compareType switch
		{
			CompareType.ValueEqualTo => text.ToUpper() switch
			{
				"IA64" => value.ToUpper() == "IA64", 
				"X64" => value.ToUpper() == "AMD64", 
				"X86" => value.ToUpper() == "INTEL", 
				_ => false, 
			}, 
			CompareType.ValueNotEqualTo => text.ToUpper() switch
			{
				"IA64" => value.ToUpper() != "IA64", 
				"X64" => value.ToUpper() != "AMD64", 
				"X86" => value.ToUpper() != "INTEL", 
				_ => false, 
			}, 
			_ => false, 
		};
	}

	public static bool Compare(string checkVersion, string propValue, CompareType compareType)
	{
		if (IsVerison(propValue))
		{
			return CompareVersions(ConvertVersionToString(checkVersion), ConvertVersionToString(propValue), compareType);
		}
		return CompareStrings(checkVersion, propValue, compareType);
	}

	private static bool IsVerison(string propValue)
	{
		if (propValue.Contains(".") || int.TryParse(propValue, out var _))
		{
			return true;
		}
		return false;
	}

	public static Version ConvertVersionToString(string neededVersion)
	{
		if (neededVersion == null)
		{
			return null;
		}
		if (neededVersion.Length < 2)
		{
			neededVersion = $"{neededVersion}.0";
		}
		int num = 0;
		int minor = 0;
		int build = 0;
		int revision = 0;
		int num2 = neededVersion.IndexOf('.');
		if (num2 != -1)
		{
			num = Convert.ToInt32(neededVersion.Substring(0, num2));
			neededVersion = neededVersion.Substring(num2 + 1, neededVersion.Length - num2 - 1);
			num2 = neededVersion.IndexOf('.');
			if (num2 != -1)
			{
				minor = Convert.ToInt32(neededVersion.Substring(0, num2));
				neededVersion = neededVersion.Substring(num2 + 1, neededVersion.Length - num2 - 1);
				num2 = neededVersion.IndexOf('.');
				build = ((num2 == -1) ? Convert.ToInt32(neededVersion) : Convert.ToInt32(neededVersion.Substring(0, num2)));
			}
			else
			{
				minor = Convert.ToInt32(neededVersion);
			}
		}
		else
		{
			num = ((!(neededVersion == "-1")) ? Convert.ToInt32(neededVersion) : 0);
		}
		return new Version(num, minor, build, revision);
	}
}

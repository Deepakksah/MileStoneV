using System;
using System.Globalization;
using Microsoft.Win32;

namespace VideoOS.Common.Integration;

public static class CheckDevelopmentRegistry
{
	private static string regKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Milestone\\Development";

	public static bool NotUsedTemplate
	{
		get
		{
			string valueName = "NotUsedTemplate";
			return Convert.ToBoolean(Registry.GetValue(regKey, valueName, 0) ?? ((object)0), CultureInfo.InvariantCulture);
		}
	}

	public static bool FakeHigherServerVersion
	{
		get
		{
			string valueName = "FakeHigherServerVersion";
			return Convert.ToBoolean(Registry.GetValue(regKey, valueName, 0) ?? ((object)0), CultureInfo.InvariantCulture);
		}
	}
}

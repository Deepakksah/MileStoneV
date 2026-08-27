using System;
using System.Reflection;

namespace VideoOS.LPR.LogService.Properties;

public abstract class AssemblyInfo
{
	private static readonly Version _version = Assembly.GetExecutingAssembly().GetName().Version;

	public static string Major => $"{_version.Major}";

	public static string Minor => $"{_version.Minor}";

	public static string Build => $"{_version.Build}";

	public static string Revision => $"{_version.Revision}";

	public static string Version => Major + "." + Minor + "." + Build + "." + Revision;
}

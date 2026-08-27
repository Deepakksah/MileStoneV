using System;
using System.Globalization;
using System.Reflection;

namespace VideoOS.Common.Integration.ServerAPI;

public static class ServerVersion
{
	public static string GetServerVersion(Assembly assembly)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), inherit: false);
		if (customAttributes.Length != 0)
		{
			return ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
		}
		return string.Empty;
	}

	public static int GetProtocolVersion(Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		return int.Parse(version.Major.ToString(CultureInfo.CurrentCulture) + version.Minor.ToString(CultureInfo.CurrentCulture) + version.Revision.ToString(CultureInfo.CurrentCulture), NumberStyles.Integer, CultureInfo.CurrentCulture);
	}

	public static int GetProtocolVersion(string version)
	{
		return GetProtocolVersion(new Version(version));
	}
}

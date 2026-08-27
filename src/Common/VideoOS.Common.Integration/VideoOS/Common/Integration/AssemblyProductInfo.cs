using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VideoOS.Common.Integration;

public static class AssemblyProductInfo
{
	public static string Major => GetVersion(Assembly.GetCallingAssembly()).Major.ToString(CultureInfo.InvariantCulture);

	public static string Minor => GetVersion(Assembly.GetCallingAssembly()).Minor.ToString(CultureInfo.InvariantCulture);

	public static string Build => GetVersion(Assembly.GetCallingAssembly()).Build.ToString(CultureInfo.InvariantCulture);

	public static string ProductVersion => GetVersion(Assembly.GetCallingAssembly()).ToString();

	public static string DisplayVersion => GetVersionDisplayString(Assembly.GetCallingAssembly());

	public static Version Version => GetVersion(Assembly.GetCallingAssembly());

	private static Version GetVersion(Assembly assembly)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		if (!TryGetVersionStringFromAttributes(assembly, out var value))
		{
			return new Version("0.0.0.0");
		}
		Match match = Regex.Match(value, "(?<Major>\\d*)\\.(?<Minor>\\d*)\\.(?<Server>[a-z]*)(?<Build>\\d*)\\.(?<Revision>\\d*)", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			return new Version("0.0.0.0");
		}
		return new Version(Convert.ToInt32(match.Groups["Major"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Minor"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Build"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Revision"].Value, CultureInfo.InvariantCulture));
	}

	private static string GetVersionDisplayString(Assembly assembly)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		if (!TryGetVersionStringFromAttributes(assembly, out var value))
		{
			value = "0.0.0.0";
		}
		Match match = Regex.Match(value, "(?<Major>\\d*)\\.(?<Minor>\\d*)\\.(?<Server>[a-z]*)(?<Build>\\d*)\\.(?<Revision>\\d*)", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			return value;
		}
		return CreateVersionDisplayValue(new Version(Convert.ToInt32(match.Groups["Major"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Minor"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Build"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(match.Groups["Revision"].Value, CultureInfo.InvariantCulture)));
	}

	private static bool TryGetVersionStringFromAttributes(Assembly assembly, out string value)
	{
		value = null;
		AssemblyInformationalVersionAttribute customAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
		if (customAttribute != null && !string.IsNullOrEmpty(customAttribute.InformationalVersion))
		{
			value = customAttribute.InformationalVersion;
			return true;
		}
		AssemblyFileVersionAttribute customAttribute2 = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
		if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.Version))
		{
			value = customAttribute2.Version;
			return true;
		}
		return false;
	}

	private static string CreateVersionDisplayValue(Version ver)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}{2}", ver.Major, ver.Minor, GetRevisionLetter(ver.Revision));
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

using System;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class VersionChecker : IVersionChecker
{
	public Version FromVersion()
	{
		return GetVersion(InstallerManager.Instance.OldInstallInfo);
	}

	private static Version GetVersion(InstallInfo installInfo)
	{
		string s = installInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MAJOR")?.Value;
		string s2 = installInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MINOR")?.Value;
		if (int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) && int.TryParse(s2, NumberStyles.None, CultureInfo.InvariantCulture, out var result2))
		{
			return new Version(result, result2);
		}
		LogManager.Write("Failed to get the version.");
		return null;
	}
}

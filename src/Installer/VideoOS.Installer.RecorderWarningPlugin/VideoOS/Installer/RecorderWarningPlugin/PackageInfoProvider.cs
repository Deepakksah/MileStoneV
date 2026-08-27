using System.Linq;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.RecorderWarningPlugin;

public class PackageInfoProvider : IPackageInfoProvider
{
	private PackageRefType _package;

	public PackageInfoProvider(string packageId)
	{
		_package = GetPackage(packageId);
	}

	bool IPackageInfoProvider.IsInstalledInHWMM()
	{
		if (_package != null)
		{
			return _package.Condition == ConditionType.Finalize;
		}
		return false;
	}

	private static PackageRefType GetPackage(string packageId)
	{
		return InstallerManager.Instance.OldInstallInfo?.InstalledPackages?.FirstOrDefault((PackageRefType p) => p.ID.ToUpperInvariant() == packageId);
	}
}

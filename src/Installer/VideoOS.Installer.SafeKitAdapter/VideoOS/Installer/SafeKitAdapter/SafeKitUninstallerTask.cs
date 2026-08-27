using System;
using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.SafeKitAdapter;

public class SafeKitUninstallerTask : MSIUninstallerTask, IInstallationAdapter
{
	private const string _managementServerFailoverConfigId = "38E8AAE3-714D-4405-A071-858EED3DE742";

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
		if (installParameters != null)
		{
			_parameters = new List<IPackageParameter>(installParameters);
		}
	}

	protected override void DoRun()
	{
		LogManager.Write("SafeKitUninstallerTask::DoRun(): Configuration task un-installation started");
		object obj = InstallerManager.Instance.OldInstallInfo?.InstalledPackages.Find((PackageRefType x) => x.ID.Equals("38E8AAE3-714D-4405-A071-858EED3DE742", StringComparison.InvariantCultureIgnoreCase));
		PackageRefType packageRefType = InstallerManager.Instance.InstallInfo?.InstalledPackages.Find((PackageRefType x) => x.ID.Equals("38E8AAE3-714D-4405-A071-858EED3DE742", StringComparison.InvariantCultureIgnoreCase));
		if (obj == null)
		{
			obj = packageRefType;
		}
		if (obj != null)
		{
			base.DoRun();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Manager;

public class PrevInstallationHelper
{
	private string _installationLocation;

	private readonly InstallInfo _installInfo;

	private readonly InstallerConfig _installConfig;

	private List<string> _stillInstPack;

	private readonly PlatformType _platform;

	public bool IsValid
	{
		get
		{
			if (_installConfig != null)
			{
				return _installInfo != null;
			}
			return false;
		}
	}

	public string ID => _installConfig.InstallerInfo.ID;

	public string MainPackageID
	{
		get
		{
			InstallerPackageType[] installerPackages = _installConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ProductManifest.MainPackage)
				{
					return installerPackageType.ID;
				}
			}
			return _installConfig.InstallerPackages[0].ID;
		}
	}

	public string MainPackage
	{
		get
		{
			InstallerPackageType[] installerPackages = _installConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ProductManifest.MainPackage)
				{
					return installerPackageType.ProductManifest.Filename;
				}
			}
			return _installConfig.InstallerPackages[0].ProductManifest.Filename;
		}
	}

	public ArpEntryDeleteByType DeleteBy => _installConfig.UninstallerInfo.ARPEntry.DeleteBy;

	public bool IsGroupInstaller => _installConfig.InstallerInfo.GroupInstaller;

	public List<string> StillInstalledPackages
	{
		get
		{
			if (_stillInstPack == null)
			{
				_stillInstPack = InstalledPackages();
			}
			return _stillInstPack;
		}
	}

	public PrevInstallationHelper(string location, PlatformType platform)
	{
		_installationLocation = location;
		_platform = platform;
		try
		{
			string text = Path.Combine(location, "InstallInfo.xml");
			if (File.Exists(text))
			{
				_installInfo = InstallerConfigManager.LoadInstallInfo(text);
			}
			if (File.Exists(Path.Combine(location, "InstallerConfig.xml")))
			{
				_installConfig = InstallerConfigManager.LoadInstallerConfigFromFile("InstallerConfig.xml", location, _platform == PlatformType.X64);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Warning);
		}
	}

	private List<string> InstalledPackages()
	{
		List<string> list = new List<string>();
		List<string> list2 = _installInfo.InstalledPackages.Select((PackageRefType i) => i.ID).ToList();
		Dictionary<string, PackageRefType> dictionary = _installConfig.InstallExecuteSequence.ToDictionary((PackageRefType i) => i.ID);
		InstallerPackageType[] installerPackages = _installConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (dictionary.ContainsKey(installerPackageType.ID) && dictionary[installerPackageType.ID].Visibility == VisibilityType.Shown && list2.Contains(installerPackageType.ID))
			{
				int len = 512;
				StringBuilder valueBuf = new StringBuilder(len);
				if (installerPackageType.ProductManifest.PackageData != null && PrevInstallationHelperInvoke.MsiGetProductInfo(installerPackageType.ProductManifest.PackageData.ProductInfo.ProductCode, "InstallSource", valueBuf, ref len) == 0)
				{
					list.Add(installerPackageType.ProductManifest.Filename);
				}
			}
		}
		return list;
	}
}

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.UI;

public static class InstallationComponentHelper
{
	public static IEnumerable<string> GetInstalledComponentsOnModify()
	{
		return GetAllComponentList(GetPackages());
	}

	public static IEnumerable<string> GetInstalledComponentsOnInstall()
	{
		return GetCurrentInstalledComponents(GetPackages());
	}

	private static Dictionary<string, VisibilityType> GetPackages()
	{
		Dictionary<string, VisibilityType> dictionary = new Dictionary<string, VisibilityType>();
		PackageRefType[] installExecuteSequence = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			dictionary[packageRefType.ID] = packageRefType.Visibility;
		}
		return dictionary;
	}

	private static List<string> GetCurrentInstalledComponents(Dictionary<string, VisibilityType> packages)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		if (InstallerManager.Instance.InstallInfo.InstalledPackages != null)
		{
			foreach (PackageRefType installedPackage in InstallerManager.Instance.InstallInfo.InstalledPackages)
			{
				InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
				foreach (InstallerPackageType installerPackageType in installerPackages)
				{
					if (installerPackageType.ID == installedPackage.ID && packages[installerPackageType.ID] != VisibilityType.Hidden && !list2.Contains(installedPackage.ID))
					{
						list.Add(installerPackageType.ProductManifest.PackageData.DisplayName);
						list2.Add(installedPackage.ID);
					}
				}
			}
		}
		return list;
	}

	private static List<string> GetAllComponentList(Dictionary<string, VisibilityType> packages)
	{
		List<string> list = new List<string>();
		Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			dictionary[installerPackageType.ID] = installerPackageType;
		}
		foreach (PackageRefType installedPackage in InstallerManager.Instance.InstallInfo.InstalledPackages)
		{
			if (packages[installedPackage.ID] != VisibilityType.Hidden && !InstallerManager.Instance.SequenceManager.UninstalledPackages.Contains(installedPackage.ID))
			{
				list.Add(dictionary[installedPackage.ID].ProductManifest.PackageData.DisplayName);
			}
		}
		foreach (PackageRefType notInstalledPackage in InstallerManager.Instance.InstallInfo.NotInstalledPackages)
		{
			if (packages[notInstalledPackage.ID] != VisibilityType.Hidden && dictionary.ContainsKey(notInstalledPackage.ID))
			{
				InstallerPackageType installerPackageType2 = dictionary[notInstalledPackage.ID];
				string productName = null;
				ComponentInstalledHelper.InstallationState installationState = ComponentInstalledHelper.ComponentIsInstalled(installerPackageType2.ProductManifest.PackageData.ProductInfo.ProductCode, Path.Combine(InstallerManager.Instance.CurrentDirectory, installerPackageType2.ProductManifest.PackageData.PrimaryFile[0]), ref productName);
				if (InstallerManager.Instance.SequenceManager.InstalledPackages.Contains(notInstalledPackage.ID))
				{
					list.Add(dictionary[notInstalledPackage.ID].ProductManifest.PackageData.DisplayName);
				}
				else if (installationState == ComponentInstalledHelper.InstallationState.InstalledByOtherSource || installationState == ComponentInstalledHelper.InstallationState.InstalledByThisSource)
				{
					productName = ((productName != null && productName.Contains(InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher)) ? productName.Replace(string.Format(CultureInfo.InvariantCulture, "{0} ", InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher), "") : dictionary[notInstalledPackage.ID].ProductManifest.PackageData.DisplayName);
					list.Add(productName);
				}
			}
		}
		return list;
	}
}

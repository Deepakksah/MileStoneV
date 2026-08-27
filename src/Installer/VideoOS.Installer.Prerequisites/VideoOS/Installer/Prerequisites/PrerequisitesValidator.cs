using System;
using System.Globalization;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Prerequisites;

public class PrerequisitesValidator
{
	private static readonly string[] _disallowedPrerequisites = new string[2] { ".NETFramework,Version=v4.", "Microsoft.Windows.Installer" };

	public void Check(PackageCollectionType[] prerequisites, string packageName)
	{
		if (prerequisites == null)
		{
			return;
		}
		for (int i = 0; i < prerequisites.Length; i++)
		{
			if (prerequisites[i].ProductInfo?.ProductManifest?.ManifestData is Product product)
			{
				CheckPrerequisite(packageName, product.ProductCode);
			}
		}
	}

	public void Check(InstallerPrerequisiteCollectionType[] prerequisites)
	{
		if (prerequisites == null)
		{
			return;
		}
		foreach (InstallerPrerequisiteCollectionType installerPrerequisiteCollectionType in prerequisites)
		{
			if (installerPrerequisiteCollectionType.PrerequisiteInfo?.ProductManifest?.PrerequisitData != null && !string.IsNullOrEmpty(installerPrerequisiteCollectionType.PrerequisiteInfo.InstallationFolder))
			{
				CheckInstallerPrerequisite(installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest.PrerequisitData.ProductCode);
			}
		}
	}

	private static void CheckPrerequisite(string packageName, string productCode)
	{
		string errorMessage = string.Format(CultureInfo.InvariantCulture, "Prerequisites '{0}' in package '{1}' is not allowed. Global .NET and Windows Installer prerequisites are enforced by the installer framework!", productCode, packageName);
		CheckProductCode(productCode, errorMessage);
	}

	private static void CheckInstallerPrerequisite(string productCode)
	{
		string errorMessage = string.Format(CultureInfo.InvariantCulture, "Installer Prerequisites '{0}' is not allowed. Global .NET and Windows Installer prerequisites are enforced by the installer framework!", productCode);
		CheckProductCode(productCode, errorMessage);
	}

	private static void CheckProductCode(string productCode, string errorMessage)
	{
		string[] disallowedPrerequisites = _disallowedPrerequisites;
		foreach (string value in disallowedPrerequisites)
		{
			if (productCode.StartsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				LogManager.Write(errorMessage);
				throw new InvalidOperationException(errorMessage);
			}
		}
	}
}

using System;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class ProductCompliance
{
	private const string ComplianceVersionKeyName = "ComplianceVersion";

	public static ProductComplianceVersion ProductComplianceVersion
	{
		get
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine(GlobalData.PublisherKey, "Installation"), writable: false);
			using RegistryKey registryKey2 = registryKey?.OpenSubKey(GlobalData.InstallationRegistryPath.ProductId, writable: false);
			ProductComplianceVersion result = ProductComplianceVersion.NA;
			if (registryKey2 != null && !Enum.TryParse<ProductComplianceVersion>(registryKey2.GetValue("ComplianceVersion") as string, out result))
			{
				result = ProductComplianceVersion.V0;
			}
			LogManager.Write(FormattableString.Invariant($"Product compliance version: {result}"));
			return result;
		}
	}

	public static void SetProductComplianceVersion(RegistryKey key, ProductComplianceVersion complianceVersion)
	{
		if (complianceVersion == ProductComplianceVersion.NA)
		{
			throw new ArgumentException("Specified compliance version is not valid.", "complianceVersion");
		}
		if (key != null)
		{
			int num = (int)complianceVersion;
			key.SetValue("ComplianceVersion", num.ToString(NumberFormatInfo.InvariantInfo));
		}
	}
}

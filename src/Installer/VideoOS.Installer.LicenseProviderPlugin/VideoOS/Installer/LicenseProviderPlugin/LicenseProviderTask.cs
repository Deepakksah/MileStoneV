using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.LicenseProviderPlugin;

public class LicenseProviderTask : MSIInstallerTask, IInstallationAdapter
{
	public override bool RunInFinalizeMode => true;

	public override bool RunInHardwareManufacturerMode => false;

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

	public LicenseProviderTask()
	{
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		string value = string.Empty;
		if (!GlobalData.TryGetValue("PRODUCT-SKU", out value))
		{
			LogManager.Write("Getting ProductSku failed.", TraceEventType.Error);
		}
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		string value = string.Empty;
		if (!GlobalData.TryGetValue("LICENSEPATH", out value))
		{
			return;
		}
		try
		{
			if (File.Exists(value))
			{
				string directoryName = Path.GetDirectoryName(value);
				File.Move(value, directoryName + "\\license13.lic");
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}
}

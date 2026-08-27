using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.AdministrationPlugin;

public class AdministrationInstallerTask : MSIInstallerTask, IInstallationAdapter
{
	public override bool RunInFinalizeMode => true;

	public override bool RunInHardwareManufacturerMode => true;

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
		_parameters = installParameters;
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			string text = _parameters?.FirstOrDefault((IPackageParameter p) => p.Target == "LANGUAGE")?.Value;
			if (!string.IsNullOrEmpty(text))
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Setting default language to {0}", text), TraceEventType.Information);
				SetDefaultLanguage(text);
			}
		}
		else if (!base.RunOnlyPlugin)
		{
			base.DoRun();
		}
	}

	private static void SetDefaultLanguage(string defaultLanguage)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Milestone\\XProtect Management Client");
			if (registryKey == null)
			{
				LogManager.Write("Failed to write default language to registry.", TraceEventType.Error);
			}
			else
			{
				registryKey.SetValue("Language", defaultLanguage);
			}
		}
		catch (SecurityException ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to write default language to registry. {0}: {1}", ex.GetType().Name, ex.Message), TraceEventType.Error);
		}
		catch (UnauthorizedAccessException ex2)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to write default language to registry. {0}: {1}", ex2.GetType().Name, ex2.Message), TraceEventType.Error);
		}
		catch (IOException ex3)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to write default language to registry. {0}: {1}", ex3.GetType().Name, ex3.Message), TraceEventType.Error);
		}
	}
}

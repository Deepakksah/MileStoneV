using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.SafeKitAdapter;

public class SafeKitInstallerTask : MSIInstallerTask, IInstallationAdapter
{
	private const string FailoverOemNameParameter = "OEMNAME";

	private const string FailoverHelpLinkParameter = "ARPHELPLINK";

	private const string FailoverAboutLinkParameter = "ARPURLINFOABOUT";

	private const string FailoverAddToDesktopParameter = "DODESKTOP";

	private const string FailoverTransformsParameter = "TRANSFORMS";

	public override bool RunInFinalizeMode => false;

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
		try
		{
			CreateOemTransform();
			_parameters = installParameters;
			_parameters?.Add(new PackageParameter("OEMNAME", OemResources.Instance.App.FailoverManagementServerInstallDirectory));
			_parameters?.Add(new PackageParameter("DODESKTOP", "0"));
			_parameters?.Add(new PackageParameter("ARPHELPLINK", OemResources.Instance.ManagementFailoverServer.SupportPortalUrl));
			_parameters?.Add(new PackageParameter("ARPURLINFOABOUT", OemResources.Instance.ManagementFailoverServer.SupportPortalUrl));
			_parameters?.Add(new PackageParameter("TRANSFORMS", SafeKitAdapterResources.FailoverTransformName));
		}
		catch (Exception ex)
		{
			LogManager.Write("An error has occurred while running the MS failover installer adapter", ex.ToString(), TraceEventType.Error);
			throw new InstallationAdapterException(ex.Message, ex);
		}
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		if (IsInstallationAllowed())
		{
			base.DoRun();
		}
	}

	protected override void PostRun()
	{
		if (!IsInstallationAllowed())
		{
			return;
		}
		try
		{
			foreach (ServiceController safekitService in GetSafekitServices())
			{
				safekitService.Refresh();
				if (safekitService.Status != ServiceControllerStatus.Stopped && safekitService.Status != ServiceControllerStatus.StopPending)
				{
					safekitService.Stop();
				}
			}
		}
		catch (Win32Exception ex)
		{
			LogManager.Write("A Win32 error has occurred while trying to stop native failover services", ex.ToString(), TraceEventType.Warning);
		}
		catch (InvalidOperationException ex2)
		{
			LogManager.Write("An invalid operation error has occurred while trying to stop native failover services", ex2.ToString(), TraceEventType.Warning);
		}
		catch (ArgumentException ex3)
		{
			LogManager.Write("An argument error has occurred while trying to stop native failover services", ex3.ToString(), TraceEventType.Warning);
		}
	}

	private static void CreateOemTransform()
	{
		try
		{
			string text = OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimple ?? "";
			ProcessStartInfo processStartInfo = new ProcessStartInfo("RunGenerateTransform.bat", SafeKitAdapterResources.FailoverInstallerName + " \"" + text + "\"");
			processStartInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
		}
		catch (Exception ex)
		{
			LogManager.Write("An error occurred while generating failover MSI OEM transform file", ex.Message, TraceEventType.Error);
		}
	}

	private static IEnumerable<ServiceController> GetSafekitServices()
	{
		List<ServiceController> list = new List<ServiceController>();
		ServiceController item = new ServiceController("safeadmin");
		ServiceController item2 = new ServiceController("safewebserver");
		ServiceController item3 = new ServiceController("Safeagent");
		list.Add(item);
		list.Add(item2);
		list.Add(item3);
		return list;
	}

	private static bool IsInstallationAllowed()
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return false;
		}
		if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			return false;
		}
		if (GlobalData.GetValueAsBoolean("SKIP_MS_FAILOVER_INSTALL"))
		{
			return false;
		}
		return true;
	}
}

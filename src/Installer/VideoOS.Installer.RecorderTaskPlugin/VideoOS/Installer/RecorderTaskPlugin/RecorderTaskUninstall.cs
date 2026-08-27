using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.RecorderPlugins;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderTaskUninstall : MSIUninstallerTask, IInstallationAdapter
{
	private Dictionary<string, string> _installParameters;

	public RecorderTaskUninstall()
		: base(null)
	{
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Finalize(List<IPackageParameter> installParameters)
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
		_installParameters = _parameters?.ToDictionary((IPackageParameter item) => item.Target, (IPackageParameter item) => item.Value);
	}

	protected override void DoRun()
	{
		if (!PluginUtilities.Instance.IsRecorderInstalledInHWMM())
		{
			string text = _installParameters["ISFAILOVER"];
			bool isFailover = text != null && string.Equals(text, bool.TrueString, StringComparison.OrdinalIgnoreCase);
			LogManager.Write("RecorderTaskUninstall DoRun(): Recorder task uninstallation started");
			base.Message = DefaultMessage;
			NotifyObservers();
			RecorderConfigShortcuts.RemoveRecorderAndFailoverShortcuts();
			RecorderConfigShortcuts.RemoveStartMenuFolder();
			bool num = RecorderConfiguration.IsVersionWithOldEncryptionImplementation();
			if (num)
			{
				RecorderConfiguration.RemoveIdentityProviderSecret();
			}
			RecorderConfigTrayController.StopTrayController("VideoOS.Recorder.Service.TrayController");
			RecorderConfigTrayController.StopTrayController("VideoOS.Failover.Service.TrayController");
			if (num)
			{
				RecorderConfiguration.RemoveWebApiServiceConfiguration(_installParameters, isFailover);
			}
			RecorderConfiguration.RemoveFirewallRule();
			RecorderConfiguration.RemoveCertificateRegistrationAndPersistData(isFailover);
			RecorderPerformanceCounters.RemovePerformanceCounterCategory(isFailover);
		}
		base.DoRun();
	}
}

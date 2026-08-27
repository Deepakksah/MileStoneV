using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.MSI;

namespace VideoOS.ShareService.InstallerPlugin;

public class ShareServiceInstallerTask : MSIInstallerTask, IInstallationAdapter
{
	private ServerConfigurator _configurator;

	private LogManagerWrapper LogComponent;

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
		if (installParameters != null)
		{
			_parameters = new List<IPackageParameter>(installParameters);
		}
		LogComponent = new LogManagerWrapper("Share Service installer plugin INSTALL");
		_configurator = new ServerConfigurator(installParameters, installation: true, LogComponent);
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		LogComponent.Write("DoRun");
		bool valueAsBoolean = GlobalData.GetValueAsBoolean("FINALIZE-MODE");
		bool valueAsBoolean2 = GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		if (!base.RunOnlyPlugin && !valueAsBoolean)
		{
			base.DoRun();
		}
		else
		{
			_result = TaskResult.Success;
		}
		if (Result == TaskResult.Success && !valueAsBoolean2)
		{
			try
			{
				base.MaximumProgress = _configurator.MaxProgress;
				base.Message = DefaultMessage;
				NotifyObservers();
				_configurator.OnProgressChanged = OnProgressChanged;
				_configurator.RunInstall();
			}
			catch (Exception ex)
			{
				LogComponent.Write(string.Format(CultureInfo.InvariantCulture, ex.StackTrace), TraceEventType.Error);
				throw new InstallationAdapterException(ex.Message, ex);
			}
		}
		if (Result != TaskResult.Success)
		{
			throw LastError;
		}
	}

	private void OnProgressChanged(int progress)
	{
		base.CurrentProgress = progress;
		NotifyObservers();
	}
}

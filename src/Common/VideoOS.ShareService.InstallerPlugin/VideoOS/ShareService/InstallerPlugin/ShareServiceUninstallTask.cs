using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.MSI;

namespace VideoOS.ShareService.InstallerPlugin;

public class ShareServiceUninstallTask : MSIUninstallerTask, IInstallationAdapter
{
	private ServerConfigurator _configurator;

	private LogManagerWrapper LogComponent;

	public ShareServiceUninstallTask()
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
		LogComponent = new LogManagerWrapper("Share Service installer plugin UNINSTALL");
		_configurator = new ServerConfigurator(installParameters, installation: false, LogComponent);
	}

	protected override void DoRun()
	{
		LogComponent.Write("Start of " + MethodBase.GetCurrentMethod().Name);
		base.DoRun();
		try
		{
			base.MaximumProgress = _configurator.MaxProgress;
			base.Message = DefaultMessage;
			NotifyObservers();
			_configurator.OnProgressChanged = OnProgressChanged;
			_configurator.RunUninstall();
		}
		catch (Exception ex)
		{
			LogComponent.Write(ex);
			throw new InstallationAdapterException(ex.Message, ex);
		}
		LogComponent.Write("End of " + MethodBase.GetCurrentMethod().Name);
	}

	private void OnProgressChanged(int progress)
	{
		base.CurrentProgress = progress;
		NotifyObservers();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.DataCollector.Plugin;

public class DataCollectorTaskUninstall : MSIUninstallerTask, IInstallationAdapter
{
	private DataCollectorConfigurator _configurator;

	private Dictionary<string, string> _installParameters;

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
		throw new NotImplementedException();
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
		if (installParameters != null)
		{
			_parameters = new List<IPackageParameter>(installParameters);
		}
		_installParameters = _parameters?.ToDictionary((IPackageParameter item) => item.Target, (IPackageParameter item) => item.Value);
		_configurator = new DataCollectorConfigurator(_installParameters);
	}

	protected override void DoRun()
	{
		LogManager.Write("DataCollectorTaskUninstall DoRun(): data collector task uninstallation started");
		if (base.RegistryCount <= 1)
		{
			_configurator.RunUninstall();
		}
		base.DoRun();
	}
}

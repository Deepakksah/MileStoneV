using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.ServerConfigurator;

namespace VideoOS.Installer.LogServerPlugins;

public class LogServerUninstallTask : MSIUninstallerTask, IInstallationAdapter
{
	public LogServerUninstallTask()
		: base(null)
	{
	}

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
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.LogServerId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.LegacyLogServerId);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveServerEndpointInstalled(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.LogServerClientId);
		}
		catch (Exception arg2)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveClientEndpointInstalled(). Error occurred. {0}{1}", Environment.NewLine, arg2));
		}
		base.DoRun();
	}
}

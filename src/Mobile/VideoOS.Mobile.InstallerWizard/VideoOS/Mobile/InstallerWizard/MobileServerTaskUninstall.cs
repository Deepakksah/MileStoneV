using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.Mobile.Configuration;
using VideoOS.Mobile.LoginThread;

namespace VideoOS.Mobile.InstallerWizard;

public class MobileServerTaskUninstall : MSIUninstallerTask, IInstallationAdapter
{
	private Dictionary<string, string> _installParameters;

	public MobileServerTaskUninstall()
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
		_installParameters = _parameters?.ToDictionary((IPackageParameter item) => item.Target, (IPackageParameter item) => item.Value);
	}

	protected override void DoRun()
	{
		try
		{
			UnregisterService();
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "UnregisterService(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(ServerConfiguratorData.MobileServerEndpointId);
		}
		catch (Exception arg2)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveServerEndpointInstalled(). Error occurred. {0}{1}", Environment.NewLine, arg2));
		}
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(ServerConfiguratorData.MobileServerClientEndpointId);
		}
		catch (Exception arg3)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveClientEndpointInstalled(). Error occurred. {0}{1}", Environment.NewLine, arg3));
		}
		base.DoRun();
	}

	private void UnregisterService()
	{
		Uri managementServerUri = new Uri(_installParameters["MANAGEMENTSERVERURI"]);
		Guid serviceId = Guid.NewGuid();
		InstallerConfiguration installerConfiguration = InstallerConfiguration.FromXml(HttpUtility.HtmlDecode(_installParameters["INSTALLERCONFIGURATION"]));
		if (installerConfiguration.ServerId != null && installerConfiguration.ServerId != Guid.Empty.ToString())
		{
			serviceId = Guid.Parse(installerConfiguration.ServerId);
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		bool isAdUser = true;
		MipSdkStandaloneActions.UnregisterGracefully(managementServerUri, serviceId, empty, empty2, isAdUser);
	}
}

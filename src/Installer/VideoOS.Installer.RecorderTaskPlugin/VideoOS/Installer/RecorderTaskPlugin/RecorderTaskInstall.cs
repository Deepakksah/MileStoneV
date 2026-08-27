using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.RecorderPlugins;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderTaskInstall : MSIInstallerTask, IInstallationAdapter
{
	private Dictionary<string, string> _installParameters;

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
		_installParameters = _parameters?.ToDictionary((IPackageParameter item) => item.Target, (IPackageParameter item) => item.Value);
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		LogManager.Write("RecoderTaskPlugin::DoRun(): Configuration task installation started");
		bool valueAsBoolean = GlobalData.GetValueAsBoolean("FINALIZE-MODE");
		bool valueAsBoolean2 = GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		string text = ((_installParameters != null) ? _installParameters["ISFAILOVER"] : null);
		bool flag = text != null && string.Equals(text, bool.TrueString, StringComparison.OrdinalIgnoreCase);
		string text2 = ((_installParameters != null) ? _installParameters["INSTALLLOCATION"].Replace(OemResources.Instance.Recorder.FullNameInstaller, OemResources.Instance.FailoverServer.DefaultInstallDir) : "");
		if (!base.RunOnlyPlugin && !valueAsBoolean)
		{
			if (flag)
			{
				base.Parameters.Add(new PackageParameter("INSTALLLOCATION_FAILOVER", text2));
			}
			base.DoRun();
		}
		else
		{
			_result = TaskResult.Success;
		}
		if (valueAsBoolean2)
		{
			PluginUtilities.Instance.DeleteValueFromRegistry("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderName");
			if (Result == TaskResult.Success)
			{
				return;
			}
			throw LastError;
		}
		if (Result == TaskResult.Success)
		{
			base.Message = DefaultMessage;
			NotifyObservers();
			try
			{
				if (_installParameters == null)
				{
					throw new InvalidOperationException("Improper use of class, install parameters have not been set.");
				}
				RecorderConfiguration.SetAccountData(_installParameters);
				RecorderConfigFileUpdater.InitializeInstallerIds(_installParameters);
				RecorderConfigFileUpdater.InitializeRegistryParameters(_installParameters, flag);
				RecorderConfigFileUpdater.UpgradeRecorderXmlFile(_installParameters, OemResources.Instance);
				RecorderPerformanceCounters.CreatePerformanceCounterCategory(flag);
				RecorderConfiguration.CreateSecureFolderAndSetPermissions(_installParameters, flag);
				TelemetrySettingsConfigurator.CreateSettingsFile(OemResources.Instance.App.CompanyName);
				if (flag)
				{
					RecorderConfigFileUpdater.UpgradeFailoverRecorderXmlFile(_installParameters, OemResources.Instance);
				}
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.RecorderServerId, _installParameters["INSTALLLOCATION"]);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.ImageServerServerId, _installParameters["INSTALLLOCATION"]);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.RecorderClientId, _installParameters["INSTALLLOCATION"]);
				List<Guid> list = new List<Guid>
				{
					ServerConfiguratorData.RecorderServerId,
					ServerConfiguratorData.ImageServerServerId
				};
				List<Guid> list2 = new List<Guid> { ServerConfiguratorData.RecorderClientId };
				RecorderConfiguration.CreateFirewallRule(_installParameters);
				bool flag2 = !GlobalData.HasParameter("CONNECTEDTOSERVER") || GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER");
				if (flag)
				{
					string fullPath = Path.GetFullPath(Path.Combine(text2, "net472"));
					ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.FailoverServerId, fullPath);
					ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.FailoverClientId, fullPath);
					list.Add(ServerConfiguratorData.FailoverServerId);
					list2.Add(ServerConfiguratorData.FailoverClientId);
					ManagementServerConfiguration.AddUserToAdminRole(_installParameters);
				}
				else
				{
					ManagementServerConfiguration.AddRecorderToManagementServer(_installParameters);
				}
				ApplyStatus applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(list);
				if (applyStatus.ResultType() != RegistrationResultType.Success)
				{
					throw new RecorderCertificateRegistrationException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
				}
				RegisterClientStatus? result = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(list2, !flag2).GetResult<RegisterClientStatus>();
				if (result == RegisterClientStatus.CouldNotStartServer || result == RegisterClientStatus.CouldNotStopServer)
				{
					throw new RecorderCertificateRegistrationException(FormattableString.Invariant($"Failed while registering client, error status code: {result}"));
				}
				RecorderConfigShortcuts.CreateRecorderAndFailoverShortcuts(_installParameters);
				RecorderConfigTrayController.StartTrayController(_installParameters, "VideoOS.Recorder.Service.TrayController.exe", isFailover: false);
				if (flag)
				{
					RecorderConfigTrayController.StartTrayController(_installParameters, "VideoOS.Failover.Service.TrayController.exe", isFailover: true);
				}
				return;
			}
			catch (Exception ex)
			{
				LogManager.Write("An error has occurred while running the recorder plugin during installation", ex.ToString(), TraceEventType.Error);
				throw new InstallationAdapterException(ex.Message, ex);
			}
		}
		throw LastError;
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Mobile.InstallerWizard;

public class MobileServerTaskInstall : MSIInstallerTask, IInstallationAdapter
{
	private const string F22B2E2D793075 = "5AC5173927744AD49773F1AECD77D27E7C1619812E814B19";

	private Dictionary<string, string> _installParameters;

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
		LogManager.Write("MobileServerTaskInstall::DoRun(): Configuration task installation started");
		if (!base.RunOnlyPlugin)
		{
			base.DoRun();
		}
		else
		{
			_result = TaskResult.Success;
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
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.MobileServerEndpointId, Path.Combine(_installParameters["INSTALLDIR"], "Registrar"));
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.MobileServerClientEndpointId, Path.Combine(_installParameters["INSTALLDIR"], "Registrar"));
				ApplyStatus applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.MobileServerEndpointId });
				if (applyStatus.ResultType() != RegistrationResultType.Success)
				{
					throw new MobileServerCertificateRegistrationException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
				}
				RegistrationResult registrationResult = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.MobileServerClientEndpointId }, offlineRegistration: false);
				if (registrationResult.ResultType != RegistrationResultType.Success)
				{
					throw new MobileServerCertificateRegistrationException(FormattableString.Invariant($"Failed while registering client, error status code: {registrationResult}"));
				}
				return;
			}
			catch (Exception ex)
			{
				LogManager.Write("An error has occured while running the MobileServerTaskInstall (InstallerWizard) plugin during installation", ex.ToString(), TraceEventType.Error);
				throw;
			}
		}
		throw LastError;
	}
}

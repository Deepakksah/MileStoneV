using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.Authentication;

namespace VideoOS.Installer.LogServerPlugins;

public class LogServerInstallTask : MSIInstallerTask, IInstallationAdapter
{
	private const string InstallDirParamName = "INSTALLDIR";

	private const string SqlDatabaseName = "SQLDATABASENAME";

	private const string SqlInstanceName = "SQLINSTANCENAME";

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
		LogManager.Write("LogServerInstallTask::DoRun(): Configuration task installation started");
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
				LogManager.Write("LogServerInstallTask.DoRun() called with the following installParameters:", TraceEventType.Information);
				foreach (string key in _installParameters.Keys)
				{
					LogManager.Write(key + " = " + _installParameters[key], TraceEventType.Information);
				}
				DatabaseConnectionInfoConfigurator databaseConnectionInfoConfigurator = new DatabaseConnectionInfoConfigurator(SqlConnectionServerType.LogServer);
				IDatabaseConnectionInfo databaseConnectionInfo;
				if (string.IsNullOrEmpty(_installParameters["SQL_CONNECTION_STRING_LOG"]))
				{
					databaseConnectionInfo = new DatabaseConnectionInfo(_installParameters["SQLINSTANCENAME"], _installParameters["SQLDATABASENAME"], AuthenticationType.IntegratedSecurityTrusted);
					LogManager.Write("Connection String generated.", TraceEventType.Information);
				}
				else
				{
					databaseConnectionInfo = new DatabaseConnectionInfo(_installParameters["SQL_CONNECTION_STRING_LOG"]);
				}
				((IDatabaseConnectionInfoConfigurator)databaseConnectionInfoConfigurator).Write(databaseConnectionInfo.ConnectionString);
				SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, LogManager.InnerLog);
				AuthenticationHandler authenticationHandler = new AuthenticationHandler(sqlConnectionManager, LogManager.InnerLog);
				DatabaseHandler databaseHandler = new DatabaseHandler(sqlConnectionManager, authenticationHandler);
				SetupDatabaseTask.SetupDatabase(_installParameters, databaseConnectionInfo, databaseHandler);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.LogServerId, _installParameters["INSTALLDIR"]);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.LegacyLogServerId, _installParameters["INSTALLDIR"]);
				ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(ServerConfiguratorData.LogServerClientId, _installParameters["INSTALLDIR"]);
				ApplyStatus applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[2]
				{
					ServerConfiguratorData.LogServerId,
					ServerConfiguratorData.LegacyLogServerId
				});
				if (applyStatus.ResultType() != RegistrationResultType.Success)
				{
					throw new LogServerCertificateRegistrationException(FormattableString.Invariant($"Failed while applying certificates, error status code: {applyStatus}"));
				}
				RegistrationResult registrationResult = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new Guid[1] { ServerConfiguratorData.LogServerClientId }, offlineRegistration: false);
				if (registrationResult.ResultType != RegistrationResultType.Success)
				{
					throw new LogServerCertificateRegistrationException(FormattableString.Invariant($"Failed while registering client, error status code: {registrationResult}"));
				}
				return;
			}
			catch (Exception ex)
			{
				LogManager.Write("An error has occured while running the LogServerInstallTask (InstallerWizard) plugin during installation", ex.ToString(), TraceEventType.Error);
				throw;
			}
		}
		throw LastError;
	}
}

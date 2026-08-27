using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using Microsoft.Win32;
using VideoOS.Installer.CoSQLExprAdapter.Properties;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.CoSQLExprAdapter;

public class SqlExpressInstallationTask : InstallerTask, IInstallationAdapter
{
	private const int _needRebootError = -2067919934;

	private const string SqlFolder = "SQLExpress";

	private const string _defaultInstanceName = "MSSQLSERVER";

	private const string _registrySQLInstance = "software\\microsoft\\microsoft sql server\\instance names\\sql";

	private const string _registrySQLSetupInstance = "\\SOFTWARE\\Microsoft\\Microsoft SQL Server\\{0}\\Setup";

	private const string _primaryRegistryKey = "software\\videoos\\installation\\server";

	private const string _primaryRegistryKeyName = "database";

	private Dictionary<string, string> _globalData = new Dictionary<string, string>();

	private Dictionary<string, string> _parameters;

	private int _interval = 4;

	private SqlServerInstaller _installer = new SqlServerInstaller();

	private string _sqlInstallerName;

	private string _configFileName;

	private bool IsLocalServer
	{
		get
		{
			if (_globalData.ContainsKey("InstallNewServerChecked"))
			{
				return _globalData["InstallNewServerChecked"] == "1";
			}
			if (_parameters.ContainsKey("SQL-SERVER-LOCAL"))
			{
				return _parameters["SQL-SERVER-LOCAL"] == "yes";
			}
			return false;
		}
	}

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	private int InstallerSize
	{
		get
		{
			string s = "100";
			if (_globalData.ContainsKey("SQL-INSTALLER-SIZE"))
			{
				s = _globalData["SQL-INSTALLER-SIZE"];
			}
			else if (_parameters.ContainsKey("SQL-INSTALLER-SIZE"))
			{
				s = _parameters["SQL-INSTALLER-SIZE"];
			}
			return (int)long.Parse(s, CultureInfo.CurrentCulture);
		}
	}

	public SqlExpressInstallationTask()
	{
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
	}

	private void Watcher_OnProgress(long currentSize, long maxSize)
	{
		base.CurrentProgress = (int)currentSize + 1;
		NotifyObservers();
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		_parameters = new Dictionary<string, string>();
		if (installParameters == null)
		{
			LogManager.Write("SqlExpressTask.CheckPrerequisite() where installParameters are empty");
			throw new ArgumentNullException("installParameters");
		}
		foreach (IPackageParameter installParameter in installParameters)
		{
			_parameters.Add(installParameter.Target, installParameter.Value);
		}
		try
		{
			_globalData = GlobalData.ToDict();
			if (!IsLocalServer)
			{
				return _installer.IsSqlServerNativeClientInstalled();
			}
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("software\\microsoft\\microsoft sql server\\instance names\\sql");
			if (registryKey != null)
			{
				LogManager.Write("SQLSERVER: Found a sql-server instance in registry", TraceEventType.Information);
				string text = (string)registryKey.GetValue("MSSQLSERVER");
				if (text != null)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: Found sql-server instance {0}", text), TraceEventType.Information);
					registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.InvariantCulture, "\\SOFTWARE\\Microsoft\\Microsoft SQL Server\\{0}\\Setup", text));
					if (registryKey != null)
					{
						return (string)registryKey.GetValue("SQLDataRoot") != null;
					}
					return VerifySQLServer("localhost", AuthenticationType.IntegratedSecurityTrusted);
				}
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: Error {0}", ex.Message), TraceEventType.Error);
		}
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
		_parameters = new Dictionary<string, string>();
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		foreach (IPackageParameter installParameter in installParameters)
		{
			_parameters.Add(installParameter.Target, installParameter.Value);
		}
		_sqlInstallerName = Path.Combine(base.TargetDir, Path.Combine("SQLExpress", _parameters["SQL-SETUP-NAME-X64"]));
		_configFileName = Path.Combine(base.TargetDir, Path.Combine("SQLExpress", _parameters["SQL-SETUP-CONFIG-X64"]));
		_installer.NTAuthorityName = _parameters["SQL-AUTHORITY-NAME"];
		_installer.NTAuthoritySid = _parameters["SQL-AUTHORITY-SID"];
	}

	private static bool ServiceExists(string serviceName)
	{
		return ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName == serviceName) != null;
	}

	private static bool VerifySQLServer(string instanceName, AuthenticationType authenticationType)
	{
		LogManager.Write("SQLSERVER: Verify SQL Server", TraceEventType.Information);
		string text = "MSSQLSERVER";
		if (!ServiceExists(text))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: {0} service was not found", text), TraceEventType.Information);
			return false;
		}
		try
		{
			using ServiceController serviceController = new ServiceController(text);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: Checking {0} service status...", text), TraceEventType.Information);
			serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(3.0));
			LogManager.Write("SQLSERVER: Service is running", TraceEventType.Information);
		}
		catch (System.ServiceProcess.TimeoutException ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: received timeout while waiting for service to be running, error {0}", ex.Message), TraceEventType.Warning);
			return false;
		}
		SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(new DatabaseConnectionInfo(instanceName, "master", authenticationType), null);
		LogManager.Write("SQLSERVER: Verifying connection string", TraceEventType.Information);
		try
		{
			using DatabaseConnection databaseConnection = sqlConnectionManager.GetSqlConnection();
			DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
			databaseCommand.CommandType = CommandType.StoredProcedure;
			databaseCommand.CommandText = "sp_databases";
			using (DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader())
			{
				while (databaseDataReader.Read())
				{
					LogManager.Write(databaseDataReader.GetString(0));
				}
			}
			LogManager.Write("SQLSERVER: server is verified OK", TraceEventType.Information);
			return true;
		}
		catch (DatabaseException ex2)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQLSERVER: Error {0}", ex2.Message), TraceEventType.Warning);
			return false;
		}
	}

	private bool VerifySQLServerUIProgress(string instanceName, AuthenticationType authenticationType)
	{
		base.Message = Resources.SQL_VerifyConnection;
		base.CurrentProgress = 0;
		base.MaximumProgress = _interval * 60;
		NotifyObservers();
		bool result = false;
		using (Timer timer = new Timer())
		{
			timer.Elapsed += OnTimedProgressUpdate;
			timer.Interval = _interval * 1000;
			timer.Enabled = true;
			result = VerifySQLServer(instanceName, authenticationType);
		}
		base.CurrentProgress = base.MaximumProgress;
		NotifyObservers();
		base.MaximumProgress = 0;
		return result;
	}

	protected override void DoRun()
	{
		LogManager.Write("DoRun(): SQL Server installation task", TraceEventType.Information);
		string text = Path.Combine(Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.User), "SQL_temp_" + Guid.NewGuid().ToString());
		if (IsLocalServer)
		{
			if (!VerifySQLServerUIProgress("localhost", AuthenticationType.IntegratedSecurityTrusted))
			{
				if (CommonHelper.IsWindows8Or2012OrBelow())
				{
					string text2 = string.Format(CultureInfo.CurrentCulture, Resources.SQL_InstallError_NotCompatible);
					LogManager.Write(text2, TraceEventType.Error);
					throw new InstallationAdapterException(text2);
				}
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.SQL_UnpackInfo, _sqlInstallerName), TraceEventType.Information);
				base.MaximumProgress = InstallerSize;
				base.CurrentProgress = 0;
				base.Message = string.Format(CultureInfo.CurrentCulture, Resources.SQL_UnpackInfo, base.DisplayName);
				using (DirectoryPropergationWatcher directoryPropergationWatcher = new DirectoryPropergationWatcher(text, base.MaximumProgress, 2000))
				{
					NotifyObservers();
					directoryPropergationWatcher.OnProgress += Watcher_OnProgress;
					directoryPropergationWatcher.Start();
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Unpacking {0} to {1}", _sqlInstallerName, text), TraceEventType.Information);
					int num = _installer.Unpack(_sqlInstallerName, text);
					directoryPropergationWatcher.OnProgress -= Watcher_OnProgress;
					if (num != 0)
					{
						string text3 = string.Format(CultureInfo.CurrentCulture, Resources.SQL_UnpackError, _sqlInstallerName, num);
						LogManager.Write(text3, TraceEventType.Error);
						if (Directory.Exists(text))
						{
							Directory.Delete(text, recursive: true);
						}
						throw new InstallationAdapterException(text3);
					}
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.SQL_UnpackOK, _sqlInstallerName), TraceEventType.Information);
				}
				string text4 = string.Format(CultureInfo.CurrentCulture, Resources.SQL_StartInstallInfo, base.DisplayName);
				LogManager.Write(text4, TraceEventType.Information);
				base.Message = text4;
				base.CurrentProgress = 0;
				base.MaximumProgress = _interval * 60;
				NotifyObservers();
				using (Timer timer = new Timer())
				{
					timer.Elapsed += OnTimedProgressUpdate;
					timer.Interval = _interval * 1000;
					timer.Enabled = true;
					int num = _installer.Install(_configFileName, text);
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQL: Error Code received {0}", num));
					if (num == -2067919934 || num == 3010)
					{
						LogManager.Write("SQL: Need to reboot before continue installation");
						base.NeedsReboot = true;
					}
					if (num != 0)
					{
						string empty = string.Empty;
						empty = ((num != -2067529676) ? string.Format(CultureInfo.CurrentCulture, Resources.SQL_InstallError, num) : string.Format(CultureInfo.CurrentCulture, Resources.SQL_InstallError_SP_Missing, num));
						LogManager.Write(empty, TraceEventType.Error);
						timer.Enabled = false;
						throw new InstallationAdapterException(empty);
					}
					LogManager.Write(Resources.SQL_InstallOK, TraceEventType.Information);
					timer.Enabled = false;
				}
				base.CurrentProgress = base.MaximumProgress;
				NotifyObservers();
				base.MaximumProgress = 0;
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQL Server is already installed locally, nothing to do."));
			}
			if (Directory.Exists(text))
			{
				try
				{
					Directory.Delete(text, recursive: true);
				}
				catch (Exception)
				{
					try
					{
						Directory.Delete(text, recursive: true);
					}
					catch (Exception)
					{
					}
				}
			}
			if (!VerifySQLServerUIProgress("localhost", AuthenticationType.IntegratedSecurityTrusted))
			{
				throw new InstallationAdapterException("Unable to verify that the newly installed SQL server is running. Check the log files for more information.");
			}
			LogManager.Write(Resources.SQL_Running, TraceEventType.Information);
		}
		else
		{
			LogManager.Write("Not using Local SQL server, skipping SQL Express install.");
		}
	}

	private void OnTimedProgressUpdate(object source, ElapsedEventArgs e)
	{
		if (base.CurrentProgress + _interval >= base.MaximumProgress)
		{
			base.CurrentProgress = _interval;
		}
		base.CurrentProgress += _interval;
		NotifyObservers();
	}

	protected override void PostRun()
	{
		if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") && IsLocalServer)
		{
			using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("software\\videoos\\installation\\server"))
			{
				registryKey?.SetValue("database", 1);
			}
			LogManager.Write("SqlExpress post run success...");
		}
	}

	protected override void PreRun()
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}

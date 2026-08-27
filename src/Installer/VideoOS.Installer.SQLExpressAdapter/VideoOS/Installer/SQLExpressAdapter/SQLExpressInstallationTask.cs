using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SQLExpressAdapter.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.SQLExpressAdapter;

public class SQLExpressInstallationTask : InstallerTask, IInstallationAdapter, IDisposable
{
	private const int _needRebootError = -2067919934;

	private const string _registrySQLInstance = "software\\microsoft\\microsoft sql server\\instance names\\sql";

	private const string _primaryRegistryKey = "software\\videoos\\installation\\server";

	private const string _primaryRegistryKeyName = "database";

	private string _instanceName;

	private bool _localSQLOnly;

	private bool _installLocal;

	private SqlServerInstaller _installer = new SqlServerInstaller();

	private string _sqlName;

	private string _configFileName;

	private int _maxLineCount;

	private long _installerSize;

	public SQLExpressInstallationTask()
	{
		_installer.OnProgress += installer_OnProgress;
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
	}

	public void Dispose()
	{
		_installer.OnProgress -= installer_OnProgress;
	}

	private void installer_OnProgress(string info)
	{
		base.CurrentProgress++;
		LogManager.Write(info);
		NotifyObservers();
	}

	private void Watcher_OnProgress(long currentSize, long maxSize)
	{
		base.CurrentProgress = (int)currentSize + 1;
		NotifyObservers();
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (IPackageParameter installParameter in installParameters)
		{
			dictionary.Add(installParameter.Target, installParameter.Value);
		}
		try
		{
			_instanceName = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_MS"]).ServerName;
			_localSQLOnly = dictionary["SQL-SERVER-LOCAL"] == "yes";
			if (!_localSQLOnly)
			{
				return true;
			}
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("software\\microsoft\\microsoft sql server\\instance names\\sql");
			if (registryKey != null)
			{
				LogManager.Write("SQLSERVER: Found a sql-server instance in registry", TraceEventType.Information);
				if ((string)registryKey.GetValue(_instanceName) != null)
				{
					LogManager.Write($"SQLSERVER: Found sql-server instance {_instanceName}", TraceEventType.Information);
					try
					{
						VerifySQLServer("localhost\\" + _instanceName, AuthenticationType.IntegratedSecurityTrusted);
						LogManager.Write(string.Format("SQLSERVER: Verified sql-server {0}", "localhost\\" + _instanceName), TraceEventType.Information);
						return true;
					}
					catch (DatabaseException ex)
					{
						LogManager.Write($"SQLSERVER: Error {ex.TypeAndMessage()}", TraceEventType.Critical);
						LogManager.Write("SQLSERVER: Attempt to start MSSQL$VIDEOOSDB...");
						TryStartSqlServerService();
						return true;
					}
				}
			}
		}
		catch (SqlExpressAdapterException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			LogManager.Write($"SQLSERVER: Error {ex3.TypeAndMessage()}", TraceEventType.Error);
		}
		return false;
	}

	private void TryStartSqlServerService()
	{
		try
		{
			string text = "MSSQL$VIDEOOSDB";
			ServiceController serviceController = new ServiceController(text);
			if (serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending)
			{
				LogManager.Write($"Service {text} is already running");
				return;
			}
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 0, 15));
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 30, 0));
			LogManager.Write($"Starting service {text} called, result: {serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending}");
		}
		catch (InvalidOperationException exception)
		{
			LogManager.Write(exception);
			throw new SqlExpressAdapterException(exception);
		}
		catch (System.ServiceProcess.TimeoutException exception2)
		{
			LogManager.Write(exception2);
			throw new SqlExpressAdapterException(exception2);
		}
		catch (Exception exception3)
		{
			LogManager.Write(exception3);
			throw new SqlExpressAdapterException(exception3);
		}
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (IPackageParameter installParameter in installParameters)
		{
			dictionary.Add(installParameter.Target, installParameter.Value);
		}
		_localSQLOnly = dictionary.ContainsKey("SQL-SERVER-LOCAL") && dictionary["SQL-SERVER-LOCAL"] == "yes";
		if (_installer.Is64BitOS())
		{
			_sqlName = Path.Combine(base.TargetDir, dictionary["SQL-SETUP-NAME-X64"]);
			_configFileName = Path.Combine(base.TargetDir, dictionary["SQL-SETUP-CONFIG-X64"]);
		}
		else
		{
			_sqlName = Path.Combine(base.TargetDir, dictionary["SQL-SETUP-NAME-X86"]);
			_configFileName = Path.Combine(base.TargetDir, dictionary["SQL-SETUP-CONFIG-X86"]);
		}
		_maxLineCount = int.Parse(dictionary["SQL-LOG-LINECOUNT"]);
		_installerSize = long.Parse(dictionary["SQL-INSTALLER-SIZE"]);
		_installer.NtAuthorityName = dictionary["SQL-AUTHORITY-NAME"];
		_installer.NtAuthoritySid = dictionary["SQL-AUTHORITY-SID"];
		_instanceName = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_MS"]).ServerName;
		_installLocal = dictionary.ContainsKey("SQL-SERVER-LOCAL");
	}

	private void VerifySQLServer(string instanceName, AuthenticationType authenticationType)
	{
		SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(new DatabaseConnectionInfo(instanceName, "master", authenticationType), null);
		LogManager.Write("Verifying connection string", TraceEventType.Information);
		using DatabaseConnection databaseConnection = ((ISqlConnectionManager)sqlConnectionManager).GetSqlConnection();
		DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandType = CommandType.StoredProcedure;
		databaseCommand.CommandText = "sp_databases";
		DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
		while (databaseDataReader.Read())
		{
		}
		databaseConnection.Close();
		LogManager.Write("SQLSERVER: server is verified OK", TraceEventType.Information);
	}

	protected override void DoRun()
	{
		string text = Path.Combine(Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine), "SQL_temp_" + Guid.NewGuid().ToString());
		LogManager.Write($"Unpacking {_sqlName}...", TraceEventType.Information);
		base.MaximumProgress = (int)_installerSize;
		base.CurrentProgress = 0;
		NotifyObservers();
		if (CommonHelper.IsWindows8Or2012OrBelow())
		{
			LogManager.Write(string.Format(CultureInfo.CurrentCulture, "SQL Server 2019 not supported by Windows version"), TraceEventType.Error);
			throw new Exception();
		}
		if (_installLocal)
		{
			base.Message = string.Format(Resources.SQL_UnpackInfo, base.DisplayName);
			int num;
			using (DirectoryPropergationWatcher directoryPropergationWatcher = new DirectoryPropergationWatcher(text, base.MaximumProgress, 2000))
			{
				directoryPropergationWatcher.OnProgress += Watcher_OnProgress;
				directoryPropergationWatcher.Start();
				LogManager.Write($"Unpacking {_sqlName} to {text}", TraceEventType.Information);
				num = _installer.Unpack(_sqlName, text);
				directoryPropergationWatcher.OnProgress -= Watcher_OnProgress;
				if (num != 0)
				{
					LogManager.Write($"Unpacking of {_sqlName} failed. Error: {num}", TraceEventType.Error);
					if (Directory.Exists(text))
					{
						Directory.Delete(text, recursive: true);
					}
					throw new Exception();
				}
				LogManager.Write($"Unpacking SQL Express file {_sqlName} succeeded", TraceEventType.Information);
			}
			LogManager.Write("Installing {0}...", TraceEventType.Information);
			base.Message = string.Format(Resources.SQL_StartInstallInfo, base.DisplayName);
			base.CurrentProgress = 0;
			base.MaximumProgress = _maxLineCount;
			NotifyObservers();
			num = _installer.Install(_configFileName, text);
			LogManager.Write($"SQL: Error Code received {num}");
			if (num == -2067919934 || num == 3010)
			{
				LogManager.Write("SQL: Need to reboot before continue installation");
				base.NeedsReboot = true;
			}
			if (num != 0)
			{
				LogManager.Write($"Installing SQL Express failed. Error code = {num}", TraceEventType.Error);
				throw new Exception();
			}
			LogManager.Write("SQL Express is installed", TraceEventType.Information);
		}
		if (Directory.Exists(text))
		{
			try
			{
				Directory.Delete(text, recursive: true);
			}
			catch (IOException)
			{
				try
				{
					Directory.Delete(text, recursive: true);
				}
				catch (IOException)
				{
				}
			}
		}
		try
		{
			LogManager.Write("Veryfing localhost\\" + _instanceName, TraceEventType.Information);
			VerifySQLServer("localhost\\" + _instanceName, AuthenticationType.IntegratedSecurityTrusted);
			LogManager.Write("SQL Express is up and running", TraceEventType.Information);
		}
		catch (DatabaseException ex3)
		{
			LogManager.Write($"SQL ERROR: {ex3.TypeAndMessage()}", TraceEventType.Error);
			throw new Exception();
		}
	}

	protected override void PostRun()
	{
		RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("software\\videoos\\installation\\server");
		registryKey.SetValue("database", 1);
		registryKey.Close();
	}

	protected override void PreRun()
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ServiceProcess;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public static class DatabaseErrorCheckHelper
{
	public enum SQLServer
	{
		Supported,
		NotSupported
	}

	private static string _sqlErrorMessage;

	private const int MINIMUM_SUPPORTED_SQL_MAJOR_VERSION = 13;

	private static readonly ReadOnlyDictionary<int, int> sqlServerVersions = new ReadOnlyDictionary<int, int>(new Dictionary<int, int>
	{
		{ 13, 2016 },
		{ 14, 2017 },
		{ 15, 2019 },
		{ 16, 2022 }
	});

	public static int MinimumSupportedSqlVersionYear => sqlServerVersions[13];

	public static void ThrowErrorMessage(DataBaseErrorString errorString)
	{
		string text = string.Empty;
		switch (errorString)
		{
		case DataBaseErrorString.DatabaseNotFound:
			text = Resources.Database_ErrorDbNotFound;
			break;
		case DataBaseErrorString.LoginDoesNotHaveSysAdmin:
			text = Resources.Database_ErrorAdminRights;
			break;
		case DataBaseErrorString.LoginError:
			text = Resources.Database_ErrorLogin;
			break;
		case DataBaseErrorString.ImpossibleOverwrite:
			text = Resources.Database_ErrorOverwriteImpossible;
			break;
		case DataBaseErrorString.ServerNotFound:
		case DataBaseErrorString.UnknownError:
			text = Resources.Database_ErrorServerNotFound;
			break;
		case DataBaseErrorString.CannotStartService:
			text = Resources.Database_CoundNotRestartService;
			break;
		case DataBaseErrorString.DbupdateExceprion:
			text = Resources.Database_dbupdateException;
			break;
		case DataBaseErrorString.DbupdateFailed:
			text = Resources.Database_dbupdateFailed;
			break;
		case DataBaseErrorString.NeedRestartService:
			text = Resources.Database_RestartSQLServerNeededText;
			break;
		case DataBaseErrorString.DatabaseExists:
			text = Resources.Database_Exists;
			break;
		case DataBaseErrorString.SQLServerNotSupported:
			text = string.Format(CultureInfo.CurrentCulture, Resources.Database_NotSupportedSQLVersion, MinimumSupportedSqlVersionYear);
			break;
		}
		if (!string.IsNullOrEmpty(_sqlErrorMessage))
		{
			throw new UIFrameworkErrorException($"{Resources.DatabaseInstallType_ConnectionErrorHeadLine + Environment.NewLine}{text + Environment.NewLine}{_sqlErrorMessage}", enablePreviousButtong: true);
		}
		throw new UIFrameworkErrorException($"{Resources.DatabaseInstallType_ConnectionErrorHeadLine + Environment.NewLine}{text}", enablePreviousButtong: true);
	}

	public static bool CheckConnectionToExistingDatabase(ISqlConnectionManager sqlConnectionManager)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		_sqlErrorMessage = string.Empty;
		DataBaseErrorString dataBaseErrorString = DataBaseErrorString.Empty;
		using (DatabaseConnection databaseConnection = sqlConnectionManager.GetSqlConnection())
		{
			try
			{
				databaseConnection.Open();
			}
			catch (DatabaseException ex)
			{
				_sqlErrorMessage = ex.Message;
				dataBaseErrorString = ex.Number switch
				{
					17 => DataBaseErrorString.DatabaseNotFound, 
					4060 => DataBaseErrorString.DatabaseNotFound, 
					_ => DataBaseErrorString.LoginError, 
				};
			}
		}
		if (dataBaseErrorString != DataBaseErrorString.Empty)
		{
			ThrowErrorMessage(dataBaseErrorString);
			return false;
		}
		return true;
	}

	public static bool DatabaseExistsWithoutException(ISqlConnectionManager sqlConnectionManager)
	{
		try
		{
			return DatabaseExists(sqlConnectionManager, throwDbExist: true);
		}
		catch (UIFrameworkErrorException)
		{
			return true;
		}
	}

	public static bool DatabaseExists(ISqlConnectionManager sqlConnectionManager, bool throwDbExist)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		DataBaseErrorString dataBaseErrorString = DataBaseErrorString.Empty;
		using (DatabaseConnection databaseConnection = sqlConnectionManager.GetSqlConnection())
		{
			try
			{
				databaseConnection.Open();
			}
			catch (DatabaseException ex)
			{
				int number = ex.Number;
				dataBaseErrorString = ((number == 17 || number == 4060) ? DataBaseErrorString.DatabaseNotFound : DataBaseErrorString.LoginError);
			}
		}
		LogManager.Write("Check DatabaseExists() Data Server: " + sqlConnectionManager.ServerName + " Database Name: " + sqlConnectionManager.DatabaseName);
		LogManager.Write("Check DatabaseExists() result: " + dataBaseErrorString);
		if (throwDbExist)
		{
			if (dataBaseErrorString == DataBaseErrorString.Empty)
			{
				ThrowErrorMessage(DataBaseErrorString.DatabaseExists);
				return true;
			}
		}
		else if (dataBaseErrorString != DataBaseErrorString.Empty)
		{
			ThrowErrorMessage(dataBaseErrorString);
			return true;
		}
		return false;
	}

	public static DataBaseErrorString CheckSelectedDB(ISqlConnectionManager sqlConnectionManager)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		using (DatabaseConnection databaseConnection = sqlConnectionManager.GetSqlConnection())
		{
			try
			{
				string commandText = "IF IS_SRVROLEMEMBER('sysadmin') = 1 SELECT 1 ELSE SELECT 0";
				using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
				if ((int)databaseCommand.ExecuteScalar() == 0)
				{
					return DataBaseErrorString.LoginDoesNotHaveSysAdmin;
				}
				SQLServer sQLServer = SQLServer.NotSupported;
				try
				{
					sQLServer = IsSQLServerSupported(databaseConnection.ServerVersion);
				}
				catch (FormatException)
				{
					LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Check for SQL server version failed due to the wrong format of the input version parameter: '{0}'", databaseConnection.ServerVersion));
				}
				if (sQLServer != SQLServer.Supported)
				{
					return DataBaseErrorString.SQLServerNotSupported;
				}
			}
			catch (DatabaseException ex2)
			{
				_sqlErrorMessage = ex2.Message;
				switch (ex2.Number)
				{
				case 2:
				case 17:
					return DataBaseErrorString.ServerNotFound;
				case 18452:
				case 18456:
					return DataBaseErrorString.LoginError;
				default:
					return DataBaseErrorString.UnknownError;
				}
			}
		}
		return DataBaseErrorString.Empty;
	}

	public static bool CheckServerConnection(ISqlConnectionManager sqlConnectionManager)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		_sqlErrorMessage = string.Empty;
		LogManager.Write("Check CheckServerConnection() Data Server: " + sqlConnectionManager.ServerName + " Database Name: " + sqlConnectionManager.DatabaseName + " ");
		DataBaseErrorString dataBaseErrorString = CheckSelectedDB(sqlConnectionManager);
		if (dataBaseErrorString != DataBaseErrorString.Empty)
		{
			ThrowErrorMessage(dataBaseErrorString);
			return false;
		}
		return true;
	}

	public static void StartService(string serviceName, int timeoutMilliseconds)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void StopService(string serviceName, int timeoutMilliseconds)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
			serviceController.Stop();
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool RestartService(string serviceName, int timeoutMilliseconds)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			int tickCount = Environment.TickCount;
			TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
			serviceController.Stop();
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			int tickCount2 = Environment.TickCount;
			timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (tickCount2 - tickCount));
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}
		catch (Exception ex)
		{
			new UIFrameworkErrorException(ex.Message, enablePreviousButtong: false);
			return false;
		}
		return true;
	}

	public static SQLServer IsSQLServerSupported(string version)
	{
		if (version != null && new Version(version).Major >= 13)
		{
			return SQLServer.Supported;
		}
		return SQLServer.NotSupported;
	}
}

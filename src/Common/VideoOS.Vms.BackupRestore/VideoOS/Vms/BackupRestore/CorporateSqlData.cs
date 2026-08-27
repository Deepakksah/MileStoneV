using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Vms.BackupRestore;

public class CorporateSqlData
{
	private ISqlConnectionManager _sqlConnectionManager;

	private ISqlConnectionManager _sqlConnectionManagerMaster;

	public CorporateSqlData(ISqlConnectionManager sqlConnectionManager)
	{
		_sqlConnectionManager = sqlConnectionManager ?? throw new ArgumentNullException("sqlConnectionManager");
		_sqlConnectionManagerMaster = new SqlConnectionManager(sqlConnectionManager.GetConnectionInfo().CloneWithMasterDatabaseName(), sqlConnectionManager.Logger);
	}

	public SqlServerInfo SqlServiceAccount()
	{
		using DatabaseConnection databaseConnection = _sqlConnectionManagerMaster.GetSqlConnection();
		try
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
			databaseCommand.CommandType = CommandType.Text;
			databaseCommand.CommandText = "DECLARE @ServiceAccount varchar(200)\r\nEXECUTE dbo.xp_instance_regread\r\nN'HKEY_LOCAL_MACHINE',\r\nN'SYSTEM\\CurrentControlSet\\Services\\MSSQLSERVER',\r\nN'ObjectName',\r\n@ServiceAccount OUTPUT,\r\nN'no_output'\r\nSELECT SERVERPROPERTY('ComputerNamePhysicalNetBIOS') as ComputerName, @ServiceAccount as SQLAccountName";
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleRow);
			if (databaseDataReader.Read())
			{
				return new SqlServerInfo((string)databaseDataReader["ComputerName"], (string)databaseDataReader["SQLAccountName"]);
			}
			throw new BackupSqlDataException("Stored procedure 'read_sqlservice_account' did not return any data.");
		}
		catch (DatabaseException ex)
		{
			if (ex.Number == 22001 || ex.Number == 916)
			{
				throw new SqlPermissionOnConnectException("No access to master database", ex);
			}
			throw;
		}
	}

	public void DoBackup(string fileName)
	{
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandType = CommandType.Text;
		databaseCommand.CommandText = "BACKUP DATABASE @DatabaseName TO DISK = @FileName";
		databaseCommand.AddParameter("@DatabaseName", SqlDbType.NVarChar, 128, databaseConnection.Database);
		databaseCommand.AddParameter("@FileName", SqlDbType.NVarChar, 4000, fileName);
		databaseCommand.CommandTimeout = 0;
		databaseCommand.ExecuteNonQuery();
	}

	public void DoRestore(string fileName, bool deleteTasks)
	{
		List<BackupFileListInfo> list = QueryBackupFileInfo(fileName);
		string path = QueryCurrentDbFolder();
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		List<string> list2 = new List<string>();
		list2.Add("USE MASTER");
		list2.Add("ALTER DATABASE [" + databaseConnection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
		list2.Add("RESTORE DATABASE [" + databaseConnection.Database + "] FROM DISK = @FileName WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10,");
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(FormattableString.Invariant($"MOVE @LogicalName{i} TO @PhysicalName{i},"));
		}
		list2[list2.Count - 1] = list2.Last().TrimEnd(',');
		list2.Add("ALTER DATABASE [" + databaseConnection.Database + "] SET MULTI_USER");
		if (deleteTasks)
		{
			list2.Add("USE [" + databaseConnection.Database + "]");
			list2.Add("DELETE [Task]");
		}
		string commandText = string.Join("\n", list2);
		try
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
			databaseCommand.CommandType = CommandType.Text;
			databaseCommand.CommandText = commandText;
			databaseCommand.CommandTimeout = 0;
			databaseCommand.AddParameter("@FileName", SqlDbType.NVarChar, 4000, fileName);
			for (int j = 0; j < list.Count; j++)
			{
				databaseCommand.AddParameter(FormattableString.Invariant($"@LogicalName{j}"), SqlDbType.NVarChar, 4000, list[j].LogicalName);
				databaseCommand.AddParameter(FormattableString.Invariant($"@PhysicalName{j}"), SqlDbType.NVarChar, 4000, Path.Combine(path, Path.GetFileName(list[j].PhysicalName)));
			}
			databaseCommand.ExecuteNonQuery();
		}
		catch (DatabaseException ex)
		{
			if (ex.Number == 916)
			{
				throw new SqlPermissionOnConnectException("No access to database: " + _sqlConnectionManager.DatabaseName, ex);
			}
			throw;
		}
	}

	public string QueryCurrentDbFolder()
	{
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		string text = "SELECT [physical_name] FROM sys.database_files";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text);
		databaseCommand.CommandType = CommandType.Text;
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleRow);
		if (databaseDataReader.Read())
		{
			return Path.GetDirectoryName((string)databaseDataReader["physical_name"]);
		}
		throw new BackupSqlDataException(FormattableString.Invariant($"'{text}' does not return any data."));
	}

	private List<BackupFileListInfo> QueryBackupFileInfo(string backupFile)
	{
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		string commandText = "RESTORE FILELISTONLY FROM DISK = @file";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		databaseCommand.CommandType = CommandType.Text;
		databaseCommand.AddParameter("@file", SqlDbType.NVarChar, 4000, backupFile);
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
		List<BackupFileListInfo> list = new List<BackupFileListInfo>();
		while (databaseDataReader.Read())
		{
			BackupFileListInfo backupFileListInfo = new BackupFileListInfo();
			backupFileListInfo.LogicalName = (string)databaseDataReader["LogicalName"];
			backupFileListInfo.PhysicalName = (string)databaseDataReader["PhysicalName"];
			list.Add(backupFileListInfo);
		}
		return list;
	}

	public string QueryVersion()
	{
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		string commandText = "SELECT version FROM System";
		DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		databaseCommand.CommandType = CommandType.Text;
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleRow);
		if (databaseDataReader.Read())
		{
			return (string)databaseDataReader["version"];
		}
		throw new BackupSqlDataException("'SELECT version FROM System' does not return any data.");
	}

	public IList<RegisteredService> GetRegisteredServices()
	{
		List<RegisteredService> list = new List<RegisteredService>();
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		string commandText = "SELECT IDServer, Type, Name, Description, Addresses, Trusted, CustomData, Enabled FROM RegisteredServers";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
		while (databaseDataReader.Read())
		{
			Guid instanceId = new Guid(databaseDataReader.GetString(databaseDataReader.GetOrdinal("IDServer")));
			Guid typeId = new Guid(databaseDataReader.GetString(databaseDataReader.GetOrdinal("Type")));
			string name = databaseDataReader.GetString(databaseDataReader.GetOrdinal("Name"));
			bool enabled = !databaseDataReader.IsDBNull(databaseDataReader.GetOrdinal("Enabled")) && databaseDataReader.GetBoolean(databaseDataReader.GetOrdinal("Enabled"));
			List<ServerAddress> addresses = new List<ServerAddress>(ServerAddresses.GetFromXml(databaseDataReader.GetString(databaseDataReader.GetOrdinal("Addresses"))));
			RegisteredService registeredService = new RegisteredService(typeId, instanceId, name, enabled, addresses);
			registeredService.Description = databaseDataReader.GetString(databaseDataReader.GetOrdinal("Description"));
			registeredService.Trusted = databaseDataReader.GetBoolean(databaseDataReader.GetOrdinal("Trusted"));
			registeredService.CustomData = databaseDataReader.GetString(databaseDataReader.GetOrdinal("CustomData"));
			list.Add(registeredService);
		}
		return list;
	}

	public void ImportRegisteredServices(IList<RegisteredService> registeredServices)
	{
		DbTransaction dbTransaction = null;
		using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
		try
		{
			dbTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
			string commandText = "DELETE FROM RegisteredServers";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand.ExecuteNonQuery();
			}
			if (registeredServices != null)
			{
				commandText = "INSERT INTO RegisteredServers(LastModified, IDServer, Type, Name, Description, Addresses, Trusted, CustomData, Enabled) ";
				commandText += "VALUES(GETDATE(), @IDServer, @Type,  @Name,  @Description,  @Addresses,  @Trusted,  @CustomData,  @Enabled)";
				foreach (RegisteredService registeredService in registeredServices)
				{
					using DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText);
					DatabaseParameter databaseParameter = new DatabaseParameter("@IDServer", SqlDbType.Char, 36);
					databaseParameter.Value = registeredService.InstanceId.ToString();
					databaseCommand2.AddParameter(databaseParameter);
					DatabaseParameter databaseParameter2 = new DatabaseParameter("@Type", SqlDbType.Char, 36);
					databaseParameter2.Value = registeredService.TypeId.ToString();
					databaseCommand2.AddParameter(databaseParameter2);
					DatabaseParameter databaseParameter3 = new DatabaseParameter("@Name", SqlDbType.NVarChar, 256);
					databaseParameter3.Value = registeredService.Name;
					databaseCommand2.AddParameter(databaseParameter3);
					DatabaseParameter databaseParameter4 = new DatabaseParameter("@Description", SqlDbType.NVarChar);
					databaseParameter4.Value = registeredService.Description;
					databaseCommand2.AddParameter(databaseParameter4);
					DatabaseParameter databaseParameter5 = new DatabaseParameter("@Addresses", SqlDbType.NVarChar);
					databaseParameter5.Value = ServerAddresses.GetAsXml(registeredService.Addresses.ToArray());
					databaseCommand2.AddParameter(databaseParameter5);
					DatabaseParameter databaseParameter6 = new DatabaseParameter("@Trusted", SqlDbType.Bit);
					databaseParameter6.Value = registeredService.Trusted;
					databaseCommand2.AddParameter(databaseParameter6);
					DatabaseParameter databaseParameter7 = new DatabaseParameter("@CustomData", SqlDbType.NText);
					databaseParameter7.Value = registeredService.CustomData;
					databaseCommand2.AddParameter(databaseParameter7);
					DatabaseParameter databaseParameter8 = new DatabaseParameter("@Enabled", SqlDbType.Bit);
					databaseParameter8.Value = registeredService.Enabled;
					databaseCommand2.AddParameter(databaseParameter8);
					databaseCommand2.ExecuteNonQuery();
				}
			}
			dbTransaction.Commit();
		}
		catch (DatabaseException)
		{
			dbTransaction.Rollback();
		}
		dbTransaction.Dispose();
		dbTransaction = null;
	}

	public void WriteToFile(string fileName, string text)
	{
		try
		{
			using DatabaseConnection databaseConnection = _sqlConnectionManager.GetSqlConnection();
			DatabaseCommand databaseCommand = databaseConnection.CreateCommand("append_to_file");
			databaseCommand.AddParameter(new DatabaseParameter
			{
				ParameterName = "@FileName",
				Value = fileName
			});
			databaseCommand.AddParameter(new DatabaseParameter
			{
				ParameterName = "@Text1",
				Value = text
			});
			databaseCommand.CommandType = CommandType.StoredProcedure;
			databaseCommand.ExecuteReader();
		}
		catch (DatabaseException ex)
		{
			if (ex.Number == 60046)
			{
				throw new ScriptingNotAvailableOnSqlServerException(ex.Message, ex);
			}
			if (ex.Number == 60105)
			{
				throw new ShareNotAvailableException(ex.Message, ex);
			}
			throw;
		}
	}
}

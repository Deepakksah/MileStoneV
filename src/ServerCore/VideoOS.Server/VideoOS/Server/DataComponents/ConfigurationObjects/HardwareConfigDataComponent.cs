using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class HardwareConfigDataComponent : VmoResourceComponent, IHardwareConfigDataComponent, IDisposable
{
	private static class HardwareConfigDataColumns
	{
		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("recorder_id");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("hardware_id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder MacAddress = new DatabaseColumnBinder("mac_address");

		private static DatabaseColumnBinder Username = new DatabaseColumnBinder("username");

		public static HardwareConfigData Bind(DatabaseDataReader reader)
		{
			HardwareConfigData hardwareConfigData = new HardwareConfigData();
			hardwareConfigData.RecorderId = RecorderId.GetStringAsGuid(reader);
			hardwareConfigData.HardwareId = HardwareId.GetStringAsGuid(reader);
			hardwareConfigData.Name = Name.GetString(reader);
			hardwareConfigData.MacAddress = MacAddress.GetString(reader);
			hardwareConfigData.Username = Username.GetString(reader);
			return hardwareConfigData;
		}
	}

	private static class HardwarePasswordConfigDataColumns
	{
		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("hardware_id");

		private static DatabaseColumnBinder MacAddress = new DatabaseColumnBinder("mac_address");

		private static DatabaseColumnBinder Username = new DatabaseColumnBinder("username");

		private static DatabaseColumnBinder MinPasswordLength = new DatabaseColumnBinder("min_password_len");

		private static DatabaseColumnBinder MaxPasswordLength = new DatabaseColumnBinder("max_password_len");

		private static DatabaseColumnBinder PasswordRequirements = new DatabaseColumnBinder("password_requirements");

		public static HardwarePasswordConfigData Bind(DatabaseDataReader reader)
		{
			HardwarePasswordConfigData hardwarePasswordConfigData = new HardwarePasswordConfigData();
			hardwarePasswordConfigData.HardwareId = HardwareId.GetStringAsGuid(reader);
			hardwarePasswordConfigData.MacAddress = MacAddress.GetString(reader);
			hardwarePasswordConfigData.Username = Username.GetString(reader);
			hardwarePasswordConfigData.MinPasswordLength = MinPasswordLength.GetInt32(reader, allowNulls: true, -1);
			hardwarePasswordConfigData.MaxPasswordLength = MaxPasswordLength.GetInt32(reader, allowNulls: true, -1);
			hardwarePasswordConfigData.PasswordRequirements = PasswordRequirements.GetString(reader, allowNulls: true);
			if (hardwarePasswordConfigData.MinPasswordLength == -1)
			{
				hardwarePasswordConfigData.MinPasswordLength = null;
			}
			if (hardwarePasswordConfigData.MaxPasswordLength == -1)
			{
				hardwarePasswordConfigData.MaxPasswordLength = null;
			}
			return hardwarePasswordConfigData;
		}
	}

	private ISqlLogFactory _sqlLogFactory;

	private ISymmetricEncryption _symmetricEncryption;

	public HardwareConfigDataComponent(ISqlLogFactory sqlLogFactory, ISymmetricEncryption symmetricEncryption)
	{
		_sqlLogFactory = sqlLogFactory;
		_symmetricEncryption = symmetricEncryption;
	}

	public int CreateHardwareBlock(Guid hardwareId, DateTime expires, string owner, string data)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("create_hardware_block");
				BindGuidType("@hardware_id", hardwareId);
				BindDateTime("@expires", expires);
				BindString("@owner", owner, 256, allowNull: false, SqlDbType.NVarChar);
				BindString("@data", data, 4000, allowNull: false, SqlDbType.NVarChar);
				return (int)ExecuteScalar();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public void DeleteHardwareBlock(int id)
	{
		try
		{
			base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("delete_hardware_block");
				BindInt("@pkid", id);
				ExecuteScalar();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public HardwareConfigData QueryHardwareById(Guid hardwareId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_hardware_info");
				BindGuidType("@hardware_id", hardwareId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : HardwareConfigDataColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public HardwareConfigData[] QueryHardwareByRecorderId(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_hardware_info");
				BindGuidType("@recorder_id", recorderId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<HardwareConfigData> list = new List<HardwareConfigData>();
				while (databaseDataReader.Read())
				{
					list.Add(HardwareConfigDataColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public void UpdateCredentials(Guid id, string userName, string password, DateTime credentialsLastModified, ICallContextInfo callContextInfo)
	{
		try
		{
			base.Connection.ExecuteRetryable(delegate
			{
				Guid guid = Guid.NewGuid();
				PrepareStoredProcedure("update_hardware");
				BindGuidType("@IDHardware", id);
				BindNameType("@name", null, allowNull: true);
				BindDescription("@description", null, allowNull: true);
				BindDescription("@URI", null, allowNull: true);
				BindDescription("@LoginID", userName, allowNull: true);
				BindBoolean("@enabled", null);
				BindPasswordType("@encryptedPassword", _symmetricEncryption.EncryptToBase64String(password), allowNull: true);
				BindGuidType("@session_id", guid);
				BindDateTime("@credentialsLastModified", credentialsLastModified);
				BindDateTime("@hardwareDataLastUpdated", null);
				ExecuteNonQuery();
				using ISqlLogProcessor sqlLogProcessor = _sqlLogFactory.CreateSqlLogProcessor();
				sqlLogProcessor.LogSession(guid, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public HardwareConfigData[] QueryHardwareWithSameMacAddressById(Guid hardwareId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_hardware_with_same_macaddress_by_id");
				BindGuidType("@IDHardware", hardwareId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<HardwareConfigData> list = new List<HardwareConfigData>();
				while (databaseDataReader.Read())
				{
					list.Add(HardwareConfigDataColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public HardwarePasswordConfigData[] QueryHardwareForPasswordChange(bool selectAllHardware, Guid[] activeRecordingServers, Guid[] hardwareIds)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_hardware_for_password_change");
				Guid[] array = activeRecordingServers;
				if (array != null && array.Length != 0)
				{
					BindIdTable("@ActiveRecordingServers", Array.ConvertAll(activeRecordingServers, (Guid x) => x.ToString()));
				}
				if (selectAllHardware)
				{
					BindBoolean("@SelectAll", selectAllHardware);
				}
				else
				{
					Guid[] array2 = hardwareIds;
					if (array2 != null && array2.Length != 0)
					{
						BindIdTable("@hardwareIds", Array.ConvertAll(hardwareIds, (Guid x) => x.ToString()));
					}
				}
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<HardwarePasswordConfigData> list = new List<HardwarePasswordConfigData>();
				while (databaseDataReader.Read())
				{
					list.Add(HardwarePasswordConfigDataColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public string QueryPasswordById(Guid hardwareId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_hardware");
				BindGuidType("@IDHardware", hardwareId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (!databaseDataReader.Read())
				{
					return (string)null;
				}
				DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("EncryptedPassword");
				string value = databaseColumnBinder.GetString(databaseDataReader);
				if (databaseDataReader.NextResult())
				{
					throw new DatabaseConfigExtraRowsException();
				}
				return _symmetricEncryption.DecryptFromBase64String(value);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}

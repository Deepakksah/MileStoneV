using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient.Server;
using VideoOS.Common.Integration.Command;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class EvidenceLockComponent : VmoResourceComponent, IEvidenceLockComponent, IDisposable
{
	private static class DeviceEvidenceLockColumns
	{
		private static readonly DatabaseColumnBinder IDMarkedData = new DatabaseColumnBinder("IDMarkedData");

		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		public static DeviceEvidenceLockData Bind(DatabaseDataReader reader)
		{
			return new DeviceEvidenceLockData
			{
				IdEvidenceLock = IDMarkedData.GetGuid2(reader),
				IdDevice = IDDevice.GetStringAsGuid(reader),
				IdRecorder = IDRecorder.GetStringAsGuid(reader)
			};
		}
	}

	private static readonly SqlMetaData[] _metadataEvidenceLockSize = new SqlMetaData[2]
	{
		new SqlMetaData("IDDevice", SqlDbType.UniqueIdentifier),
		new SqlMetaData("Size", SqlDbType.BigInt)
	};

	public bool DeviceHasEvidenceLock(string deviceId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("device_has_markeddata");
				BindString("@IDDevice", deviceId, 256, allowNull: false, SqlDbType.NVarChar);
				return (bool)ExecuteScalar();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public List<DeviceEvidenceLockData> QueryDeviceEvidenceLocks(IEnumerable<Guid> evidenceLockIds)
	{
		List<DeviceEvidenceLockData> evidenceLocks = new List<DeviceEvidenceLockData>();
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("device_evidence_lock_query_by_ids");
				BindIdTable("@evidence_lock_ids", evidenceLockIds);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					evidenceLocks.Add(DeviceEvidenceLockColumns.Bind(databaseDataReader));
				}
				return evidenceLocks;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public List<Guid> QueryAllEvidenceLockIds()
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				List<Guid> list = new List<Guid>();
				PrepareStoredProcedure("markeddata_query_all_ids");
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					list.Add(databaseDataReader.GetGuid(0));
				}
				return list;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public bool RecorderHasLockedDeviceDataDistribution(Guid id)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("recording_server_has_locked_device_data_distribution");
				BindGuidType("@recorder_id", id);
				return (bool)ExecuteScalar();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public bool UpdateEvidenceLockSize(Guid evidenceLockId, IEnumerable<DeviceEvidenceLockSize> deviceEvidenceLockSizes)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("markeddata_update_sizes");
				BindGuidType("@IDMarkedData", evidenceLockId);
				BindEvidenceLockSizeTable("@StickerSizes", deviceEvidenceLockSizes);
				ExecuteNonQuery();
				return true;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	private DatabaseParameter BindEvidenceLockSizeTable(string parameterName, IEnumerable<DeviceEvidenceLockSize> deviceEvidenceLockSizes)
	{
		deviceEvidenceLockSizes = deviceEvidenceLockSizes ?? Enumerable.Empty<DeviceEvidenceLockSize>();
		Func<DeviceEvidenceLockSize, SqlDataRecord> selector = (DeviceEvidenceLockSize stickers) => BindDeviceEvidenceLockSizeRow(stickers, _metadataEvidenceLockSize);
		return BindTable(parameterName, "tvp_StickerSizeTable", deviceEvidenceLockSizes.Select(selector).ToList(), convertEmptyToNull: true);
	}

	private static SqlDataRecord BindDeviceEvidenceLockSizeRow(DeviceEvidenceLockSize value, SqlMetaData[] type)
	{
		SqlDataRecord sqlDataRecord = new SqlDataRecord(type);
		sqlDataRecord.SetGuid(0, value.DeviceId);
		sqlDataRecord.SetInt64(1, value.Size);
		return sqlDataRecord;
	}

	public IEnumerable<Guid> QueryEvidenceLockIdsWithoutSize()
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				List<Guid> list = new List<Guid>();
				PrepareStoredProcedure("markeddata_query_ids_without_size");
				using DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					list.Add(databaseDataReader.GetGuid(0));
				}
				return list;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}

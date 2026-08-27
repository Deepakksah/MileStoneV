using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class RecorderOverviewComponent : ReportingResourceComponent
{
	private static class RecorderNameColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		public static RecorderName Bind(DatabaseDataReader reader)
		{
			return new RecorderName
			{
				RecorderId = IDRecorder.GetStringAsGuid(reader),
				Name = Name.GetString(reader)
			};
		}
	}

	private static class RecorderOverviewColumns
	{
		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("CollectorId");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("HostName");

		private static DatabaseColumnBinder Authorized = new DatabaseColumnBinder("Authorized");

		private static DatabaseColumnBinder CpuInstanceId = new DatabaseColumnBinder("CpuInstanceId");

		private static DatabaseColumnBinder MemoryInstanceId = new DatabaseColumnBinder("MemoryInstanceId");

		public static RecorderOverview Bind(DatabaseDataReader reader)
		{
			return new RecorderOverview
			{
				Name = Name.GetString(reader),
				RecorderId = IDRecorder.GetStringAsGuid(reader),
				DataCollectorId = DataCollectorId.GetInt32(reader),
				HostName = HostName.GetString(reader),
				Authorized = Authorized.GetBoolean(reader),
				CpuInstanceId = CpuInstanceId.GetInt32(reader, allowNulls: true, 0),
				MemoryInstanceId = MemoryInstanceId.GetInt32(reader, allowNulls: true, 0)
			};
		}
	}

	private static class DiskOverviewColumns
	{
		private static DatabaseColumnBinder DriveName = new DatabaseColumnBinder("DriveName");

		private static DatabaseColumnBinder MediaDataUsed = new DatabaseColumnBinder("MediaDataUsed");

		private static DatabaseColumnBinder MediaDataUsedInstanceId = new DatabaseColumnBinder("MediaDataUsedInstanceId");

		private static DatabaseColumnBinder OtherDataUsed = new DatabaseColumnBinder("OtherDataUsed");

		private static DatabaseColumnBinder DiskOtherDataInstanceId = new DatabaseColumnBinder("DiskOtherDataInstanceId");

		private static DatabaseColumnBinder FreeData = new DatabaseColumnBinder("FreeData");

		private static DatabaseColumnBinder DiskFreeBytesInstanceId = new DatabaseColumnBinder("DiskFreeBytesInstanceId");

		private static DatabaseColumnBinder TotalSize = new DatabaseColumnBinder("TotalSize");

		private static DatabaseColumnBinder DiskWriteSpeedInstanceId = new DatabaseColumnBinder("DiskWriteSpeedInstanceId");

		private static DatabaseColumnBinder DiskWriteSpeedInstanceName = new DatabaseColumnBinder("DiskWriteSpeedInstanceName");

		private static DatabaseColumnBinder DiskReadSpeedInstanceId = new DatabaseColumnBinder("DiskReadSpeedInstanceId");

		private static DatabaseColumnBinder DiskReadSpeedInstanceName = new DatabaseColumnBinder("DiskReadSpeedInstanceName");

		public static DiskOverview Bind(DatabaseDataReader reader)
		{
			DiskOverview diskOverview = new DiskOverview
			{
				MediaDataUsed = new DiskCounter(),
				OtherDataUsed = new DiskCounter(),
				FreeData = new DiskCounter()
			};
			diskOverview.DiskName = DriveName.GetString(reader, allowNulls: true, "");
			long @int = MediaDataUsed.GetInt64(reader, allowNulls: true, 0L);
			(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(@int, new Unit[4]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB
			}, 1024L, 1024u);
			diskOverview.MediaDataUsed.Raw = @int;
			diskOverview.MediaDataUsed.Unit = calculatedUnit.Item2;
			diskOverview.MediaDataUsed.Value = calculatedUnit.Item1;
			diskOverview.MediaDataUsed.InstanceId = MediaDataUsedInstanceId.GetInt32(reader, allowNulls: true, -1);
			long int2 = OtherDataUsed.GetInt64(reader, allowNulls: true, 0L);
			(float, Unit) calculatedUnit2 = DataUnitUtility.GetCalculatedUnit(int2, new Unit[4]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB
			}, 1024L, 1024u);
			diskOverview.OtherDataUsed.Raw = int2;
			diskOverview.OtherDataUsed.Unit = calculatedUnit2.Item2;
			diskOverview.OtherDataUsed.Value = calculatedUnit2.Item1;
			diskOverview.OtherDataUsed.InstanceId = DiskOtherDataInstanceId.GetInt32(reader, allowNulls: true, -1);
			long int3 = FreeData.GetInt64(reader, allowNulls: true, 0L);
			(float, Unit) calculatedUnit3 = DataUnitUtility.GetCalculatedUnit(int3, new Unit[4]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB
			}, 1024L, 1024u);
			diskOverview.FreeData.Raw = int3;
			diskOverview.FreeData.Unit = calculatedUnit3.Item2;
			diskOverview.FreeData.Value = calculatedUnit3.Item1;
			diskOverview.FreeData.InstanceId = DiskFreeBytesInstanceId.GetInt32(reader, allowNulls: true, -1);
			long int4 = TotalSize.GetInt64(reader, allowNulls: true, 0L);
			(float, Unit) calculatedUnit4 = DataUnitUtility.GetCalculatedUnit(int4, new Unit[4]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB
			}, 1024L, 1024u);
			diskOverview.TotalDataSize = calculatedUnit4.Item1;
			diskOverview.TotalDataSizeUnit = calculatedUnit4.Item2;
			diskOverview.TotalSizeInBytes = int4;
			diskOverview.DiskWriteSpeedInstanceId = DiskWriteSpeedInstanceId.GetInt32(reader, allowNulls: true, -1);
			diskOverview.DiskWriteSpeedInstanceName = DiskWriteSpeedInstanceName.GetString(reader, allowNulls: true, "");
			diskOverview.DiskReadSpeedInstanceId = DiskReadSpeedInstanceId.GetInt32(reader, allowNulls: true, -1);
			diskOverview.DiskReadSpeedInstanceName = DiskReadSpeedInstanceName.GetString(reader, allowNulls: true, "");
			return diskOverview;
		}
	}

	private static class AdapterOverviewColumns
	{
		private static DatabaseColumnBinder AdapterName = new DatabaseColumnBinder("AdapterName");

		private static DatabaseColumnBinder AdapterTotal = new DatabaseColumnBinder("AdapterTotal");

		private static DatabaseColumnBinder AdapterTotalInstanceId = new DatabaseColumnBinder("AdapterTotalInstanceId");

		private static DatabaseColumnBinder AdapterReceived = new DatabaseColumnBinder("AdapterReceived");

		private static DatabaseColumnBinder AdapterRecievedInstanceId = new DatabaseColumnBinder("AdapterReceivedInstanceId");

		private static DatabaseColumnBinder AdapterSent = new DatabaseColumnBinder("AdapterSent");

		private static DatabaseColumnBinder AdapterSentInstanceId = new DatabaseColumnBinder("AdapterSentInstanceId");

		public static AdapterOverview Bind(DatabaseDataReader reader)
		{
			AdapterOverview adapterOverview = new AdapterOverview
			{
				AdapterTotal = new AdapterCounter(),
				AdapterReceived = new AdapterCounter(),
				AdapterSent = new AdapterCounter()
			};
			adapterOverview.AdapterName = AdapterName.GetString(reader, allowNulls: true, "");
			double num = AdapterTotal.GetDouble(reader, allowNulls: true, 0.0);
			adapterOverview.AdapterTotal.Raw = num;
			(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(num, new Unit[3]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec
			}, 1024.0, 1024u);
			adapterOverview.AdapterTotal.Unit = calculatedUnit.Item2;
			adapterOverview.AdapterTotal.Value = calculatedUnit.Item1;
			adapterOverview.AdapterTotal.InstanceId = AdapterTotalInstanceId.GetInt32(reader, allowNulls: true, 0);
			double num2 = AdapterReceived.GetDouble(reader, allowNulls: true, 0.0);
			adapterOverview.AdapterReceived.Raw = num2;
			(float, Unit) calculatedUnit2 = DataUnitUtility.GetCalculatedUnit(num2, new Unit[3]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec
			}, 1024.0, 1024u);
			adapterOverview.AdapterReceived.Unit = calculatedUnit2.Item2;
			adapterOverview.AdapterReceived.Value = calculatedUnit2.Item1;
			adapterOverview.AdapterReceived.InstanceId = AdapterRecievedInstanceId.GetInt32(reader, allowNulls: true, 0);
			double num3 = AdapterSent.GetDouble(reader, allowNulls: true, 0.0);
			adapterOverview.AdapterSent.Raw = num3;
			(float, Unit) calculatedUnit3 = DataUnitUtility.GetCalculatedUnit(num3, new Unit[3]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec
			}, 1024.0, 1024u);
			adapterOverview.AdapterSent.Unit = calculatedUnit3.Item2;
			adapterOverview.AdapterSent.Value = calculatedUnit3.Item1;
			adapterOverview.AdapterSent.InstanceId = AdapterSentInstanceId.GetInt32(reader, allowNulls: true, 0);
			return adapterOverview;
		}
	}

	private static class StorageOverviewColumns
	{
		private static DatabaseColumnBinder StorageId = new DatabaseColumnBinder("StorageId");

		private static DatabaseColumnBinder ArchiveId = new DatabaseColumnBinder("ArchiveId");

		private static DatabaseColumnBinder StorageName = new DatabaseColumnBinder("StorageName");

		private static DatabaseColumnBinder StoragePath = new DatabaseColumnBinder("StoragePath");

		private static DatabaseColumnBinder IsDefaultStorage = new DatabaseColumnBinder("IsDefaultStorage");

		private static DatabaseColumnBinder StorageSize = new DatabaseColumnBinder("StorageSize");

		private static DatabaseColumnBinder RetainDays = new DatabaseColumnBinder("RetainDays");

		private static DatabaseColumnBinder RecRetain = new DatabaseColumnBinder("RecRetain");

		private static DatabaseColumnBinder StorageUsed = new DatabaseColumnBinder("StorageUsed");

		private static DatabaseColumnBinder MaxRetain = new DatabaseColumnBinder("max_retain");

		private static DatabaseColumnBinder InstanceId = new DatabaseColumnBinder("instance_id");

		public static StorageOverview Bind(DatabaseDataReader reader)
		{
			StorageOverview storageOverview = new StorageOverview();
			storageOverview.StorageId = StorageId.GetString(reader);
			storageOverview.ArchiveId = ArchiveId.GetString(reader, allowNulls: true, "");
			storageOverview.Name = StorageName.GetString(reader);
			storageOverview.Path = StoragePath.GetString(reader);
			storageOverview.IsDefault = IsDefaultStorage.GetBoolean(reader);
			long @int = StorageSize.GetInt64(reader);
			(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(@int, new Unit[3]
			{
				Unit.MB,
				Unit.GB,
				Unit.TB
			}, 1024L, 1024u);
			storageOverview.Size = calculatedUnit.Item1;
			storageOverview.SizeUnit = calculatedUnit.Item2;
			storageOverview.SizeRaw = @int * 1024 * 1024;
			storageOverview.RetainDays = RetainDays.GetInt32(reader);
			storageOverview.RecordingRetain = RecRetain.GetInt32(reader);
			long int2 = StorageUsed.GetInt64(reader);
			(float, Unit) calculatedUnit2 = DataUnitUtility.GetCalculatedUnit(int2, new Unit[5]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB,
				Unit.TB
			}, 1024L, 1024u);
			storageOverview.Used = calculatedUnit2.Item1;
			storageOverview.UsedUnit = calculatedUnit2.Item2;
			storageOverview.MaxRetain = MaxRetain.GetInt32(reader);
			storageOverview.DiskUsageInstanceId = InstanceId.GetInt32(reader, allowNulls: true, 0);
			storageOverview.UsedPercent = (int)((decimal)int2 / (decimal)storageOverview.SizeRaw * 100m);
			return storageOverview;
		}
	}

	public RecorderName[] QueryRecorderNames(IEnumerable<Guid> recorderIds)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_recording_server_names");
				BindIdTable("@recorder_ids", recorderIds.Select((Guid g) => g.ToString()));
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RecorderName> list = new List<RecorderName>();
				while (databaseDataReader.Read())
				{
					list.Add(RecorderNameColumns.Bind(databaseDataReader));
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

	public string GetRecordingServerWebAddress(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_recording_server_web_address");
				BindGuidType("@recorder_id", recorderId);
				return (!ExecuteReader().Read()) ? null : ((string)ExecuteScalar());
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public RecorderOverview GetRecorderOverview(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_recorder_overview");
				BindString("@recorder_id", recorderId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : RecorderOverviewColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public DiskOverview[] GetDiskOverview(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_details_disks");
				BindString("@IDRecorder", recorderId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<DiskOverview> list = new List<DiskOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(DiskOverviewColumns.Bind(databaseDataReader));
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

	public AdapterOverview[] GetAdapterOverview(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_details_adapter");
				BindString("@IDRecorder", recorderId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<AdapterOverview> list = new List<AdapterOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(AdapterOverviewColumns.Bind(databaseDataReader));
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

	public StorageOverview[] GetStorageOverview(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_details_storages");
				BindString("@IDRecorder", recorderId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<StorageOverview> list = new List<StorageOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(StorageOverviewColumns.Bind(databaseDataReader));
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
}

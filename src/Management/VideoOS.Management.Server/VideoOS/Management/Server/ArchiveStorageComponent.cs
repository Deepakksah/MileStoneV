using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class ArchiveStorageComponent : GeminiResourceComponent, IArchiveStorageComponent, IDisposable
{
	private static class ArchiveStorageColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder Path = new DatabaseColumnBinder("path");

		private static DatabaseColumnBinder RetainMinutes = new DatabaseColumnBinder("retain_minutes");

		private static DatabaseColumnBinder MaxSize = new DatabaseColumnBinder("max_size_mb");

		private static DatabaseColumnBinder FramerateReductionEnabled = new DatabaseColumnBinder("framerate_reduction_enabled");

		private static DatabaseColumnBinder TargetFramerate = new DatabaseColumnBinder("target_framerate");

		private static DatabaseColumnBinder Schedule = new DatabaseColumnBinder("schedule");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder Mounted = new DatabaseColumnBinder("mounted");

		private static DatabaseColumnBinder RecordingStorageId = new DatabaseColumnBinder("recordingstorage_id");

		private static DatabaseColumnBinder RecordingServerId = new DatabaseColumnBinder("recordingserver_id");

		public static ArchiveStorageDataContract Bind(DatabaseDataReader reader)
		{
			return new ArchiveStorageDataContract
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Path = Path.GetString(reader),
				RetainMinutes = RetainMinutes.GetInt32(reader),
				MaxSize = MaxSize.GetInt64(reader),
				FramerateReductionEnabled = FramerateReductionEnabled.GetBoolean(reader),
				TargetFramerate = TargetFramerate.GetDouble(reader),
				ArchiveSchedule = Schedule.GetString(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				Mounted = Mounted.GetBoolean(reader),
				RecordingStorageId = RecordingStorageId.GetString(reader),
				RecordingServerId = RecordingServerId.GetString(reader)
			};
		}
	}

	private static class ArchiveStorageInformationColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder RecordingServerId = new DatabaseColumnBinder("recordingserver_id");

		private static DatabaseColumnBinder DiskUsageXml = new DatabaseColumnBinder("data");

		public static StorageDiskInformationInternal Bind(DatabaseDataReader reader)
		{
			return new StorageDiskInformationInternal
			{
				RecordingServerId = RecordingServerId.GetString(reader),
				StorageId = ID.GetString(reader),
				DiskUsageXml = DiskUsageXml.GetString(reader)
			};
		}
	}

	public ArchiveStorageDataContract[] QueryArchiveStorages(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storages");
			BindGuidType("@recordingstorage_id", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ArchiveStorageDataContract> list = new List<ArchiveStorageDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(ArchiveStorageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ArchiveStorageDataContract[] QueryArchiveStorages(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storages_by_ids");
			BindIdTable("@archive_storage_ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ArchiveStorageDataContract> list = new List<ArchiveStorageDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(ArchiveStorageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryArchiveStorageIdsByDeviceId(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_ids_by_device_id");
			BindGuidType("@device_id", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("pkid");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryArchiveStorageIdsByRecordingStorageId(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storage_ids_by_recording_storage_id");
			BindGuidType("@recordingstorage_id", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("pkid");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryArchiveStorageIdsByRecordingServerId(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storage_ids_by_recording_server_id");
			BindGuidType("@recordingserver_id", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("pkid");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ArchiveStorageDataContract QueryArchiveStorage(string archiveStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storage_by_id");
			BindGuidType("@pkid", archiveStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (ArchiveStorageDataContract)null;
			}
			ArchiveStorageDataContract result = ArchiveStorageColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateArchiveStorage(string recordingStorageId, ArchiveStorageDataContract archiveStorage, Guid sessionId, int limit)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_archive_storage");
			BindGuidType("@pkid", archiveStorage.Id);
			BindGuidType("@recordingstorage_id", recordingStorageId);
			BindNameType("@name", archiveStorage.Name);
			BindDescription("@description", archiveStorage.Description);
			BindUriType("@path", archiveStorage.Path);
			BindInt("@retain_minutes", archiveStorage.RetainMinutes);
			BindLong("@max_size_mb", archiveStorage.MaxSize);
			BindBoolean("@framerate_reduction_enabled", archiveStorage.FramerateReductionEnabled);
			BindFloat("@target_framerate", archiveStorage.TargetFramerate);
			BindXml("@schedule", archiveStorage.ArchiveSchedule);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", archiveStorage.CustomProperties);
			BindInt("@archives_per_storage_limit", limit);
			ExecuteNonQuery();
		});
	}

	public void UpdateArchiveStorage(ArchiveStorageDataContract archiveStorage, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_archive_storage");
			BindGuidType("@pkid", archiveStorage.Id);
			BindNameType("@name", archiveStorage.Name);
			BindDescription("@description", archiveStorage.Description);
			BindInt("@retain_minutes", archiveStorage.RetainMinutes);
			BindLong("@max_size_mb", archiveStorage.MaxSize);
			BindBoolean("@framerate_reduction_enabled", archiveStorage.FramerateReductionEnabled);
			BindFloat("@target_framerate", archiveStorage.TargetFramerate);
			BindXml("@schedule", archiveStorage.ArchiveSchedule);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", archiveStorage.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public string GetRecordingServerId(string archiveStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder sbSelect = new StringBuilder();
			sbSelect.Append("SELECT r.[recordingserver_id] FROM [ArchiveStorage] a ");
			sbSelect.Append("INNER JOIN [RecordingStorage] r ON a.[recordingstorage_id] = r.[pkid] ");
			sbSelect.Append("WHERE a.[pkid] = @pkid ");
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareSqlBatch();
				AddStatement(sbSelect.ToString());
				BindGuidType("@pkid", archiveStorageId);
				return (string)ExecuteScalar();
			});
		});
	}

	public string GetRecordingStorageId(string archiveStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder sbSelect = new StringBuilder();
			sbSelect.Append("SELECT r.[pkid] FROM [ArchiveStorage] a ");
			sbSelect.Append("INNER JOIN [RecordingStorage] r ON a.[recordingstorage_id] = r.[pkid] ");
			sbSelect.Append("WHERE a.[pkid] = @pkid ");
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareSqlBatch();
				AddStatement(sbSelect.ToString());
				BindGuidType("@pkid", archiveStorageId);
				return (string)ExecuteScalar();
			});
		});
	}

	public StorageDiskInformationInternal QueryArchiveStorageDiskUsage(string archiveStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_archive_storage_statusdata");
			BindGuidType("@IDStorage", archiveStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (StorageDiskInformationInternal)null;
			}
			StorageDiskInformationInternal result = ArchiveStorageInformationColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void EnableFramerateReduction(string archiveStorageId, bool enable, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_archive_storage_enable_framerate_reduction");
			BindGuidType("@pkid", archiveStorageId);
			BindBoolean("@enable", enable);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}

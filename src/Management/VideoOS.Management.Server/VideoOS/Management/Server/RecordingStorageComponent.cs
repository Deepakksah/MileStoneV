using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class RecordingStorageComponent : GeminiResourceComponent, IRecordingStorageComponent, IDisposable
{
	private static class RecordingStorageColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder Path = new DatabaseColumnBinder("path");

		private static DatabaseColumnBinder IsDefault = new DatabaseColumnBinder("is_default");

		private static DatabaseColumnBinder RetainMinutes = new DatabaseColumnBinder("retain_minutes");

		private static DatabaseColumnBinder MaxSize = new DatabaseColumnBinder("max_size_mb");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder Encryption = new DatabaseColumnBinder("encryption_method");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder Signing = new DatabaseColumnBinder("signing");

		private static DatabaseColumnBinder RecordingServerId = new DatabaseColumnBinder("recordingserver_id");

		public static RecordingStorage Bind(DatabaseDataReader reader)
		{
			return new RecordingStorage
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Path = Path.GetString(reader),
				IsDefault = IsDefault.GetBoolean(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				RetainMinutes = RetainMinutes.GetInt32(reader),
				MaxSize = MaxSize.GetInt64(reader),
				Encryption = StringToEnum<EncryptionMethod>(Encryption.GetString(reader)),
				Signing = Signing.GetBoolean(reader),
				RecordingServerId = RecordingServerId.GetString(reader)
			};
		}

		private static T StringToEnum<T>(string enumName)
		{
			return (T)Enum.Parse(typeof(T), enumName);
		}
	}

	private static class RecordingStorageInformationColumns
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

	public RecordingStorage[] QueryRecordingStorages(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storages");
			BindGuidType("@recordingServer_id", recordingServerId);
			List<RecordingStorage> list = new List<RecordingStorage>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RecordingStorageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RecordingStorage QueryRecordingStorage(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_by_id");
			BindGuidType("@pkid", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RecordingStorage)null;
			}
			RecordingStorage result = RecordingStorageColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public RecordingStorage QueryRecordingStorage(string recordingServerId, string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_by_id_and_server");
			BindGuidType("@pkid", recordingStorageId);
			BindGuidType("@recordingserver_id", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RecordingStorage)null;
			}
			RecordingStorage result = RecordingStorageColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public RecordingStorage[] QueryRecordingStorages(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storages_by_ids");
			BindIdTable("@recording_storage_ids", ids);
			List<RecordingStorage> list = new List<RecordingStorage>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RecordingStorageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryRecordingStorageIds(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_ids");
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

	public string[] QueryRecordingStorageIdsByDeviceId(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_ids_by_device_id");
			BindGuidType("@device_id", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("recordingstorage_id");
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

	public void CreateRecordingStorage(RecordingStorage recordingStorage, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_recording_storage");
			BindGuidType("@pkid", recordingStorage.Id);
			BindGuidType("@recordingserver_id", recordingStorage.RecordingServerId);
			BindNameType("@name", recordingStorage.Name);
			BindDescription("@description", recordingStorage.Description);
			BindUriType("@path", recordingStorage.Path);
			BindInt("@retain_minutes", recordingStorage.RetainMinutes);
			BindLong("@max_size_mb", recordingStorage.MaxSize);
			BindGuidType("@session_id", sessionId.ToString());
			BindBoolean("@signing", recordingStorage.Signing);
			BindCustomProperties("@custom_properties", recordingStorage.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateRecordingStorage(RecordingStorage recordingStorage, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage");
			BindGuidType("@pkid", recordingStorage.Id);
			BindNameType("@name", recordingStorage.Name);
			BindDescription("@description", recordingStorage.Description);
			BindInt("@retain_minutes", recordingStorage.RetainMinutes);
			BindLong("@max_size_mb", recordingStorage.MaxSize);
			BindGuidType("@session_id", sessionId.ToString());
			BindBoolean("@signing", recordingStorage.Signing);
			BindCustomProperties("@custom_properties", recordingStorage.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_set_default");
			BindGuidType("@pkid", recordingStorageId);
			ExecuteNonQuery();
		});
	}

	public string GetIdFromArchive(string archiveStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT arch.[recordingstorage_id] ");
			stringBuilder.Append("FROM [ArchiveStorage] arch ");
			stringBuilder.Append("WHERE arch.[pkid] = @pkid ");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindGuidType("@pkid", archiveStorageId);
			return (string)ExecuteScalar();
		});
	}

	public string GetRecordingServerId(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT rs.[recordingserver_id] ");
			stringBuilder.Append("FROM [RecordingStorage] rs ");
			stringBuilder.Append("WHERE rs.[pkid] = @recordingStorageId ");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindGuidType("@recordingStorageId", recordingStorageId);
			return (string)ExecuteScalar();
		});
	}

	public StorageDiskInformationInternal QueryRecordingStorageDiskUsage(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_statusdata");
			BindGuidType("@IDStorage", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (StorageDiskInformationInternal)null;
			}
			StorageDiskInformationInternal result = RecordingStorageInformationColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string QueryEncryptionPassword(string recordingStorageId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_storage_encryption_password");
			BindGuidType("@pkid", recordingStorageId);
			string encryptedValue = (string)ExecuteScalar();
			return RequestContext.GetEncryptionService().Decrypt(encryptedValue);
		});
	}
}

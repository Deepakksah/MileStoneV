using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class TaskComponent : GeminiResourceComponent, ITaskComponent, IDisposable
{
	private static class TaskIdColumn
	{
		private static DatabaseColumnBinder TaskId = new DatabaseColumnBinder("task_id");

		public static long Bind(DatabaseDataReader reader)
		{
			return TaskId.GetInt64(reader);
		}
	}

	private static class TaskColumns
	{
		private static DatabaseColumnBinder pkid = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder target = new DatabaseColumnBinder("target");

		private static DatabaseColumnBinder data_a = new DatabaseColumnBinder("data_a");

		private static DatabaseColumnBinder percent_completed = new DatabaseColumnBinder("percent_completed");

		private static DatabaseColumnBinder status_details = new DatabaseColumnBinder("status_details");

		private static DatabaseColumnBinder task_status = new DatabaseColumnBinder("task_status");

		private static DatabaseColumnBinder start_time = new DatabaseColumnBinder("start_time");

		private static DatabaseColumnBinder end_time = new DatabaseColumnBinder("end_time");

		public static Task Bind(DatabaseDataReader reader, ISymmetricEncryption symmetricEncryption)
		{
			Task task = new Task();
			task.Id = pkid.GetInt64(reader).ToString(CultureInfo.InvariantCulture);
			string taskTypeDb = (task.TypeDb = type.GetString(reader));
			task.Type = DatabaseTaskTypeStrings.TaskTypeDbToTaskType(taskTypeDb);
			task.Target = target.GetGuid(reader);
			string xml = XmlEncryptionHelper.DecryptXml(taskTypeDb, data_a.GetString(reader), (string value, byte[] salt) => symmetricEncryption.DecryptFromBase64String(value));
			task.TaskProperties = ReadAllProperties(xml);
			task.PercentCompleted = percent_completed.GetInt32(reader, allowNulls: true, 0);
			task.StartTime = start_time.GetDateTime(reader);
			task.EndTime = end_time.GetDateTime(reader, allowNulls: true, DateTime.MaxValue);
			string empty = string.Empty;
			try
			{
				empty = symmetricEncryption.DecryptFromBase64String(status_details.GetString(reader));
			}
			catch (FormatException)
			{
				empty = status_details.GetString(reader);
			}
			task.AsyncStatus = new AsyncStatus
			{
				LastException = empty,
				ExceptionStatus = (ExceptionStatus)task_status.GetInt32(reader)
			};
			return task;
		}

		private static TaskPropertyCollection ReadAllProperties(string xml)
		{
			TaskPropertyCollection taskPropertyCollection = new TaskPropertyCollection();
			using (StringReader input = new StringReader(xml))
			{
				XmlReader xmlReader = new XmlTextReader(input);
				if (xmlReader.ReadToFollowing("task"))
				{
					xmlReader.Read();
					bool flag = xmlReader.EOF;
					while (!flag)
					{
						if (xmlReader.NodeType == XmlNodeType.Element)
						{
							string name = xmlReader.Name;
							if (name.Equals("callcontext"))
							{
								flag = true;
							}
							else
							{
								string value = xmlReader.ReadElementContentAsString().Trim();
								taskPropertyCollection.Add(new KeyValue
								{
									Key = name,
									Value = value,
									Type = "string"
								});
							}
						}
						else
						{
							xmlReader.Read();
						}
						flag = flag || xmlReader.EOF;
					}
				}
			}
			return taskPropertyCollection;
		}
	}

	public Task[] QueryTasksByRecordingServer(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_tasks_by_recording_server");
			BindGuidType("@recordingServerId", recordingServerId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Task> list = new List<Task>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, RequestContext.GetEncryptionService().SymmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Task[] QueryTasksByParentRecordingServer(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_tasks_by_parent_recording_server");
			BindGuidType("@recordingServerId", recordingServerId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Task> list = new List<Task>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, RequestContext.GetEncryptionService().SymmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Task[] QueryTasksForManagementServer()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_tasks_for_management_server");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Task> list = new List<Task>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, RequestContext.GetEncryptionService().SymmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Task QueryTask(string taskId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_task_by_id");
			BindLong("@pkid", long.Parse(taskId, NumberStyles.Integer, CultureInfo.InvariantCulture));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Task)null;
			}
			Task result = TaskColumns.Bind(databaseDataReader, RequestContext.GetEncryptionService().SymmetricEncryption);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Task[] QueryTasksByIds(string[] taskIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_tasks_by_ids");
			BindIdTable("@pkids", taskIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Task> list = new List<Task>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, RequestContext.GetEncryptionService().SymmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long[] QueryTasksIds(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_task_ids_by_target");
			BindGuidType("@target_id", recordingServerId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<long> list = new List<long>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskIdColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long[] QueryAllTasksIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_all_task_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<long> list = new List<long>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskIdColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long[] QueryTasksIds(Task.TaskType taskType, string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_task_ids_by_target_and_type");
			BindGuidType("@target_id", recordingServerId, allowNullOrEmpty: false);
			BindString("@type", DatabaseTaskTypeStrings.TaskTypeToTaskTypeDb(taskType), 100, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<long> list = new List<long>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskIdColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void RequestStopTask(string taskId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("request_stop_on_task");
			BindLong("@pkid", long.Parse(taskId, NumberStyles.Integer, CultureInfo.InvariantCulture));
			ExecuteNonQuery();
		});
	}

	public void CleanupExpiredTasks(Task.TaskType taskType, int retainMinutes)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_tasks_expired_by_type");
			BindString("@type", DatabaseTaskTypeStrings.TaskTypeToTaskTypeDb(taskType), 100, allowNull: false, SqlDbType.NVarChar);
			BindInt("@retain_minutes", retainMinutes);
			ExecuteNonQuery();
		});
	}

	public long TruncateDeviceStorage(string deviceId, string recordingServerId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_truncate_device_storage");
			BindGuidType("@device_id", deviceId);
			BindGuidType("@recording_server_id", recordingServerId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long ChangeDeviceRecordingStorage(string deviceId, string targetStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_storage");
			BindGuidType("@deviceId", deviceId);
			BindGuidType("@targetRecordingStorageId", targetStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long DeleteRecordingStorage(string recordingStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_recording_storage");
			BindGuidType("@pkid", recordingStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long MoveRecordingStorage(string recordingStorageId, string newPath, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_move");
			BindGuidType("@pkid", recordingStorageId);
			BindString("@target_path", newPath, 512, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long MoveArchiveStorage(string archiveStorageId, string newPath, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_archive_storage_move");
			BindGuidType("@pkid", archiveStorageId);
			BindString("@target_path", newPath, 512, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long MountArchiveStorage(string archiveStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("archive_storage_mount");
			BindGuidType("@pkid", archiveStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long UnmountArchiveStorage(string archiveStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("archive_storage_unmount");
			BindGuidType("@pkid", archiveStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long DeleteArchiveStorage(string archiveStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_archive_storage");
			BindGuidType("@pkid", archiveStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_enable_encryption");
			BindGuidType("@pkid", recordingStorageId);
			BindEnumType("@encryption_method", typeof(EncryptionMethod), encryptionMethod);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			BindGuidType("@session_id", sessionId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<long> list = new List<long>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskIdColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long[] DisableRecordingStorageEncryption(string recordingStorageId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_disable_encryption");
			BindGuidType("@pkid", recordingStorageId);
			BindGuidType("@session_id", sessionId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<long> list = new List<long>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskIdColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long ChangeRecordingStorageEncryptionMethod(string recordingStorageId, EncryptionMethod encryptionMethod, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_change_encryption_method");
			BindGuidType("@pkid", recordingStorageId);
			BindEnumType("@encryption_method", typeof(EncryptionMethod), encryptionMethod);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long ChangeRecordingStorageEncryptionPassword(string recordingStorageId, string newPassword, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_change_encryption_password");
			BindGuidType("@pkid", recordingStorageId);
			BindPasswordType("@new_password", RequestContext.GetEncryptionService().Encrypt(newPassword));
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long RequestEdgeStorageRetrieval(string deviceId, DateTime startTime, DateTime endTime, string metadata, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("request_edge_storage_retrieval");
			BindGuidType("@deviceId", deviceId);
			BindString("@edgeStartTime", startTime.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture), int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindString("@edgeEndTime", endTime.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture), int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindString("@metadata", SecurityElement.Escape(metadata), int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long AddHardware(string recordingServerId, string address, string userName, string password, int driverNumber, string definitionXml, string customData)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_add_hardware");
			BindGuidType("@recording_server_id", recordingServerId);
			BindUriTypeShort("@address", address, allowNull: false);
			BindUserIdType("@user_name", userName);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			BindInt("@driver_number", driverNumber);
			BindString("@definition_xml", definitionXml, -1, allowNull: false, SqlDbType.NVarChar);
			BindString("@customdata", customData, -1, allowNull: false, SqlDbType.NVarChar);
			BindStringMax("@location_ip_address", RequestContext.RemoteIPAddress);
			BindStringMax("@user_domain_name", RequestContext.UserDomainAccountName);
			BindStringMax("@user_sid", RequestContext.AuthenticatedUserSid);
			return (long)ExecuteScalar();
		});
	}

	public long HardwareScan(string recordingServerId, string address, string userName, string password, bool useDefaultCredentials, string selectedDriversXml, string prioritizedDriversXml, string addressDriverPairsXml)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_hardware_scan");
			BindGuidType("@recording_server_id", recordingServerId);
			BindUriTypeShort("@address", address, allowNull: false);
			BindUserIdType("@user_name", userName);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			BindBoolean("@use_default_credentials", useDefaultCredentials);
			BindStringMax("@selected_drivers_xml", selectedDriversXml);
			BindStringMax("@prioritized_drivers_xml", prioritizedDriversXml);
			BindStringMax("@address_with_driver_list_xml", addressDriverPairsXml);
			return (long)ExecuteScalar();
		});
	}

	public long MoveHardware(string hardwareId, string destinationRecorderId, string destinationStorageId, bool ignoreSourceRecordingServer, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_move_hardware");
			BindGuidType("@hardware_id", hardwareId);
			BindGuidType("@destination_recording_server_id", destinationRecorderId);
			BindGuidType("@destination_storage_id", destinationStorageId);
			BindBoolean("@ignore_source_recording_server", ignoreSourceRecordingServer);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long DeleteHardware(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_delete_hardware");
			BindGuidType("@hardware_id", hardwareId);
			BindStringMax("@location_ip_address", RequestContext.RemoteIPAddress);
			BindStringMax("@user_domain_name", RequestContext.UserDomainAccountName);
			BindStringMax("@user_sid", RequestContext.AuthenticatedUserSid);
			return (long)ExecuteScalar();
		});
	}

	public long DeleteRecordingServer(string recorderId, bool userAcceptedToDeleteMarkedData)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_delete_recorder");
			BindGuidType("@recorder_id", recorderId);
			BindBoolean("@deleteMarkedData", userAcceptedToDeleteMarkedData);
			BindStringMax("@location_ip_address", RequestContext.RemoteIPAddress);
			BindStringMax("@user_domain_name", RequestContext.UserDomainAccountName);
			BindStringMax("@user_sid", RequestContext.AuthenticatedUserSid);
			return (long)ExecuteScalar();
		});
	}

	public long UpdateHardware(string hardwareId, Guid sessionId, long sessionDataId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_update_hardware");
			BindGuidType("@hardware_id", hardwareId);
			BindGuidType("@session_id", sessionId.ToString());
			BindLong("@session_data_id", sessionDataId);
			return (long)ExecuteScalar();
		});
	}

	public long ReplaceHardware(string hardwareId, string hardwareAddress, int port, bool useHttps, int httpsPort, string userName, string password, int driverNumber, string customDriverData, bool allowDeletingDisabledDevices, string actingUserSid, string actingUserDomainAccountName, string actingUserLocationIPAddress)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_replace_hardware");
			BindGuidType("@hardware_id", hardwareId);
			BindUriTypeShort("@hardware_address", hardwareAddress, allowNull: false);
			BindInt("@port", port);
			BindBoolean("@use_https", useHttps);
			BindInt("@https_port", httpsPort);
			BindUserIdType("@user_name", userName);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			BindInt("@driver_number", driverNumber);
			customDriverData = RequestContext.GetEncryptionService().Encrypt(customDriverData);
			BindString("@custom_driver_data", customDriverData, 1024, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@allow_deleting_disabled_devices", allowDeletingDisabledDevices);
			BindSIDType("@acting_user_sid", actingUserSid);
			BindUserIdType("@acting_user_domain_account_name", actingUserDomainAccountName);
			BindUriTypeShort("@acting_user_location_ip_address", actingUserLocationIPAddress, allowNull: false);
			return (long)ExecuteScalar();
		});
	}

	public long ChangeHardwarePassword(string hardwareId, string userName, string password, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_change_hardware_password");
			BindGuidType("@hardware_id", hardwareId);
			BindUserIdType("@user_name", userName);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			BindGuidType("@session_id", sessionId.ToString());
			return (long)ExecuteScalar();
		});
	}

	public long ActivateLicense(string userName, string password)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_activate_license");
			BindString("@userName", userName, -1, allowNull: false, SqlDbType.NVarChar);
			password = RequestContext.GetEncryptionService().Encrypt(password);
			BindPasswordType("@password", password);
			return (long)ExecuteScalar();
		});
	}

	public void DeleteCompletedTasksByDeviceIdBeforeDt(string deviceId, DateTime dt)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_completed_tasks_by_device_id_before_time");
			BindGuidType("@IDDevice", deviceId);
			BindDateTime("@Time", dt);
			ExecuteNonQuery();
		});
	}

	public void CleanupTask(string taskId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("cleanup_task");
			BindLong("@pkid", long.Parse(taskId, NumberStyles.Integer, CultureInfo.InvariantCulture));
			ExecuteNonQuery();
		});
	}

	public long HardwareScanExpress(string recordingServerId, string credentialsXml, bool useDefaultCredentials, bool useSecurePort, string prioritizedDriversXml)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_hardware_scan_express");
			BindGuidType("@recording_server_id", recordingServerId);
			BindBoolean("@use_default_credentials", useDefaultCredentials);
			BindBoolean("@use_secure_port", useSecurePort);
			credentialsXml = RequestContext.GetEncryptionService().Encrypt(credentialsXml);
			BindStringMax("@credentials_list_xml", credentialsXml);
			BindStringMax("@prioritized_drivers_xml", prioritizedDriversXml);
			return (long)ExecuteScalar();
		});
	}

	public long GetDevicePresets(string recordingServerId, string deviceId, long sessionDataId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_get_device_presets");
			BindGuidType("@recording_server_id", recordingServerId);
			BindGuidType("@device_id", deviceId);
			BindLong("@session_data_id", sessionDataId);
			return (long)ExecuteScalar();
		});
	}

	public long AddIdpAdmin(string adminId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_add_idp_admin");
			BindGuidType("@admin_id", adminId);
			return (long)ExecuteScalar();
		});
	}

	public long DeleteIdpAdmin(string adminId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_delete_idp_admin");
			BindGuidType("@admin_id", adminId);
			return (long)ExecuteScalar();
		});
	}

	public long DeleteIdpUser(string userId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_delete_idp_user");
			BindGuidType("@id", userId);
			return (long)ExecuteScalar();
		});
	}

	public long FirmwareUpgrade(string hardwareId, string fileId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task_firmware_upgrade");
			BindGuidType("@hardware_id", hardwareId);
			BindDescription("@file_id", fileId);
			BindGuidType("@session_id", sessionId.ToString());
			BindStringMax("@location_ip_address", RequestContext.RemoteIPAddress);
			BindStringMax("@user_domain_name", RequestContext.UserDomainAccountName);
			BindStringMax("@user_sid", RequestContext.AuthenticatedUserSid);
			return (long)ExecuteScalar();
		});
	}
}

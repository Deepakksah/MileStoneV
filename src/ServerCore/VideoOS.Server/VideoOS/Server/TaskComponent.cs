using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class TaskComponent : VmoResourceComponent, ITaskComponent, IDisposable
{
	private static class TaskColumns
	{
		private static DatabaseColumnBinder TaskId = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder DataA = new DatabaseColumnBinder("data_a");

		private static DatabaseColumnBinder DataB = new DatabaseColumnBinder("data_b");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("start_time");

		private static DatabaseColumnBinder StatusDetails = new DatabaseColumnBinder("status_details");

		private static DatabaseColumnBinder TaskStatus = new DatabaseColumnBinder("task_status");

		public static TaskHandling.RawServerTask Bind(DatabaseDataReader reader, ISymmetricEncryption symmetricEncryption)
		{
			string type = Type.GetString(reader);
			TaskHandling.RawServerTask rawServerTask = new TaskHandling.RawServerTask
			{
				SequenceNumber = TaskId.GetInt64(reader),
				Type = type,
				DataA = XmlEncryptionHelper.DecryptXml(type, DataA.GetString(reader), (string value, byte[] salt) => symmetricEncryption.DecryptFromBase64String(value)),
				DataB = XmlEncryptionHelper.DecryptXml(type, DataB.GetString(reader, allowNulls: true), (string value, byte[] salt) => symmetricEncryption.DecryptFromBase64String(value)),
				StartTime = StartTime.GetDateTime(reader),
				TaskStatus = TaskStatus.GetInt32(reader)
			};
			try
			{
				rawServerTask.StatusDetails = symmetricEncryption.DecryptFromBase64String(StatusDetails.GetString(reader));
			}
			catch (FormatException)
			{
				rawServerTask.StatusDetails = StatusDetails.GetString(reader);
			}
			return rawServerTask;
		}
	}

	private ISymmetricEncryption _symmetricEncryption;

	public TaskComponent(ISymmetricEncryption symmetricEncryption)
	{
		_symmetricEncryption = symmetricEncryption;
	}

	public TaskHandling.RawServerTask[] GetPendingTasks(Guid serviceId, long sequenceNumber)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_pending_tasks_for_target");
			BindGuidType("@target_id", serviceId);
			BindLong("@sequence_number", sequenceNumber);
			BindBoolean("@delete_old", parameterValue: true);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<TaskHandling.RawServerTask> list = new List<TaskHandling.RawServerTask>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, _symmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public TaskHandling.RawServerTask[] GetTasksByType(Guid serviceId, string type)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_task_by_target_and_type");
			BindGuidType("@target_id", serviceId);
			BindString("@type", type, 50, allowNull: false, SqlDbType.VarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<TaskHandling.RawServerTask> list = new List<TaskHandling.RawServerTask>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, _symmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public TaskHandling.RawServerTask[] GetTasksWithPendingStop(Guid serviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_stopping_tasks_for_target");
			BindGuidType("@target_id", serviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<TaskHandling.RawServerTask> list = new List<TaskHandling.RawServerTask>();
			while (databaseDataReader.Read())
			{
				list.Add(TaskColumns.Bind(databaseDataReader, _symmetricEncryption));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public long CreateTask(string type, Guid serviceId, string data_a, int percentComplete, DateTime startTime)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_task");
			BindString("@type", type, 50, allowNull: false, SqlDbType.VarChar);
			BindGuidType("@target", serviceId);
			BindXml("@data_a", XmlEncryptionHelper.EncryptXml(type, data_a, (string value, byte[] salt) => _symmetricEncryption.EncryptToBase64String(value)), allowNull: false);
			BindInt("@percent_completed", percentComplete);
			BindDateTime("@start_time", startTime);
			return Convert.ToInt64(ExecuteScalar(), NumberFormatInfo.InvariantInfo);
		});
	}

	public void RemoveTask(long taskId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_task");
			BindLong("@pkid", taskId);
			ExecuteNonQuery();
		});
	}

	public string UpdateTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_task");
			BindLong("@pkid", taskId);
			BindInt("@percent_completed", percentComplete);
			BindStringMax("@status_details", _symmetricEncryption.EncryptToBase64String(details), allowNull: true);
			BindInt("@task_status", status);
			if (endTime == DateTime.MinValue)
			{
				BindNullValue("@end_time", SqlDbType.DateTime);
			}
			else
			{
				BindDateTime("@end_time", endTime);
			}
			return (string)ExecuteScalar();
		});
	}

	public string StopTask(long taskId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stop_task");
			BindLong("@pkid", taskId);
			return (string)ExecuteScalar();
		});
	}

	public void RemoveTasksByDeviceIds(Guid[] deviceIds)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_task_by_deviceids");
			BindIdTable("@IDDevices", new List<Guid>(deviceIds).ConvertAll((Guid x) => x.ToString()).ToArray());
			ExecuteNonQuery();
		});
	}
}

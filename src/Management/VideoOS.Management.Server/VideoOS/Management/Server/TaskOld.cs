using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "TaskInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class TaskOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Type { get; set; }

	[DataMember]
	public string Target { get; set; }

	[DataMember]
	public string Information { get; set; }

	[DataMember]
	public AsyncStatus AsyncStatus { get; set; }

	[DataMember]
	public string AsyncOperation { get; set; }

	[DataMember]
	public DateTime StartTime { get; set; }

	[DataMember]
	public DateTime EndTime { get; set; }

	[DataMember]
	public int PercentCompleted { get; set; }

	[DataMember]
	public KeyValue[] TaskProperties { get; set; }

	public string DataA { get; set; }

	public static TaskOld[] EnableRecordingStorageEncryption(IGeminiRequestContext requestContext, string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		return ConvertToTaskOlds(Task.EnableRecordingStorageEncryption(requestContext, recordingStorageId, encryptionMethod, password));
	}

	public static TaskOld[] DisableRecordingStorageEncryption(IGeminiRequestContext requestContext, string recordingStorageId, string password)
	{
		return ConvertToTaskOlds(Task.DisableRecordingStorageEncryption(requestContext, recordingStorageId, password));
	}

	private static TaskOld ConvertToTaskOld(Task task)
	{
		return new TaskOld
		{
			Id = task.Id,
			Type = task.TypeDb,
			Target = task.Target,
			Information = task.Information,
			AsyncStatus = new AsyncStatus
			{
				ExceptionStatus = task.AsyncStatus.ExceptionStatus,
				LastException = task.AsyncStatus.LastException
			},
			AsyncOperation = task.AsyncOperation,
			StartTime = task.StartTime,
			EndTime = task.EndTime,
			PercentCompleted = task.PercentCompleted,
			TaskProperties = task.TaskProperties.ToArray()
		};
	}

	private static TaskOld[] ConvertToTaskOlds(Task[] tasks)
	{
		List<TaskOld> list = new List<TaskOld>();
		foreach (Task task in tasks)
		{
			list.Add(ConvertToTaskOld(task));
		}
		return list.ToArray();
	}
}

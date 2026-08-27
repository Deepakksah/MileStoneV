using System;

namespace VideoOS.Common.Integration.Threading;

public class TaskState
{
	public string TaskID { get; set; }

	public string TaskType { get; set; }

	public object TaskData { get; set; }

	public TaskResultStatus ResultStatus { get; set; }

	public Exception TaskException { get; set; }

	public TaskState(string taskID, string taskType, object taskData)
	{
		TaskID = taskID;
		TaskType = taskType;
		TaskData = taskData;
	}
}

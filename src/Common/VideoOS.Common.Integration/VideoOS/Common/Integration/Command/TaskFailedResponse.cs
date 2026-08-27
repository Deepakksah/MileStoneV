using System;
using VideoOS.Management.Common.Tasks;

namespace VideoOS.Common.Integration.Command;

public class TaskFailedResponse : TaskResponseBase
{
	public bool IsFatal { get; private set; }

	public string Description { get; private set; }

	public string StatusDetails { get; private set; }

	public TaskFailedResponse(bool isFatal, string description)
	{
		IsFatal = isFatal;
		Description = description;
		StatusDetails = description;
	}

	public TaskFailedResponse(bool isFatal, string description, TaskError taskError)
		: this(isFatal, description)
	{
		if (taskError != null)
		{
			StatusDetails = taskError.ToXml();
		}
	}

	public TaskFailedResponse(bool isFatal, string description, Exception exception)
		: this(isFatal, description)
	{
		if (exception != null)
		{
			StatusDetails = exception.Message;
		}
	}
}

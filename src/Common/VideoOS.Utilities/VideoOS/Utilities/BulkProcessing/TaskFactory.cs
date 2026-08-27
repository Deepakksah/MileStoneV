using System;
using System.Threading.Tasks;

namespace VideoOS.Utilities.BulkProcessing;

public class TaskFactory : ITaskFactory
{
	public void StartTask(Action action)
	{
		Task.Run(action);
	}
}

using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration;

public class TaskFactory : ITaskFactory
{
	public void StartTask(Action action)
	{
		Task.Run(action);
	}
}

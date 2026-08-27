using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Integration.Tasks;

public interface ITaskManager
{
	bool HasRunningTasks { get; }

	WaitHandle StopEvent { get; }

	ILogger Logger { get; }

	void Start();

	void Stop();

	void CheckForStoppingTasks();

	void CheckForTasksAvailable();
}

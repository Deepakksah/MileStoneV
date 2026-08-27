using System;
using System.Collections.Generic;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server;

public interface IAsyncTaskMonitor
{
	bool Started { get; }

	bool Initialized { get; }

	bool Finished { get; }

	object ContextState { get; set; }

	event EventHandler TasksCompleted;

	event EventHandler TimeoutElapsed;

	event EventHandler<IAsyncTask> TaskCompleted;

	void Start(TimeSpan timeout);

	void Initialize(IEnumerable<IAsyncTask> tasks);
}

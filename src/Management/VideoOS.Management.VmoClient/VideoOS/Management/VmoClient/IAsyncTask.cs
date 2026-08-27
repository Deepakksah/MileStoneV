using System;

namespace VideoOS.Management.VmoClient;

public interface IAsyncTask
{
	Guid Target { get; }

	int PercentCompleted { get; }

	string Name { get; }

	string EntityName { get; }

	AsyncTaskStatus Status { get; }

	void Cleanup();

	void Refresh();
}

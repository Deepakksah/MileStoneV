using System;

namespace VideoOS.Common.Integration.Command;

public interface IStoppableTaskCommand : ITaskCommandBase
{
	bool Stopping { get; }

	event EventHandler<StoppableTaskCommandEventArgs> TaskStopped;

	void StopTask();
}

using System;

namespace VideoOS.Common.Integration.Command;

public class StoppableTaskCommandEventArgs : EventArgs
{
	private long _taskId;

	public long TaskId => _taskId;

	public StoppableTaskCommandEventArgs(long taskId)
	{
		_taskId = taskId;
	}
}

using System;

namespace VideoOS.Utilities.BulkProcessing;

public interface ITaskFactory
{
	void StartTask(Action action);
}

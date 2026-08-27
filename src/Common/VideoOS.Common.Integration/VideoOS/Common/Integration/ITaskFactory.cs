using System;

namespace VideoOS.Common.Integration;

public interface ITaskFactory
{
	void StartTask(Action action);
}

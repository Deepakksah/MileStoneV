using System;

namespace VideoOS.Common.EnvironmentServices;

public interface ISynchronizationService
{
	bool IsSynchronizationRequired();

	void Invoke(Action action);
}

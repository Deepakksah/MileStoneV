using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace VideoOS.Common.EnvironmentServices;

[ExcludeFromCodeCoverage]
public class SynchronizationService : ISynchronizationService
{
	public bool IsSynchronizationRequired()
	{
		return Dispatcher.CurrentDispatcher.CheckAccess();
	}

	public void Invoke(Action action)
	{
		Dispatcher.CurrentDispatcher.Invoke(action);
	}
}

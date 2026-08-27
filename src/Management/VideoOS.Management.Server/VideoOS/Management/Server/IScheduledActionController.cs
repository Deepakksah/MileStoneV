using System;

namespace VideoOS.Management.Server;

public interface IScheduledActionController<T> : IDisposable
{
	void UpdateSchedule(IScheduledActionConfigurationProvider<T> scheduleActionConfigurationProvider);
}

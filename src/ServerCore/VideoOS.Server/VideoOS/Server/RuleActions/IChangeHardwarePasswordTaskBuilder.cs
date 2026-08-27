using System;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.RuleActions;

public interface IChangeHardwarePasswordTaskBuilder : IDisposable
{
	IAsyncTask CreateChangeHardwarePasswordTask(Guid hardwareId, string username, string password);
}

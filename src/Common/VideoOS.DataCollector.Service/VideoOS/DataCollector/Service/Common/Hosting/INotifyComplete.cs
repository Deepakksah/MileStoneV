using System;
using System.Threading;

namespace VideoOS.DataCollector.Service.Common.Hosting;

public interface INotifyComplete : IDisposable
{
	CancellationToken StartupComplete { get; }
}

using System;
using System.Threading;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public interface IMessageManagerResponseCollector : IDisposable
{
	ResponseBase GetNextResponse(TimeSpan timeout);

	ResponseBase GetNextResponse(TimeSpan timeout, CancellationToken cancellationToken);
}

using System;
using System.Threading;

namespace VideoOS.Common.Integration.Command;

public interface IResponseCollector
{
	ResponseBase GetNextResponse(TimeSpan timeout);

	ResponseBase GetNextResponse(TimeSpan timeout, CancellationToken cancellationToken);
}

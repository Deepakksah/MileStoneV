using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.DataCollector.Service.Common.BackOff;

public interface IBackOffProvider
{
	int RetryDelayMilliseconds { get; set; }

	int MaxRetryDelayMilliseconds { get; set; }

	Task Delay(CancellationToken cancellationToken);
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.DataCollector.Service.Common.BackOff;

public class ExponentialBackOff : IBackOffProvider
{
	private int _currentDelayMilliseconds;

	private int _retries;

	public int RetryDelayMilliseconds { get; set; }

	public int MaxRetryDelayMilliseconds { get; set; }

	public Task Delay(CancellationToken cancellationToken)
	{
		_retries++;
		if (_currentDelayMilliseconds <= MaxRetryDelayMilliseconds)
		{
			_currentDelayMilliseconds = Convert.ToInt32(Math.Floor((double)RetryDelayMilliseconds * Math.Pow(2.0, _retries - 1)));
		}
		int delay = Math.Min(_currentDelayMilliseconds, MaxRetryDelayMilliseconds);
		return TaskDelay(delay, cancellationToken);
	}

	public virtual Task TaskDelay(int delay, CancellationToken cancellationToken)
	{
		return Task.Delay(delay, cancellationToken);
	}
}

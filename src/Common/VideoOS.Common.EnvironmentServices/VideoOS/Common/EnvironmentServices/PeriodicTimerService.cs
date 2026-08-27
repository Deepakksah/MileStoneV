using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices;

public class PeriodicTimerService : IPeriodicTimerService, IDisposable
{
	private AsyncAutoResetEvent _timerSyncEvent = new AsyncAutoResetEvent();

	private Timer _timer;

	private bool _disposedValue;

	public PeriodicTimerService(TimeSpan period)
	{
		if (period.TotalMilliseconds < 0.0 || period.TotalMilliseconds >= 4294967295.0)
		{
			throw new ArgumentOutOfRangeException("period");
		}
		_timer = new Timer(delegate
		{
			_timerSyncEvent.Set();
		}, null, TimeSpan.Zero, period);
	}

	public Task<bool> WaitForNextTickAsync()
	{
		return WaitForNextTickAsync(default(CancellationToken));
	}

	public async Task<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
	{
		try
		{
			await _timerSyncEvent.WaitAsync(cancellationToken);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			_timer.Dispose();
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

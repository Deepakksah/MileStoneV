using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Management.Server;

public class ScheduledActionController<T> : IScheduledActionController<T>, IDisposable
{
	private int _updates;

	private int _updating;

	private CancellationTokenSource _disposeTokenSource;

	private IScheduledAction<T> _scheduleAction;

	public ScheduledActionController(IScheduledAction<T> scheduleAction, ICancellationTokenSourceFactory tokenSourceFactory)
	{
		_scheduleAction = scheduleAction;
		_disposeTokenSource = tokenSourceFactory.CreateCancellationTokenSource();
	}

	public ScheduledActionController(IScheduledAction<T> scheduleAction)
	{
		_scheduleAction = scheduleAction;
		_disposeTokenSource = new CancellationTokenSource();
	}

	public async void UpdateSchedule(IScheduledActionConfigurationProvider<T> scheduleActionConfigurationProvider)
	{
		RegisterScheduleUpdate();
		if (!IsNotUpdating())
		{
			return;
		}
		StartUpdating();
		try
		{
			_scheduleAction.StopSchedule();
			T val = await scheduleActionConfigurationProvider.GetActionConfigurationAsync(_disposeTokenSource.Token);
			if (!EqualityComparer<T>.Default.Equals(val, default(T)))
			{
				_scheduleAction.StartSchedule(val);
			}
		}
		catch (OperationCanceledException)
		{
		}
		finally
		{
			DoneUpdating();
		}
		RerunIfUpdatesAreRegistered(scheduleActionConfigurationProvider);
	}

	private void RerunIfUpdatesAreRegistered(IScheduledActionConfigurationProvider<T> scheduleActionConfigurationProvider)
	{
		if (Interlocked.Exchange(ref _updates, 0) > 0)
		{
			UpdateSchedule(scheduleActionConfigurationProvider);
		}
	}

	private void RegisterScheduleUpdate()
	{
		Interlocked.Increment(ref _updates);
	}

	private bool IsNotUpdating()
	{
		return Interlocked.Exchange(ref _updating, 1) == 0;
	}

	private void DoneUpdating()
	{
		Interlocked.Decrement(ref _updating);
	}

	private void StartUpdating()
	{
		Interlocked.Exchange(ref _updates, 0);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _disposeTokenSource != null)
		{
			_disposeTokenSource.Cancel();
			_disposeTokenSource.Dispose();
			_disposeTokenSource = null;
		}
	}
}

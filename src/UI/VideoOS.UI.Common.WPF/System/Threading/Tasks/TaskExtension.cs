using System.Collections.Concurrent;
using System.Windows;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace System.Threading.Tasks;

public static class TaskExtension
{
	private class AsynchronousRegistration
	{
		private readonly object _lock = new object();

		private readonly Func<CancellationToken, Task> _asyncAction;

		private readonly int _delayMilliseconds;

		private CancellationTokenSource _cancellationSource;

		private Task _previousExecution;

		private TaskCompletionSource<object> _tcs;

		public AsynchronousRegistration(Func<CancellationToken, Task> asyncAction, int delayMilliseconds)
		{
			_delayMilliseconds = delayMilliseconds;
			_asyncAction = asyncAction;
		}

		public Task Execute()
		{
			lock (_lock)
			{
				if (_tcs == null || _tcs.Task.Status != TaskStatus.WaitingForActivation)
				{
					_tcs = new TaskCompletionSource<object>();
				}
				if (_cancellationSource != null)
				{
					_cancellationSource.Cancel();
				}
				_cancellationSource = new CancellationTokenSource();
				_previousExecution = Execute(_previousExecution, _tcs, _cancellationSource.Token);
				return _tcs.Task;
			}
		}

		private async Task Execute(Task previousAttempt, TaskCompletionSource<object> tcs, CancellationToken token)
		{
			await Task.Delay(_delayMilliseconds);
			if (previousAttempt != null)
			{
				await previousAttempt;
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			await _asyncAction(token);
			lock (_lock)
			{
				if (!token.IsCancellationRequested)
				{
					tcs.SetResult(null);
				}
			}
		}
	}

	private class SynchronousRegistration
	{
		private readonly TaskScheduler _scheduler;

		private readonly Action _action;

		private int _isInvalidated;

		public SynchronousRegistration(TaskScheduler scheduler, Action action)
		{
			_scheduler = scheduler;
			_action = action;
		}

		public void Invalidate()
		{
			Interlocked.Exchange(ref _isInvalidated, 1);
			Task.Factory.StartNew(Update, CancellationToken.None, TaskCreationOptions.None, _scheduler);
		}

		public void Update()
		{
			if (Interlocked.Exchange(ref _isInvalidated, 0) > 0)
			{
				_action();
			}
		}
	}

	private static readonly ConcurrentDictionary<Action, SynchronousRegistration> _syncRegistrations = new ConcurrentDictionary<Action, SynchronousRegistration>();

	private static readonly ConcurrentDictionary<Func<CancellationToken, Task>, AsynchronousRegistration> _asyncRegistrations = new ConcurrentDictionary<Func<CancellationToken, Task>, AsynchronousRegistration>();

	public static Task RunOnCurrentSynchronizationContext(this TaskFactory @this, Action action)
	{
		Task task = null;
		if (SynchronizationContext.Current == null)
		{
			Application.Current.Dispatcher.InvokeOrCall(delegate
			{
				task = RunTask(@this, action);
			});
		}
		else
		{
			task = RunTask(@this, action);
		}
		return task;
	}

	private static Task RunTask(TaskFactory @this, Action action)
	{
		return @this.StartNew(action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
	}

	public static void ExecuteOnceDeferred(this TaskScheduler @this, Action updateAction)
	{
		_syncRegistrations.GetOrAdd(updateAction, (Action ua) => new SynchronousRegistration(@this, ua)).Invalidate();
	}

	public static Task ExecuteOnceDeferred(this TaskFactory @this, Func<CancellationToken, Task> action, int delayMilliseconds)
	{
		return _asyncRegistrations.GetOrAdd(action, (Func<CancellationToken, Task> a) => new AsynchronousRegistration(a, delayMilliseconds)).Execute();
	}
}

using System.Collections.Generic;

namespace System.Threading.Tasks;

public static class TaskExtension
{
	public static Task ToAsync(this Task @this, AsyncCallback callback, object state)
	{
		if (@this == null)
		{
			throw new ArgumentNullException("@this");
		}
		TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state);
		@this.ContinueWith(delegate
		{
			tcs.SetFromTask(@this);
			if (callback != null)
			{
				callback(tcs.Task);
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		return tcs.Task;
	}

	public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> @this, Task task)
	{
		if (@this == null)
		{
			throw new ArgumentNullException("@this");
		}
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		Task<TResult> task2 = task as Task<TResult>;
		switch (task.Status)
		{
		case TaskStatus.RanToCompletion:
			@this.SetResult((task2 != null) ? task2.Result : default(TResult));
			break;
		case TaskStatus.Faulted:
			@this.SetException(task.Exception.InnerExceptions);
			break;
		case TaskStatus.Canceled:
			@this.SetCanceled();
			break;
		default:
			throw new InvalidOperationException("The task was not completed.");
		}
	}

	public static Task Delay(this TaskFactory @this, TimeSpan dueTime, CancellationToken cancellation)
	{
		if (@this == null)
		{
			throw new ArgumentNullException("@this");
		}
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(@this.CreationOptions);
		Timer timer = new Timer(delegate
		{
			tcs.TrySetResult(result: true);
		}, null, dueTime, TimeSpan.Zero);
		CancellationTokenRegistration tokenRegistration = cancellation.Register(delegate
		{
			tcs.TrySetCanceled();
		});
		tcs.Task.ContinueWith(delegate
		{
			tokenRegistration.Dispose();
			timer.Dispose();
		}, TaskContinuationOptions.ExecuteSynchronously);
		return tcs.Task;
	}

	public static Task Delay(this TaskFactory @this, TimeSpan dueTime)
	{
		return @this.Delay(dueTime, CancellationToken.None);
	}

	public static Task RunAsync(this IEnumerable<Task> asyncMethod, bool propagateExceptions)
	{
		IEnumerator<Task> enumerator = asyncMethod.GetEnumerator();
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		tcs.Task.ContinueWith(delegate
		{
			enumerator.Dispose();
		});
		Action<Task> enumeratorStep = null;
		enumeratorStep = delegate(Task previous)
		{
			TaskStatus taskStatus = previous?.Status ?? TaskStatus.RanToCompletion;
			bool flag = false;
			switch (taskStatus)
			{
			case TaskStatus.Canceled:
				if (propagateExceptions)
				{
					tcs.SetCanceled();
				}
				else
				{
					tcs.SetResult(result: false);
				}
				break;
			case TaskStatus.Faulted:
				if (propagateExceptions)
				{
					tcs.SetException(previous.Exception);
				}
				else
				{
					flag = true;
				}
				break;
			case TaskStatus.RanToCompletion:
				flag = true;
				break;
			}
			if (flag)
			{
				try
				{
					if (enumerator.MoveNext())
					{
						enumerator.Current.ContinueWith(enumeratorStep);
					}
					else
					{
						tcs.SetResult(result: true);
					}
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			}
		};
		enumeratorStep(null);
		return tcs.Task;
	}

	public static Task RunAsync(this IEnumerable<Task> asyncMethod)
	{
		return asyncMethod.RunAsync(propagateExceptions: false);
	}
}

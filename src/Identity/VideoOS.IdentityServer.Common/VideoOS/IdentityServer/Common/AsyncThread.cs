using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common;

public abstract class AsyncThread<T>
{
	private volatile bool _checkCancel;

	private ThreadPriority _priority;

	private string _threadName;

	public AsyncThread(ThreadPriority priority, string threadName)
	{
		_priority = priority;
		_threadName = threadName;
	}

	public abstract Task<T> Run();

	public abstract Task<T> Run(CancellationToken cancellationToken);

	protected async Task<T> Run(Func<T> func, CancellationToken cancellationToken)
	{
		T value = default(T);
		Exception exception = null;
		Thread thread = new Thread((ThreadStart)delegate
		{
			try
			{
				value = func();
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex3)
			{
				exception = ex3;
			}
		});
		try
		{
			thread.Name = _threadName;
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = true;
			thread.Priority = _priority;
			thread.Start();
			_checkCancel = true;
			Task task = CheckCancelTask(thread, cancellationToken);
			thread.Join();
			_checkCancel = false;
			await task.ConfigureAwait(continueOnCapturedContext: false);
			cancellationToken.ThrowIfCancellationRequested();
			if (exception != null)
			{
				throw exception;
			}
			return value;
		}
		catch (AggregateException ex)
		{
			ex.Handle((Exception h) => true);
			return value;
		}
	}

	private async Task CheckCancelTask(Thread thread, CancellationToken cancellationToken)
	{
		while (_checkCancel)
		{
			await Task.Delay(100, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			if (cancellationToken.IsCancellationRequested)
			{
				thread.Abort();
				break;
			}
		}
	}
}

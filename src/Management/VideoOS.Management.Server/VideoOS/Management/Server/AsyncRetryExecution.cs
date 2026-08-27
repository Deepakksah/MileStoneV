using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public static class AsyncRetryExecution
{
	public static async System.Threading.Tasks.Task ExecuteWithRetry(Action action, CancellationToken cancellationToken, string retryMessage, int millisecondsDelay, params Type[] exceptions)
	{
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		await ExecuteWithRetry(delegate
		{
			action();
			return true;
		}, cancellationToken, retryMessage, millisecondsDelay, exceptions);
	}

	public static async Task<U> ExecuteWithRetry<U>(Func<U> func, CancellationToken cancellationToken, string retryMessage, int millisecondsDelay, params Type[] exceptions)
	{
		if (func == null)
		{
			throw new ArgumentNullException("func");
		}
		if (millisecondsDelay < 0)
		{
			throw new ArgumentException("The delay must be 0 or higher", "millisecondsDelay");
		}
		if (exceptions == null)
		{
			throw new ArgumentNullException("exceptions");
		}
		if (exceptions.Length == 0)
		{
			throw new ArgumentException("No exception types have been provided", "exceptions");
		}
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			return func();
		}
		catch (Exception ex)
		{
			if (!exceptions.Contains(ex.GetType()))
			{
				GeminiTrace.Error("Retention Time Rule failed and will be cancelled", ex);
				throw new TaskCanceledException();
			}
			GeminiTrace.Error(retryMessage, ex);
		}
		await System.Threading.Tasks.Task.Delay(millisecondsDelay, cancellationToken);
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			return func();
		}
		catch (Exception ex2)
		{
			if (!exceptions.Contains(ex2.GetType()))
			{
				GeminiTrace.Error("Retention Time Rule failed and will be cancelled", ex2);
			}
		}
		throw new TaskCanceledException();
	}
}

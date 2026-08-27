using System;
using System.Threading.Tasks;

namespace VideoOS.Platform.Common.MessageClient;

public static class TaskExtensions
{
	public static async Task<T> TimeoutAfter<T>(this Task<T> task, string title, TimeSpan timeout)
	{
		if (task == await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(continueOnCapturedContext: false))
		{
			return await task.ConfigureAwait(continueOnCapturedContext: false);
		}
		throw new TimeoutException($"Timed out on task: '{title}' after [{timeout}]");
	}
}

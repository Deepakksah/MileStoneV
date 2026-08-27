using System;
using System.Threading.Tasks;

namespace VideoOS.Central.Util;

public class DelayedExecutor
{
	public static async Task<bool> DoAsync(Func<bool> successAction, IDelayedExecutorSettings settings, bool inTask = true)
	{
		if (successAction())
		{
			return true;
		}
		if (DateTime.UtcNow - settings.StartTimeUtc < settings.MaxDuration)
		{
			if (inTask)
			{
				return await Task.Run(async () => await RecursiveActionAsync().ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
			}
			return await RecursiveActionAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		return false;
		async Task<bool> RecursiveActionAsync()
		{
			await Task.Delay(settings.Delay).ConfigureAwait(continueOnCapturedContext: false);
			return await DoAsync(successAction, settings, inTask: false).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}

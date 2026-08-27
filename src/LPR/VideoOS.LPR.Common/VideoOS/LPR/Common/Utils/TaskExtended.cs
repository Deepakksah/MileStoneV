using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.LPR.Common.Utils;

public static class TaskExtended
{
	public static async Task<TRes[]> WhenAll<TRes>(IEnumerable<Task<TRes>> tasks, CancellationToken cancellationToken)
	{
		using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		Task<TRes> cancellationTask = DelayResult(-1, default(TRes), linkedCts.Token);
		try
		{
			return await Task.WhenAll(tasks.Select(async (Task<TRes> t) => await (await Task.WhenAny<TRes>(t, cancellationTask).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false))).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			linkedCts.Cancel();
		}
	}

	public static async Task<Task<TRes>> WhenAny<TRes>(IEnumerable<Task<TRes>> tasks, CancellationToken cancellationToken)
	{
		using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		Task<TRes> cancellationTask = DelayResult(-1, default(TRes), linkedCts.Token);
		IEnumerable<Task<TRes>> tasks2 = tasks.Concat(new Task<TRes>[1] { cancellationTask });
		try
		{
			Task<TRes> task = await Task.WhenAny(tasks2).ConfigureAwait(continueOnCapturedContext: false);
			if (task == cancellationTask)
			{
				await task.ConfigureAwait(continueOnCapturedContext: false);
			}
			return task;
		}
		finally
		{
			linkedCts.Cancel();
		}
	}

	public static async Task<TRes> DelayResult<TRes>(int millisecondsDelay, TRes value, CancellationToken cancellationToken)
	{
		await Task.Delay(millisecondsDelay, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return value;
	}
}

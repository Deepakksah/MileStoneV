using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace VideoOS.DataCollector.Service.Common.Hosting;

public class WaitForAppStartupService
{
	public async Task<bool> WaitForApplicationStartedAsync(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
	{
		TaskCompletionSource startedSource = new TaskCompletionSource();
		TaskCompletionSource cancelledSource = new TaskCompletionSource();
		bool result;
		await using (lifetime.ApplicationStarted.Register(delegate
		{
			startedSource.SetResult();
		}))
		{
			bool flag;
			await using (stoppingToken.Register(delegate
			{
				cancelledSource.SetResult();
			}))
			{
				flag = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(continueOnCapturedContext: false) == startedSource.Task;
			}
			result = flag;
		}
		return result;
	}
}

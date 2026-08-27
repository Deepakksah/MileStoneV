using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace VideoOS.DataCollector.Service.Common.Hosting;

public abstract class WaitForNotifyCompleteBase : BackgroundService
{
	private readonly TaskCompletionSource _startCompletionSource = new TaskCompletionSource();

	protected WaitForNotifyCompleteBase(INotifyComplete applicationStartup)
	{
		applicationStartup.StartupComplete.Register(delegate
		{
			_startCompletionSource.SetResult();
		});
	}

	public virtual async Task WaitForNotifyComplete(CancellationToken stoppingToken)
	{
		TaskCompletionSource cancelledSource = new TaskCompletionSource();
		await using (stoppingToken.Register(delegate
		{
			cancelledSource.SetResult();
		}))
		{
			await Task.WhenAny(_startCompletionSource.Task, cancelledSource.Task).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}

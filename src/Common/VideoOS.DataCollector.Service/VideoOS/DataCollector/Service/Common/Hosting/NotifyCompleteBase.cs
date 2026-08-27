using System.Threading;
using Microsoft.Extensions.Hosting;

namespace VideoOS.DataCollector.Service.Common.Hosting;

public abstract class NotifyCompleteBase : BackgroundService
{
	private bool _disposedValue;

	private readonly NotifyCompleteService? _applicationStartup;

	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private readonly string _cancellationName;

	protected NotifyCompleteBase(INotifyComplete applicationStartup)
	{
		_cancellationName = GetType().Name;
		_applicationStartup = applicationStartup as NotifyCompleteService;
		_applicationStartup?.RegisterSource(_cancellationName, _cancellationTokenSource);
	}

	public virtual void NotifyComplete()
	{
		_applicationStartup?.NotifyComplete(_cancellationName);
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_cancellationTokenSource.Dispose();
			}
			_disposedValue = true;
		}
		base.Dispose();
	}
}

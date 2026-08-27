using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.DataCollector.Service.Common.Hosting;

public class NotifyCompleteService : INotifyComplete, IDisposable
{
	private readonly CancellationTokenSource _startupSource = new CancellationTokenSource();

	private readonly ILogger<NotifyCompleteService> _logger;

	private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationSources = new ConcurrentDictionary<string, CancellationTokenSource>();

	public CancellationToken StartupComplete => _startupSource.Token;

	public NotifyCompleteService(ILogger<NotifyCompleteService> logger)
	{
		_logger = logger;
	}

	public void RegisterSource(string name, CancellationTokenSource source)
	{
		if (!_cancellationSources.TryAdd(name, source))
		{
			throw new InvalidOperationException("An error occurred register notify event.");
		}
	}

	public void NotifyComplete(string name)
	{
		if (!_cancellationSources.TryGetValue(name, out CancellationTokenSource value))
		{
			throw new InvalidOperationException("An error occurred notify complete event.");
		}
		try
		{
			ExecuteHandlers(value);
			if (_cancellationSources.All<KeyValuePair<string, CancellationTokenSource>>((KeyValuePair<string, CancellationTokenSource> c) => c.Value.IsCancellationRequested))
			{
				ExecuteHandlers(_startupSource);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError("An error occurred starting the application: {ex}", ex);
			throw;
		}
	}

	private static void ExecuteHandlers(CancellationTokenSource cancel)
	{
		if (!cancel.IsCancellationRequested)
		{
			cancel.Cancel(throwOnFirstException: false);
		}
	}

	public void Dispose()
	{
		_startupSource.Dispose();
	}
}

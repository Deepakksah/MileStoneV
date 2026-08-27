using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Services;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;

public class ConfigurationCache : IConfigurationCache, IDisposable
{
	private const string DataCollectorIdKey = "_DataCollectorId";

	private readonly ClusterOptions _options;

	private readonly IMemoryCache _cache;

	private readonly IMediator _mediator;

	private readonly SemaphoreSlim _dataCollectorIdLock = new SemaphoreSlim(1, 1);

	private readonly IApplicationIdService _applicationIdService;

	private readonly WaitForAppStartupService _waitForAppStartupService;

	private readonly IHostApplicationLifetime _lifetime;

	public ConfigurationCache(IMemoryCache cache, IMediator mediator, IOptions<ClusterOptions> options, IApplicationIdService applicationIdService, WaitForAppStartupService waitForAppStartupService, IHostApplicationLifetime lifetime)
	{
		_cache = cache;
		_mediator = mediator;
		_options = options.Value;
		_applicationIdService = applicationIdService;
		_waitForAppStartupService = waitForAppStartupService;
		_lifetime = lifetime;
	}

	public async Task<int> GetOrAddDataCollectorIdAsync(CancellationToken cancellationToken)
	{
		if (_cache.TryGetValue<int>("_DataCollectorId", out var value))
		{
			return value;
		}
		await _dataCollectorIdLock.WaitAsync(cancellationToken);
		int value2;
		try
		{
			if (_cache.TryGetValue<int>("_DataCollectorId", out value))
			{
				return value;
			}
			if (!(await _waitForAppStartupService.WaitForApplicationStartedAsync(_lifetime, cancellationToken)))
			{
				return 0;
			}
			RegisterDataCollectorServerCommand request = new RegisterDataCollectorServerCommand
			{
				ApplicationId = _applicationIdService.ApplicationId,
				ClusterAddress = _options.ClusterAddress,
				ClusterApplicationId = _options.ClusterApplicationId,
				DataCollectorVersion = Convert.ToInt32("25", CultureInfo.InvariantCulture),
				HostName = (string.IsNullOrEmpty(_options.ClusterAddress) ? Dns.GetHostEntry(Dns.GetHostName()).HostName : _options.ClusterAddress)
			};
			value2 = (await _mediator.Send((IRequest<Result<int>>)request, cancellationToken)).Value;
			_cache.Set("_DataCollectorId", value2, new MemoryCacheEntryOptions
			{
				Priority = CacheItemPriority.NeverRemove
			});
		}
		finally
		{
			_dataCollectorIdLock.Release();
		}
		return value2;
	}

	public void Dispose()
	{
		_cache.Dispose();
		_dataCollectorIdLock.Dispose();
	}
}

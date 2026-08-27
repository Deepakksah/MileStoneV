using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.Sample.EventHandlers.Historical;
using VideoOS.DataCollector.Service.Sample.Services;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class BatchSenderService : WaitForNotifyCompleteBase
{
	private readonly IMediator _mediator;

	private readonly IConfigurationCache _configurationCache;

	private readonly IPerformanceCounterValueChannel _batchSenderChannel;

	private readonly ILogger<BatchSenderService> _logger;

	private readonly PerformanceCounterOptions _options;

	public BatchSenderService(IMediator mediator, IConfigurationCache configurationCache, IPerformanceCounterValueChannel batchSenderChannel, ILogger<BatchSenderService> logger, INotifyComplete applicationStartup, IOptions<PerformanceCounterOptions> options)
		: base(applicationStartup)
	{
		_mediator = mediator;
		_configurationCache = configurationCache;
		_batchSenderChannel = batchSenderChannel;
		_logger = logger;
		_options = options.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await WaitForNotifyComplete(stoppingToken);
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				List<PerformanceCounterValue> batch = new List<PerformanceCounterValue>();
				await foreach (PerformanceCounterValue item in _batchSenderChannel.ReadChannelValues(stoppingToken))
				{
					batch.Add(item);
					if (batch.Count == _options.BatchSize || !_batchSenderChannel.ReaderTryPeek(out PerformanceCounterValue _))
					{
						_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Sending batch to server, batch size: {0}, items remaining in queue: {1}", batch.Count, _batchSenderChannel.ReaderCount));
						SendBatchToServerServerCommand request = new SendBatchToServerServerCommand(await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken), batch.ToArray());
						await _mediator.Send((IRequest<Result<Unit>>)request, stoppingToken);
						batch.Clear();
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception value2)
		{
			_logger.LogError($"{value2}");
			Exit();
		}
	}

	public virtual void Exit()
	{
		Environment.Exit(1);
	}
}

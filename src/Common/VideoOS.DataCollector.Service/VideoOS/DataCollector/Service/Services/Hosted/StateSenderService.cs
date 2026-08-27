using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;
using VideoOS.DataCollector.Service.Sample.Services.Rule;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class StateSenderService : BackgroundService
{
	private readonly IMediator _mediator;

	private readonly IRuleStateChangeChannel _stateChangeSenderChannel;

	private readonly ILogger<StateSenderService> _logger;

	public StateSenderService(IMediator mediator, IRuleStateChangeChannel stateChangeSenderChannel, ILogger<StateSenderService> logger)
	{
		_mediator = mediator;
		_stateChangeSenderChannel = stateChangeSenderChannel;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_ = 2;
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				StateChangeData stateChangeData = await _stateChangeSenderChannel.ReadAsync(stoppingToken);
				DeleteStateChangeData deleteStateChangeData = stateChangeData as DeleteStateChangeData;
				if (deleteStateChangeData != null)
				{
					await _mediator.Send((IRequest<Result<Unit>>)new DeleteStateServerCommand(deleteStateChangeData), stoppingToken);
				}
				else
				{
					await _mediator.Send((IRequest<Result<Unit>>)new StateChangeServerCommand(stateChangeData), stoppingToken);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception value)
		{
			_logger.LogError($"{value}");
			Exit();
		}
	}

	public virtual void Exit()
	{
		Environment.Exit(1);
	}
}

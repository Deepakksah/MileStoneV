using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.DomainEventInfrastructure.Monitoring;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxProcessor(IServiceProvider services, ILogger<OutboxProcessor> logger, IOptions<OutboxProcessorOptions> options, ITransientExceptionDetector transientExceptionDetector, IOutboxMetricsTracker monitor) : BackgroundService
{
	private readonly string _instanceId = Guid.NewGuid().ToString();

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("Outbox Processor is starting.");
		int pendingMessageAttempts = 0;
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				OutboxMessageResult outboxMessageResult = await monitor.MonitorBatch(() => ProcessOutboxMessagesAsync(stoppingToken), _instanceId);
				if (outboxMessageResult.IsFullBatch)
				{
					logger.LogDebug("Full batch processed. Triggering immediate next batch.");
					pendingMessageAttempts = 0;
				}
				else if (outboxMessageResult.HasPendingMessages)
				{
					pendingMessageAttempts++;
					double num = Math.Min(100.0 * Math.Pow(2.0, pendingMessageAttempts), 1000.0);
					logger.LogDebug("Pending messages detected. Short backoff for {Delay}ms. Attempt {Count}", num, pendingMessageAttempts);
					await Task.Delay((int)num, stoppingToken);
				}
				else
				{
					pendingMessageAttempts = 0;
					logger.LogDebug("No pending messages. Waiting for {Interval} seconds before next polling cycle.", options.Value.PollingIntervalSeconds);
					await Task.Delay(TimeSpan.FromSeconds(options.Value.PollingIntervalSeconds), stoppingToken);
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Error occurred during processing. Will retry on next polling cycle.");
				await Task.Delay(TimeSpan.FromSeconds(options.Value.PollingIntervalSeconds), stoppingToken);
			}
		}
		logger.LogInformation("Outbox Processor is stopping.");
	}

	private async Task<OutboxMessageResult> ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = services.CreateScope();
		IOutboxStore store = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
		OutboxMessageResult result = await store.ClaimMessages(_instanceId, cancellationToken);
		if (!result.Messages.Any())
		{
			return result;
		}
		List<MessageUpdate> updates = new List<MessageUpdate>();
		foreach (OutboxMessage message in result.Messages)
		{
			await ProcessMessage(message, updates, scope, cancellationToken);
		}
		if (updates.Any())
		{
			await store.UpdateMessages(updates, cancellationToken);
		}
		return result;
	}

	private async Task ProcessMessage(OutboxMessage message, List<MessageUpdate> updates, IServiceScope scope, CancellationToken cancellationToken)
	{
		Type type;
		try
		{
			type = Type.GetType(message.EventType);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error loading type {EventType} for message {MessageId}", message.EventType, message.Id);
			updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Failed, ex.Message));
			return;
		}
		if ((object)type == null)
		{
			logger.LogError("Unknown event type: {EventType} for message {MessageId}", message.EventType, message.Id);
			updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Failed, "Unknown event type: " + message.EventType));
			return;
		}
		object obj;
		try
		{
			obj = JsonSerializer.Deserialize(message.Payload, type);
		}
		catch (Exception ex2)
		{
			logger.LogError(ex2, "Error deserializing payload for message {MessageId}", message.Id);
			updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Failed, ex2.Message));
			return;
		}
		if (obj == null)
		{
			updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Failed, "Failed to deserialize event payload"));
			return;
		}
		try
		{
			IPublisher requiredService = scope.ServiceProvider.GetRequiredService<IPublisher>();
			await requiredService.Publish(obj, cancellationToken);
			updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Delivered));
		}
		catch (Exception ex3)
		{
			if (transientExceptionDetector.IsTransient(ex3))
			{
				logger.LogWarning(ex3, "Transient error publishing message {MessageId}. Will retry.", message.Id);
				updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.RetryRequired, ex3.Message));
			}
			else
			{
				logger.LogError(ex3, "Error publishing message {MessageId}", message.Id);
				updates.Add(new MessageUpdate(message.Id, OutboxMessageStatus.Failed, ex3.Message));
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server;

public sealed class EventCollector : IDisposable
{
	private IEventConsumer _consumer;

	private IEventReceiver _eventReceiver;

	private ILogger _serverLogger;

	private BulkProcessor<TriggerEvent> _bulkProcessor;

	private volatile bool _isRunning;

	private static Dictionary<Guid, string> _systemMonitorEventIds = (from fieldInfo in typeof(BuiltInEventTypes.EventIDs).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField)
		select new KeyValuePair<Guid, string>((Guid)fieldInfo.GetValue(null), fieldInfo.Name) into keyValue
		where keyValue.Value.StartsWith("SystemMonitor", StringComparison.Ordinal) && !keyValue.Value.EndsWith("Undefined", StringComparison.Ordinal)
		select keyValue).ToDictionary((KeyValuePair<Guid, string> f) => f.Key, (KeyValuePair<Guid, string> f) => f.Value);

	private bool _disposed;

	public EventCollector(IEventConsumer consumer, IEventReceiver eventReceiver, ILogger serverLogger)
	{
		if (consumer == null)
		{
			throw new ArgumentNullException("consumer");
		}
		if (eventReceiver == null)
		{
			throw new ArgumentNullException("eventReceiver");
		}
		if (serverLogger == null)
		{
			throw new ArgumentNullException("serverLogger");
		}
		_consumer = consumer;
		_eventReceiver = eventReceiver;
		_serverLogger = serverLogger;
		_bulkProcessor = new BulkProcessor<TriggerEvent>(_consumer.SentEventToConsumer, FailedProcessedEvents, 1000);
		_isRunning = false;
	}

	private void FailedProcessedEvents(Exception ex, TriggerEvent[] failedEvents)
	{
		if (failedEvents != null && failedEvents.Length != 0)
		{
			_serverLogger.LogDebug("Failed to sent {FailedEvents} events to event server. Exception message: {ExceptionMessage}", failedEvents.Length, ex.Message);
		}
	}

	public void Start()
	{
		_eventReceiver.TriggerEventReceived += EventReciever_TriggerEventReceived;
		_isRunning = true;
	}

	public void Stop()
	{
		_eventReceiver.TriggerEventReceived -= EventReciever_TriggerEventReceived;
		_isRunning = false;
	}

	private void EventReciever_TriggerEventReceived(object sender, TriggerEventEventArgs e)
	{
		if (_isRunning && ShouldEventBeFired(e.TriggerEvent))
		{
			_bulkProcessor.EnqueueItem(e.TriggerEvent);
		}
	}

	public static bool ShouldEventBeFired(TriggerEvent triggerEvent)
	{
		if (triggerEvent.Definition.OccursGlobally)
		{
			if (triggerEvent.Definition.GeneratorType.Equals(EventGeneratorType.External.ToString(), StringComparison.OrdinalIgnoreCase) && (triggerEvent.Definition.GeneratorSubtype.Equals(EventGeneratorSubtype.UserDefined.ToString(), StringComparison.OrdinalIgnoreCase) || triggerEvent.Definition.GeneratorSubtype.Equals(EventGeneratorSubtype.Generic.ToString(), StringComparison.OrdinalIgnoreCase)))
			{
				return true;
			}
			if (_systemMonitorEventIds.ContainsKey(triggerEvent.EventTypeId))
			{
				return true;
			}
			if (triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.AutomaticActivationFailed || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.FailoverStarted || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.FailoverStopped || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.ScheduledPasswordChangeStarted || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.ScheduledPasswordChangeCompletedSuccessfully || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.ScheduledPasswordChangeCompletedWithErrors || triggerEvent.EventTypeId == BuiltInEventTypes.EventIDs.ManagementServerFailoverNodeStarted)
			{
				return true;
			}
		}
		return false;
	}

	private void Dispose(bool disposing)
	{
		if (_isRunning)
		{
			Stop();
		}
		if (!_disposed)
		{
			if (disposing && _bulkProcessor != null)
			{
				_bulkProcessor.Dispose();
				_bulkProcessor = null;
			}
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~EventCollector()
	{
		Dispose(disposing: false);
	}
}

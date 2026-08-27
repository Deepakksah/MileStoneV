using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public abstract class AlarmClient : AbstractClient
{
	public delegate void FaultExceptionHandler(Exception exception, int requestId);

	public delegate void GetAlarmHandler(int requestId, Alarm alarm);

	public delegate void GetEventHandler(int requestId, BaseEvent baseEvent);

	public delegate void UpdateAlarmValuesHandler(int requestId);

	public delegate void UpdateAlarmHandler(int requestId);

	public delegate void UpdateMultipleAlarmsHandler(int requestId);

	public delegate void AcknowledgeHandler(int requestId);

	public delegate void AcknowledgeMultipleAlarmsHandler(int requestId);

	public delegate void StartAlarmLineSessionHandler(int requestId, Guid sessionId);

	public delegate void StopAlarmLineSessionHandler(int requestId);

	public delegate void GetSessionAlarmLinesHandler(int requestId, AlarmUpdateData alarmUpdateData);

	public delegate void GetAlarmLinesHandler(int requestId, AlarmLine[] alarmLines);

	public delegate void GetAlarmLinesWithRelatedHardwareHandler(int requestId, AlarmLine[] alarmLines);

	public delegate void StartEventLineSessionHandler(int requestId, Guid sessionId);

	public delegate void StopEventLineSessionHandler(int requestId);

	public delegate void GetSessionEventLinesHandler(int requestId, EventUpdateData eventUpdateData);

	public delegate void GetEventLinesHandler(int requestId, EventLine[] eventLines);

	public delegate void GetStatisticsHandler(int requestId, Statistic[] statistics);

	public delegate void GetAlarmHistoryHandler(int requestId, AlarmHistory[] alarmHistory);

	public delegate void GetAlarmUpdateHistoryHandler(int requestId, AlarmUpdate[] alarmUpdate);

	public delegate void GetAlarmMessagesHandler(int requestId, string[] messages);

	protected ServerId ServerId { get; }

	public event FaultExceptionHandler FaultExceptionEvent;

	public event GetAlarmHandler GetAlarmEvent;

	public event GetEventHandler GetEventEvent;

	public event UpdateAlarmValuesHandler UpdateAlarmValuesEvent;

	public event UpdateAlarmHandler UpdateAlarmEvent;

	public event UpdateMultipleAlarmsHandler UpdateMultipleAlarmsEvent;

	public event AcknowledgeHandler AcknowledgeEvent;

	public event AcknowledgeMultipleAlarmsHandler AcknowledgeMultipleAlarmsEvent;

	public event StartAlarmLineSessionHandler StartAlarmLineSessionEvent;

	public event StopAlarmLineSessionHandler StopAlarmLineSessionEvent;

	public event GetSessionAlarmLinesHandler GetSessionAlarmLinesEvent;

	public event GetAlarmLinesHandler GetAlarmLinesEvent;

	public event GetAlarmLinesWithRelatedHardwareHandler GetAlarmLinesWithRelatedHardwareEvent;

	public event StartEventLineSessionHandler StartEventLineSessionEvent;

	public event StopEventLineSessionHandler StopEventLineSessionEvent;

	public event GetSessionEventLinesHandler GetSessionEventLinesEvent;

	public event GetEventLinesHandler GetEventLinesEvent;

	public event GetStatisticsHandler GetStatisticsEvent;

	public event GetAlarmHistoryHandler GetAlarmHistoryEvent;

	public event GetAlarmUpdateHistoryHandler GetAlarmUpdateHistoryEvent;

	public event GetAlarmMessagesHandler GetAlarmMessagesEvent;

	protected AlarmClient(ServerId serverId, string eventServerAddress, Version eventServerVersion, Uri uri)
		: base(eventServerAddress, eventServerVersion, uri)
	{
		ServerId = serverId;
	}

	protected override void HandleRequestObject(RequestObject requestObject)
	{
		if (requestObject is GetAlarmRequestObject)
		{
			GetAlarmRequestObject getAlarmRequestObject = requestObject as GetAlarmRequestObject;
			Alarm alarm = GetAlarm(getAlarmRequestObject.Id);
			FireGetAlarm(getAlarmRequestObject.RequestId, alarm);
			return;
		}
		if (requestObject is GetEventRequestObject)
		{
			GetEventRequestObject getEventRequestObject = requestObject as GetEventRequestObject;
			BaseEvent baseEvent = GetEvent(getEventRequestObject.Id);
			FireGetEvent(getEventRequestObject.RequestId, baseEvent);
			return;
		}
		if (requestObject is UpdateAlarmValuesRequestObject)
		{
			UpdateAlarmValuesRequestObject updateAlarmValuesRequestObject = requestObject as UpdateAlarmValuesRequestObject;
			UpdateAlarmValues(updateAlarmValuesRequestObject.Id, updateAlarmValuesRequestObject.Updates);
			FireUpdateAlarmValues(updateAlarmValuesRequestObject.RequestId);
			return;
		}
		if (requestObject is UpdateAlarmRequestObject)
		{
			UpdateAlarmRequestObject updateAlarmRequestObject = requestObject as UpdateAlarmRequestObject;
			UpdateAlarm(updateAlarmRequestObject.Id, updateAlarmRequestObject.Text, updateAlarmRequestObject.State, updateAlarmRequestObject.Priority, updateAlarmRequestObject.Time, updateAlarmRequestObject.AssignedTo);
			FireUpdateAlarm(updateAlarmRequestObject.RequestId);
			return;
		}
		if (requestObject is UpdateMultipleAlarmsRequestObject)
		{
			UpdateMultipleAlarmsRequestObject updateMultipleAlarmsRequestObject = requestObject as UpdateMultipleAlarmsRequestObject;
			UpdateMultipleAlarms(updateMultipleAlarmsRequestObject.OjectId, updateMultipleAlarmsRequestObject.OldState, updateMultipleAlarmsRequestObject.NewState, updateMultipleAlarmsRequestObject.Time, updateMultipleAlarmsRequestObject.AssignedTo);
			FireUpdateMultipleAlarms(updateMultipleAlarmsRequestObject.RequestId);
			return;
		}
		if (requestObject is AcknowledgeRequestObject)
		{
			AcknowledgeRequestObject acknowledgeRequestObject = requestObject as AcknowledgeRequestObject;
			Acknowledge(acknowledgeRequestObject.Id);
			FireAcknowledge(acknowledgeRequestObject.RequestId);
			return;
		}
		if (requestObject is AcknowledgeMultipleAlarmsRequestObject)
		{
			AcknowledgeMultipleAlarmsRequestObject acknowledgeMultipleAlarmsRequestObject = requestObject as AcknowledgeMultipleAlarmsRequestObject;
			AcknowledgeMultipleAlarms(acknowledgeMultipleAlarmsRequestObject.ObjectId);
			FireAcknowledgeMultipleAlarms(acknowledgeMultipleAlarmsRequestObject.RequestId);
			return;
		}
		if (requestObject is StartAlarmLineSessionRequestObject)
		{
			StartAlarmLineSessionRequestObject startAlarmLineSessionRequestObject = requestObject as StartAlarmLineSessionRequestObject;
			Guid sessionId = StartAlarmLineSession(startAlarmLineSessionRequestObject.Filter);
			FireStartAlarmLineSession(startAlarmLineSessionRequestObject.RequestId, sessionId);
			return;
		}
		if (requestObject is StopAlarmLineSessionRequestObject)
		{
			StopAlarmLineSessionRequestObject stopAlarmLineSessionRequestObject = requestObject as StopAlarmLineSessionRequestObject;
			StopAlarmLineSession(stopAlarmLineSessionRequestObject.SessionId);
			FireStopAlarmLineSession(stopAlarmLineSessionRequestObject.RequestId);
			return;
		}
		if (requestObject is GetAlarmLinesRequestObject)
		{
			GetAlarmLinesRequestObject getAlarmLinesRequestObject = requestObject as GetAlarmLinesRequestObject;
			AlarmLine[] alarmLines = GetAlarmLines(getAlarmLinesRequestObject.From, getAlarmLinesRequestObject.MaxCount, getAlarmLinesRequestObject.Filter);
			FireGetAlarmLines(getAlarmLinesRequestObject.RequestId, alarmLines);
			return;
		}
		if (requestObject is GetAlarmWithRelatedHardwareRequestObject)
		{
			GetAlarmWithRelatedHardwareRequestObject getAlarmWithRelatedHardwareRequestObject = requestObject as GetAlarmWithRelatedHardwareRequestObject;
			AlarmLine[] alarmsWithRelatedHardware = GetAlarmsWithRelatedHardware(getAlarmWithRelatedHardwareRequestObject.MaxCount, getAlarmWithRelatedHardwareRequestObject.Filter);
			FireGetAlarmLinesWithRelatedHardware(getAlarmWithRelatedHardwareRequestObject.RequestId, alarmsWithRelatedHardware);
			return;
		}
		if (requestObject is StartEventLineSessionRequestObject)
		{
			StartEventLineSessionRequestObject startEventLineSessionRequestObject = requestObject as StartEventLineSessionRequestObject;
			Guid sessionId2 = StartEventLineSession(startEventLineSessionRequestObject.Filter);
			FireStartEventLineSession(startEventLineSessionRequestObject.RequestId, sessionId2);
			return;
		}
		if (requestObject is StopEventLineSessionRequestObject)
		{
			StopEventLineSessionRequestObject stopEventLineSessionRequestObject = requestObject as StopEventLineSessionRequestObject;
			StopEventLineSession(stopEventLineSessionRequestObject.SessionId);
			FireStopEventLineSession(stopEventLineSessionRequestObject.RequestId);
			return;
		}
		if (requestObject is GetSessionEventLinesRequestObject)
		{
			GetSessionEventLinesRequestObject getSessionEventLinesRequestObject = requestObject as GetSessionEventLinesRequestObject;
			EventUpdateData sessionEventLines = GetSessionEventLines(getSessionEventLinesRequestObject.MaxCount, getSessionEventLinesRequestObject.SessionId);
			FireGetSessionEventLines(getSessionEventLinesRequestObject.RequestId, sessionEventLines);
			return;
		}
		if (requestObject is GetEventLinesRequestObject)
		{
			GetEventLinesRequestObject getEventLinesRequestObject = requestObject as GetEventLinesRequestObject;
			EventLine[] eventLines = GetEventLines(getEventLinesRequestObject.From, getEventLinesRequestObject.MaxCount, getEventLinesRequestObject.Filter);
			FireGetEventLines(getEventLinesRequestObject.RequestId, eventLines);
			return;
		}
		if (requestObject is GetStatisticsRequestObject)
		{
			GetStatisticsRequestObject getStatisticsRequestObject = requestObject as GetStatisticsRequestObject;
			Statistic[] statistics = GetStatistics();
			FireGetStatistics(getStatisticsRequestObject.RequestId, statistics);
			return;
		}
		if (requestObject is GetAlarmHistoryRequestObject)
		{
			GetAlarmHistoryRequestObject getAlarmHistoryRequestObject = requestObject as GetAlarmHistoryRequestObject;
			AlarmHistory[] alarmHistory = GetAlarmHistory(getAlarmHistoryRequestObject.Id);
			FireGetAlarmHistory(getAlarmHistoryRequestObject.RequestId, alarmHistory);
			return;
		}
		if (requestObject is GetAlarmUpdateHistoryRequestObject)
		{
			GetAlarmUpdateHistoryRequestObject getAlarmUpdateHistoryRequestObject = requestObject as GetAlarmUpdateHistoryRequestObject;
			AlarmUpdate[] alarmUpdateHistory = GetAlarmUpdateHistory(getAlarmUpdateHistoryRequestObject.Id);
			FireGetAlarmUpdateHistory(getAlarmUpdateHistoryRequestObject.RequestId, alarmUpdateHistory);
			return;
		}
		if (requestObject is GetAlarmMessagesRequestObject)
		{
			GetAlarmMessagesRequestObject getAlarmMessagesRequestObject = requestObject as GetAlarmMessagesRequestObject;
			string[] alarmMessages = GetAlarmMessages();
			FireGetAlarmMessages(getAlarmMessagesRequestObject.RequestId, alarmMessages);
			return;
		}
		if (requestObject is GetSessionAlarmLinesRequestObject)
		{
			GetSessionAlarmLinesRequestObject getSessionAlarmLinesRequestObject = requestObject as GetSessionAlarmLinesRequestObject;
			AlarmUpdateData sessionAlarmLines = GetSessionAlarmLines(getSessionAlarmLinesRequestObject.MaxCount, getSessionAlarmLinesRequestObject.SessionId);
			FireGetSessionAlarmLines(getSessionAlarmLinesRequestObject.RequestId, sessionAlarmLines);
			return;
		}
		throw new ArgumentException("Unknown request object");
	}

	protected override void FireFaultException(Exception exception, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(exception, requestId);
		}
	}

	private void FireGetAlarm(int requestId, Alarm alarm)
	{
		if (this.GetAlarmEvent != null)
		{
			this.GetAlarmEvent(requestId, alarm);
		}
	}

	public void BeginGetAlarm(int requestId, Guid id)
	{
		Enqueue(new GetAlarmRequestObject(requestId, id));
	}

	public abstract Alarm GetAlarm(Guid id);

	private void FireGetEvent(int requestId, BaseEvent baseEvent)
	{
		if (this.GetEventEvent != null)
		{
			this.GetEventEvent(requestId, baseEvent);
		}
	}

	public void BeginGetEvent(int requestId, Guid id)
	{
		Enqueue(new GetEventRequestObject(requestId, id));
	}

	public abstract BaseEvent GetEvent(Guid id);

	private void FireUpdateAlarmValues(int requestId)
	{
		if (this.UpdateAlarmValuesEvent != null)
		{
			this.UpdateAlarmValuesEvent(requestId);
		}
	}

	public void BeginUpdateAlarmValues(int requestId, Guid id, KeyValuePair<string, string>[] updates)
	{
		Enqueue(new UpdateAlarmValuesRequestObject(requestId, id, updates));
	}

	public abstract void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates);

	private void FireUpdateAlarm(int requestId)
	{
		if (this.UpdateAlarmEvent != null)
		{
			this.UpdateAlarmEvent(requestId);
		}
	}

	public void BeginUpdateAlarm(int requestId, Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		Enqueue(new UpdateAlarmRequestObject(requestId, id, text, state, priority, time, assignedTo));
	}

	public abstract void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo);

	private void FireUpdateMultipleAlarms(int requestId)
	{
		if (this.UpdateMultipleAlarmsEvent != null)
		{
			this.UpdateMultipleAlarmsEvent(requestId);
		}
	}

	public void BeginUpdateMultipleAlarms(int requestId, Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		Enqueue(new UpdateMultipleAlarmsRequestObject(requestId, objectId, oldState, newState, time, assignedTo));
	}

	public abstract void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo);

	private void FireAcknowledge(int requestId)
	{
		if (this.AcknowledgeEvent != null)
		{
			this.AcknowledgeEvent(requestId);
		}
	}

	public void BeginAcknowledge(int requestId, Guid id)
	{
		Enqueue(new AcknowledgeRequestObject(requestId, id));
	}

	public abstract void Acknowledge(Guid id);

	private void FireAcknowledgeMultipleAlarms(int requestId)
	{
		if (this.AcknowledgeMultipleAlarmsEvent != null)
		{
			this.AcknowledgeMultipleAlarmsEvent(requestId);
		}
	}

	public void BeginAcknowledgeMultipleAlarms(int requestId, Guid objectId)
	{
		Enqueue(new AcknowledgeMultipleAlarmsRequestObject(requestId, objectId));
	}

	public abstract void AcknowledgeMultipleAlarms(Guid objectId);

	private void FireStartAlarmLineSession(int requestId, Guid sessionId)
	{
		if (this.StartAlarmLineSessionEvent != null)
		{
			this.StartAlarmLineSessionEvent(requestId, sessionId);
		}
	}

	public void BeginStartAlarmLineSession(int requestId, AlarmFilter filter)
	{
		Enqueue(new StartAlarmLineSessionRequestObject(requestId, filter));
	}

	public abstract Guid StartAlarmLineSession(AlarmFilter filter);

	private void FireStopAlarmLineSession(int requestId)
	{
		if (this.StopAlarmLineSessionEvent != null)
		{
			this.StopAlarmLineSessionEvent(requestId);
		}
	}

	public void BeginStopAlarmLineSession(int requestId, Guid sessionId)
	{
		Enqueue(new StopAlarmLineSessionRequestObject(requestId, sessionId));
	}

	public abstract void StopAlarmLineSession(Guid sessionId);

	private void FireGetSessionAlarmLines(int requestId, AlarmUpdateData alarmUpdateData)
	{
		if (this.GetSessionAlarmLinesEvent != null)
		{
			this.GetSessionAlarmLinesEvent(requestId, alarmUpdateData);
		}
	}

	public void BeginGetSessionAlarmLines(int requestId, int maxCount, Guid sessionId)
	{
		Enqueue(new GetSessionAlarmLinesRequestObject(requestId, maxCount, sessionId));
	}

	public abstract AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId);

	private void FireGetAlarmLines(int requestId, AlarmLine[] alarmLines)
	{
		if (this.GetAlarmLinesEvent != null)
		{
			this.GetAlarmLinesEvent(requestId, alarmLines);
		}
	}

	private void FireGetAlarmLinesWithRelatedHardware(int requestId, AlarmLine[] alarmLines)
	{
		if (this.GetAlarmLinesWithRelatedHardwareEvent != null)
		{
			this.GetAlarmLinesWithRelatedHardwareEvent(requestId, alarmLines);
		}
	}

	public void BeginGetAlarmLines(int requestId, int from, int maxCount, AlarmFilter filter)
	{
		Enqueue(new GetAlarmLinesRequestObject(requestId, from, maxCount, filter));
	}

	public abstract AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter);

	public abstract AlarmLine[] GetAlarmsWithRelatedHardware(int maxCount, AlarmSearchCriteria[] filter);

	private void FireStartEventLineSession(int requestId, Guid sessionId)
	{
		if (this.StartEventLineSessionEvent != null)
		{
			this.StartEventLineSessionEvent(requestId, sessionId);
		}
	}

	public void BeginStartEventLineSession(int requestId, EventFilter filter)
	{
		Enqueue(new StartEventLineSessionRequestObject(requestId, filter));
	}

	public abstract Guid StartEventLineSession(EventFilter filter);

	private void FireStopEventLineSession(int requestId)
	{
		if (this.StopEventLineSessionEvent != null)
		{
			this.StopEventLineSessionEvent(requestId);
		}
	}

	public void BeginStopEventLineSession(int requestId, Guid sessionId)
	{
		Enqueue(new StopEventLineSessionRequestObject(requestId, sessionId));
	}

	public abstract void StopEventLineSession(Guid sessionId);

	private void FireGetSessionEventLines(int requestId, EventUpdateData eventUpdateData)
	{
		if (this.GetSessionEventLinesEvent != null)
		{
			this.GetSessionEventLinesEvent(requestId, eventUpdateData);
		}
	}

	public void BeginGetSessionEventLines(int requestId, int maxCount, Guid sessionId)
	{
		Enqueue(new GetSessionEventLinesRequestObject(requestId, maxCount, sessionId));
	}

	public abstract EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId);

	private void FireGetEventLines(int requestId, EventLine[] eventLines)
	{
		if (this.GetEventLinesEvent != null)
		{
			this.GetEventLinesEvent(requestId, eventLines);
		}
	}

	public void BeginGetEventLines(int requestId, int from, int maxCount, EventFilter filter)
	{
		Enqueue(new GetEventLinesRequestObject(requestId, from, maxCount, filter));
	}

	public abstract EventLine[] GetEventLines(int from, int maxCount, EventFilter filter);

	public abstract IAsyncResult BeginGetEventLines2(GetEventLines2Params parameters, AsyncCallback callback, object asyncState);

	public abstract GetEventLines2Resp EndGetEventLines2(IAsyncResult result);

	private void FireGetStatistics(int requestId, Statistic[] statistics)
	{
		if (this.GetStatisticsEvent != null)
		{
			this.GetStatisticsEvent(requestId, statistics);
		}
	}

	public void BeginGetStatistics(int requestId)
	{
		Enqueue(new GetStatisticsRequestObject(requestId));
	}

	public abstract Statistic[] GetStatistics();

	private void FireGetAlarmHistory(int requestId, AlarmHistory[] alarmHistory)
	{
		if (this.GetAlarmHistoryEvent != null)
		{
			this.GetAlarmHistoryEvent(requestId, alarmHistory);
		}
	}

	public void BeginGetAlarmHistory(int requestId, Guid id)
	{
		Enqueue(new GetAlarmHistoryRequestObject(requestId, id));
	}

	public abstract AlarmHistory[] GetAlarmHistory(Guid id);

	private void FireGetAlarmUpdateHistory(int requestId, AlarmUpdate[] alarmUpdate)
	{
		if (this.GetAlarmUpdateHistoryEvent != null)
		{
			this.GetAlarmUpdateHistoryEvent(requestId, alarmUpdate);
		}
	}

	public void BeginGetAlarmUpdateHistory(int requestId, Guid id)
	{
		Enqueue(new GetAlarmUpdateHistoryRequestObject(requestId, id));
	}

	public abstract AlarmUpdate[] GetAlarmUpdateHistory(Guid id);

	private void FireGetAlarmMessages(int requestId, string[] messages)
	{
		if (this.GetAlarmMessagesEvent != null)
		{
			this.GetAlarmMessagesEvent(requestId, messages);
		}
	}

	public void BeginGetAlarmMessages(int requestId)
	{
		Enqueue(new GetAlarmMessagesRequestObject(requestId));
	}

	public abstract string[] GetAlarmMessages();

	public abstract IAsyncResult BeginGetEventCount(GetEventCountParams parameters, AsyncCallback callback, object asyncState);

	public abstract GetEventCountResp EndGetEventCount(IAsyncResult result);

	public abstract IAsyncResult BeginGetEvents(GetEventsParams parameters, AsyncCallback callback, object asyncState);

	public abstract GetEventsResp EndGetEvents(IAsyncResult result);
}

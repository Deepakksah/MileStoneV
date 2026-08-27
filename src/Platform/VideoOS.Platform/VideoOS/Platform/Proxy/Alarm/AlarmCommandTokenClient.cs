using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class AlarmCommandTokenClient : ClientBase<IAlarmCommandToken>, IAlarmCommandToken
{
	public AlarmCommandTokenClient()
	{
	}

	public AlarmCommandTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlarmCommandTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void Add(string token, VideoOS.Platform.Data.Alarm alarm)
	{
		base.Channel.Add(token, alarm);
	}

	public void AddEvent(string token, BaseEvent baseEvent)
	{
		base.Channel.AddEvent(token, baseEvent);
	}

	public VideoOS.Platform.Data.Alarm Get(string token, Guid id)
	{
		return base.Channel.Get(token, id);
	}

	public BaseEvent GetEvent(string token, Guid id)
	{
		return base.Channel.GetEvent(token, id);
	}

	public EventHeader GetHeader(string token, Guid id)
	{
		return base.Channel.GetHeader(token, id);
	}

	public SnapshotList GetSnapshots(string token, Guid id)
	{
		return base.Channel.GetSnapshots(token, id);
	}

	public void AttachSnapshot(string token, Guid id, Snapshot snapshot)
	{
		base.Channel.AttachSnapshot(token, id, snapshot);
	}

	public void UpdateAlarmValues(string token, Guid id, KeyValuePair<string, string>[] updates)
	{
		base.Channel.UpdateAlarmValues(token, id, updates);
	}

	public void UpdateAlarm(string token, Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		base.Channel.UpdateAlarm(token, id, text, state, priority, time, assignedTo);
	}

	public void Acknowledge(string token, Guid id)
	{
		base.Channel.Acknowledge(token, id);
	}

	public void UpdateMultipleAlarms(string token, Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		base.Channel.UpdateMultipleAlarms(token, objectId, oldState, newState, time, assignedTo);
	}

	public void AcknowledgeMultipleAlarms(string token, Guid objectId)
	{
		base.Channel.AcknowledgeMultipleAlarms(token, objectId);
	}

	public Guid StartAlarmLineSession(string token, AlarmFilter filter)
	{
		return base.Channel.StartAlarmLineSession(token, filter);
	}

	public void StopAlarmLineSession(string token, Guid sessionId)
	{
		base.Channel.StopAlarmLineSession(token, sessionId);
	}

	public AlarmUpdateData GetSessionAlarmLines(string token, int maxCount, Guid sessionId)
	{
		return base.Channel.GetSessionAlarmLines(token, maxCount, sessionId);
	}

	public AlarmLine[] GetAlarmLines(string token, int from, int maxCount, AlarmFilter filter)
	{
		return base.Channel.GetAlarmLines(token, from, maxCount, filter);
	}

	public Guid StartEventLineSession(string token, EventFilter filter)
	{
		return base.Channel.StartEventLineSession(token, filter);
	}

	public void StopEventLineSession(string token, Guid sessionId)
	{
		base.Channel.StopEventLineSession(token, sessionId);
	}

	public EventUpdateData GetSessionEventLines(string token, int maxCount, Guid sessionId)
	{
		return base.Channel.GetSessionEventLines(token, maxCount, sessionId);
	}

	public EventLine[] GetEventLines(string token, int from, int maxCount, EventFilter filter)
	{
		return base.Channel.GetEventLines(token, from, maxCount, filter);
	}

	public Statistic[] GetStatistics(string token)
	{
		return base.Channel.GetStatistics(token);
	}

	public AlarmHistory[] GetAlarmHistory(string token, Guid id)
	{
		return base.Channel.GetAlarmHistory(token, id);
	}

	public AlarmUpdate[] GetAlarmUpdateHistory(string token, Guid id)
	{
		return base.Channel.GetAlarmUpdateHistory(token, id);
	}

	public string[] GetAlarmMessages(string token)
	{
		return base.Channel.GetAlarmMessages(token);
	}
}

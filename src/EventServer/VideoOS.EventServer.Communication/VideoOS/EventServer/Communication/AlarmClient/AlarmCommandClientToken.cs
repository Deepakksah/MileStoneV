using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AlarmCommandClientToken : ClientBase<IAlarmCommandToken>, IAlarmCommandToken
{
	public AlarmCommandClientToken()
	{
	}

	public AlarmCommandClientToken(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlarmCommandClientToken(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandClientToken(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandClientToken(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void Add(string token, Alarm alarm)
	{
		base.Channel.Add(token, alarm);
	}

	public void AddEvent(string token, BaseEvent baseEvent)
	{
		base.Channel.AddEvent(token, baseEvent);
	}

	public void AddAsEvent(string token, Alarm alarm)
	{
		base.Channel.AddAsEvent(token, alarm);
	}

	public Alarm Get(string token, Guid id)
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

	public AlarmLine[] GetAlarmsWithRelatedHardware(string token, int maxCount, AlarmSearchCriteria[] filter)
	{
		return base.Channel.GetAlarmsWithRelatedHardware(token, maxCount, filter);
	}

	public Task<AlarmLine[]> GetAlarmsWithRelatedHardwareAsync(string token, int maxCount, AlarmSearchCriteria[] filter)
	{
		return base.Channel.GetAlarmsWithRelatedHardwareAsync(token, maxCount, filter);
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

	public IAsyncResult BeginGetEventLines2(string token, GetEventLines2Params parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEventLines2(token, parameters, callback, asyncState);
	}

	public GetEventLines2Resp EndGetEventLines2(IAsyncResult result)
	{
		return base.Channel.EndGetEventLines2(result);
	}

	public IAsyncResult BeginGetAccessControlEvents(string token, GetAccessControlEventsParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetAccessControlEvents(token, parameters, callback, asyncState);
	}

	public GetAccessControlEventsResp EndGetAccessControlEvents(IAsyncResult result)
	{
		return base.Channel.EndGetAccessControlEvents(result);
	}

	public IAsyncResult BeginGetEventCount(string token, GetEventCountParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEventCount(token, parameters, callback, asyncState);
	}

	public GetEventCountResp EndGetEventCount(IAsyncResult result)
	{
		return base.Channel.EndGetEventCount(result);
	}

	public IAsyncResult BeginGetEvents(string token, GetEventsParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEvents(token, parameters, callback, asyncState);
	}

	public GetEventsResp EndGetEvents(IAsyncResult result)
	{
		return base.Channel.EndGetEvents(result);
	}
}

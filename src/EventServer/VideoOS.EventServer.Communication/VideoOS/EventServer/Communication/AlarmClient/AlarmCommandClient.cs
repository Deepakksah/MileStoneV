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
public class AlarmCommandClient : ClientBase<IAlarmCommand>, IAlarmCommand
{
	public AlarmCommandClient()
	{
	}

	public AlarmCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AlarmCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AlarmCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void Add(Alarm alarm)
	{
		base.Channel.Add(alarm);
	}

	public void AddEvent(BaseEvent baseEvent)
	{
		base.Channel.AddEvent(baseEvent);
	}

	public void AddAsEvent(Alarm alarm)
	{
		base.Channel.AddAsEvent(alarm);
	}

	public Alarm Get(Guid id)
	{
		return base.Channel.Get(id);
	}

	public BaseEvent GetEvent(Guid id)
	{
		return base.Channel.GetEvent(id);
	}

	public EventHeader GetHeader(Guid id)
	{
		return base.Channel.GetHeader(id);
	}

	public SnapshotList GetSnapshots(Guid id)
	{
		return base.Channel.GetSnapshots(id);
	}

	public void AttachSnapshot(Guid id, Snapshot snapshot)
	{
		base.Channel.AttachSnapshot(id, snapshot);
	}

	public void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates)
	{
		base.Channel.UpdateAlarmValues(id, updates);
	}

	public void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		base.Channel.UpdateAlarm(id, text, state, priority, time, assignedTo);
	}

	public void Acknowledge(Guid id)
	{
		base.Channel.Acknowledge(id);
	}

	public void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		base.Channel.UpdateMultipleAlarms(objectId, oldState, newState, time, assignedTo);
	}

	public void AcknowledgeMultipleAlarms(Guid objectId)
	{
		base.Channel.AcknowledgeMultipleAlarms(objectId);
	}

	public Guid StartAlarmLineSession(AlarmFilter filter)
	{
		return base.Channel.StartAlarmLineSession(filter);
	}

	public void StopAlarmLineSession(Guid sessionId)
	{
		base.Channel.StopAlarmLineSession(sessionId);
	}

	public AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId)
	{
		return base.Channel.GetSessionAlarmLines(maxCount, sessionId);
	}

	public AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter)
	{
		return base.Channel.GetAlarmLines(from, maxCount, filter);
	}

	public AlarmLine[] GetAlarmsWithRelatedHardware(int maxCount, AlarmSearchCriteria[] filter)
	{
		return base.Channel.GetAlarmsWithRelatedHardware(maxCount, filter);
	}

	public Task<AlarmLine[]> GetAlarmsWithRelatedHardwareAsync(int maxCount, AlarmSearchCriteria[] filter)
	{
		return base.Channel.GetAlarmsWithRelatedHardwareAsync(maxCount, filter);
	}

	public Guid StartEventLineSession(EventFilter filter)
	{
		return base.Channel.StartEventLineSession(filter);
	}

	public void StopEventLineSession(Guid sessionId)
	{
		base.Channel.StopEventLineSession(sessionId);
	}

	public EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId)
	{
		return base.Channel.GetSessionEventLines(maxCount, sessionId);
	}

	public EventLine[] GetEventLines(int from, int maxCount, EventFilter filter)
	{
		return base.Channel.GetEventLines(from, maxCount, filter);
	}

	public Statistic[] GetStatistics()
	{
		return base.Channel.GetStatistics();
	}

	public AlarmHistory[] GetAlarmHistory(Guid id)
	{
		return base.Channel.GetAlarmHistory(id);
	}

	public AlarmUpdate[] GetAlarmUpdateHistory(Guid id)
	{
		return base.Channel.GetAlarmUpdateHistory(id);
	}

	public string[] GetAlarmMessages()
	{
		return base.Channel.GetAlarmMessages();
	}

	public IAsyncResult BeginGetEventLines2(GetEventLines2Params parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEventLines2(parameters, callback, asyncState);
	}

	public GetEventLines2Resp EndGetEventLines2(IAsyncResult result)
	{
		return base.Channel.EndGetEventLines2(result);
	}

	public IAsyncResult BeginGetAccessControlEvents(GetAccessControlEventsParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetAccessControlEvents(parameters, callback, asyncState);
	}

	public GetAccessControlEventsResp EndGetAccessControlEvents(IAsyncResult result)
	{
		return base.Channel.EndGetAccessControlEvents(result);
	}

	public IAsyncResult BeginGetEventCount(GetEventCountParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEventCount(parameters, callback, asyncState);
	}

	public GetEventCountResp EndGetEventCount(IAsyncResult result)
	{
		return base.Channel.EndGetEventCount(result);
	}

	public IAsyncResult BeginGetEvents(GetEventsParams parameters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetEvents(parameters, callback, asyncState);
	}

	public GetEventsResp EndGetEvents(IAsyncResult result)
	{
		return base.Channel.EndGetEvents(result);
	}
}

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AlarmClientToken : AlarmClient
{
	private readonly ClientPool<AlarmCommandClientToken> _eventsClientPool;

	private readonly ClientPool<AlarmCommandClientToken> _alarmsClientPool;

	private readonly string _alarmServerAddress;

	public AlarmClientToken(ServerId serverId, string eventServerAddress, Version eventServerVersion, string alarmServerAddress)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/AlarmServiceToken"))
	{
		_alarmServerAddress = alarmServerAddress;
		_eventsClientPool = new ClientPool<AlarmCommandClientToken>(CreateEventsClient, 2);
		_alarmsClientPool = (string.IsNullOrEmpty(alarmServerAddress) ? _eventsClientPool : new ClientPool<AlarmCommandClientToken>(CreateAlarmsClient, 2));
	}

	private AlarmCommandClientToken CreateEventsClient()
	{
		AlarmCommandClientToken alarmCommandClientToken = new AlarmCommandClientToken(BindingFactory.CreateBasicHttpBindingToken(_uri.IsHttps()), new EndpointAddress(_uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(alarmCommandClientToken);
		return alarmCommandClientToken;
	}

	private AlarmCommandClientToken CreateAlarmsClient()
	{
		Uri uri = new Uri(_alarmServerAddress + "Central/AlarmServiceToken");
		AlarmCommandClientToken alarmCommandClientToken = new AlarmCommandClientToken(BindingFactory.CreateBasicHttpBindingToken(uri.IsHttps()), new EndpointAddress(uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(alarmCommandClientToken);
		return alarmCommandClientToken;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	public override Alarm GetAlarm(Guid id)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.Get(GetToken(), id));
	}

	public override BaseEvent GetEvent(Guid id)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetEvent(GetToken(), id));
	}

	public override void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.UpdateAlarmValues(GetToken(), id, updates);
		});
	}

	public override void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.UpdateAlarm(GetToken(), id, text, state, priority, time, assignedTo);
		});
	}

	public override void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.UpdateMultipleAlarms(GetToken(), objectId, oldState, newState, time, assignedTo);
		});
	}

	public override void Acknowledge(Guid id)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.Acknowledge(GetToken(), id);
		});
	}

	public override void AcknowledgeMultipleAlarms(Guid objectId)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.AcknowledgeMultipleAlarms(GetToken(), objectId);
		});
	}

	public override Guid StartAlarmLineSession(AlarmFilter filter)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.StartAlarmLineSession(GetToken(), filter));
	}

	public override void StopAlarmLineSession(Guid sessionId)
	{
		_alarmsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.StopAlarmLineSession(GetToken(), sessionId);
		});
	}

	public override AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetSessionAlarmLines(GetToken(), maxCount, sessionId));
	}

	public override AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetAlarmLines(GetToken(), from, maxCount, filter));
	}

	public override AlarmLine[] GetAlarmsWithRelatedHardware(int maxCount, AlarmSearchCriteria[] filter)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetAlarmsWithRelatedHardware(GetToken(), maxCount, filter));
	}

	public override Guid StartEventLineSession(EventFilter filter)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.StartEventLineSession(GetToken(), filter));
	}

	public override void StopEventLineSession(Guid sessionId)
	{
		_eventsClientPool.CallWithRetry(delegate(AlarmCommandClientToken client)
		{
			client.StopEventLineSession(GetToken(), sessionId);
		});
	}

	public override EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetSessionEventLines(GetToken(), maxCount, sessionId));
	}

	public override EventLine[] GetEventLines(int from, int maxCount, EventFilter filter)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetEventLines(GetToken(), from, maxCount, filter));
	}

	public override Statistic[] GetStatistics()
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetStatistics(GetToken()));
	}

	public override AlarmHistory[] GetAlarmHistory(Guid id)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetAlarmHistory(GetToken(), id));
	}

	public override AlarmUpdate[] GetAlarmUpdateHistory(Guid id)
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetAlarmUpdateHistory(GetToken(), id));
	}

	public override string[] GetAlarmMessages()
	{
		return _alarmsClientPool.CallWithRetry((AlarmCommandClientToken client) => client.GetAlarmMessages(GetToken()));
	}

	public override IAsyncResult BeginGetEventLines2(GetEventLines2Params parameters, AsyncCallback callback, object asyncState)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client, AsyncCallback cb) => client.BeginGetEventLines2(GetToken(), parameters, cb, null), (AlarmCommandClientToken client, IAsyncResult asyncRes) => client.EndGetEventLines2(asyncRes), callback, asyncState);
	}

	public override GetEventLines2Resp EndGetEventLines2(IAsyncResult result)
	{
		if (!(result is Task<GetEventLines2Resp> task))
		{
			throw new ArgumentException("Result must be returned by BeginGetEventLines2");
		}
		try
		{
			return task.Result;
		}
		catch (AggregateException ex)
		{
			throw ex.InnerException;
		}
	}

	public override IAsyncResult BeginGetEventCount(GetEventCountParams parameters, AsyncCallback callback, object asyncState)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client, AsyncCallback cb) => client.BeginGetEventCount(GetToken(), parameters, cb, null), (AlarmCommandClientToken client, IAsyncResult asyncRes) => client.EndGetEventCount(asyncRes), callback, asyncState);
	}

	public override GetEventCountResp EndGetEventCount(IAsyncResult result)
	{
		if (!(result is Task<GetEventCountResp> task))
		{
			throw new ArgumentException("Result must be returned by BeginGetEventCount");
		}
		try
		{
			return task.Result;
		}
		catch (AggregateException ex)
		{
			throw ex.InnerException;
		}
	}

	public override IAsyncResult BeginGetEvents(GetEventsParams parameters, AsyncCallback callback, object asyncState)
	{
		return _eventsClientPool.CallWithRetry((AlarmCommandClientToken client, AsyncCallback cb) => client.BeginGetEvents(GetToken(), parameters, cb, null), (AlarmCommandClientToken client, IAsyncResult asyncRes) => client.EndGetEvents(asyncRes), callback, asyncState);
	}

	public override GetEventsResp EndGetEvents(IAsyncResult result)
	{
		if (!(result is Task<GetEventsResp> task))
		{
			throw new ArgumentException("Result must be returned by BeginGetEvents");
		}
		try
		{
			return task.Result;
		}
		catch (AggregateException ex)
		{
			throw ex.InnerException;
		}
	}

	public override void Close()
	{
		base.Close();
		_eventsClientPool.Dispose();
	}
}

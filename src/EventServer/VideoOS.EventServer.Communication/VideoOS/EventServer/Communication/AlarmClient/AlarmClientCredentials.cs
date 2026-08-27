using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AlarmClientCredentials : AlarmClient
{
	private readonly ClientPool<AlarmCommandClient> _clientPool;

	private static readonly Version _secureAlarmServiceVersion = new Version(10, 2);

	public AlarmClientCredentials(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, (eventServerVersion < _secureAlarmServiceVersion) ? new Uri(new Uri(eventServerAddress), "Central/AlarmService") : new Uri(new Uri(eventServerAddress), "Central/AlarmService2"))
	{
		_clientPool = new ClientPool<AlarmCommandClient>(CreateClient, 2);
	}

	private AlarmCommandClient CreateClient()
	{
		Binding binding = ((!(base.EventServerVersion2 < _secureAlarmServiceVersion)) ? ((Binding)BindingFactory.CreateWSHttpBindingCredentials(_uri.IsHttps())) : ((Binding)BindingFactory.CreateBasicHttpBindingCredentials()));
		string spn = SpnFactory.GetSpn(_uri);
		AlarmCommandClient alarmCommandClient = new AlarmCommandClient(binding, new EndpointAddress(_uri, AbstractClient.CreateSpnIdentity(spn)));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(alarmCommandClient);
		if (base.EventServerVersion2 >= _secureAlarmServiceVersion)
		{
			alarmCommandClient.EnsureLargeObjectGraphsAllowed<AlarmCommandClient, IAlarmCommand>();
		}
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		alarmCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		return alarmCommandClient;
	}

	public override Alarm GetAlarm(Guid id)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.Get(id));
	}

	public override BaseEvent GetEvent(Guid id)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetEvent(id));
	}

	public override void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.UpdateAlarmValues(id, updates);
		});
	}

	public override void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.UpdateAlarm(id, text, state, priority, time, assignedTo);
		});
	}

	public override void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.UpdateMultipleAlarms(objectId, oldState, newState, time, assignedTo);
		});
	}

	public override void Acknowledge(Guid id)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.Acknowledge(id);
		});
	}

	public override void AcknowledgeMultipleAlarms(Guid objectId)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.AcknowledgeMultipleAlarms(objectId);
		});
	}

	public override Guid StartAlarmLineSession(AlarmFilter filter)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.StartAlarmLineSession(filter));
	}

	public override void StopAlarmLineSession(Guid sessionId)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.StopAlarmLineSession(sessionId);
		});
	}

	public override AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetSessionAlarmLines(maxCount, sessionId));
	}

	public override AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetAlarmLines(from, maxCount, filter));
	}

	public override AlarmLine[] GetAlarmsWithRelatedHardware(int maxCount, AlarmSearchCriteria[] filter)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetAlarmsWithRelatedHardware(maxCount, filter));
	}

	public override Guid StartEventLineSession(EventFilter filter)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.StartEventLineSession(filter));
	}

	public override void StopEventLineSession(Guid sessionId)
	{
		_clientPool.CallWithRetry(delegate(AlarmCommandClient client)
		{
			client.StopEventLineSession(sessionId);
		});
	}

	public override EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetSessionEventLines(maxCount, sessionId));
	}

	public override EventLine[] GetEventLines(int from, int maxCount, EventFilter filter)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetEventLines(from, maxCount, filter));
	}

	public override Statistic[] GetStatistics()
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetStatistics());
	}

	public override AlarmHistory[] GetAlarmHistory(Guid id)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetAlarmHistory(id));
	}

	public override AlarmUpdate[] GetAlarmUpdateHistory(Guid id)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetAlarmUpdateHistory(id));
	}

	public override string[] GetAlarmMessages()
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client) => client.GetAlarmMessages());
	}

	public override IAsyncResult BeginGetEventLines2(GetEventLines2Params parameters, AsyncCallback callback, object asyncState)
	{
		return _clientPool.CallWithRetry((AlarmCommandClient client, AsyncCallback cb) => client.BeginGetEventLines2(parameters, cb, null), (AlarmCommandClient client, IAsyncResult asyncRes) => client.EndGetEventLines2(asyncRes), callback, asyncState);
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
		return _clientPool.CallWithRetry((AlarmCommandClient client, AsyncCallback cb) => client.BeginGetEventCount(parameters, cb, null), (AlarmCommandClient client, IAsyncResult asyncRes) => client.EndGetEventCount(asyncRes), callback, asyncState);
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
		return _clientPool.CallWithRetry((AlarmCommandClient client, AsyncCallback cb) => client.BeginGetEvents(parameters, cb, null), (AlarmCommandClient client, IAsyncResult asyncRes) => client.EndGetEvents(asyncRes), callback, asyncState);
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
		_clientPool.Dispose();
	}
}

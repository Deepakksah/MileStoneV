using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AlarmCommandClientManager
{
	public delegate void FaultExceptionHandler(Exception error, int requestId);

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

	public delegate void StartEventLineSessionHandler(int requestId, Guid sessionId);

	public delegate void StopEventLineSessionHandler(int requestId);

	public delegate void GetSessionEventLinesHandler(int requestId, EventUpdateData eventUpdateData);

	public delegate void GetEventLinesHandler(int requestId, EventLine[] eventLines);

	public delegate void GetStatisticsHandler(int requestId, Statistic[] statistics);

	public delegate void GetAlarmHistoryHandler(int requestId, AlarmHistory[] alarmHistory);

	public delegate void GetAlarmUpdateHistoryHandler(int requestId, AlarmUpdate[] alarmUpdate);

	public delegate void GetAlarmMessagesHandler(int requestId, string[] messages);

	private readonly Dictionary<ServerId, AlarmClient> _clients = new Dictionary<ServerId, AlarmClient>();

	private readonly Dictionary<ServerId, VideoOS.Platform.Configuration.ServiceURIInfo> _serviceUris = new Dictionary<ServerId, VideoOS.Platform.Configuration.ServiceURIInfo>();

	private readonly object _lockObject = new object();

	private readonly IAlarmClientFactory _clientFactory;

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

	public event StartEventLineSessionHandler StartEventLineSessionEvent;

	public event StopEventLineSessionHandler StopEventLineSessionEvent;

	public event GetSessionEventLinesHandler GetSessionEventLinesEvent;

	public event GetEventLinesHandler GetEventLinesEvent;

	public event GetStatisticsHandler GetStatisticsEvent;

	public event GetAlarmHistoryHandler GetAlarmHistoryEvent;

	public event GetAlarmUpdateHistoryHandler GetAlarmUpdateHistoryEvent;

	public event GetAlarmMessagesHandler GetAlarmMessagesEvent;

	public AlarmCommandClientManager()
		: this(new AlarmClientFactory())
	{
	}

	public AlarmCommandClientManager(IAlarmClientFactory alarmClientFactory)
	{
		_clientFactory = alarmClientFactory;
	}

	public AlarmClient GetAlarmCommandClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			if (!_clients.TryGetValue(serverId, out var _))
			{
				AlarmClient alarmClient = CreateAlarmCommandClient(serverId);
				if (alarmClient == null)
				{
					throw new ServiceNotRegisteredException();
				}
				_clients.Add(serverId, alarmClient);
			}
			return _clients[serverId];
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (AlarmClient value in _clients.Values)
			{
				DestroyAlarmCommandClient(value);
			}
			_clients.Clear();
		}
	}

	private AlarmClient CreateAlarmCommandClient(ServerId serverId)
	{
		try
		{
			VideoOS.Platform.Configuration instance = VideoOS.Platform.Configuration.Instance;
			try
			{
				List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
				List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo2 = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_AlarmsService, serverId);
				if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
				{
					throw new ServiceNotRegisteredException();
				}
				_serviceUris[serverId] = registeredServiceUriInfo[0];
				string alarmServerAddress = ((registeredServiceUriInfo2 == null || registeredServiceUriInfo2.Count == 0 || registeredServiceUriInfo2[0].UriArray.Count == 0) ? null : registeredServiceUriInfo2[0].UriArray[0]);
				Version eventServerVersion = ServerReceiver.GetEventServerVersion2(registeredServiceUriInfo[0]);
				AlarmClient alarmClient = _clientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], eventServerVersion ?? new Version(1, 0), alarmServerAddress);
				alarmClient.FaultExceptionEvent += FireFaultException;
				alarmClient.GetAlarmEvent += FireGetAlarm;
				alarmClient.GetEventEvent += FireGetEvent;
				alarmClient.UpdateAlarmValuesEvent += FireUpdateAlarmValues;
				alarmClient.UpdateAlarmEvent += FireUpdateAlarm;
				alarmClient.UpdateMultipleAlarmsEvent += FireUpdateMultipleAlarms;
				alarmClient.AcknowledgeEvent += FireAcknowledge;
				alarmClient.AcknowledgeMultipleAlarmsEvent += FireAcknowledgeMultipleAlarms;
				alarmClient.StartAlarmLineSessionEvent += FireStartAlarmLineSession;
				alarmClient.StopAlarmLineSessionEvent += FireStopAlarmLineSession;
				alarmClient.GetAlarmLinesEvent += FireGetAlarmLines;
				alarmClient.StartEventLineSessionEvent += FireStartEventLineSession;
				alarmClient.StopEventLineSessionEvent += FireStopEventLineSession;
				alarmClient.GetSessionEventLinesEvent += FireGetSessionEventLines;
				alarmClient.GetEventLinesEvent += FireGetEventLines;
				alarmClient.GetStatisticsEvent += FireGetStatistics;
				alarmClient.GetAlarmHistoryEvent += FireGetAlarmHistory;
				alarmClient.GetAlarmUpdateHistoryEvent += FireGetAlarmUpdateHistory;
				alarmClient.GetAlarmMessagesEvent += FireGetAlarmMessages;
				return alarmClient;
			}
			catch (CommunicationMIPException innerException)
			{
				throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
			}
		}
		catch (ThreadAbortException)
		{
			return null;
		}
		catch
		{
			throw;
		}
	}

	private void DestroyAlarmCommandClient(AlarmClient alarmClient)
	{
		if (alarmClient != null)
		{
			alarmClient.FaultExceptionEvent -= FireFaultException;
			alarmClient.GetAlarmEvent -= FireGetAlarm;
			alarmClient.GetEventEvent -= FireGetEvent;
			alarmClient.UpdateAlarmValuesEvent -= FireUpdateAlarmValues;
			alarmClient.UpdateAlarmEvent -= FireUpdateAlarm;
			alarmClient.UpdateMultipleAlarmsEvent -= FireUpdateMultipleAlarms;
			alarmClient.AcknowledgeEvent -= FireAcknowledge;
			alarmClient.AcknowledgeMultipleAlarmsEvent -= FireAcknowledgeMultipleAlarms;
			alarmClient.StartAlarmLineSessionEvent -= FireStartAlarmLineSession;
			alarmClient.StopAlarmLineSessionEvent -= FireStopAlarmLineSession;
			alarmClient.GetAlarmLinesEvent -= FireGetAlarmLines;
			alarmClient.StartEventLineSessionEvent -= FireStartEventLineSession;
			alarmClient.StopEventLineSessionEvent -= FireStopEventLineSession;
			alarmClient.GetSessionEventLinesEvent -= FireGetSessionEventLines;
			alarmClient.GetEventLinesEvent -= FireGetEventLines;
			alarmClient.GetStatisticsEvent -= FireGetStatistics;
			alarmClient.GetAlarmHistoryEvent -= FireGetAlarmHistory;
			alarmClient.GetAlarmUpdateHistoryEvent -= FireGetAlarmUpdateHistory;
			alarmClient.GetAlarmMessagesEvent -= FireGetAlarmMessages;
			alarmClient.Close();
		}
	}

	public VideoOS.Platform.Configuration.ServiceURIInfo GetServiceURIInfo(ServerId serverId)
	{
		lock (_lockObject)
		{
			_serviceUris.TryGetValue(serverId, out var value);
			return value;
		}
	}

	private void FireFaultException(Exception error, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(error, requestId);
		}
	}

	private void FireGetAlarm(int requestId, Alarm alarm)
	{
		if (this.GetAlarmEvent != null)
		{
			this.GetAlarmEvent(requestId, alarm);
		}
	}

	private void FireGetEvent(int requestId, BaseEvent baseEvent)
	{
		if (this.GetEventEvent != null)
		{
			this.GetEventEvent(requestId, baseEvent);
		}
	}

	private void FireUpdateAlarmValues(int requestId)
	{
		if (this.UpdateAlarmValuesEvent != null)
		{
			this.UpdateAlarmValuesEvent(requestId);
		}
	}

	private void FireUpdateAlarm(int requestId)
	{
		if (this.UpdateAlarmEvent != null)
		{
			this.UpdateAlarmEvent(requestId);
		}
	}

	private void FireUpdateMultipleAlarms(int requestId)
	{
		if (this.UpdateMultipleAlarmsEvent != null)
		{
			this.UpdateMultipleAlarmsEvent(requestId);
		}
	}

	private void FireAcknowledge(int requestId)
	{
		if (this.AcknowledgeEvent != null)
		{
			this.AcknowledgeEvent(requestId);
		}
	}

	private void FireAcknowledgeMultipleAlarms(int requestId)
	{
		if (this.AcknowledgeMultipleAlarmsEvent != null)
		{
			this.AcknowledgeMultipleAlarmsEvent(requestId);
		}
	}

	private void FireStartAlarmLineSession(int requestId, Guid sessionId)
	{
		if (this.StartAlarmLineSessionEvent != null)
		{
			this.StartAlarmLineSessionEvent(requestId, sessionId);
		}
	}

	private void FireStopAlarmLineSession(int requestId)
	{
		if (this.StopAlarmLineSessionEvent != null)
		{
			this.StopAlarmLineSessionEvent(requestId);
		}
	}

	private void FireGetSessionAlarmLines(int requestId, AlarmUpdateData alarmUpdateData)
	{
		if (this.GetSessionAlarmLinesEvent != null)
		{
			this.GetSessionAlarmLinesEvent(requestId, alarmUpdateData);
		}
	}

	private void FireGetAlarmLines(int requestId, AlarmLine[] alarmLines)
	{
		if (this.GetAlarmLinesEvent != null)
		{
			this.GetAlarmLinesEvent(requestId, alarmLines);
		}
	}

	private void FireStartEventLineSession(int requestId, Guid sessionId)
	{
		if (this.StartEventLineSessionEvent != null)
		{
			this.StartEventLineSessionEvent(requestId, sessionId);
		}
	}

	private void FireStopEventLineSession(int requestId)
	{
		if (this.StopEventLineSessionEvent != null)
		{
			this.StopEventLineSessionEvent(requestId);
		}
	}

	private void FireGetSessionEventLines(int requestId, EventUpdateData eventUpdateData)
	{
		if (this.GetSessionEventLinesEvent != null)
		{
			this.GetSessionEventLinesEvent(requestId, eventUpdateData);
		}
	}

	private void FireGetEventLines(int requestId, EventLine[] eventLines)
	{
		if (this.GetEventLinesEvent != null)
		{
			this.GetEventLinesEvent(requestId, eventLines);
		}
	}

	private void FireGetStatistics(int requestId, Statistic[] statistics)
	{
		if (this.GetStatisticsEvent != null)
		{
			this.GetStatisticsEvent(requestId, statistics);
		}
	}

	private void FireGetAlarmHistory(int requestId, AlarmHistory[] alarmHistory)
	{
		if (this.GetAlarmHistoryEvent != null)
		{
			this.GetAlarmHistoryEvent(requestId, alarmHistory);
		}
	}

	private void FireGetAlarmUpdateHistory(int requestId, AlarmUpdate[] alarmUpdate)
	{
		if (this.GetAlarmUpdateHistoryEvent != null)
		{
			this.GetAlarmUpdateHistoryEvent(requestId, alarmUpdate);
		}
	}

	private void FireGetAlarmMessages(int requestId, string[] messages)
	{
		if (this.GetAlarmMessagesEvent != null)
		{
			this.GetAlarmMessagesEvent(requestId, messages);
		}
	}
}

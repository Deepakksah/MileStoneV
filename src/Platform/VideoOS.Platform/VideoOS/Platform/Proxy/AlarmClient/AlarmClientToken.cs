using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Proxy.AlarmClient;

public class AlarmClientToken : IAlarmClient
{
	private AlarmCommandTokenClient _client;

	private readonly int MESSAGE_BUFFER_SIZE = 500000000;

	private LoginSettings _loginSettings;

	private BasicHttpBinding _binding;

	private EndpointAddress _ep;

	private Version _eventServerVersion = new Version(1, 0);

	public CommunicationState State => _client.State;

	public AlarmClientToken(ServerId serverId, string eventServerAddr, Version eventServerVersion)
	{
		_eventServerVersion = eventServerVersion;
		Uri uri = new Uri(new Uri(eventServerAddr), "Central/AlarmServiceToken");
		if (uri.Port == 0)
		{
			uri = new UriBuilder(eventServerAddr.StartsWith("https", StringComparison.OrdinalIgnoreCase) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, uri.Host, 22331, "Central/AlarmServiceToken").Uri;
		}
		string absoluteUri = uri.AbsoluteUri;
		_loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		_ep = new EndpointAddress(absoluteUri);
		_binding = new BasicHttpBinding();
		if (uri.Scheme == Uri.UriSchemeHttps)
		{
			_binding.Security.Mode = BasicHttpSecurityMode.Transport;
		}
		_binding.MaxReceivedMessageSize = MESSAGE_BUFFER_SIZE;
		_binding.MaxBufferSize = MESSAGE_BUFFER_SIZE;
		_binding.MaxBufferPoolSize = MESSAGE_BUFFER_SIZE;
		_binding.ReaderQuotas.MaxArrayLength = MESSAGE_BUFFER_SIZE;
		_binding.ReaderQuotas.MaxBytesPerRead = MESSAGE_BUFFER_SIZE;
		_binding.ReaderQuotas.MaxNameTableCharCount = MESSAGE_BUFFER_SIZE;
		_binding.ReaderQuotas.MaxDepth = MESSAGE_BUFFER_SIZE;
		_binding.ReaderQuotas.MaxStringContentLength = MESSAGE_BUFFER_SIZE;
		CreateClient();
	}

	public void Add(VideoOS.Platform.Data.Alarm alarm)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.Add(_loginSettings.Token, alarm);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void AddAsEvent(VideoOS.Platform.Data.Alarm alarm)
	{
		EnvironmentManager.Instance.Log(error: true, "NewAlarmAsEventCommand", "NewAlarmAsEventCommand is no longer supported. Please, use NewEventCommand instead.", null);
	}

	public void AddEvent(BaseEvent baseEvent)
	{
		if (_eventServerVersion != null)
		{
			int num = 3;
			while (num-- > 0)
			{
				try
				{
					_client.AddEvent(_loginSettings.Token, baseEvent);
					break;
				}
				catch (Exception)
				{
					if (num == 0)
					{
						throw;
					}
					CreateClient();
				}
			}
		}
		else
		{
			AddAsEvent(AlarmBuilder.CreateAlarmFromEvent(baseEvent));
		}
	}

	public VideoOS.Platform.Data.Alarm Get(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.Get(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public BaseEvent GetEvent(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetEvent(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public EventHeader GetHeader(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetHeader(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public SnapshotList GetSnapshots(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetSnapshots(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public void AttachSnapshot(Guid id, Snapshot snapshot)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.AttachSnapshot(_loginSettings.Token, id, snapshot);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void UpdateAlarm(Guid id, string text, int state, int priority, DateTime time, string assignedTo)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.UpdateAlarm(_loginSettings.Token, id, text, state, priority, time, assignedTo);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void UpdateMultipleAlarms(Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.UpdateMultipleAlarms(_loginSettings.Token, objectId, oldState, newState, time, assignedTo);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void Acknowledge(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.Acknowledge(_loginSettings.Token, id);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void AcknowledgeMultipleAlarms(Guid objectId)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.AcknowledgeMultipleAlarms(_loginSettings.Token, objectId);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public AlarmLine[] GetAlarmLines(int from, int maxCount, AlarmFilter filter)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetAlarmLines(_loginSettings.Token, from, maxCount, filter);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public EventLine[] GetEventLines(int from, int maxCount, EventFilter filter)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetEventLines(_loginSettings.Token, from, maxCount, filter);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public Statistic[] GetStatistics()
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetStatistics(_loginSettings.Token);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public AlarmHistory[] GetAlarmHistory(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetAlarmHistory(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public string[] GetAlarmMessages()
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetAlarmMessages(_loginSettings.Token);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public void OpenClient()
	{
		try
		{
			_client.InnerChannel.Open();
			long num = DateTime.Now.Ticks + 1000000;
			while (DateTime.Now.Ticks < num && _client.State == CommunicationState.Opening)
			{
				Thread.Sleep(5);
			}
			if (_client.State == CommunicationState.Opening && _client.State == CommunicationState.Opened)
			{
				EnvironmentManager.Instance.Log(error: true, GetType().Name, string.Format(CultureInfo.InvariantCulture, "AlarmClientToken was not opened correctly. State: {0}", _client.State.ToString()));
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void CloseClient()
	{
		_client.Close();
	}

	public AlarmUpdate[] GetAlarmUpdateHistory(Guid id)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetAlarmUpdateHistory(_loginSettings.Token, id);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public AlarmUpdateData GetSessionAlarmLines(int maxCount, Guid sessionId)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetSessionAlarmLines(_loginSettings.Token, maxCount, sessionId);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public EventUpdateData GetSessionEventLines(int maxCount, Guid sessionId)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.GetSessionEventLines(_loginSettings.Token, maxCount, sessionId);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return null;
	}

	public Guid StartAlarmLineSession(AlarmFilter filter)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.StartAlarmLineSession(_loginSettings.Token, filter);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return default(Guid);
	}

	public void StopAlarmLineSession(Guid sessionId)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.StopAlarmLineSession(_loginSettings.Token, sessionId);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public Guid StartEventLineSession(EventFilter filter)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				return _client.StartEventLineSession(_loginSettings.Token, filter);
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
		return default(Guid);
	}

	public void StopEventLineSession(Guid sessionId)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.StopEventLineSession(_loginSettings.Token, sessionId);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	public void UpdateAlarmValues(Guid id, KeyValuePair<string, string>[] updates)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.UpdateAlarmValues(_loginSettings.Token, id, updates);
				break;
			}
			catch (Exception)
			{
				if (num == 0)
				{
					throw;
				}
				CreateClient();
			}
		}
	}

	private void CreateClient()
	{
		_client = new AlarmCommandTokenClient(_binding, _ep);
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Proxy.AlarmClient;

public class AlarmClientCredentials : IAlarmClient, IDisposable
{
	private AlarmCommandClient _client;

	private const int MESSAGE_BUFFER_SIZE = 500000000;

	private const int MAX_ITEMS = 1000000;

	private LoginSettings _loginSettings;

	private Version _eventServerVersion;

	private Version _secureAlarmServiceVersion = new Version(10, 2);

	private Binding _binding;

	private EndpointAddress _endPointAddress;

	public CommunicationState State
	{
		get
		{
			if (_client == null)
			{
				CreateClient();
			}
			return _client.State;
		}
	}

	public AlarmClientCredentials(ServerId serverId, string eventServerAddr, Version eventServerVersion)
	{
		_eventServerVersion = eventServerVersion;
		_loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		if (eventServerVersion == null || eventServerVersion < _secureAlarmServiceVersion)
		{
			Uri address = new Uri(new Uri(eventServerAddr), "Central/AlarmService");
			_binding = CreateBasicHttpBindingCredentials();
			_endPointAddress = GetEndPointAddress(address);
			CreateClient();
		}
		else
		{
			Uri uri = new Uri(new Uri(eventServerAddr), "Central/AlarmService2");
			_binding = CreateWSHttpBindingCredentials((uri.Scheme == Uri.UriSchemeHttps) ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message);
			_endPointAddress = GetEndPointAddress(uri);
			CreateClient();
		}
	}

	public void Add(VideoOS.Platform.Data.Alarm alarm)
	{
		int num = 3;
		while (num-- > 0)
		{
			try
			{
				_client.Add(alarm);
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
					_client.AddEvent(baseEvent);
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
				return _client.Get(id);
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
				return _client.GetEvent(id);
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
				return _client.GetHeader(id);
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
				return _client.GetSnapshots(id);
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
				_client.AttachSnapshot(id, snapshot);
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
				_client.UpdateAlarm(id, text, state, priority, time, assignedTo);
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
				_client.UpdateMultipleAlarms(objectId, oldState, newState, time, assignedTo);
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
				_client.Acknowledge(id);
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
				_client.AcknowledgeMultipleAlarms(objectId);
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
				return _client.GetAlarmLines(from, maxCount, filter);
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
				return _client.GetEventLines(from, maxCount, filter);
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
				return _client.GetStatistics();
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
				return _client.GetAlarmHistory(id);
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
				return _client.GetAlarmMessages();
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
				EnvironmentManager.Instance.Log(error: true, GetType().Name, string.Format(CultureInfo.InvariantCulture, "AlarmClientCredential was not opened correctly. State: {0}", _client.State.ToString()));
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
				return _client.GetAlarmUpdateHistory(id);
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
				return _client.GetSessionAlarmLines(maxCount, sessionId);
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
				return _client.GetSessionEventLines(maxCount, sessionId);
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
				return _client.StartAlarmLineSession(filter);
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
				_client.StopAlarmLineSession(sessionId);
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
				return _client.StartEventLineSession(filter);
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
				_client.StopEventLineSession(sessionId);
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
				_client.UpdateAlarmValues(id, updates);
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

	private static BasicHttpBinding CreateBasicHttpBindingCredentials()
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBindingPlain();
		basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
		basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
		return basicHttpBinding;
	}

	private static BasicHttpBinding CreateBasicHttpBindingPlain()
	{
		return new BasicHttpBinding
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferSize = 500000000,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			}
		};
	}

	private static WSHttpBinding CreateWSHttpBindingCredentials(SecurityMode bindingSecurityMode)
	{
		WSHttpBinding wSHttpBinding = CreateWSHttpBindingPlain();
		wSHttpBinding.Security.Mode = bindingSecurityMode;
		wSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
		wSHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
		wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.Security.Message.EstablishSecurityContext = false;
		wSHttpBinding.Security.Message.NegotiateServiceCredential = true;
		return wSHttpBinding;
	}

	private static WSHttpBinding CreateWSHttpBindingPlain()
	{
		return new WSHttpBinding(SecurityMode.None)
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			}
		};
	}

	private static EndpointAddress GetEndPointAddress(Uri address)
	{
		string spn = SpnFactory.GetSpn(address);
		return new EndpointAddress(address, new SpnEndpointIdentity(spn));
	}

	private void CreateClient()
	{
		_client = new AlarmCommandClient(_binding, _endPointAddress);
		foreach (OperationDescription operation in _client.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
			}
		}
		_client.ClientCredentials.Windows.ClientCredential = _loginSettings.NetworkCredential;
	}

	public void Dispose()
	{
		if (_client == null)
		{
			return;
		}
		try
		{
			if (_client.State != CommunicationState.Faulted)
			{
				_client.Close();
			}
		}
		finally
		{
			if (_client.State != CommunicationState.Closed)
			{
				_client.Abort();
			}
		}
		_client = null;
	}
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

namespace VideoOS.Platform.SDK.StatusClient;

public class SystemStatusClient
{
	private readonly ConcurrentDictionary<FQID, StatusSession> _sessions = new ConcurrentDictionary<FQID, StatusSession>();

	private readonly MemoryCache _dedupeCache = new MemoryCache("StatusApiDedupe");

	private readonly HashSet<Guid> _userdefinedEvents = new HashSet<Guid>();

	private readonly object _dummyCacheValue = new object();

	private readonly FQID _managementServerId;

	private readonly object _stateLock = new object();

	private object _regObj1;

	private Collection<Guid> _deviceKinds = new Collection<Guid>();

	public event EventHandler<StatusSessionChangesArgs> ConnectionStateChanged;

	public event EventHandler<StatusApiEventArgs> ConfigurationChanged;

	public event EventHandler<EventFiredEventArgs> EventFired;

	public event EventHandler<CameraStateChangedEventArgs> CameraStateChanged;

	public event EventHandler<IODeviceStateChangedEventArgs> InputDeviceStateChanged;

	public event EventHandler<IODeviceStateChangedEventArgs> OutputDeviceStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> MicrophoneStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> SpeakerStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> MetadataStateChanged;

	public event EventHandler<HardwareStateChangedEventArgs> HardwareStateChanged;

	public SystemStatusClient(FQID managementServerId)
	{
		_managementServerId = managementServerId;
	}

	public void CreateAndStartSessions()
	{
		if (_regObj1 == null)
		{
			_regObj1 = EnvironmentManager.Instance.RegisterReceiver(LocalConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
		}
		List<Item> children = _managementServerId.ServerId.UserContext.Configuration.GetItem(_managementServerId).GetChildren();
		try
		{
			lock (_stateLock)
			{
				foreach (Item item in children)
				{
					if (item.FQID.Kind == Kind.Server && StatusSession.ServerSupportsStatusApi(item.FQID.ServerId))
					{
						HandleRecordingServerItem(item);
					}
					if (item.FQID.Kind == Kind.TriggerEvent)
					{
						HandleUserDefinedEventsItem(item);
					}
				}
			}
		}
		catch (Exception)
		{
			StopAndRemoveSessions();
			throw;
		}
	}

	public void StopAndRemoveSessions()
	{
		StopAndRemoveSessions(waitForsessionCompletetion: false);
	}

	public void StopAndRemoveSessions(bool waitForsessionCompletetion)
	{
		if (_regObj1 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
		}
		_regObj1 = null;
		lock (_stateLock)
		{
			foreach (StatusSession value in _sessions.Values)
			{
				value.Dispose();
			}
			if (waitForsessionCompletetion)
			{
				foreach (StatusSession value2 in _sessions.Values)
				{
					value2.WaitForSessionCompletion();
				}
			}
			_sessions.Clear();
			_userdefinedEvents.Clear();
			_deviceKinds.Clear();
		}
	}

	public void SetSubscribedEventsOnCreatedSessions(ISet<Guid> eventGuids)
	{
		if (eventGuids == null)
		{
			throw new ArgumentNullException("eventGuids");
		}
		lock (_stateLock)
		{
			foreach (StatusSession value in _sessions.Values)
			{
				value.SetSubscribedEvents(eventGuids);
			}
		}
	}

	public void AddSubscriptionsToDevicesOfKindOnCreatedSessions(Guid deviceKind)
	{
		lock (_stateLock)
		{
			if (!_deviceKinds.Contains(deviceKind))
			{
				_deviceKinds.Add(deviceKind);
			}
			foreach (StatusSession value in _sessions.Values)
			{
				value.AddSubscriptionsToDevicesOfKind(deviceKind);
			}
		}
	}

	public void SubscribeToConfigurationChanges(bool subscribe)
	{
		lock (_stateLock)
		{
			foreach (StatusSession value in _sessions.Values)
			{
				value.SetConfigurationChangesSubscription(subscribe);
			}
		}
	}

	public ReadOnlyCollection<StatusEventMessages> GetAllStatusEventMessages()
	{
		Collection<StatusEventMessages> collection = new Collection<StatusEventMessages>();
		if (_managementServerId.ServerId.UserContext.Configuration is SDKConfiguration sDKConfiguration)
		{
			sDKConfiguration.GetItem(_managementServerId);
			ISystemInfo systemInfo = sDKConfiguration.GetSystemInfo(_managementServerId.ServerId.Id);
			if (systemInfo != null)
			{
				ISystemEventTypeInfo[] systemEventTypes = systemInfo.ConfigurationInfo.SystemEventTypes;
				foreach (ISystemEventTypeInfo systemEventTypeInfo in systemEventTypes)
				{
					collection.Add(new StatusEventMessages
					{
						Id = systemEventTypeInfo.EventTypeId,
						StatusEventName = systemEventTypeInfo.Name,
						EventMessageName = EventMapper.StatusEventGuidToBaseEventMessageId(systemEventTypeInfo.EventTypeId, systemEventTypeInfo.Name)
					});
				}
			}
		}
		return new ReadOnlyCollection<StatusEventMessages>(collection);
	}

	private object LocalConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list)
		{
			List<StatusSession> list2 = new List<StatusSession>();
			foreach (FQID item3 in list)
			{
				StatusSession statusSessionServerId = GetStatusSessionServerId(item3.ServerId);
				if (statusSessionServerId != null)
				{
					if ((statusSessionServerId.DeviceKinds.Contains(item3.Kind) || CheckSubscribedElseWhere(item3)) && !statusSessionServerId.ContainsDeviceId(item3.ObjectId) && !list2.Contains(statusSessionServerId))
					{
						list2.Add(statusSessionServerId);
					}
					continue;
				}
				Item item = _managementServerId.ServerId.UserContext.Configuration.GetItem(item3.ServerId, item3.ServerId.Id, Kind.Server);
				if (item != null && StatusSession.ServerSupportsStatusApi(item.FQID.ServerId))
				{
					HandleRecordingServerItem(item);
					StatusSession item2 = _sessions[item.FQID];
					list2.Add(item2);
				}
			}
			foreach (StatusSession item4 in list2)
			{
				foreach (Guid deviceKind in _deviceKinds)
				{
					item4.AddSubscriptionsToDevicesOfKind(deviceKind);
				}
			}
		}
		return null;
	}

	private StatusSession GetStatusSessionServerId(ServerId serverId)
	{
		if (_sessions != null)
		{
			foreach (FQID key in _sessions.Keys)
			{
				if (key.ServerId.Id == serverId.Id)
				{
					return _sessions[key];
				}
			}
		}
		return null;
	}

	private bool CheckSubscribedElseWhere(FQID fqid)
	{
		if (_sessions != null)
		{
			foreach (StatusSession value in _sessions.Values)
			{
				if (value.ContainsDeviceId(fqid.ObjectId))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void HandleUserDefinedEventsItem(Item item)
	{
		foreach (Item child in item.GetChildren())
		{
			_userdefinedEvents.Add(child.FQID.ObjectId);
		}
	}

	private void HandleRecordingServerItem(Item item)
	{
		StatusSession statusSession = CreateStatusSession(item);
		if (!_sessions.TryAdd(item.FQID, statusSession))
		{
			EnvironmentManager.Instance.Log(error: true, GetType().FullName, "Status API server with FQID " + ((object)item.FQID)?.ToString() + " already registered");
		}
		statusSession.StartSession();
	}

	private StatusSession CreateStatusSession(Item item)
	{
		StatusSession statusSession = new StatusSession(item);
		statusSession.EventFired += StatusSessionOnEventFired;
		statusSession.ConnectionStateChanged += StatusSessionOnConnectionStateChanged;
		statusSession.ConfigurationChanged += StatusSessionOnConfigurationChanged;
		statusSession.CameraStateChanged += StatusSessionOnCameraStateChanged;
		statusSession.InputDeviceStateChanged += StatusSessionOnInputDeviceStateChanged;
		statusSession.MicrophoneStateChanged += StatusSessionOnMicrophoneStateChanged;
		statusSession.OutputDeviceStateChanged += StatusSessionOnOutputDeviceStateChanged;
		statusSession.SpeakerStateChanged += StatusSessionOnSpeakerStateChanged;
		statusSession.MetadataStateChanged += StatusSessionOnMetadataStateChanged;
		statusSession.HardwareStateChanged += StatusSessionOnHardwareStateChanged;
		return statusSession;
	}

	private void StatusSessionOnHardwareStateChanged(object sender, HardwareStateChangedEventArgs eventArgs)
	{
		this.HardwareStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnMetadataStateChanged(object sender, MediaStreamDeviceStateChangedEventArgs eventArgs)
	{
		this.MetadataStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnSpeakerStateChanged(object sender, MediaStreamDeviceStateChangedEventArgs eventArgs)
	{
		this.SpeakerStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnOutputDeviceStateChanged(object sender, IODeviceStateChangedEventArgs eventArgs)
	{
		this.OutputDeviceStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnMicrophoneStateChanged(object sender, MediaStreamDeviceStateChangedEventArgs eventArgs)
	{
		this.MicrophoneStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnInputDeviceStateChanged(object sender, IODeviceStateChangedEventArgs eventArgs)
	{
		this.InputDeviceStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnCameraStateChanged(object sender, CameraStateChangedEventArgs eventArgs)
	{
		this.CameraStateChanged?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnConnectionStateChanged(object sender, VideoOS.Platform.SDK.Config.ConnectionStateChangedEventArgs eventArgs)
	{
		StatusSessionChangesArgs e = new StatusSessionChangesArgs((sender as StatusSession).RecorderServer, eventArgs.ConnectionState);
		this.ConnectionStateChanged?.Invoke(this, e);
	}

	private void StatusSessionOnEventFired(object sender, EventFiredEventArgs eventArgs)
	{
		if (EventCanHaveDuplicates(eventArgs))
		{
			string uniqueKey = eventArgs.GetUniqueKey();
			if (_dedupeCache.AddOrGetExisting(uniqueKey, _dummyCacheValue, DateTimeOffset.Now.AddMinutes(1.0)) != null)
			{
				return;
			}
		}
		this.EventFired?.Invoke(this, eventArgs);
	}

	private void StatusSessionOnConfigurationChanged(object sender, StatusApiEventArgs eventArgs)
	{
		string uniqueKey = eventArgs.GetUniqueKey();
		if (_dedupeCache.AddOrGetExisting(uniqueKey, _dummyCacheValue, DateTimeOffset.Now.AddMinutes(1.0)) == null)
		{
			this.ConfigurationChanged?.Invoke(this, eventArgs);
		}
	}

	private bool EventCanHaveDuplicates(EventFiredEventArgs eventArgs)
	{
		lock (_stateLock)
		{
			return _userdefinedEvents.Contains(eventArgs.EventId);
		}
	}
}

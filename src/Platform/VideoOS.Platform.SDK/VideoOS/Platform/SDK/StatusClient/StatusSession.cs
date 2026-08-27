using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Proxy.Status2.Net;
using VideoOS.Platform.SDK.StatusClient.StatusEventArgs;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.StatusClient;

public class StatusSession : IDisposable
{
	private class AsyncPoller
	{
		private const int ServerTimeoutInMilliseconds = 5000;

		private readonly StatusSession _parent;

		private readonly Lazy<Thread> _listenerThread;

		private RecorderStatusService2 _soapClient;

		private Guid? _statusSessionId;

		private readonly LoginSettings _loginSettings;

		private volatile HashSet<Guid> _idsOfNextDevicesToSubscribe;

		private volatile HashSet<Guid> _idsOfNextEventsToSubscribe;

		private volatile HashSet<Guid> _idsOfNextHardwareToSubscribe;

		private volatile bool _subscribeConfigurationChanges;

		private volatile bool _devicesSubscribedOnServer;

		private volatile bool _hardwareSubscribedOnServer;

		private volatile bool _eventsSubscribedOnServer;

		private volatile bool _configurationChangesSubscribedOnServer;

		private volatile bool _shuttingDown;

		private static readonly TimeSpan InitialBackoffTimer = new TimeSpan(0L);

		private TimeSpan _backOffInterval = InitialBackoffTimer;

		private readonly TimeSpan _maxBackOffTime = TimeSpan.FromSeconds(30.0);

		private bool _isInvalidTokenLogged;

		public AsyncPoller(StatusSession parent)
		{
			_parent = parent;
			_loginSettings = _parent.GetLoginSettingsForServer();
			_listenerThread = new Lazy<Thread>(() => new Thread(CommunicationThreadMainLoop)
			{
				Name = $"Status API listener thread for endpoint {_parent._recorderServer.Name}"
			}, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public void StartPolling()
		{
			_listenerThread.Value.Start();
		}

		public void Shutdown()
		{
			_shuttingDown = true;
		}

		public void SetSubscribedDevices(IEnumerable<Guid> nextDevicesToSubscribes)
		{
			HashSet<Guid> idsOfNextDevicesToSubscribe = new HashSet<Guid>(nextDevicesToSubscribes);
			_idsOfNextDevicesToSubscribe = idsOfNextDevicesToSubscribe;
			SignalDeviceSubscriptionChanged();
		}

		public void SetSubscribedHardware(IEnumerable<Guid> nextHardwareToSubscribes)
		{
			HashSet<Guid> idsOfNextHardwareToSubscribe = new HashSet<Guid>(nextHardwareToSubscribes);
			_idsOfNextHardwareToSubscribe = idsOfNextHardwareToSubscribe;
			SignalHardwareSubscriptionChanged();
		}

		public bool ContainsSubscribedDeviceId(Guid id)
		{
			if (_idsOfNextDevicesToSubscribe != null)
			{
				return _idsOfNextDevicesToSubscribe.Contains(id);
			}
			return false;
		}

		public void SetSubscribedEvents(IEnumerable<Guid> nextEventsToSubscribe)
		{
			HashSet<Guid> idsOfNextEventsToSubscribe = new HashSet<Guid>(nextEventsToSubscribe);
			_idsOfNextEventsToSubscribe = idsOfNextEventsToSubscribe;
			SignalEventSubscriptionChanged();
		}

		public void WaitForSessionCompletion()
		{
			if (_listenerThread.IsValueCreated)
			{
				_listenerThread.Value.Join();
			}
		}

		public void SetConfigurationChangesSubscription(bool subscribe)
		{
			_subscribeConfigurationChanges = subscribe;
			SignalConfigurationChangesSubcriptionChanged();
		}

		private void CommunicationThreadMainLoop()
		{
			while (!_shuttingDown)
			{
				try
				{
					StartSoapSessionIfNotPresent();
					StartSessionIfNotPresent();
					RegisterSubscriptionsIfNotRegistered();
					PollStatus();
					ResetBackOffTimer();
					_isInvalidTokenLogged = false;
				}
				catch (FaultException ex)
				{
					if (ex.Code != null && ex.Code.Name == "40000")
					{
						_parent.ConnectionState = ConnectStates.NotConnected;
						ResetSession();
						EnvironmentManager.Instance.Log(error: false, GetType().FullName, "status session lost - creating new status session");
						goto IL_0175;
					}
					if (ex.Code != null && ex.Code.Name == "20001")
					{
						if (!_isInvalidTokenLogged)
						{
							EnvironmentManager.Instance.Log(error: false, GetType().FullName, $"Invalid token from server (Note: Only logged once). Server name: {_parent._recorderServer.Name}");
							_isInvalidTokenLogged = true;
						}
						_loginSettings.FireRequestNewToken();
						goto IL_0175;
					}
					if (ex.Code != null && ex.Code.Name == "Server")
					{
						ResetSession();
						EnvironmentManager.Instance.Log(error: false, GetType().FullName, "status session lost - creating new status session. Exception message: " + ex.Message);
						continue;
					}
					EnvironmentManager.Instance.ExceptionHandler(GetType().FullName, string.Format(CultureInfo.InvariantCulture, "Error communicating with the status API at server {0}", _parent._recorderServer.Name), ex);
					goto IL_0175;
					IL_0175:
					WaitWithBackoff();
				}
				catch (Exception exception)
				{
					_parent.ConnectionState = ConnectStates.NotConnected;
					ResetSoapSession();
					EnvironmentManager.Instance.ExceptionHandler(GetType().FullName, string.Format(CultureInfo.InvariantCulture, "Error communicating with the status API at server {0}", _parent._recorderServer.Name), exception);
					WaitWithBackoff();
				}
			}
			StopRemoteSession();
		}

		private void StartSoapSessionIfNotPresent()
		{
			if (_soapClient == null)
			{
				EnvironmentManager.Instance.Log(error: false, GetType().Name, "Creating StatusAPI SOAP for: " + _parent._recorderServer.FQID.ServerId.Uri);
				Uri recorderCommandUri = GetRecorderCommandUri();
				_soapClient = new RecorderStatusService2(ManagementServerWcfHelper.GetNonOAuthBinding(recorderCommandUri.Scheme == Uri.UriSchemeHttps), ManagementServerWcfHelper.GetEndpointAddress(recorderCommandUri));
				ManagementServerWcfHelper.SetupCertificateValidation(_soapClient);
				_soapClient.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(15.0);
			}
		}

		private Uri GetRecorderCommandUri()
		{
			ServerId serverId = _parent._recorderServer.FQID.ServerId;
			return new UriBuilder(serverId.ServerScheme, serverId.ServerHostname, serverId.Serverport, "RecorderStatusService/RecorderStatusService2.asmx").Uri;
		}

		private void ResetSoapSession()
		{
			if (_soapClient != null)
			{
				if (_parent != null && _parent._recorderServer != null && _parent._recorderServer.FQID != null)
				{
					EnvironmentManager.Instance.Log(error: false, GetType().Name, "Disposing StatusAPI SOAP for: " + _parent._recorderServer.FQID.ServerId.Uri);
				}
				else
				{
					EnvironmentManager.Instance.Log(error: false, GetType().Name, "Disposing StatusAPI SOAP");
				}
				_soapClient.InnerChannel.Dispose();
				_soapClient = null;
			}
		}

		private void SignalDeviceSubscriptionChanged()
		{
			_devicesSubscribedOnServer = false;
		}

		private void SignalHardwareSubscriptionChanged()
		{
			_hardwareSubscribedOnServer = false;
		}

		private void SignalEventSubscriptionChanged()
		{
			_eventsSubscribedOnServer = false;
		}

		private void SignalConfigurationChangesSubcriptionChanged()
		{
			_configurationChangesSubscribedOnServer = false;
		}

		private void ResetSession()
		{
			if (_parent != null && _parent._recorderServer != null && _parent._recorderServer.FQID != null)
			{
				EnvironmentManager.Instance.Log(error: false, GetType().Name, "Resetting StatusAPI virtual session for: " + _parent._recorderServer.FQID.ServerId.Uri);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, GetType().Name, "Resetting StatusAPI virtual session");
			}
			_statusSessionId = null;
		}

		private void ResetBackOffTimer()
		{
			_backOffInterval = InitialBackoffTimer;
		}

		private void WaitWithBackoff()
		{
			if (_backOffInterval == InitialBackoffTimer)
			{
				_backOffInterval = TimeSpan.FromSeconds(1.0);
				return;
			}
			Thread.Sleep(_backOffInterval);
			if (_backOffInterval != _maxBackOffTime)
			{
				TimeSpan timeSpan = _backOffInterval.Add(_backOffInterval);
				_backOffInterval = ((_maxBackOffTime < timeSpan) ? _maxBackOffTime : timeSpan);
			}
		}

		private static ArrayOfGuid CreateArrayOfGuid(IEnumerable<Guid> guids)
		{
			ArrayOfGuid arrayOfGuid = new ArrayOfGuid();
			arrayOfGuid.AddRange(guids);
			return arrayOfGuid;
		}

		private void RegisterSubscriptionsIfNotRegistered()
		{
			if (!_devicesSubscribedOnServer)
			{
				if (_idsOfNextDevicesToSubscribe != null)
				{
					_soapClient.SubscribeDeviceStatus(_loginSettings.Token, _statusSessionId.Value, CreateArrayOfGuid(_idsOfNextDevicesToSubscribe));
				}
				_devicesSubscribedOnServer = true;
			}
			if (!_hardwareSubscribedOnServer)
			{
				if (_idsOfNextHardwareToSubscribe != null)
				{
					_soapClient.SubscribeHardwareStatus(_loginSettings.Token, _statusSessionId.Value, CreateArrayOfGuid(_idsOfNextHardwareToSubscribe));
				}
				_hardwareSubscribedOnServer = true;
			}
			if (!_eventsSubscribedOnServer)
			{
				if (_idsOfNextEventsToSubscribe != null)
				{
					_soapClient.SubscribeEventStatus(_loginSettings.Token, _statusSessionId.Value, CreateArrayOfGuid(_idsOfNextEventsToSubscribe));
				}
				_eventsSubscribedOnServer = true;
			}
			if (!_configurationChangesSubscribedOnServer)
			{
				_soapClient.SubscribeConfigurationStatus(_loginSettings.Token, _statusSessionId.Value, _subscribeConfigurationChanges);
				_configurationChangesSubscribedOnServer = true;
			}
		}

		private void StartSessionIfNotPresent()
		{
			if (!_statusSessionId.HasValue)
			{
				_parent.ConnectionState = ConnectStates.TryingToLogOn;
				UpdateProxyHostToHandleFailover();
				_statusSessionId = _soapClient.StartStatusSession(_loginSettings.Token);
				_parent.ConnectionState = ConnectStates.Connected;
				SignalDeviceSubscriptionChanged();
				SignalEventSubscriptionChanged();
				SignalConfigurationChangesSubcriptionChanged();
				EnvironmentManager.Instance.Log(error: false, GetType().Name, "Creating StatusAPI virtual session for: " + _soapClient.Endpoint.Address.Uri.ToString());
			}
		}

		private void UpdateProxyHostToHandleFailover()
		{
			Uri recorderCommandUri = GetRecorderCommandUri();
			_soapClient.Endpoint.Address = ManagementServerWcfHelper.GetEndpointAddress(recorderCommandUri);
		}

		private void PollStatus()
		{
			Status status = _soapClient.GetStatus(_loginSettings.Token, _statusSessionId.Value, 5000);
			if (!_shuttingDown)
			{
				FireEventsFrom(status);
			}
		}

		private void FireEventsFrom(Status status)
		{
			if (status.ConfigurationChangedStatus != null)
			{
				_parent.OnConfigurationChanged(status.ConfigurationChangedStatus);
			}
			ApplyActionToElements(status.EventStatusArray, _parent.OnEventFired);
			ApplyActionToElements(status.CameraDeviceStatusArray, _parent.OnCameraStateChanged);
			ApplyActionToElements(status.InputDeviceStatusArray, _parent.OnInputDeviceStateChanged);
			ApplyActionToElements(status.MicrophoneDeviceStatusArray, _parent.OnMicrophoneStateChanged);
			ApplyActionToElements(status.OutputDeviceStatusArray, _parent.OnOutputDeviceStateChanged);
			ApplyActionToElements(status.SpeakerDeviceStatusArray, _parent.OnSpeakerStateChanged);
			ApplyActionToElements(status.MetadataDeviceStatusArray, _parent.OnMetadataStateChanged);
			ApplyActionToElements(status.HardwareStatusArray, _parent.OnHardwareStateChanged);
		}

		private static void ApplyActionToElements<T>(IEnumerable<T> elements, Action<T> action)
		{
			if (elements == null)
			{
				return;
			}
			foreach (T element in elements)
			{
				action(element);
			}
		}

		private void StopRemoteSession()
		{
			if (_statusSessionId.HasValue && _soapClient != null)
			{
				try
				{
					_soapClient.StopStatusSession(_loginSettings.Token, _statusSessionId.Value);
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler(GetType().FullName, "Failed to stop remote session - server will have to clean up by itself", exception);
				}
			}
		}
	}

	private volatile ConnectStates _connectState = ConnectStates.NotConnected;

	private HashSet<Guid> _idsOfSubscribedDevices = new HashSet<Guid>();

	private HashSet<Guid> _idsOfSubscribedEvents = new HashSet<Guid>();

	private HashSet<Guid> _idsOfSubscribedHardware = new HashSet<Guid>();

	private readonly EventArgsMapper _eventArgsMapper = new EventArgsMapper();

	private readonly Collection<Guid> _deviceKinds = new BindingList<Guid>();

	private bool _disposed;

	private readonly Item _recorderServer;

	private readonly AsyncPoller _asyncPoller;

	public Collection<Guid> DeviceKinds => _deviceKinds;

	[Browsable(false)]
	public ConnectStates ConnectionState
	{
		get
		{
			return _connectState;
		}
		private set
		{
			if (value != _connectState)
			{
				_connectState = value;
				OnConnectionStateChanged(value);
			}
		}
	}

	public Item RecorderServer => _recorderServer;

	public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

	public event EventHandler<StatusApiEventArgs> ConfigurationChanged;

	public event EventHandler<EventFiredEventArgs> EventFired;

	public event EventHandler<CameraStateChangedEventArgs> CameraStateChanged;

	public event EventHandler<IODeviceStateChangedEventArgs> InputDeviceStateChanged;

	public event EventHandler<IODeviceStateChangedEventArgs> OutputDeviceStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> MicrophoneStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> SpeakerStateChanged;

	public event EventHandler<MediaStreamDeviceStateChangedEventArgs> MetadataStateChanged;

	public event EventHandler<HardwareStateChangedEventArgs> HardwareStateChanged;

	public StatusSession(Item recorderServer)
	{
		if (recorderServer == null)
		{
			throw new ArgumentNullException("recorderServer");
		}
		if (!ServerSupportsStatusApi(recorderServer.FQID.ServerId))
		{
			throw new ArgumentException("The supplied Item does not point to a server with a Status API Service", "recorderServer");
		}
		_recorderServer = recorderServer;
		_asyncPoller = new AsyncPoller(this);
	}

	public static bool ServerSupportsStatusApi(ServerId serverId)
	{
		return serverId.ServerType == "XPCORS";
	}

	public ReadOnlyCollection<StatusEventMessages> GetAllStatusEventMessages()
	{
		Collection<StatusEventMessages> collection = new Collection<StatusEventMessages>();
		if (_recorderServer.GetParent().FQID.ServerId.UserContext.Configuration is SDKConfiguration sDKConfiguration)
		{
			ISystemInfo systemInfo = sDKConfiguration.GetSystemInfo(_recorderServer.GetParent().FQID.ServerId.Id);
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

	public void StartSession()
	{
		_asyncPoller.StartPolling();
	}

	public ISet<Guid> AddSubscriptionsToDevicesOfKind(Guid deviceKind)
	{
		List<Item> children = _recorderServer.GetChildren();
		HashSet<Guid> hashSet = new HashSet<Guid>(from item in FindAllDevicesOfKind(children, deviceKind)
			select item.FQID.ObjectId);
		if (!_deviceKinds.Contains(deviceKind))
		{
			_deviceKinds.Add(deviceKind);
		}
		if (hashSet.Any())
		{
			_idsOfSubscribedDevices.UnionWith(hashSet);
			_asyncPoller.SetSubscribedDevices(_idsOfSubscribedDevices);
		}
		return hashSet;
	}

	public void SetSubscribedDevicesForStateChanges(ISet<Guid> deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		if (deviceIds.Count == 0)
		{
			throw new ArgumentException("deviceIds cannot be the empty set", "deviceIds");
		}
		_idsOfSubscribedDevices = new HashSet<Guid>(deviceIds);
		_asyncPoller.SetSubscribedDevices(_idsOfSubscribedDevices);
	}

	public void SetSubscribedHardwareForStateChanges(ISet<Guid> hardwareIds)
	{
		if (hardwareIds == null)
		{
			throw new ArgumentNullException("hardwareIds");
		}
		if (hardwareIds.Count == 0)
		{
			throw new ArgumentException("hardwareIds cannot be the empty set", "hardwareIds");
		}
		_idsOfSubscribedHardware = new HashSet<Guid>(hardwareIds);
		_asyncPoller.SetSubscribedHardware(_idsOfSubscribedHardware);
	}

	public bool IsDeviceBeingSubscribed(Guid id)
	{
		return _idsOfSubscribedDevices.Contains(id);
	}

	public void SetSubscribedEvents(ISet<Guid> eventIds)
	{
		if (eventIds == null || eventIds.Count == 0)
		{
			throw new ArgumentNullException("eventIds");
		}
		if (eventIds.Count == 0)
		{
			throw new ArgumentException("eventIds cannot be the empty set", "eventIds");
		}
		_idsOfSubscribedEvents = new HashSet<Guid>(eventIds);
		_asyncPoller.SetSubscribedEvents(_idsOfSubscribedEvents);
	}

	public void SetConfigurationChangesSubscription(bool subscribe)
	{
		_asyncPoller.SetConfigurationChangesSubscription(subscribe);
	}

	private IEnumerable<Item> FindAllDevicesOfKind(IEnumerable<Item> items, Guid deviceKind)
	{
		foreach (Item item in items)
		{
			if (item.FQID.Kind == deviceKind && item.FQID.ParentId == _recorderServer.FQID.ServerId.Id && item.FQID.FolderType == FolderType.No)
			{
				yield return item;
			}
			else
			{
				if (item.FQID.FolderType == FolderType.No)
				{
					continue;
				}
				foreach (Item item2 in FindAllDevicesOfKind(item.GetChildren(), deviceKind))
				{
					yield return item2;
				}
			}
		}
	}

	public bool ContainsDeviceId(Guid id)
	{
		return _asyncPoller.ContainsSubscribedDeviceId(id);
	}

	public void WaitForSessionCompletion()
	{
		_asyncPoller.WaitForSessionCompletion();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_asyncPoller.Shutdown();
			if (disposing)
			{
				ConnectionState = ConnectStates.NotConnected;
			}
			StopAllEvents();
			_idsOfSubscribedDevices = null;
			_idsOfSubscribedEvents = null;
			_disposed = true;
		}
	}

	private void StopAllEvents()
	{
		this.CameraStateChanged = null;
		this.ConfigurationChanged = null;
		this.ConnectionStateChanged = null;
		this.EventFired = null;
		this.InputDeviceStateChanged = null;
		this.MicrophoneStateChanged = null;
		this.OutputDeviceStateChanged = null;
		this.SpeakerStateChanged = null;
		this.MetadataStateChanged = null;
		this.HardwareStateChanged = null;
	}

	~StatusSession()
	{
		Dispose(disposing: false);
	}

	private void OnConnectionStateChanged(ConnectStates state)
	{
		this.ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(state));
		EnvironmentManager.Instance.Log(error: false, GetType().FullName, "Connection state changed to " + state.ToString() + ", for:" + ((_recorderServer != null) ? _recorderServer.Name : ""));
	}

	private void OnEventFired(EventStatus eventStatus)
	{
		EventHandler<EventFiredEventArgs> eventHandler = this.EventFired;
		if (eventHandler != null)
		{
			EventFiredEventArgs e = EventArgsMapper.MapToEventFiredEventArgs(eventStatus);
			eventHandler(this, e);
		}
	}

	private void OnCameraStateChanged(CameraDeviceStatus cameraDeviceStatus)
	{
		EventHandler<CameraStateChangedEventArgs> eventHandler = this.CameraStateChanged;
		if (eventHandler != null)
		{
			CameraStateChangedEventArgs e = EventArgsMapper.MapToCameraStateEventArgs(cameraDeviceStatus);
			eventHandler(this, e);
		}
	}

	private void OnInputDeviceStateChanged(InputDeviceStatus inputDeviceStatus)
	{
		EventHandler<IODeviceStateChangedEventArgs> eventHandler = this.InputDeviceStateChanged;
		if (eventHandler != null)
		{
			IODeviceStateChangedEventArgs e = EventArgsMapper.MapToIODeviceStateChangedEventArgs(inputDeviceStatus);
			eventHandler(this, e);
		}
	}

	private void OnOutputDeviceStateChanged(OutputDeviceStatus outputStatus)
	{
		EventHandler<IODeviceStateChangedEventArgs> eventHandler = this.OutputDeviceStateChanged;
		if (eventHandler != null)
		{
			IODeviceStateChangedEventArgs e = EventArgsMapper.MapToIODeviceStateChangedEventArgs(outputStatus);
			eventHandler(this, e);
		}
	}

	private void OnMicrophoneStateChanged(MediaStreamDeviceStatusBase microphoneStatus)
	{
		EventHandler<MediaStreamDeviceStateChangedEventArgs> eventHandler = this.MicrophoneStateChanged;
		if (eventHandler != null)
		{
			MediaStreamDeviceStateChangedEventArgs e = EventArgsMapper.ToMediaStreamDeviceStateChangedEventArgs(microphoneStatus);
			eventHandler(this, e);
		}
	}

	private void OnSpeakerStateChanged(MediaStreamDeviceStatusBase speakerStatus)
	{
		EventHandler<MediaStreamDeviceStateChangedEventArgs> eventHandler = this.SpeakerStateChanged;
		if (eventHandler != null)
		{
			MediaStreamDeviceStateChangedEventArgs e = EventArgsMapper.ToMediaStreamDeviceStateChangedEventArgs(speakerStatus);
			eventHandler(this, e);
		}
	}

	private void OnMetadataStateChanged(MediaStreamDeviceStatusBase metadataStatus)
	{
		EventHandler<MediaStreamDeviceStateChangedEventArgs> eventHandler = this.MetadataStateChanged;
		if (eventHandler != null)
		{
			MediaStreamDeviceStateChangedEventArgs e = EventArgsMapper.ToMediaStreamDeviceStateChangedEventArgs(metadataStatus);
			eventHandler(this, e);
		}
	}

	private void OnHardwareStateChanged(HardwareStatus hardwareStatus)
	{
		EventHandler<HardwareStateChangedEventArgs> eventHandler = this.HardwareStateChanged;
		if (eventHandler != null)
		{
			HardwareStateChangedEventArgs e = EventArgsMapper.ToHardwareStateChangedEventArgs(hardwareStatus);
			eventHandler(this, e);
		}
	}

	private void OnConfigurationChanged(ConfigurationChangedStatus configurationChangedStatus)
	{
		EventHandler<StatusApiEventArgs> eventHandler = this.ConfigurationChanged;
		if (eventHandler != null)
		{
			StatusApiEventArgs e = EventArgsMapper.MapToStatusApiEventArgs(configurationChangedStatus);
			eventHandler(this, e);
		}
	}

	private LoginSettings GetLoginSettingsForServer()
	{
		return _recorderServer.FQID.ServerId.UserContext.GetLoginSettings(_recorderServer.FQID);
	}
}

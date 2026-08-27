using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.AdaptiveStreaming;
using VideoOS.Platform.Util.ToolkitXmlProviders;

namespace VideoOS.Platform.Live;

public abstract class BaseLiveSource<TLiveContent, TLiveContentEventArgs> : LiveSource where TLiveContent : class where TLiveContentEventArgs : BaseLiveContentEventArgs, new()
{
	private bool _running;

	private bool _applicationWishLiveStart;

	private bool _errorRecovery;

	private bool _closeRequest;

	private bool _initialized;

	private Thread _thread;

	private object _sessionId;

	private readonly TimeSpan _timeoutSpan = TimeSpan.FromSeconds(3.0);

	private LiveStatus _liveStatus = new LiveStatus(StatusFlags.ClientLiveStopped);

	private LoginSettings _loginSettings;

	private bool _changeToken;

	private string _currentToken;

	private string _media;

	private bool _multicastEnabled;

	private readonly StreamSelectionParams _streamSelectionParams = new StreamSelectionParams();

	private IEnumerable<Guid> _supportedItemTypes;

	private bool _multicastConfigDone;

	private bool _clientSetCameraConnectionLostFlag;

	private bool _informAboutAllFlags;

	public override Item Item
	{
		get
		{
			return base.Item;
		}
		set
		{
			SetItemInternal(value);
		}
	}

	public IEnumerable<Guid> SupportedItemTypes
	{
		get
		{
			return _supportedItemTypes;
		}
		set
		{
			if (value == null || !value.Any())
			{
				throw new MIPException("List of supported item types is empty");
			}
			_supportedItemTypes = value;
		}
	}

	public bool IsRawSource { get; set; }

	public Guid StreamId { get; set; }

	public object SessionId => _sessionId;

	public bool MulticastEnabled
	{
		get
		{
			return _multicastEnabled;
		}
		protected set
		{
			_multicastEnabled = value;
		}
	}

	public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromMilliseconds(-1.0);

	public bool SendInitialImage { get; set; } = true;

	public virtual IStreamSelectionParams StreamSelectionParams => _streamSelectionParams;

	public override bool LiveModeStart
	{
		get
		{
			return base.LiveModeStart;
		}
		set
		{
			_applicationWishLiveStart = value;
			if (_applicationWishLiveStart)
			{
				StatusFlagManager.Register(this, Item.FQID.ObjectId);
			}
			if (_errorRecovery && _sessionId != null && !_running && _thread == null)
			{
				_running = true;
				StartPollingThread();
			}
		}
	}

	public StatusFlags CurrentStatusFlags => _liveStatus.CurrentStatus;

	public bool IsInitialized => _initialized;

	public override event EventHandler LiveContentEvent;

	public override event EventHandler LiveStatusEvent;

	protected BaseLiveSource(Item item, IEnumerable<Guid> supportedTypes, bool isRawSource)
		: base(item)
	{
		SupportedItemTypes = supportedTypes;
		SetItemInternal(item);
		IsRawSource = isRawSource;
	}

	public override void Init()
	{
		if (base.Item == null)
		{
			throw new MIPException("Item has not been initialized");
		}
		if (base.Item.FQID.Kind != Kind.Camera && base.Item.FQID.Kind != Kind.Microphone && base.Item.FQID.Kind != Kind.Speaker && base.Item.FQID.Kind != Kind.Metadata)
		{
			throw new MIPException("Invalid Item Kind - only Camera, Microphone or Speaker");
		}
		_media = base.Item.FQID.Kind.ToMedia();
		if (!_errorRecovery && _thread == null)
		{
			_closeRequest = false;
			base.Init();
			_errorRecovery = true;
			_running = false;
			StartPollingThread();
			SetupTokenRefresh();
			_initialized = true;
		}
	}

	private void SetupTokenRefresh()
	{
		_loginSettings = LoginSettingsCache.GetLoginSettings(Item.FQID);
		if (_loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "BaseLiveSource", "Unable to setup Token refresh - LoginSettings not found", null);
		}
		else
		{
			_loginSettings.TokenChangedEvent += TokenChangedEvent;
		}
	}

	private void SetItemInternal(Item item)
	{
		Guid kind = item.FQID.Kind;
		if (!SupportedItemTypes.Contains(kind))
		{
			throw new MIPException("Not supported Item Kind. Initilize corectly supportedTypes or the item.");
		}
		if (base.Item.FQID.Kind != Kind.Camera && base.Item.FQID.Kind != Kind.Microphone && base.Item.FQID.Kind != Kind.Metadata && base.Item.FQID.Kind != Kind.Speaker)
		{
			throw new MIPException("Can only accept Camera, Microphone, Metadata or Speaker kind of Item");
		}
		base.Item = item;
	}

	public override void Close()
	{
		_closeRequest = true;
		base.Close();
		if (_running || _errorRecovery)
		{
			_running = false;
			_errorRecovery = false;
			if (_loginSettings != null)
			{
				_loginSettings.TokenChangedEvent -= TokenChangedEvent;
			}
			if (_thread != null && _thread.IsAlive)
			{
				_running = false;
				_errorRecovery = false;
			}
		}
		_initialized = false;
	}

	private void TokenChangedEvent(object sender, EventArgs e)
	{
		_currentToken = ((LoginSettings)sender).Token;
		_changeToken = true;
	}

	[Obsolete("Deprecated. May be deleted in the future. Avoid using it")]
	public void ReleaseObject(IDisposable usedObject)
	{
	}

	protected abstract void OnUpdateLiveStart();

	private int UpdateLiveStart(int timeoutCounter)
	{
		try
		{
			if (_sessionId != null)
			{
				OnUpdateLiveStart();
				StartStopLiveStream(_applicationWishLiveStart);
				base.LiveModeStart = _applicationWishLiveStart;
				timeoutCounter = 0;
				StatusFlags initialFlags = StatusFlagManager.GetInitialFlags(this, Item.FQID.ObjectId);
				_liveStatus = new LiveStatus(initialFlags);
				OnLiveStatusEvent(new LiveStatusEventArgs
				{
					ChangedStatusFlags = _liveStatus.CurrentStatus,
					CurrentStatusFlags = _liveStatus.CurrentStatus
				});
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "BaseLiveSource:PollThreadPaused", message, args);
			HandleLiveContentException(ex);
			_running = false;
			base.LiveModeStart = false;
			_errorRecovery = true;
			WaitForRetry();
		}
		return timeoutCounter;
	}

	protected abstract void OnUpdateSettings();

	private void PollThread()
	{
		bool flag = true;
		int num = 0;
		StatusFlagManager.Register(this, Item.FQID.ObjectId);
		while ((_running || _errorRecovery) && !_closeRequest)
		{
			if (!_errorRecovery)
			{
				if (base.LiveModeStart != _applicationWishLiveStart)
				{
					num = UpdateLiveStart(num);
				}
				try
				{
					if (_sessionId != null)
					{
						if (_changeToken)
						{
							_changeToken = false;
							InternalMediaService.Instance.SetNewToken(_sessionId, _currentToken);
						}
						if (base.LiveModeStart)
						{
							OnUpdateSettings();
							OnStreamSelectionChange();
						}
					}
				}
				catch (Exception)
				{
				}
				if (!base.LiveModeStart)
				{
					Thread.Sleep(25);
				}
				else if (!ProcessNextFrame())
				{
					Thread.Sleep(1);
				}
				else
				{
					try
					{
						object obj = InternalMediaService.Instance.LiveGet(_sessionId, _timeoutSpan, IsRawSource);
						if (obj is LiveSourceTimeout)
						{
							num++;
							if (num > 5)
							{
								throw new CommunicationMIPException("No video received within 15 seconds");
							}
						}
						else if (obj is TLiveContent)
						{
							num = 0;
							ProcessingNewFrame();
							HandleLiveDataContent(obj as TLiveContent);
						}
						else if (obj is LiveSourceStatus)
						{
							num = 0;
							HandleLiveStatusContent(obj as LiveSourceStatus);
						}
					}
					catch (Exception ex2)
					{
						EnvironmentManager instance = EnvironmentManager.Instance;
						string message = ex2.Message;
						object[] args = new Exception[1] { ex2 };
						instance.Log(error: true, "BaseLiveSource:PollThread", message, args);
						HandleLiveContentException(ex2);
						_running = false;
						base.LiveModeStart = false;
						_errorRecovery = true;
						WaitForRetry();
					}
				}
			}
			if (!_errorRecovery || _closeRequest)
			{
				continue;
			}
			if (_sessionId != null)
			{
				try
				{
					InternalMediaService.Instance.MarkSessionForNoReuse(_sessionId);
					InternalMediaService.Instance.LiveDisconnect(_sessionId);
					_sessionId = null;
				}
				catch
				{
				}
			}
			if (MulticastEnabled)
			{
				_multicastConfigDone = false;
				InternalMediaService.Instance.MulticastConfigDoneEvent += Instance_MulticastConfigDoneEvent;
				if (InternalMediaService.Instance.SetupMulticast(Item.FQID))
				{
					for (int i = 0; i < 30; i++)
					{
						if (_closeRequest)
						{
							break;
						}
						if (_multicastConfigDone)
						{
							break;
						}
						Thread.Sleep(100);
					}
					if (!_closeRequest && !_multicastConfigDone)
					{
						MulticastEnabled = false;
					}
				}
				else
				{
					MulticastEnabled = false;
				}
				InternalMediaService.Instance.MulticastConfigDoneEvent -= Instance_MulticastConfigDoneEvent;
			}
			try
			{
				_informAboutAllFlags = true;
				_sessionId = ConnectLiveSource(_media);
				StatusFlagManager.Register(this, Item.FQID.ObjectId);
				if (_sessionId != null)
				{
					StartStopLiveStream(LiveModeStart);
					_running = true;
					_errorRecovery = false;
					num = 0;
				}
			}
			catch (Exception ex3)
			{
				if (flag)
				{
					flag = false;
					HandleLiveContentException(ex3);
				}
				WaitForRetry();
			}
		}
		if (_sessionId != null)
		{
			try
			{
				StopAndDisconnectLiveSource();
				_sessionId = null;
			}
			catch
			{
			}
		}
		StatusFlagManager.UnRegister(this);
		_thread = null;
	}

	private void Instance_MulticastConfigDoneEvent(object sender, InternalMediaService.MulticastConfigDoneEventArgs e)
	{
		if (e.DeviceId == Item.FQID.ObjectId)
		{
			_multicastConfigDone = true;
		}
	}

	private void StartStopLiveStream(bool start)
	{
		if (StreamSelectionParams == null)
		{
			InternalMediaService.Instance.SetLiveModeStart(_sessionId, start);
			return;
		}
		_streamSelectionParams.Changed = false;
		InternalMediaService.Instance.SetLiveModeStart(_sessionId, start, StreamSelectionParams);
	}

	private void OnStreamSelectionChange()
	{
		if (StreamSelectionParams != null && _streamSelectionParams.Changed)
		{
			_streamSelectionParams.Changed = false;
			InternalMediaService.Instance.SetStreamSelectionParams(_sessionId, _streamSelectionParams);
		}
	}

	protected void ConnectAndStartLiveSource()
	{
		_sessionId = ConnectLiveSource(_media);
		StartStopLiveStream(_applicationWishLiveStart);
	}

	protected void StopAndDisconnectLiveSource()
	{
		StartStopLiveStream(start: false);
		InternalMediaService.Instance.LiveDisconnect(_sessionId);
	}

	private void WaitForRetry()
	{
		for (int i = 0; i < 30; i++)
		{
			if (_closeRequest)
			{
				break;
			}
			Thread.Sleep(100);
		}
	}

	protected abstract void HandleLiveDataContent(TLiveContent data);

	private void HandleLiveStatusContent(LiveSourceStatus lss)
	{
		StatusFlags initialFlags = StatusFlagManager.GetInitialFlags(this, Item.FQID.ObjectId);
		StatusFlags statusFlags = _liveStatus.UpdateStatus(lss, initialFlags);
		if (_clientSetCameraConnectionLostFlag)
		{
			_clientSetCameraConnectionLostFlag = false;
			statusFlags |= StatusFlags.CameraConnectionLost;
			_liveStatus.CurrentStatus = (_liveStatus.CurrentStatus &= ~StatusFlags.CameraConnectionLost);
		}
		if (_informAboutAllFlags)
		{
			statusFlags = StatusFlags.All;
			_informAboutAllFlags = false;
		}
		StatusFlagManager.StoreFlags(this, Item.FQID.ObjectId, _liveStatus.CurrentStatus);
		OnLiveStatusEvent(new LiveStatusEventArgs
		{
			ChangedStatusFlags = statusFlags,
			CurrentStatusFlags = _liveStatus.CurrentStatus
		});
	}

	private void HandleLiveContentException(Exception ex)
	{
		if (!_closeRequest)
		{
			StatusFlags statusFlags = StatusFlags.CameraConnectionLost | StatusFlags.ClientLiveStopped;
			_clientSetCameraConnectionLostFlag = true;
			StatusFlagManager.StoreFlags(this, Item.FQID.ObjectId, statusFlags);
			OnLiveStatusEvent(new LiveStatusEventArgs
			{
				ChangedStatusFlags = StatusFlags.All,
				CurrentStatusFlags = statusFlags
			});
			OnLiveContentEvent(CreateExceptionEventArgs(ex));
		}
	}

	public virtual EventArgs CreateExceptionEventArgs(Exception ex)
	{
		return new TLiveContentEventArgs
		{
			Exception = ex
		};
	}

	protected abstract object ConnectLiveSource(string media);

	protected void OnLiveStatusEvent(EventArgs eventArgs)
	{
		EventHandler eventHandler = LiveStatusEvent;
		if (eventHandler != null && !_closeRequest)
		{
			eventHandler(this, eventArgs);
		}
		else
		{
			_informAboutAllFlags = true;
		}
	}

	protected bool OnLiveContentEvent(EventArgs eventArgs)
	{
		bool result = false;
		EventHandler eventHandler = LiveContentEvent;
		if (eventHandler != null && !_closeRequest)
		{
			eventHandler(this, eventArgs);
			result = true;
		}
		return result;
	}

	private void StartPollingThread()
	{
		_thread = new Thread(PollThread)
		{
			Name = "RawLiveSource.PollThread",
			IsBackground = true
		};
		_thread.Start();
	}
}

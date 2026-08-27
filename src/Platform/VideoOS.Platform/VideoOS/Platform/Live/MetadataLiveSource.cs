using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Metadata;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class MetadataLiveSource
{
	private readonly Item _item;

	private LiveStatus _liveStatus = new LiveStatus(StatusFlags.ClientLiveStopped);

	private LoginSettings _loginSettings;

	private volatile bool _errorRecovery;

	private Thread _thread;

	private volatile bool _running;

	private string _currentToken;

	private bool _changeToken;

	private volatile bool _closeRequest;

	private volatile bool _applicationWishLiveStart;

	private bool _liveModeStart;

	private object _sessionId;

	private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(1.0);

	private object _regObj1;

	public bool LiveModeStart
	{
		get
		{
			return _liveModeStart;
		}
		set
		{
			_applicationWishLiveStart = value;
			if (_applicationWishLiveStart)
			{
				StatusFlagManager.Register(this, _item.FQID.ObjectId);
			}
			if (_errorRecovery && _sessionId != null && !_running && _thread == null)
			{
				_running = true;
				StartPollingThread();
			}
		}
	}

	public event Action<MetadataLiveSource, MetadataLiveContent> LiveContentEvent;

	public event Action<MetadataLiveSource, Exception> ErrorEvent;

	public event Action<MetadataLiveSource, LiveStatusEventArgs> LiveStatusEvent;

	public MetadataLiveSource(Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID.Kind != Kind.Metadata)
		{
			throw new MIPException("Can only accept Metadata kind of Item");
		}
		_item = item;
		new TrackPageViewData(typeof(MetadataLiveSource)).Send();
	}

	public void Close()
	{
		_closeRequest = true;
		if (_regObj1 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
		}
		_regObj1 = null;
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
	}

	public void Init()
	{
		if (!_errorRecovery && _thread == null)
		{
			_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
			_errorRecovery = true;
			_running = false;
			StartPollingThread();
			_loginSettings = LoginSettingsCache.GetLoginSettings(_item.FQID);
			if (_loginSettings == null)
			{
				EnvironmentManager.Instance.Log(error: true, "MetadataLiveSource", "Unable to setup Token refresh up - LoginSettings not found", null);
			}
			else
			{
				_loginSettings.TokenChangedEvent += TokenChangedEvent;
			}
		}
	}

	private void PollThread()
	{
		bool flag = true;
		int num = 0;
		while ((_running || _errorRecovery) && !_closeRequest)
		{
			if (!_errorRecovery)
			{
				if (_liveModeStart != _applicationWishLiveStart)
				{
					try
					{
						if (_sessionId != null)
						{
							InternalMediaService.Instance.SetLiveModeStart(_sessionId, _applicationWishLiveStart);
							_liveModeStart = _applicationWishLiveStart;
							num = 0;
							_liveStatus = new LiveStatus(StatusFlagManager.GetInitialFlags(this, _item.FQID.ObjectId));
							OnLiveStatusEvent(new LiveStatusEventArgs
							{
								ChangedStatusFlags = _liveStatus.CurrentStatus,
								CurrentStatusFlags = _liveStatus.CurrentStatus
							});
						}
					}
					catch (Exception ex)
					{
						LogLocal("MetadataLiveSource:PollThreadPaused", ex);
						OnErrorEvent(ex);
						_running = false;
						_liveModeStart = false;
						_errorRecovery = true;
					}
				}
				try
				{
					if (_sessionId != null && _liveModeStart && _changeToken)
					{
						_changeToken = false;
						InternalMediaService.Instance.SetNewToken(_sessionId, _currentToken);
					}
				}
				catch (Exception)
				{
				}
				if (!_liveModeStart)
				{
					Thread.Yield();
				}
				else
				{
					try
					{
						object obj = InternalMediaService.Instance.LiveGet(_sessionId, Timeout);
						if (obj is LiveSourceContent)
						{
							num = 0;
							HandleLiveSourceContent(obj as LiveSourceContent);
						}
						else if (obj is LiveSourceStatus)
						{
							num = 0;
							HandleLiveSourceStatus(obj as LiveSourceStatus);
						}
						else if (obj is LiveSourceTimeout)
						{
							num++;
							if (num > 5)
							{
								throw new CommunicationMIPException("No metadata received within 15 seconds");
							}
						}
					}
					catch (Exception ex3)
					{
						LogLocal("MetadataLiveSource:PollThread", ex3);
						OnErrorEvent(ex3);
						_running = false;
						_liveModeStart = false;
						_errorRecovery = true;
						WaitForRetry();
					}
				}
				Thread.Sleep(1);
			}
			if (!_errorRecovery)
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
				catch (Exception ex4)
				{
					LogLocal("MetadataLiveSource:PollThread", ex4);
				}
			}
			try
			{
				_sessionId = InternalMediaService.Instance.LiveVideoRawConnect(_item.FQID, Guid.Empty, "METADATA", requestSignatures: false);
				StatusFlagManager.Register(this, _item.FQID.ObjectId);
				if (_sessionId != null)
				{
					InternalMediaService.Instance.SetLiveModeStart(_sessionId, LiveModeStart);
					_running = true;
					_errorRecovery = false;
				}
			}
			catch (Exception exception)
			{
				if (flag)
				{
					flag = false;
					OnErrorEvent(exception);
				}
				WaitForRetry();
			}
		}
		if (_sessionId != null)
		{
			try
			{
				InternalMediaService.Instance.LiveDisconnect(_sessionId);
				_sessionId = null;
			}
			catch (Exception ex5)
			{
				LogLocal("MetadataLiveSource:PollThread.Disconnect", ex5);
			}
		}
		StatusFlagManager.UnRegister(this);
		_thread = null;
	}

	private void LogLocal(string where, Exception ex)
	{
		if (_closeRequest || EnvironmentManager.Instance == null)
		{
			return;
		}
		try
		{
			EnvironmentManager.Instance.Log(error: true, where, ex.Message, new object[1] { ex });
		}
		catch
		{
		}
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

	private void HandleLiveSourceStatus(LiveSourceStatus lss)
	{
		StatusFlags initialFlags = StatusFlagManager.GetInitialFlags(this, _item.FQID.ObjectId);
		StatusFlags changedStatusFlags = _liveStatus.UpdateStatus(lss, initialFlags);
		StatusFlagManager.StoreFlags(this, _item.FQID.ObjectId, _liveStatus.CurrentStatus);
		OnLiveStatusEvent(new LiveStatusEventArgs
		{
			ChangedStatusFlags = changedStatusFlags,
			CurrentStatusFlags = _liveStatus.CurrentStatus
		});
	}

	private void HandleLiveSourceContent(LiveSourceContent sourceContent)
	{
		MetadataLiveContent eventArgs = new MetadataLiveContent(sourceContent.BeginTime, sourceContent.EndTime, new MetadataContent(sourceContent.Content), sourceContent.Properties);
		sourceContent.Dispose();
		OnLiveContentEvent(eventArgs);
	}

	private void TokenChangedEvent(object sender, EventArgs e)
	{
		_currentToken = ((LoginSettings)sender).Token;
		_changeToken = true;
	}

	private void OnErrorEvent(Exception exception)
	{
		this.ErrorEvent?.Invoke(this, exception);
		LiveStatusEventArgs e = new LiveStatusEventArgs();
		e.ChangedStatusFlags = StatusFlags.All;
		e.CurrentStatusFlags = StatusFlags.CameraConnectionLost;
		_liveStatus.CurrentStatus = StatusFlags.None;
		OnLiveStatusEvent(e);
	}

	private void OnLiveStatusEvent(LiveStatusEventArgs eventArgs)
	{
		Action<MetadataLiveSource, LiveStatusEventArgs> action = this.LiveStatusEvent;
		if (action != null && _running && !_closeRequest)
		{
			action(this, eventArgs);
		}
	}

	private void OnLiveContentEvent(MetadataLiveContent eventArgs)
	{
		Action<MetadataLiveSource, MetadataLiveContent> action = this.LiveContentEvent;
		if (action != null && _running && !_closeRequest)
		{
			action(this, eventArgs);
		}
	}

	private void StartPollingThread()
	{
		_thread = new Thread(PollThread)
		{
			Name = "MetadataLiveSource.PollThread"
		};
		_thread.IsBackground = true;
		_thread.Start();
	}

	private object SystemConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list)
		{
			foreach (FQID item2 in list)
			{
				if (item2.Kind == Kind.Metadata && item2.ObjectId == _item.FQID.ObjectId && item2.ServerId.UserContext == _item.FQID.ServerId.UserContext)
				{
					Item item = ((item2.ServerId.UserContext == null) ? Configuration.Instance.GetItem(item2) : item2.ServerId.UserContext.Configuration.GetItem(item2));
					if (item != null)
					{
						_item.FQID = item.FQID;
					}
					break;
				}
			}
		}
		return null;
	}
}

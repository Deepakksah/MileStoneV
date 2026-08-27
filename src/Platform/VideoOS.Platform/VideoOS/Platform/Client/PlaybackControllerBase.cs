using System;
using System.Collections;
using System.Globalization;
using System.Timers;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class PlaybackControllerBase : PlaybackController
{
	private readonly ServerId _playbackControllerServerId = new ServerId("BR", "localhost", 0, Guid.NewGuid());

	private DateTime _lastPlaybackTimeIndication = DateTime.MinValue;

	private Mode _mode = Mode.ClientPlayback;

	private PlaybackModeType _playbackMode = PlaybackModeType.Stop;

	private double _playbackSpeed = 1.0;

	private bool _skipGapsWhenPlaying = true;

	private bool _stopAtSequenceEnd;

	private readonly Hashtable _playbackImageTimeHashtable = new Hashtable();

	private readonly TimeSpan _maxGapBeforePlaybackSkipTimespan = new TimeSpan(0, 0, 2);

	private TimeSpan _connectionToCameraLostTimespan = new TimeSpan(0, 0, 10);

	private DateTime _playbackTime = DateTime.UtcNow;

	private readonly Timer _updateTimer = new Timer();

	private double _speed = 1.0;

	private DateTime _lastUpdate;

	private PlayState _play;

	private bool _loopSequence;

	private bool _loopRestarted;

	private DateTime _sequenceStartTime = DateTime.MinValue;

	private DateTime _sequenceEndTime = DateTime.MaxValue;

	private bool _initialized;

	private object _msg1;

	private object _msg2;

	private object _msg3;

	private object _msg4;

	private object _msg5;

	private bool _inTimerTick;

	private DateTime _lastLog = DateTime.Now;

	public Mode Mode => _mode;

	public override DateTime PlaybackTime
	{
		get
		{
			return _playbackTime;
		}
		set
		{
			ClearBrowseImageTimes();
			SetPlaybackTime(value, stopPlayback: false);
		}
	}

	public DateTime BrowseTime => _playbackTime;

	public override PlaybackModeType PlaybackMode
	{
		get
		{
			return _playbackMode;
		}
		set
		{
			if (_playbackMode != value)
			{
				_playbackMode = value;
				OnPlaybackModeChangedEvent(_playbackMode);
				ClearBrowseImageTimes();
				string command = "PlayStop";
				if (_playbackMode == PlaybackModeType.Forward)
				{
					command = "PlayForward";
				}
				if (_playbackMode == PlaybackModeType.Reverse)
				{
					command = "PlayReverse";
				}
				EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackIndication", new PlaybackCommandData
				{
					Command = command
				}), null, FQID);
				Play = ((_playbackMode == PlaybackModeType.Forward) ? PlayState.Forward : ((_playbackMode == PlaybackModeType.Reverse) ? PlayState.Reverse : PlayState.Stop));
			}
		}
	}

	public override double PlaybackSpeed
	{
		get
		{
			return _playbackSpeed;
		}
		set
		{
			if (value <= 32.0 && _playbackSpeed != value)
			{
				_playbackSpeed = value;
				if (EnvironmentManager.Instance.TracePlaybackDetails)
				{
					EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "PlaybackSpeed = " + _playbackSpeed.ToString(CultureInfo.InvariantCulture));
				}
				FirePlaybackSpeedChangedEvent(new PlaybackSpeedEventArgs(_playbackSpeed));
				Speed = value;
			}
		}
	}

	public override bool SkipGaps
	{
		get
		{
			return _skipGapsWhenPlaying;
		}
		set
		{
			_skipGapsWhenPlaying = value;
		}
	}

	private PlayState Play
	{
		get
		{
			return _play;
		}
		set
		{
			if (value != _play)
			{
				if (EnvironmentManager.Instance.TracePlaybackDetails)
				{
					EnvironmentManager.Instance.Log(error: false, "TraceDetail", "Play state changed to " + value);
				}
				_lastUpdate = DateTime.UtcNow;
				_play = value;
				_updateTimer.Enabled = Play != PlayState.Stop;
			}
		}
	}

	private double Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			double num = value;
			if (num >= 0.0)
			{
				if (num > 1.0)
				{
					num = (int)num;
				}
				if (_speed != num)
				{
					_speed = num;
					_updateTimer.Enabled = Play != PlayState.Stop;
					_updateTimer.Start();
				}
			}
		}
	}

	private bool IsPlaying => Play != PlayState.Stop;

	public event EventHandler<PlaybackSpeedEventArgs> PlaybackSpeedChangedEvent;

	public event EventHandler PlaybackForwardEvent;

	public event EventHandler CustomIgnoreSkipValueChangedEvent;

	public event EventHandler<TimeEventArgs> RecordedImageDisplayed;

	public event BrowseGotoChangedEventHandler BrowseGotoEvent;

	public event EventHandler BrowseNextEvent;

	public event EventHandler BrowsePreviousEvent;

	public event EventHandler BrowseNextSequenceEvent;

	public event EventHandler BrowsePreviousSequenceEvent;

	public event EventHandler BrowseBeginEvent;

	public event EventHandler BrowseEndEvent;

	public override event EventHandler<ProgressChangedEventArgs> SequenceProgressChanged;

	public PlaybackControllerBase()
	{
		FQID = new FQID(_playbackControllerServerId, Guid.Empty, Guid.NewGuid(), FolderType.No, Kind.PlaybackController);
	}

	public override void Init()
	{
		if (!_initialized)
		{
			_initialized = true;
			_updateTimer.Interval = 40.0;
			_updateTimer.Elapsed += _updateTimer_Elapsed;
			if (EnvironmentManager.Instance.TracePlaybackDetails)
			{
				EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "Init() Id=" + ((FQID == null) ? "Default" : FQID.ObjectId.ToString()));
			}
			InitializeEvent();
		}
	}

	public override void Close()
	{
		_updateTimer.Elapsed -= _updateTimer_Elapsed;
		if (EnvironmentManager.Instance.TracePlaybackDetails)
		{
			EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "Close() Id=" + ((FQID == null) ? "Default" : FQID.ObjectId.ToString()));
		}
		UnInitializeEvent();
	}

	private void InitializeEvent()
	{
		_msg1 = EnvironmentManager.Instance.RegisterReceiver(PlaybackCommandHandler, new PlaybackControllerReceiverFilter("SmartClient.PlaybackCommand", FQID));
		_msg2 = EnvironmentManager.Instance.RegisterReceiver(GetCurrentPlaybackTimeHandler, new PlaybackControllerReceiverFilter("SmartClient.GetCurrentPlaybackTimeRequest", FQID));
		_msg3 = EnvironmentManager.Instance.RegisterReceiver(ModeChangeCommandHandler, new PlaybackControllerReceiverFilter(MessageId.System.ModeChangeCommand, FQID));
		_msg4 = EnvironmentManager.Instance.RegisterReceiver(SkipModeChangeCommandHandler, new PlaybackControllerReceiverFilter("SmartClient.PlaybackSkipModeCommand", FQID));
		_msg5 = EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeInformationHandler, new PlaybackControllerReceiverFilter("SmartClient.PlaybackDeviceInformation", FQID));
	}

	private void UnInitializeEvent()
	{
		EnvironmentManager.Instance.UnRegisterReceiver(_msg1);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg2);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg3);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg4);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg5);
		_msg1 = (_msg2 = (_msg3 = (_msg4 = (_msg5 = null))));
	}

	private object SkipModeChangeCommandHandler(Message message, FQID dest, FQID sender)
	{
		switch ((PlaybackSkipModeData)message.Data)
		{
		case PlaybackSkipModeData.Noskip:
			_stopAtSequenceEnd = false;
			_skipGapsWhenPlaying = false;
			break;
		case PlaybackSkipModeData.Skip:
			_stopAtSequenceEnd = false;
			_skipGapsWhenPlaying = true;
			break;
		case PlaybackSkipModeData.StopAtSequenceEnd:
			_stopAtSequenceEnd = true;
			_skipGapsWhenPlaying = false;
			break;
		}
		return null;
	}

	private object ModeChangeCommandHandler(Message message, FQID dest, FQID sender)
	{
		_mode = (Mode)message.Data;
		if (_mode == Mode.ClientLive)
		{
			PlaybackMode = PlaybackModeType.Stop;
			Play = PlayState.Stop;
		}
		_ = _mode;
		_ = 3;
		EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.ModeChangedIndication, _mode), null, FQID);
		new TrackEventData(MessageId.System.ModeChangeCommand).Send();
		return null;
	}

	private object PlaybackCommandHandler(Message message, FQID dest, FQID sender)
	{
		if (message.Data is PlaybackCommandData playbackCommandData)
		{
			if (EnvironmentManager.Instance.TracePlaybackDetails)
			{
				EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "PlaybackCommandHandler() Id=" + ((FQID == null) ? "Default" : FQID.ObjectId.ToString()) + "Start - " + playbackCommandData.Command + "( Play=" + Play.ToString() + ", PlaybackMode=" + PlaybackMode.ToString() + ", Timer.Enabled=" + _updateTimer.Enabled + ")");
			}
			switch (playbackCommandData.Command)
			{
			case "Begin":
				ClearBrowseImageTimes();
				BrowseBegin();
				break;
			case "End":
				ClearBrowseImageTimes();
				BrowseEnd();
				break;
			case "Next":
				ClearBrowseImageTimes();
				BrowseNext();
				break;
			case "Previous":
				ClearBrowseImageTimes();
				BrowsePrevious();
				break;
			case "NextSequence":
				ClearBrowseImageTimes();
				BrowseNextSequence();
				break;
			case "PreviousSequence":
				ClearBrowseImageTimes();
				BrowsePreviousSequence();
				break;
			case "Play":
				if (playbackCommandData.Speed > 0.0)
				{
					PlaybackSpeed = playbackCommandData.Speed;
				}
				PlaybackMode = PlaybackModeType.Forward;
				Play = PlayState.Forward;
				break;
			case "PlayForward":
				if (playbackCommandData.Speed > 0.0)
				{
					PlaybackSpeed = playbackCommandData.Speed;
				}
				PlaybackMode = PlaybackModeType.Forward;
				Play = PlayState.Forward;
				break;
			case "PlayReverse":
				if (playbackCommandData.Speed > 0.0)
				{
					PlaybackSpeed = playbackCommandData.Speed;
				}
				PlaybackMode = PlaybackModeType.Reverse;
				Play = PlayState.Reverse;
				break;
			case "PlayStop":
				PlaybackSpeed = 1.0;
				PlaybackMode = PlaybackModeType.Stop;
				Play = PlayState.Stop;
				break;
			case "Speed":
				if (playbackCommandData.Speed > 0.0)
				{
					PlaybackSpeed = playbackCommandData.Speed;
				}
				break;
			case "Goto":
				ClearBrowseImageTimes();
				break;
			}
			EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackIndication", playbackCommandData), null, FQID);
			if (playbackCommandData.Command == "Goto")
			{
				BrowseGoto(playbackCommandData.DateTime);
				SetPlaybackTime(playbackCommandData.DateTime, stopPlayback: false);
			}
		}
		new TrackEventData("SmartClient.PlaybackCommand", deprecated: false, (message.Data is PlaybackCommandData) ? ((PlaybackCommandData)message.Data).Command : null).Send();
		return null;
	}

	private object PlaybackTimeInformationHandler(Message message, FQID dest, FQID sender)
	{
		if (message.Data is PlaybackTimeInformationData playbackTimeInformationData)
		{
			SetPlaybackImageTime(playbackTimeInformationData.Item.ObjectId, playbackTimeInformationData.PreviousTime, playbackTimeInformationData.CurrentTime, playbackTimeInformationData.NextTime);
		}
		return null;
	}

	private object GetCurrentPlaybackTimeHandler(Message message, FQID dest, FQID sender)
	{
		return PlaybackTime;
	}

	protected virtual void FirePlaybackSpeedChangedEvent(PlaybackSpeedEventArgs e)
	{
		if (this.PlaybackSpeedChangedEvent != null)
		{
			this.PlaybackSpeedChangedEvent(this, e);
		}
	}

	protected virtual void FirePlaybackForwardEvent()
	{
		if (this.PlaybackForwardEvent != null)
		{
			this.PlaybackForwardEvent(this, new EventArgs());
		}
	}

	protected virtual void FireCustomIgnoreSkipValueChangedEvent()
	{
		if (this.CustomIgnoreSkipValueChangedEvent != null)
		{
			this.CustomIgnoreSkipValueChangedEvent(this, new EventArgs());
		}
	}

	protected virtual void FireRecordedImageDisplayedEvent(DateTime displayedImageTime)
	{
		if (this.RecordedImageDisplayed != null)
		{
			this.RecordedImageDisplayed(this, new TimeEventArgs(displayedImageTime));
		}
	}

	public override void SetSequence(DateTime startTime, DateTime endTime)
	{
		_sequenceStartTime = startTime;
		_sequenceEndTime = endTime;
		_loopSequence = startTime < endTime;
		SignalSequenceProgress();
	}

	public void SetBrowseTime(DateTime browseTime, bool stopPlayback)
	{
		SetPlaybackTime(browseTime, stopPlayback);
	}

	public void SetPlaybackTime(DateTime playbackTime, bool stopPlayback)
	{
		playbackTime = EnsureUtcDateTime(playbackTime);
		if (stopPlayback)
		{
			PlaybackMode = PlaybackModeType.Stop;
		}
		if (DateTime.Compare(playbackTime, _playbackTime) == 0)
		{
			return;
		}
		DateTime dateTime = playbackTime;
		if (PlaybackMode != PlaybackModeType.Stop && !_loopRestarted && (SkipGaps || _stopAtSequenceEnd))
		{
			dateTime = GetNextGapSkippedBrowseTime(dateTime);
		}
		if (_loopRestarted)
		{
			ClearBrowseImageTimes();
			_loopRestarted = false;
		}
		if (_loopSequence)
		{
			if (dateTime > _sequenceEndTime)
			{
				dateTime = _sequenceStartTime;
				_loopRestarted = true;
			}
			else if (dateTime < _sequenceStartTime)
			{
				dateTime = _sequenceEndTime;
				_loopRestarted = true;
			}
		}
		_playbackTime = dateTime;
		if (EnvironmentManager.Instance.TracePlaybackDetails)
		{
			EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "PlaybackTime changed to " + _playbackTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
		}
		SignalSequenceProgress();
		OnPlaybackTimeChanged(_playbackTime);
		if (_playbackTime != _lastPlaybackTimeIndication)
		{
			if (EnvironmentManager.Instance.TracePlaybackDetails)
			{
				EnvironmentManager.Instance.Log(error: false, "PlaybackControllerBase", "PlaybackControllerPlaybackTimeChangedEvent " + _playbackTime.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
			}
			EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackCurrentTimeIndication", _playbackTime), null, FQID);
			_lastPlaybackTimeIndication = _playbackTime;
		}
	}

	public void BrowseGoto(DateTime gotoDateTime)
	{
		FireBrowseGotoEvent(gotoDateTime);
	}

	public void BrowseNext()
	{
		FireBrowseNextEvent();
	}

	public void BrowsePrevious()
	{
		FireBrowsePreviousEvent();
	}

	public void BrowseNextSequence()
	{
		FireBrowseNextSequenceEvent();
	}

	public void BrowsePreviousSequence()
	{
		FireBrowsePreviousSequenceEvent();
	}

	public void BrowseBegin()
	{
		FireBrowseBeginEvent();
	}

	public void BrowseEnd()
	{
		FireBrowseEndEvent();
	}

	protected virtual void FireBrowseGotoEvent(DateTime gotoDateTime)
	{
		if (this.BrowseGotoEvent != null)
		{
			this.BrowseGotoEvent(this, new TimeEventArgs(gotoDateTime));
		}
	}

	protected virtual void FireBrowseNextEvent()
	{
		if (this.BrowseNextEvent != null)
		{
			this.BrowseNextEvent(this, new EventArgs());
		}
	}

	protected virtual void FireBrowsePreviousEvent()
	{
		if (this.BrowsePreviousEvent != null)
		{
			this.BrowsePreviousEvent(this, new EventArgs());
		}
	}

	protected virtual void FireBrowseNextSequenceEvent()
	{
		if (this.BrowseNextSequenceEvent != null)
		{
			this.BrowseNextSequenceEvent(this, new EventArgs());
		}
	}

	protected virtual void FireBrowsePreviousSequenceEvent()
	{
		if (this.BrowsePreviousSequenceEvent != null)
		{
			this.BrowsePreviousSequenceEvent(this, new EventArgs());
		}
	}

	protected virtual void FireBrowseBeginEvent()
	{
		if (this.BrowseBeginEvent != null)
		{
			this.BrowseBeginEvent(this, new EventArgs());
		}
	}

	protected virtual void FireBrowseEndEvent()
	{
		if (this.BrowseEndEvent != null)
		{
			this.BrowseEndEvent(this, new EventArgs());
		}
	}

	public void ClearBrowseImageTimes()
	{
		lock (_playbackImageTimeHashtable)
		{
			_playbackImageTimeHashtable.Clear();
		}
	}

	public void SetBrowseImageTime(Guid deviceId, DateTime previousTime, DateTime currentTime, DateTime nextTime)
	{
		SetPlaybackImageTime(deviceId, previousTime, currentTime, nextTime);
	}

	public void SetPlaybackImageTime(Guid deviceId, DateTime previousTime, DateTime currentTime, DateTime nextTime)
	{
		if (!(currentTime < previousTime))
		{
			_ = currentTime > nextTime;
		}
		lock (_playbackImageTimeHashtable)
		{
			if (_playbackImageTimeHashtable.Contains(deviceId))
			{
				BrowseTimeInfo browseTimeInfo = (BrowseTimeInfo)_playbackImageTimeHashtable[deviceId];
				browseTimeInfo.PreviousTime = EnsureUtcDateTime(previousTime);
				browseTimeInfo.CurrentTime = EnsureUtcDateTime(currentTime);
				browseTimeInfo.NextTime = EnsureUtcDateTime(nextTime);
			}
			else
			{
				BrowseTimeInfo browseTimeInfo = new BrowseTimeInfo(deviceId, EnsureUtcDateTime(previousTime), EnsureUtcDateTime(currentTime), EnsureUtcDateTime(nextTime));
				_playbackImageTimeHashtable.Add(deviceId, browseTimeInfo);
			}
		}
		FireRecordedImageDisplayedEvent(currentTime);
	}

	private DateTime EnsureUtcDateTime(DateTime dt)
	{
		return dt.Kind switch
		{
			DateTimeKind.Local => dt.ToUniversalTime(), 
			DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime(), 
			DateTimeKind.Utc => dt, 
			_ => dt, 
		};
	}

	public void StopPlayback()
	{
		_updateTimer.Enabled = false;
		_loopSequence = false;
		Play = PlayState.Stop;
	}

	protected void OnLoopProgressChanged(float progress)
	{
		if (SequenceProgressChanged != null)
		{
			SequenceProgressChanged(this, new ProgressChangedEventArgs(progress));
		}
	}

	private void SignalSequenceProgress()
	{
		if (!_loopSequence)
		{
			return;
		}
		long ticks = _sequenceEndTime.Ticks;
		long ticks2 = _sequenceStartTime.Ticks;
		long ticks3 = _playbackTime.Ticks;
		if (ticks - ticks2 > 0)
		{
			long num = ticks3 - ticks2;
			long num2 = ticks - ticks2;
			float num3 = (float)num / (float)num2;
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			OnLoopProgressChanged(num3);
		}
		else
		{
			OnLoopProgressChanged(1f);
		}
	}

	private void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (!_inTimerTick)
		{
			_inTimerTick = true;
			UpdateTimerTick(sender, e);
			_inTimerTick = false;
		}
	}

	private void UpdateTimerTick(object sender, EventArgs e)
	{
		try
		{
			if (EnvironmentManager.Instance.TracePlaybackDetails && _lastLog + TimeSpan.FromSeconds(1.0) < DateTime.Now)
			{
				EnvironmentManager.Instance.Log(error: false, "PlaybackController", "UpdateTimerTick - Play=" + Play);
				_lastLog = DateTime.Now;
			}
			if (Play != PlayState.CustomPlay && IsPlaying)
			{
				DateTime utcNow = DateTime.UtcNow;
				double num = 0.0;
				num = ((!(_lastUpdate == DateTime.MinValue)) ? (Speed * (utcNow - _lastUpdate).TotalMilliseconds * 10000.0) : (Speed * _updateTimer.Interval * 10000.0));
				DateTime playbackTime = ((Play != PlayState.Forward) ? (_playbackTime - new TimeSpan((long)num)) : (_playbackTime + new TimeSpan((long)num)));
				SetPlaybackTime(playbackTime, stopPlayback: false);
				_lastUpdate = utcNow;
			}
		}
		catch (ArgumentOutOfRangeException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "PlaybackControllerBase", "Timer stopped1: " + ex.Message);
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: true, "PlaybackControllerBase", "Timer stopped2: " + ex2.Message);
		}
	}

	private DateTime GetNextGapSkippedBrowseTime(DateTime utcRequestedTime)
	{
		if (_playbackImageTimeHashtable.Count == 0)
		{
			return utcRequestedTime;
		}
		_ = utcRequestedTime.Kind;
		_ = 1;
		TimeSpan timeSpan = ((!(_playbackSpeed < 0.0)) ? new TimeSpan((int)((double)_connectionToCameraLostTimespan.Ticks * _playbackSpeed)) : _connectionToCameraLostTimespan);
		DateTime dateTime = DateTime.MaxValue;
		DateTime result;
		lock (_playbackImageTimeHashtable)
		{
			if (PlaybackMode == PlaybackModeType.Forward)
			{
				foreach (BrowseTimeInfo value in _playbackImageTimeHashtable.Values)
				{
					result = value.NextTime;
					if (result.Ticks < utcRequestedTime.Ticks)
					{
						continue;
					}
					result = value.NextTime;
					if (new TimeSpan(result.Ticks - utcRequestedTime.Ticks) <= _maxGapBeforePlaybackSkipTimespan)
					{
						result = utcRequestedTime;
						goto IL_053d;
					}
					if (utcRequestedTime.Ticks < value.PreviousTime.Ticks && new TimeSpan(value.PreviousTime.Ticks - utcRequestedTime.Ticks) > _maxGapBeforePlaybackSkipTimespan)
					{
						dateTime = value.PreviousTime;
						ClearBrowseImageTimes();
						break;
					}
				}
				foreach (BrowseTimeInfo value2 in _playbackImageTimeHashtable.Values)
				{
					if (!(value2.NextTime != value2.CurrentTime) || value2.NextTime.Ticks >= utcRequestedTime.Ticks || !(new TimeSpan(utcRequestedTime.Ticks - value2.NextTime.Ticks) < timeSpan))
					{
						continue;
					}
					result = utcRequestedTime;
					goto IL_053d;
				}
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(10.0);
				foreach (BrowseTimeInfo value3 in _playbackImageTimeHashtable.Values)
				{
					if (value3.NextTime.Ticks > utcRequestedTime.Ticks && dateTime > value3.NextTime)
					{
						dateTime = value3.NextTime;
						timeSpan2 = value3.NextTime - utcRequestedTime;
					}
				}
				if (_stopAtSequenceEnd && timeSpan2 > TimeSpan.FromSeconds(2.0))
				{
					PlaybackMode = PlaybackModeType.Stop;
					Play = PlayState.Stop;
					result = utcRequestedTime;
					goto IL_053d;
				}
				if (dateTime != DateTime.MaxValue)
				{
					result = dateTime;
					goto IL_053d;
				}
			}
			else if (PlaybackMode == PlaybackModeType.Reverse)
			{
				dateTime = DateTime.MinValue;
				foreach (BrowseTimeInfo value4 in _playbackImageTimeHashtable.Values)
				{
					DateTime dateTime2 = ((utcRequestedTime.Ticks <= value4.CurrentTime.Ticks) ? value4.PreviousTime : value4.CurrentTime);
					if (dateTime2.Ticks > utcRequestedTime.Ticks || !(new TimeSpan(utcRequestedTime.Ticks - dateTime2.Ticks) <= _maxGapBeforePlaybackSkipTimespan))
					{
						continue;
					}
					result = utcRequestedTime;
					goto IL_053d;
				}
				foreach (BrowseTimeInfo value5 in _playbackImageTimeHashtable.Values)
				{
					DateTime dateTime2 = ((utcRequestedTime.Ticks <= value5.CurrentTime.Ticks) ? value5.PreviousTime : value5.CurrentTime);
					if (!(dateTime2 != value5.CurrentTime) || dateTime2.Ticks <= utcRequestedTime.Ticks || !(new TimeSpan(dateTime2.Ticks - utcRequestedTime.Ticks) < timeSpan))
					{
						continue;
					}
					result = utcRequestedTime;
					goto IL_053d;
				}
				TimeSpan timeSpan3 = TimeSpan.FromSeconds(10.0);
				foreach (BrowseTimeInfo value6 in _playbackImageTimeHashtable.Values)
				{
					DateTime dateTime2 = ((utcRequestedTime.Ticks <= value6.CurrentTime.Ticks) ? value6.PreviousTime : value6.CurrentTime);
					if (dateTime2.Ticks < utcRequestedTime.Ticks && dateTime < dateTime2)
					{
						dateTime = dateTime2;
						timeSpan3 = utcRequestedTime - dateTime2;
					}
				}
				if (_stopAtSequenceEnd && timeSpan3 > TimeSpan.FromSeconds(2.0))
				{
					PlaybackMode = PlaybackModeType.Stop;
					Play = PlayState.Stop;
					result = utcRequestedTime;
					goto IL_053d;
				}
				if (dateTime != DateTime.MinValue)
				{
					result = dateTime;
					goto IL_053d;
				}
			}
		}
		return utcRequestedTime;
		IL_053d:
		return result;
	}
}

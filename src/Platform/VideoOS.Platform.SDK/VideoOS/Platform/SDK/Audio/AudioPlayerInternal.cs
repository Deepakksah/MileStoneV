#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Audio;

public class AudioPlayerInternal : IAudioPlayerInternal
{
	private IMicrophone _selectedMicrophone;

	private Speaker _selectedSpeaker;

	private FQID _fqid;

	private ConnectStates _deviceConnectionState = ConnectStates.NotConnected;

	private bool _mute;

	private double _volume = 1.0;

	private PlaybackControllerBase _playbackController;

	private NonUIAudioPlayer _nonUIAudioPlayer;

	private Mode _myMode;

	private Mode _audioPlayerMode = Mode.ClientSetup;

	private object _regObj1;

	private object _regObj2;

	private DateTime _lastPlaybackResync;

	private readonly TimeSpan _playbackResyncInterval = new TimeSpan(0, 0, 0, 0, 200);

	private Timer _reconnectTimer;

	public FQID MicrophoneFQID
	{
		get
		{
			if (_selectedMicrophone == null)
			{
				return null;
			}
			return _fqid;
		}
		set
		{
			if (_selectedSpeaker != null)
			{
				throw new MIPException("SpeakerFQID already set. Only one of MicrophoneFQID and SpeakerFQID may be set.");
			}
			_fqid = value;
			try
			{
				ISystemInfo systemInfo = ((SDKConfiguration)_fqid.ServerId.UserContext.Configuration).FindSystemInfo(_fqid.ServerId);
				_selectedMicrophone = systemInfo.Microphones[_fqid.ObjectId];
				if (DeviceConnectionState == ConnectStates.Connected)
				{
					Disconnect();
					Connect();
				}
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("AudioPlayerControl.MicrophoneFQID", exception);
			}
		}
	}

	public FQID SpeakerFQID
	{
		get
		{
			if (_selectedSpeaker == null)
			{
				return null;
			}
			return _fqid;
		}
		set
		{
			if (_selectedMicrophone != null)
			{
				throw new MIPException("SpeakerFQID already set. Only one of MicrophoneFQID and SpeakerFQID may be set.");
			}
			_fqid = value;
			try
			{
				ISystemInfo systemInfo = ((SDKConfiguration)_fqid.ServerId.UserContext.Configuration).FindSystemInfo(_fqid.ServerId);
				_selectedSpeaker = systemInfo.Speakers[_fqid.ObjectId];
				if (DeviceConnectionState == ConnectStates.Connected)
				{
					Disconnect();
					Connect();
				}
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("AudioPlayerControl.SpeakerFQID", exception);
			}
		}
	}

	public FQID PlaybackControllerFQID
	{
		get
		{
			return _playbackController?.FQID;
		}
		set
		{
			if (DeviceConnectionState == ConnectStates.Connected)
			{
				UnInitializePlaybackEvents();
			}
			_playbackController = ClientControl.Instance.GetPlaybackController(value) as PlaybackControllerBase;
			if (DeviceConnectionState == ConnectStates.Connected)
			{
				InitializePlaybackEvents();
			}
		}
	}

	[DefaultValue(1.0)]
	public double Volume
	{
		get
		{
			return _volume;
		}
		set
		{
			if (value < 0.0 || value > 1.0)
			{
				throw new ArgumentOutOfRangeException("value", "Volume parameter out of range (0..1)");
			}
			_volume = value;
			if (DeviceConnectionState == ConnectStates.Connected)
			{
				_nonUIAudioPlayer.DesiredVolume = value;
			}
		}
	}

	public bool Mute
	{
		get
		{
			return _mute;
		}
		set
		{
			_mute = value;
			if (DeviceConnectionState == ConnectStates.Connected)
			{
				_nonUIAudioPlayer.IsMuted = value;
			}
		}
	}

	public bool ReportAudioMeterValue
	{
		get
		{
			return _nonUIAudioPlayer?.ReportAudioMeterValue ?? false;
		}
		set
		{
			if (_nonUIAudioPlayer != null)
			{
				_nonUIAudioPlayer.ReportAudioMeterValue = value;
			}
		}
	}

	public int ConnectionState => (int)DeviceConnectionState;

	private bool HasSelectedDevice
	{
		get
		{
			if (_selectedMicrophone == null)
			{
				return _selectedSpeaker != null;
			}
			return true;
		}
	}

	private ConnectStates DeviceConnectionState
	{
		get
		{
			return _deviceConnectionState;
		}
		set
		{
			if (_deviceConnectionState != value)
			{
				_deviceConnectionState = value;
				FireConnectStateChanged(value);
			}
		}
	}

	public event ConnectResponseEventHandler ConnectResponseEvent;

	public event AudioPlayer.AudioMeterValueChangedHandler AudioMeterValueChangedEvent;

	public event EventHandler ConnectionLost;

	public event AudioPlayer.ConnectionStateChangedHandler ConnectionStateChangedEvent;

	public AudioPlayerInternal(PlaybackController defaultPlaybackController)
	{
		_playbackController = defaultPlaybackController as PlaybackControllerBase;
		_myMode = EnvironmentManager.Instance.Mode;
	}

	private void AudioPlayerActiveXOnConnectionLost(object sender, EventArgs e)
	{
		DeviceConnectionState = ConnectStates.NotConnected;
	}

	public void Initialize()
	{
		try
		{
			if (_nonUIAudioPlayer != null)
			{
				_nonUIAudioPlayer.Dispose();
			}
			_nonUIAudioPlayer = new NonUIAudioPlayer();
			UnregisterEvents();
			RegisterEvents();
			if (_reconnectTimer == null)
			{
				_reconnectTimer = new Timer();
				_reconnectTimer.Interval = 2000.0;
				_reconnectTimer.Enabled = false;
				_reconnectTimer.Elapsed += ReconnectTimer_Tick;
			}
			_regObj1 = EnvironmentManager.Instance.RegisterReceiver(ModeChangingEvent, new MessageIdFilter(MessageId.System.ModeChangedIndication));
			_nonUIAudioPlayer.DesiredVolume = Volume;
			_nonUIAudioPlayer.IsMuted = Mute;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("AudioControl.Initialize", exception);
		}
	}

	private void RegisterEvents()
	{
		if (_nonUIAudioPlayer != null)
		{
			_nonUIAudioPlayer.AudioPlayerConnectionAttemptCompletedEvent += AudioPlayerControlOnAudioPlayerConnectionAttemptCompletedEvent;
			_nonUIAudioPlayer.AudioMeterValueChangedEvent += AudioPlayerControl_AudioMeterValueChangedEvent;
			_nonUIAudioPlayer.ConnectionLost += AudioPlayerComponent_ConnectionLost;
		}
	}

	private void UnregisterEvents()
	{
		if (_nonUIAudioPlayer != null)
		{
			_nonUIAudioPlayer.AudioPlayerConnectionAttemptCompletedEvent -= AudioPlayerControlOnAudioPlayerConnectionAttemptCompletedEvent;
			_nonUIAudioPlayer.AudioMeterValueChangedEvent -= AudioPlayerControl_AudioMeterValueChangedEvent;
			_nonUIAudioPlayer.ConnectionLost -= AudioPlayerComponent_ConnectionLost;
		}
	}

	public void Connect()
	{
		try
		{
			DeviceConnectionState = ConnectStates.TryingToLogOn;
			IStreamDevice streamDevice;
			if (_selectedMicrophone == null)
			{
				IStreamDevice selectedSpeaker = _selectedSpeaker;
				streamDevice = selectedSpeaker;
			}
			else
			{
				IStreamDevice selectedSpeaker = _selectedMicrophone;
				streamDevice = selectedSpeaker;
			}
			IStreamDevice device = streamDevice;
			_nonUIAudioPlayer.Connect(device);
			if (_regObj2 == null)
			{
				_regObj2 = EnvironmentManager.Instance.RegisterReceiver(ServerCommandController_LoginTokenChangedEvent, new MessageIdFilter(MessageId.System.LoginTokenChangedIndication));
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("AudioControl:Connect", exception);
		}
	}

	public void Close()
	{
		if (_regObj1 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
			_regObj1 = null;
		}
		if (_reconnectTimer != null)
		{
			_reconnectTimer.Elapsed -= ReconnectTimer_Tick;
			_reconnectTimer.Stop();
			_reconnectTimer.Dispose();
			_reconnectTimer = null;
		}
		Disconnect();
		UnregisterEvents();
		if (_nonUIAudioPlayer != null)
		{
			_nonUIAudioPlayer.Dispose();
		}
	}

	public void Disconnect()
	{
		if (DeviceConnectionState != ConnectStates.NotConnected)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regObj2);
			_regObj2 = null;
			UnInitializePlaybackEvents();
			DeviceConnectionState = ConnectStates.NotConnected;
			_audioPlayerMode = Mode.ClientSetup;
			_nonUIAudioPlayer.Disconnect();
		}
	}

	public void StartBrowse()
	{
		if (HasSelectedDevice && DeviceConnectionState == ConnectStates.Connected && _audioPlayerMode != Mode.ClientPlayback)
		{
			if (_audioPlayerMode == Mode.ClientLive)
			{
				_nonUIAudioPlayer.StopLive();
				_audioPlayerMode = Mode.ClientSetup;
			}
			UpdatePlaybackState();
		}
	}

	public void StartLive()
	{
		if (HasSelectedDevice && DeviceConnectionState == ConnectStates.Connected && _audioPlayerMode != Mode.ClientLive)
		{
			if (_audioPlayerMode == Mode.ClientPlayback)
			{
				_nonUIAudioPlayer.StopPlayback();
			}
			_audioPlayerMode = Mode.ClientLive;
			_nonUIAudioPlayer.StartLive();
		}
	}

	public void StartSetup()
	{
		if (HasSelectedDevice && DeviceConnectionState == ConnectStates.Connected)
		{
			if (_audioPlayerMode == Mode.ClientLive)
			{
				_audioPlayerMode = Mode.ClientSetup;
				_nonUIAudioPlayer.StopLive();
			}
			if (_audioPlayerMode == Mode.ClientPlayback)
			{
				_audioPlayerMode = Mode.ClientSetup;
				_nonUIAudioPlayer.StopPlayback();
			}
		}
	}

	private void AudioPlayerControlOnAudioPlayerConnectionAttemptCompletedEvent(object sender, NonUIAudioPlayer.ConnectionAttemptCompletedEventArgs e)
	{
		if (e.ConnectionSuccessful)
		{
			InitializePlaybackEvents();
			_reconnectTimer.Enabled = false;
			DeviceConnectionState = ConnectStates.Connected;
			HandleApplicationMode(_myMode);
			_nonUIAudioPlayer.IsMuted = _mute;
			_nonUIAudioPlayer.DesiredVolume = _volume;
		}
		else
		{
			DeviceConnectionState = ConnectStates.FailedToConnect;
			_reconnectTimer.Interval = 2000.0;
			_reconnectTimer.Enabled = true;
		}
		if (this.ConnectResponseEvent != null)
		{
			this.ConnectResponseEvent(this, new ConnectResponseEventEventArgs(e.ConnectionSuccessful ? ((short)1) : ((short)0)));
		}
	}

	private void AudioPlayerControl_AudioMeterValueChangedEvent(object sender, AudioPlayer.AudioMeterValueEventArgs args)
	{
		this.AudioMeterValueChangedEvent?.Invoke(sender, new AudioPlayer.AudioMeterValueEventArgs(args.AudioMeterValue));
	}

	private void AudioPlayerComponent_ConnectionLost(object sender, EventArgs args)
	{
		this.ConnectionLost?.Invoke(this, args);
	}

	private void InitializePlaybackEvents()
	{
		if (_playbackController != null)
		{
			_playbackController.PlaybackTimeChangedEvent += PlaybackController_browseTimeChangedEvent;
			_playbackController.PlaybackSpeedChangedEvent += PlaybackController_PlaybackSpeedChangedEvent;
			_playbackController.PlaybackModeChangedEvent += PlaybackController_PlaybackModeChangedEvent;
			_playbackController.BrowseGotoEvent += PlaybackController_BrowseGotoChangedEvent;
		}
	}

	private void UnInitializePlaybackEvents()
	{
		if (_playbackController != null)
		{
			_playbackController.PlaybackTimeChangedEvent -= PlaybackController_browseTimeChangedEvent;
			_playbackController.PlaybackSpeedChangedEvent -= PlaybackController_PlaybackSpeedChangedEvent;
			_playbackController.PlaybackModeChangedEvent -= PlaybackController_PlaybackModeChangedEvent;
			_playbackController.BrowseGotoEvent -= PlaybackController_BrowseGotoChangedEvent;
		}
	}

	private object ModeChangingEvent(Message message, FQID dest, FQID sender)
	{
		if (DestForMe(sender) && _myMode != (Mode)message.Data)
		{
			_myMode = (Mode)message.Data;
			HandleApplicationMode(_myMode);
		}
		return null;
	}

	private bool DestForMe(FQID fqid)
	{
		if (fqid == null && (_playbackController == null || _playbackController.FQID == null))
		{
			return true;
		}
		if (fqid == null || _playbackController == null || _playbackController.FQID == null)
		{
			return false;
		}
		if (fqid.ObjectId == _playbackController.FQID.ObjectId)
		{
			return true;
		}
		return false;
	}

	private void HandleApplicationMode(Mode applicationMode)
	{
		if (HasSelectedDevice)
		{
			switch (applicationMode)
			{
			case Mode.ClientLive:
				StartLive();
				break;
			case Mode.ClientPlayback:
				StartBrowse();
				break;
			case Mode.ClientSetup:
				StartSetup();
				break;
			}
		}
	}

	private object ServerCommandController_LoginTokenChangedEvent(Message message, FQID dest, FQID sender)
	{
		try
		{
			if (HasSelectedDevice)
			{
				if (DeviceConnectionState == ConnectStates.Connected)
				{
					IStreamDevice streamDevice;
					if (_selectedMicrophone == null)
					{
						IStreamDevice selectedSpeaker = _selectedSpeaker;
						streamDevice = selectedSpeaker;
					}
					else
					{
						IStreamDevice selectedSpeaker = _selectedMicrophone;
						streamDevice = selectedSpeaker;
					}
					IStreamDevice device = streamDevice;
					_nonUIAudioPlayer.UpdateConfiguration(device);
				}
				else
				{
					Connect();
					_nonUIAudioPlayer.IsMuted = Mute;
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SDKAudioControl", ex.Message);
		}
		return null;
	}

	private void PlaybackController_browseTimeChangedEvent(object sender, PlaybackController.TimeEventArgs e)
	{
		if (HasSelectedDevice && _audioPlayerMode == Mode.ClientPlayback)
		{
			ResyncPlayback(e.Time);
		}
	}

	private void PlaybackController_PlaybackSpeedChangedEvent(object sender, PlaybackController.PlaybackSpeedEventArgs e)
	{
		if (HasSelectedDevice)
		{
			UpdatePlaybackState();
		}
	}

	private void PlaybackController_PlaybackModeChangedEvent(object sender, PlaybackController.PlaybackModeEventArgs e)
	{
		if (HasSelectedDevice)
		{
			UpdatePlaybackState();
		}
	}

	private void PlaybackController_BrowseGotoChangedEvent(object sender, PlaybackController.TimeEventArgs e)
	{
	}

	private void UpdatePlaybackState()
	{
		Trace.WriteLine("UpdatePlaybackState");
		bool flag = false;
		if (HasSelectedDevice && _myMode == Mode.ClientPlayback && _playbackController.PlaybackMode == PlaybackController.PlaybackModeType.Forward && _playbackController.PlaybackSpeed == 1.0)
		{
			flag = true;
		}
		if (flag)
		{
			if (StartPlayback(_playbackController.BrowseTime))
			{
				_audioPlayerMode = Mode.ClientPlayback;
			}
		}
		else
		{
			StopPlayback();
			_audioPlayerMode = Mode.ClientSetup;
		}
	}

	private bool StartPlayback(DateTime startTime)
	{
		if (DeviceConnectionState != ConnectStates.Connected)
		{
			return false;
		}
		_nonUIAudioPlayer.StartPlayback(startTime);
		return true;
	}

	private void ResyncPlayback(DateTime syncTime)
	{
		if (DeviceConnectionState == ConnectStates.Connected && DateTime.Now.Ticks - _lastPlaybackResync.Ticks > _playbackResyncInterval.Ticks)
		{
			_lastPlaybackResync = DateTime.Now;
			_nonUIAudioPlayer.ReSyncPlayback(syncTime);
		}
	}

	private void StopPlayback()
	{
		if (DeviceConnectionState != ConnectStates.NotConnected)
		{
			Trace.WriteLine("Stopping audio playback");
			_nonUIAudioPlayer.StopPlayback();
		}
	}

	private void FireConnectStateChanged(ConnectStates state)
	{
		if (this.ConnectionStateChangedEvent != null)
		{
			this.ConnectionStateChangedEvent(this, new ConnectionState(state == ConnectStates.Connected, (int)state));
		}
	}

	private void ReconnectTimer_Tick(object obj, ElapsedEventArgs e)
	{
		_reconnectTimer.Interval = 10000.0;
		if (DeviceConnectionState != ConnectStates.Connected)
		{
			Connect();
		}
	}
}

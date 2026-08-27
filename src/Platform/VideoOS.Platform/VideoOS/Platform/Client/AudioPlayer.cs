using System;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class AudioPlayer
{
	public delegate void ConnectionStateChangedHandler(object sender, ConnectionState connectionState);

	public delegate void AudioMeterValueChangedHandler(object sender, AudioMeterValueEventArgs audioMeterValue);

	public class AudioMeterValueEventArgs : EventArgs
	{
		public int AudioMeterValue { get; set; }

		public AudioMeterValueEventArgs(int audioMeterValue)
		{
			AudioMeterValue = audioMeterValue;
		}
	}

	private AudioMeterValueChangedHandler _audioMeterValueChangedEvent;

	private IAudioPlayerInternal _player;

	public FQID MicrophoneFQID
	{
		get
		{
			return _player.MicrophoneFQID;
		}
		set
		{
			_player.MicrophoneFQID = value;
		}
	}

	public FQID SpeakerFQID
	{
		get
		{
			return _player.SpeakerFQID;
		}
		set
		{
			_player.SpeakerFQID = value;
		}
	}

	public FQID PlaybackControllerFQID
	{
		get
		{
			return _player.PlaybackControllerFQID;
		}
		set
		{
			_player.PlaybackControllerFQID = value;
		}
	}

	public double Volume
	{
		get
		{
			return _player.Volume;
		}
		set
		{
			_player.Volume = value;
		}
	}

	public bool Mute
	{
		get
		{
			return _player.Mute;
		}
		set
		{
			_player.Mute = value;
		}
	}

	public int ConnectionState => _player.ConnectionState;

	public event ConnectResponseEventHandler ConnectResponseEvent;

	public event ConnectionStateChangedHandler ConnectionStateChangedEvent;

	public event EventHandler ConnectionLost;

	public event AudioMeterValueChangedHandler AudioMeterValueChangedEvent
	{
		add
		{
			_audioMeterValueChangedEvent = (AudioMeterValueChangedHandler)Delegate.Combine(_audioMeterValueChangedEvent, value);
			_player.ReportAudioMeterValue = true;
		}
		remove
		{
			_audioMeterValueChangedEvent = (AudioMeterValueChangedHandler)Delegate.Remove(_audioMeterValueChangedEvent, value);
			if (_audioMeterValueChangedEvent == null)
			{
				_player.ReportAudioMeterValue = false;
			}
		}
	}

	public AudioPlayer()
		: this(null)
	{
	}

	public AudioPlayer(FQID playbackControllerFQID)
	{
		_player = ClientControl.Instance.CreateAudioPlayerInternal(playbackControllerFQID);
		if (_player == null)
		{
			throw new MIPException("Not supported for current environment");
		}
		_player.ConnectionStateChangedEvent += _player_ConnectionStateChangedEvent;
		_player.ConnectionLost += _player_ConnectionLostEvent;
		_player.ConnectResponseEvent += _player_ConnectResponseEvent;
		_player.AudioMeterValueChangedEvent += _player_AudioMeterValueChangedEvent;
	}

	private void _player_ConnectResponseEvent(object sender, ConnectResponseEventEventArgs e)
	{
		this.ConnectResponseEvent?.Invoke(this, e);
	}

	private void _player_ConnectionStateChangedEvent(object sender, ConnectionState connectionState)
	{
		this.ConnectionStateChangedEvent?.Invoke(this, connectionState);
	}

	private void _player_ConnectionLostEvent(object sender, EventArgs args)
	{
		this.ConnectionLost?.Invoke(this, args);
	}

	private void _player_AudioMeterValueChangedEvent(object sender, AudioMeterValueEventArgs audioMeterValue)
	{
		_audioMeterValueChangedEvent?.Invoke(this, audioMeterValue);
	}

	public void Initialize()
	{
		_player.Initialize();
		new TrackPageViewData(typeof(AudioPlayer)).Send();
	}

	public void Connect()
	{
		_player.Connect();
	}

	public void Disconnect()
	{
		_player.Disconnect();
	}

	public void Close()
	{
		_player.Close();
	}
}

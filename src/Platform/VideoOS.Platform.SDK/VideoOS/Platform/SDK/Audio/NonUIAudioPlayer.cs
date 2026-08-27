using System;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Platform.Client;
using VideoOS.Platform.Common;
using VideoOS.Platform.Common.Audio;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Audio;

public class NonUIAudioPlayer
{
	public delegate void ConnectionAttemptCompletedEventHandler(object sender, ConnectionAttemptCompletedEventArgs e);

	public class ConnectionAttemptCompletedEventArgs : EventArgs
	{
		protected Guid _deviceId;

		public bool _connectionSuccessful;

		public virtual Guid DeviceId
		{
			get
			{
				return _deviceId;
			}
			set
			{
				_deviceId = value;
			}
		}

		public virtual bool ConnectionSuccessful
		{
			get
			{
				return _connectionSuccessful;
			}
			set
			{
				_connectionSuccessful = value;
			}
		}

		public ConnectionAttemptCompletedEventArgs(Guid deviceId, bool connectionSuccessful)
		{
			_deviceId = deviceId;
			_connectionSuccessful = connectionSuccessful;
		}
	}

	private IStreamDevice _device;

	private IAudioPlayerComponent _audioPlayerComponent;

	private DateTime _lastPlaybackReSync = DateTime.MinValue;

	private readonly TimeSpan _playbackReSyncInterval = TimeSpan.FromMilliseconds(200.0);

	private bool? _audioPlaybackSupported;

	private double _volumeFactor = 1.0;

	private double _desiredVolume = 1.0;

	private bool _isMuted;

	private bool _isPlaying;

	public Guid Id { get; set; } = Guid.Empty;

	public bool AudioPlaybackSupported
	{
		get
		{
			if (!_audioPlaybackSupported.HasValue)
			{
				_audioPlaybackSupported = _audioPlayerComponent.SupportsPlayback;
			}
			return _audioPlaybackSupported.Value;
		}
	}

	public bool IsConnected
	{
		get
		{
			if (_audioPlayerComponent != null)
			{
				return _audioPlayerComponent.Connected;
			}
			return false;
		}
	}

	public bool IsMicrophone
	{
		get
		{
			if (_device != null && _device is IMicrophone)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsSpeaker
	{
		get
		{
			if (_device != null && _device is ISpeaker)
			{
				return true;
			}
			return false;
		}
	}

	public double VolumeFactor
	{
		get
		{
			return _volumeFactor;
		}
		set
		{
			if (Math.Abs(_volumeFactor - value) > 1E-10)
			{
				_volumeFactor = value;
				UpdateVolume();
			}
		}
	}

	public double DesiredVolume
	{
		get
		{
			return _desiredVolume;
		}
		set
		{
			if (Math.Abs(_desiredVolume - value) > 1E-10)
			{
				_desiredVolume = value;
				UpdateVolume();
			}
		}
	}

	public bool IsMuted
	{
		get
		{
			return _isMuted;
		}
		set
		{
			_isMuted = value;
			_audioPlayerComponent.Mute = _isMuted;
		}
	}

	public bool ReportAudioMeterValue
	{
		get
		{
			return _audioPlayerComponent.ReportAudioMeterValue;
		}
		set
		{
			_audioPlayerComponent.ReportAudioMeterValue = value;
		}
	}

	public bool IsPlaying
	{
		get
		{
			return _isPlaying;
		}
		set
		{
			if (_isPlaying != value)
			{
				_isPlaying = value;
			}
		}
	}

	public event ConnectionAttemptCompletedEventHandler AudioPlayerConnectionAttemptCompletedEvent = delegate
	{
	};

	public event EventHandler ConnectionLost;

	public event EventHandler ConnectionAttemptCompleted = delegate
	{
	};

	public event AudioPlayer.AudioMeterValueChangedHandler AudioMeterValueChangedEvent;

	public NonUIAudioPlayer()
	{
		_audioPlayerComponent = new NAudioPlayerComponent(200, 200);
		_audioPlayerComponent.OnConnectionAttemptCompleted += AudioPlayerComponent_ConnectionAttemptCompleted;
		_audioPlayerComponent.OnConnectionLost += AudioPlayerComponent_ConnectionLost;
		_audioPlayerComponent.OnAudioMeterValueChanged += AudioPlayerComponent_AudioMeterValueChanged;
	}

	public void Dispose()
	{
		if (_audioPlayerComponent != null)
		{
			_audioPlayerComponent.OnConnectionAttemptCompleted -= AudioPlayerComponent_ConnectionAttemptCompleted;
			_audioPlayerComponent.OnConnectionLost -= AudioPlayerComponent_ConnectionLost;
			_audioPlayerComponent.OnAudioMeterValueChanged -= AudioPlayerComponent_AudioMeterValueChanged;
			_audioPlayerComponent.Dispose();
			_audioPlayerComponent = null;
		}
	}

	private void FireAudioPlayerConnectionAttemptCompletedEvent(Guid deviceId, bool connectionSuccessful)
	{
		this.AudioPlayerConnectionAttemptCompletedEvent(this, new ConnectionAttemptCompletedEventArgs(deviceId, connectionSuccessful));
		this.ConnectionAttemptCompleted(this, EventArgs.Empty);
	}

	private void AudioPlayerComponent_ConnectionAttemptCompleted(object sender, EventArgs<bool> connected)
	{
		FireAudioPlayerConnectionAttemptCompletedEvent(Id, connected.Value);
	}

	private void AudioPlayerComponent_ConnectionLost(object sender, EventArgs e)
	{
		IsPlaying = false;
		this.ConnectionLost?.Invoke(this, e);
	}

	private void AudioPlayerComponent_AudioMeterValueChanged(object sender, EventArgs<int> args)
	{
		this.AudioMeterValueChangedEvent?.Invoke(sender, new AudioPlayer.AudioMeterValueEventArgs(args.Value));
	}

	public void Connect(IStreamDevice device)
	{
		if (device != null && device.Recorder != null)
		{
			_device = device;
			Id = device.Guid;
			XmlDocument xmlDocument = AddPlayerToolkitToSourceToolkitXml(device.GetToolkitConfigurationXml(Guid.Empty).ChildNodes.Item(1) as XmlElement);
			_audioPlayerComponent.ConnectUsingConfiguration(xmlDocument.OuterXml);
		}
	}

	public void UpdateConfiguration(IStreamDevice device)
	{
		if (device != null && device.Recorder != null)
		{
			_device = device;
			Id = device.Guid;
			XmlDocument xmlDocument = AddPlayerToolkitToSourceToolkitXml(device.GetToolkitConfigurationXml(Guid.Empty).ChildNodes.Item(1) as XmlElement);
			_audioPlayerComponent.SetConfiguration(xmlDocument.OuterXml);
		}
	}

	private static XmlDocument AddPlayerToolkitToSourceToolkitXml(XmlElement xml)
	{
		using MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "audio");
		xmlTextWriter.WriteElementString("provider", "aud");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteElementString("prebuffer_time", "200");
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "mmp");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteStartElement("audio_decoder");
		xmlTextWriter.WriteAttributeString("sampling_rate", "44100");
		xmlTextWriter.WriteRaw(xml.OuterXml);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}

	public void Disconnect()
	{
		_audioPlayerComponent.Disconnect();
		_device = null;
		Id = Guid.Empty;
	}

	public void StartLive()
	{
		_audioPlayerComponent.LiveStart();
		IsPlaying = true;
	}

	public void StopLive()
	{
		_audioPlayerComponent.LiveStop();
		IsPlaying = false;
	}

	public void StartPlayback(DateTime startTime)
	{
		_audioPlayerComponent.PlaybackStart(startTime);
		IsPlaying = true;
	}

	public void ReSyncPlayback(DateTime syncTime)
	{
		if (DateTime.UtcNow.Ticks - _lastPlaybackReSync.Ticks > _playbackReSyncInterval.Ticks)
		{
			_lastPlaybackReSync = DateTime.UtcNow;
			_audioPlayerComponent.PlaybackReSync(syncTime);
		}
	}

	public void StopPlayback()
	{
		_audioPlayerComponent.PlaybackStop();
		IsPlaying = false;
	}

	private void UpdateVolume()
	{
		double volume = VolumeFactor * DesiredVolume;
		_audioPlayerComponent.SetVolume(volume);
	}
}

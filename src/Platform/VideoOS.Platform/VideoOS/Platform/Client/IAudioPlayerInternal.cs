using System;

namespace VideoOS.Platform.Client;

public interface IAudioPlayerInternal
{
	FQID MicrophoneFQID { get; set; }

	FQID SpeakerFQID { get; set; }

	FQID PlaybackControllerFQID { get; set; }

	double Volume { get; set; }

	bool Mute { get; set; }

	bool ReportAudioMeterValue { get; set; }

	int ConnectionState { get; }

	event ConnectResponseEventHandler ConnectResponseEvent;

	event AudioPlayer.ConnectionStateChangedHandler ConnectionStateChangedEvent;

	event EventHandler ConnectionLost;

	event AudioPlayer.AudioMeterValueChangedHandler AudioMeterValueChangedEvent;

	void Initialize();

	void Connect();

	void Disconnect();

	void Close();
}

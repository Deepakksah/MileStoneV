using System;

namespace VideoOS.Platform.Common.Audio;

public interface IAudioPlayerComponent : IDisposable
{
	bool SupportsPlayback { get; }

	bool Connected { get; }

	bool Mute { get; set; }

	bool ReportAudioMeterValue { get; set; }

	event EventHandler<EventArgs<bool>> OnConnectionAttemptCompleted;

	event EventHandler OnConnectionLost;

	event EventHandler<EventArgs<int>> OnAudioMeterValueChanged;

	void ConnectUsingConfiguration(string configuration);

	void Disconnect();

	void SetConfiguration(string configuration);

	void LiveStart();

	void LiveStop();

	void PlaybackStart(DateTime startTime);

	void PlaybackReSync(DateTime syncTime);

	void PlaybackStop();

	void SetVolume(double volume);
}

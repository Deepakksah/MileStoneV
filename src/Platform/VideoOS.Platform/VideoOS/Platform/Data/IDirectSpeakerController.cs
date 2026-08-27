using System;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Data;

public interface IDirectSpeakerController
{
	bool DoRetryConnect { get; set; }

	LoginSettings LoginSettings { set; }

	bool Connected { get; }

	bool Busy { get; }

	bool Mute { get; set; }

	bool OutgoingAudioTransmitEnableState { get; set; }

	string OutgoingAudioErrorText { get; set; }

	event EventHandler MuteStatusChangedEvent;

	event EventHandler OutgoingAudioTransmitEnableStateChangedEvent;

	event EventHandler OutgoingAudioErrorTextEvent;

	void Close(int timeoutMSec = 100);

	void Dispose();

	bool Connect(FQID speakerFQID, int timeoutMSec = 10000);

	void TransmitStart();

	void TransmitStop();

	void SendData(byte[] audioData);

	void InitOutgoingWaveFormat(int channelsCount, int bitsPerSample, int samplesPerSecond);

	void SendWrappedData(byte[] audioData);
}

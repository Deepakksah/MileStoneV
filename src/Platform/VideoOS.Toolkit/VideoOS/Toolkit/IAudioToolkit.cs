using System;
using System.Collections.Generic;

namespace VideoOS.Toolkit;

public interface IAudioToolkit : IToolkit
{
	public enum State
	{
		Disconnected,
		Connecting,
		Connected,
		ConnectionFailed,
		ConnectionLost,
		Reconnecting
	}

	public enum Flags
	{
		LiveNotSupported = 1,
		PlaybackNotSupported = 2,
		NotAuthorized = 4,
		LicenseInvalid = 8,
		MaxNumberOfConnectionsReached = 0x10
	}

	public interface IAudioInformation : IDisposable
	{
		IntPtr RawData();

		string Codec();

		long NumberOfChannels();

		long SamplingRate();

		long BitDepth();

		long DataSize();

		DateTime TimeStamp();
	}

	public interface IAudioHandler
	{
		void HandleAudioData(IAudioInformation audioInformation);
	}

	public interface IStateHandler
	{
		void HandleStateChanged(State state, Flags flags);
	}

	public interface IOutOfBandHandler
	{
		void HandleOutOfBandData(IOutOfBandData outOfBandData);
	}

	void UpdateConfiguration(IList<KeyValuePair<string, string>> configurationParameterList);

	void UpdateConfiguration(string configurationParameterName, string configurationParameterValue);

	void Connect();

	void Disconnect();

	void DoPlayback(DateTime timeStamp, double speed);

	void DoLive();

	void StopLive();

	void SetAudioHandler(IAudioHandler audioHandler);

	void SetOutOfBandHandler(IOutOfBandHandler outOfBandHandler);

	void SetStateHandler(IStateHandler stateHandler);
}

using System;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class AudioSampleSettings : IAudioSampleSettings, ICloneable
{
	public int SampleRate { get; set; }

	public int BitsPerSample { get; set; }

	public int ChannelsCount { get; set; }

	public int AvrBytesPerSec { get; set; }

	public AudioSampleSettings()
	{
	}

	public AudioSampleSettings(PcmSourceSettings other)
	{
		InitializeFrom(other);
	}

	public virtual object Clone()
	{
		AudioSampleSettings audioSampleSettings = new AudioSampleSettings();
		audioSampleSettings.InitializeFrom(this);
		return audioSampleSettings;
	}

	public void InitializeFrom(IAudioSampleSettings other)
	{
		SampleRate = other.SampleRate;
		BitsPerSample = other.BitsPerSample;
		ChannelsCount = other.ChannelsCount;
		AvrBytesPerSec = other.AvrBytesPerSec;
	}

	public void InitializeFrom(PcmSourceSettings other)
	{
		SampleRate = (int)other.SamplingRate;
		BitsPerSample = (int)other.BitsPerSample;
		ChannelsCount = (int)other.Channels;
		AvrBytesPerSec = SampleRate * (BitsPerSample / 8) * ChannelsCount;
	}
}

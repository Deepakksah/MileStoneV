using System;

namespace VideoOS.Platform.Util.Pcm;

public static class PcmHelper
{
	public static int MillisecondsToBytesCount(double milliseconds, int sampleRate, int numberOfChannels, int bytesPerSample)
	{
		return (int)(Math.Round(milliseconds / 1000.0 * (double)sampleRate) * (double)numberOfChannels * (double)bytesPerSample);
	}

	public static double BytesCountToMilliseconds(int bytesCount, int sampleRate, int numberOfChannels, int bytesPerSample)
	{
		return (double)bytesCount / (double)numberOfChannels / (double)bytesPerSample * 1000.0 / (double)sampleRate;
	}

	public static double BytesCountToMilliseconds(int bytesCount, IPcmSourceSettings pcmSourceSettings)
	{
		return BytesCountToMilliseconds(bytesCount, (int)pcmSourceSettings.SamplingRate, (int)pcmSourceSettings.Channels, (int)pcmSourceSettings.BitsPerSample / 8);
	}
}

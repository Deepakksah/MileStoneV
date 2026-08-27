namespace VideoOS.Platform.Util;

public interface IPcmSourceSettings
{
	PcmSourceSettings.ChannelsType Channels { get; }

	PcmSourceSettings.BitsPerSampleType BitsPerSample { get; }

	uint SamplingRate { get; }
}

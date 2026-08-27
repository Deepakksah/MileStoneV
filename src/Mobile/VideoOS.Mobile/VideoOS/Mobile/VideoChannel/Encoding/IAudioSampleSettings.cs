namespace VideoOS.Mobile.VideoChannel.Encoding;

public interface IAudioSampleSettings
{
	int SampleRate { get; }

	int BitsPerSample { get; }

	int ChannelsCount { get; }

	int AvrBytesPerSec { get; }
}

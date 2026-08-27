using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public interface IAudioEncoderSettings : IAudioSampleSettings
{
	AudioEncoderMf.AudioEncoderTypes EncoderType { get; }
}

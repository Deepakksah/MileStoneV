using VideoOS.Platform.Util;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public interface IAudioEncoderSettingsExt : IAudioEncoderSettings, IAudioSampleSettings
{
	PcmSourceSettings PcmSourceSettings { get; }
}

using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class AudioEncoderSettings : AudioSampleSettings, IAudioEncoderSettings, IAudioSampleSettings
{
	public AudioEncoderMf.AudioEncoderTypes EncoderType { get; set; }

	public override object Clone()
	{
		AudioEncoderSettings audioEncoderSettings = new AudioEncoderSettings();
		audioEncoderSettings.InitializeFrom(this);
		return audioEncoderSettings;
	}

	public void InitializeFrom(IAudioEncoderSettings other)
	{
		EncoderType = other.EncoderType;
		InitializeFrom((IAudioSampleSettings)other);
	}
}

using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class AudioEncoder : AudioTransformWrapper
{
	private readonly AudioEncoderSettings _audioEncoderSettings = new AudioEncoderSettings();

	public IAudioEncoderSettings EncoderSettings => _audioEncoderSettings;

	public AudioEncoder(IAudioEncoderSettings encoderSettings)
		: base(encoderSettings)
	{
		_audioEncoderSettings.InitializeFrom(encoderSettings);
	}

	protected override Transform CreateTransform()
	{
		return AudioEncoderMf.GetEncoder(AudioEncoderMf.AudioEncoderTypes.Mp3, EncoderSettings.SampleRate, EncoderSettings.ChannelsCount, EncoderSettings.BitsPerSample, EncoderSettings.AvrBytesPerSec);
	}
}

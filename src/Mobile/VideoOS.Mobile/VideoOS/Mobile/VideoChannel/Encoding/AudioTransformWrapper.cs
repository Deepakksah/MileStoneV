using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public abstract class AudioTransformWrapper : TransformWrapper
{
	private readonly AudioSampleSettings _audioSampleSettings = new AudioSampleSettings();

	private long _currentTime;

	public IAudioSampleSettings SampleSettings => _audioSampleSettings;

	protected AudioTransformWrapper(IAudioSampleSettings sampleSettings)
	{
		_audioSampleSettings.InitializeFrom(sampleSettings);
	}

	protected override void SetInputSampleProperties(Sample inputSample)
	{
		inputSample.SetAudioSampleProps(SampleSettings, ref _currentTime);
	}
}

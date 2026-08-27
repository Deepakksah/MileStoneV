using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class AudioResampler : AudioTransformWrapper
{
	private int _outputSampleRate;

	public int OutputSampleRate
	{
		get
		{
			return _outputSampleRate;
		}
		set
		{
			if (_outputSampleRate != value)
			{
				base.NeedsTransformRecreation = true;
			}
			_outputSampleRate = value;
		}
	}

	public AudioResampler(IAudioSampleSettings sampleSettings)
		: base(sampleSettings)
	{
		OutputSampleRate = sampleSettings.SampleRate;
	}

	protected override Transform CreateTransform()
	{
		return AudioResamplerMf.GetResampler(base.SampleSettings.SampleRate, OutputSampleRate, base.SampleSettings.ChannelsCount, base.SampleSettings.BitsPerSample, 70);
	}
}

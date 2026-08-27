using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Interfaces;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class AudioResamplerMf
{
	public static readonly Guid AudioResamplerTransformClsid = new Guid("{f447b69e-1884-4a7e-8055-346f74d6edb3}");

	public const int ResamplerMinQuality = 1;

	public const int ResamplerMaxQuality = 100;

	private const int ResamplerMaxQualityWmf = 60;

	public static Transform Create()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new Transform(AudioResamplerTransformClsid);
	}

	public static Transform Configure(this Transform transform, int inputSampleRate, int outputSampleRate, int numChannels, int bitsPerSample)
	{
		MediaType val = MediaTypeHelper.CreateAudioMediaTypeExt(inputSampleRate, numChannels, bitsPerSample);
		transform.SetInputType(0, val, 0);
		MediaType val2 = MediaTypeHelper.CreateAudioMediaTypeExt(outputSampleRate, numChannels, bitsPerSample);
		transform.SetOutputType(0, val2, 0);
		return transform;
	}

	public static Transform Configure(this Transform transform, int qualityPercent)
	{
		int wmfQualityFromPercent = GetWmfQualityFromPercent(qualityPercent);
		((IWMResamplerProps)Marshal.GetObjectForIUnknown(((CppObject)transform).NativePointer)).SetHalfFilterLength(wmfQualityFromPercent);
		return transform;
	}

	public static int GetWmfQualityFromPercent(int qualityPercent)
	{
		qualityPercent = Math.Min(qualityPercent, 100);
		qualityPercent = Math.Max(qualityPercent, 1);
		qualityPercent = (int)((float)qualityPercent / 100f * 60f);
		qualityPercent = Math.Max(qualityPercent, 1);
		return qualityPercent;
	}

	public static Transform GetResampler(int inputSampleRate, int outputSampleRate, int numChannels, int bitsPerSample, int qualityPercent)
	{
		return Create().Configure(inputSampleRate, outputSampleRate, numChannels, bitsPerSample).Configure(qualityPercent);
	}
}

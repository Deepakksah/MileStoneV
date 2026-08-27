using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class AudioEncoderMf
{
	public enum AudioEncoderTypes
	{
		Aac,
		Alac,
		Amr,
		Dolby,
		Flac,
		Mp3,
		Mpeg2,
		Wma
	}

	public static readonly Guid Mp3AudioEncoderTransformClsid = new Guid("{11103421-354C-4CCA-A7A3-1AFF9A5B6701}");

	public static readonly Guid FlacAudioEncoderTransformClsid = new Guid("{128509e9-c44e-45dc-95e9-c255b8f466a6}");

	public static readonly Guid Mpeg2AudioEncoderTransformClsid = new Guid("{46a4dd5c-73f8-4304-94df-308f760974f4}");

	public static readonly Guid WmAudioEncoderTransformClsid = new Guid("{70f598e9-f4ab-495a-99e2-a7c4d3d89abf}");

	public static readonly Guid AacAudioEncoderTransformClsid = new Guid("{93af0c51-2275-45d2-a35b-f2ba21caed00}");

	public static readonly Guid AmrAudioEncoderTransformClsid = new Guid("{2fae8afe-04a3-423a-a814-85db454712b0}");

	public static readonly Guid AlacAudioEncoderTransformClsid = new Guid("{9ab6a28c-748e-4b6a-bfff-cc443b8e8fb4}");

	public static readonly Guid DolbyAudioEncoderTransformClsid = new Guid("{ac3315c9-f481-45d7-826c-0b406c1f64b8}");

	public static readonly Guid[] AudioEncoderTypesClsids = new Guid[8] { AacAudioEncoderTransformClsid, AlacAudioEncoderTransformClsid, AmrAudioEncoderTransformClsid, DolbyAudioEncoderTransformClsid, FlacAudioEncoderTransformClsid, Mp3AudioEncoderTransformClsid, Mpeg2AudioEncoderTransformClsid, WmAudioEncoderTransformClsid };

	public static Transform Create(AudioEncoderTypes audioEncoder)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		return new Transform(AudioEncoderTypesClsids[(int)audioEncoder]);
	}

	public static Transform Configure(Transform transform, int sampleRate, int numChannels, int bitsPerSample, int targetAvrBytesPerSec, Action<MediaType> onInputMediaType = null, Action<MediaType> onOutputMediaType = null)
	{
		IEnumerable<MediaType> allMediaTypes = transform.GetOutputAvailableTypes(0);
		MediaType nearestMediaType = MediaTypeHelper.GetNearestMediaType(MediaTypeHelper.GetMatchingMediaTypes(allMediaTypes, MediaTypeHelper.GetMatchingProps(sampleRate, numChannels, null, null)), ((MediaAttributeKey)MediaTypeAttributeKeys.AudioAvgBytesPerSecond).Guid, targetAvrBytesPerSec);
		transform.SetOutputType(0, nearestMediaType, 0);
		IEnumerable<MediaType> allMediaTypes2 = transform.GetInputAvailableTypes(0);
		MediaType val = MediaTypeHelper.GetMatchingMediaTypes(allMediaTypes2, MediaTypeHelper.GetMatchingProps(sampleRate, numChannels, bitsPerSample, null)).First();
		transform.SetInputType(0, val, 0);
		CallbackMediaType(onInputMediaType, val, ref allMediaTypes2);
		CallbackMediaType(onOutputMediaType, nearestMediaType, ref allMediaTypes);
		MediaTypeHelper.ClearMediaTypes(allMediaTypes);
		MediaTypeHelper.ClearMediaTypes(allMediaTypes2);
		return transform;
	}

	private static void CallbackMediaType(Action<MediaType> onMediaType, MediaType mediaType, ref IEnumerable<MediaType> allMediaTypes)
	{
		if (onMediaType != null)
		{
			onMediaType(mediaType);
			allMediaTypes = from el in allMediaTypes
				select (el) into el
				where el != mediaType
				select el;
		}
	}

	private static Transform ConfigureExt(this Transform transform, int sampleRate, int numChannels, int bitsPerSample, int targetAvrBytesPerSec)
	{
		return Configure(transform, sampleRate, numChannels, bitsPerSample, targetAvrBytesPerSec);
	}

	public static Transform GetEncoder(AudioEncoderTypes audioEncoder, int sampleRate, int numChannels, int bitsPerSample, int targetAvrBytesPerSec)
	{
		return Create(audioEncoder).ConfigureExt(sampleRate, numChannels, bitsPerSample, targetAvrBytesPerSec);
	}
}

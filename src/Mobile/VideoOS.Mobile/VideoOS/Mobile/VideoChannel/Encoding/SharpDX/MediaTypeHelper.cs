using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.MediaFoundation;
using SharpDX.Multimedia;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class MediaTypeHelper
{
	public static readonly IEnumerable<MediaAttributeKey> MediaAttributes = (IEnumerable<MediaAttributeKey>)(object)new MediaAttributeKey[11]
	{
		(MediaAttributeKey)MediaTypeAttributeKeys.MajorType,
		(MediaAttributeKey)MediaTypeAttributeKeys.Subtype,
		(MediaAttributeKey)MediaTypeAttributeKeys.UserData,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioBitsPerSample,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioNumChannels,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioSamplesPerSecond,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioBlockAlignment,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioPreferWaveformatex,
		(MediaAttributeKey)MediaTypeAttributeKeys.AudioAvgBytesPerSecond,
		(MediaAttributeKey)MediaTypeAttributeKeys.Compressed,
		(MediaAttributeKey)MediaTypeAttributeKeys.AllSamplesIndependent
	};

	private static Dictionary<Guid, string> _propertyIdToName = null;

	public static Dictionary<Guid, string> PropertyIdToName
	{
		get
		{
			if (_propertyIdToName == null)
			{
				_propertyIdToName = new Dictionary<Guid, string>();
				foreach (MediaAttributeKey mediaAttribute in MediaAttributes)
				{
					_propertyIdToName.Add(mediaAttribute.Guid, mediaAttribute.Name);
				}
			}
			return _propertyIdToName;
		}
	}

	public static void PrintMediaType(MediaType mediaType, string mediaTypeName)
	{
		PrintMediaTypes((IEnumerable<MediaType>)(object)new MediaType[1] { mediaType }, mediaTypeName);
	}

	public static void PrintMediaTypes(IEnumerable<MediaType> mediaTypes, string mediaTypesName)
	{
	}

	public static IEnumerable<MediaType> GetMatchingMediaTypes(IEnumerable<MediaType> mediaTypes, IDictionary<Guid, object> matchingProps)
	{
		List<MediaType> list = new List<MediaType>();
		foreach (MediaType mediaType in mediaTypes)
		{
			bool flag = true;
			foreach (KeyValuePair<Guid, object> matchingProp in matchingProps)
			{
				if (!matchingProp.Value.Equals(((MediaAttributes)mediaType).Get(matchingProp.Key)))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(mediaType);
			}
		}
		PrintMediaTypes(list, "Matching media types");
		return list;
	}

	public static MediaType GetNearestMediaType(IEnumerable<MediaType> mediaTypes, Guid targetPropKey, int targetPropValue)
	{
		List<MediaType> list = new List<MediaType>(mediaTypes);
		list.Sort((MediaType first, MediaType second) => Math.Abs((int)((MediaAttributes)first).Get(targetPropKey) - targetPropValue) - Math.Abs((int)((MediaAttributes)second).Get(targetPropKey) - targetPropValue));
		PrintMediaType(list.First(), "Nearest media type");
		return list.First();
	}

	public static IDictionary<Guid, object> GetMatchingProps(int? sampleRate, int? numberOfChannels, int? bitsPerSample, int? blockAlignment)
	{
		Dictionary<Guid, object> dictionary = new Dictionary<Guid, object>();
		if (sampleRate.HasValue)
		{
			dictionary.Add(((MediaAttributeKey)MediaTypeAttributeKeys.AudioSamplesPerSecond).Guid, sampleRate.Value);
		}
		if (numberOfChannels.HasValue)
		{
			dictionary.Add(((MediaAttributeKey)MediaTypeAttributeKeys.AudioNumChannels).Guid, numberOfChannels.Value);
		}
		if (bitsPerSample.HasValue)
		{
			dictionary.Add(((MediaAttributeKey)MediaTypeAttributeKeys.AudioBitsPerSample).Guid, bitsPerSample.Value);
		}
		if (blockAlignment.HasValue)
		{
			dictionary.Add(((MediaAttributeKey)MediaTypeAttributeKeys.AudioBlockAlignment).Guid, blockAlignment.Value);
		}
		return dictionary;
	}

	public static MediaType CreateAudioMediaType(int? sampleRate, int? numberOfChannels, int? bitsPerSample, int? fourCc)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		MediaType val = new MediaType();
		((MediaAttributes)val).Set<Guid>(MediaTypeAttributeKeys.MajorType, MediaTypeGuids.Audio);
		if (fourCc.HasValue)
		{
			((MediaAttributes)val).Set<Guid>(MediaTypeAttributeKeys.Subtype, ToMediaSubtype(fourCc.Value));
		}
		if (bitsPerSample.HasValue)
		{
			((MediaAttributes)val).Set<int>(MediaTypeAttributeKeys.AudioBitsPerSample, bitsPerSample.Value);
		}
		if (numberOfChannels.HasValue)
		{
			((MediaAttributes)val).Set<int>(MediaTypeAttributeKeys.AudioNumChannels, numberOfChannels.Value);
		}
		if (sampleRate.HasValue)
		{
			((MediaAttributes)val).Set<int>(MediaTypeAttributeKeys.AudioSamplesPerSecond, sampleRate.Value);
		}
		return val;
	}

	public static Guid ToMediaSubtype(int fourCc)
	{
		return new Guid(fourCc, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);
	}

	public static MediaType CreateAudioMediaTypeExt(int sampleRate, int numberOfChannels, int bitsPerSample)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		MediaType val = new MediaType();
		WaveFormat val2 = new WaveFormat(sampleRate, bitsPerSample, numberOfChannels);
		MediaFactory.InitMediaTypeFromWaveFormatEx(val, (WaveFormat[])(object)new WaveFormat[1] { val2 }, 18);
		return val;
	}

	public static void ClearMediaTypes(IEnumerable<MediaType> mediaTypes)
	{
		if (mediaTypes == null)
		{
			return;
		}
		foreach (MediaType mediaType in mediaTypes)
		{
			((DisposeBase)mediaType).Dispose();
		}
	}
}

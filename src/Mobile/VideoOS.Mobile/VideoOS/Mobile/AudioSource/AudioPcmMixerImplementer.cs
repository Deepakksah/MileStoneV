using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.VideoSource;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource;

public class AudioPcmMixerImplementer : IRsStreamMuxerImplementer<AudioData>
{
	public const int MaxRetriesToWaitForData = 3;

	private readonly IPcmSourceSettings _pcmSourceSettings;

	public AudioPcmMixerImplementer(IPcmSourceSettings pcmSourceSettings)
	{
		_pcmSourceSettings = pcmSourceSettings;
	}

	public IList<IRsStreamMuxerItemConsumer<AudioData>> Process(IList<IRsStreamMuxerItemConsumer<AudioData>> datas, OnStreamData<AudioData> onMuxedData)
	{
		int num = FindMinCommonBufferLen(datas);
		if (num > 0)
		{
			List<byte[]> input = ExtractBuffers(datas, num);
			List<int[]> inputs = ConvertToIntArrays(input);
			int[] input2 = Mix(inputs);
			input2 = DivideIfOneByte(input2);
			byte[] result = ConvertToByteArray(input2);
			NotifySubscribers(onMuxedData, result);
		}
		return datas;
	}

	private int FindMinCommonBufferLen(IList<IRsStreamMuxerItemConsumer<AudioData>> datas)
	{
		int num = int.MaxValue;
		foreach (IRsStreamMuxerItemConsumer<AudioData> data in datas)
		{
			if (data.MissingCount < 3)
			{
				num = Math.Min(num, data.DataList.TotalCount());
			}
		}
		if (num != int.MaxValue)
		{
			return num;
		}
		return 0;
	}

	private List<byte[]> ExtractBuffers(IList<IRsStreamMuxerItemConsumer<AudioData>> datas, int buffLen)
	{
		return datas.Select((IRsStreamMuxerItemConsumer<AudioData> data) => data.DataList.ExtractBytes(buffLen)).ToList();
	}

	private List<int[]> ConvertToIntArrays(IList<byte[]> input)
	{
		int bytesPerInt = (int)_pcmSourceSettings.BitsPerSample >> 3;
		return input.Select((byte[] byteArray) => ByteArrayHelper.ByteArrayToIntArray(byteArray, 0, byteArray.Length >> bytesPerInt - 1, bytesPerInt)).ToList();
	}

	private int[] Mix(IEnumerable<int[]> inputs)
	{
		int[] result = new int[inputs.First().Length];
		List<int[]> list = new List<int[]>(inputs);
		switch (inputs.Count())
		{
		case 1:
			result = list[0];
			break;
		case 2:
			result.Mix(list[0], list[1]);
			break;
		case 3:
			result.Mix(list[0], list[1], list[2]);
			break;
		default:
			result.Mix(list);
			break;
		}
		return result;
	}

	private int[] DivideIfOneByte(int[] input)
	{
		if (_pcmSourceSettings.BitsPerSample == PcmSourceSettings.BitsPerSampleType.OneByteInt)
		{
			for (int i = 0; i < input.Length; i++)
			{
				input[i] >>= 1;
			}
		}
		return input;
	}

	private byte[] ConvertToByteArray(int[] input)
	{
		return ByteArrayHelper.IntArrayToByteArray(input, 0, input.Length, (int)_pcmSourceSettings.BitsPerSample >> 3);
	}

	private void NotifySubscribers(OnStreamData<AudioData> onMuxedData, byte[] result)
	{
		AudioData audioData = new AudioData
		{
			Content = result,
			BeginTimeUtc = DateTime.UtcNow
		};
		onMuxedData?.Invoke(new AudioData[1] { audioData });
	}
}

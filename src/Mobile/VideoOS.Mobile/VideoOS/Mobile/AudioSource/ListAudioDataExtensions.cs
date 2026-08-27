using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.AudioSource;

public static class ListAudioDataExtensions
{
	public static int TotalCount(this List<AudioData> datas)
	{
		if (datas == null)
		{
			return 0;
		}
		int num = 0;
		foreach (AudioData data in datas)
		{
			num += data.Content.Length;
		}
		return num;
	}

	public static byte[] ExtractBytes(this List<AudioData> datas, int dataLen)
	{
		if (dataLen <= 0)
		{
			return null;
		}
		byte[] array = new byte[dataLen];
		int num = 0;
		while (num < dataLen && datas.Any())
		{
			AudioData audioData = datas[0];
			datas.RemoveAt(0);
			int val = dataLen - num;
			int num2 = audioData.Content.Length;
			int num3 = Math.Min(val, num2);
			Buffer.BlockCopy(audioData.Content, 0, array, num, num3);
			num += num3;
			int num4 = num2 - num3;
			if (num4 > 0)
			{
				byte[] array2 = new byte[num4];
				Buffer.BlockCopy(audioData.Content, audioData.Content.Length - num4, array2, 0, num4);
				audioData.Content = array2;
				datas.Insert(0, audioData);
				break;
			}
		}
		return array;
	}
}

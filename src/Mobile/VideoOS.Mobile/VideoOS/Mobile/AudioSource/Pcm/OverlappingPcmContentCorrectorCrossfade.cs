using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmContentCorrectorCrossfade : OverlappingPcmContentCorrector
{
	public OverlappingPcmContentCorrectorCrossfade(PcmSourceSettings pcmSourceSettings)
		: base(pcmSourceSettings, ActualCorrector)
	{
	}

	public static void ActualCorrector(LiveSourcePcmContent next, int bytesToBeRemoved, IPcmSourceSettings pcmSourceSettings)
	{
		if (next != null && pcmSourceSettings != null)
		{
			next.Content = ActualCorrector(next.Content, bytesToBeRemoved, pcmSourceSettings);
		}
	}

	public static byte[] ActualCorrector(byte[] content, int bytesToBeRemoved, IPcmSourceSettings pcmSourceSettings)
	{
		if (content == null || pcmSourceSettings == null)
		{
			return null;
		}
		int num = (int)pcmSourceSettings.BitsPerSample >> 3;
		int num2 = 0;
		if (bytesToBeRemoved > content.Length / 2)
		{
			bytesToBeRemoved = content.Length - bytesToBeRemoved;
			num2 = content.Length - bytesToBeRemoved * 2;
		}
		int intCounts = bytesToBeRemoved / num;
		int[] array = ByteArrayHelper.ByteArrayToIntArray(content, num2, intCounts, num);
		int[] second = ByteArrayHelper.ByteArrayToIntArray(content, num2 + bytesToBeRemoved, intCounts, num);
		Crossfade(array, second, array);
		content = OverlappingPcmContentCorrectorPrevious.ActualCorrector(content, num2 + bytesToBeRemoved, pcmSourceSettings);
		ByteArrayHelper.IntArrayToByteArray(array, 0, content, 0, intCounts, num);
		return content;
	}

	public static void Crossfade(int[] first, int[] second, int[] result)
	{
		int num = Math.Min(first.Length, Math.Min(second.Length, result.Length));
		for (int i = 0; i < num; i++)
		{
			result[i] = ((num - i) * first[i] + i * second[i]) / num;
		}
	}
}

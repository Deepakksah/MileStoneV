using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmContentCorrectorPrevious : OverlappingPcmContentCorrector
{
	public OverlappingPcmContentCorrectorPrevious(PcmSourceSettings pcmSourceSettings)
		: base(pcmSourceSettings, ActualCorrector)
	{
	}

	public static void ActualCorrector(LiveSourcePcmContent next, int bytesToBeRemoved, IPcmSourceSettings pcmSourceSettings)
	{
		if (next != null)
		{
			next.Content = ActualCorrector(next.Content, bytesToBeRemoved, pcmSourceSettings);
		}
	}

	public static byte[] ActualCorrector(byte[] content, int bytesToBeRemoved, IPcmSourceSettings pcmSourceSettings)
	{
		if (content == null)
		{
			return null;
		}
		int num = content.Length - bytesToBeRemoved;
		byte[] array = new byte[num];
		int num2 = Math.Min(num, content.Length);
		int sourceIndex = content.Length - num2;
		int destinationIndex = num - num2;
		Array.Copy(content, sourceIndex, array, destinationIndex, num2);
		return array;
	}
}

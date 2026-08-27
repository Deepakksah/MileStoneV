using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmContentCorrectorNext : OverlappingPcmContentCorrector
{
	public OverlappingPcmContentCorrectorNext(PcmSourceSettings pcmSourceSettings)
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
		Array.Copy(content, array, Math.Min(content.Length, num));
		return array;
	}
}

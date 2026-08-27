using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmContentCorrectorNull : IOverlappingPcmContentCorrector
{
	public Action<LiveSourcePcmContent> OnCorrectedPcmContent { get; set; }

	public IPcmSourceSettings PcmSourceSettings { get; private set; }

	public OverlappingPcmContentCorrectorNull(PcmSourceSettings pcmSourceSettings)
	{
		PcmSourceSettings = pcmSourceSettings;
	}

	public void OnPcmContent(LiveSourcePcmContent pcmContent)
	{
		OnCorrectedPcmContent(pcmContent);
	}

	public LiveSourcePcmContent PopLast()
	{
		return null;
	}
}

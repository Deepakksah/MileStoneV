using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.Pcm;

public interface IOverlappingPcmContentCorrector
{
	IPcmSourceSettings PcmSourceSettings { get; }

	Action<LiveSourcePcmContent> OnCorrectedPcmContent { get; set; }

	void OnPcmContent(LiveSourcePcmContent pcmContent);

	LiveSourcePcmContent PopLast();
}

using VideoOS.Mobile.AudioSource.Pcm;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource;

public static class StreamSourceSettingsHelper
{
	public static IOverlappingPcmContentCorrector CreatePcmContentCorrector(IStreamSourceSettings streamSourceSettings, PcmSourceSettings pcmSettings)
	{
		return streamSourceSettings?.AudioCorrection switch
		{
			AudioCorrection.Truncate => new OverlappingPcmContentCorrectorPrevious(pcmSettings), 
			AudioCorrection.CrossFade => new OverlappingPcmContentCorrectorCrossfade(pcmSettings), 
			_ => new OverlappingPcmContentCorrectorNull(pcmSettings), 
		};
	}

	public static IOverlappingPcmDataListCorrector CreatePcmDataListCorrector(IStreamSourceSettings streamSourceSettings)
	{
		return streamSourceSettings?.AudioCorrection switch
		{
			AudioCorrection.Truncate => new OverlappingPcmDataListCorrectorPrevious(), 
			AudioCorrection.CrossFade => new OverlappingPcmDataListCorrectorCrossfade(), 
			_ => new OverlappingPcmDataListCorrectorNull(), 
		};
	}
}

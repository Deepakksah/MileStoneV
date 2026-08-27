using VideoOS.Platform.Data;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmDataListCorrectorNull : IOverlappingPcmDataListCorrector
{
	public PcmAudioDataList Do(PcmAudioDataList pcmAudioDataList)
	{
		return pcmAudioDataList;
	}

	public void Reset()
	{
	}
}

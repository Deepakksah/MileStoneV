using VideoOS.Platform.Data;

namespace VideoOS.Mobile.AudioSource.Pcm;

public interface IOverlappingPcmDataListCorrector
{
	PcmAudioDataList Do(PcmAudioDataList pcmAudioDataList);

	void Reset();
}

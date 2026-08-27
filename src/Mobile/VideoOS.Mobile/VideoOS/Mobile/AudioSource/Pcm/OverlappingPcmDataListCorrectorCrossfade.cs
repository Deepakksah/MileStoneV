namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmDataListCorrectorCrossfade : OverlappingPcmDataListCorrector
{
	public OverlappingPcmDataListCorrectorCrossfade()
		: base(OverlappingPcmContentCorrectorCrossfade.ActualCorrector)
	{
	}
}

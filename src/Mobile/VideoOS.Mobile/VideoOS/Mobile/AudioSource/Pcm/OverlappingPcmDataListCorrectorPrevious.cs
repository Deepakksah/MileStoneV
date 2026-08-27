namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmDataListCorrectorPrevious : OverlappingPcmDataListCorrector
{
	public OverlappingPcmDataListCorrectorPrevious()
		: base(OverlappingPcmContentCorrectorPrevious.ActualCorrector)
	{
	}
}

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmDataListCorrectorNext : OverlappingPcmDataListCorrector
{
	public OverlappingPcmDataListCorrectorNext()
		: base(OverlappingPcmContentCorrectorNext.ActualCorrector)
	{
	}
}

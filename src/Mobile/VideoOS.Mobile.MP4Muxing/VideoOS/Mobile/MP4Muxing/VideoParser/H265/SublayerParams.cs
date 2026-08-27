namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class SublayerParams
{
	public ProfileTierLevelParams ProfileTierLevelParams;

	public byte? LevelIdc;

	public SublayerParams()
	{
		ProfileTierLevelParams = null;
		LevelIdc = null;
	}
}

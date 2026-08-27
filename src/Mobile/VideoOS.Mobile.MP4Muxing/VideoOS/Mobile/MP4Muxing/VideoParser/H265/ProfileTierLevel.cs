using System.Collections.Generic;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class ProfileTierLevel
{
	public ProfileTierLevelParams ProfileTierLevelParams = new ProfileTierLevelParams();

	public byte LevelIdc;

	public readonly List<SublayerParams> SubLayersParams = new List<SublayerParams>();
}

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class VideoWallPresetProxy : IVideoWallPresetProxy
{
	public VideoWallPreset QueryVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		return VideoWallPreset.QueryVideoWallPreset(requestContext, videoWallPresetId);
	}
}

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class VideoWallProxy : IVideoWallProxy
{
	public VideoWall QueryVideoWall(IGeminiRequestContext requestContext, string videoWallId)
	{
		return VideoWall.QueryVideoWall(requestContext, videoWallId);
	}
}

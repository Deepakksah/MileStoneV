namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IVideoWallProxy
{
	VideoWall QueryVideoWall(IGeminiRequestContext requestContext, string videoWallId);
}

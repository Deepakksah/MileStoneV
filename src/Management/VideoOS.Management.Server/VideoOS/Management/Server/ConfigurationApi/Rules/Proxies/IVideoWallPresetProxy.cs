namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IVideoWallPresetProxy
{
	VideoWallPreset QueryVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId);
}

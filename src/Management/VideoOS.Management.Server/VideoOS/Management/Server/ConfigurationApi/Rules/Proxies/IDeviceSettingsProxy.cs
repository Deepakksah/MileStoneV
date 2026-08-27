namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IDeviceSettingsProxy
{
	bool Validate(IGeminiRequestContext requestContext, string deviceId, string xmlSettings);
}

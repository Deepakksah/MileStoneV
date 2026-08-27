namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IHardwareProxy
{
	Hardware QueryHardwareById(IGeminiRequestContext requestContext, string hardwareId);
}

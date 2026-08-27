namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class HardwareProxy : IHardwareProxy
{
	public Hardware QueryHardwareById(IGeminiRequestContext requestContext, string hardwareId)
	{
		return Hardware.QueryHardwareById(requestContext, hardwareId);
	}
}

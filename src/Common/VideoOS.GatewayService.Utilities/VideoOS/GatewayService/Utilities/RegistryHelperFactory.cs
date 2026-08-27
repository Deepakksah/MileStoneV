using VideoOS.GatewayService.Utilities.Oem;

namespace VideoOS.GatewayService.Utilities;

public class RegistryHelperFactory : IRegistryHelperFactory
{
	public IRegistryHelper CreateRegistryHelper()
	{
		return new RegistryHelper(new GatewayServiceOemResource());
	}
}

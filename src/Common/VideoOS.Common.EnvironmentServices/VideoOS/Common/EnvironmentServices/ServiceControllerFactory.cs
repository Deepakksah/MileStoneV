namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
public class ServiceControllerFactory : IServiceControllerFactory
{
	public IServiceControllerService Create(string serviceName)
	{
		return new ServiceControllerService(serviceName);
	}
}

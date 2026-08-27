namespace VideoOS.Common.EnvironmentServices;

public interface IServiceControllerFactory
{
	IServiceControllerService Create(string serviceName);
}

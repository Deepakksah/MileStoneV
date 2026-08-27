namespace VideoOS.GatewayService.InstallerPlugin;

public class SetupIISFactory : ISetupIISFactory
{
	public ISetupIIS CreateSetupIIS()
	{
		return new SetupIIS();
	}
}

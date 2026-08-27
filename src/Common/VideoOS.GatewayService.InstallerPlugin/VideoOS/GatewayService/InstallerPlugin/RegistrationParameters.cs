using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.GatewayService.InstallerPlugin;

public class RegistrationParameters
{
	public string ManagementServerAddress { get; set; }

	public string GatewayServiceWebSiteId { get; set; }

	public bool IsGatewayServiceSecure { get; set; }

	public IServiceRegistrationServiceProxy ServiceRegistrationProxy { get; set; }
}

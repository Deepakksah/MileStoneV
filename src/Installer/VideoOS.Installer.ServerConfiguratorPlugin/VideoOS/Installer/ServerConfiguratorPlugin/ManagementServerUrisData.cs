namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class ManagementServerUrisData
{
	public string ProductVersion { get; set; }

	public string UnsecureManagementServer { get; set; }

	public string SecureManagementServer { get; set; }

	public string IdentityProvider { get; set; }

	public string[] ApiGateways { get; set; }
}

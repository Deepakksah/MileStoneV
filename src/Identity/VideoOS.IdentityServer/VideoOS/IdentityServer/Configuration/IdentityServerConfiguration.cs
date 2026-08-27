namespace VideoOS.IdentityServer.Configuration;

public class IdentityServerConfiguration
{
	public bool LogRequestResponse { get; set; }

	public bool AllowInsecureCookies { get; set; }

	public KeyManagementSettings KeyManagementSettings { get; set; } = new KeyManagementSettings();

	public IdentityProviderSettings IdentityProviderSettings { get; set; }

	public LockoutSettings LockoutSettings { get; set; } = new LockoutSettings();

	public PasswordSettings PasswordSettings { get; set; } = new PasswordSettings();

	public LoginSettings LoginSettings { get; set; } = new LoginSettings();

	public ProxySettings ProxySettings { get; set; } = new ProxySettings();

	public DataProtectionSettings DataProtectionSettings { get; set; } = new DataProtectionSettings();
}

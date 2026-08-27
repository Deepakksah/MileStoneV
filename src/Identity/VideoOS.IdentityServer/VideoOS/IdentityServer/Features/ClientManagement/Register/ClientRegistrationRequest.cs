namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public class ClientRegistrationRequest
{
	public string ClientName { get; set; }

	public string[] ClientScopes { get; set; }

	public string[] ClientGrantTypes { get; set; } = new string[1] { "client_credentials" };

	public string[] ClientRedirectUris { get; set; }

	public string[] PostLogoutRedirectUris { get; set; }

	public bool? RequireSecretForAuthCodeFlow { get; set; }
}

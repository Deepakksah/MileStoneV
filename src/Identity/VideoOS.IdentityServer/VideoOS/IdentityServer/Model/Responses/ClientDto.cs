namespace VideoOS.IdentityServer.Model.Responses;

public class ClientDto
{
	public int Id { get; set; }

	public string ClientId { get; set; }

	public string ClientName { get; set; }

	public string[] ClientScopes { get; set; }

	public string[] ClientRedirectUris { get; set; }
}

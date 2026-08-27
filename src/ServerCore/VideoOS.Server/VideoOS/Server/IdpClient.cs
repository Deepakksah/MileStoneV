namespace VideoOS.Server;

public class IdpClient
{
	public int Id { get; set; }

	public string ClientId { get; set; }

	public string ClientName { get; set; }

	public string[] ClientScopes { get; set; }

	public string[] ClientRedirectUris { get; set; }
}

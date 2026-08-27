namespace VideoOS.IdentityServer.Model.Responses;

public class RegisterClientResponse
{
	public string Name { get; set; }

	public string Secret { get; set; }

	public string[] Scopes { get; set; }
}

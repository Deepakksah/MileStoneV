namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public record PutScimUserRequest
{
	public string[] Schemas { get; set; }

	public bool Active { get; set; } = true;
}

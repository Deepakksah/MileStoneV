namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public record PatchScimUserRequest
{
	public string[] Schemas { get; set; }

	public ScimOperation[] Operations { get; set; }
}

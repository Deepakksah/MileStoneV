namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public record CreateScimUserRequest
{
	public string[] Schemas { get; set; }

	public string UserName { get; set; }

	public string ExternalId { get; set; }

	public bool Active { get; set; } = true;
}

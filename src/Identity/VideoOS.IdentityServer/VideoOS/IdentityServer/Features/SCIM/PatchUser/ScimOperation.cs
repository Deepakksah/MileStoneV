using System.Text.Json;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public record ScimOperation
{
	public string Op { get; set; }

	public string Path { get; set; }

	public JsonElement? Value { get; set; }
}

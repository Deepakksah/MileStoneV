using System.Text.Json.Serialization;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public record ScimErrorResponse
{
	[JsonPropertyName("schemas")]
	public string[] Schemas { get; set; } = new string[1] { "urn:ietf:params:scim:api:messages:2.0:Error" };

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("scimType")]
	public string ScimType { get; set; }

	[JsonPropertyName("detail")]
	public string Detail { get; set; }
}

using System.Text.Json.Serialization;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public record SchemaListResponse(int TotalResults, int StartIndex, int ItemsPerPage, SchemaResponse[] Resources)
{
	[JsonPropertyOrder(-1)]
	public string[] Schemas { get; } = new string[1] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" };
}

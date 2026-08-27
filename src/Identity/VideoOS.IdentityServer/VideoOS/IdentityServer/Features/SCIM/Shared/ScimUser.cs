using System;
using System.Text.Json.Serialization;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public record ScimUser([property: JsonPropertyName("id"), ScimSchema("string", false, "Unique identifier for the SCIM resource", true, "readOnly", "always", "server")] string Id, [property: JsonPropertyName("userName"), ScimSchema("string", false, "User's username", true, "readOnly", "always", "global")] string UserName, [property: JsonPropertyName("externalId"), ScimSchema("string", false, "A globally unique identifier for the user, set by the client", true, "readOnly", "default", "server")] string ExternalId, [property: JsonPropertyName("active"), ScimSchema("boolean", false, "A Boolean value indicating the user is active", false, "readWrite", "default", "none")] bool Active, [property: JsonPropertyName("meta"), ScimSchema("complex", false, "Metadata associated with the user", false, "readOnly", "default", "none")] ScimUser.ScimMeta Meta)
{
	public record ScimMeta([property: JsonPropertyName("created"), ScimSchema("dateTime", false, "The date and time the resource was created", false, "readOnly", "always", "none")] DateTime Created, [property: JsonPropertyName("lastModified"), ScimSchema("dateTime", false, "The date and time the resource was last modified", false, "readOnly", "always", "none")] DateTime LastModified, [property: JsonPropertyName("location"), ScimSchema("string", false, "The URI of the resource", false, "readOnly", "always", "none")] string Location)
	{
		[JsonPropertyName("resourceType")]
		[ScimSchema("string", false, "The type of resource", false, "readOnly", "always", "none")]
		public string ResourceType => "User";
	}

	[ScimSchema("string", false, "Schemas defining the attributes of the SCIM resource", true, "readOnly", "always", "none")]
	public string[] Schemas { get; } = new string[1] { "urn:ietf:params:scim:schemas:core:2.0:User" };
}

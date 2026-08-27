using System.Text.Json.Serialization;

namespace VideoOS.IdentityServer.Features.SCIM.GetSchema;

public record SchemaResponse(string Id, string Name, string Description, SchemaResponse.Attribute[] Attributes, SchemaResponse.MetaData Meta)
{
	public record Attribute(string Name, string Type, bool MultiValued, string Description, bool Required, string Mutability, string Returned, string Uniqueness)
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Attribute[] SubAttributes { get; init; }
	}

	public record MetaData(string ResourceType, string Location);

	[JsonPropertyOrder(-1)]
	public string[] Schemas { get; } = new string[1] { "urn:ietf:params:scim:schemas:core:2.0:Schema" };
}

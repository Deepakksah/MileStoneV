using System;
using System.Linq;
using System.Reflection;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Features.SCIM.GetSchema;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public class GetUserSchemaService(IRequestUrlBuilder requestUrlBuilder)
{
	private static readonly Lazy<SchemaResponse.Attribute[]> UserSchemaAttributesLazy = new Lazy<SchemaResponse.Attribute[]>(() => GenerateAttributes(typeof(ScimUser)));

	public SchemaResponse.Attribute[] GetUserSchemaAttributes()
	{
		return UserSchemaAttributesLazy.Value;
	}

	public SchemaResponse GenerateScimUserSchema()
	{
		SchemaResponse.Attribute[] attributes = UserSchemaAttributesLazy.Value.Where((SchemaResponse.Attribute attr) => !string.Equals(attr.Name, "Schemas", StringComparison.OrdinalIgnoreCase)).ToArray();
		return GenerateSchema("urn:ietf:params:scim:schemas:core:2.0:User", "User", "SCIM User Schema", attributes);
	}

	private SchemaResponse GenerateSchema(string id, string name, string description, SchemaResponse.Attribute[] attributes)
	{
		return new SchemaResponse(id, name, description, attributes, new SchemaResponse.MetaData("Schema", requestUrlBuilder.BuildBaseUrl()));
	}

	private static SchemaResponse.Attribute[] GenerateAttributes(Type type)
	{
		return type.GetProperties().Select(GenerateAttribute).ToArray();
	}

	private static SchemaResponse.Attribute GenerateAttribute(PropertyInfo property)
	{
		ScimSchemaAttribute customAttribute = property.GetCustomAttribute<ScimSchemaAttribute>();
		bool isArray = property.PropertyType.IsArray;
		Type propertyType = property.PropertyType;
		SchemaResponse.Attribute[] subAttributes = (((object)propertyType != null && propertyType.IsClass && !propertyType.IsArray && property.PropertyType != typeof(string)) ? property.PropertyType.GetProperties().Select(GenerateAttribute).ToArray() : null);
		return new SchemaResponse.Attribute(property.Name.ToLowerFirstChar(), customAttribute?.Type ?? (isArray ? "complex" : "string"), isArray || (customAttribute?.MultiValued ?? false), customAttribute?.Description ?? ("SCIM attribute for " + property.Name), customAttribute?.Required ?? false, customAttribute?.Mutability ?? "readWrite", customAttribute?.Returned ?? "always", customAttribute?.Uniqueness ?? "none")
		{
			SubAttributes = subAttributes
		};
	}
}

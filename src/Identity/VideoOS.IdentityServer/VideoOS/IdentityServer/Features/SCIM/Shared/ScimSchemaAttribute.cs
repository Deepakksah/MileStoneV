using System;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

[AttributeUsage(AttributeTargets.Property)]
public class ScimSchemaAttribute(string type, bool multiValued = false, string description = "", bool required = false, string mutability = "readWrite", string returned = "always", string uniqueness = "none") : Attribute
{
	public string Type { get; } = type;

	public bool MultiValued { get; } = multiValued;

	public string Description { get; } = description;

	public bool Required { get; } = required;

	public string Mutability { get; } = mutability;

	public string Returned { get; } = returned;

	public string Uniqueness { get; } = uniqueness;
}

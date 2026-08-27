namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimConstants
{
	public static class Provider
	{
		public const string ProviderId = "Scim.ProviderId";

		public const string ProviderDisplayName = "Scim.ProviderDisplayName";
	}

	public static class Policies
	{
		public const string ScimPolicy = "ScimPolicy";
	}

	public static class Schemas
	{
		public const string CoreUserSchema = "urn:ietf:params:scim:schemas:core:2.0:User";
	}
}

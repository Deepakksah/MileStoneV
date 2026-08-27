namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public static class ScimToVmsUserMapper
{
	public static string MapScimUserAttributeToVmsUserAttribute(string attribute)
	{
		return attribute.ToLowerInvariant() switch
		{
			"id" => "Id", 
			"username" => "UserName", 
			"externalid" => "Logins", 
			"active" => "Active", 
			"meta.created" => "CreatedDate", 
			"meta.lastmodified" => "ModifiedDate", 
			_ => string.Empty, 
		};
	}
}

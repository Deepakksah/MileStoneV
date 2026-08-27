using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace VideoOS.IdentityServer;

public static class ClaimActionCollectionExtension
{
	public static void MapAllExcept(this ClaimActionCollection collection, params string[] exclusions)
	{
		collection.Clear();
		collection.Add(new MapAllClaimsActionAndEnumerateArray());
		collection.DeleteClaims(exclusions);
	}
}

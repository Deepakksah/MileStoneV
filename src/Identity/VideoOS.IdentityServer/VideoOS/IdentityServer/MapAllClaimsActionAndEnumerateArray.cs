using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace VideoOS.IdentityServer;

public class MapAllClaimsActionAndEnumerateArray : ClaimAction
{
	public MapAllClaimsActionAndEnumerateArray()
		: base("All", "http://www.w3.org/2001/XMLSchema#string")
	{
	}

	public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
	{
		foreach (JsonProperty item in userData.EnumerateObject())
		{
			if (item.Value.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement item2 in item.Value.EnumerateArray())
				{
					string claimValue = item2.ToString();
					AddClaimIfNotDuplicate(identity, issuer, item.Name, claimValue);
				}
			}
			else
			{
				string claimValue2 = item.Value.ToString();
				AddClaimIfNotDuplicate(identity, issuer, item.Name, claimValue2);
			}
		}
	}

	private static void AddClaimIfNotDuplicate(ClaimsIdentity identity, string issuer, string type, string claimValue)
	{
		if (identity.FindFirst((Claim c) => string.Equals(c.Type, type, StringComparison.Ordinal) && string.Equals(c.Value, claimValue, StringComparison.Ordinal)) == null)
		{
			identity.AddClaim(new Claim(type, claimValue, "http://www.w3.org/2001/XMLSchema#string", issuer));
		}
	}
}

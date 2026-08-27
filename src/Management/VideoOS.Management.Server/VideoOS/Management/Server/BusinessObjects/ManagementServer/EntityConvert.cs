using System.Collections.Generic;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer;

public static class EntityConvert
{
	public static GenericObject2 ConvertToGenericObject2(Claim claim)
	{
		if (claim == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("ClaimProvider", claim.LoginProviderId.ToString()));
		list.Add(new ObjectProperty2("ClaimName", claim.Name));
		list.Add(new ObjectProperty2("ClaimValue", claim.Value));
		return new GenericObject2(null, null, list.ToArray(), null);
	}

	public static GenericObject2[] ConvertToGenericObject2s(IEnumerable<Claim> claims)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Claim claim in claims)
		{
			list.Add(ConvertToGenericObject2(claim));
		}
		return list.ToArray();
	}
}

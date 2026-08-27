using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("RegisteredClaim")]
public class RegisteredClaimGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(RegisteredClaim.QueryById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(RegisteredClaim.QueryByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "LoginProvider")
		{
			return RegisteredClaim.QueryIdsByLoginProvider(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		RegisteredClaimNotValidated registeredClaimNotValidated = ConvertToRegisteredClaim(properties);
		return ConvertToGenericObject2(RegisteredClaim.Create(requestContext, registeredClaimNotValidated.LoginProviderId, registeredClaimNotValidated.Name, registeredClaimNotValidated.DisplayName, registeredClaimNotValidated.CaseSensitive));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		RegisteredClaimNotValidated registeredClaimNotValidated = ConvertToRegisteredClaim(genericObject);
		return ConvertToGenericObject2(RegisteredClaim.Update(requestContext, registeredClaimNotValidated.Id.ToString(CultureInfo.InvariantCulture), registeredClaimNotValidated.Name, registeredClaimNotValidated.DisplayName));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		RegisteredClaim.Delete(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(RegisteredClaimData claim)
	{
		if (claim == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", claim.Id));
		list.Add(new ObjectProperty2("Name", claim.Name));
		list.Add(new ObjectProperty2("LoginProviderId", claim.LoginProviderId.ToString()));
		list.Add(new ObjectProperty2("DisplayName", claim.DisplayName));
		list.Add(new ObjectProperty2("CaseSensitive", claim.CaseSensitive));
		return new GenericObject2(claim.Id.ToString(CultureInfo.InvariantCulture), "RegisteredClaim", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGenericObject2(IEnumerable<RegisteredClaimData> claims)
	{
		if (claims != null)
		{
			return claims.Select((RegisteredClaimData c) => ConvertToGenericObject2(c)).ToArray();
		}
		return Array.Empty<GenericObject2>();
	}

	private static RegisteredClaimNotValidated ConvertToRegisteredClaim(GenericObject2 genericObject)
	{
		return ConvertToRegisteredClaim(genericObject.Properties);
	}

	private static RegisteredClaimNotValidated ConvertToRegisteredClaim(ObjectProperty2[] properties)
	{
		RegisteredClaimNotValidated registeredClaimNotValidated = new RegisteredClaimNotValidated();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			registeredClaimNotValidated.Id = GenericObjectHelper2.GetProperty<int>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			registeredClaimNotValidated.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "LoginProviderId"))
		{
			registeredClaimNotValidated.LoginProviderId = GenericObjectHelper2.GetProperty<string>(properties, "LoginProviderId");
		}
		if (GenericObjectHelper2.Contains(properties, "DisplayName"))
		{
			registeredClaimNotValidated.DisplayName = GenericObjectHelper2.GetProperty<string>(properties, "DisplayName");
		}
		if (GenericObjectHelper2.Contains(properties, "CaseSensitive"))
		{
			registeredClaimNotValidated.CaseSensitive = GenericObjectHelper2.GetProperty<bool>(properties, "CaseSensitive");
		}
		return registeredClaimNotValidated;
	}
}

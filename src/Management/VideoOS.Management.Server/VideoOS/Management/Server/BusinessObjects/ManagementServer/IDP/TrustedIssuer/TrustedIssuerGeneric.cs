using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.DataComponents.IDP;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.IDP.TrustedIssuer;

[EntityName("TrustedIssuer")]
public class TrustedIssuerGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(TrustedIssuer.QueryById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		TrustedIssuerData[] array = TrustedIssuer.QueryByIds(requestContext, ids);
		if (array != null)
		{
			return array.Select(ConvertToGenericObject2).ToArray();
		}
		return Array.Empty<GenericObject2>();
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (!(parentEntityName == "ManagementServer"))
		{
			return null;
		}
		return TrustedIssuer.QueryIds(requestContext);
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		TrustedIssuerData trustedIssuerData = ConvertToTrustedIssuerData(properties);
		trustedIssuerData = TrustedIssuer.Create(requestContext, trustedIssuerData.Issuer, trustedIssuerData.Address);
		return ConvertToGenericObject2(trustedIssuerData);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		TrustedIssuerData trustedIssuerData = ConvertToTrustedIssuerData(genericObject.Properties);
		trustedIssuerData = TrustedIssuer.Update(requestContext, trustedIssuerData.Id, trustedIssuerData.Issuer, trustedIssuerData.Address);
		return ConvertToGenericObject2(trustedIssuerData);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		TrustedIssuer.ConvertToInt(id, "id", out var result);
		TrustedIssuer.Delete(requestContext, result);
	}

	private static GenericObject2 ConvertToGenericObject2(TrustedIssuerData data)
	{
		if (data == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>
		{
			new ObjectProperty2("Id", data.Id),
			new ObjectProperty2("Issuer", data.Issuer),
			new ObjectProperty2("Address", data.Address)
		};
		return new GenericObject2(data.Id.ToString(CultureInfo.InvariantCulture), "TrustedIssuer", list.ToArray(), null);
	}

	private static TrustedIssuerData ConvertToTrustedIssuerData(ObjectProperty2[] properties)
	{
		TrustedIssuerData trustedIssuerData = new TrustedIssuerData();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			trustedIssuerData.Id = GenericObjectHelper2.GetProperty<int>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Address"))
		{
			trustedIssuerData.Address = GenericObjectHelper2.GetProperty<string>(properties, "Address");
		}
		if (GenericObjectHelper2.Contains(properties, "Issuer"))
		{
			trustedIssuerData.Issuer = GenericObjectHelper2.GetProperty<string>(properties, "Issuer");
		}
		return trustedIssuerData;
	}
}

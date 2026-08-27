using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

[FunctionHandler("GetVmsWebClient")]
[FunctionHandler("UpdateVmsWebClient")]
public class IdpClientGeneric : IGenericFunction
{
	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (!(functionName == "GetVmsWebClient"))
		{
			if (functionName == "UpdateVmsWebClient")
			{
				return UpdateVmsWebClient(requestContext, parameters);
			}
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
		}
		return GetVmsWebClient(requestContext, parameters);
	}

	private static GenericObject2[] GetVmsWebClient(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		try
		{
			GenericObject2 genericObject = parameters.FirstOrDefault();
			Validation.CheckObjectReference(genericObject, "parameters");
			Validation.CheckObjectReference(genericObject.Properties, "properties");
			IdpClientData idpClientData = IdpClient.QueryByClientId(requestContext, "VmsWebClient");
			return ConvertToGenericObject2s(new IdpClientData[1] { idpClientData });
		}
		catch (NotFoundException)
		{
			return null;
		}
	}

	private static GenericObject2[] UpdateVmsWebClient(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string[] uris = genericObject.Properties.Select((ObjectProperty2 prop) => prop.Value as string).ToArray();
		IdpClient.Update(requestContext, "VmsWebClient", "VmsWebClient", IdpClient.WebClientValues.Scopes, uris);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditClientRedirectUrisSet("VmsWebClient", uris, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return null;
	}

	private static GenericObject2 ConvertToGenericObject2(IdpClientData idpClient)
	{
		if (idpClient == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", idpClient.Id));
		list.Add(new ObjectProperty2("ClientId", idpClient.ClientId));
		list.Add(new ObjectProperty2("ClientName", idpClient.ClientName));
		list.Add(new ObjectProperty2("ClientScopes", idpClient.Scopes.Select((string s, int i) => new ObjectProperty2(i.ToString(CultureInfo.InvariantCulture), s)).ToArray()));
		list.Add(new ObjectProperty2("ClientRedirectUris", idpClient.RedirectUris.Select((string s, int i) => new ObjectProperty2(i.ToString(CultureInfo.InvariantCulture), s)).ToArray()));
		return new GenericObject2(idpClient.Id.ToString(CultureInfo.InvariantCulture), "IdpClient", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGenericObject2s(IEnumerable<IdpClientData> clients)
	{
		if (clients != null)
		{
			return clients.Select((IdpClientData p) => ConvertToGenericObject2(p)).ToArray();
		}
		return Array.Empty<GenericObject2>();
	}
}

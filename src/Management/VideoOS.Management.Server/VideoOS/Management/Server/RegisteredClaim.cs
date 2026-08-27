using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public static class RegisteredClaim
{
	public static RegisteredClaimData QueryById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectIdAsInt32(id, "id", allowNullOrEmpty: false);
		int id2 = Convert.ToInt32(id, CultureInfo.InvariantCulture);
		using IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent();
		return registeredClaimComponent.QueryById(id2);
	}

	public static string[] QueryIdsByLoginProvider(IGeminiRequestContext requestContext, string loginProviderId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectId(loginProviderId, "loginProviderId", allowNullOrEmpty: false);
		using IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent();
		return (from id in registeredClaimComponent.QueryIdsByLoginProvider(new Guid(loginProviderId))
			select id.ToString(CultureInfo.InvariantCulture)).ToArray();
	}

	public static RegisteredClaimData[] QueryByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectIdInt32Array(ids, "ids", allowNullOrEmptyIds: false);
		IEnumerable<int> ids2 = ids.Select((string id) => Convert.ToInt32(id, CultureInfo.InvariantCulture));
		using IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent();
		return registeredClaimComponent.QueryByIds(ids2);
	}

	public static RegisteredClaimData Create(IGeminiRequestContext requestContext, string loginProviderId, string name, string displayName, bool? caseSensitive)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(loginProviderId, "loginProviderId", allowNullOrEmpty: false);
		Validation.CheckName(displayName, "displayName");
		Validation.CheckName(name, "name");
		Validation.CheckName(displayName, "displayName");
		Validation.CheckBooleanValue(caseSensitive, "caseSensitive", allowNull: false);
		using (ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent())
		{
			loginProviderIdpComponent.QueryLoginProviderById(new Guid(loginProviderId));
		}
		RegisteredClaimData registeredClaimData;
		using (IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent())
		{
			registeredClaimData = registeredClaimComponent.Create(new Guid(loginProviderId), name, displayName, caseSensitive.Value);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditClaimCreated(registeredClaimData.Id, registeredClaimData.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return registeredClaimData;
	}

	public static RegisteredClaimData Update(IGeminiRequestContext requestContext, string id, string name, string displayName)
	{
		UpdateValidation(requestContext, id, name, displayName);
		int id2 = Convert.ToInt32(id, CultureInfo.InvariantCulture);
		RegisteredClaimData registeredClaimData = null;
		RegisteredClaimData registeredClaimData2;
		using (IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent())
		{
			if (!string.IsNullOrEmpty(name))
			{
				registeredClaimData = registeredClaimComponent.QueryById(id2);
				if (registeredClaimData == null)
				{
					throw VmoExceptionFactory.VMO60367_ObjectNotFoundException();
				}
			}
			registeredClaimComponent.Update(id2, name, displayName);
			registeredClaimData2 = registeredClaimComponent.QueryById(id2);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditClaimUpdated(registeredClaimData2.Id, registeredClaimData2.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		if (registeredClaimData != null)
		{
			ClaimToRoleMapper.RegisteredClaimNameChanged(requestContext, registeredClaimData2.LoginProviderId, registeredClaimData.Name, registeredClaimData2.Name);
		}
		return registeredClaimData2;
	}

	public static void UpdateValidation(IGeminiRequestContext requestContext, string id, string name, string displayName)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectIdAsInt32(id, "id", allowNullOrEmpty: false);
		Validation.CheckName(displayName, "displayName", allowNull: true);
		Validation.CheckName(name, "name", allowNull: true);
	}

	public static void Delete(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectIdAsInt32(id, "id", allowNullOrEmpty: false);
		int id2 = Convert.ToInt32(id, CultureInfo.InvariantCulture);
		Guid sessionId = Guid.NewGuid();
		using (IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent())
		{
			registeredClaimComponent.Delete(id2, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}

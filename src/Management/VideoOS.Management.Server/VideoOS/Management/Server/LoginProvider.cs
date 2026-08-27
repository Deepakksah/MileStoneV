using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public static class LoginProvider
{
	public static LoginProviderData[] QueryAll(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent();
		return loginProviderIdpComponent.QueryLoginProviders();
	}

	public static string[] QueryIds(IGeminiRequestContext requestContext)
	{
		return (from lp in QueryAll(requestContext)
			select lp.Id.ToString()).ToArray();
	}

	public static LoginProviderData QueryById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: true);
		using ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent();
		return loginProviderIdpComponent.QueryLoginProviderById(new Guid(id));
	}

	public static LoginProviderData[] QuerysByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectIdArray(ids, "ids", allowNullOrEmptyIds: false);
		using ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent();
		return loginProviderIdpComponent.QueryLoginProvidersByIds(ids.Select((string id) => new Guid(id)));
	}

	public static LoginProviderData Create(IGeminiRequestContext requestContext, string id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool promptForLogin, bool enabled)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: true);
		Validation.CheckName(name, "name");
		Validation.CheckName(clientId, "clientId");
		Validation.CheckName(clientSecret, "clientSecret");
		Validation.CheckName(clientSecretType, "clientSecretType", allowNull: true);
		Validation.CheckLoginProviderCallbackPath(callbackPath, "callbackPath");
		Validation.CheckSecureWebAddress(authority, "authority", allowNull: false);
		Validation.EnsureStringNotNull(ref userNameClaimType, "userNameClaimType");
		Validation.CheckObjectReference(scopes, "scopes");
		foreach (string scope in scopes)
		{
			Validation.CheckName(scope, "scopes");
		}
		id = GeminiResourceComponent.GetGuid(id);
		Guid guid = new Guid(id);
		LoginProviderData loginProviderData;
		using (ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent())
		{
			loginProviderData = loginProviderIdpComponent.CreateLoginProvider(guid, name, clientId, clientSecret, clientSecretType, callbackPath, authority, scopes, userNameClaimType, promptForLogin, enabled);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditLoginProviderCreated(guid, loginProviderData.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return loginProviderData;
	}

	public static LoginProviderData Update(IGeminiRequestContext requestContext, string id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool? promptForLogin, bool? enabled)
	{
		UpdateValidation(requestContext, id, name, clientId, clientSecret, clientSecretType, callbackPath, authority, scopes);
		Guid guid = new Guid(id);
		LoginProviderData loginProviderData;
		using (ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent())
		{
			loginProviderData = loginProviderIdpComponent.UpdateLoginProvider(guid, name, clientId, clientSecret, clientSecretType, callbackPath, authority, scopes, userNameClaimType, promptForLogin, enabled);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditLoginProviderUpdated(guid, loginProviderData.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return loginProviderData;
	}

	public static void UpdateValidation(IGeminiRequestContext requestContext, string id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckName(name, "name", allowNull: true);
		Validation.CheckName(clientId, "clientId", allowNull: true);
		Validation.CheckName(clientSecret, "clientSecret", allowNull: true);
		Validation.CheckName(clientSecretType, "clientSecretType", allowNull: true);
		Validation.CheckLoginProviderCallbackPath(callbackPath, "callbackPath", allowNull: true);
		Validation.CheckSecureWebAddress(authority, "authority", allowNull: true);
		if (scopes == null)
		{
			return;
		}
		foreach (string scope in scopes)
		{
			Validation.CheckName(scope, "scopes");
		}
	}

	public static void Delete(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Guid providerId = new Guid(id);
		using (IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent())
		{
			if (userIdpComponent.GetAllVmsUsers().Any((BasicUser u) => u.IsExternal && u.Claims.Any((Claim c) => c.LoginProviderId == providerId)))
			{
				throw VmoExceptionFactory.VMO60369_VmoServerServiceException();
			}
		}
		using (IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent())
		{
			if (registeredClaimComponent.QueryIdsByLoginProvider(providerId).Any())
			{
				throw VmoExceptionFactory.VMO60368_VmoServerServiceException();
			}
		}
		LoginProviderData loginProviderData;
		using (ILoginProviderIdpComponent loginProviderIdpComponent = requestContext.GetService<IdpClientService>().CreateLoginProviderIdpComponent())
		{
			try
			{
				loginProviderData = loginProviderIdpComponent.QueryLoginProviderById(providerId);
			}
			catch (ObjectNotFoundException)
			{
				throw VmoExceptionFactory.VMO60365_ObjectNotFoundException();
			}
			loginProviderIdpComponent.DeleteLoginProvider(providerId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditLoginProviderDeleted(providerId, loginProviderData.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}

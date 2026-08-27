using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public static class IdpClient
{
	public static class WebClientValues
	{
		public const string ClientId = "VmsWebClient";

		public const string ClientName = "VmsWebClient";

		public static string[] Scopes = new string[5] { "managementserver", "openid", "profile", "offline_access", "email" };
	}

	public static IdpClientData[] QueryAll(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRegistrationClient registrationClient = new IdpRegistrationClient(new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/idp")));
		return (from el in registrationClient.GetClients().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult()
			select ConvertToIdpClientData(el)).ToArray();
	}

	public static string[] QueryClientIds(IGeminiRequestContext requestContext)
	{
		return (from lp in QueryAll(requestContext)
			select lp.ClientId.ToString()).ToArray();
	}

	public static IdpClientData QueryByClientId(IGeminiRequestContext requestContext, string clientId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckName(clientId, "clientId");
		using IRegistrationClient registrationClient = new IdpRegistrationClient(new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/idp")));
		return ConvertToIdpClientData(registrationClient.GetClient(clientId).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult());
	}

	public static IdpClientData[] QuerysByClientIds(IGeminiRequestContext requestContext, string[] clientIds)
	{
		HashSet<string> clientIdHash = new HashSet<string>(clientIds);
		return (from lp in QueryAll(requestContext)
			where clientIdHash.Contains(lp.ClientId.ToString())
			select lp).ToArray();
	}

	public static void Update(IGeminiRequestContext requestContext, string clientId, string name, IEnumerable<string> scopes, IEnumerable<string> uris)
	{
		UpdateValidation(requestContext, clientId, name, scopes, uris);
		using (IRegistrationClient registrationClient = new IdpRegistrationClient(new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/idp"))))
		{
			if (uris != null && uris.Any())
			{
				registrationClient.RegisterAuthorizationCodeClient(clientId, name, scopes, uris).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
			}
			else
			{
				registrationClient.RegisterClient(clientId, name, scopes).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditClientRedirectUrisSet(clientId, uris ?? Enumerable.Empty<string>(), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static void UpdateValidation(IGeminiRequestContext requestContext, string clientId, string name, IEnumerable<string> scopes, IEnumerable<string> uris)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckStringNotNullOrWhiteSpace(clientId, "clientId");
		Validation.CheckName(name, "name", allowNull: true);
		if (scopes != null)
		{
			foreach (string scope in scopes)
			{
				Validation.CheckName(scope, "scopes");
			}
		}
		if (uris == null)
		{
			return;
		}
		foreach (string uri in uris)
		{
			Validation.CheckUri(uri, "uri");
		}
	}

	private static IdpClientData ConvertToIdpClientData(ClientGetModel getModel)
	{
		if (getModel == null)
		{
			return null;
		}
		return new IdpClientData
		{
			Id = getModel.Id,
			ClientId = getModel.ClientId,
			ClientName = getModel.ClientName,
			Scopes = new Collection<string>(getModel.ClientScopes ?? Array.Empty<string>()),
			RedirectUris = new Collection<string>(getModel.ClientRedirectUris ?? Array.Empty<string>())
		};
	}
}

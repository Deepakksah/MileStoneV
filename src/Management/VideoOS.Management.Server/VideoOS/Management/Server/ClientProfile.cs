using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class ClientProfile
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime LastModified { get; set; }

	public bool IsDefaultProfile { get; set; }

	public string SettingsXml { get; set; }

	public ClientProfileType Profiletype { get; set; }

	public static string[] QueryClientProfileIds(IGeminiRequestContext requestContext, ClientProfileType profiletype)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryClientProfileIds(profiletype);
	}

	public static string QueryClientProfileIdByRoleId(IGeminiRequestContext requestContext, ClientProfileType profiletype, string roleId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryProfileIdByRoleId(roleId, profiletype);
	}

	public static ClientProfile QueryClientProfileById(IGeminiRequestContext requestContext, string id)
	{
		if (QueryActiveProfileId(requestContext, ClientProfileType.SmartClientProfile) != id)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryClientProfileById(id);
	}

	public static ClientProfile QueryActiveSmartClientProfile(IGeminiRequestContext requestContext)
	{
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		string id = clientProfileComponent.QueryActiveProfileId(requestContext.AuthenticatedUserSid, ClientProfileType.SmartClientProfile);
		return clientProfileComponent.QueryClientProfileById(id);
	}

	public static ClientProfile[] QueryClientProfilesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryClientProfilesByIds(ids);
	}

	public static ClientProfile CreateClientProfile(IGeminiRequestContext requestContext, string id, string name, string description, string settings, ClientProfileType profileType)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		CheckSmartClientProfilesLicense(requestContext);
		id = GeminiResourceComponent.GetGuid(id);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
		ClientProfile clientProfile;
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfileComponent.CreateClientProfile(id, name, description, settings, profileType);
			clientProfile = clientProfileComponent.QueryClientProfileById(id);
		}
		if (profileType == ClientProfileType.SmartClientProfile)
		{
			using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
			vmoLogManager.LogEntryAuditLogger.AuditClientProfileCreated(new Guid(clientProfile.Id), clientProfile.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ClientProfile", clientProfile.Id, EventAction.Added, clientProfile.Name);
		return clientProfile;
	}

	public static ClientProfile UpdateClientProfile(IGeminiRequestContext requestContext, string id, string name, string description, string settings, ClientProfileType profileType)
	{
		UpdateClientProfileValidation(requestContext, ref id, name, ref description);
		ClientProfile clientProfile;
		ClientProfile clientProfile2;
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfile = clientProfileComponent.QueryClientProfileById(id);
			clientProfileComponent.UpdateClientProfile(id, name, description, settings);
			clientProfile2 = clientProfileComponent.QueryClientProfileById(id);
		}
		if (profileType == ClientProfileType.SmartClientProfile)
		{
			if (!string.Equals(clientProfile.Name, clientProfile2.Name))
			{
				using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
				vmoLogManager.LogEntryAuditLogger.AuditClientProfileRenamed(clientProfile.Name, clientProfile2.Name, new Guid(clientProfile2.Id), requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			if (!string.Equals(clientProfile.SettingsXml, clientProfile2.SettingsXml))
			{
				using VmoLogManager vmoLogManager2 = new VmoLogManager(requestContext);
				vmoLogManager2.LogEntryAuditLogger.AuditClientProfileEdited(new Guid(clientProfile2.Id), clientProfile2.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
		}
		Integration.FireConfigurationChanged(requestContext, "ClientProfile", clientProfile2.Id, EventAction.Updated, clientProfile2.Name);
		return clientProfile2;
	}

	public static void UpdateClientProfileValidation(IGeminiRequestContext requestContext, ref string id, string name, ref string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			throw new UnlicensedSmartClientProfilesFeatureException();
		}
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
	}

	public static void DeleteClientProfile(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			throw new UnlicensedSmartClientProfilesFeatureException();
		}
		ClientProfile clientProfile;
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfile = clientProfileComponent.QueryClientProfileById(id);
			if (clientProfile == null)
			{
				throw new ClientProfileDeleteException();
			}
			clientProfileComponent.DeleteClientProfile(id);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditClientProfileDelete(new Guid(clientProfile.Id), clientProfile.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ClientProfile", clientProfile.Id, EventAction.Deleted, clientProfile.Name);
	}

	public static void UpdateClientProfileMoveUp(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfileComponent.MovePriorityUp(id);
		}
		Integration.FireConfigurationChanged(requestContext, "ClientProfile", id, EventAction.Updated, string.Empty);
	}

	public static void UpdateClientProfileMoveDown(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfileComponent.MovePriorityDown(id);
		}
		Integration.FireConfigurationChanged(requestContext, "ClientProfile", id, EventAction.Updated, string.Empty);
	}

	public static string QueryActiveProfileId(IGeminiRequestContext requestContext, ClientProfileType profileType)
	{
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryActiveProfileId(requestContext.AuthenticatedUserSid, profileType);
	}

	public static string QueryActiveProfileId(IGeminiRequestContext requestContext, ClientProfileType profileType, string sid)
	{
		using IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent();
		return clientProfileComponent.QueryActiveProfileId(sid, profileType);
	}

	public static void AttachClientProfileToRole(IGeminiRequestContext requestContext, string id, string roleId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
		{
			clientProfileComponent.AttachClientProfileToRole(id, roleId, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	private static void CheckSmartClientProfilesLicense(IGeminiRequestContext requestContext)
	{
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			if (int.TryParse(GeminiApplicationCore.FeatureCacheManager.GetFeatureParameterValue(requestContext, "SmartClientProfiles", "SmartClientProfilesLimit"), out var result))
			{
				string[] array;
				using (IClientProfileComponent clientProfileComponent = requestContext.CreateClientProfileComponent())
				{
					array = clientProfileComponent.QueryClientProfileIds(ClientProfileType.SmartClientProfile);
				}
				if (array.Length + 1 > result)
				{
					throw new UnlicensedSmartClientProfilesFeatureException();
				}
			}
			return;
		}
		throw new UnlicensedSmartClientProfilesFeatureException();
	}
}

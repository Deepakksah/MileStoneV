using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public class PatrollingProfile
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string CameraDeviceId { get; set; }

	public string DefinitionXml { get; set; }

	public DateTime LastModified { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static PatrollingProfile[] QueryPatrollingProfiles(IGeminiRequestContext requestContext, string[] patrollingProfileIds)
	{
		PatrollingProfile[] patrollingProfiles;
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfiles = patrollingProfileComponent.QueryPatrollingProfiles(patrollingProfileIds);
		}
		return FilterPatrollingProfilesBasedOnPermissions(requestContext, patrollingProfiles);
	}

	public static PatrollingProfile[] QueryPatrollingProfiles(IGeminiRequestContext requestContext, string cameraDeviceId)
	{
		PatrollingProfile[] patrollingProfiles;
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfiles = patrollingProfileComponent.QueryPatrollingProfiles(cameraDeviceId);
		}
		return FilterPatrollingProfilesBasedOnPermissions(requestContext, patrollingProfiles);
	}

	private static PatrollingProfile[] FilterPatrollingProfilesBasedOnPermissions(IGeminiRequestContext requestContext, PatrollingProfile[] patrollingProfiles)
	{
		List<PatrollingProfile> list = new List<PatrollingProfile>();
		foreach (PatrollingProfile patrollingProfile in patrollingProfiles)
		{
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", patrollingProfile.CameraDeviceId))
			{
				list.Add(patrollingProfile);
			}
		}
		return list.ToArray();
	}

	public static PatrollingProfile CreatePatrollingProfile(IGeminiRequestContext requestContext, PatrollingProfile newPatrollingProfile)
	{
		newPatrollingProfile.Id = GeminiResourceComponent.GetGuid(newPatrollingProfile.Id);
		Validation.CheckName(newPatrollingProfile.Name, "name", allowNull: false);
		Validation.CheckPatrollingProfileDefinition("definitionXml", newPatrollingProfile.DefinitionXml);
		PatrollingSchema patrollingSchema = new PatrollingSchema(new Guid(newPatrollingProfile.Id));
		patrollingSchema.LoadXml(newPatrollingProfile.DefinitionXml);
		newPatrollingProfile.DefinitionXml = patrollingSchema.ToXml();
		Guid sessionId = Guid.NewGuid();
		ValidatePatrollingCore(requestContext, newPatrollingProfile.CameraDeviceId);
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfileComponent.CreatePatrollingProfile(newPatrollingProfile.CameraDeviceId, newPatrollingProfile, sessionId);
			newPatrollingProfile = patrollingProfileComponent.QueryPatrollingProfileById(newPatrollingProfile.Id);
			Integration.FirePatrollingChanged(requestContext, newPatrollingProfile.Id, newPatrollingProfile.CameraDeviceId, EventAction.Added);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newPatrollingProfile;
	}

	public static PatrollingProfile UpdatePatrollingProfile(IGeminiRequestContext requestContext, PatrollingProfile patrollingProfile)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckName(patrollingProfile.Name, "name", allowNull: false);
		Validation.CheckPatrollingProfileDefinition("definitionXml", patrollingProfile.DefinitionXml);
		ValidatePatrollingCore(requestContext, patrollingProfile.CameraDeviceId);
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfileComponent.UpdatePatrollingProfile(patrollingProfile.Id, patrollingProfile.Name, patrollingProfile.Description, patrollingProfile.DefinitionXml, sessionId);
			patrollingProfile = patrollingProfileComponent.QueryPatrollingProfileById(patrollingProfile.Id);
			Integration.FirePatrollingChanged(requestContext, patrollingProfile.Id, patrollingProfile.CameraDeviceId, EventAction.Updated);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return patrollingProfile;
	}

	public static void DeletePatrollingProfile(IGeminiRequestContext requestContext, string patrollingProfileId)
	{
		Guid sessionId = Guid.NewGuid();
		PatrollingProfile patrollingProfile;
		using (IPatrollingProfileComponent patrollingProfileComponent = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfile = patrollingProfileComponent.QueryPatrollingProfileById(patrollingProfileId);
		}
		if (patrollingProfile == null)
		{
			throw new RemoveNonExistingPatrollingProfileException();
		}
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", patrollingProfile.CameraDeviceId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new DeviceNotFoundException();
		}
		using (IPatrollingProfileComponent patrollingProfileComponent2 = requestContext.CreatePatrollingProfileComponent())
		{
			patrollingProfileComponent2.DeletePatrollingProfile(patrollingProfile.Id, patrollingProfile.Name, sessionId);
			Integration.FirePatrollingChanged(requestContext, patrollingProfile.Id, patrollingProfile.CameraDeviceId, EventAction.Deleted);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static void ValidatePatrollingCore(IGeminiRequestContext requestContext, string deviceId)
	{
		switch (requestContext.DeviceTypeLookup.QueryDeviceTypeFromDeviceId(requestContext, deviceId))
		{
		case DeviceType.Unknown:
			throw new DeviceNotFoundException();
		default:
			throw new PatrollingProfilePtzNotSupportedException();
		case DeviceType.Camera:
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", deviceId);
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities((deviceComponent.QueryPtzPresets(deviceId) ?? throw new DeviceNotFoundException()).SettingsXml);
			if (!deviceSettingUtilities.IsPresetEnabled || deviceSettingUtilities.PresetConfiguration == null)
			{
				throw new PatrollingProfilePtzNotSupportedException();
			}
			break;
		}
		}
	}
}

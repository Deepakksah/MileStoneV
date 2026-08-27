using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class TimeProfile
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public TimeProfileType Type { get; set; }

	public string DefinitionXml { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static TimeProfile QueryTimeProfile(IGeminiRequestContext requestContext, string timeProfileId)
	{
		TimeProfile timeProfile = null;
		Validation.CheckObjectId(ref timeProfileId, "timeProfileId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return null;
		}
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		return timeProfileComponent.QueryTimeProfile(timeProfileId);
	}

	public static string[] QueryTimeProfileIds(IGeminiRequestContext requestContext)
	{
		string[] array = null;
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return new string[0];
		}
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		return timeProfileComponent.QueryTimeProfileIds();
	}

	public static string[] QueryTimeProfileIds(IGeminiRequestContext requestContext, TimeProfileType type)
	{
		string[] array = null;
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		return timeProfileComponent.QueryTimeProfileIds(type);
	}

	public static TimeProfile[] QueryTimeProfiles(IGeminiRequestContext requestContext, string[] ids)
	{
		TimeProfile[] array = null;
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return new TimeProfile[0];
		}
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		return timeProfileComponent.QueryTimeProfilesByIds(ids);
	}

	public static TimeProfile CreateTimeProfile(IGeminiRequestContext requestContext, TimeProfile timeProfile)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		string name = timeProfile.Name;
		Validation.CheckName(name, "name");
		Validation.CheckTimeProfileDefinitionXml(timeProfile.DefinitionXml, timeProfile.Type, "definitionXml");
		timeProfile.Id = GeminiResourceComponent.GetGuid(timeProfile.Id);
		using (ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent())
		{
			timeProfileComponent.CreateTimeProfile(timeProfile);
			timeProfile = timeProfileComponent.QueryTimeProfile(timeProfile.Id);
			Integration.FireConfigurationChanged(requestContext, "TimeProfile", timeProfile.Id, EventAction.Added, name);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditTimeProfileCreated(timeProfile.Id.ToGuid(), timeProfile.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return timeProfile;
	}

	public static TimeProfile UpdateTimeProfile(IGeminiRequestContext requestContext, TimeProfile timeProfile)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		string name = timeProfile.Name;
		Validation.CheckName(name, "name");
		Validation.CheckTimeProfileDefinitionXml(timeProfile.DefinitionXml, timeProfile.Type, "definitionXml");
		using (ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent())
		{
			timeProfileComponent.UpdateTimeProfile(timeProfile, sessionId);
			timeProfile = FindTimeProfile(requestContext, timeProfile.Id);
			Integration.FireConfigurationChanged(requestContext, "TimeProfile", timeProfile.Id, EventAction.Updated, name);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return timeProfile;
	}

	public static void DeleteTimeProfile(IGeminiRequestContext requestContext, string timeProfileId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref timeProfileId, "timeProfileId", allowNullOrEmpty: false);
		TimeProfile timeProfile = null;
		using (ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent())
		{
			timeProfile = timeProfileComponent.QueryTimeProfile(timeProfileId);
			timeProfileComponent.DeleteTimeProfile(timeProfileId);
			Integration.FireConfigurationChanged(requestContext, "TimeProfile", timeProfileId, EventAction.Deleted, string.Empty);
			Integration.TryNotifyServerAsync();
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditTimeProfileDelete(timeProfile.Id.ToGuid(), timeProfile.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static TimeProfile FindTimeProfile(IGeminiRequestContext requestContext, string timeProfileId)
	{
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		return timeProfileComponent.QueryTimeProfile(timeProfileId);
	}
}

using System;

namespace VideoOS.Management.Server;

public sealed class MailNotificationProfile
{
	public DateTime LastModified { get; set; }

	public Guid ProfileId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string SettingsXml { get; set; }

	public static MailNotificationProfile[] QueryProfiles(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMailNotificationProfileComponent mailNotificationProfileComponent = requestContext.CreateMailNotificationProfileComponent();
		return mailNotificationProfileComponent.QueryProfiles();
	}

	public static MailNotificationProfile QueryProfile(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMailNotificationProfileComponent mailNotificationProfileComponent = requestContext.CreateMailNotificationProfileComponent();
		return mailNotificationProfileComponent.QueryProfile(id);
	}

	public static MailNotificationProfile UpdateProfile(IGeminiRequestContext requestContext, Guid id, string name, string description, string settingsXML)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IMailNotificationProfileComponent mailNotificationProfileComponent = requestContext.CreateMailNotificationProfileComponent();
		MailNotificationProfile result = mailNotificationProfileComponent.UpdateProfile(id, name, description, settingsXML);
		Integration.FireConfigurationChanged(requestContext, "MailNotificationProfile", id.ToString(), EventAction.Updated, name);
		return result;
	}

	public static MailNotificationProfile CreateProfile(IGeminiRequestContext requestContext, Guid id, string name, string description, string settingsXML)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IMailNotificationProfileComponent mailNotificationProfileComponent = requestContext.CreateMailNotificationProfileComponent();
		MailNotificationProfile result = mailNotificationProfileComponent.CreateProfile(id, name, description, settingsXML, Guid.NewGuid());
		Integration.FireConfigurationChanged(requestContext, "MailNotificationProfile", id.ToString(), EventAction.Added, name);
		return result;
	}

	public static void DeleteProfile(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IMailNotificationProfileComponent mailNotificationProfileComponent = requestContext.CreateMailNotificationProfileComponent();
		mailNotificationProfileComponent.DeleteProfile(id);
		Integration.FireConfigurationChanged(requestContext, "MailNotificationProfile", id.ToString(), EventAction.Deleted, string.Empty);
	}
}

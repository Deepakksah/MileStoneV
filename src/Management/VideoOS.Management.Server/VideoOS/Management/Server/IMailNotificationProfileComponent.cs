using System;

namespace VideoOS.Management.Server;

public interface IMailNotificationProfileComponent : IDisposable
{
	MailNotificationProfile[] QueryProfiles();

	MailNotificationProfile QueryProfile(Guid id);

	MailNotificationProfile UpdateProfile(Guid id, string name, string description, string settingsXML);

	MailNotificationProfile CreateProfile(Guid id, string name, string description, string settingsXML, Guid settingsId);

	void DeleteProfile(Guid id);
}

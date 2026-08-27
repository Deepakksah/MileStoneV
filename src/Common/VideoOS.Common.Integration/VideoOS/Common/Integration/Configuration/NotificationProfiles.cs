using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationProfiles : CollectionBase
{
	public new NotificationProfile this[Guid id] => base[id] as NotificationProfile;

	public NotificationProfiles(ConfigurationManager manager)
		: base(manager, manager.Dataset.NotificationProfiles)
	{
	}

	public NotificationProfile Add(Guid notificationProfileId, string name, string description)
	{
		return Add(_configurationManager.Dataset.NotificationProfiles.AddNotificationProfilesRow(DateTime.UtcNow, notificationProfileId.ToString(), name, description)) as NotificationProfile;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new NotificationProfile(_configurationManager, row as ConfigurationDataSet.NotificationProfilesRow);
	}

	public override void Remove(Guid IDNotificationProfile)
	{
		foreach (NotificationType value in _configurationManager.NotificationTypes.Values)
		{
			_configurationManager.NotificationSettings.Remove(IDNotificationProfile, value.IDNotificationType);
		}
		base.Remove(IDNotificationProfile);
	}
}

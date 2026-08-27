using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationSettings : CollectionBase
{
	public new NotificationSetting this[Guid id] => base[id] as NotificationSetting;

	public NotificationSetting this[Guid notificationProfile, Guid notificationType]
	{
		get
		{
			foreach (NotificationSetting value in base.Values)
			{
				if (value.IDNotificationProfile == notificationProfile && value.IDNotificationType == notificationType)
				{
					return value;
				}
			}
			return null;
		}
	}

	public NotificationSettings(ConfigurationManager manager)
		: this(manager, Guid.Empty)
	{
	}

	public NotificationSettings(ConfigurationManager manager, Guid notificationProfileId)
		: base(manager, manager.Dataset.NotificationSettings)
	{
		if (!(notificationProfileId != Guid.Empty))
		{
			return;
		}
		foreach (ConfigurationDataSet.NotificationSettingsRow row in _table.Rows)
		{
			if (notificationProfileId != new Guid(row.IDNotificationProfile))
			{
				base.Dictionary.Remove(new Guid(row.IDNotificationSetting));
			}
		}
	}

	public NotificationSetting Add(Guid notificationSettingId, string profileSettings, Guid notificationTypeId, Guid notificationProfileId)
	{
		return Add(_configurationManager.Dataset.NotificationSettings.AddNotificationSettingsRow(DateTime.UtcNow, notificationSettingId.ToString(), profileSettings, notificationTypeId.ToString(), notificationProfileId.ToString())) as NotificationSetting;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new NotificationSetting(_configurationManager, row as ConfigurationDataSet.NotificationSettingsRow);
	}

	public void Remove(Guid notificationProfileId, Guid notificationTypeId)
	{
		foreach (NotificationSetting value in base.Values)
		{
			if (notificationProfileId == value.IDNotificationProfile && notificationTypeId == value.IDNotificationType)
			{
				base.Remove(value.IDNotificationSetting);
				break;
			}
		}
	}
}

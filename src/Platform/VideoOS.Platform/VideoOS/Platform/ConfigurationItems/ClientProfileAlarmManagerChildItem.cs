using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileAlarmManagerChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> AlarmManagerShowDesktopNotificationsValues => base.Wrapper.GetEnumValues("AlarmManagerShowDesktopNotifications");

	public string AlarmManagerShowDesktopNotifications
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AlarmManagerShowDesktopNotifications");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AlarmManagerShowDesktopNotifications", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AlarmManagerShowDesktopNotificationsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AlarmManagerShowDesktopNotificationsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AlarmManagerShowDesktopNotificationsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AlarmManagerPlaySoundNotificationsValues => base.Wrapper.GetEnumValues("AlarmManagerPlaySoundNotifications");

	public string AlarmManagerPlaySoundNotifications
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AlarmManagerPlaySoundNotifications");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AlarmManagerPlaySoundNotifications", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AlarmManagerPlaySoundNotificationsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AlarmManagerPlaySoundNotificationsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AlarmManagerPlaySoundNotificationsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileAlarmManagerChildItem(ServerId serverId, ConfigurationItem clientProfileAlarmManagerChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileAlarmManagerChildItem, parentWrapper)
	{
	}

	public ClientProfileAlarmManagerChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileAlarmManagerChildItem(FQID fqid)
		: base(fqid)
	{
	}
}

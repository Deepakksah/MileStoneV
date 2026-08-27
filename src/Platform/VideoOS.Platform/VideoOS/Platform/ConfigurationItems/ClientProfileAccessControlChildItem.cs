using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileAccessControlChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> ShowAccessRequestNotificationsValues => base.Wrapper.GetEnumValues("ShowAccessRequestNotifications");

	public string ShowAccessRequestNotifications
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ShowAccessRequestNotifications");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ShowAccessRequestNotifications", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ShowAccessRequestNotificationsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ShowAccessRequestNotificationsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ShowAccessRequestNotificationsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileAccessControlChildItem(ServerId serverId, ConfigurationItem clientProfileAccessControlChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileAccessControlChildItem, parentWrapper)
	{
	}

	public ClientProfileAccessControlChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileAccessControlChildItem(FQID fqid)
		: base(fqid)
	{
	}
}

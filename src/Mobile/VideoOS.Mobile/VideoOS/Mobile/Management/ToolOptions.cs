using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

namespace VideoOS.Mobile.Management;

public static class ToolOptions
{
	public static ICollection<IConfigurationItem> GetOptions()
	{
		return new VideoOS.Platform.ConfigurationItems.ManagementServer(EnvironmentManager.Instance.MasterSite).ToolOptionFolder.ToolOptions;
	}

	public static ToolOptionBookmark GetBookmarkOptions()
	{
		return GetById(ToolOptionIds.BookmarkToolOptionId) as ToolOptionBookmark;
	}

	public static ToolOptionServerLog GetServerLogOptions()
	{
		return GetById(ToolOptionIds.ServerLogToolOptionId) as ToolOptionServerLog;
	}

	public static ToolOptionPrivacySetting GetPrivacySettingOptions()
	{
		return GetById(ToolOptionIds.PrivacySettingsToolOptionId) as ToolOptionPrivacySetting;
	}

	private static IConfigurationItem GetById(Guid guid)
	{
		return GetOptions().SingleOrDefault((IConfigurationItem o) => o.Path.IndexOf(guid.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0);
	}
}

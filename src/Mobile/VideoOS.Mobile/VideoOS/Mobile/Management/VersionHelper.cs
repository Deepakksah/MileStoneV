using System.Collections.Generic;
using VideoOS.Platform;

namespace VideoOS.Mobile.Management;

public class VersionHelper
{
	public static readonly Version MissingVersion = new Version(12u, 2u, 0u, 1u);

	public static readonly Version CurrentVersion = new Version("25.3.0.0");

	public static Version GetVersionFromItem(Item configurationItem)
	{
		return GetVersionFromProps(configurationItem.Properties);
	}

	public static Version GetVersionFromProps(Dictionary<string, string> properties)
	{
		if (properties.ContainsKey("Version"))
		{
			return new Version(properties["Version"]);
		}
		return MissingVersion;
	}

	public static void SetVersionToItem(Item configurationItem, Version version)
	{
		configurationItem.Properties["Version"] = version.ToString();
	}

	public static void SetCurrentVersionToItem(Item configurationItem)
	{
		SetVersionToItem(configurationItem, CurrentVersion);
	}
}

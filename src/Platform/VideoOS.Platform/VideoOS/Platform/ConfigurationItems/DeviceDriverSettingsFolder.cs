using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class DeviceDriverSettingsFolder : IConfigurationItem
{
	public ICollection<DeviceDriverSettings> DeviceDriverSettings
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<DeviceDriverSettings> list = new List<DeviceDriverSettings>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "DeviceDriverSettings")
				{
					list.Add(new DeviceDriverSettings(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public DeviceDriverSettingsFolder(ServerId serverId, ConfigurationItem deviceDriverSettingsFolder)
		: base(serverId, deviceDriverSettingsFolder)
	{
	}

	public DeviceDriverSettingsFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public DeviceDriverSettingsFolder(FQID fqid)
		: base(fqid)
	{
	}
}

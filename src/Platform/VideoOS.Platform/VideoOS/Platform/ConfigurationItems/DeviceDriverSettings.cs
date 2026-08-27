using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class DeviceDriverSettings : IConfigurationItem
{
	public DeviceDriverSettingsChildItem DeviceDriverSettingsChildItem
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "DeviceDriverSettings")
				{
					return new DeviceDriverSettingsChildItem(base.ServerId, configurationItem, base.Wrapper);
				}
			}
			return null;
		}
	}

	public PTZChildItem PTZChildItem
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "PTZ")
				{
					return new PTZChildItem(base.ServerId, configurationItem, base.Wrapper);
				}
			}
			return null;
		}
	}

	public PTZSessionTimeoutChildItem PTZSessionTimeoutChildItem
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "PTZSessionTimeout")
				{
					return new PTZSessionTimeoutChildItem(base.ServerId, configurationItem, base.Wrapper);
				}
			}
			return null;
		}
	}

	public ICollection<StreamChildItem> StreamChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<StreamChildItem> list = new List<StreamChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Stream")
				{
					list.Add(new StreamChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public DeviceDriverSettings(ServerId serverId, ConfigurationItem deviceDriverSettings)
		: base(serverId, deviceDriverSettings)
	{
	}

	public DeviceDriverSettings(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public DeviceDriverSettings(FQID fqid)
		: base(fqid)
	{
	}
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwarePtzSettings : IConfigurationItem
{
	public ICollection<HardwarePtzDeviceSettingChildItem> HardwarePtzDeviceSettingChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwarePtzDeviceSettingChildItem> list = new List<HardwarePtzDeviceSettingChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwarePtzDeviceSetting")
				{
					list.Add(new HardwarePtzDeviceSettingChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public HardwarePtzSettings(ServerId serverId, ConfigurationItem hardwarePtzSettings)
		: base(serverId, hardwarePtzSettings)
	{
	}

	public HardwarePtzSettings(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwarePtzSettings(FQID fqid)
		: base(fqid)
	{
	}
}

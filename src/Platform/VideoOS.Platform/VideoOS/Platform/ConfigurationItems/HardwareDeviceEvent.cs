using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDeviceEvent : IConfigurationItem
{
	public ICollection<HardwareDeviceEventChildItem> HardwareDeviceEventChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwareDeviceEventChildItem> list = new List<HardwareDeviceEventChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwareDeviceEvent")
				{
					list.Add(new HardwareDeviceEventChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public HardwareDeviceEvent(ServerId serverId, ConfigurationItem hardwareDeviceEvent)
		: base(serverId, hardwareDeviceEvent)
	{
	}

	public HardwareDeviceEvent(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDeviceEvent(FQID fqid)
		: base(fqid)
	{
	}
}

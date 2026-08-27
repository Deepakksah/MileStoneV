using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDeviceEventFolder : IConfigurationItem
{
	public ICollection<HardwareDeviceEvent> HardwareDeviceEvents
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwareDeviceEvent> list = new List<HardwareDeviceEvent>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwareDeviceEvent")
				{
					list.Add(new HardwareDeviceEvent(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public HardwareDeviceEventFolder(ServerId serverId, ConfigurationItem hardwareDeviceEventFolder)
		: base(serverId, hardwareDeviceEventFolder)
	{
	}

	public HardwareDeviceEventFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDeviceEventFolder(FQID fqid)
		: base(fqid)
	{
	}
}

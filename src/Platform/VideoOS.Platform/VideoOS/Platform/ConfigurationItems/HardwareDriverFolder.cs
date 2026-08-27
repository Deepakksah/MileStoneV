using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDriverFolder : IConfigurationItem
{
	public ICollection<HardwareDriver> HardwareDrivers
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwareDriver> list = new List<HardwareDriver>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwareDriver")
				{
					list.Add(new HardwareDriver(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public HardwareDriverFolder(ServerId serverId, ConfigurationItem hardwareDriverFolder)
		: base(serverId, hardwareDriverFolder)
	{
	}

	public HardwareDriverFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDriverFolder(FQID fqid)
		: base(fqid)
	{
	}
}

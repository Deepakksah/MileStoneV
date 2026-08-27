using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SystemAddressFolder : IConfigurationItem
{
	public ICollection<SystemAddressChildItem> SystemAddressChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<SystemAddressChildItem> list = new List<SystemAddressChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "SystemAddress")
				{
					list.Add(new SystemAddressChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public SystemAddressFolder(ServerId serverId, ConfigurationItem systemAddressFolder)
		: base(serverId, systemAddressFolder)
	{
	}

	public SystemAddressFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SystemAddressFolder(FQID fqid)
		: base(fqid)
	{
	}

	public SystemAddressFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/SystemAddressFolder")
	{
	}
}

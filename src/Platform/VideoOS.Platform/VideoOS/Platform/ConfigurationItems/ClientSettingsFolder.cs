using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientSettingsFolder : IConfigurationItem
{
	public ICollection<ClientSettings> ClientSettings
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientSettings> list = new List<ClientSettings>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientSettings")
				{
					list.Add(new ClientSettings(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ClientSettingsFolder(ServerId serverId, ConfigurationItem clientSettingsFolder)
		: base(serverId, clientSettingsFolder)
	{
	}

	public ClientSettingsFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientSettingsFolder(FQID fqid)
		: base(fqid)
	{
	}
}

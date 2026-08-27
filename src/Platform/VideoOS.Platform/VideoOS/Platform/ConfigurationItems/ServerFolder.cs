using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ServerFolder : IConfigurationItem
{
	public ICollection<Server> Servers
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Server> list = new List<Server>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Server")
				{
					list.Add(new Server(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ServerFolder(ServerId serverId, ConfigurationItem serverFolder)
		: base(serverId, serverFolder)
	{
	}

	public ServerFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ServerFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/ServerFolder")
	{
	}
}

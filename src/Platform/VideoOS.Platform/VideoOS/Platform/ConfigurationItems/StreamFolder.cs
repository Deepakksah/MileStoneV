using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StreamFolder : IConfigurationItem
{
	public ICollection<StreamDefinition> Streams
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<StreamDefinition> list = new List<StreamDefinition>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Stream")
				{
					list.Add(new StreamDefinition(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public StreamFolder(ServerId serverId, ConfigurationItem streamFolder)
		: base(serverId, streamFolder)
	{
	}

	public StreamFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StreamFolder(FQID fqid)
		: base(fqid)
	{
	}
}

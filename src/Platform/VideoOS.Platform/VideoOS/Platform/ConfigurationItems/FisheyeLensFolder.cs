using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FisheyeLensFolder : IConfigurationItem
{
	public ICollection<FisheyeLens> FisheyeLens
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<FisheyeLens> list = new List<FisheyeLens>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "FisheyeLens")
				{
					list.Add(new FisheyeLens(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public FisheyeLensFolder(ServerId serverId, ConfigurationItem fisheyeLensFolder)
		: base(serverId, fisheyeLensFolder)
	{
	}

	public FisheyeLensFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FisheyeLensFolder(FQID fqid)
		: base(fqid)
	{
	}
}

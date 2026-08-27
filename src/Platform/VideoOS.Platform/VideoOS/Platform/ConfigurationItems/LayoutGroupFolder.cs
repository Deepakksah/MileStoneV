using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LayoutGroupFolder : IConfigurationItem
{
	public ICollection<LayoutGroup> LayoutGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LayoutGroup> list = new List<LayoutGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LayoutGroup")
				{
					list.Add(new LayoutGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public LayoutGroupFolder(ServerId serverId, ConfigurationItem layoutGroupFolder)
		: base(serverId, layoutGroupFolder)
	{
	}

	public LayoutGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LayoutGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public LayoutGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/LayoutGroupFolder")
	{
	}
}

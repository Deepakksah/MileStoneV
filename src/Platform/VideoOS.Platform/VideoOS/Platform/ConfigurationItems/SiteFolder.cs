using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SiteFolder : IConfigurationItem
{
	public ICollection<Site> Sites
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Site> list = new List<Site>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Site")
				{
					list.Add(new Site(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public SiteFolder(ServerId serverId, ConfigurationItem siteFolder)
		: base(serverId, siteFolder)
	{
	}

	public SiteFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SiteFolder(FQID fqid)
		: base(fqid)
	{
	}

	public SiteFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/SiteFolder")
	{
	}
}

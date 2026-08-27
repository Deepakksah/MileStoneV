using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SiteAddressFolder : IConfigurationItem
{
	public ICollection<SiteAddressChildItem> SiteAddressChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<SiteAddressChildItem> list = new List<SiteAddressChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "SiteAddress")
				{
					list.Add(new SiteAddressChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public SiteAddressFolder(ServerId serverId, ConfigurationItem siteAddressFolder)
		: base(serverId, siteAddressFolder)
	{
	}

	public SiteAddressFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SiteAddressFolder(FQID fqid)
		: base(fqid)
	{
	}
}

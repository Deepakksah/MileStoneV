using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseOverviewAllFolder : IConfigurationItem
{
	public ICollection<LicenseOverviewAllChildItem> LicenseOverviewAllChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LicenseOverviewAllChildItem> list = new List<LicenseOverviewAllChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LicenseOverviewAll")
				{
					list.Add(new LicenseOverviewAllChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public LicenseOverviewAllFolder(ServerId serverId, ConfigurationItem licenseOverviewAllFolder)
		: base(serverId, licenseOverviewAllFolder)
	{
	}

	public LicenseOverviewAllFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseOverviewAllFolder(FQID fqid)
		: base(fqid)
	{
	}
}

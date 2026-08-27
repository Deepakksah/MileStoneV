using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseInstalledProductFolder : IConfigurationItem
{
	public ICollection<LicenseInstalledProductChildItem> LicenseInstalledProductChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LicenseInstalledProductChildItem> list = new List<LicenseInstalledProductChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LicenseInstalledProduct")
				{
					list.Add(new LicenseInstalledProductChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public LicenseInstalledProductFolder(ServerId serverId, ConfigurationItem licenseInstalledProductFolder)
		: base(serverId, licenseInstalledProductFolder)
	{
	}

	public LicenseInstalledProductFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseInstalledProductFolder(FQID fqid)
		: base(fqid)
	{
	}
}

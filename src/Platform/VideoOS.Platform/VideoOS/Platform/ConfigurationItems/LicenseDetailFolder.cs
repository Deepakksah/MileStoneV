using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseDetailFolder : IConfigurationItem
{
	public ICollection<LicenseDetailChildItem> LicenseDetailChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LicenseDetailChildItem> list = new List<LicenseDetailChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LicenseDetail")
				{
					list.Add(new LicenseDetailChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public LicenseDetailFolder(ServerId serverId, ConfigurationItem licenseDetailFolder)
		: base(serverId, licenseDetailFolder)
	{
	}

	public LicenseDetailFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseDetailFolder(FQID fqid)
		: base(fqid)
	{
	}
}

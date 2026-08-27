using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseInformationFolder : IConfigurationItem
{
	public ICollection<LicenseInformation> LicenseInformations
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LicenseInformation> list = new List<LicenseInformation>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LicenseInformation")
				{
					list.Add(new LicenseInformation(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public LicenseInformationFolder(ServerId serverId, ConfigurationItem licenseInformationFolder)
		: base(serverId, licenseInformationFolder)
	{
	}

	public LicenseInformationFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseInformationFolder(FQID fqid)
		: base(fqid)
	{
	}

	public LicenseInformationFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/LicenseInformationFolder")
	{
	}
}

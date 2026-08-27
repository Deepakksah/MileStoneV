using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AutoDetectConfigurationFolder : IConfigurationItem
{
	public ICollection<AutoDetectConfiguration> AutoDetectConfigurations
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AutoDetectConfiguration> list = new List<AutoDetectConfiguration>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AutoDetectConfiguration")
				{
					list.Add(new AutoDetectConfiguration(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public AutoDetectConfigurationFolder(ServerId serverId, ConfigurationItem autoDetectConfigurationFolder)
		: base(serverId, autoDetectConfigurationFolder)
	{
	}

	public AutoDetectConfigurationFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AutoDetectConfigurationFolder(FQID fqid)
		: base(fqid)
	{
	}

	public AutoDetectConfigurationFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/AutoDetectConfigurationFolder")
	{
	}
}

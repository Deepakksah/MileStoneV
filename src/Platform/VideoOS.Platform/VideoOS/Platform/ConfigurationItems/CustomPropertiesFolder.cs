using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CustomPropertiesFolder : IConfigurationItem
{
	public CustomProperties CustomProperties
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CustomProperties")
				{
					return new CustomProperties(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CustomPropertiesFolder(ServerId serverId, ConfigurationItem customPropertiesFolder)
		: base(serverId, customPropertiesFolder)
	{
	}

	public CustomPropertiesFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CustomPropertiesFolder(FQID fqid)
		: base(fqid)
	{
	}
}

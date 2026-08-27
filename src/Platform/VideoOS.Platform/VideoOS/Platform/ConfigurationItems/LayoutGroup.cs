using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LayoutGroup : IConfigurationItem
{
	public LayoutFolder LayoutFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LayoutFolder")
				{
					return new LayoutFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public LayoutGroup(ServerId serverId, ConfigurationItem layoutGroup)
		: base(serverId, layoutGroup)
	{
	}

	public LayoutGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LayoutGroup(FQID fqid)
		: base(fqid)
	{
	}
}

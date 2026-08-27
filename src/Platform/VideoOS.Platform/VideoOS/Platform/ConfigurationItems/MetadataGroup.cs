using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MetadataGroup : IConfigurationItem
{
	public MetadataFolder MetadataFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MetadataFolder")
				{
					return new MetadataFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MetadataGroupFolder MetadataGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MetadataGroupFolder")
				{
					return new MetadataGroupFolder(base.ServerId, configurationItem);
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

	public bool BuiltIn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BuiltIn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public MetadataGroup(ServerId serverId, ConfigurationItem metadataGroup)
		: base(serverId, metadataGroup)
	{
	}

	public MetadataGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MetadataGroup(FQID fqid)
		: base(fqid)
	{
	}
}

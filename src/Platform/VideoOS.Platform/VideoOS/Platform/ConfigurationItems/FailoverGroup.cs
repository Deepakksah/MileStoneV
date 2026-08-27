using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FailoverGroup : IConfigurationItem
{
	public FailoverRecorderFolder FailoverRecorderFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "FailoverRecorderFolder")
				{
					return new FailoverRecorderFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

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

	public FailoverGroup(ServerId serverId, ConfigurationItem failoverGroup)
		: base(serverId, failoverGroup)
	{
	}

	public FailoverGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FailoverGroup(FQID fqid)
		: base(fqid)
	{
	}
}

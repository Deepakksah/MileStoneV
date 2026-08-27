using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EventTypeGroup : IConfigurationItem
{
	public AnalyticsEventFolder AnalyticsEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "AnalyticsEventFolder")
				{
					return new AnalyticsEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public EventTypeFolder EventTypeFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "EventTypeFolder")
				{
					return new EventTypeFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public GenericEventFolder GenericEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "GenericEventFolder")
				{
					return new GenericEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public UserDefinedEventFolder UserDefinedEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "UserDefinedEventFolder")
				{
					return new UserDefinedEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public EventTypeGroup(ServerId serverId, ConfigurationItem eventTypeGroup)
		: base(serverId, eventTypeGroup)
	{
	}

	public EventTypeGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EventTypeGroup(FQID fqid)
		: base(fqid)
	{
	}
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EventTypeFolder : IConfigurationItem
{
	public ICollection<EventType> EventTypes
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<EventType> list = new List<EventType>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "EventType")
				{
					list.Add(new EventType(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public EventTypeFolder(ServerId serverId, ConfigurationItem eventTypeFolder)
		: base(serverId, eventTypeFolder)
	{
	}

	public EventTypeFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EventTypeFolder(FQID fqid)
		: base(fqid)
	{
	}
}

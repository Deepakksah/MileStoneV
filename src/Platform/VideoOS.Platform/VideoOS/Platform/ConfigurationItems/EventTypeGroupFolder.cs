using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EventTypeGroupFolder : IConfigurationItem
{
	public ICollection<EventTypeGroup> EventTypeGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<EventTypeGroup> list = new List<EventTypeGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "EventTypeGroup")
				{
					list.Add(new EventTypeGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public EventTypeGroupFolder(ServerId serverId, ConfigurationItem eventTypeGroupFolder)
		: base(serverId, eventTypeGroupFolder)
	{
	}

	public EventTypeGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EventTypeGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public EventTypeGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/EventTypeGroupFolder")
	{
	}
}

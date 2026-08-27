using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public static class SerializableServerEndpoint
{
	public const string ConnectionString = "ConnectionString";

	public const string EndpointType = "EndpointType";

	public static List<SerializableItem> GetItems(IEnumerable<ServerEndpoint> connectionMethods)
	{
		List<SerializableItem> list = null;
		if (connectionMethods != null && connectionMethods.Any())
		{
			list = new List<SerializableItem>();
			SerializableItem serializableItem = new SerializableItem
			{
				IsFolder = true
			};
			serializableItem.SetItemType(SerializableItemTypes.Endpoint);
			serializableItem.Children = connectionMethods.Select(GetItem).ToList();
			list.Add(serializableItem);
		}
		return list;
	}

	private static SerializableItem GetItem(ServerEndpoint entry)
	{
		SerializableItem serializableItem = new SerializableItem();
		serializableItem.SetItemType(SerializableItemTypes.Endpoint);
		serializableItem.Properties.Add("ConnectionString", entry.ConnectionString);
		serializableItem.Properties.Add("EndpointType", EnumsHelper.GetDescription(entry.Type));
		return serializableItem;
	}

	public static List<ServerEndpoint> GetServerEndPoints(List<SerializableItem> items)
	{
		List<ServerEndpoint> result = null;
		SerializableItem serializableItem = items?.FirstOrDefault((SerializableItem item) => item.IsFolder && item.GetItemType() == SerializableItemTypes.Endpoint && item.Children != null && item.Children.Count > 0);
		if (serializableItem != null)
		{
			result = (from item in serializableItem.Children
				where item.GetItemType() == SerializableItemTypes.Endpoint
				select GetServerEndPoint(item)).ToList();
		}
		return result;
	}

	private static ServerEndpoint GetServerEndPoint(SerializableItem serializableItem)
	{
		string connectionString = (serializableItem.Properties.ContainsKey("ConnectionString") ? serializableItem.Properties["ConnectionString"] : string.Empty);
		ServerEndpointType type = (serializableItem.Properties.ContainsKey("EndpointType") ? EnumsHelper.EnumConverter<ServerEndpointType>(serializableItem.Properties["EndpointType"]) : ServerEndpointType.AutomaticUrl);
		return new ServerEndpoint
		{
			ConnectionString = connectionString,
			Type = type
		};
	}
}

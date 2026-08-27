using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public static class SerializableBookmark
{
	public static List<SerializableItem> GetItems(IEnumerable<IBookmarkEntry> bookmarks)
	{
		return bookmarks.Where(delegate(IBookmarkEntry b)
		{
			(Guid, string, SerializableItemTypes) relatedDevice = b.RelatedDevice;
			return relatedDevice.Item1 != default(Guid) || relatedDevice.Item2 != null || relatedDevice.Item3 != SerializableItemTypes.Unknown;
		}).Select(GetItem).ToList();
	}

	public static SerializableItem GetItem(IBookmarkEntry bookmark)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = bookmark.Id,
			Name = bookmark.Name
		};
		serializableItem.SetItemType(SerializableItemTypes.Bookmark);
		serializableItem.Properties.Add("Description", bookmark.Description);
		serializableItem.Properties.Add("Reference", bookmark.Reference);
		serializableItem.Properties.Add("StartTime", bookmark.TimeBegin.ToMilisecString());
		serializableItem.Properties.Add("EndTime", bookmark.TimeEnd.ToMilisecString());
		serializableItem.Properties.Add("Time", bookmark.TimeTrigged.ToMilisecString());
		serializableItem.Properties.Add("Username", bookmark.User);
		(Guid, string, SerializableItemTypes) relatedDevice = bookmark.RelatedDevice;
		if (relatedDevice.Item1 != default(Guid) || relatedDevice.Item2 != null || relatedDevice.Item3 != SerializableItemTypes.Unknown)
		{
			SerializableItem serializableItem2 = new SerializableItem
			{
				Id = bookmark.RelatedDevice.Id,
				Name = bookmark.RelatedDevice.Name
			};
			serializableItem2.SetItemType(bookmark.RelatedDevice.Type);
			serializableItem.Children.Add(serializableItem2);
		}
		return serializableItem;
	}

	public static IBookmarkEntry GetBookmark(SerializableItem item)
	{
		IBookmarkEntry bookmarkEntry = null;
		if (item != null && SerializableItemTypes.Bookmark == item.GetItemType())
		{
			bookmarkEntry = new BookmarkEntry
			{
				Id = item.Id,
				Name = item.Name
			};
			SerializableProperties<string, string, StringStringFactory> properties = item.Properties;
			if (properties.ContainsKey("Description"))
			{
				bookmarkEntry.Description = properties["Description"];
			}
			if (properties.ContainsKey("Reference"))
			{
				bookmarkEntry.Reference = properties["Reference"];
			}
			if (properties.ContainsKey("StartTime"))
			{
				bookmarkEntry.TimeBegin = TimeConverter.FromString(properties["StartTime"]);
			}
			if (properties.ContainsKey("EndTime"))
			{
				bookmarkEntry.TimeEnd = TimeConverter.FromString(properties["EndTime"]);
			}
			if (properties.ContainsKey("Time"))
			{
				bookmarkEntry.TimeTrigged = TimeConverter.FromString(properties["Time"]);
			}
			if (properties.ContainsKey("Username"))
			{
				bookmarkEntry.User = properties["Username"];
			}
			if (item.Children.Count > 0)
			{
				SerializableItem serializableItem = item.Children.Find((SerializableItem x) => !string.IsNullOrEmpty(x.Name));
				bookmarkEntry.RelatedDevice = ((serializableItem != null) ? (Id: serializableItem.Id, Name: serializableItem.Name, Type: item.GetItemType()) : default((Guid, string, SerializableItemTypes)));
			}
		}
		return bookmarkEntry;
	}

	public static IEnumerable<IBookmarkEntry> GetBookmarks(IEnumerable<SerializableItem> bookmarks)
	{
		return bookmarks?.Select(GetBookmark).ToList();
	}
}

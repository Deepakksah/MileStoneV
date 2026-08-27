using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Platform.Util;

public class KindConfiguration
{
	private Dictionary<Guid, Item> _items = new Dictionary<Guid, Item>();

	private HashSet<Guid> _parentObjects = new HashSet<Guid>();

	public Guid Kind;

	public DateTime LastModified = DateTime.MinValue;

	public bool ContainsAllObjects;

	public DateTime LastAccessed = DateTime.MinValue;

	public List<Item> GetItems()
	{
		if (ContainsAllObjects)
		{
			LastAccessed = DateTime.UtcNow;
			return _items.Values.ToList();
		}
		return null;
	}

	public Item GetItem(Guid objectId)
	{
		if (_items.ContainsKey(objectId))
		{
			LastAccessed = DateTime.UtcNow;
			return _items[objectId];
		}
		return null;
	}

	public void StoreItem(Item item)
	{
		LastAccessed = DateTime.UtcNow;
		if (item.FQID.ObjectId != Guid.Empty)
		{
			_items[item.FQID.ObjectId] = CloneItem(item);
		}
		UpdateLastModified(item);
	}

	private static Item CloneItem(Item item)
	{
		Item item2 = new Item(item.FQID.Clone(), item.Name);
		foreach (KeyValuePair<string, string> property in item.Properties)
		{
			item2.Properties[property.Key] = property.Value;
		}
		return item2;
	}

	public void StoreItems(Guid parentId, List<Item> items, bool containsAllObjects)
	{
		LastAccessed = DateTime.UtcNow;
		foreach (Item item in items)
		{
			StoreItem(item);
		}
		if (containsAllObjects)
		{
			ContainsAllObjects = true;
		}
		else if (parentId != Guid.Empty)
		{
			_parentObjects.Add(parentId);
		}
	}

	public List<Item> GetItems(Guid parentId)
	{
		if (ContainsAllObjects || _parentObjects.Contains(parentId))
		{
			LastAccessed = DateTime.UtcNow;
			List<Item> list = new List<Item>();
			{
				foreach (Item value in _items.Values)
				{
					if (value.FQID.ParentId == parentId)
					{
						list.Add(value);
					}
				}
				return list;
			}
		}
		return null;
	}

	private void UpdateLastModified(Item item)
	{
		if (item.Properties.TryGetValue("LastModified", out var value) && DateTime.TryParse(value, out var result) && result > LastModified)
		{
			LastModified = result;
		}
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Cache for: {0} - Item count={1}", Kind, _items.Count);
	}
}

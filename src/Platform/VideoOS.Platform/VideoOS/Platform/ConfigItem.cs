using System;
using System.Collections.Generic;

namespace VideoOS.Platform;

public class ConfigItem : Item
{
	public ConfigItem()
	{
	}

	public ConfigItem(FQID fqid, string name)
		: base(fqid, name)
	{
	}

	public ConfigItem(ServerId serverId, string name)
		: base(serverId, name)
	{
	}

	public ConfigItem(ServerId serverId, string name, FolderType folderType, Guid kind)
		: base(serverId, name, folderType, kind)
	{
	}

	public override List<Item> GetChildren()
	{
		return _children;
	}

	public virtual ConfigItem AddChild(ConfigItem item)
	{
		_children.Add(item);
		_isChildrenUsed = true;
		return item;
	}

	public virtual void AddRangeChild(ICollection<ConfigItem> items)
	{
		foreach (ConfigItem item in items)
		{
			_children.Add(item);
		}
		_isChildrenUsed = true;
	}

	public virtual Item AddChild(Item item)
	{
		_children.Add(item);
		_isChildrenUsed = true;
		return item;
	}

	public virtual void AddRangeChild(ICollection<Item> items)
	{
		_children.AddRange(items);
		_isChildrenUsed = true;
	}

	public virtual ConfigItem AddChild(FQID fqid, string name)
	{
		ConfigItem configItem = new ConfigItem(fqid, name);
		_children.Add(configItem);
		_isChildrenUsed = true;
		return configItem;
	}

	public virtual ConfigItem AddChild(string name, Guid kind, FolderType folderType)
	{
		return AddChild(new FQID(FQID.ServerId, folderType, kind), name);
	}

	public virtual void RemoveChild(Item childItem)
	{
		if (_children.Contains(childItem))
		{
			_children.Remove(childItem);
		}
	}

	public virtual void PropertiesModified()
	{
	}
}

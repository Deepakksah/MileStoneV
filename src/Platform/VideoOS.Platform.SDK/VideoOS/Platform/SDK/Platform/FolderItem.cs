using System;
using System.Collections.Generic;

namespace VideoOS.Platform.SDK.Platform;

public class FolderItem : Item
{
	public FolderItem(ServerId serverId, Guid id, string name, Guid itemKind, List<Item> children)
		: base(new FQID(serverId, serverId.Id, id, FolderType.UserDefined, itemKind), name)
	{
		_children = children;
	}

	public FolderItem(ServerId serverId, Guid id, string name, FolderType folderType, Guid itemKind, List<Item> children)
		: base(new FQID(serverId, serverId.Id, id, folderType, itemKind), name)
	{
		_children = children;
	}

	public override Item GetParent()
	{
		foreach (Item item in ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).GetItemsByKind(Kind.Server, ItemHierarchy.UserDefined))
		{
			if (item.FQID.ServerId.Id == base.FQID.ServerId.Id)
			{
				return item;
			}
		}
		return null;
	}

	public override List<Item> GetChildren()
	{
		if (_children == null)
		{
			return new List<Item>();
		}
		return _children;
	}
}

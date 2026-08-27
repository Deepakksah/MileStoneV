using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.OEGroupItem;

public class OEGroupTree : IOEGroupItem, IOEGroupTree
{
	private List<IOEGroupItem> _list;

	public OEItemType ItemType { get; private set; }

	public string ItemName { get; private set; }

	public Guid ObjectId { get; private set; }

	public Guid CameraId { get; private set; }

	public Guid ServerId { get; private set; }

	public bool CanActivate { get; private set; }

	public OEGroupTree()
	{
		InitInternal(OEItemType.Root, "root", Guid.Empty, Guid.Empty, Guid.Empty, canActivate: false);
	}

	public OEGroupTree(OEItemType type, string name, Guid objId, Guid camID, Guid srvId, bool canActivate)
	{
		InitInternal(type, name, objId, camID, srvId, canActivate);
	}

	private void InitInternal(OEItemType type, string name, Guid objId, Guid camID, Guid srvId, bool canActivate)
	{
		_list = new List<IOEGroupItem>();
		ItemType = type;
		ItemName = name;
		ObjectId = objId;
		CameraId = camID;
		ServerId = srvId;
		CanActivate = canActivate;
	}

	public int GetMembersCount()
	{
		return _list.Count;
	}

	public List<IOEGroupItem> GetMembersList()
	{
		return new List<IOEGroupItem>(_list);
	}

	public void AddSubItem(IOEGroupItem subTree)
	{
		_list.Add(subTree);
	}

	public void ClearSubItems()
	{
		_list.Clear();
	}

	public void SortItems()
	{
		_list = _list.OrderBy((IOEGroupItem x) => ((IOEGroupTree)x).ItemName).ToList();
	}

	public bool Contains(Guid objectId)
	{
		foreach (IOEGroupItem item in _list)
		{
			if (item is OEGroupTree oEGroupTree && oEGroupTree.ObjectId == objectId)
			{
				return true;
			}
		}
		return false;
	}
}

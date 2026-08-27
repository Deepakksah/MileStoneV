using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

public class ViewGroupTree : IViewGroupTree, IViewGroupItem
{
	public delegate bool CheckRemoveItem(IViewGroupItem tree);

	public static readonly Guid PublicViewsGuid = Guid.Empty;

	public static readonly Guid PrivateViewsGuid = new Guid("33E315DA-12B0-487C-988F-704FC9E09C70");

	public static readonly Guid AllCamerasListGuid = new Guid("BB16CC8F-A2C5-44F8-9D20-6E9AC57806F5");

	public static readonly Guid AllCamerasFolderGuid = new Guid("1926418B-893E-4AD6-A258-FB4A9AB57453");

	public static readonly Guid AllViewsAndCamerasGuid = new Guid("1b1496cb-1cc1-4486-961a-cc9d78542cfd");

	public static readonly string PublicViewsName = "Public Views";

	public static readonly string PrivateViewsName = "Private Views";

	public static readonly string AllCamerasViewName = "All Cameras";

	private List<IViewGroupTree> _children;

	private readonly CameraComparer _cameraComparer = new CameraComparer();

	public ViewItemType ItemType { get; private set; }

	public string ItemName { get; private set; }

	public Guid ItemId { get; private set; }

	public Guid CameraId { get; private set; }

	public int CameraTimeout { get; private set; }

	public bool IsCamera => ViewItemType.Camera == ItemType;

	public Dictionary<string, string> Properties { get; private set; }

	public ViewGroupTree()
	{
		InitInternal(ViewItemType.Group, "root", Guid.Empty, Guid.Empty, 0, null);
	}

	public ViewGroupTree(ViewItemType type, string name, Guid id, Guid camID, int cameraTimeout = 0, Dictionary<string, string> properties = null)
	{
		InitInternal(type, name, id, camID, cameraTimeout, properties);
	}

	private void InitInternal(ViewItemType type, string name, Guid id, Guid camID, int cameraTimeout, Dictionary<string, string> properties)
	{
		_children = new List<IViewGroupTree>();
		ItemType = type;
		ItemName = name;
		ItemId = id;
		CameraId = camID;
		CameraTimeout = cameraTimeout;
		Properties = properties ?? new Dictionary<string, string>();
	}

	public List<IViewGroupTree> GetMembersList()
	{
		return _children.ToList();
	}

	public IEnumerable<IViewGroupTree> Descendants()
	{
		return GetMembersList().SelectMany((IViewGroupTree child) => child.DescendantsAndSelf());
	}

	public IEnumerable<IViewGroupTree> Descendants(ViewItemType itemType)
	{
		return from child in Descendants()
			where child.ItemType == itemType
			select child;
	}

	public IEnumerable<IViewGroupTree> DescendantsAndSelf()
	{
		return Enumerable.Repeat(this, 1).Concat(Descendants());
	}

	public void AddSubItem(IViewGroupTree subTree)
	{
		_children.Add(subTree);
	}

	public void ClearSubItems()
	{
		_children.Clear();
	}

	public bool ItemExist(ViewGroupTree searchView, IComparer<ViewGroupTree> iComparer)
	{
		if (iComparer.Compare(this, searchView) == 0)
		{
			return true;
		}
		return _children.Any((IViewGroupTree el) => ((ViewGroupTree)el).ItemExist(searchView, iComparer));
	}

	public ViewGroupTree GetAllCamerasList()
	{
		HashSet<Guid> seenCameras = new HashSet<Guid>();
		ViewGroupTree viewGroupTree = new ViewGroupTree(ViewItemType.View, AllCamerasViewName, AllCamerasListGuid, Guid.Empty);
		ToCamerasList(viewGroupTree, seenCameras);
		viewGroupTree._children.Sort(CompareCameras);
		return viewGroupTree;
	}

	public ViewGroupTree GetAllCamerasFolder()
	{
		return new ViewGroupTree(ViewItemType.Group, AllCamerasViewName, AllCamerasFolderGuid, Guid.Empty)
		{
			_children = { (IViewGroupTree)GetAllCamerasList() }
		};
	}

	private void ToCamerasList(ViewGroupTree newViewGroup, HashSet<Guid> seenCameras)
	{
		if (IsCamera && !seenCameras.Contains(CameraId))
		{
			newViewGroup._children.Add(new ViewGroupTree(ItemType, ItemName, ItemId, CameraId, 0, Properties));
			seenCameras.Add(CameraId);
		}
		foreach (ViewGroupTree child in _children)
		{
			child.ToCamerasList(newViewGroup, seenCameras);
		}
	}

	private static int CompareCameras(IViewGroupTree x, IViewGroupTree y)
	{
		return ((ViewGroupTree)x).ItemName.CompareTo(((ViewGroupTree)y).ItemName);
	}

	public ViewGroupTree GetItem(Guid reqItemId)
	{
		if (ItemId == reqItemId)
		{
			return this;
		}
		foreach (ViewGroupTree child in _children)
		{
			ViewGroupTree item = child.GetItem(reqItemId);
			if (item != null)
			{
				return item;
			}
		}
		return null;
	}

	public ViewGroupTree RemoveItems(CheckRemoveItem checker)
	{
		List<IViewGroupTree> list = new List<IViewGroupTree>();
		foreach (IViewGroupTree child in _children)
		{
			try
			{
				if (child is ViewGroupTree viewGroupTree)
				{
					viewGroupTree.RemoveItems(checker);
					viewGroupTree.ItemName = viewGroupTree.ItemName.SanitizedCustom("<>'");
					if (checker(viewGroupTree))
					{
						list.Add(child);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "ViewGroupTree.RemoveItems", ex.Message);
			}
		}
		foreach (IViewGroupTree item in list)
		{
			_children.Remove(item);
		}
		return this;
	}

	public void SortChilds(Comparison<IViewGroupTree> comparison, Func<IViewGroupTree, bool> sortChildsCondition)
	{
		if (!sortChildsCondition(this))
		{
			return;
		}
		_children.Sort(comparison);
		foreach (IViewGroupTree child in _children)
		{
			(child as ViewGroupTree)?.SortChilds(comparison, sortChildsCondition);
		}
	}

	public void TraverseDfs(Action<ViewGroupTree> onNode)
	{
		foreach (ViewGroupTree members in GetMembersList())
		{
			onNode(members);
			members.TraverseDfs(onNode);
		}
	}

	public override string ToString()
	{
		if (!string.IsNullOrWhiteSpace(ItemName))
		{
			return ItemName;
		}
		if (Guid.Empty != ItemId)
		{
			return ItemId.ToString();
		}
		return CameraId.ToString();
	}
}

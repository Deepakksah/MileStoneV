using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableViewGroupTree
{
	public static List<SerializableItem> GetItems(IEnumerable<ViewGroupTree> trees, IThumbnailManager manager)
	{
		return trees.Select((ViewGroupTree item) => GetItem(item, manager)).ToList();
	}

	public static bool ShouldAddProperties(IViewGroupItem tree)
	{
		if (tree.ItemType != ViewItemType.Camera && tree.ItemType != ViewItemType.Microphone)
		{
			return tree.ItemType == ViewItemType.Speaker;
		}
		return true;
	}

	public static SerializableItem GetItem(ViewGroupTree tree, IThumbnailManager manager)
	{
		SerializableItem serializableItem = new SerializableItem
		{
			Id = ((tree.CameraId != Guid.Empty) ? tree.CameraId : tree.ItemId),
			Name = tree.ItemName
		};
		if (ShouldAddProperties(tree))
		{
			serializableItem.Properties.AddRange(tree.Properties);
		}
		serializableItem.SetItemType(SerializableItemTypesHelper.ViewItemTypeToSerializableItemType.GetValueByKey(tree.ItemType));
		foreach (ViewGroupTree members in tree.GetMembersList())
		{
			serializableItem.Children.Add(GetItem(members, manager));
		}
		return serializableItem;
	}

	public static ViewGroupTree GetViewGroupTree(SerializableItem item)
	{
		ViewGroupTree viewGroupTree = null;
		if (item != null)
		{
			Guid camID = ((item.Type == ViewItemType.Camera.ToString()) ? item.Id : Guid.Empty);
			viewGroupTree = new ViewGroupTree(SerializableItemTypesHelper.SerializableItemTypeToViewItemType.GetValueByKey(item.GetItemType()), item.Name, item.Id, camID, 0, item.Properties);
			foreach (SerializableItem child in item.Children)
			{
				viewGroupTree.AddSubItem(GetViewGroupTree(child));
			}
		}
		return viewGroupTree;
	}
}

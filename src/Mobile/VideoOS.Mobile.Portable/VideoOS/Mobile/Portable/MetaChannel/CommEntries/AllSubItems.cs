using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class AllSubItems
{
	public const string AllSubItemsArray = "AllSubItems";

	public SerializableItem AllItems;

	public AllSubItems()
	{
		Initialize();
	}

	public AllSubItems(ViewGroupTree root, IThumbnailManager iThumbsManager)
	{
		Initialize();
		if (root == null)
		{
			return;
		}
		AllItems.Id = root.ItemId;
		AllItems.Name = root.ItemName;
		AllItems.SetItemType(SerializableItemTypes.Folder);
		foreach (IViewGroupTree members in root.GetMembersList())
		{
			AllItems.Children.Add(SerializableViewGroupTree.GetItem((ViewGroupTree)members, iThumbsManager));
		}
	}

	private void Initialize()
	{
		AllItems = new SerializableItem();
	}
}

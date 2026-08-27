using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SubViews
{
	public const string SubItemsArray = "SubItems";

	public const string ItemsNode = "Item";

	[JsonProperty("Item")]
	[XmlElement(ElementName = "Item")]
	public List<ViewItem> ViewItems;

	public SubViews()
	{
		Initialize();
	}

	public SubViews(IViewGroupTree root, IThumbnailManager iThumbsManager)
	{
		Initialize();
		if (root == null)
		{
			return;
		}
		foreach (IViewGroupTree members in root.GetMembersList())
		{
			ViewItems.Add(new ViewItem(members, iThumbsManager));
		}
	}

	private void Initialize()
	{
		ViewItems = new List<ViewItem>();
	}
}

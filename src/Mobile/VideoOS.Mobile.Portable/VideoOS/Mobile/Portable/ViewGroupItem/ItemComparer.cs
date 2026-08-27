using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

public class ItemComparer : IComparer<ViewGroupTree>
{
	int IComparer<ViewGroupTree>.Compare(ViewGroupTree x, ViewGroupTree y)
	{
		return x.ItemId.CompareTo(y.ItemId);
	}
}

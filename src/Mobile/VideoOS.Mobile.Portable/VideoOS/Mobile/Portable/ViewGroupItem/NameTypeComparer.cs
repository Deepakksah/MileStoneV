using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

[ExcludeFromCodeCoverage]
public class NameTypeComparer : IComparer<ViewGroupTree>
{
	int IComparer<ViewGroupTree>.Compare(ViewGroupTree x, ViewGroupTree y)
	{
		int num = x.ItemType - y.ItemType;
		if (num == 0)
		{
			num = string.Compare(x.ItemName, y.ItemName, StringComparison.OrdinalIgnoreCase);
		}
		return num;
	}

	public int Compare(IViewGroupTree x, IViewGroupTree y)
	{
		return ((IComparer<ViewGroupTree>)this).Compare(x as ViewGroupTree, y as ViewGroupTree);
	}
}

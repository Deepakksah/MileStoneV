using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

public interface IViewGroupTree : IViewGroupItem
{
	List<IViewGroupTree> GetMembersList();

	IEnumerable<IViewGroupTree> Descendants();

	IEnumerable<IViewGroupTree> Descendants(ViewItemType itemType);

	IEnumerable<IViewGroupTree> DescendantsAndSelf();

	void AddSubItem(IViewGroupTree subTree);
}

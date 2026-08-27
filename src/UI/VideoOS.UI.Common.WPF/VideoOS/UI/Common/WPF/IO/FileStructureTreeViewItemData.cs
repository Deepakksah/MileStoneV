using VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

namespace VideoOS.UI.Common.WPF.IO;

public class FileStructureTreeViewItemData : TreeViewItemData
{
	public FileStructureTreeViewItemData(object identifier, object header, bool hasPossibleSubItems, TreeViewItemData parent)
		: base(identifier, header, hasPossibleSubItems, parent)
	{
	}
}

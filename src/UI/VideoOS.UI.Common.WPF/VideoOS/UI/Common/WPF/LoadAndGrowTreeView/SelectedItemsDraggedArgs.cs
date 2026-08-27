using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class SelectedItemsDraggedArgs : EventArgs
{
	private List<TreeViewItemData> _draggedData;

	public List<TreeViewItemData> DraggedData
	{
		get
		{
			return _draggedData;
		}
		set
		{
			_draggedData = value;
		}
	}

	public SelectedItemsDraggedArgs(List<TreeViewItemData> draggedData)
	{
		_draggedData = draggedData;
	}
}

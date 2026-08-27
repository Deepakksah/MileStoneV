using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class ItemHasBeenExpandedASynchronousArgs : EventArgs
{
	public delegate void CallBack(TreeViewItemData node, List<TreeViewItemData> subNodes);

	private TreeViewItemData _data;

	private CallBack _callBackMethod;

	public TreeViewItemData Data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public CallBack CallBackMethod => _callBackMethod;

	public ItemHasBeenExpandedASynchronousArgs(TreeViewItemData data, CallBack callBackMethod)
	{
		_data = data;
		_callBackMethod = callBackMethod;
	}
}

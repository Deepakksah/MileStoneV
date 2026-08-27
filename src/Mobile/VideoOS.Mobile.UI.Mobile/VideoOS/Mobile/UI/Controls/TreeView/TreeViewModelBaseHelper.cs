using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.UI.Controls.TreeView;

public static class TreeViewModelBaseHelper<T> where T : ITreeViewModelItem
{
	public static IEnumerable<TreeViewModelBase<T>> GetSelectedLeaves(IEnumerable<TreeViewModelBase<T>> tree, Func<T, bool> filter = null)
	{
		List<TreeViewModelBase<T>> list = new List<TreeViewModelBase<T>>();
		foreach (TreeViewModelBase<T> item in tree)
		{
			if (item.Children != null && item.Children.Count != 0)
			{
				list.AddRange(GetSelectedLeaves(item.Children, filter));
			}
			else if (item.IsChecked.HasValue && item.IsChecked.Value && (filter == null || filter(item.InnerObject)))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static void Reset(IEnumerable<TreeViewModelBase<T>> tree)
	{
		foreach (TreeViewModelBase<T> item in GetEnumerator(tree))
		{
			item.IsChecked = false;
		}
	}

	public static TreeViewModelBase<T> GetItemById(IEnumerable<TreeViewModelBase<T>> tree, Guid id)
	{
		TreeViewModelBase<T> result = null;
		foreach (TreeViewModelBase<T> item in GetEnumerator(tree))
		{
			if (item.Id.Equals(id))
			{
				result = item;
				break;
			}
		}
		return result;
	}

	public static IEnumerable<TreeViewModelBase<T>> GetEnumerator(IEnumerable<TreeViewModelBase<T>> tree)
	{
		foreach (TreeViewModelBase<T> treeItem in tree)
		{
			yield return treeItem;
			if (treeItem.Children == null || treeItem.Children.Count <= 0)
			{
				continue;
			}
			foreach (TreeViewModelBase<T> item in GetEnumerator(treeItem.Children))
			{
				yield return item;
			}
		}
	}
}

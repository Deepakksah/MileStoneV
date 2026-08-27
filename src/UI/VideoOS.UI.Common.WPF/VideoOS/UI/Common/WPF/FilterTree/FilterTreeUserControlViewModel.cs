using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.UI.Common.WPF.FilterTree;

public class FilterTreeUserControlViewModel
{
	private class FilterTreeBuilder
	{
		private static readonly Guid RootId = Guid.Empty;

		private readonly IEnumerable<IFilterTreeItemContent> _allFilterItems;

		private readonly bool _includeFilterItemsInTree;

		private readonly Dictionary<Guid, IFilterTreeItemContent> _topLevelParentsDictionary;

		private readonly Dictionary<Guid, FilterTreeItem> _visitedNodes = new Dictionary<Guid, FilterTreeItem>();

		public FilterTreeBuilder(IEnumerable<IFilterTreeItemContent> topLevelParents, IEnumerable<IFilterTreeItemContent> allFilterItems, bool includeFilterItemsInTree)
		{
			_topLevelParentsDictionary = topLevelParents?.ToDictionary((IFilterTreeItemContent p) => p.Id) ?? new Dictionary<Guid, IFilterTreeItemContent>();
			_allFilterItems = allFilterItems;
			_includeFilterItemsInTree = includeFilterItemsInTree;
		}

		public FilterTreeItem BuildTree(string rootItemName)
		{
			FilterTreeItem result = BuildRootNode(rootItemName);
			BuildTree();
			return result;
		}

		private FilterTreeItem BuildRootNode(string rootItemName)
		{
			FilterTreeItem filterTreeItem = new FilterTreeItem(rootItemName);
			_visitedNodes.Add(RootId, filterTreeItem);
			return filterTreeItem;
		}

		private void BuildTree()
		{
			foreach (IFilterTreeItemContent allFilterItem in _allFilterItems)
			{
				BuildFilterItem(allFilterItem);
			}
		}

		private void BuildFilterItem(IFilterTreeItemContent filterItem)
		{
			FilterTreeItem filterTreeItem = BuildAncestors(filterItem, filterItem);
			if (_includeFilterItemsInTree)
			{
				FilterTreeItem filterTreeItem2 = CreateFilterItemNode(filterItem);
				filterTreeItem2.FilteredItems.Add(filterItem);
				BuildChildren(filterTreeItem2);
				filterTreeItem.Children.Add(filterTreeItem2);
			}
		}

		private FilterTreeItem BuildAncestors(IFilterTreeItemContent currentNode, IFilterTreeItemContent filterItem)
		{
			FilterTreeItem filterTreeItem = ((currentNode.Parent != null) ? TraverseParent(currentNode.Parent, filterItem) : ((currentNode.TopLevelParent == null || !_topLevelParentsDictionary.ContainsKey(currentNode.TopLevelParent.Id)) ? _visitedNodes[RootId] : TraverseParent(currentNode.TopLevelParent, filterItem)));
			filterTreeItem.FilteredItems.Add(filterItem);
			return filterTreeItem;
		}

		private FilterTreeItem TraverseParent(IFilterTreeItemContent parent, IFilterTreeItemContent filterItem)
		{
			FilterTreeItem filterTreeItem = BuildAncestors(parent, filterItem);
			if (!_visitedNodes.TryGetValue(parent.Id, out var value))
			{
				value = CreateFilterItemNode(parent);
				filterTreeItem.Children.Add(value);
			}
			return value;
		}

		private void BuildChildren(FilterTreeItem node)
		{
			foreach (IFilterTreeItemContent child in node.Content.Children)
			{
				FilterTreeItem filterTreeItem = new FilterTreeItem(child);
				node.Children.Add(filterTreeItem);
				BuildChildren(filterTreeItem);
			}
		}

		private FilterTreeItem CreateFilterItemNode(IFilterTreeItemContent filterItem)
		{
			FilterTreeItem filterTreeItem = new FilterTreeItem(filterItem);
			_visitedNodes.Add(filterTreeItem.Content.Id, filterTreeItem);
			return filterTreeItem;
		}
	}

	public IEnumerable<FilterTreeItem> RootItems { get; private set; }

	public FilterTreeUserControlViewModel(string rootItemName, IEnumerable<IFilterTreeItemContent> topLevelParents, IEnumerable<IFilterTreeItemContent> allFilterItems, bool includeFilterItemsInTree, IEnumerable<IFilterTreeItemContent> customRootItems = null)
	{
		FilterTreeItem item = new FilterTreeBuilder(topLevelParents, allFilterItems, includeFilterItemsInTree).BuildTree(rootItemName);
		List<FilterTreeItem> list = new List<FilterTreeItem> { item };
		if (customRootItems != null)
		{
			list.AddRange(customRootItems.Select((IFilterTreeItemContent custom) => new FilterTreeItem(custom)));
		}
		RootItems = list;
	}
}

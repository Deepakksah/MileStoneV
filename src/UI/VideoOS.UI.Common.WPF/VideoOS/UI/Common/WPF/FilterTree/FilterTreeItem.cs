using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace VideoOS.UI.Common.WPF.FilterTree;

public class FilterTreeItem
{
	private readonly Lazy<IList<IFilterTreeItemContent>> _descendants;

	public Image Image { get; private set; }

	public string Title { get; private set; }

	public ObservableCollection<FilterTreeItem> Children { get; }

	public ObservableCollection<IFilterTreeItemContent> FilteredItems { get; private set; }

	public IFilterTreeItemContent Content { get; }

	public bool IsCustomItem
	{
		get
		{
			if (Content != null)
			{
				return Content.IsCustomItem;
			}
			return false;
		}
	}

	public IEnumerable<IFilterTreeItemContent> SelfAndDescendantItems
	{
		get
		{
			if (Content == null)
			{
				return DescendantItems;
			}
			return DescendantItems.Concat(Enumerable.Repeat(Content, 1));
		}
	}

	public IEnumerable<IFilterTreeItemContent> DescendantItems => _descendants.Value;

	public FilterTreeItem(string name)
	{
		Title = name;
		Children = new ObservableCollection<FilterTreeItem>();
		FilteredItems = new ObservableCollection<IFilterTreeItemContent>();
		_descendants = new Lazy<IList<IFilterTreeItemContent>>(() => EnumerateDescendants().ToList());
	}

	public FilterTreeItem(IFilterTreeItemContent content)
		: this(content.Name)
	{
		Image = content.Icon;
		Content = content;
	}

	private IEnumerable<IFilterTreeItemContent> EnumerateDescendants()
	{
		return Children.Select((FilterTreeItem child) => child.Content).Concat(Children.SelectMany((FilterTreeItem child) => child.DescendantItems));
	}
}

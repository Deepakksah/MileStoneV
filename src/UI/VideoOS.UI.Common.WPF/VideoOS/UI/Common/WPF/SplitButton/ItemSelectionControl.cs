using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.SplitButton;

public partial class ItemSelectionControl : UserControl, IComponentConnector
{
	public List<SplitButtonItem> Items
	{
		set
		{
			_itemListBox.Items.Clear();
			value.ForEach(delegate(SplitButtonItem item)
			{
				_itemListBox.Items.Add(new ListBoxItemWrapper
				{
					SplitButtonItem = item,
					Selected = false
				});
			});
		}
	}

	public SplitButtonItem SelectedItem
	{
		get
		{
			if (_itemListBox.SelectedItem is ListBoxItemWrapper listBoxItemWrapper)
			{
				return listBoxItemWrapper.SplitButtonItem;
			}
			return null;
		}
		set
		{
			foreach (ListBoxItemWrapper item in (IEnumerable)_itemListBox.Items)
			{
				if (item.SplitButtonItem == value)
				{
					item.Selected = true;
					_itemListBox.SelectedItem = item;
				}
				else
				{
					item.Selected = false;
				}
			}
		}
	}

	public event EventHandler SelectedItemChangedEvent;

	public ItemSelectionControl()
	{
		InitializeComponent();
	}

	private void _itemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_itemListBox.SelectedItem is ListBoxItemWrapper listBoxItemWrapper)
		{
			SelectedItem = listBoxItemWrapper.SplitButtonItem;
		}
		else
		{
			SelectedItem = null;
		}
		if (this.SelectedItemChangedEvent != null)
		{
			this.SelectedItemChangedEvent(this, new EventArgs());
		}
	}
}

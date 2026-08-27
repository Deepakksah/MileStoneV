using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public partial class MultipleSelectComboBoxItem : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ListBoxStyleProperty = DependencyProperty.Register("ListBoxStyle", typeof(Style), typeof(MultipleSelectComboBoxItem), new PropertyMetadata((object)null));

	public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle", typeof(Style), typeof(MultipleSelectComboBoxItem), new PropertyMetadata((object)null));

	public static readonly DependencyProperty CheckBoxStyleProperty = DependencyProperty.Register("CheckBoxStyle", typeof(Style), typeof(MultipleSelectComboBoxItem), new PropertyMetadata((object)null));

	public Style ListBoxStyle
	{
		get
		{
			return (Style)GetValue(ListBoxStyleProperty);
		}
		set
		{
			SetValue(ListBoxStyleProperty, value);
		}
	}

	public Style TextBlockStyle
	{
		get
		{
			return (Style)GetValue(TextBlockStyleProperty);
		}
		set
		{
			SetValue(TextBlockStyleProperty, value);
		}
	}

	public Style CheckBoxStyle
	{
		get
		{
			return (Style)GetValue(CheckBoxStyleProperty);
		}
		set
		{
			SetValue(CheckBoxStyleProperty, value);
		}
	}

	public event EventHandler SelectedItemsChanged = delegate
	{
	};

	public event EventHandler<CustomItemEventArgs> CustomItemSelected = delegate
	{
	};

	public MultipleSelectComboBoxItem()
	{
		InitializeComponent();
	}

	private void MultipleSelectComboBoxItem_OnLoaded(object sender, RoutedEventArgs e)
	{
		UpdateAllCheckBox();
	}

	private void MultipleSelectComboBoxItem_OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void _allCheckbox_OnClick(object sender, RoutedEventArgs e)
	{
		bool flag = false;
		if (!_allCheckbox.IsChecked.HasValue)
		{
			_allCheckbox.IsChecked = false;
		}
		bool valueOrDefault = _allCheckbox.IsChecked == true;
		if (base.DataContext is IEnumerable<Item> enumerable)
		{
			foreach (Item item in enumerable)
			{
				if (item.IsSelected != valueOrDefault)
				{
					item.IsSelected = valueOrDefault;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.SelectedItemsChanged(this, null);
		}
	}

	private void _customItemsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_customItemsListBox.SelectedItem is CustomItem customItem)
		{
			this.CustomItemSelected(this, new CustomItemEventArgs
			{
				CustomItem = customItem
			});
		}
		_customItemsListBox.SelectedItem = null;
	}

	private void OnUpdateAllCheckBox(object sender, ExecutedRoutedEventArgs e)
	{
		UpdateAllCheckBox();
		this.SelectedItemsChanged(this, new EventArgs());
	}

	private void UpdateAllCheckBox()
	{
		bool flag = true;
		bool flag2 = true;
		if (base.DataContext is IEnumerable<Item> enumerable)
		{
			foreach (Item item in enumerable)
			{
				if (item.IsSelected)
				{
					flag2 = false;
				}
				else
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			_allCheckbox.IsChecked = true;
		}
		else if (flag2)
		{
			_allCheckbox.IsChecked = false;
		}
		else
		{
			_allCheckbox.IsChecked = null;
		}
	}
}

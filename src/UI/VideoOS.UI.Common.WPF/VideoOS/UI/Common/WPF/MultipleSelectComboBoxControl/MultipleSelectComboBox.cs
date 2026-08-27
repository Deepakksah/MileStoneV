using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public sealed partial class MultipleSelectComboBox : UserControl, IWeakEventListener, IComponentConnector
{
	public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle", typeof(Style), typeof(MultipleSelectComboBox), new PropertyMetadata((object)null));

	private bool _isTextValid;

	public static DependencyProperty ComboBoxStyleProperty = DependencyProperty.Register("ComboBoxStyle", typeof(Style), typeof(MultipleSelectComboBox));

	public static readonly DependencyProperty CustomLabelProperty = DependencyProperty.Register("CustomLabel", typeof(string), typeof(MultipleSelectComboBox), new PropertyMetadata(CustomLabelPropertyChanged));

	public static readonly DependencyProperty SelectionRequiredProperty = DependencyProperty.Register("SelectionRequired", typeof(bool), typeof(MultipleSelectComboBox), new PropertyMetadata(SelectionRequiredPropertyChanged));

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

	public Style ComboBoxStyle
	{
		get
		{
			return (Style)GetValue(ComboBoxStyleProperty);
		}
		set
		{
			SetValue(ComboBoxStyleProperty, value);
		}
	}

	public string CustomLabel
	{
		get
		{
			return (string)GetValue(CustomLabelProperty);
		}
		set
		{
			SetValue(CustomLabelProperty, value);
		}
	}

	public bool SelectionRequired
	{
		get
		{
			return (bool)GetValue(SelectionRequiredProperty);
		}
		set
		{
			SetValue(SelectionRequiredProperty, value);
		}
	}

	public Action<string> SetCustomLabelCallback { get; set; }

	public List<CustomItem> CustomItems { get; set; }

	public string NoItemsSelectedLabel { get; set; }

	public string AllItemsSelectedLabel { get; set; }

	public string AllItemsCheckBoxLabel { get; set; }

	public bool FlatStyle { get; set; }

	public bool IsAllItemsSelected
	{
		get
		{
			if (base.DataContext is IEnumerable<Item> enumerable)
			{
				foreach (Item item in enumerable)
				{
					if (!item.IsSelected)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public List<Item> SelectedItems
	{
		get
		{
			List<Item> list = new List<Item>();
			if (base.DataContext is IEnumerable<Item> enumerable)
			{
				foreach (Item item in enumerable)
				{
					if (item.IsSelected)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public event EventHandler SelectedItemsChanged = delegate
	{
	};

	public event EventHandler<CustomItemEventArgs> CustomItemSelected = delegate
	{
	};

	public MultipleSelectComboBox()
	{
		CustomItems = new List<CustomItem>();
		FlatStyle = true;
		SelectionRequired = true;
		InitializeComponent();
		SetResourceReference(ComboBoxStyleProperty, "ScComboBoxFlatNormalState");
		base.DataContextChanged += MultipleSelectComboBox_DataContextChanged;
	}

	private static void CustomLabelPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		((MultipleSelectComboBox)obj).Update();
	}

	private static void SelectionRequiredPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		((MultipleSelectComboBox)obj).Update();
	}

	public void Update()
	{
		_isTextValid = false;
		base.Dispatcher.BeginInvoke(delegate
		{
			UpdateCore();
		});
	}

	public void UpdateCore()
	{
		if (!_isTextValid)
		{
			UpdateText();
			_isTextValid = true;
		}
	}

	private void _combobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		_combobox.SelectedItem = null;
	}

	private void MultipleSelectComboBox_OnLoaded(object sender, RoutedEventArgs e)
	{
		if (!FlatStyle)
		{
			_combobox.Style = (Style)FindResource("ScComboBox");
		}
		UpdateText();
	}

	private void MultipleSelectComboBoxItem_OnSelectedItemsChanged(object sender, EventArgs e)
	{
		UpdateText();
		this.SelectedItemsChanged(this, new EventArgs());
	}

	private void MultipleSelectComboBoxItem_OnCustomItemSelected(object sender, CustomItemEventArgs e)
	{
		this.CustomItemSelected(this, e);
	}

	private void _combobox_OnDropDownClosed(object sender, EventArgs e)
	{
		EnsureItemSelected();
		UpdateText();
	}

	private void EnsureItemSelected()
	{
		if (!base.IsLoaded || !SelectionRequired || !(base.DataContext is IEnumerable<Item> enumerable))
		{
			return;
		}
		bool flag = true;
		foreach (Item item in enumerable)
		{
			if (item.IsSelected)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		foreach (Item item2 in enumerable)
		{
			item2.IsSelected = true;
		}
		this.SelectedItemsChanged(this, new EventArgs());
	}

	private void UpdateText()
	{
		if (!base.IsLoaded)
		{
			return;
		}
		if (CustomLabel != null)
		{
			_comboBoxTextBlock.Text = CustomLabel;
			return;
		}
		string text = string.Empty;
		bool flag = true;
		if (base.DataContext is IEnumerable<Item> enumerable)
		{
			foreach (Item item in enumerable)
			{
				if (item.IsSelected)
				{
					text = text + item.Text + ", ";
				}
				else
				{
					flag = false;
				}
			}
		}
		text = (flag ? AllItemsSelectedLabel : ((!string.IsNullOrEmpty(text)) ? text.Substring(0, text.Length - 2) : NoItemsSelectedLabel));
		_comboBoxTextBlock.Text = text;
	}

	private void MultipleSelectComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateText();
		foreach (Item item in GetItems())
		{
			PropertyChangedEventManager.AddListener(item, this, string.Empty);
		}
	}

	private IEnumerable<Item> GetItems()
	{
		if (!(base.DataContext is IEnumerable<Item> enumerable))
		{
			yield break;
		}
		foreach (Item item in enumerable)
		{
			yield return item;
		}
	}

	bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
	{
		Update();
		return true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}

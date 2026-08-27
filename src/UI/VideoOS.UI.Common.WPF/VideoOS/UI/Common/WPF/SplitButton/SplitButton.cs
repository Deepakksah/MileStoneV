using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.SplitButton;

public partial class SplitButton : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(SplitButton), new PropertyMetadata(Brushes.Black));

	public static readonly DependencyProperty PopupBackgroundBrushProperty = DependencyProperty.Register("PopupBackgroundBrush", typeof(Brush), typeof(SplitButton), new PropertyMetadata(Brushes.White));

	public static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(List<SplitButtonItem>), typeof(SplitButton), new PropertyMetadata(new List<SplitButtonItem>(), null, ItemsCoerceValue));

	public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(SplitButtonItem), typeof(SplitButton), new PropertyMetadata(null, SelectedItem_Changed, SelectedItem_CoerceValue));

	public bool AutoSelectNextItemOnActionBottonClick { get; set; }

	public Brush ArrowBrush
	{
		get
		{
			return (Brush)GetValue(ArrowBrushProperty);
		}
		set
		{
			SetValue(ArrowBrushProperty, value);
		}
	}

	public Brush PopupBackgroundBrush
	{
		get
		{
			return (Brush)GetValue(PopupBackgroundBrushProperty);
		}
		set
		{
			SetValue(PopupBackgroundBrushProperty, value);
		}
	}

	public List<SplitButtonItem> Items
	{
		get
		{
			return (List<SplitButtonItem>)GetValue(ItemsProperty);
		}
		set
		{
			SetValue(ItemsProperty, value);
		}
	}

	public SplitButtonItem SelectedItem
	{
		get
		{
			return (SplitButtonItem)GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public event EventHandler ActionButtonClickedEvent;

	public event EventHandler SelectedItemChangedEvent;

	public SplitButton()
	{
		InitializeComponent();
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
	}

	private static object ItemsCoerceValue(DependencyObject obj, object value)
	{
		if (obj is SplitButton splitButton)
		{
			splitButton.OnItemsCoerceValue(value);
		}
		return value;
	}

	private void OnItemsCoerceValue(object value)
	{
		List<SplitButtonItem> list = value as List<SplitButtonItem>;
		_itemSelectionPopup.IsOpen = false;
		if (list != null && SelectedItem != null && !list.Contains(SelectedItem))
		{
			SelectedItem = null;
		}
	}

	private static object SelectedItem_CoerceValue(DependencyObject obj, object value)
	{
		if (obj is SplitButton splitButton && value is SplitButtonItem item && !splitButton.Items.Contains(item))
		{
			return splitButton.SelectedItem;
		}
		return value;
	}

	private static void SelectedItem_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
	{
		if (obj is SplitButton splitButton)
		{
			splitButton.OnSelectedItemChanged();
		}
	}

	private void OnSelectedItemChanged()
	{
		if (SelectedItem == null)
		{
			_image.Source = null;
		}
		else if (SelectedItem.ImageSource != null)
		{
			_image.Source = SelectedItem.ImageSource;
		}
		else if (SelectedItem.ImageName != null)
		{
			_image.SetResourceReference(Image.SourceProperty, SelectedItem.ImageName);
		}
		else
		{
			_image.Source = null;
		}
	}

	public void ClosePopup()
	{
		_itemSelectionPopup.IsOpen = false;
	}

	private void Action_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (AutoSelectNextItemOnActionBottonClick)
		{
			if (Items == null || Items.Count == 0 || (Items.Count == 1 && SelectedItem != null))
			{
				return;
			}
			if (SelectedItem == null)
			{
				SelectedItem = Items[0];
			}
			else if (Items.IndexOf(SelectedItem) == Items.Count - 1)
			{
				SelectedItem = Items[0];
			}
			else
			{
				SelectedItem = Items[Items.IndexOf(SelectedItem) + 1];
			}
			if (this.SelectedItemChangedEvent != null)
			{
				this.SelectedItemChangedEvent(this, new EventArgs());
			}
		}
		if (this.ActionButtonClickedEvent != null)
		{
			this.ActionButtonClickedEvent(this, new EventArgs());
		}
	}

	private void Arrow_Click(object sender, RoutedEventArgs e)
	{
		_itemSelectionControl.Items = Items;
		_itemSelectionControl.SelectedItem = SelectedItem;
		_itemSelectionPopup.IsOpen = true;
	}

	private void _itemSelectionPopup_Closed(object sender, EventArgs e)
	{
		if (SelectedItem != _itemSelectionControl.SelectedItem && Items.Contains(_itemSelectionControl.SelectedItem))
		{
			SelectedItem = _itemSelectionControl.SelectedItem;
			if (this.SelectedItemChangedEvent != null)
			{
				this.SelectedItemChangedEvent(this, new EventArgs());
			}
		}
	}

	private void _itemSelectionControl_SelectedItemChangedEvent(object sender, EventArgs e)
	{
		_itemSelectionPopup.IsOpen = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}

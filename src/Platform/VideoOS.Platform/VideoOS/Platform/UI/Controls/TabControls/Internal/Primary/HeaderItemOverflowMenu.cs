using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public partial class HeaderItemOverflowMenu : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(List<object>), typeof(HeaderItemOverflowMenu), new PropertyMetadata(null, ItemsChanged));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(HeaderItemOverflowMenu), new PropertyMetadata(false, IsOpenChanged));

	private VideoOSContextMenuLarge _contextMenu;

	public static IMultiValueConverter IsTabStopConverter { get; } = new LambdaConverter2<List<object>, object>((List<object> items, object selectedItem) => items?.Contains(selectedItem) ?? false, "IsTabStopConverter");

	public static IMultiValueConverter BackgroundConverter { get; } = new LambdaConverter6<DependencyObject, List<object>, object, bool, bool, bool>(delegate(DependencyObject self, List<object> items, object selectedItem, bool isEnabled, bool isMouseOver, bool isOpen)
	{
		object resourceKey = ((selectedItem != null && items != null && items.Any() && items.Contains(selectedItem)) ? ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedDisabledBrush" : (isOpen ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedHoveredBrush"))) : ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedDisabledBrush" : (isOpen ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedHoveredBrush"))));
		return self.FindResource(resourceKey);
	}, "BackgroundConverter");

	public static IMultiValueConverter SelectedIndicatorFillConverter { get; } = new LambdaConverter4<DependencyObject, List<object>, object, bool>(delegate(DependencyObject self, List<object> items, object selectedItem, bool isEnabled)
	{
		object resourceKey = ((selectedItem == null || items == null || !items.Any() || !items.Contains(selectedItem)) ? ((object)VideoOSResourceKeys.TransparentBrush) : (isEnabled ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabSelectedIndicatorIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabSelectedIndicatorDisabledBrush"));
		return self.FindResource(resourceKey);
	}, "SelectedIndicatorFillConverter");

	public List<object> Items
	{
		get
		{
			return (List<object>)GetValue(ItemsProperty);
		}
		set
		{
			SetValue(ItemsProperty, value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((HeaderItemOverflowMenu)d).UpdateContextMenu();
	}

	private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		HeaderItemOverflowMenu headerItemOverflowMenu = (HeaderItemOverflowMenu)d;
		if ((bool)e.NewValue)
		{
			headerItemOverflowMenu.OpenContextMenu();
		}
	}

	public HeaderItemOverflowMenu()
	{
		InitializeComponent();
	}

	private void HeaderItemOverflowMenu_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		SetCurrentValue(IsOpenProperty, true);
	}

	private void HeaderItemOverflowMenu_OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return || e.Key == Key.Space)
		{
			SetCurrentValue(IsOpenProperty, true);
		}
	}

	private void OpenContextMenu()
	{
		if (Items != null && Items.Any())
		{
			_contextMenu = new VideoOSContextMenuLarge();
			_contextMenu.Placement = PlacementMode.Custom;
			_contextMenu.PlacementTarget = this;
			_contextMenu.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
			_contextMenu.StaysOpen = false;
			_contextMenu.Opened += ContextMenuOnOpened;
			_contextMenu.Closed += ContextMenuOnClosed;
			_contextMenu.ItemDragInitiated += ContextMenuOnItemDragInitiated;
			UpdateContextMenuItems();
			_contextMenu.IsOpen = true;
		}
	}

	private void UpdateContextMenu()
	{
		if (_contextMenu != null && _contextMenu.IsOpen)
		{
			if (Items != null && Items.Any())
			{
				UpdateContextMenuItems();
			}
			else
			{
				_contextMenu.IsOpen = false;
			}
		}
	}

	private void ContextMenuOnItemDragInitiated(object sender, RoutedEventArgs e)
	{
		object tag = ((VideoOSContextMenuItem)((VideoOSContextMenuBase.ItemDragInitiatedEventArgs)e).Item).Tag;
		this.FindAncestor<VideoOSTabControlBase>().RaiseEvent(new VideoOSTabControlBase.ItemDragInitiatedEventArgs(VideoOSTabControlBase.ItemDragInitiatedEvent, tag, this));
	}

	private void ContextMenuOnOpened(object sender, RoutedEventArgs e)
	{
		_contextMenu.ItemsSource.OfType<VideoOSContextMenuItem>().FirstOrDefault((VideoOSContextMenuItem i) => i.IsChecked)?.Focus();
	}

	private void ContextMenuOnClosed(object sender, RoutedEventArgs e)
	{
		SetCurrentValue(IsOpenProperty, false);
	}

	private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
	{
		CustomPopupVerticalPlacementResult customPopupVerticalPlacementResult = PopupUtils.CustomPopupVerticalPlacementCallback(popupSize, targetSize, offset, _contextMenu.PlacementTarget, PopupUtils.CustomVerticalPlacement.Bottom, PopupUtils.CustomPlacementAnchoring.End, _contextMenu.ResolveDropShadowThickness());
		if (customPopupVerticalPlacementResult.SuggestedMaxHeight.HasValue && customPopupVerticalPlacementResult.SuggestedMaxHeight.Value < _contextMenu.MaxHeight)
		{
			_contextMenu.MaxHeight = customPopupVerticalPlacementResult.SuggestedMaxHeight.Value;
		}
		return customPopupVerticalPlacementResult.Placements;
	}

	private static VideoOSIconSourceBase ResolveItemIcon(object item, VideoOSTabControlPrimaryBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.IconSource;
		}
		return item.ResolvePropertyValue<VideoOSIconSourceBase>(tabControlPrimary.ItemIconSourcePropertyName, throwOnError: false);
	}

	private static string ResolveItemText(object item, VideoOSTabControlBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.Text;
		}
		return item.ResolvePropertyValue<string>(tabControlPrimary.ItemTextPropertyName, throwOnError: false);
	}

	private static string ResolveItemToolTip(object item, VideoOSTabControlBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.ToolTip;
		}
		return item.ResolvePropertyValue<string>(tabControlPrimary.ItemToolTipPropertyName, throwOnError: false);
	}

	private static bool ResolveItemIsEnabled(object item, VideoOSTabControlBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.IsEnabled;
		}
		return item.ResolvePropertyValue(tabControlPrimary.ItemIsEnabledPropertyName, throwOnError: false, defaultValue: true);
	}

	private static bool ResolveItemIsDraggable(object item, VideoOSTabControlBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.IsDraggable;
		}
		return item.ResolvePropertyValue(tabControlPrimary.ItemIsDraggablePropertyName, throwOnError: false, defaultValue: false);
	}

	private static ICollection<VideoOSActionBase> ResolveItemActions(object item, VideoOSTabControlPrimaryBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.Actions;
		}
		return item.ResolvePropertyValue<ICollection<VideoOSActionBase>>(tabControlPrimary.ItemActionsPropertyName, throwOnError: false);
	}

	private static VideoOSAlert ResolveItemAlert(object item, VideoOSTabControlPrimaryBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.Alert;
		}
		return item.ResolvePropertyValue<VideoOSAlert>(tabControlPrimary.ItemAlertPropertyName, throwOnError: false);
	}

	private static string ResolveItemAutomationId(object item, VideoOSTabControlBase tabControlPrimary)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.AutomationId;
		}
		return item.ResolvePropertyValue(tabControlPrimary.ItemAutomationIdPropertyName, throwOnError: false, string.Empty);
	}

	private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
	{
		if (sender == e.OriginalSource)
		{
			VideoOSContextMenuItem videoOSContextMenuItem = (VideoOSContextMenuItem)sender;
			this.FindAncestor<VideoOSTabControlBase>().SelectedItem = videoOSContextMenuItem.Tag;
		}
	}

	private void UpdateContextMenuItems()
	{
		VideoOSTabControlPrimaryBase videoOSTabControlPrimaryBase = this.FindAncestor<VideoOSTabControlPrimaryBase>();
		ObservableCollection<VideoOSContextMenuItem> observableCollection = (_contextMenu.ItemsSource as ObservableCollection<VideoOSContextMenuItem>) ?? new ObservableCollection<VideoOSContextMenuItem>();
		List<VideoOSContextMenuItem> list = new List<VideoOSContextMenuItem>();
		foreach (VideoOSContextMenuItem item2 in observableCollection)
		{
			if (!Items.Contains(item2.Tag))
			{
				list.Add(item2);
			}
		}
		foreach (VideoOSContextMenuItem item3 in list)
		{
			observableCollection.Remove(item3);
		}
		for (int i = 0; i < Items.Count; i++)
		{
			object item = Items[i];
			VideoOSContextMenuItem videoOSContextMenuItem = observableCollection.FirstOrDefault((VideoOSContextMenuItem x) => x.Tag == item);
			if (videoOSContextMenuItem != null)
			{
				int num = observableCollection.IndexOf(videoOSContextMenuItem);
				if (num != i)
				{
					observableCollection.Move(num, i);
				}
				continue;
			}
			string text = ResolveItemText(item, videoOSTabControlPrimaryBase);
			string text2 = ResolveItemToolTip(item, videoOSTabControlPrimaryBase);
			VideoOSContextMenuItem videoOSContextMenuItem2 = new VideoOSContextMenuItem
			{
				IconSource = ResolveItemIcon(item, videoOSTabControlPrimaryBase),
				Data = text,
				ToolTip = (object.Equals(text, text2) ? null : text2),
				IsCheckable = true,
				IsEnabled = ResolveItemIsEnabled(item, videoOSTabControlPrimaryBase),
				IsDraggable = ResolveItemIsDraggable(item, videoOSTabControlPrimaryBase),
				StaysOpenOnClick = false,
				CheckedAppearance = VideoOSContextMenuItem.CheckedAppearances.Highlighted,
				Actions = ResolveItemActions(item, videoOSTabControlPrimaryBase),
				Alert = ResolveItemAlert(item, videoOSTabControlPrimaryBase),
				Tag = item
			};
			AutomationProperties.SetAutomationId(videoOSContextMenuItem2, ResolveItemAutomationId(item, videoOSTabControlPrimaryBase));
			videoOSContextMenuItem2.Click += ContextMenuItem_Click;
			observableCollection.Add(videoOSContextMenuItem2);
		}
		foreach (VideoOSContextMenuItem item4 in observableCollection)
		{
			item4.IsChecked = item4.Tag == videoOSTabControlPrimaryBase.SelectedItem;
		}
		_contextMenu.ItemsSource = observableCollection;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}

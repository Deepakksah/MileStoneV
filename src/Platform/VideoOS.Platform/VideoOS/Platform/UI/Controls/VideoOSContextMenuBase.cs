using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSContextMenuBase : ContextMenu
{
	public sealed class ItemDragInitiatedEventArgs : RoutedEventArgs
	{
		public object Item { get; }

		public FrameworkElement FrameworkElement { get; }

		public ItemDragInitiatedEventArgs(RoutedEvent routedEvent, object item, FrameworkElement frameworkElement)
			: base(routedEvent)
		{
			Item = item;
			FrameworkElement = frameworkElement;
		}
	}

	public static class VideoOSContextMenuBaseInternals
	{
		public const double ContextMenuMinWidth = 120.0;

		public const double ContextMenuMaxWidth = 350.0;

		public static Thickness PopupBorderThickness = new Thickness(1.0);

		public const string ItemsListBorderStyleKey = "VideoOSContextMenuBaseItemsListBorderStyleKey";

		public const string SeparatorStyleKey = "VideoOSContextMenuBaseSeparatorStyleKey";

		public static IMultiValueConverter ThrowOnlyVideoOSContextMenuItemSupportedExceptionConverter { get; } = new LambdaConverter1<MenuItem>(delegate
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Platform context menus derived from {0} ({1}, {2}, ...) only supports menu items of the type {3}.", "VideoOSContextMenuBase", "VideoOSContextMenuLarge", "VideoOSContextMenuSmall", "VideoOSContextMenuItem")));
		}, "ThrowOnlyVideoOSContextMenuItemSupportedExceptionConverter");

		public static IMultiValueConverter ItemBackgroundConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, VideoOSContextMenuItem.CheckedAppearances, bool>(delegate(DependencyObject self, bool isHighlighted, bool isSelected, bool isPressed, VideoOSContextMenuItem.CheckedAppearances checkedAppearance, bool isInteractive)
		{
			object resourceKey = VideoOSResourceKeys.TransparentBrush;
			if (isInteractive)
			{
				if (isPressed)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSContextMenuItemBackgroundPressedBrush";
				}
				else if (isSelected && checkedAppearance == VideoOSContextMenuItem.CheckedAppearances.Highlighted)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSContextMenuItemBackgroundSelectedBrush";
				}
				else if (isHighlighted)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSContextMenuItemBackgroundHoveredBrush";
				}
			}
			return self.FindResource(resourceKey);
		}, "ItemBackgroundConverter");

		public static IMultiValueConverter CheckBoxVisibilityConverter { get; } = new LambdaConverter2<bool, VideoOSContextMenuItem.CheckedAppearances>((bool isSelectable, VideoOSContextMenuItem.CheckedAppearances checkedAppearance) => (isSelectable && checkedAppearance == VideoOSContextMenuItem.CheckedAppearances.Marked) ? ((object)Visibility.Visible) : ((object)Visibility.Collapsed), "CheckBoxVisibilityConverter");

		public static IMultiValueConverter ActionsButtonVisibilityConverter { get; } = new LambdaConverter6<ICollection<VideoOSActionBase>, bool, bool, bool, bool, VideoOSContextMenuItem.CheckedAppearances>((ICollection<VideoOSActionBase> actions, bool isChecked, bool isOpen, bool isMouseOver, bool isInteractive, VideoOSContextMenuItem.CheckedAppearances checkedAppearance) => (isInteractive && actions != null && actions.Count > 0) ? (((isChecked && checkedAppearance == VideoOSContextMenuItem.CheckedAppearances.Highlighted) || isOpen || isMouseOver) ? ((object)Visibility.Visible) : ((object)Visibility.Hidden)) : ((object)Visibility.Collapsed), "ActionsButtonVisibilityConverter");

		public static IMultiValueConverter ControlsAreaVisibilityConverter { get; } = new LambdaConverter2<bool, bool>((bool isFilterControlsVisible, bool isCheckedControlsVisible) => (!(isFilterControlsVisible || isCheckedControlsVisible)) ? Visibility.Collapsed : Visibility.Visible, "ControlsAreaVisibilityConverter");

		public static IMultiValueConverter ControlsAreaGapRowHeightConverter { get; } = new LambdaConverter2<bool, bool>((bool isFilterControlsVisible, bool isCheckedControlsVisible) => (isFilterControlsVisible && isCheckedControlsVisible) ? new GridLength(4.0, GridUnitType.Pixel) : new GridLength(0.0, GridUnitType.Pixel), "ControlsAreaGapRowHeightConverter");

		public static IMultiValueConverter ControlsAreaMaxWidthConverter { get; } = new LambdaConverter3<DataTemplate, DataTemplate, double>(delegate(DataTemplate headerTemplate, DataTemplate footerTemplate, double contextMenuWidth)
		{
			double num = (double.IsNaN(contextMenuWidth) ? 350.0 : contextMenuWidth);
			return (headerTemplate != null || footerTemplate != null) ? double.PositiveInfinity : num;
		}, "ControlsAreaMaxWidthConverter");

		public static IMultiValueConverter ContentTemplateConverter { get; } = new LambdaConverter2<DataTemplate, DataTemplate>((DataTemplate itemContentTemplate, DataTemplate sharedContentTemplate) => itemContentTemplate ?? sharedContentTemplate, "ContentTemplateConverter");

		public static IMultiValueConverter FooterSeparatorVisibilityConverter { get; } = new LambdaConverter2<bool, DataTemplate>((bool hasItems, DataTemplate headerTemplate) => (!hasItems && headerTemplate == null) ? Visibility.Collapsed : Visibility.Visible, "FooterSeparatorVisibilityConverter");

		public static IMultiValueConverter DropShadowThicknessConverter { get; } = new LambdaConverter1<FrameworkElement>((FrameworkElement frameworkElement) => ResolveDropShadowThickness(frameworkElement), "DropShadowThicknessConverter");

		public static IMultiValueConverter HeaderFooterContentConverter { get; } = new LambdaConverter2<object, DataTemplate>((object dataContext, DataTemplate template) => (template != null) ? dataContext : null, "HeaderFooterContentConverter");
	}

	public delegate bool FilterCallbackHandler(string filterValue, VideoOSContextMenuItem item);

	public static readonly RoutedEvent ItemDragInitiatedEvent;

	public static readonly DependencyProperty ContentTemplateProperty;

	public static readonly DependencyProperty FilterCallbackProperty;

	public static readonly RoutedEvent IsBulkCheckedChangesInProgressChangedEvent;

	public static readonly DependencyProperty IsBulkCheckedChangesInProgressProperty;

	public static readonly DependencyProperty IsFilterControlsVisibleProperty;

	public static readonly DependencyProperty IsCheckedControlsVisibleProperty;

	public static readonly DependencyProperty HideUncheckedItemsProperty;

	public static readonly DependencyProperty FilterTextProperty;

	public static readonly DependencyProperty NoMatchingItemsProperty;

	public static readonly DependencyProperty SubMenuPopupOpenedClosedCommandProperty;

	public static readonly DependencyProperty ClearCheckedItemsCommandProperty;

	public static readonly DependencyProperty ItemDragInitiatedCommandProperty;

	public static readonly DependencyProperty HeaderTemplateProperty;

	public static readonly DependencyProperty FooterTemplateProperty;

	private VideoOSScrollViewer _rootScrollViewer;

	public DataTemplate ContentTemplate
	{
		get
		{
			return (DataTemplate)GetValue(ContentTemplateProperty);
		}
		set
		{
			SetValue(ContentTemplateProperty, value);
		}
	}

	public FilterCallbackHandler FilterCallback
	{
		get
		{
			return (FilterCallbackHandler)GetValue(FilterCallbackProperty);
		}
		set
		{
			SetValue(FilterCallbackProperty, value);
		}
	}

	public bool IsBulkCheckedChangesInProgress
	{
		get
		{
			return (bool)GetValue(IsBulkCheckedChangesInProgressProperty);
		}
		private set
		{
			SetValue(IsBulkCheckedChangesInProgressProperty, value);
		}
	}

	public bool IsFilterControlsVisible
	{
		get
		{
			return (bool)GetValue(IsFilterControlsVisibleProperty);
		}
		set
		{
			SetValue(IsFilterControlsVisibleProperty, value);
		}
	}

	public bool IsCheckedControlsVisible
	{
		get
		{
			return (bool)GetValue(IsCheckedControlsVisibleProperty);
		}
		set
		{
			SetValue(IsCheckedControlsVisibleProperty, value);
		}
	}

	public bool HideUncheckedItems
	{
		get
		{
			return (bool)GetValue(HideUncheckedItemsProperty);
		}
		set
		{
			SetValue(HideUncheckedItemsProperty, value);
		}
	}

	public string FilterText
	{
		get
		{
			return (string)GetValue(FilterTextProperty);
		}
		set
		{
			SetValue(FilterTextProperty, value);
		}
	}

	public bool NoMatchingItems
	{
		get
		{
			return (bool)GetValue(NoMatchingItemsProperty);
		}
		private set
		{
			SetValue(NoMatchingItemsProperty, value);
		}
	}

	public DelegateCommand SubMenuPopupOpenedClosedCommand
	{
		get
		{
			return (DelegateCommand)GetValue(SubMenuPopupOpenedClosedCommandProperty);
		}
		private set
		{
			SetValue(SubMenuPopupOpenedClosedCommandProperty, value);
		}
	}

	public DelegateCommand ClearCheckedItemsCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ClearCheckedItemsCommandProperty);
		}
		private set
		{
			SetValue(ClearCheckedItemsCommandProperty, value);
		}
	}

	public DelegateCommand ItemDragInitiatedCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ItemDragInitiatedCommandProperty);
		}
		private set
		{
			SetValue(ItemDragInitiatedCommandProperty, value);
		}
	}

	private static FilterCallbackHandler DefaultFilterCallback { get; }

	public DataTemplate HeaderTemplate
	{
		get
		{
			return (DataTemplate)GetValue(HeaderTemplateProperty);
		}
		set
		{
			SetValue(HeaderTemplateProperty, value);
		}
	}

	public DataTemplate FooterTemplate
	{
		get
		{
			return (DataTemplate)GetValue(FooterTemplateProperty);
		}
		set
		{
			SetValue(FooterTemplateProperty, value);
		}
	}

	public event RoutedEventHandler ItemDragInitiated
	{
		add
		{
			AddHandler(ItemDragInitiatedEvent, value);
		}
		remove
		{
			RemoveHandler(ItemDragInitiatedEvent, value);
		}
	}

	public event RoutedEventHandler IsBulkCheckedChangesInProgressChanged
	{
		add
		{
			AddHandler(IsBulkCheckedChangesInProgressChangedEvent, value);
		}
		remove
		{
			RemoveHandler(IsBulkCheckedChangesInProgressChangedEvent, value);
		}
	}

	private static Thickness ResolveDropShadowThickness(FrameworkElement frameworkElement)
	{
		Thickness thickness = (Thickness)frameworkElement.FindResource(VideoOSResourceKeys.DropShadowThickness);
		return new Thickness(0.0, 0.0, thickness.Right, thickness.Bottom);
	}

	static VideoOSContextMenuBase()
	{
		ItemDragInitiatedEvent = EventManager.RegisterRoutedEvent("ItemDragInitiated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSContextMenuBase));
		ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(VideoOSContextMenuBase), new PropertyMetadata(null));
		FilterCallbackProperty = DependencyProperty.Register("FilterCallback", typeof(FilterCallbackHandler), typeof(VideoOSContextMenuBase), new PropertyMetadata(null));
		IsBulkCheckedChangesInProgressChangedEvent = EventManager.RegisterRoutedEvent("IsBulkCheckedChangesInProgressChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSContextMenuBase));
		IsBulkCheckedChangesInProgressProperty = DependencyProperty.Register("IsBulkCheckedChangesInProgress", typeof(bool), typeof(VideoOSContextMenuBase), new PropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSContextMenuBase)o).RaiseEvent(new RoutedEventArgs(IsBulkCheckedChangesInProgressChangedEvent));
		}));
		IsFilterControlsVisibleProperty = DependencyProperty.Register("IsFilterControlsVisible", typeof(bool), typeof(VideoOSContextMenuBase), new PropertyMetadata(false));
		IsCheckedControlsVisibleProperty = DependencyProperty.Register("IsCheckedControlsVisible", typeof(bool), typeof(VideoOSContextMenuBase), new PropertyMetadata(false));
		HideUncheckedItemsProperty = DependencyProperty.Register("HideUncheckedItems", typeof(bool), typeof(VideoOSContextMenuBase), new PropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSContextMenuBase)o).HideUncheckedItemsChanged();
		}));
		FilterTextProperty = DependencyProperty.Register("FilterText", typeof(string), typeof(VideoOSContextMenuBase), new PropertyMetadata(string.Empty, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSContextMenuBase)o).FilterTextChanged();
		}));
		NoMatchingItemsProperty = DependencyProperty.Register("NoMatchingItems", typeof(bool), typeof(VideoOSContextMenuBase), new PropertyMetadata(false));
		SubMenuPopupOpenedClosedCommandProperty = DependencyProperty.Register("SubMenuPopupOpenedClosedCommand", typeof(DelegateCommand), typeof(VideoOSContextMenuBase), new PropertyMetadata((object)null));
		ClearCheckedItemsCommandProperty = DependencyProperty.Register("ClearCheckedItemsCommand", typeof(DelegateCommand), typeof(VideoOSContextMenuBase), new PropertyMetadata((object)null));
		ItemDragInitiatedCommandProperty = DependencyProperty.Register("ItemDragInitiatedCommand", typeof(DelegateCommand), typeof(VideoOSContextMenuBase), new PropertyMetadata((object)null));
		DefaultFilterCallback = delegate(string filterValue, VideoOSContextMenuItem item)
		{
			if (item.Data is string input)
			{
				string pattern = filterValue.Replace("*", ".*");
				return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
			}
			return true;
		};
		HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(VideoOSContextMenuBase), new PropertyMetadata(null));
		FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(VideoOSContextMenuBase), new PropertyMetadata(null));
		UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(VideoOSContextMenuBase), new FrameworkPropertyMetadata(true));
	}

	public VideoOSContextMenuBase()
	{
		SubMenuPopupOpenedClosedCommand = new DelegateCommand(SubMenuPopupOpenedClosedCommandFunction);
		ClearCheckedItemsCommand = new DelegateCommand(ClearCheckedItemsCommandFunction);
		ItemDragInitiatedCommand = new DelegateCommand(ItemDragInitiatedCommandFunction);
		base.Opened += VideoOSContextMenuBase_Opened;
		base.Closed += VideoOSContextMenuBase_Closed;
	}

	public override void OnApplyTemplate()
	{
		_rootScrollViewer = this.GetTemplateChild<VideoOSScrollViewer>("PART_RootScrollViewer");
	}

	public Thickness ResolveDropShadowThickness()
	{
		return ResolveDropShadowThickness(this);
	}

	private void VideoOSContextMenuBase_Opened(object sender, RoutedEventArgs e)
	{
		if (IsCheckedControlsVisible)
		{
			foreach (object item in (IEnumerable)base.Items)
			{
				if (item is VideoOSContextMenuItem videoOSContextMenuItem)
				{
					videoOSContextMenuItem.Checked += VideoOsContextMenuItem_CheckedChanged;
					videoOSContextMenuItem.Unchecked += VideoOsContextMenuItem_CheckedChanged;
				}
			}
		}
		if (IsFilterControlsVisible || IsCheckedControlsVisible)
		{
			UpdateItemsVisibilityAndControlsState();
		}
	}

	private void VideoOSContextMenuBase_Closed(object sender, RoutedEventArgs e)
	{
		if (IsCheckedControlsVisible)
		{
			foreach (object item in (IEnumerable)base.Items)
			{
				if (item is VideoOSContextMenuItem videoOSContextMenuItem)
				{
					videoOSContextMenuItem.Checked -= VideoOsContextMenuItem_CheckedChanged;
					videoOSContextMenuItem.Unchecked -= VideoOsContextMenuItem_CheckedChanged;
				}
			}
		}
		_rootScrollViewer.ScrollToTop();
		_rootScrollViewer.ClearValue(FrameworkElement.MinWidthProperty);
	}

	private static void SubMenuPopupOpenedClosedCommandFunction(object args)
	{
		EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)args;
		Popup popup = (Popup)eventsToCommandMappingArgs.Sender;
		if (eventsToCommandMappingArgs.EventName == "Opened")
		{
			VideoOSScrollViewer videoOSScrollViewer = (VideoOSScrollViewer)eventsToCommandMappingArgs.CommandParameter2;
			Point point = ((FrameworkElement)popup.PlacementTarget).PointToScreen(new Point(0.0, 0.0));
			Point point2 = popup.Child.PointToScreen(new Point(0.0, 0.0));
			double num;
			if ((popup.FlowDirection == FlowDirection.LeftToRight && point2.X > point.X) || (popup.FlowDirection == FlowDirection.RightToLeft && point2.X < point.X))
			{
				num = VideoOSContextMenuBaseInternals.PopupBorderThickness.Right;
				if (videoOSScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
				{
					num += 17.0;
				}
			}
			else
			{
				num = 0.0 - VideoOSContextMenuBaseInternals.PopupBorderThickness.Right;
			}
			popup.HorizontalOffset = num;
		}
		else if (eventsToCommandMappingArgs.EventName == "Closed")
		{
			((VideoOSScrollViewer)eventsToCommandMappingArgs.CommandParameter).ScrollToTop();
			popup.HorizontalOffset = 0.0;
		}
	}

	private void ClearCheckedItemsCommandFunction(object args)
	{
		foreach (object item in (IEnumerable)base.Items)
		{
			if (item is VideoOSContextMenuItem { IsChecked: not false } videoOSContextMenuItem)
			{
				IsBulkCheckedChangesInProgress = true;
				videoOSContextMenuItem.IsChecked = false;
			}
		}
		IsBulkCheckedChangesInProgress = false;
	}

	private void ItemDragInitiatedCommandFunction(object args)
	{
		EventsToCommandMappingArgs obj = (EventsToCommandMappingArgs)args;
		object commandParameter = obj.CommandParameter;
		FrameworkElement frameworkElement = (FrameworkElement)obj.Sender;
		RaiseEvent(new ItemDragInitiatedEventArgs(ItemDragInitiatedEvent, commandParameter, frameworkElement));
	}

	private void VideoOsContextMenuItem_CheckedChanged(object sender, RoutedEventArgs e)
	{
		UpdateItemsVisibilityAndControlsState();
	}

	private void HideUncheckedItemsChanged()
	{
		UpdateItemsVisibilityAndControlsState();
	}

	private void FilterTextChanged()
	{
		UpdateItemsVisibilityAndControlsState();
	}

	private void UpdateItemsVisibilityAndControlsState()
	{
		bool isEnabled = false;
		bool flag = false;
		foreach (object item in (IEnumerable)base.Items)
		{
			if (item is VideoOSContextMenuItem videoOSContextMenuItem)
			{
				bool flag2 = false;
				if (videoOSContextMenuItem.IsChecked)
				{
					isEnabled = true;
				}
				else if (HideUncheckedItems)
				{
					flag2 = true;
				}
				if (!string.IsNullOrWhiteSpace(FilterText))
				{
					flag2 = !(FilterCallback ?? DefaultFilterCallback)(FilterText, videoOSContextMenuItem);
				}
				if (flag2)
				{
					_rootScrollViewer.MinWidth = _rootScrollViewer.ActualWidth;
					videoOSContextMenuItem.Visibility = Visibility.Collapsed;
				}
				else
				{
					videoOSContextMenuItem.Visibility = Visibility.Visible;
					flag = true;
				}
			}
		}
		NoMatchingItems = !flag;
		ClearCheckedItemsCommand.IsEnabled = isEnabled;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSContextMenuBaseAutomationPeer(this);
	}
}

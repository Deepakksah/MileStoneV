using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSDropDownBase : Control
{
	public static class VideoOSDropDownBaseInternals
	{
		public const double DropDownContextMenuMinWidth = 148.0;

		public const double DropDownContextMenuMaxHeightDefault = 296.8;

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isOpen)
		{
			string resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBackgroundIdleBrush";
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBackgroundDisabledBrush";
			}
			else if (isOpen)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBackgroundFocusedBrush";
			}
			else if (isMouseOver)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBackgroundHoveredBrush";
			}
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");

		public static IMultiValueConverter CustomContentVisibilityConverter { get; } = new LambdaConverter4<IList<object>, string, DataTemplate, string>(delegate(IList<object> selectedItems, string itemContentTemplatePropertyName, DataTemplate sharedContentTemplate, string customDisplayValue)
		{
			if (customDisplayValue != null || selectedItems == null || selectedItems.Count != 1)
			{
				return Visibility.Collapsed;
			}
			object obj = selectedItems[0];
			return (obj is VideoOSDropDownItem videoOSDropDownItem) ? ((object)(((videoOSDropDownItem.ContentTemplate ?? sharedContentTemplate) == null) ? Visibility.Collapsed : Visibility.Visible)) : ((object)(((ResolveItemContentTemplate(obj, itemContentTemplatePropertyName) ?? sharedContentTemplate) == null) ? Visibility.Collapsed : Visibility.Visible));
		}, "CustomContentVisibilityConverter");
	}

	public enum SelectionModes
	{
		Single,
		Multiple
	}

	public delegate bool FilterCallbackHandler(string filterValue, object item);

	public static readonly DependencyProperty ItemsSourceProperty;

	public static readonly RoutedEvent OpenedEvent;

	public static readonly RoutedEvent ClosedEvent;

	public static readonly DependencyProperty PlaceholderTextProperty;

	public static readonly DependencyProperty SelectionModeProperty;

	public static readonly DependencyProperty UpdateSelectedItemsWhileDropDownIsOpenProperty;

	public static readonly RoutedEvent SelectedItemChangedEvent;

	public static readonly DependencyProperty SelectedItemProperty;

	public static readonly RoutedEvent SelectedItemsChangedEvent;

	public static readonly DependencyProperty SelectedItemsProperty;

	public static readonly DependencyProperty CustomDisplayValueProperty;

	public static readonly DependencyProperty IsOpenProperty;

	public static readonly DependencyProperty IsFilterControlsVisibleProperty;

	public static readonly DependencyProperty FilterCallbackProperty;

	public static readonly DependencyProperty IsCheckedControlsVisibleProperty;

	public static readonly DependencyProperty MaxDropDownHeightProperty;

	public static readonly DependencyProperty ContentTemplateProperty;

	public static readonly DependencyProperty ItemIconSourcePropertyNameProperty;

	public static readonly DependencyProperty ItemDataPropertyNameProperty;

	public static readonly DependencyProperty ItemIsEnabledPropertyNameProperty;

	public static readonly DependencyProperty ItemContentTemplatePropertyNameProperty;

	public static readonly DependencyProperty UserInteractionCommandProperty;

	private VideoOSContextMenuBase _contextMenu;

	private object _itemToSelectWhenContextMenuOpens;

	private readonly List<VideoOSContextMenuItem> _checkedWhileDropDownOpen = new List<VideoOSContextMenuItem>();

	private readonly List<VideoOSContextMenuItem> _uncheckedWhileDropDownOpen = new List<VideoOSContextMenuItem>();

	public ICollection ItemsSource
	{
		get
		{
			return (ICollection)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public string PlaceholderText
	{
		get
		{
			return (string)GetValue(PlaceholderTextProperty);
		}
		set
		{
			SetValue(PlaceholderTextProperty, value);
		}
	}

	public SelectionModes SelectionMode
	{
		get
		{
			return (SelectionModes)GetValue(SelectionModeProperty);
		}
		set
		{
			SetValue(SelectionModeProperty, value);
		}
	}

	public bool UpdateSelectedItemsWhileDropDownIsOpen
	{
		get
		{
			return (bool)GetValue(UpdateSelectedItemsWhileDropDownIsOpenProperty);
		}
		set
		{
			SetValue(UpdateSelectedItemsWhileDropDownIsOpenProperty, value);
		}
	}

	public object SelectedItem
	{
		get
		{
			return GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public IList<object> SelectedItems
	{
		get
		{
			return (IList<object>)GetValue(SelectedItemsProperty);
		}
		set
		{
			SetValue(SelectedItemsProperty, value);
		}
	}

	public string CustomDisplayValue
	{
		get
		{
			return (string)GetValue(CustomDisplayValueProperty);
		}
		set
		{
			SetValue(CustomDisplayValueProperty, value);
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

	public double MaxDropDownHeight
	{
		get
		{
			return (double)GetValue(MaxDropDownHeightProperty);
		}
		set
		{
			SetValue(MaxDropDownHeightProperty, value);
		}
	}

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

	public string ItemIconSourcePropertyName
	{
		get
		{
			return (string)GetValue(ItemIconSourcePropertyNameProperty);
		}
		set
		{
			SetValue(ItemIconSourcePropertyNameProperty, value);
		}
	}

	public string ItemDataPropertyName
	{
		get
		{
			return (string)GetValue(ItemDataPropertyNameProperty);
		}
		set
		{
			SetValue(ItemDataPropertyNameProperty, value);
		}
	}

	public string ItemIsEnabledPropertyName
	{
		get
		{
			return (string)GetValue(ItemIsEnabledPropertyNameProperty);
		}
		set
		{
			SetValue(ItemIsEnabledPropertyNameProperty, value);
		}
	}

	public string ItemContentTemplatePropertyName
	{
		get
		{
			return (string)GetValue(ItemContentTemplatePropertyNameProperty);
		}
		set
		{
			SetValue(ItemContentTemplatePropertyNameProperty, value);
		}
	}

	public DelegateCommand UserInteractionCommand
	{
		get
		{
			return (DelegateCommand)GetValue(UserInteractionCommandProperty);
		}
		private set
		{
			SetValue(UserInteractionCommandProperty, value);
		}
	}

	public virtual double? ContextMenuDesiredWidth => base.ActualWidth;

	public virtual UIElement ContextMenuPlacementTarget => this;

	public virtual double? ContextMenuVerticalOffset => null;

	private FilterCallbackHandler DefaultFilterCallback => delegate(string filterValue, object item)
	{
		string input = ResolveItemData(item).ToString() ?? string.Empty;
		string pattern = filterValue.Replace("*", ".*");
		return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
	};

	public event RoutedEventHandler Opened
	{
		add
		{
			AddHandler(OpenedEvent, value);
		}
		remove
		{
			RemoveHandler(OpenedEvent, value);
		}
	}

	public event RoutedEventHandler Closed
	{
		add
		{
			AddHandler(ClosedEvent, value);
		}
		remove
		{
			RemoveHandler(ClosedEvent, value);
		}
	}

	public event RoutedEventHandler SelectedItemChanged
	{
		add
		{
			AddHandler(SelectedItemChangedEvent, value);
		}
		remove
		{
			RemoveHandler(SelectedItemChangedEvent, value);
		}
	}

	public event RoutedEventHandler SelectedItemsChanged
	{
		add
		{
			AddHandler(SelectedItemsChangedEvent, value);
		}
		remove
		{
			RemoveHandler(SelectedItemsChangedEvent, value);
		}
	}

	static VideoOSDropDownBase()
	{
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(VideoOSDropDownBase), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));
		OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSDropDownBase));
		ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSDropDownBase));
		PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionModes), typeof(VideoOSDropDownBase), new PropertyMetadata(SelectionModes.Single, SelectionModeChanged));
		UpdateSelectedItemsWhileDropDownIsOpenProperty = DependencyProperty.Register("UpdateSelectedItemsWhileDropDownIsOpen", typeof(bool), typeof(VideoOSDropDownBase), new PropertyMetadata(true));
		SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSDropDownBase));
		SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(VideoOSDropDownBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));
		SelectedItemsChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemsChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSDropDownBase));
		SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList<object>), typeof(VideoOSDropDownBase), new PropertyMetadata(null, OnSelectedItemsChanged));
		CustomDisplayValueProperty = DependencyProperty.Register("CustomDisplayValue", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata((object)null));
		IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(VideoOSDropDownBase), new PropertyMetadata(false, IsOpenChanged));
		IsFilterControlsVisibleProperty = DependencyProperty.Register("IsFilterControlsVisible", typeof(bool), typeof(VideoOSDropDownBase), new PropertyMetadata(false));
		FilterCallbackProperty = DependencyProperty.Register("FilterCallback", typeof(FilterCallbackHandler), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		IsCheckedControlsVisibleProperty = DependencyProperty.Register("IsCheckedControlsVisible", typeof(bool), typeof(VideoOSDropDownBase), new PropertyMetadata(false));
		MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(VideoOSDropDownBase), new PropertyMetadata(296.8));
		ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		ItemIconSourcePropertyNameProperty = DependencyProperty.Register("ItemIconSourcePropertyName", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		ItemDataPropertyNameProperty = DependencyProperty.Register("ItemDataPropertyName", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		ItemIsEnabledPropertyNameProperty = DependencyProperty.Register("ItemIsEnabledPropertyName", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		ItemContentTemplatePropertyNameProperty = DependencyProperty.Register("ItemContentTemplatePropertyName", typeof(string), typeof(VideoOSDropDownBase), new PropertyMetadata(null));
		UserInteractionCommandProperty = DependencyProperty.Register("UserInteractionCommand", typeof(DelegateCommand), typeof(VideoOSDropDownBase), new PropertyMetadata((object)null));
		FrameworkElement.CursorProperty.OverrideMetadata(typeof(VideoOSDropDownBase), new FrameworkPropertyMetadata(Cursors.Hand));
	}

	private static void SelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSDropDownBase videoOSDropDownBase = (VideoOSDropDownBase)d;
		if (videoOSDropDownBase._contextMenu != null && videoOSDropDownBase._contextMenu.IsOpen)
		{
			throw new InvalidOperationException("Modifying the SelectionMode property while the drop down is open is not allowed.");
		}
		if (videoOSDropDownBase.SelectionMode == SelectionModes.Single && videoOSDropDownBase.SelectedItem != null)
		{
			videoOSDropDownBase.SetCurrentValue(SelectedItemsProperty, new List<object> { videoOSDropDownBase.SelectedItem });
		}
	}

	private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSDropDownBase videoOSDropDownBase = (VideoOSDropDownBase)d;
		if (videoOSDropDownBase.SelectedItem != videoOSDropDownBase.SelectedItems?.FirstOrDefault())
		{
			videoOSDropDownBase.SetCurrentValue(SelectedItemsProperty, (videoOSDropDownBase.SelectedItem == null) ? null : new List<object> { videoOSDropDownBase.SelectedItem });
		}
		videoOSDropDownBase.RaiseEvent(new RoutedEventArgs(SelectedItemChangedEvent));
	}

	private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSDropDownBase videoOSDropDownBase = (VideoOSDropDownBase)d;
		object obj = videoOSDropDownBase.SelectedItems?.FirstOrDefault();
		if (!object.Equals(videoOSDropDownBase.SelectedItem, obj))
		{
			videoOSDropDownBase.SetCurrentValue(SelectedItemProperty, obj);
		}
		videoOSDropDownBase.RaiseEvent(new RoutedEventArgs(SelectedItemsChangedEvent));
	}

	private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSDropDownBase videoOSDropDownBase = (VideoOSDropDownBase)d;
		if (!(e.NewValue is bool flag))
		{
			return;
		}
		videoOSDropDownBase.RaiseEvent(flag ? new RoutedEventArgs(OpenedEvent) : new RoutedEventArgs(ClosedEvent));
		if (flag)
		{
			if (videoOSDropDownBase.ItemsSource == null || videoOSDropDownBase.ItemsSource.Count == 0)
			{
				videoOSDropDownBase.IsOpen = false;
				return;
			}
			videoOSDropDownBase.Focus();
			videoOSDropDownBase.OpenContextMenu();
		}
		else
		{
			videoOSDropDownBase.CloseContextMenu();
		}
	}

	public VideoOSDropDownBase()
	{
		CollectionHelper.Initialize(this, ItemsSourceProperty, ItemsSourceChanged);
		UserInteractionCommand = new DelegateCommand(OnUserInteraction);
	}

	private void ItemsSourceChanged(CollectionChangedInfo info)
	{
		if (_contextMenu != null && _contextMenu.IsOpen)
		{
			throw new InvalidOperationException("Modifying the ItemsSource property while the drop down is open is not allowed.");
		}
		switch (info.Change)
		{
		case CollectionChange.NewOrResetCollection:
		case CollectionChange.Remove:
		case CollectionChange.Replace:
		{
			if (ItemsSource == null)
			{
				SetCurrentValue(SelectedItemsProperty, null);
				break;
			}
			HashSet<object> hashSet = new HashSet<object>(ItemsSource.OfType<object>());
			SetCurrentValue(SelectedItemsProperty, (ItemsSource == null) ? null : SelectedItems?.Where(hashSet.Contains).ToList());
			break;
		}
		default:
			throw new ArgumentOutOfRangeException("Change", info.Change, null);
		case CollectionChange.HostLoaded:
		case CollectionChange.HostUnloaded:
		case CollectionChange.Add:
		case CollectionChange.Move:
			break;
		}
	}

	private VideoOSIconSourceBase ResolveItemIcon(object item)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.IconSource;
		}
		return item.ResolvePropertyValue<VideoOSIconSourceBase>(ItemIconSourcePropertyName, throwOnError: false);
	}

	private object ResolveItemData(object item)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.Data;
		}
		return item.ResolvePropertyValue(ItemDataPropertyName, throwOnError: false, item);
	}

	public bool IsUsingCustomTemplate(object item)
	{
		return (ContentTemplate ?? ResolveItemContentTemplate(item, ItemContentTemplatePropertyName)) != null;
	}

	private static DataTemplate ResolveItemContentTemplate(object item, string propertyName)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.ContentTemplate;
		}
		return item.ResolvePropertyValue<DataTemplate>(propertyName, throwOnError: false);
	}

	private bool ResolveItemIsEnabled(object item)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.IsEnabled;
		}
		return item.ResolvePropertyValue(ItemIsEnabledPropertyName, throwOnError: false, defaultValue: true);
	}

	public virtual VideoOSContextMenuBase CreateContextMenu()
	{
		return new VideoOSContextMenuLarge();
	}

	private void OnUserInteraction(object parameter)
	{
		EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)parameter;
		string eventName = eventsToCommandMappingArgs.EventName;
		if (!(eventName == "KeyDown"))
		{
			if (eventName == "MouseDown")
			{
				if (SelectionMode == SelectionModes.Single)
				{
					_itemToSelectWhenContextMenuOpens = SelectedItem;
				}
				else if (SelectionMode == SelectionModes.Multiple)
				{
					_itemToSelectWhenContextMenuOpens = SelectedItems?.FirstOrDefault();
				}
				IsOpen = true;
				((RoutedEventArgs)eventsToCommandMappingArgs.OriginalEventArgs).Handled = true;
			}
		}
		else
		{
			HandleKeyDown(eventsToCommandMappingArgs);
		}
	}

	private void HandleKeyDown(EventsToCommandMappingArgs args)
	{
		KeyEventArgs e = (KeyEventArgs)args.OriginalEventArgs;
		Key key = e.Key;
		if (key == Key.System)
		{
			key = e.SystemKey;
		}
		switch (key)
		{
		case Key.Return:
			e.Handled = true;
			if (SelectionMode == SelectionModes.Single)
			{
				_itemToSelectWhenContextMenuOpens = SelectedItem;
			}
			else if (SelectionMode == SelectionModes.Multiple)
			{
				_itemToSelectWhenContextMenuOpens = SelectedItems?.FirstOrDefault();
			}
			IsOpen = true;
			break;
		case Key.Down:
			e.Handled = true;
			HandleDownKeyPressed(e);
			break;
		case Key.Up:
			e.Handled = true;
			HandleUpKeyPressed(e);
			break;
		}
	}

	private void HandleDownKeyPressed(KeyEventArgs keyEventArgs)
	{
		if (SelectionMode == SelectionModes.Single)
		{
			if (keyEventArgs.KeyboardDevice.IsKeyDown(Key.LeftAlt) || keyEventArgs.KeyboardDevice.IsKeyDown(Key.RightAlt))
			{
				_itemToSelectWhenContextMenuOpens = SelectedItem;
				IsOpen = true;
				return;
			}
			List<object> list = new List<object>(ItemsSource.OfType<object>());
			if (SelectedItem == null)
			{
				SetCurrentValue(SelectedItemProperty, list.FirstOrDefault());
				return;
			}
			int num = list.IndexOf(SelectedItem);
			if (num < list.Count - 1)
			{
				SetCurrentValue(SelectedItemProperty, list[num + 1]);
			}
		}
		else
		{
			object itemToSelectWhenContextMenuOpens = SelectedItems?.FirstOrDefault() ?? ItemsSource?.OfType<object>().FirstOrDefault();
			_itemToSelectWhenContextMenuOpens = itemToSelectWhenContextMenuOpens;
			IsOpen = true;
		}
	}

	private void HandleUpKeyPressed(KeyEventArgs keyEventArgs)
	{
		if (SelectionMode == SelectionModes.Single)
		{
			if (keyEventArgs.KeyboardDevice.IsKeyDown(Key.LeftAlt) || keyEventArgs.KeyboardDevice.IsKeyDown(Key.RightAlt))
			{
				return;
			}
			List<object> list = new List<object>(ItemsSource.OfType<object>());
			if (SelectedItem != null)
			{
				int num = list.IndexOf(SelectedItem);
				if (num > 0)
				{
					SetCurrentValue(SelectedItemProperty, list[num - 1]);
				}
			}
		}
		else
		{
			object itemToSelectWhenContextMenuOpens = SelectedItems?.LastOrDefault() ?? ItemsSource?.OfType<object>().LastOrDefault();
			_itemToSelectWhenContextMenuOpens = itemToSelectWhenContextMenuOpens;
			IsOpen = true;
		}
	}

	private void OpenContextMenu()
	{
		if (_contextMenu != null)
		{
			CleanupContextMenu();
		}
		if (ItemsSource == null || !ItemsSource.OfType<object>().Any())
		{
			return;
		}
		_contextMenu = CreateContextMenu();
		_contextMenu.ContentTemplate = ContentTemplate;
		_contextMenu.FilterCallback = (string filterValue, VideoOSContextMenuItem item) => (FilterCallback ?? DefaultFilterCallback)(filterValue, item.Tag);
		_contextMenu.Tag = _contextMenu.ResolveDropShadowThickness();
		HashSet<object> hashSet = new HashSet<object>();
		if (SelectionMode == SelectionModes.Single && SelectedItem != null)
		{
			hashSet.Add(SelectedItem);
		}
		if (SelectionMode == SelectionModes.Multiple && SelectedItems != null)
		{
			foreach (object selectedItem in SelectedItems)
			{
				hashSet.Add(selectedItem);
			}
		}
		List<VideoOSContextMenuItem> list = new List<VideoOSContextMenuItem>();
		foreach (object item in ItemsSource)
		{
			if (item is VideoOSDropDownItem videoOSDropDownItem)
			{
				list.Add(AddEventHandlers(videoOSDropDownItem.AsContextMenuItem(SelectionMode == SelectionModes.Multiple, hashSet.Contains(item))));
				continue;
			}
			bool flag = SelectionMode == SelectionModes.Multiple;
			VideoOSContextMenuItem contextMenuItem = new VideoOSContextMenuItem
			{
				IconSource = ResolveItemIcon(item),
				Data = ResolveItemData(item),
				ContentTemplate = ResolveItemContentTemplate(item, ItemContentTemplatePropertyName),
				IsCheckable = flag,
				IsChecked = hashSet.Contains(item),
				IsEnabled = ResolveItemIsEnabled(item),
				StaysOpenOnClick = flag,
				CheckedAppearance = (flag ? VideoOSContextMenuItem.CheckedAppearances.Marked : VideoOSContextMenuItem.CheckedAppearances.Highlighted),
				Tag = item
			};
			list.Add(AddEventHandlers(contextMenuItem));
		}
		_contextMenu.ItemsSource = list;
		_contextMenu.MaxHeight = MaxDropDownHeight;
		_contextMenu.MinWidth = 148.0;
		if (ContextMenuDesiredWidth.HasValue)
		{
			VideoOSContextMenuBase contextMenu = _contextMenu;
			double maxWidth = (_contextMenu.Width = Math.Max(_contextMenu.MinWidth, ContextMenuDesiredWidth.Value + ((Thickness)_contextMenu.Tag).Left + ((Thickness)_contextMenu.Tag).Right));
			contextMenu.MaxWidth = maxWidth;
		}
		if (ContextMenuVerticalOffset.HasValue)
		{
			_contextMenu.VerticalOffset = ContextMenuVerticalOffset.Value;
		}
		_contextMenu.Placement = PlacementMode.Custom;
		_contextMenu.PlacementTarget = this;
		_contextMenu.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
		_contextMenu.StaysOpen = false;
		_contextMenu.IsFilterControlsVisible = IsFilterControlsVisible;
		_contextMenu.IsCheckedControlsVisible = IsCheckedControlsVisible;
		_contextMenu.Opened += ContextMenuOnOpened;
		_contextMenu.Closed += ContextMenuOnClosed;
		_contextMenu.IsBulkCheckedChangesInProgressChanged += ContextMenuOnIsBulkCheckedChangesInProgressChanged;
		_contextMenu.IsOpen = true;
	}

	private void CloseContextMenu()
	{
		if (_contextMenu != null && _contextMenu.IsOpen)
		{
			_contextMenu.IsOpen = false;
		}
	}

	private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
	{
		CustomPopupVerticalPlacementResult customPopupVerticalPlacementResult = PopupUtils.CustomPopupVerticalPlacementCallback(popupSize, targetSize, offset, _contextMenu.PlacementTarget, PopupUtils.CustomVerticalPlacement.Bottom, PopupUtils.CustomPlacementAnchoring.Begin, (Thickness)_contextMenu.Tag);
		if (customPopupVerticalPlacementResult.SuggestedMaxHeight.HasValue && customPopupVerticalPlacementResult.SuggestedMaxHeight.Value < _contextMenu.MaxHeight)
		{
			_contextMenu.MaxHeight = customPopupVerticalPlacementResult.SuggestedMaxHeight.Value;
		}
		return customPopupVerticalPlacementResult.Placements;
	}

	private void CleanupContextMenu()
	{
		_contextMenu.Closed -= ContextMenuOnClosed;
		_contextMenu.IsBulkCheckedChangesInProgressChanged -= ContextMenuOnIsBulkCheckedChangesInProgressChanged;
		foreach (VideoOSContextMenuItem item in (IEnumerable)_contextMenu.Items)
		{
			RemoveEventHandlers(item);
		}
		_contextMenu = null;
	}

	private VideoOSContextMenuItem AddEventHandlers(VideoOSContextMenuItem contextMenuItem)
	{
		contextMenuItem.Checked += ContextMenuItemOnChecked;
		contextMenuItem.Unchecked += ContextMenuItemOnUnchecked;
		contextMenuItem.Click += ContextMenuItemOnClick;
		return contextMenuItem;
	}

	private void RemoveEventHandlers(VideoOSContextMenuItem contextMenuItem)
	{
		contextMenuItem.Checked -= ContextMenuItemOnChecked;
		contextMenuItem.Unchecked -= ContextMenuItemOnUnchecked;
		contextMenuItem.Click -= ContextMenuItemOnClick;
	}

	private void ContextMenuOnOpened(object sender, RoutedEventArgs e)
	{
		if (_itemToSelectWhenContextMenuOpens != null)
		{
			foreach (VideoOSContextMenuItem item in (IEnumerable)_contextMenu.Items)
			{
				if (item.Tag == _itemToSelectWhenContextMenuOpens)
				{
					item.Focus();
					break;
				}
			}
			_itemToSelectWhenContextMenuOpens = null;
		}
		_contextMenu.Opened -= ContextMenuOnOpened;
	}

	private void ContextMenuOnClosed(object sender, RoutedEventArgs e)
	{
		if (SelectionMode == SelectionModes.Multiple && !UpdateSelectedItemsWhileDropDownIsOpen)
		{
			UpdateSelectedItems();
		}
		_checkedWhileDropDownOpen.Clear();
		_uncheckedWhileDropDownOpen.Clear();
		CleanupContextMenu();
		IsOpen = false;
	}

	private void ContextMenuOnIsBulkCheckedChangesInProgressChanged(object sender, RoutedEventArgs e)
	{
		VideoOSContextMenuBase videoOSContextMenuBase = (VideoOSContextMenuBase)sender;
		if (UpdateSelectedItemsWhileDropDownIsOpen && !videoOSContextMenuBase.IsBulkCheckedChangesInProgress)
		{
			UpdateSelectedItems();
		}
	}

	private void UpdateSelectedItems()
	{
		IEnumerable<object> selectedItems = SelectedItems;
		List<object> list = new List<object>(selectedItems ?? Enumerable.Empty<object>());
		HashSet<object> hashSet = new HashSet<object>(_uncheckedWhileDropDownOpen.Select((VideoOSContextMenuItem contextMenuItem) => contextMenuItem.Tag));
		IEnumerable<object> collection = _checkedWhileDropDownOpen.Select((VideoOSContextMenuItem contextMenuItem) => contextMenuItem.Tag);
		list.RemoveAll(hashSet.Contains);
		list.AddRange(collection);
		SetCurrentValue(SelectedItemsProperty, list.Any() ? list : null);
	}

	private void ContextMenuItemOnChecked(object sender, RoutedEventArgs e)
	{
		VideoOSContextMenuItem videoOSContextMenuItem = (VideoOSContextMenuItem)sender;
		if (!UpdateSelectedItemsWhileDropDownIsOpen || _contextMenu.IsBulkCheckedChangesInProgress)
		{
			_uncheckedWhileDropDownOpen.Remove(videoOSContextMenuItem);
			_checkedWhileDropDownOpen.Add(videoOSContextMenuItem);
			return;
		}
		object tag = videoOSContextMenuItem.Tag;
		IEnumerable<object> selectedItems = SelectedItems;
		List<object> list = new List<object>(selectedItems ?? Enumerable.Empty<object>());
		list.Add(tag);
		SetCurrentValue(SelectedItemsProperty, list);
	}

	private void ContextMenuItemOnUnchecked(object sender, RoutedEventArgs e)
	{
		VideoOSContextMenuItem videoOSContextMenuItem = (VideoOSContextMenuItem)sender;
		if (!UpdateSelectedItemsWhileDropDownIsOpen || _contextMenu.IsBulkCheckedChangesInProgress)
		{
			_checkedWhileDropDownOpen.Remove(videoOSContextMenuItem);
			_uncheckedWhileDropDownOpen.Add(videoOSContextMenuItem);
			return;
		}
		object tag = videoOSContextMenuItem.Tag;
		IEnumerable<object> selectedItems = SelectedItems;
		List<object> list = new List<object>(selectedItems ?? Enumerable.Empty<object>());
		list.Remove(tag);
		SetCurrentValue(SelectedItemsProperty, list.Any() ? list : null);
	}

	private void ContextMenuItemOnClick(object sender, RoutedEventArgs e)
	{
		object tag = ((VideoOSContextMenuItem)sender).Tag;
		if (SelectionMode == SelectionModes.Single)
		{
			SetCurrentValue(SelectedItemProperty, tag);
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSDropDownBaseAutomationPeer(this);
	}
}

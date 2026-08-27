using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTabControlBase : Control
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

	[Flags]
	public enum ItemSeparatorPlacements
	{
		None = 0,
		Between = 2,
		BeforeFirst = 4,
		AfterLast = 8,
		All = 0xE
	}

	public enum ItemMouseSelectionModes
	{
		LeftButtonDown,
		LeftButtonUp
	}

	public static readonly RoutedEvent SelectedItemChangedEvent;

	public static readonly RoutedEvent ItemDragInitiatedEvent;

	public static readonly DependencyProperty ItemsSourceProperty;

	public static readonly DependencyProperty SelectedItemProperty;

	public static readonly DependencyProperty ContentTemplateProperty;

	public static readonly DependencyProperty ItemTextPropertyNameProperty;

	public static readonly DependencyProperty ItemToolTipPropertyNameProperty;

	public static readonly DependencyProperty ItemContentTemplatePropertyNameProperty;

	public static readonly DependencyProperty ItemDataPropertyNameProperty;

	public static readonly DependencyProperty ItemIsEnabledPropertyNameProperty;

	public static readonly DependencyProperty ItemIsDraggablePropertyNameProperty;

	public static readonly DependencyProperty ItemAutomationIdPropertyNameProperty;

	public static readonly DependencyProperty IsContentVisibleProperty;

	public static readonly DependencyProperty IsContentBorderVisibleProperty;

	public static readonly DependencyProperty ItemSeparatorPlacementProperty;

	public static readonly DependencyProperty ItemMouseSelectionModeProperty;

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

	public string ItemTextPropertyName
	{
		get
		{
			return (string)GetValue(ItemTextPropertyNameProperty);
		}
		set
		{
			SetValue(ItemTextPropertyNameProperty, value);
		}
	}

	public string ItemToolTipPropertyName
	{
		get
		{
			return (string)GetValue(ItemToolTipPropertyNameProperty);
		}
		set
		{
			SetValue(ItemToolTipPropertyNameProperty, value);
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

	public string ItemIsDraggablePropertyName
	{
		get
		{
			return (string)GetValue(ItemIsDraggablePropertyNameProperty);
		}
		set
		{
			SetValue(ItemIsDraggablePropertyNameProperty, value);
		}
	}

	public string ItemAutomationIdPropertyName
	{
		get
		{
			return (string)GetValue(ItemAutomationIdPropertyNameProperty);
		}
		set
		{
			SetValue(ItemAutomationIdPropertyNameProperty, value);
		}
	}

	public bool IsContentVisible
	{
		get
		{
			return (bool)GetValue(IsContentVisibleProperty);
		}
		set
		{
			SetValue(IsContentVisibleProperty, value);
		}
	}

	public bool IsContentBorderVisible
	{
		get
		{
			return (bool)GetValue(IsContentBorderVisibleProperty);
		}
		set
		{
			SetValue(IsContentBorderVisibleProperty, value);
		}
	}

	public ItemSeparatorPlacements ItemSeparatorPlacement
	{
		get
		{
			return (ItemSeparatorPlacements)GetValue(ItemSeparatorPlacementProperty);
		}
		set
		{
			SetValue(ItemSeparatorPlacementProperty, value);
		}
	}

	public ItemMouseSelectionModes ItemMouseSelectionMode
	{
		get
		{
			return (ItemMouseSelectionModes)GetValue(ItemMouseSelectionModeProperty);
		}
		set
		{
			SetValue(ItemMouseSelectionModeProperty, value);
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

	static VideoOSTabControlBase()
	{
		SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSTabControlBase));
		ItemDragInitiatedEvent = EventManager.RegisterRoutedEvent("ItemDragInitiated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSTabControlBase));
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(VideoOSTabControlBase), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));
		SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(VideoOSTabControlBase), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTabControlBase)o).RaiseEvent(new RoutedEventArgs(SelectedItemChangedEvent));
		}));
		ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemTextPropertyNameProperty = DependencyProperty.Register("ItemTextPropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemToolTipPropertyNameProperty = DependencyProperty.Register("ItemToolTipPropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemContentTemplatePropertyNameProperty = DependencyProperty.Register("ItemContentTemplatePropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemDataPropertyNameProperty = DependencyProperty.Register("ItemDataPropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemIsEnabledPropertyNameProperty = DependencyProperty.Register("ItemIsEnabledPropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemIsDraggablePropertyNameProperty = DependencyProperty.Register("ItemIsDraggablePropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		ItemAutomationIdPropertyNameProperty = DependencyProperty.Register("ItemAutomationIdPropertyName", typeof(string), typeof(VideoOSTabControlBase), new PropertyMetadata(null));
		IsContentVisibleProperty = DependencyProperty.Register("IsContentVisible", typeof(bool), typeof(VideoOSTabControlBase), new PropertyMetadata(true));
		IsContentBorderVisibleProperty = DependencyProperty.Register("IsContentBorderVisible", typeof(bool), typeof(VideoOSTabControlBase), new PropertyMetadata(true));
		ItemSeparatorPlacementProperty = DependencyProperty.Register("ItemSeparatorPlacement", typeof(ItemSeparatorPlacements), typeof(VideoOSTabControlBase), new PropertyMetadata(ItemSeparatorPlacements.Between));
		ItemMouseSelectionModeProperty = DependencyProperty.Register("ItemMouseSelectionMode", typeof(ItemMouseSelectionModes), typeof(VideoOSTabControlBase), new PropertyMetadata(ItemMouseSelectionModes.LeftButtonDown));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSTabControlBase), new FrameworkPropertyMetadata(false));
		UIElement.FocusableProperty.OverrideMetadata(typeof(VideoOSTabControlBase), new FrameworkPropertyMetadata(false));
	}

	public VideoOSTabControlBase()
	{
		CollectionHelper.Initialize(this, ItemsSourceProperty, ItemsCollectionChanged);
	}

	private void ItemsCollectionChanged(CollectionChangedInfo obj)
	{
		if (SelectedItem == null)
		{
			return;
		}
		bool flag = false;
		if (ItemsSource != null)
		{
			foreach (object item in ItemsSource)
			{
				if (item == SelectedItem)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			SelectedItem = null;
		}
	}
}

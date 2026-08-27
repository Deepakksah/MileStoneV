using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSMenuButtonBase : Control
{
	public static class VideoOSMenuButtonBaseInternals
	{
		public static IMultiValueConverter ContentIsEnabledConverter { get; } = new LambdaConverter3<ICollection<VideoOSActionBase>, DataTemplate, DataTemplate>((ICollection<VideoOSActionBase> actions, DataTemplate headerTemplate, DataTemplate footerTemplate) => (actions != null && actions.Count > 0) || headerTemplate != null || footerTemplate != null, "ContentIsEnabledConverter");
	}

	public enum MenuSizes
	{
		Large,
		Small
	}

	public static readonly DependencyProperty IconSourceProperty;

	public static readonly DependencyProperty ContentProperty;

	public static readonly DependencyProperty ActionsProperty;

	public static readonly RoutedEvent OpenedEvent;

	public static readonly RoutedEvent ClosedEvent;

	public static readonly DependencyProperty IsOpenProperty;

	public static readonly DependencyProperty IsMenuIndicatorIconVisibleProperty;

	public static readonly DependencyProperty MenuHeaderTemplateProperty;

	public static readonly DependencyProperty MenuFooterTemplateProperty;

	public static readonly DependencyProperty OpenMenuCommandProperty;

	private VideoOSContextMenuBase _contextMenu;

	private VideoOSButtonBase _activeButton;

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(IconSourceProperty);
		}
		set
		{
			SetValue(IconSourceProperty, value);
		}
	}

	public string Content
	{
		get
		{
			return (string)GetValue(ContentProperty);
		}
		set
		{
			SetValue(ContentProperty, value);
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return (ICollection<VideoOSActionBase>)GetValue(ActionsProperty);
		}
		set
		{
			SetValue(ActionsProperty, value);
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

	public bool IsMenuIndicatorIconVisible
	{
		get
		{
			return (bool)GetValue(IsMenuIndicatorIconVisibleProperty);
		}
		set
		{
			SetValue(IsMenuIndicatorIconVisibleProperty, value);
		}
	}

	public DataTemplate MenuHeaderTemplate
	{
		get
		{
			return (DataTemplate)GetValue(MenuHeaderTemplateProperty);
		}
		set
		{
			SetValue(MenuHeaderTemplateProperty, value);
		}
	}

	public DataTemplate MenuFooterTemplate
	{
		get
		{
			return (DataTemplate)GetValue(MenuFooterTemplateProperty);
		}
		set
		{
			SetValue(MenuFooterTemplateProperty, value);
		}
	}

	public DelegateCommand OpenMenuCommand
	{
		get
		{
			return (DelegateCommand)GetValue(OpenMenuCommandProperty);
		}
		private set
		{
			SetValue(OpenMenuCommandProperty, value);
		}
	}

	public abstract MenuSizes MenuSize { get; }

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

	static VideoOSMenuButtonBase()
	{
		IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSMenuButtonBase), new PropertyMetadata(null));
		ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(VideoOSMenuButtonBase), new PropertyMetadata((object)null));
		ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(VideoOSMenuButtonBase), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));
		OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSMenuButtonBase));
		ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSMenuButtonBase));
		IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(VideoOSMenuButtonBase), new PropertyMetadata(false, IsOpenChangedCallback));
		IsMenuIndicatorIconVisibleProperty = DependencyProperty.Register("IsMenuIndicatorIconVisible", typeof(bool), typeof(VideoOSMenuButtonBase), new PropertyMetadata(true));
		MenuHeaderTemplateProperty = DependencyProperty.Register("MenuHeaderTemplate", typeof(DataTemplate), typeof(VideoOSMenuButtonBase), new PropertyMetadata(null));
		MenuFooterTemplateProperty = DependencyProperty.Register("MenuFooterTemplate", typeof(DataTemplate), typeof(VideoOSMenuButtonBase), new PropertyMetadata(null));
		OpenMenuCommandProperty = DependencyProperty.Register("OpenMenuCommand", typeof(DelegateCommand), typeof(VideoOSMenuButtonBase), new PropertyMetadata((object)null));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSMenuButtonBase), new FrameworkPropertyMetadata(false));
	}

	private static void IsOpenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSMenuButtonBase videoOSMenuButtonBase = (VideoOSMenuButtonBase)d;
		if (e.NewValue is bool flag)
		{
			videoOSMenuButtonBase.RaiseEvent(flag ? new RoutedEventArgs(OpenedEvent) : new RoutedEventArgs(ClosedEvent));
			if (flag)
			{
				VideoOSButtonBase activator = videoOSMenuButtonBase.FindDescendant<VideoOSButtonBase>();
				videoOSMenuButtonBase.CreateAndOpenMenu(activator);
			}
			else
			{
				videoOSMenuButtonBase.CloseMenu();
			}
		}
	}

	public VideoOSMenuButtonBase()
	{
		CollectionHelper<VideoOSActionBase>.Initialize(this, ActionsProperty, null);
		OpenMenuCommand = new DelegateCommand(OpenMenu);
	}

	private void OpenMenu(object parameter)
	{
		SetCurrentValue(IsOpenProperty, true);
	}

	private void CloseMenu()
	{
		if (_contextMenu != null && _contextMenu.IsOpen)
		{
			_contextMenu.IsOpen = false;
		}
	}

	private void CreateAndOpenMenu(VideoOSButtonBase activator)
	{
		_contextMenu = CreateMenu();
		if (_contextMenu.HasItems || MenuHeaderTemplate != null || MenuFooterTemplate != null)
		{
			OnMenuOpening(_contextMenu, activator);
			_contextMenu.IsOpen = true;
		}
		else
		{
			SetCurrentValue(IsOpenProperty, false);
		}
	}

	private VideoOSContextMenuBase CreateMenu()
	{
		VideoOSContextMenuBase videoOSContextMenuBase = MenuSize switch
		{
			MenuSizes.Large => new VideoOSContextMenuLarge(), 
			MenuSizes.Small => new VideoOSContextMenuSmall(), 
			_ => throw new ArgumentException("Invalid menu size"), 
		};
		videoOSContextMenuBase.DataContext = base.DataContext;
		videoOSContextMenuBase.HeaderTemplate = MenuHeaderTemplate;
		videoOSContextMenuBase.FooterTemplate = MenuFooterTemplate;
		videoOSContextMenuBase.HorizontalContentAlignment = base.HorizontalContentAlignment;
		IEnumerable<VideoOSActionBase> actions = Actions;
		foreach (VideoOSActionBase item in actions ?? Enumerable.Empty<VideoOSActionBase>())
		{
			object newItem = item.AsContextMenuItem();
			videoOSContextMenuBase.Items.Add(newItem);
		}
		videoOSContextMenuBase.PlacementTarget = this;
		videoOSContextMenuBase.Placement = PlacementMode.Bottom;
		videoOSContextMenuBase.StaysOpen = false;
		AutomationProperties.SetAutomationId(videoOSContextMenuBase, "VideoOSMenuButtonBase_ContextMenu");
		return videoOSContextMenuBase;
	}

	private static void TrimSeparators(List<object> contextMenuItems)
	{
		HashSet<object> hashSet = new HashSet<object>();
		foreach (object contextMenuItem in contextMenuItems)
		{
			if (contextMenuItem is Separator)
			{
				hashSet.Add(contextMenuItem);
				continue;
			}
			break;
		}
		for (int num = contextMenuItems.Count - 1; num >= 0; num--)
		{
			object obj = contextMenuItems[num];
			if (!(obj is Separator))
			{
				break;
			}
			hashSet.Add(obj);
		}
		foreach (object item in hashSet)
		{
			contextMenuItems.Remove(item);
		}
	}

	private void OnMenuOpening(VideoOSContextMenuBase menu, object activator)
	{
		if (menu != null && activator is VideoOSButtonBase activeButton)
		{
			_activeButton = activeButton;
			_activeButton.ForcePressedState = true;
			menu.Unloaded += OnMenuOnUnloaded;
		}
	}

	private void OnMenuOnUnloaded(object sender, RoutedEventArgs e)
	{
		VideoOSContextMenuBase obj = (VideoOSContextMenuBase)sender;
		_activeButton.ForcePressedState = false;
		_activeButton = null;
		obj.Unloaded -= OnMenuOnUnloaded;
		SetCurrentValue(IsOpenProperty, false);
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSMenuButtonBaseAutomationPeer(this);
	}
}

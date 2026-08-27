using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Controls.Popups;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

[ContentProperty("Child")]
public sealed class VideoOSPopover : FrameworkElement, IAddChild
{
	public enum Appearances
	{
		PrimaryDark,
		PrimaryLight,
		Accent,
		Neutral
	}

	public enum TextAlignments
	{
		Left,
		Center,
		Right
	}

	public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSPopover));

	public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSPopover));

	public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(VideoOSPopover), new FrameworkPropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnChildChanged();
	}));

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(VideoOSPopover), new FrameworkPropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnTextChanged();
	}));

	public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignments), typeof(VideoOSPopover), new FrameworkPropertyMetadata(TextAlignments.Left, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnTextAlignmentChanged();
	}));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(VideoOSPopover), new FrameworkPropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnIsOpenChanged();
	}));

	public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(VideoOSPopover), new FrameworkPropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnStaysOpenChanged();
	}));

	public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(FrameworkElement), typeof(VideoOSPopover), new FrameworkPropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnPlacementTargetChanged();
	}));

	public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSPopover), new PropertyMetadata(Appearances.PrimaryDark, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((VideoOSPopover)o).OnAppearanceChanged();
	}));

	private Popup _popup;

	public UIElement Child
	{
		get
		{
			return (UIElement)GetValue(ChildProperty);
		}
		set
		{
			SetValue(ChildProperty, value);
		}
	}

	public string Text
	{
		get
		{
			return (string)GetValue(TextProperty);
		}
		set
		{
			SetValue(TextProperty, value);
		}
	}

	public TextAlignments TextAlignment
	{
		get
		{
			return (TextAlignments)GetValue(TextAlignmentProperty);
		}
		set
		{
			SetValue(TextAlignmentProperty, value);
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

	public bool StaysOpen
	{
		get
		{
			return (bool)GetValue(StaysOpenProperty);
		}
		set
		{
			SetValue(StaysOpenProperty, value);
		}
	}

	public FrameworkElement PlacementTarget
	{
		get
		{
			return (FrameworkElement)GetValue(PlacementTargetProperty);
		}
		set
		{
			SetValue(PlacementTargetProperty, value);
		}
	}

	public Appearances Appearance
	{
		get
		{
			return (Appearances)GetValue(AppearanceProperty);
		}
		set
		{
			SetValue(AppearanceProperty, value);
		}
	}

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

	void IAddChild.AddChild(object value)
	{
		UIElement uIElement = value as UIElement;
		if (uIElement == null && value != null)
		{
			throw new ArgumentException("Value must be of type UIElement or null", "value");
		}
		Child = uIElement;
	}

	void IAddChild.AddText(string text)
	{
		Text = text;
	}

	private void OnIsOpenChanged()
	{
		if (IsOpen)
		{
			_popup = new Popup
			{
				AllowsTransparency = true,
				PlacementTarget = (PlacementTarget ?? this.FindAncestor<FrameworkElement>()),
				Placement = PlacementMode.Custom,
				StaysOpen = StaysOpen
			};
			_popup.Child = new PopoverContent(_popup)
			{
				Appearance = Appearance
			};
			UpdatePopoverContent();
			_popup.Closed += _popup_Closed;
			_popup.IsOpen = true;
			RaiseEvent(new RoutedEventArgs(OpenedEvent));
		}
		else
		{
			if (_popup != null)
			{
				_popup.IsOpen = false;
			}
			RaiseEvent(new RoutedEventArgs(ClosedEvent));
		}
	}

	private void OnTextChanged()
	{
		UpdatePopoverContent();
	}

	private void OnTextAlignmentChanged()
	{
		UpdatePopoverContent();
	}

	private void OnChildChanged()
	{
		UpdatePopoverContent();
	}

	private void OnStaysOpenChanged()
	{
		if (_popup != null)
		{
			_popup.StaysOpen = StaysOpen;
		}
	}

	private void OnPlacementTargetChanged()
	{
		if (_popup != null)
		{
			_popup.PlacementTarget = PlacementTarget ?? this.FindAncestor<FrameworkElement>();
			UIElement child = _popup.Child;
			_popup.Child = null;
			_popup.Child = child;
		}
	}

	private void OnAppearanceChanged()
	{
		UpdatePopoverContent();
	}

	private void UpdatePopoverContent()
	{
		if (_popup != null)
		{
			PopoverContent obj = (PopoverContent)_popup.Child;
			obj.Appearance = Appearance;
			obj.TextAlignment = TextAlignment;
			obj.Content = ((Child == null) ? ((object)Text) : ((object)Child));
		}
	}

	private void _popup_Closed(object sender, EventArgs e)
	{
		Popup obj = (Popup)sender;
		obj.Closed -= _popup_Closed;
		obj.Child = null;
		if (obj == _popup)
		{
			_popup = null;
			SetCurrentValue(IsOpenProperty, false);
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSPopoverAutomationPeer(this);
	}
}

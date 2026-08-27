using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSContextMenuItem : MenuItem
{
	public enum CheckedAppearances
	{
		None,
		Marked,
		Highlighted
	}

	public static readonly RoutedEvent IsPressedByMouseOrKeyboardChangedEvent = EventManager.RegisterRoutedEvent("IsPressedByMouseOrKeyboardChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSContextMenuItem));

	public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSContextMenuItem), new PropertyMetadata(null));

	public static readonly DependencyProperty CheckedAppearanceProperty = DependencyProperty.Register("SelectionMode", typeof(CheckedAppearances), typeof(VideoOSContextMenuItem), new PropertyMetadata(CheckedAppearances.Marked));

	public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(VideoOSContextMenuItem), new PropertyMetadata(null));

	public static readonly DependencyProperty IsDestructiveProperty = DependencyProperty.Register("IsDestructive", typeof(bool), typeof(VideoOSContextMenuItem), new PropertyMetadata(false));

	public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register("IsInteractive", typeof(bool), typeof(VideoOSContextMenuItem), new PropertyMetadata(true, IsInteractiveChangedCallback));

	public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(VideoOSContextMenuItem), new PropertyMetadata(null));

	public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(VideoOSContextMenuItem), new PropertyMetadata(null));

	public static readonly DependencyProperty AlertProperty = DependencyProperty.Register("Alert", typeof(VideoOSAlert), typeof(VideoOSContextMenuItem), new PropertyMetadata(null));

	private static readonly DependencyPropertyKey IsPressedByMouseOrKeyboardPropertyKey = DependencyProperty.RegisterReadOnly("IsPressedByMouseOrKeyboard", typeof(bool), typeof(VideoOSContextMenuItem), new PropertyMetadata(false, IsPressedByMouseOrKeyboardChangedCallback));

	public static readonly DependencyProperty IsPressedByMouseOrKeyboardProperty = IsPressedByMouseOrKeyboardPropertyKey.DependencyProperty;

	public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register("IsDraggable", typeof(bool), typeof(VideoOSContextMenuItem), new PropertyMetadata(false));

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

	public CheckedAppearances CheckedAppearance
	{
		get
		{
			return (CheckedAppearances)GetValue(CheckedAppearanceProperty);
		}
		set
		{
			SetValue(CheckedAppearanceProperty, value);
		}
	}

	public object Data
	{
		get
		{
			return GetValue(DataProperty);
		}
		set
		{
			SetValue(DataProperty, value);
		}
	}

	public bool IsDestructive
	{
		get
		{
			return (bool)GetValue(IsDestructiveProperty);
		}
		set
		{
			SetValue(IsDestructiveProperty, value);
		}
	}

	public bool IsInteractive
	{
		get
		{
			return (bool)GetValue(IsInteractiveProperty);
		}
		set
		{
			SetValue(IsInteractiveProperty, value);
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

	public VideoOSAlert Alert
	{
		get
		{
			return (VideoOSAlert)GetValue(AlertProperty);
		}
		set
		{
			SetValue(AlertProperty, value);
		}
	}

	public bool IsPressedByMouseOrKeyboard
	{
		get
		{
			return (bool)GetValue(IsPressedByMouseOrKeyboardProperty);
		}
		set
		{
			SetValue(IsPressedByMouseOrKeyboardPropertyKey, value);
		}
	}

	public bool IsDraggable
	{
		get
		{
			return (bool)GetValue(IsDraggableProperty);
		}
		set
		{
			SetValue(IsDraggableProperty, value);
		}
	}

	public event RoutedEventHandler IsPressedByMouseOrKeyboardChanged
	{
		add
		{
			AddHandler(IsPressedByMouseOrKeyboardChangedEvent, value);
		}
		remove
		{
			RemoveHandler(IsPressedByMouseOrKeyboardChangedEvent, value);
		}
	}

	private static void IsInteractiveChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is bool focusable)
		{
			((VideoOSContextMenuItem)d).Focusable = focusable;
		}
	}

	private static void IsPressedByMouseOrKeyboardChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSContextMenuItem)d).RaiseEvent(new RoutedEventArgs(IsPressedByMouseOrKeyboardChangedEvent));
	}

	public VideoOSContextMenuItem()
	{
		base.Loaded += OnLoaded;
		base.Unloaded += OnUnloaded;
		base.PreviewKeyDown += OnKeyDown;
		base.KeyUp += OnKeyUp;
		base.LostKeyboardFocus += OnLostKeyboardFocus;
	}

	protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
	{
		base.OnPreviewMouseUp(e);
		if (!IsInteractive)
		{
			e.Handled = true;
		}
	}

	public override string ToString()
	{
		if (Data is string result)
		{
			return result;
		}
		return base.ToString();
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		AddIsPressedHandler();
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		RemoveIsPressedHandler();
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			UpdateIsPressedByMouseOrKeyboard();
		}
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			UpdateIsPressedByMouseOrKeyboard();
		}
	}

	private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	{
		UpdateIsPressedByMouseOrKeyboard();
	}

	private void AddIsPressedHandler()
	{
		RemoveIsPressedHandler();
		DependencyPropertyDescriptor.FromProperty(MenuItem.IsPressedProperty, typeof(VideoOSContextMenuItem)).AddValueChanged(this, OnIsPressedChanged);
	}

	private void RemoveIsPressedHandler()
	{
		DependencyPropertyDescriptor.FromProperty(MenuItem.IsPressedProperty, typeof(VideoOSContextMenuItem)).RemoveValueChanged(this, OnIsPressedChanged);
	}

	private void OnIsPressedChanged(object sender, EventArgs e)
	{
		UpdateIsPressedByMouseOrKeyboard();
	}

	private void UpdateIsPressedByMouseOrKeyboard()
	{
		if (base.IsPressed || (base.IsKeyboardFocused && Keyboard.IsKeyDown(Key.Return)))
		{
			IsPressedByMouseOrKeyboard = true;
		}
		else
		{
			IsPressedByMouseOrKeyboard = false;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSContextMenuItemAutomationPeer(this);
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using VideoOS.UI.Common.WPF.Utils;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace VideoOS.UI.Common.WPF.CustomControls;

[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
[TemplatePart(Name = "PART_PopupPresenter", Type = typeof(ContentPresenter))]
public class DropDown : ContentControl
{
	public static readonly DependencyProperty DropDownContentProperty;

	public static readonly DependencyProperty IsDropDownOpenProperty;

	public static readonly DependencyProperty MaxDropDownHeightProperty;

	public static readonly DependencyProperty MaxDropDownWidthProperty;

	public static readonly DependencyProperty PlacementProperty;

	public static readonly DependencyProperty HorizontalOffsetProperty;

	public static readonly DependencyProperty VerticalOffsetProperty;

	public static readonly DependencyProperty StaysOpenProperty;

	public static readonly DependencyProperty ToggleButtonStyleProperty;

	public static readonly DependencyProperty ArrowVisibilityProperty;

	public static readonly DependencyProperty MouseWheelOutsidePopupBehaviorProperty;

	private ContentPresenter _popupPresenter;

	private Popup _popup;

	private Point? _popupPointWhenOpened;

	public object DropDownContent
	{
		get
		{
			return GetValue(DropDownContentProperty);
		}
		set
		{
			SetValue(DropDownContentProperty, value);
		}
	}

	public bool IsDropDownOpen
	{
		get
		{
			return (bool)GetValue(IsDropDownOpenProperty);
		}
		set
		{
			SetValue(IsDropDownOpenProperty, value);
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

	public PlacementMode Placement
	{
		get
		{
			return (PlacementMode)GetValue(PlacementProperty);
		}
		set
		{
			SetValue(PlacementProperty, value);
		}
	}

	public double HorizontalOffset
	{
		get
		{
			return (double)GetValue(HorizontalOffsetProperty);
		}
		set
		{
			SetValue(HorizontalOffsetProperty, value);
		}
	}

	public double VerticalOffset
	{
		get
		{
			return (double)GetValue(VerticalOffsetProperty);
		}
		set
		{
			SetValue(VerticalOffsetProperty, value);
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

	public double MaxDropDownWidth
	{
		get
		{
			return (double)GetValue(MaxDropDownWidthProperty);
		}
		set
		{
			SetValue(MaxDropDownWidthProperty, value);
		}
	}

	public Style ToggleButtonStyle
	{
		get
		{
			return (Style)GetValue(ToggleButtonStyleProperty);
		}
		set
		{
			SetValue(ToggleButtonStyleProperty, value);
		}
	}

	public Visibility ArrowVisibility
	{
		get
		{
			return (Visibility)GetValue(ArrowVisibilityProperty);
		}
		set
		{
			SetValue(ArrowVisibilityProperty, value);
		}
	}

	public MouseWheelOutsidePopupBehavior MouseWheelOutsidePopupBehavior
	{
		get
		{
			return (MouseWheelOutsidePopupBehavior)GetValue(MouseWheelOutsidePopupBehaviorProperty);
		}
		set
		{
			SetValue(MouseWheelOutsidePopupBehaviorProperty, value);
		}
	}

	static DropDown()
	{
		DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(DropDown));
		IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner(typeof(DropDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsDropDownOpenPropertyChanged));
		MaxDropDownHeightProperty = ComboBox.MaxDropDownHeightProperty.AddOwner(typeof(DropDown));
		MaxDropDownWidthProperty = DependencyProperty.Register("MaxDropDownWidth", typeof(double), typeof(DropDown), new FrameworkPropertyMetadata(double.MaxValue));
		PlacementProperty = Popup.PlacementProperty.AddOwner(typeof(DropDown));
		HorizontalOffsetProperty = Popup.HorizontalOffsetProperty.AddOwner(typeof(DropDown));
		VerticalOffsetProperty = Popup.VerticalOffsetProperty.AddOwner(typeof(DropDown));
		StaysOpenProperty = Popup.StaysOpenProperty.AddOwner(typeof(DropDown), new PropertyMetadata(false));
		ToggleButtonStyleProperty = DependencyProperty.Register("ToggleButtonStyle", typeof(Style), typeof(DropDown), new PropertyMetadata((object)null));
		ArrowVisibilityProperty = DependencyProperty.Register("ArrowVisibility", typeof(Visibility), typeof(DropDown), new PropertyMetadata(Visibility.Visible));
		MouseWheelOutsidePopupBehaviorProperty = DependencyProperty.Register("MouseWheelOutsidePopupBehavior", typeof(MouseWheelOutsidePopupBehavior), typeof(DropDown), new FrameworkPropertyMetadata(MouseWheelOutsidePopupBehavior.RemainOpen));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDown), new FrameworkPropertyMetadata(typeof(DropDown)));
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		if (_popup != null)
		{
			_popup.PreviewMouseWheel -= _popup_PreviewMouseWheel;
		}
		if (_popupPresenter != null)
		{
			_popupPresenter.KeyDown -= _popupPresenter_KeyDown;
		}
		_popup = GetTemplateChild("PART_Popup") as Popup;
		if (_popup != null)
		{
			_popup.PreviewMouseWheel += _popup_PreviewMouseWheel;
			_popup.Opened += delegate
			{
				_popupPointWhenOpened = PointToScreen(default(Point));
			};
			base.LayoutUpdated += OnLayoutUpdated;
		}
		_popupPresenter = GetTemplateChild("PART_PopupPresenter") as ContentPresenter;
		if (_popupPresenter != null)
		{
			_popupPresenter.KeyDown += _popupPresenter_KeyDown;
		}
	}

	private void OnLayoutUpdated(object sender, EventArgs e)
	{
		if (_popup != null && _popupPointWhenOpened.HasValue && PresentationSource.FromVisual(this) != null)
		{
			Point value = PointToScreen(default(Point));
			if (_popup.IsOpen && _popupPointWhenOpened != value)
			{
				PlacementMode placement = _popup.Placement;
				_popup.Visibility = Visibility.Hidden;
				_popup.Placement = placement.Next();
				_popup.Placement = placement;
				_popup.Visibility = Visibility.Visible;
			}
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if ((e.Key == Key.F4 && Keyboard.Modifiers == ModifierKeys.None) || (e.Key == Key.Down && Keyboard.Modifiers == ModifierKeys.None) || (e.Key == Key.Down && Keyboard.Modifiers == ModifierKeys.Alt))
		{
			SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
			e.Handled = true;
		}
		else if (e.Key == Key.Escape)
		{
			SetCurrentValue(IsDropDownOpenProperty, false);
			e.Handled = true;
		}
		base.OnKeyDown(e);
	}

	private static void IsDropDownOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		DropDown dropDown = sender as DropDown;
		if (dropDown.IsDropDownOpen)
		{
			dropDown.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}
	}

	private void _popupPresenter_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Key == Key.F4 && Keyboard.Modifiers == ModifierKeys.None) || e.Key == Key.Escape)
		{
			e.Handled = true;
			SetCurrentValue(IsDropDownOpenProperty, false);
			Focus();
		}
	}

	private void _popup_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		switch (MouseWheelOutsidePopupBehavior)
		{
		case MouseWheelOutsidePopupBehavior.Close:
			_popup.IsOpen = IsMouseOverPopup();
			break;
		case MouseWheelOutsidePopupBehavior.Suppress:
			e.Handled = !IsMouseOverPopup();
			break;
		case MouseWheelOutsidePopupBehavior.RemainOpen:
			break;
		}
	}

	private bool IsMouseOverPopup()
	{
		IntPtr? intPtr = (PresentationSource.FromVisual(_popup.Child) as HwndSource)?.Handle;
		if (!intPtr.HasValue)
		{
			return false;
		}
		NativeMethods.GetWindowRect(intPtr.Value, out var lpRect);
		Point position = Mouse.GetPosition(this);
		Point point = PointToScreen(position);
		lpRect.Top += 2;
		lpRect.Right--;
		lpRect.Bottom -= 3;
		if (point.X >= (double)lpRect.Left && point.X <= (double)lpRect.Right && point.Y >= (double)lpRect.Top)
		{
			return point.Y <= (double)lpRect.Bottom;
		}
		return false;
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public partial class WpfMessageBoxWindow : Window, IComponentConnector
{
	private const int WM_SHOWWINDOW = 24;

	private bool _buttonHandlersAttached;

	public static readonly DependencyProperty ResultProperty = DependencyProperty.RegisterAttached("Result", typeof(WpfMessageBoxResult), typeof(WpfMessageBoxWindow), new PropertyMetadata(WpfMessageBoxResult.None));

	public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool), typeof(WpfMessageBoxButton));

	public ICommand InitCommand { get; set; }

	public WpfMessageBoxResult Result
	{
		get
		{
			return (WpfMessageBoxResult)GetValue(ResultProperty);
		}
		set
		{
			SetValue(ResultProperty, value);
		}
	}

	public bool IsCloseButtonEnabled { get; set; }

	public FrameworkElement OwnerElement { get; set; }

	public WpfMessageBoxWindow()
	{
		IsCloseButtonEnabled = true;
		InitializeComponent();
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
	}

	public static void SetCloseButtonEnabled(DependencyObject d, bool value)
	{
		FindGoverningMessageBoxWindow(d).IsCloseButtonEnabled = value;
	}

	public static WpfMessageBoxWindow FindGoverningMessageBoxWindow(DependencyObject d)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = null;
		while (d != null && (wpfMessageBoxWindow = d as WpfMessageBoxWindow) == null)
		{
			d = VisualTreeHelper.GetParent(d);
		}
		if (wpfMessageBoxWindow == null)
		{
			throw new ArgumentException("The specified dependency object is not in the visual tree of a WpfMessageBoxWindow.", "d");
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxResult GetResult(DependencyObject d)
	{
		return (WpfMessageBoxResult)d.GetValue(ResultProperty);
	}

	public static void SetResult(DependencyObject d, WpfMessageBoxResult value)
	{
		d.SetValue(ResultProperty, value);
	}

	public static bool GetDialogResult(DependencyObject d)
	{
		return (bool)d.GetValue(DialogResultProperty);
	}

	public static void SetDialogResult(DependencyObject d, bool value)
	{
		d.SetValue(DialogResultProperty, value);
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);
		if (!IsCloseButtonEnabled && PresentationSource.FromVisual(this) is HwndSource hwndSource)
		{
			hwndSource.AddHook(hwndSourceHook);
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		base.OnActivated(e);
		if (_buttonHandlersAttached)
		{
			return;
		}
		foreach (Button item in EnumerateButtons())
		{
			if (GetResult(item) != WpfMessageBoxResult.None)
			{
				item.Click += button_Click;
			}
		}
		if (InitCommand != null && InitCommand.CanExecute(null))
		{
			InitCommand.Execute(null);
		}
		_buttonHandlersAttached = true;
	}

	private void button_Click(object sender, RoutedEventArgs e)
	{
		Button d = sender as Button;
		Result = GetResult(d);
		if (Result == WpfMessageBoxResult.None)
		{
			return;
		}
		if (ComponentDispatcher.IsThreadModal)
		{
			if (!base.DialogResult.HasValue)
			{
				base.DialogResult = GetDialogResult(d);
			}
		}
		else
		{
			Close();
		}
	}

	private IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 24)
		{
			IntPtr systemMenu = NativeMethods.GetSystemMenu(hwnd, bRevert: false);
			if (systemMenu != IntPtr.Zero)
			{
				NativeMethods.EnableMenuItem(systemMenu, NativeMethods.IdEnableItem.SC_CLOSE, NativeMethods.Enable.MF_GRAYED);
			}
		}
		return IntPtr.Zero;
	}

	public void CloseDialog()
	{
		if (!base.DialogResult.HasValue)
		{
			base.DialogResult = true;
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		if (Result != WpfMessageBoxResult.None)
		{
			return;
		}
		foreach (Button item in EnumerateButtons())
		{
			switch (GetResult(item))
			{
			case WpfMessageBoxResult.Cancel:
				Result = WpfMessageBoxResult.Cancel;
				return;
			case WpfMessageBoxResult.No:
				Result = WpfMessageBoxResult.No;
				return;
			case WpfMessageBoxResult.Ok:
			{
				WpfMessageBoxResult wpfMessageBoxResult = WpfMessageBoxResult.Ok;
				return;
			}
			}
		}
	}

	private IEnumerable<Button> EnumerateButtons()
	{
		return EnumerateButtons(this);
	}

	private IEnumerable<Button> EnumerateButtons(DependencyObject d)
	{
		for (int childIdx = 0; childIdx < VisualTreeHelper.GetChildrenCount(d); childIdx++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(d, childIdx);
			if (child is Button button)
			{
				yield return button;
			}
			foreach (Button item in EnumerateButtons(child))
			{
				yield return item;
			}
		}
	}
}

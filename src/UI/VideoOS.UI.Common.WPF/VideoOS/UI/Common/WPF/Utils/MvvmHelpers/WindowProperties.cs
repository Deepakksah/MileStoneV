using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class WindowProperties : DependencyObject
{
	public static readonly DependencyProperty TitleProperty = Window.TitleProperty.AddOwner(typeof(WindowProperties));

	public static readonly DependencyProperty ResizeModeProperty = Window.ResizeModeProperty.AddOwner(typeof(WindowProperties));

	public static readonly DependencyProperty ShowInTaskbarProperty = Window.ShowInTaskbarProperty.AddOwner(typeof(WindowProperties));

	public static readonly DependencyProperty IconProperty = Window.IconProperty.AddOwner(typeof(WindowProperties));

	public static readonly DependencyProperty MinHeightProperty = DependencyProperty.RegisterAttached("MinHeight", typeof(double), typeof(WindowProperties), new PropertyMetadata(0.0));

	public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth", typeof(double), typeof(WindowProperties), new PropertyMetadata(0.0));

	public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.RegisterAttached("MaxHeight", typeof(double), typeof(WindowProperties), new PropertyMetadata(double.PositiveInfinity));

	public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth", typeof(double), typeof(WindowProperties), new PropertyMetadata(double.PositiveInfinity));

	public static readonly DependencyProperty WindowStartupLocationProperty = DependencyProperty.RegisterAttached("WindowStartupLocation", typeof(WindowStartupLocation), typeof(WindowProperties), new PropertyMetadata(WindowStartupLocation.CenterOwner));

	public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.RegisterAttached("SizeToContent", typeof(SizeToContent), typeof(WindowProperties), new PropertyMetadata(SizeToContent.WidthAndHeight));

	public static readonly DependencyProperty TaskbarIntermediateProgressStateProperty = DependencyProperty.RegisterAttached("TaskbarIntermediateProgressState", typeof(bool), typeof(WindowProperties), new UIPropertyMetadata(false, TaskbarIntermediateProgressStateChanged));

	public static readonly DependencyProperty DisableCloseButtonProperty = DependencyProperty.RegisterAttached("DisableCloseButton", typeof(bool), typeof(WindowProperties), new UIPropertyMetadata(false, DisableCloseButtonChanged));

	public static readonly DependencyProperty CancelClosingProperty = DependencyProperty.RegisterAttached("CancelClosing", typeof(bool), typeof(WindowProperties), new UIPropertyMetadata(false, CancelClosingChanged));

	public static string GetTitle(DependencyObject @object)
	{
		return (string)@object.GetValue(TitleProperty);
	}

	public static void SetTitle(DependencyObject @object, string value)
	{
		@object.SetValue(TitleProperty, value);
	}

	public static double GetMinHeight(DependencyObject @object)
	{
		return (double)@object.GetValue(MinHeightProperty);
	}

	public static void SetMinHeight(DependencyObject @object, double value)
	{
		@object.SetValue(MinHeightProperty, value);
	}

	public static double GetMinWidth(DependencyObject @object)
	{
		return (double)@object.GetValue(MinWidthProperty);
	}

	public static void SetMinWidth(DependencyObject @object, double value)
	{
		@object.SetValue(MinWidthProperty, value);
	}

	public static double GetMaxHeight(DependencyObject @object)
	{
		return (double)@object.GetValue(MaxHeightProperty);
	}

	public static void SetMaxHeight(DependencyObject @object, double value)
	{
		@object.SetValue(MaxHeightProperty, value);
	}

	public static double GetMaxWidth(DependencyObject @object)
	{
		return (double)@object.GetValue(MaxWidthProperty);
	}

	public static void SetMaxWidth(DependencyObject @object, double value)
	{
		@object.SetValue(MaxWidthProperty, value);
	}

	public static ResizeMode GetResizeMode(DependencyObject @object)
	{
		return (ResizeMode)@object.GetValue(ResizeModeProperty);
	}

	public static void SetResizeMode(DependencyObject @object, ResizeMode value)
	{
		@object.SetValue(ResizeModeProperty, value);
	}

	public static SizeToContent GetSizeToContent(DependencyObject @object)
	{
		return (SizeToContent)@object.GetValue(SizeToContentProperty);
	}

	public static void SetSizeToContent(DependencyObject @object, SizeToContent value)
	{
		@object.SetValue(SizeToContentProperty, value);
	}

	public static bool GetShowInTaskbar(DependencyObject @object)
	{
		return (bool)@object.GetValue(ShowInTaskbarProperty);
	}

	public static void SetShowInTaskbar(DependencyObject @object, bool value)
	{
		@object.SetValue(ShowInTaskbarProperty, value);
	}

	public static WindowStartupLocation GetWindowStartupLocation(DependencyObject @object)
	{
		return (WindowStartupLocation)@object.GetValue(WindowStartupLocationProperty);
	}

	public static void SetWindowStartupLocation(DependencyObject @object, WindowStartupLocation value)
	{
		@object.SetValue(WindowStartupLocationProperty, value);
	}

	public static ImageSource GetIcon(DependencyObject @object)
	{
		return (ImageSource)@object.GetValue(IconProperty);
	}

	public static void SetIcon(DependencyObject @object, ImageSource value)
	{
		@object.SetValue(IconProperty, value);
	}

	public static bool GetCancelClosing(DependencyObject obj)
	{
		return (bool)obj?.GetValue(CancelClosingProperty);
	}

	public static void SetCancelClosing(DependencyObject obj, bool value)
	{
		obj?.SetValue(CancelClosingProperty, value);
	}

	public static bool GetTaskbarIntermediateProgressState(DependencyObject obj)
	{
		return (bool)obj?.GetValue(TaskbarIntermediateProgressStateProperty);
	}

	public static void SetTaskbarIntermediateProgressState(DependencyObject obj, bool value)
	{
		obj?.SetValue(TaskbarIntermediateProgressStateProperty, value);
	}

	public static bool GetDisableCloseButton(DependencyObject obj)
	{
		return (bool)obj?.GetValue(DisableCloseButtonProperty);
	}

	public static void SetDisableCloseButton(DependencyObject obj, bool value)
	{
		obj?.SetValue(DisableCloseButtonProperty, value);
	}

	private static void CancelClosingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		Window window = (Window)sender;
		if ((bool)e.NewValue)
		{
			window.Closing += Window_Closing;
		}
		else
		{
			window.Closing -= Window_Closing;
		}
	}

	private static void Window_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
	}

	private static void TaskbarIntermediateProgressStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		Window obj = (Window)sender;
		TaskbarItemProgressState progressState = (((bool)e.NewValue) ? TaskbarItemProgressState.Indeterminate : TaskbarItemProgressState.None);
		obj.TaskbarItemInfo = new TaskbarItemInfo
		{
			ProgressState = progressState
		};
	}

	private static void DisableCloseButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		Window window = (Window)sender;
		bool isDisabled = (bool)e.NewValue;
		SetCloseButtonState(window, isDisabled);
	}

	private static void SetCloseButtonState(Window window, bool isDisabled)
	{
		IntPtr systemMenu = NativeMethods.GetSystemMenu(new WindowInteropHelper(window).Handle, bRevert: false);
		if (systemMenu != IntPtr.Zero)
		{
			NativeMethods.Enable enable = (isDisabled ? NativeMethods.Enable.MF_GRAYED : NativeMethods.Enable.MF_BYCOMMAND);
			NativeMethods.EnableMenuItem(systemMenu, NativeMethods.IdEnableItem.SC_CLOSE, NativeMethods.Enable.MF_BYCOMMAND | enable);
		}
		else
		{
			window.SourceInitialized += delegate
			{
				SetCloseButtonState(window, isDisabled);
			};
		}
	}
}

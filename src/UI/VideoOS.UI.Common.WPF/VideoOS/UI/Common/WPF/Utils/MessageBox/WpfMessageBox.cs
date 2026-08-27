using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using VideoOS.Common.Definition;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public static class WpfMessageBox
{
	private const WpfMessageBoxButton _defaultButton = WpfMessageBoxButton.Ok;

	private const WpfMessageBoxImage _defaultImage = WpfMessageBoxImage.None;

	private const string _defaultCaption = "";

	public static WpfMessageBoxResult Show(Visual owner, string text)
	{
		return CreateMessageBox(owner, text).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Visual owner, string text, string caption)
	{
		return CreateMessageBox(owner, text, string.Empty, caption).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Visual owner, string text, string caption, WpfMessageBoxButton button)
	{
		return CreateMessageBox(owner, text, string.Empty, caption, button).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Visual owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return CreateMessageBox(owner, text, string.Empty, caption, button, configuration).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Visual owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return CreateMessageBox(owner, text, string.Empty, caption, button, null, image).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Visual owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration, WpfMessageBoxImage image)
	{
		return CreateMessageBox(owner, text, string.Empty, caption, button, configuration, image).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text)
	{
		return CreateMessageBox(text, "", "", WpfMessageBoxButton.Ok, null, WpfMessageBoxImage.None, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text, string caption)
	{
		return CreateMessageBox(text, string.Empty, caption, WpfMessageBoxButton.Ok, null, WpfMessageBoxImage.None, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text, string caption, WpfMessageBoxButton button)
	{
		return CreateMessageBox(text, string.Empty, caption, button, null, WpfMessageBoxImage.None, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return CreateMessageBox(text, string.Empty, caption, button, configuration, WpfMessageBoxImage.None, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return CreateMessageBox(text, string.Empty, caption, button, null, image, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, List<Inline> textInlines, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return CreateMessageBox(textInlines, string.Empty, caption, button, null, image, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(Window owner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration, WpfMessageBoxImage image)
	{
		return CreateMessageBox(text, string.Empty, caption, button, configuration, image, owner).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text)
	{
		return CreateMessageBox(formsOwner, text).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text, string caption)
	{
		return CreateMessageBox(formsOwner, text, string.Empty, caption).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text, string caption, WpfMessageBoxButton button)
	{
		return CreateMessageBox(formsOwner, text, string.Empty, caption, button).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return CreateMessageBox(formsOwner, text, string.Empty, caption, button, configuration).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return CreateMessageBox(formsOwner, text, string.Empty, caption, button, null, image).ShowDialogEx();
	}

	public static WpfMessageBoxResult Show(IntPtr formsOwner, string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration, WpfMessageBoxImage image)
	{
		return CreateMessageBox(formsOwner, text, string.Empty, caption, button, configuration, image).ShowDialogEx();
	}

	public static void Show(Visual owner, string text, string caption, Exception exception, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		CreateMessageBox(owner, text, ExceptionUtils.ExceptionToDetailsText(exception), caption, button, null, image).ShowDialogEx();
	}

	private static WpfMessageBoxWindow PrepareWindow(object content, string caption, ImageSource icon)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = new WpfMessageBoxWindow
		{
			Content = content,
			Title = caption
		};
		if (icon != null)
		{
			wpfMessageBoxWindow.Icon = icon;
		}
		return wpfMessageBoxWindow;
	}

	private static bool IsWpfWindowOwner(Visual owner, out Window wpfowner, out IntPtr formsOwner)
	{
		if (owner != null)
		{
			Window window = Window.GetWindow(owner);
			if (window != null)
			{
				wpfowner = window;
				formsOwner = IntPtr.Zero;
				return true;
			}
			if (PresentationSource.FromVisual(owner) is HwndSource hwndSource)
			{
				wpfowner = null;
				formsOwner = hwndSource.Handle;
				return false;
			}
		}
		wpfowner = null;
		formsOwner = IntPtr.Zero;
		return true;
	}

	public static WpfMessageBoxWindow CreateMessageBox(Visual owner, string text, string detailsText = "", string caption = "", WpfMessageBoxButton button = WpfMessageBoxButton.Ok, WpfMessageBoxConfiguration configuration = null, WpfMessageBoxImage icon = WpfMessageBoxImage.None)
	{
		return CreateMessageBox(owner, new DefaultMessageBoxContent(text, detailsText, button, icon, configuration), caption);
	}

	public static WpfMessageBoxWindow CreateMessageBox(Visual owner, object content, string caption = "")
	{
		if (IsWpfWindowOwner(owner, out var wpfowner, out var formsOwner))
		{
			return CreateMessageBox(content, caption, wpfowner);
		}
		return CreateMessageBox(formsOwner, content, caption);
	}

	public static WpfMessageBoxWindow CreateMessageBox(string text, string detailsText = "", string caption = "", WpfMessageBoxButton button = WpfMessageBoxButton.Ok, WpfMessageBoxConfiguration configuration = null, WpfMessageBoxImage icon = WpfMessageBoxImage.None, Window owner = null)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = PrepareWindow(new DefaultMessageBoxContent(text, detailsText, button, icon, configuration), caption, configuration?.Icon);
		if (owner != null)
		{
			wpfMessageBoxWindow.Owner = owner;
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxWindow CreateMessageBox(List<Inline> textInlines, string detailsText = "", string caption = "", WpfMessageBoxButton button = WpfMessageBoxButton.Ok, WpfMessageBoxConfiguration configuration = null, WpfMessageBoxImage icon = WpfMessageBoxImage.None, Window owner = null)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = PrepareWindow(new DefaultMessageBoxContent(textInlines, detailsText, button, icon, configuration), caption, configuration?.Icon);
		if (owner != null)
		{
			wpfMessageBoxWindow.Owner = owner;
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxWindow CreateMessageBox(IntPtr formsOwner, string text, string detailsText = "", string caption = "", WpfMessageBoxButton button = WpfMessageBoxButton.Ok, WpfMessageBoxConfiguration configuration = null, WpfMessageBoxImage icon = WpfMessageBoxImage.None)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = PrepareWindow(new DefaultMessageBoxContent(text, detailsText, button, icon, configuration), caption, configuration?.Icon);
		if (formsOwner != IntPtr.Zero)
		{
			new WindowInteropHelper(wpfMessageBoxWindow).Owner = formsOwner;
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxWindow CreateMessageBox(object content, string caption = "", Window owner = null)
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = PrepareWindow(content, caption, null);
		if (owner != null)
		{
			wpfMessageBoxWindow.Owner = owner;
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxWindow CreateMessageBox(IntPtr formsOwner, object content, string caption = "")
	{
		WpfMessageBoxWindow wpfMessageBoxWindow = PrepareWindow(content, caption, null);
		if (formsOwner != IntPtr.Zero)
		{
			new WindowInteropHelper(wpfMessageBoxWindow).Owner = formsOwner;
		}
		return wpfMessageBoxWindow;
	}

	public static WpfMessageBoxResult ShowDialogEx(this WpfMessageBoxWindow self, ICommand initCommand = null)
	{
		if (self.Owner == null)
		{
			self.Topmost = true;
		}
		self.InitCommand = initCommand;
		self.ShowDialog();
		return self.Result;
	}
}

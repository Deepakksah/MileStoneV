using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using VideoOS.Common.Definition;
using VideoOS.UI.Common.WPF.Resources;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public partial class DefaultMessageBoxContent : UserControl, IComponentConnector
{
	private WpfMessageBoxButton _button;

	public DefaultMessageBoxContent()
	{
		InitializeComponent();
	}

	public DefaultMessageBoxContent(string text, string detailsText, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration)
	{
		_button = button;
		InitializeComponent();
		Icon icon = ResolveIcon(image);
		BitmapSource imageSource = null;
		if (icon != null)
		{
			imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
		WpfMessageBoxData dataContext = new WpfMessageBoxData(text, detailsText, button, imageSource);
		if (configuration != null)
		{
			UpdateUI(configuration);
		}
		_detailsHyperLink.Inlines.Add(Strings.MessageBoxShowDetails);
		base.DataContext = dataContext;
	}

	public DefaultMessageBoxContent(List<Inline> textInlines, string detailsText, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration)
	{
		_button = button;
		InitializeComponent();
		Icon icon = ResolveIcon(image);
		BitmapSource imageSource = null;
		if (icon != null)
		{
			imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
		WpfMessageBoxData dataContext = new WpfMessageBoxData(null, detailsText, button, imageSource);
		_textBlockInlines.Inlines.AddRange(textInlines);
		_textBlockInlines.Visibility = base.Visibility;
		_textBlockText.Visibility = Visibility.Collapsed;
		if (configuration != null)
		{
			UpdateUI(configuration);
		}
		base.DataContext = dataContext;
	}

	private static Icon ResolveIcon(WpfMessageBoxImage image)
	{
		return image switch
		{
			WpfMessageBoxImage.None => null, 
			WpfMessageBoxImage.Error => SystemIcons.Error, 
			WpfMessageBoxImage.Question => SystemIcons.Question, 
			WpfMessageBoxImage.Information => SystemIcons.Information, 
			WpfMessageBoxImage.Warning => SystemIcons.Warning, 
			_ => null, 
		};
	}

	private void UpdateUI(WpfMessageBoxConfiguration configuration)
	{
		if (!string.IsNullOrWhiteSpace(configuration.OkButtonText))
		{
			Button okButton = _okButton1;
			object content = (_okButton2.Content = configuration.OkButtonText);
			okButton.Content = content;
		}
		if (!string.IsNullOrWhiteSpace(configuration.CancelButtonText))
		{
			Button cancelButton = _cancelButton1;
			Button cancelButton2 = _cancelButton2;
			object obj = (_cancelButton3.Content = configuration.CancelButtonText);
			object content = (cancelButton2.Content = obj);
			cancelButton.Content = content;
		}
		if (!string.IsNullOrWhiteSpace(configuration.YesButtonText))
		{
			Button yesButton = _yesButton1;
			object content = (_yesButton2.Content = configuration.YesButtonText);
			yesButton.Content = content;
		}
		if (!string.IsNullOrWhiteSpace(configuration.NoButtonText))
		{
			Button noButton = _noButton1;
			object content = (_noButton2.Content = configuration.NoButtonText);
			noButton.Content = content;
		}
		if (!string.IsNullOrWhiteSpace(configuration.RemoveButtonText))
		{
			_removeButton1.Content = configuration.RemoveButtonText;
		}
		if (configuration.OkButtonStyle != null)
		{
			Button okButton2 = _okButton1;
			Style style = (_okButton2.Style = configuration.OkButtonStyle);
			okButton2.Style = style;
		}
		if (configuration.CancelButtonStyle != null)
		{
			Button cancelButton3 = _cancelButton1;
			Button cancelButton4 = _cancelButton2;
			Style style2 = (_cancelButton3.Style = configuration.CancelButtonStyle);
			Style style = (cancelButton4.Style = style2);
			cancelButton3.Style = style;
		}
		if (configuration.YesButtonStyle != null)
		{
			Button yesButton2 = _yesButton1;
			Style style = (_yesButton2.Style = configuration.YesButtonStyle);
			yesButton2.Style = style;
		}
		if (configuration.NoButtonStyle != null)
		{
			Button noButton2 = _noButton1;
			Style style = (_noButton2.Style = configuration.NoButtonStyle);
			noButton2.Style = style;
		}
		if (configuration.RemoveButtonStyle != null)
		{
			_removeButton1.Style = configuration.RemoveButtonStyle;
		}
	}

	private void DefaultMessageBoxContent_Loaded(object sender, RoutedEventArgs e)
	{
		if (_button == WpfMessageBoxButton.YesNo)
		{
			WpfMessageBoxWindow.SetCloseButtonEnabled(this, value: false);
		}
	}

	private void _detailsLink_OnClick(object sender, RoutedEventArgs e)
	{
		bool flag = _detailsGrid.Visibility == Visibility.Visible;
		_detailsGrid.Visibility = (flag ? Visibility.Collapsed : Visibility.Visible);
		_detailsHyperLink.Inlines.Clear();
		_detailsHyperLink.Inlines.Add(flag ? Strings.MessageBoxShowDetails : Strings.MessageBoxHideDetails);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}

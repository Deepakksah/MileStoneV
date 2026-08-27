using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.CustomControls;

public partial class SearchBoxPopup : UserControl, IComponentConnector, IStyleConnector
{
	public static readonly DependencyProperty ListViewItemSelectedUnfocusedBackgroundBrushProperty = DependencyProperty.Register("ListViewItemSelectedUnfocusedBackgroundBrush", typeof(SolidColorBrush), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ListViewItemHoverBackgroundBrushProperty = DependencyProperty.Register("ListViewItemHoverBackgroundBrush", typeof(SolidColorBrush), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ListViewStyleProperty = DependencyProperty.Register("ListViewStyle", typeof(Style), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty SearchBoxBackgroundProperty = DependencyProperty.Register("SearchBoxBackground", typeof(SolidColorBrush), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty SearchBoxBorderBrushProperty = DependencyProperty.Register("SearchBoxBorderBrush", typeof(SolidColorBrush), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty SearchBoxForegroundProperty = DependencyProperty.Register("SearchBoxForeground", typeof(SolidColorBrush), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ScrollViewerTemplateProperty = DependencyProperty.Register("ScrollViewerTemplate", typeof(ControlTemplate), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty SearchIconImageProperty = DependencyProperty.Register("SearchIconImage", typeof(ImageSource), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ClearButtonImageProperty = DependencyProperty.Register("ClearButtonImage", typeof(ImageSource), typeof(SearchBoxPopup), new PropertyMetadata((object)null));

	public SolidColorBrush ListViewItemSelectedUnfocusedBackgroundBrush
	{
		get
		{
			return (SolidColorBrush)GetValue(ListViewItemSelectedUnfocusedBackgroundBrushProperty);
		}
		set
		{
			SetValue(ListViewItemSelectedUnfocusedBackgroundBrushProperty, value);
		}
	}

	public SolidColorBrush ListViewItemHoverBackgroundBrush
	{
		get
		{
			return (SolidColorBrush)GetValue(ListViewItemHoverBackgroundBrushProperty);
		}
		set
		{
			SetValue(ListViewItemHoverBackgroundBrushProperty, value);
		}
	}

	public Style ListViewStyle
	{
		get
		{
			return (Style)GetValue(ListViewStyleProperty);
		}
		set
		{
			SetValue(ListViewStyleProperty, value);
		}
	}

	public SolidColorBrush SearchBoxBackground
	{
		get
		{
			return (SolidColorBrush)GetValue(SearchBoxBackgroundProperty);
		}
		set
		{
			SetValue(SearchBoxBackgroundProperty, value);
		}
	}

	public SolidColorBrush SearchBoxBorderBrush
	{
		get
		{
			return (SolidColorBrush)GetValue(SearchBoxBorderBrushProperty);
		}
		set
		{
			SetValue(SearchBoxBorderBrushProperty, value);
		}
	}

	public SolidColorBrush SearchBoxForeground
	{
		get
		{
			return (SolidColorBrush)GetValue(SearchBoxForegroundProperty);
		}
		set
		{
			SetValue(SearchBoxForegroundProperty, value);
		}
	}

	public ControlTemplate ScrollViewerTemplate
	{
		get
		{
			return (ControlTemplate)GetValue(ScrollViewerTemplateProperty);
		}
		set
		{
			SetValue(ScrollViewerTemplateProperty, value);
		}
	}

	public ImageSource SearchIconImage
	{
		get
		{
			return (ImageSource)GetValue(SearchIconImageProperty);
		}
		set
		{
			SetValue(SearchIconImageProperty, value);
		}
	}

	public ImageSource ClearButtonImage
	{
		get
		{
			return (ImageSource)GetValue(ClearButtonImageProperty);
		}
		set
		{
			SetValue(ClearButtonImageProperty, value);
		}
	}

	public SearchBoxPopup()
	{
		InitializeComponent();
	}

	private void EventSetter_OnGotFocusHandler(object sender, RoutedEventArgs e)
	{
		if (base.DataContext != null && base.DataContext is SearchBoxModel { GotFocusCommand: not null } searchBoxModel && searchBoxModel.GotFocusCommand.CanExecute(null))
		{
			searchBoxModel.GotFocusCommand.Execute(null);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.Mobile.UI.Controls.GroupBoxExpander;

public class GroupBoxExpanderControl : ContentControl
{
	private ContentPresenter _presenter;

	private GroupBox _mainGroupBox;

	public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(GroupBoxExpanderControl), new PropertyMetadata((object)null));

	public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(GroupBoxExpanderControl), new PropertyMetadata(false, OnIsExpandedChanged));

	public static readonly DependencyProperty GroupBoxBorderBrushProperty = DependencyProperty.Register("GroupBoxBorderBrush", typeof(Brush), typeof(GroupBoxExpanderControl), new PropertyMetadata(null, OnBrushChanged));

	public string HeaderText
	{
		get
		{
			return (string)GetValue(HeaderTextProperty);
		}
		set
		{
			SetValue(HeaderTextProperty, value);
		}
	}

	public bool IsExpanded
	{
		get
		{
			return (bool)GetValue(IsExpandedProperty);
		}
		set
		{
			SetValue(IsExpandedProperty, value);
		}
	}

	public Brush GroupBoxBorderBrush
	{
		get
		{
			return (Brush)GetValue(GroupBoxBorderBrushProperty);
		}
		set
		{
			SetValue(GroupBoxBorderBrushProperty, value);
		}
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_presenter = GetTemplateChild("LayoutPresenter") as ContentPresenter;
		_mainGroupBox = GetTemplateChild("MainGroupBox") as GroupBox;
		ApplyBrush(GroupBoxBorderBrush);
		UpdateContentVisibility();
	}

	private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is GroupBoxExpanderControl groupBoxExpanderControl)
		{
			groupBoxExpanderControl.UpdateContentVisibility();
		}
	}

	private void UpdateContentVisibility()
	{
		if (_presenter != null)
		{
			_presenter.Visibility = ((!IsExpanded) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is GroupBoxExpanderControl groupBoxExpanderControl)
		{
			groupBoxExpanderControl.ApplyBrush(e.NewValue as Brush);
		}
	}

	private void ApplyBrush(Brush brush)
	{
		if (_mainGroupBox != null && brush != null)
		{
			_mainGroupBox.BorderBrush = brush;
		}
	}
}

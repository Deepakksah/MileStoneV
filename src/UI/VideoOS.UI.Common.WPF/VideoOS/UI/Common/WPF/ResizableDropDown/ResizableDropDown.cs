using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.ResizableDropDown;

public class ResizableDropDown : ComboBox
{
	private ContentPresenter _contentPresenter;

	private Border _popup;

	private Thumb _resizeButton;

	private const double DeltaArrowSize = 30.0;

	public static readonly DependencyProperty DropDownContentProperty;

	public new static readonly DependencyProperty SelectedItemProperty;

	public static readonly DependencyProperty CustomTextProperty;

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

	public new object SelectedItem
	{
		get
		{
			return GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public object CustomText
	{
		get
		{
			return GetValue(CustomTextProperty);
		}
		set
		{
			SetValue(CustomTextProperty, value);
		}
	}

	static ResizableDropDown()
	{
		DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(ResizableDropDown), new PropertyMetadata(null));
		SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ResizableDropDown), new PropertyMetadata(null, OnSelectedItemChanged));
		CustomTextProperty = DependencyProperty.Register("CustomText", typeof(object), typeof(ResizableDropDown), new PropertyMetadata(null, OnCustomTextChanged));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizableDropDown), new FrameworkPropertyMetadata(typeof(ResizableDropDown)));
	}

	protected override void OnMouseWheel(MouseWheelEventArgs e)
	{
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_resizeButton = (Thumb)GetTemplateChild("PART_ResizeButton");
		_resizeButton.DragDelta += ResizeButtonOnDragDelta;
		_popup = (Border)GetTemplateChild("DropDownBorder");
		_contentPresenter = (ContentPresenter)GetTemplateChild("ContentPresenter");
		TextBlock textBlock = new TextBlock
		{
			Text = " ",
			MinWidth = ((base.MinWidth >= 30.0) ? (base.MinWidth - 30.0) : 0.0),
			MaxWidth = ((base.MaxWidth >= 30.0) ? (base.MaxWidth - 30.0) : 30.0)
		};
		_contentPresenter.Content = textBlock;
		_contentPresenter.ContentTemplate = null;
		SetHeaderText(textBlock);
	}

	private void ResizeButtonOnDragDelta(object sender, DragDeltaEventArgs e)
	{
		_popup.Width = Math.Max(_popup.MinWidth, _popup.ActualWidth + e.HorizontalChange);
		_popup.Height = Math.Max(_popup.MinHeight, _popup.ActualHeight + e.VerticalChange);
	}

	private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		((ResizableDropDown)sender).UpdateContentText();
	}

	private static void OnCustomTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		((ResizableDropDown)sender).UpdateContentText();
	}

	private void UpdateContentText()
	{
		if (_contentPresenter != null)
		{
			TextBlock headerText = (TextBlock)_contentPresenter.Content;
			SetHeaderText(headerText);
		}
	}

	private void SetHeaderText(TextBlock tb)
	{
		string toolTip = (tb.Text = ((SelectedItem != null) ? SelectedItem.ToString() : ((CustomText as string) ?? " ")));
		base.ToolTip = toolTip;
	}
}

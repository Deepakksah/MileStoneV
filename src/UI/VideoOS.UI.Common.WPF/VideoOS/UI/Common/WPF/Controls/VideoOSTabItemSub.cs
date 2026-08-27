using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTabItemSub : TabItem
{
	public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(VideoOSTabItemSub), new PropertyMetadata(false));

	public bool IsPressed
	{
		get
		{
			return (bool)GetValue(IsPressedProperty);
		}
		set
		{
			SetValue(IsPressedProperty, value);
		}
	}

	protected override void OnMouseDown(MouseButtonEventArgs e)
	{
		base.OnMouseDown(e);
		IsPressed = true;
	}

	protected override void OnMouseLeave(MouseEventArgs e)
	{
		base.OnMouseLeave(e);
		IsPressed = false;
	}

	protected override void OnMouseUp(MouseButtonEventArgs e)
	{
		base.OnMouseUp(e);
		IsPressed = false;
	}
}

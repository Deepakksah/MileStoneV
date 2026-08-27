using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSRadioButton : RadioButton
{
	public static readonly DependencyProperty BulletFillBrushProperty;

	public Brush BulletFillBrush
	{
		get
		{
			return (Brush)GetValue(BulletFillBrushProperty);
		}
		set
		{
			SetValue(BulletFillBrushProperty, value);
		}
	}

	static VideoOSRadioButton()
	{
		BulletFillBrushProperty = DependencyProperty.Register("BulletFillBrush", typeof(Brush), typeof(VideoOSRadioButton), new PropertyMetadata((object)null));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSRadioButton), new FrameworkPropertyMetadata(typeof(VideoOSRadioButton)));
	}
}

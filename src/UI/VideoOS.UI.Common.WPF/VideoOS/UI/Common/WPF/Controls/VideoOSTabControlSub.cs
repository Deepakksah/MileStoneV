using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTabControlSub : System.Windows.Controls.TabControl
{
	private static readonly Color ForegroundHoverColor;

	private static readonly Color ForegroundPressedColor;

	private static readonly Color ForegroundSelectedColor;

	private static readonly Color ForegroundDisabledColor;

	private static readonly Color ForegroundDisabledSelectedColor;

	private static readonly Color ContentHolderBackgroundDisabledColor;

	public static readonly DependencyProperty ForegroundHoverProperty;

	public static readonly DependencyProperty ForegroundPressedProperty;

	public static readonly DependencyProperty ForegroundSelectedProperty;

	public static readonly DependencyProperty ForegroundDisabledProperty;

	public static readonly DependencyProperty ForegroundDisabledSelectedProperty;

	public static readonly DependencyProperty HeaderPanelVisibilityProperty;

	public static readonly DependencyProperty ContentHolderBackgroundDisabledProperty;

	public Brush ForegroundHover
	{
		get
		{
			return (Brush)GetValue(ForegroundHoverProperty);
		}
		set
		{
			SetValue(ForegroundHoverProperty, value);
		}
	}

	public Brush ForegroundPressed
	{
		get
		{
			return (Brush)GetValue(ForegroundPressedProperty);
		}
		set
		{
			SetValue(ForegroundPressedProperty, value);
		}
	}

	public Brush ForegroundSelected
	{
		get
		{
			return (Brush)GetValue(ForegroundSelectedProperty);
		}
		set
		{
			SetValue(ForegroundSelectedProperty, value);
		}
	}

	public Brush ForegroundDisabled
	{
		get
		{
			return (Brush)GetValue(ForegroundDisabledProperty);
		}
		set
		{
			SetValue(ForegroundDisabledProperty, value);
		}
	}

	public Brush ForegroundDisabledSelected
	{
		get
		{
			return (Brush)GetValue(ForegroundDisabledSelectedProperty);
		}
		set
		{
			SetValue(ForegroundDisabledSelectedProperty, value);
		}
	}

	public Visibility HeaderPanelVisibility
	{
		get
		{
			return (Visibility)GetValue(HeaderPanelVisibilityProperty);
		}
		set
		{
			SetValue(HeaderPanelVisibilityProperty, value);
		}
	}

	public Brush ContentHolderBackgroundDisabled
	{
		get
		{
			return (Brush)GetValue(ContentHolderBackgroundDisabledProperty);
		}
		set
		{
			SetValue(ContentHolderBackgroundDisabledProperty, value);
		}
	}

	static VideoOSTabControlSub()
	{
		ForegroundHoverColor = Color.FromArgb(byte.MaxValue, 119, 176, 214);
		ForegroundPressedColor = Color.FromArgb(byte.MaxValue, 48, 127, 184);
		ForegroundSelectedColor = Color.FromArgb(byte.MaxValue, 60, 159, 230);
		ForegroundDisabledColor = Color.FromArgb(byte.MaxValue, 67, 67, 67);
		ForegroundDisabledSelectedColor = Color.FromArgb(byte.MaxValue, 49, 79, 100);
		ContentHolderBackgroundDisabledColor = Color.FromArgb(25, 84, 92, 95);
		ForegroundHoverProperty = DependencyProperty.Register("ForegroundHover", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ForegroundHoverColor)));
		ForegroundPressedProperty = DependencyProperty.Register("ForegroundPressed", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ForegroundPressedColor)));
		ForegroundSelectedProperty = DependencyProperty.Register("ForegroundSelected", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ForegroundSelectedColor)));
		ForegroundDisabledProperty = DependencyProperty.Register("ForegroundDisabled", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ForegroundDisabledColor)));
		ForegroundDisabledSelectedProperty = DependencyProperty.Register("ForegroundDisabledSelected", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ForegroundDisabledSelectedColor)));
		HeaderPanelVisibilityProperty = DependencyProperty.Register("HeaderPanelVisibility", typeof(Visibility), typeof(VideoOSTabControlSub), new PropertyMetadata(Visibility.Visible));
		ContentHolderBackgroundDisabledProperty = DependencyProperty.Register("ContentHolderBackgroundDisabled", typeof(Brush), typeof(VideoOSTabControlSub), new PropertyMetadata(new SolidColorBrush(ContentHolderBackgroundDisabledColor)));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTabControlSub), new FrameworkPropertyMetadata(typeof(VideoOSTabControlSub)));
	}
}

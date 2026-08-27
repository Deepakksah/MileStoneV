using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Controls.Internal;

public abstract class VideoOSButtonBase : Button
{
	public static readonly DependencyProperty MouseOverBorderBrushProperty;

	public static readonly DependencyProperty MouseOverBackgroundProperty;

	public static readonly DependencyProperty MouseOverForegroundProperty;

	public static readonly DependencyProperty PressedBorderBrushProperty;

	public static readonly DependencyProperty PressedBackgroundProperty;

	public static readonly DependencyProperty PressedForegroundProperty;

	public static readonly DependencyProperty DisabledBorderBrushProperty;

	public static readonly DependencyProperty DisabledBackgroundProperty;

	public static readonly DependencyProperty DisabledForegroundProperty;

	public static readonly DependencyProperty CornerRadiusProperty;

	public Brush MouseOverBorderBrush
	{
		get
		{
			return (Brush)GetValue(MouseOverBorderBrushProperty);
		}
		set
		{
			SetValue(MouseOverBorderBrushProperty, value);
		}
	}

	public Brush MouseOverBackground
	{
		get
		{
			return (Brush)GetValue(MouseOverBackgroundProperty);
		}
		set
		{
			SetValue(MouseOverBackgroundProperty, value);
		}
	}

	public Brush MouseOverForeground
	{
		get
		{
			return (Brush)GetValue(MouseOverForegroundProperty);
		}
		set
		{
			SetValue(MouseOverForegroundProperty, value);
		}
	}

	public Brush PressedBorderBrush
	{
		get
		{
			return (Brush)GetValue(PressedBorderBrushProperty);
		}
		set
		{
			SetValue(PressedBorderBrushProperty, value);
		}
	}

	public Brush PressedBackground
	{
		get
		{
			return (Brush)GetValue(PressedBackgroundProperty);
		}
		set
		{
			SetValue(PressedBackgroundProperty, value);
		}
	}

	public Brush PressedForeground
	{
		get
		{
			return (Brush)GetValue(PressedForegroundProperty);
		}
		set
		{
			SetValue(PressedForegroundProperty, value);
		}
	}

	public Brush DisabledBorderBrush
	{
		get
		{
			return (Brush)GetValue(DisabledBorderBrushProperty);
		}
		set
		{
			SetValue(DisabledBorderBrushProperty, value);
		}
	}

	public Brush DisabledBackground
	{
		get
		{
			return (Brush)GetValue(DisabledBackgroundProperty);
		}
		set
		{
			SetValue(DisabledBackgroundProperty, value);
		}
	}

	public Brush DisabledForeground
	{
		get
		{
			return (Brush)GetValue(DisabledForegroundProperty);
		}
		set
		{
			SetValue(DisabledForegroundProperty, value);
		}
	}

	public CornerRadius CornerRadius
	{
		get
		{
			return (CornerRadius)GetValue(CornerRadiusProperty);
		}
		set
		{
			SetValue(CornerRadiusProperty, value);
		}
	}

	static VideoOSButtonBase()
	{
		MouseOverBorderBrushProperty = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		MouseOverBackgroundProperty = DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		MouseOverForegroundProperty = DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		PressedBorderBrushProperty = DependencyProperty.Register("PressedBorderBrush", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		PressedForegroundProperty = DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		DisabledBorderBrushProperty = DependencyProperty.Register("DisabledBorderBrush", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		DisabledBackgroundProperty = DependencyProperty.Register("DisabledBackground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		DisabledForegroundProperty = DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(VideoOSButtonBase), new PropertyMetadata((object)null));
		CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(VideoOSButtonBase), new PropertyMetadata(default(CornerRadius)));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonBase), new FrameworkPropertyMetadata(typeof(VideoOSButtonBase)));
	}
}

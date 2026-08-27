using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VideoOS.UI.Common.WPF.ImageSpinnerControl;

public partial class ImageSpinner : UserControl, IComponentConnector
{
	public static readonly DependencyProperty SpinnerDirectionProperty = DependencyProperty.Register("SpinnerDirection", typeof(SpinnerDirection), typeof(ImageSpinner), new PropertyMetadata(SpinnerDirection.Clockwise));

	public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageSpinner), new PropertyMetadata((object)null));

	private Storyboard _spinClockwiseStoryBoard;

	private Storyboard _spinCounterClockwiseStoryBoard;

	private RotateTransform _rotateTransform;

	public SpinnerDirection SpinnerDirection
	{
		get
		{
			return (SpinnerDirection)GetValue(SpinnerDirectionProperty);
		}
		set
		{
			SetValue(SpinnerDirectionProperty, value);
		}
	}

	public ImageSource ImageSource
	{
		get
		{
			return (ImageSource)GetValue(ImageSourceProperty);
		}
		set
		{
			SetValue(ImageSourceProperty, value);
		}
	}

	public ImageSpinner()
	{
		InitializeComponent();
		_image.RenderTransform = (_rotateTransform = new RotateTransform());
		_image.SizeChanged += delegate
		{
			_rotateTransform.CenterX = base.ActualWidth / 2.0;
			_rotateTransform.CenterY = base.ActualHeight / 2.0;
		};
		_spinClockwiseStoryBoard = (Storyboard)FindResource("StoryboardSpinClockwise");
		_spinCounterClockwiseStoryBoard = (Storyboard)FindResource("StoryboardSpinCounterClockwise");
		base.IsVisibleChanged += delegate
		{
			if (base.IsVisible)
			{
				if (SpinnerDirection == SpinnerDirection.Clockwise)
				{
					_spinClockwiseStoryBoard.Begin();
				}
				else
				{
					_spinCounterClockwiseStoryBoard.Begin();
				}
			}
			else
			{
				_spinClockwiseStoryBoard.Stop();
				_spinCounterClockwiseStoryBoard.Stop();
			}
		};
	}
}

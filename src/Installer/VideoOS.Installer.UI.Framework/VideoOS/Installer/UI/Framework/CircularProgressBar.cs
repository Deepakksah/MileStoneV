using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoOS.Installer.UI.Framework;

public partial class CircularProgressBar : UserControl, IComponentConnector, IStyleConnector
{
	private Storyboard _storyboard;

	public static readonly DependencyProperty ItemsColorProperty = DependencyProperty.Register("ItemsColor", typeof(SolidColorBrush), typeof(CircularProgressBar), new UIPropertyMetadata(null));

	public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(string), typeof(CircularProgressBar), new UIPropertyMetadata(null));

	public SolidColorBrush ItemsColor
	{
		get
		{
			return (SolidColorBrush)GetValue(ItemsColorProperty);
		}
		set
		{
			SetValue(ItemsColorProperty, value);
		}
	}

	public string Speed
	{
		get
		{
			return (string)GetValue(SpeedProperty);
		}
		set
		{
			SetValue(SpeedProperty, value);
		}
	}

	public CircularProgressBar()
	{
		InitializeComponent();
	}

	private double CalculateRadius(double canvasRadius)
	{
		return canvasRadius * Math.Tan(0.1975) / (1.0 + Math.Tan(0.1975));
	}

	private void CreateStoryboard(Canvas target)
	{
		_storyboard = new Storyboard();
		DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
		doubleAnimationUsingKeyFrames.BeginTime = TimeSpan.FromSeconds(0.0);
		doubleAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
		SplineDoubleKeyFrame splineDoubleKeyFrame = new SplineDoubleKeyFrame();
		KeyTimeConverter keyTimeConverter = new KeyTimeConverter();
		splineDoubleKeyFrame.KeyTime = (KeyTime)keyTimeConverter.ConvertFromString(Speed);
		splineDoubleKeyFrame.Value = 360.0;
		doubleAnimationUsingKeyFrames.KeyFrames.Add(splineDoubleKeyFrame);
		Storyboard.SetTarget(doubleAnimationUsingKeyFrames, target);
		Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)"));
		_storyboard.Children.Add(doubleAnimationUsingKeyFrames);
	}

	private void HandleLoaded(object sender, RoutedEventArgs e)
	{
		try
		{
			Canvas canvas = (Canvas)base.Template.FindName("indicator", this);
			if (canvas == null)
			{
				return;
			}
			double actualWidth = canvas.ActualWidth;
			int num = 0;
			double num2 = CalculateRadius(actualWidth);
			double num3 = (actualWidth - num2) / 2.0;
			foreach (UIElement child in canvas.Children)
			{
				if (child is Ellipse ellipse)
				{
					ellipse.Width = num2;
					ellipse.Height = num2;
					ellipse.SetValue(Canvas.LeftProperty, num3 + Math.Sin(Math.PI + (double)(float)num * (Math.PI / 5.0)) * num3);
					ellipse.SetValue(Canvas.TopProperty, num3 + Math.Cos(Math.PI + (double)(float)num * (Math.PI / 5.0)) * num3);
					num++;
				}
			}
			CreateStoryboard(canvas);
			_storyboard.Begin();
		}
		catch (Exception)
		{
		}
	}

	private void HandleIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		try
		{
			if (_storyboard != null)
			{
				if ((bool)e.NewValue)
				{
					_storyboard.Begin();
				}
				else
				{
					_storyboard.Stop();
				}
			}
		}
		catch
		{
		}
	}
}

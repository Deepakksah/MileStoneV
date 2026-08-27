using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public partial class PrimaryNavigationControl : UserControl, IComponentConnector
{
	private double _playbackSliderValue;

	private double _playbackSliderScaleOffset;

	public PrimaryNavigationControlModel Model { get; private set; }

	public double PlaybackSliderValue
	{
		get
		{
			return _playbackSliderValue;
		}
		private set
		{
			if (_playbackSliderValue != value)
			{
				_playbackSliderValue = value;
				if (this.PlaybackSliderValueChanged != null)
				{
					this.PlaybackSliderValueChanged(this, new EventArgs());
				}
			}
		}
	}

	public event EventHandler<EventArgs> PlaybackBackwardButtonClicked;

	public event EventHandler<EventArgs> PlaybackForwardButtonClicked;

	public event EventHandler<EventArgs> NavigatePreviousButtonClicked;

	public event EventHandler<EventArgs> NavigateNextButtonClicked;

	public event EventHandler<EventArgs> PlaybackSliderValueChanged;

	public PrimaryNavigationControl()
	{
		InitializeComponent();
		Model = new PrimaryNavigationControlModel();
		base.DataContext = Model;
		_playbackSliderScaleOffset = (_playbackSlider.Maximum - _playbackSlider.Minimum) / 2.0;
	}

	public void SetPlaybackSliderValue(double value)
	{
		_playbackSlider.Value = value * _playbackSliderScaleOffset;
	}

	private void _navigatePreviousButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigatePreviousButtonClicked != null)
		{
			this.NavigatePreviousButtonClicked(this, new EventArgs());
		}
	}

	private void _navigateNextButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigateNextButtonClicked != null)
		{
			this.NavigateNextButtonClicked(this, new EventArgs());
		}
	}

	private void _playbackBackwardButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.PlaybackBackwardButtonClicked != null)
		{
			this.PlaybackBackwardButtonClicked(this, new EventArgs());
		}
	}

	private void _playbackForwardButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.PlaybackForwardButtonClicked != null)
		{
			this.PlaybackForwardButtonClicked(this, new EventArgs());
		}
	}

	private void _playbackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		if (e.NewValue > 0.0)
		{
			PlaybackSliderValue = e.NewValue / _playbackSlider.Maximum;
		}
		else if (e.NewValue < 0.0)
		{
			PlaybackSliderValue = (0.0 - e.NewValue) / _playbackSlider.Minimum;
		}
		else
		{
			PlaybackSliderValue = 0.0;
		}
	}

	private void _playbackSlider_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		_playbackSlider.Value = 0.0;
	}
}

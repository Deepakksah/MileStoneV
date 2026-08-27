using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Utilities.DateTimeHelper;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimeMarkers;

public partial class TimeMarker : Grid, IComponentConnector
{
	private MarkerState _markerState;

	private bool _narrow;

	private double _leftPosition = -2147483648.0;

	private DateTime _time = DateTime.MinValue;

	private string _format = "";

	private TimeZoneInfo _timeZone;

	private double _width;

	public TimeMarker()
	{
		InitializeComponent();
		_width = base.Width;
	}

	public void Hide()
	{
		base.Visibility = Visibility.Hidden;
		_markerState = MarkerState.Hidden;
	}

	public void Update(MarkerState markerState, double leftPosition, bool narrow, DateTime time, string format, TimeZoneInfo timeZone)
	{
		bool flag = false;
		if (_markerState != markerState || _time != time || _format != format || _timeZone != timeZone)
		{
			flag = true;
		}
		if (_markerState != markerState || _narrow != narrow)
		{
			_markerState = markerState;
			_narrow = narrow;
			_primaryMarker.Visibility = Visibility.Hidden;
			_secondaryMarker.Visibility = Visibility.Hidden;
			_tertiaryMarker.Visibility = Visibility.Hidden;
			_legendTextBlock.Visibility = Visibility.Hidden;
			_primaryMarkerNarrow.Visibility = Visibility.Hidden;
			_secondaryMarkerNarrow.Visibility = Visibility.Hidden;
			_tertiaryMarkerNarrow.Visibility = Visibility.Hidden;
			_legendTextBlockNarrow.Visibility = Visibility.Hidden;
			switch (_markerState)
			{
			case MarkerState.Hidden:
				base.Visibility = Visibility.Hidden;
				break;
			case MarkerState.Primary:
				base.Visibility = Visibility.Visible;
				if (narrow)
				{
					_primaryMarkerNarrow.Visibility = Visibility.Visible;
					_legendTextBlockNarrow.Visibility = Visibility.Visible;
				}
				else
				{
					_primaryMarker.Visibility = Visibility.Visible;
					_legendTextBlock.Visibility = Visibility.Visible;
				}
				break;
			case MarkerState.Secondary:
				base.Visibility = Visibility.Visible;
				if (narrow)
				{
					_secondaryMarkerNarrow.Visibility = Visibility.Visible;
				}
				else
				{
					_secondaryMarker.Visibility = Visibility.Visible;
				}
				break;
			case MarkerState.Tertiary:
				base.Visibility = Visibility.Visible;
				if (narrow)
				{
					_tertiaryMarkerNarrow.Visibility = Visibility.Visible;
				}
				else
				{
					_tertiaryMarker.Visibility = Visibility.Visible;
				}
				break;
			}
		}
		if (leftPosition != _leftPosition)
		{
			_leftPosition = leftPosition;
			Canvas.SetLeft(this, leftPosition - _width / 2.0);
		}
		_time = time;
		_format = format;
		_timeZone = timeZone;
		if (flag)
		{
			if (_markerState == MarkerState.Primary)
			{
				string text = DateTimeFormatter.ConvertAndFormat(_format, _timeZone, _time);
				_legendTextBlock.Text = text;
				_legendTextBlockNarrow.Text = text;
			}
			else
			{
				_legendTextBlock.Text = string.Empty;
				_legendTextBlockNarrow.Text = string.Empty;
			}
		}
	}
}

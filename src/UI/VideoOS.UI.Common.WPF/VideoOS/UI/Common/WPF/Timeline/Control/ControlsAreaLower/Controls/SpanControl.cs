using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower.Controls;

public partial class SpanControl : UserControl, IComponentConnector
{
	private List<TimelineSpan> _spans = new List<TimelineSpan>();

	private TimelineSpan _selectedSpan;

	public List<TimelineSpan> Spans => _spans;

	public TimelineSpan SelectedSpan
	{
		get
		{
			return _selectedSpan;
		}
		set
		{
			if (_selectedSpan != value)
			{
				_selectedSpan = value;
				if (_spans.Contains(_selectedSpan))
				{
					_spanSlider.Value = _spans.IndexOf(_selectedSpan);
				}
				_spanTextBlock.Text = _selectedSpan.Text;
				if (this.SelectedSpanChanged != null)
				{
					this.SelectedSpanChanged(this, new EventArgs());
				}
			}
		}
	}

	public event EventHandler SelectedSpanChanged;

	public SpanControl()
	{
		InitializeComponent();
	}

	public void SetTimelineSpans(List<TimelineSpan> timelineSpans, TimelineSpan newTimelineSpan)
	{
		_spans = timelineSpans;
		_spanSlider.Minimum = 0.0;
		_spanSlider.Maximum = _spans.Count - 1;
		SelectedSpan = newTimelineSpan;
	}

	private void _spanSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		SelectedSpan = _spans[(int)_spanSlider.Value];
	}
}

using System.Collections.Generic;
using System.Windows;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimelineLabels;

public class TimelineLabelsLayerModel : ViewModelBase
{
	private Thickness _timelineLabelBackgroundMargin;

	private Visibility _upperTimelineLabelVisibility;

	private string _upperTimelineLabelText;

	private Thickness _upperTimelineLabelMargin;

	private Visibility _lowerTimelineLabelVisibility;

	private string _lowerTimelineLabelText;

	private Thickness _lowerTimelineLabelMargin;

	private Visibility _legendVisibility;

	private Thickness _legendMargin;

	private IEnumerable<LegendEntry> _legend;

	public Thickness TimelineLabelBackgroundMargin
	{
		get
		{
			return _timelineLabelBackgroundMargin;
		}
		set
		{
			SetProperty(ref _timelineLabelBackgroundMargin, value, "TimelineLabelBackgroundMargin");
		}
	}

	public Visibility UpperTimelineLabelVisibility
	{
		get
		{
			return _upperTimelineLabelVisibility;
		}
		set
		{
			SetProperty(ref _upperTimelineLabelVisibility, value, "UpperTimelineLabelVisibility");
		}
	}

	public string UpperTimelineLabelText
	{
		get
		{
			return _upperTimelineLabelText;
		}
		set
		{
			SetProperty(ref _upperTimelineLabelText, value, "UpperTimelineLabelText");
		}
	}

	public Thickness UpperTimelineLabelMargin
	{
		get
		{
			return _upperTimelineLabelMargin;
		}
		set
		{
			SetProperty(ref _upperTimelineLabelMargin, value, "UpperTimelineLabelMargin");
		}
	}

	public Visibility LowerTimelineLabelVisibility
	{
		get
		{
			return _lowerTimelineLabelVisibility;
		}
		set
		{
			SetProperty(ref _lowerTimelineLabelVisibility, value, "LowerTimelineLabelVisibility");
		}
	}

	public string LowerTimelineLabelText
	{
		get
		{
			return _lowerTimelineLabelText;
		}
		set
		{
			SetProperty(ref _lowerTimelineLabelText, value, "LowerTimelineLabelText");
		}
	}

	public Thickness LowerTimelineLabelMargin
	{
		get
		{
			return _lowerTimelineLabelMargin;
		}
		set
		{
			SetProperty(ref _lowerTimelineLabelMargin, value, "LowerTimelineLabelMargin");
		}
	}

	public Visibility LegendVisibility
	{
		get
		{
			return _legendVisibility;
		}
		set
		{
			SetProperty(ref _legendVisibility, value, "LegendVisibility");
		}
	}

	public Thickness LegendMargin
	{
		get
		{
			return _legendMargin;
		}
		set
		{
			SetProperty(ref _legendMargin, value, "LegendMargin");
		}
	}

	public IEnumerable<LegendEntry> Legend
	{
		get
		{
			return _legend;
		}
		public set
		{
			SetProperty(ref _legend, value, "Legend");
		}
	}
}

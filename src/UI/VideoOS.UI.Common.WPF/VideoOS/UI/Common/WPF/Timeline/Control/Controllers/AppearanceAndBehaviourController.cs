using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers.resources;
using VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower;
using VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.Control.Controllers;

public class AppearanceAndBehaviourController : INotifyPropertyChanged
{
	public enum PropertyNames
	{
		SequenceSources,
		TimelineSpans,
		PlaybackSpeeds,
		PlaybackState,
		PlaybackSpeed,
		PlaybackPrimaryNavigationSpeed,
		TimeZone,
		UpperTimelineLabel,
		LowerTimelineLabel,
		TimelineAreaDisplayMode,
		ShowLegend,
		ShowUpperTimelineTopLine,
		ShowUpperTimelineBottomLine,
		ShowUpperTimelineCustomDataLine,
		ShowLowerTimelineTopLine,
		ShowLowerTimelineBottomLine,
		ShowLowerTimelineCustomDataLine,
		ShowControlsAreaUpper,
		ShowControlsAreaLower,
		ShowTimeSelectionControl,
		ShowPlaybackSpeedControl,
		ShowPrimaryNavigationControl,
		ShowSecondaryNavigationControl,
		ShowSpanControl,
		ShowSetupControl,
		ShowTimelineAreaPlaybackControl,
		ShowSpanContextMenuItem,
		ShowPlaybackPlayPauseControls,
		UserSelectionMarkerTimeChangeEnabled,
		UserSelectionControlEnabled,
		UserPlaybackNavigationControlEnabled,
		TimelineLineBackgroundBrush,
		SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush,
		SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush,
		SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush,
		SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush,
		SequenceSourceVisualAppearanceMarkerImage,
		SequenceSourceVisualAppearanceFillExtraZOrder1Brush,
		SequenceSourceVisualAppearanceFillExtraZOrder2Brush,
		SelectionInfoAvailable,
		NoDataDisplayText,
		PlaybackSliderMaxSpeed
	}

	private List<TimelineSpan> _availableTimelineSpans = new List<TimelineSpan>();

	private List<PlaybackSpeed> _availablePlaybackSpeeds = new List<PlaybackSpeed>();

	private ITimelineStyles _timelineStyles;

	private IEnumerable<ISequenceSource> _upperSequenceSources = new List<ISequenceSource>();

	private IEnumerable<ISequenceSource> _lowerSequenceSources = new List<ISequenceSource>();

	private List<TimelineSpan> _timelineSpans = new List<TimelineSpan>();

	private List<PlaybackSpeed> _playbackSpeeds = new List<PlaybackSpeed>();

	private PlaybackState _playbackState = PlaybackState.Stopped;

	private PlaybackSpeed _playbackSpeed;

	private double _playbackPrimaryNavigationSpeed;

	private TimeZoneInfo _timeZone = TimeZoneInfo.Local;

	private string _upperTimelineLabel = "";

	private string _lowerTimelineLabel = "";

	private TimelineAreaDisplayMode _timelineAreaDisplayMode;

	private bool _ShowLegend;

	private bool _showUpperTimelineTopLine = true;

	private bool _showUpperTimelineBottomLine = true;

	private bool _showUpperTimelineCustomDataLine = true;

	private bool _showLowerTimelineTopLine = true;

	private bool _showLowerTimelineBottomLine = true;

	private bool _showLowerTimelineCustomDataLine = true;

	private bool _showControlsAreaUpper = true;

	private bool _showControlsAreaLower = true;

	private bool _showTimeSelectionControl = true;

	private bool _showPlaybackSpeedControl = true;

	private bool _showPrimaryNavigationControl = true;

	private bool _showSecondaryNavigationControl = true;

	private bool _showSpanControl = true;

	private bool _showSetupControl = true;

	private bool _showTimelineAreaPlaybackControl;

	private bool _showSpanContextMenuItem = true;

	private bool _showPlaybackPlayPauseControls = true;

	private bool _userSelectionMarkerTimeChangeEnabled = true;

	private bool _userSelectionControlEnabled = true;

	private bool _userPlaybackNavigationControlEnabled = true;

	private Brush _timelineLineBackgroundBrush;

	private Brush _sequenceSourceVisualAppearanceFillTopLineLowZOrderBrush;

	private Brush _sequenceSourceVisualAppearanceFillTopLineHighZOrderBrush;

	private Brush _sequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush;

	private Brush _sequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush;

	private BitmapImage _sequenceSourceVisualAppearanceMarkerImage;

	private Brush _sequenceSourceVisualAppearanceFillExtraZOrder1Brush;

	private Brush _sequenceSourceVisualAppearanceFillExtraZOrder2Brush;

	private bool _selectionInfoAvailable = true;

	private string _noDataDisplayText;

	private double _playbackSliderMaxSpeed = 4.0;

	public ITimelineStyles TimelineStyles
	{
		get
		{
			return _timelineStyles;
		}
		set
		{
			_timelineStyles = value;
		}
	}

	public IEnumerable<ISequenceSource> UpperSequenceSources => _upperSequenceSources;

	public IEnumerable<ISequenceSource> LowerSequenceSources => _lowerSequenceSources;

	public List<TimelineSpan> TimelineSpans
	{
		get
		{
			return _timelineSpans;
		}
		private set
		{
			if (_timelineSpans != value)
			{
				_timelineSpans = value;
				OnPropertyChanged(PropertyNames.TimelineSpans);
			}
		}
	}

	public List<PlaybackSpeed> PlaybackSpeeds
	{
		get
		{
			return _playbackSpeeds;
		}
		private set
		{
			if (_playbackSpeeds != value)
			{
				_playbackSpeeds = value;
				OnPropertyChanged(PropertyNames.PlaybackSpeeds);
			}
		}
	}

	public PlaybackState PlaybackState
	{
		get
		{
			return _playbackState;
		}
		set
		{
			if (_playbackState != value)
			{
				_playbackState = value;
				OnPropertyChanged(PropertyNames.PlaybackState);
			}
		}
	}

	public PlaybackSpeed PlaybackSpeed
	{
		get
		{
			return _playbackSpeed;
		}
		set
		{
			if (_playbackSpeed != value)
			{
				_playbackSpeed = value;
				OnPropertyChanged(PropertyNames.PlaybackSpeed);
			}
		}
	}

	public double PlaybackPrimaryNavigationSpeed
	{
		get
		{
			return _playbackPrimaryNavigationSpeed;
		}
		set
		{
			if (_playbackPrimaryNavigationSpeed != value)
			{
				_playbackPrimaryNavigationSpeed = value;
				OnPropertyChanged(PropertyNames.PlaybackPrimaryNavigationSpeed);
			}
		}
	}

	public TimeZoneInfo TimeZone
	{
		get
		{
			return _timeZone;
		}
		set
		{
			if (_timeZone != value)
			{
				_timeZone = value;
				OnPropertyChanged(PropertyNames.TimeZone);
			}
		}
	}

	public string UpperTimelineLabel
	{
		get
		{
			return _upperTimelineLabel;
		}
		set
		{
			if (_upperTimelineLabel != value)
			{
				_upperTimelineLabel = value;
				OnPropertyChanged(PropertyNames.UpperTimelineLabel);
			}
		}
	}

	public string LowerTimelineLabel
	{
		get
		{
			return _lowerTimelineLabel;
		}
		set
		{
			if (_lowerTimelineLabel != value)
			{
				_lowerTimelineLabel = value;
				OnPropertyChanged(PropertyNames.LowerTimelineLabel);
			}
		}
	}

	public TimelineAreaDisplayMode TimelineAreaDisplayMode
	{
		get
		{
			return _timelineAreaDisplayMode;
		}
		set
		{
			if (_timelineAreaDisplayMode != value)
			{
				_timelineAreaDisplayMode = value;
				OnPropertyChanged(PropertyNames.TimelineAreaDisplayMode);
			}
		}
	}

	public bool ShowLegend
	{
		get
		{
			return _ShowLegend;
		}
		set
		{
			if (_ShowLegend != value)
			{
				_ShowLegend = value;
				OnPropertyChanged(PropertyNames.ShowLegend);
			}
		}
	}

	public bool ShowUpperTimelineTopLine
	{
		get
		{
			return _showUpperTimelineTopLine;
		}
		set
		{
			if (_showUpperTimelineTopLine != value)
			{
				_showUpperTimelineTopLine = value;
				OnPropertyChanged(PropertyNames.ShowUpperTimelineTopLine);
			}
		}
	}

	public bool ShowUpperTimelineBottomLine
	{
		get
		{
			return _showUpperTimelineBottomLine;
		}
		set
		{
			if (_showUpperTimelineBottomLine != value)
			{
				_showUpperTimelineBottomLine = value;
				OnPropertyChanged(PropertyNames.ShowUpperTimelineBottomLine);
			}
		}
	}

	public bool ShowUpperTimelineCustomDataLine
	{
		get
		{
			return _showUpperTimelineCustomDataLine;
		}
		set
		{
			if (_showUpperTimelineCustomDataLine != value)
			{
				_showUpperTimelineCustomDataLine = value;
				OnPropertyChanged(PropertyNames.ShowUpperTimelineCustomDataLine);
			}
		}
	}

	public bool ShowLowerTimelineTopLine
	{
		get
		{
			return _showLowerTimelineTopLine;
		}
		set
		{
			if (_showLowerTimelineTopLine != value)
			{
				_showLowerTimelineTopLine = value;
				OnPropertyChanged(PropertyNames.ShowLowerTimelineTopLine);
			}
		}
	}

	public bool ShowLowerTimelineBottomLine
	{
		get
		{
			return _showLowerTimelineBottomLine;
		}
		set
		{
			if (_showLowerTimelineBottomLine != value)
			{
				_showLowerTimelineBottomLine = value;
				OnPropertyChanged(PropertyNames.ShowLowerTimelineBottomLine);
			}
		}
	}

	public bool ShowLowerTimelineCustomDataLine
	{
		get
		{
			return _showLowerTimelineCustomDataLine;
		}
		set
		{
			if (_showLowerTimelineCustomDataLine != value)
			{
				_showLowerTimelineCustomDataLine = value;
				OnPropertyChanged(PropertyNames.ShowLowerTimelineCustomDataLine);
			}
		}
	}

	public bool ShowControlsAreaUpper
	{
		get
		{
			return _showControlsAreaUpper;
		}
		set
		{
			if (_showControlsAreaUpper != value)
			{
				_showControlsAreaUpper = value;
				OnPropertyChanged(PropertyNames.ShowControlsAreaUpper);
			}
		}
	}

	public bool ShowControlsAreaLower
	{
		get
		{
			return _showControlsAreaLower;
		}
		set
		{
			if (_showControlsAreaLower != value)
			{
				_showControlsAreaLower = value;
				OnPropertyChanged(PropertyNames.ShowControlsAreaLower);
			}
		}
	}

	public bool ShowTimeSelectionControl
	{
		get
		{
			return _showTimeSelectionControl;
		}
		set
		{
			if (_showTimeSelectionControl != value)
			{
				_showTimeSelectionControl = value;
				OnPropertyChanged(PropertyNames.ShowTimeSelectionControl);
			}
		}
	}

	public bool ShowPlaybackSpeedControl
	{
		get
		{
			return _showPlaybackSpeedControl;
		}
		set
		{
			if (_showPlaybackSpeedControl != value)
			{
				_showPlaybackSpeedControl = value;
				OnPropertyChanged(PropertyNames.ShowPlaybackSpeedControl);
			}
		}
	}

	public bool ShowPrimaryNavigationControl
	{
		get
		{
			return _showPrimaryNavigationControl;
		}
		set
		{
			if (_showPrimaryNavigationControl != value)
			{
				_showPrimaryNavigationControl = value;
				OnPropertyChanged(PropertyNames.ShowPrimaryNavigationControl);
			}
		}
	}

	public bool ShowSecondaryNavigationControl
	{
		get
		{
			return _showSecondaryNavigationControl;
		}
		set
		{
			if (_showSecondaryNavigationControl != value)
			{
				_showSecondaryNavigationControl = value;
				OnPropertyChanged(PropertyNames.ShowSecondaryNavigationControl);
			}
		}
	}

	public bool ShowSpanControl
	{
		get
		{
			return _showSpanControl;
		}
		set
		{
			if (_showSpanControl != value)
			{
				_showSpanControl = value;
				OnPropertyChanged(PropertyNames.ShowSpanControl);
			}
		}
	}

	public bool ShowSetupControl
	{
		get
		{
			return _showSetupControl;
		}
		set
		{
			if (_showSetupControl != value)
			{
				_showSetupControl = value;
				OnPropertyChanged(PropertyNames.ShowSetupControl);
			}
		}
	}

	public bool ShowTimelineAreaPlaybackControl
	{
		get
		{
			return _showTimelineAreaPlaybackControl;
		}
		set
		{
			if (_showTimelineAreaPlaybackControl != value)
			{
				_showTimelineAreaPlaybackControl = value;
				OnPropertyChanged(PropertyNames.ShowTimelineAreaPlaybackControl);
			}
		}
	}

	public bool ShowSpanContextMenuItem
	{
		get
		{
			return _showSpanContextMenuItem;
		}
		set
		{
			if (_showSpanContextMenuItem != value)
			{
				_showSpanContextMenuItem = value;
				OnPropertyChanged(PropertyNames.ShowSpanContextMenuItem);
			}
		}
	}

	public bool ShowPlaybackPlayPauseControls
	{
		get
		{
			return _showPlaybackPlayPauseControls;
		}
		set
		{
			if (_showPlaybackPlayPauseControls != value)
			{
				_showPlaybackPlayPauseControls = value;
				OnPropertyChanged(PropertyNames.ShowPlaybackPlayPauseControls);
			}
		}
	}

	public bool UserSelectionMarkerTimeChangeEnabled
	{
		get
		{
			return _userSelectionMarkerTimeChangeEnabled;
		}
		set
		{
			if (_userSelectionMarkerTimeChangeEnabled != value)
			{
				_userSelectionMarkerTimeChangeEnabled = value;
				OnPropertyChanged(PropertyNames.UserSelectionMarkerTimeChangeEnabled);
			}
		}
	}

	public bool UserSelectionControlEnabled
	{
		get
		{
			return _userSelectionControlEnabled;
		}
		set
		{
			if (_userSelectionControlEnabled != value)
			{
				_userSelectionControlEnabled = value;
				OnPropertyChanged(PropertyNames.UserSelectionControlEnabled);
			}
		}
	}

	public bool UserPlaybackNavigationControlEnabled
	{
		get
		{
			return _userPlaybackNavigationControlEnabled;
		}
		set
		{
			if (_userPlaybackNavigationControlEnabled != value)
			{
				_userPlaybackNavigationControlEnabled = value;
				OnPropertyChanged(PropertyNames.UserPlaybackNavigationControlEnabled);
			}
		}
	}

	public Brush TimelineLineBackgroundBrush
	{
		get
		{
			return _timelineLineBackgroundBrush;
		}
		set
		{
			if (_timelineLineBackgroundBrush != value)
			{
				_timelineLineBackgroundBrush = value;
				OnPropertyChanged(PropertyNames.TimelineLineBackgroundBrush);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillTopLineLowZOrderBrush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillTopLineLowZOrderBrush != value)
			{
				_sequenceSourceVisualAppearanceFillTopLineLowZOrderBrush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillTopLineHighZOrderBrush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillTopLineHighZOrderBrush != value)
			{
				_sequenceSourceVisualAppearanceFillTopLineHighZOrderBrush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush != value)
			{
				_sequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush != value)
			{
				_sequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush);
			}
		}
	}

	public BitmapImage SequenceSourceVisualAppearanceMarkerImage
	{
		get
		{
			return _sequenceSourceVisualAppearanceMarkerImage;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceMarkerImage != value)
			{
				_sequenceSourceVisualAppearanceMarkerImage = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceMarkerImage);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillExtraZOrder1Brush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillExtraZOrder1Brush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillExtraZOrder1Brush != value)
			{
				_sequenceSourceVisualAppearanceFillExtraZOrder1Brush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder1Brush);
			}
		}
	}

	public Brush SequenceSourceVisualAppearanceFillExtraZOrder2Brush
	{
		get
		{
			return _sequenceSourceVisualAppearanceFillExtraZOrder2Brush;
		}
		set
		{
			if (_sequenceSourceVisualAppearanceFillExtraZOrder2Brush != value)
			{
				_sequenceSourceVisualAppearanceFillExtraZOrder2Brush = value;
				OnPropertyChanged(PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder2Brush);
			}
		}
	}

	public bool SelectionInfoAvailable
	{
		get
		{
			return _selectionInfoAvailable;
		}
		set
		{
			if (_selectionInfoAvailable != value)
			{
				_selectionInfoAvailable = value;
				OnPropertyChanged(PropertyNames.SelectionInfoAvailable);
			}
		}
	}

	public string NoDataDisplayText
	{
		get
		{
			return _noDataDisplayText;
		}
		set
		{
			if (_noDataDisplayText != value)
			{
				_noDataDisplayText = value;
				OnPropertyChanged(PropertyNames.NoDataDisplayText);
			}
		}
	}

	public double PlaybackSliderMaxSpeed
	{
		get
		{
			return _playbackSliderMaxSpeed;
		}
		set
		{
			if (_playbackSliderMaxSpeed != value)
			{
				_playbackSliderMaxSpeed = value;
				OnPropertyChanged(PropertyNames.PlaybackSliderMaxSpeed);
			}
		}
	}

	public int TimelineAreaRulerSectionHeight
	{
		get
		{
			if (TimelineAreaDisplayMode != TimelineAreaDisplayMode.Full)
			{
				return 26;
			}
			return 30;
		}
	}

	public int TimelineAreaTracksSectionHeight
	{
		get
		{
			if ((ShowUpperTimelineTopLine || ShowUpperTimelineBottomLine || ShowUpperTimelineCustomDataLine) && (ShowLowerTimelineTopLine || ShowLowerTimelineBottomLine || ShowLowerTimelineCustomDataLine))
			{
				if (TimelineAreaDisplayMode != TimelineAreaDisplayMode.Full)
				{
					return 32;
				}
				return 46;
			}
			if (TimelineAreaDisplayMode != TimelineAreaDisplayMode.Full)
			{
				return 18;
			}
			return 26;
		}
	}

	public int TimelineAreaTracksMargin
	{
		get
		{
			if (TimelineAreaDisplayMode != TimelineAreaDisplayMode.Full)
			{
				return 4;
			}
			return 6;
		}
	}

	public int ControlsAreaUpperHeight
	{
		get
		{
			if (!ShowControlsAreaUpper)
			{
				return 0;
			}
			return (int)VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.ControlsAreaUpper.ControlsAreaUpperHeight;
		}
	}

	public int ControlsAreaLowerHeight
	{
		get
		{
			if (!ShowControlsAreaLower)
			{
				return 0;
			}
			return (int)VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower.ControlsAreaLower.ControlsAreaLowerHeight;
		}
	}

	public int TimelineTotalHeight => ControlsAreaUpperHeight + TimelineAreaRulerSectionHeight + TimelineAreaTracksSectionHeight + ControlsAreaLowerHeight;

	public event PropertyChangedEventHandler PropertyChanged;

	public AppearanceAndBehaviourController()
	{
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromMinutes(1.0), Strings.SpanLabel1Minute));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromMinutes(5.0), Strings.SpanLabel5Minutes));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromMinutes(10.0), Strings.SpanLabel10Minutes));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromMinutes(30.0), Strings.SpanLabel30Minutes));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromHours(1.0), Strings.SpanLabel1Hour));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromHours(2.0), Strings.SpanLabel2Hours));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromHours(4.0), Strings.SpanLabel4Hours));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromHours(8.0), Strings.SpanLabel8Hours));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromHours(16.0), Strings.SpanLabel16Hours));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromDays(1.0), Strings.SpanLabel1Day));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromDays(2.0), Strings.SpanLabel2Days));
		_availableTimelineSpans.Add(new TimelineSpan(TimeSpan.FromDays(7.0), Strings.SpanLabel1Week));
		UpdateTimelineSpans(_availableTimelineSpans[0].TimeSpan, _availableTimelineSpans[_availableTimelineSpans.Count - 1].TimeSpan);
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(0.1, Strings.SpeedLabelp10));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(0.25, Strings.SpeedLabelp25));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(0.5, Strings.SpeedLabelp50));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(1.0, Strings.SpeedLabel1));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(2.0, Strings.SpeedLabel2));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(4.0, Strings.SpeedLabel4));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(8.0, Strings.SpeedLabel8));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(16.0, Strings.SpeedLabel16));
		_availablePlaybackSpeeds.Add(new PlaybackSpeed(32.0, Strings.SpeedLabel32));
		UpdatePlaybackSpeeds(_availablePlaybackSpeeds[0].Speed, _availablePlaybackSpeeds[_availablePlaybackSpeeds.Count - 1].Speed);
		_playbackSpeed = _playbackSpeeds[3];
	}

	public void SetSequenceSources(IEnumerable<ISequenceSource> upperSequenceSources, IEnumerable<ISequenceSource> lowerSequenceSources)
	{
		_upperSequenceSources = upperSequenceSources;
		_lowerSequenceSources = lowerSequenceSources;
		OnPropertyChanged(PropertyNames.SequenceSources);
	}

	public void UpdatePlaybackSpeeds(double minSpeed, double maxSpeed)
	{
		List<PlaybackSpeed> list = _availablePlaybackSpeeds.Where((PlaybackSpeed playbackSpeed) => playbackSpeed.Speed >= minSpeed && playbackSpeed.Speed <= maxSpeed).ToList();
		if (list.Count > 0)
		{
			PlaybackSpeeds = list;
		}
	}

	public void UpdateTimelineSpans(TimeSpan minSpan, TimeSpan maxSpan)
	{
		List<TimelineSpan> list = _availableTimelineSpans.Where((TimelineSpan timelineSpan) => timelineSpan.TimeSpan >= minSpan && timelineSpan.TimeSpan <= maxSpan).ToList();
		if (list.Count > 0)
		{
			TimelineSpans = list;
		}
	}

	private void OnPropertyChanged(Enum fieldName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(fieldName.ToString()));
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public partial class PlaybackWpfUserControlMini : PlaybackWpfUserControlInternal, IComponentConnector
{
	private bool _mouseDownStartedInControl;

	private FQID _playbackControllerFQID;

	private PlaybackController _playbackController;

	private object _msg1;

	private object _msg2;

	private List<FQID> _sources;

	private bool _showTallUserControl;

	private bool _showSpeedControl;

	private bool _showTimeSpanControl;

	private string _playbackMode = "PlayStop";

	private readonly Pen GrayPen = new Pen(Brushes.Gray, 1.0);

	private DateTime _startTime;

	private DateTime _endTime;

	private string _timePattern;

	private string _hourPattern;

	private string _hourOfDayPattern;

	private double _rulerTimeFontSize = 11.0;

	private double _initialMouseX;

	private DateTime _minRequested = DateTime.MaxValue;

	private DateTime _maxRequested = DateTime.MinValue;

	private DateTime _minRequestedRecordings = DateTime.MaxValue;

	private DateTime _maxRequestedRecordings = DateTime.MinValue;

	private SequenceCache _cache = new SequenceCache();

	private SequenceCache _cacheRecordings = new SequenceCache();

	private bool _isMotionSequencesSupported;

	private bool _isRecordingSequencesSupported;

	private int _numberOfSequencesToRetreive = 50000;

	private DateTime _timeBeforeMouseMove;

	private DateTime _currentTime;

	private bool _isBeingDragged;

	private long _ticksPerPixel;

	private long _ticksPerMouseX;

	private double _speedLastPosition = 3.0;

	private string _currentCommand;

	private double _localSpeed;

	private bool _isSpeedFromServer;

	private bool _isRequestInProgress;

	private Item _item;

	private SequenceDataSource _dataSource;

	private readonly object _syncState = new object();

	private Model _model;

	public override bool ShowTallUserControl
	{
		get
		{
			return _showTallUserControl;
		}
		set
		{
			_showTallUserControl = value;
			UpdateShowSpeedControl();
			UpdateTimeSpanControl();
			MakeLowerUserControlVisiable(value);
		}
	}

	public override bool ShowSpeedControl
	{
		get
		{
			return _showSpeedControl;
		}
		set
		{
			_showSpeedControl = value;
			UpdateShowSpeedControl();
		}
	}

	public override bool ShowTimeSpanControl
	{
		get
		{
			return _showTimeSpanControl;
		}
		set
		{
			_showTimeSpanControl = value;
			UpdateTimeSpanControl();
		}
	}

	public override DateTime SelectionFromTime { get; set; }

	public override DateTime SelectionToTime { get; set; }

	public override TimeSpan TimeSpan
	{
		get
		{
			return TimeSpan.FromMinutes(_model.TimeSpanInMinutes);
		}
		set
		{
			_model.TimeSpanInMinutes = Math.Max((int)value.TotalMinutes, 1);
		}
	}

	private Item Item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
			if (_dataSource != null)
			{
				_dataSource.Close();
				_dataSource = null;
				ClearCache();
				ClearCacheRecordings();
			}
			if (_item == null)
			{
				return;
			}
			_dataSource = new SequenceDataSource(_item);
			List<DataType> types = _dataSource.GetTypes();
			_isMotionSequencesSupported = false;
			_isRecordingSequencesSupported = false;
			foreach (DataType item in types)
			{
				if (item.Id == DataType.SequenceTypeGuids.RecordingSequence)
				{
					_isRecordingSequencesSupported = true;
				}
				if (item.Id == DataType.SequenceTypeGuids.MotionSequence)
				{
					_isMotionSequencesSupported = true;
				}
			}
		}
	}

	protected override void OnMouseDown(MouseButtonEventArgs e)
	{
		_mouseDownStartedInControl = true;
		Mouse.Capture(this);
		_initialMouseX = Mouse.GetPosition(this).X;
		_timeBeforeMouseMove = _currentTime;
		_isBeingDragged = true;
		base.OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_mouseDownStartedInControl && _isBeingDragged && _timeBeforeMouseMove.Ticks != 0L)
		{
			long num = (long)((0.0 - (Mouse.GetPosition(this).X - _initialMouseX)) * (double)_ticksPerMouseX);
			DateTime newCenter = new DateTime(_timeBeforeMouseMove.Ticks + num, DateTimeKind.Local);
			UpdateUiTimeLine(newCenter);
			OnGoToCommand(_currentTime.ToUniversalTime());
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseButtonEventArgs e)
	{
		if (_mouseDownStartedInControl)
		{
			_mouseDownStartedInControl = false;
			Mouse.Capture(null);
			_isBeingDragged = false;
			long value = -(int)(Mouse.GetPosition(this).X - _initialMouseX) * _ticksPerMouseX;
			_timeBeforeMouseMove += TimeSpan.FromTicks(value);
			if ((!(_startTime >= _minRequested) || !(_startTime <= _maxRequested)) && (!(_endTime >= _minRequested) || !(_endTime <= _maxRequested)))
			{
				ClearCache();
			}
			if ((!(_startTime >= _minRequestedRecordings) || !(_startTime <= _maxRequestedRecordings)) && (!(_endTime >= _minRequestedRecordings) || !(_endTime <= _maxRequestedRecordings)))
			{
				ClearCacheRecordings();
			}
			UpdateUiDateTime(_timeBeforeMouseMove);
		}
		base.OnMouseUp(e);
	}

	public PlaybackWpfUserControlMini()
	{
		InitializeComponent();
		base.DataContext = (_model = new Model());
		base.Loaded += PlaybackWpfUserControlMini_Loaded;
		base.SizeChanged += PlaybackWpfUserControlMini_SizeChanged;
		_model.PropertyChanged += _model_PropertyChanged;
	}

	private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (_currentTime != DateTime.MinValue)
		{
			UpdateUiTimeLine(_currentTime);
		}
	}

	private void PlaybackWpfUserControlMini_Loaded(object sender, RoutedEventArgs e)
	{
		InitializeTimeline();
		UpdateUiTimeLine(DateTime.Now);
		ShowTallUserControl = false;
	}

	private void PlaybackWpfUserControlMini_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (_currentTime != DateTime.MinValue)
		{
			UpdateUiTimeLine(_currentTime);
		}
	}

	public override void Init(FQID playbackControllerFQID)
	{
		_playbackControllerFQID = playbackControllerFQID;
		_playbackController = ClientControl.Instance.GetPlaybackController(_playbackControllerFQID);
		_msg1 = EnvironmentManager.Instance.RegisterReceiver(TimeChangedHandler, new MessageIdFilter("SmartClient.PlaybackCurrentTimeIndication"));
		_msg2 = EnvironmentManager.Instance.RegisterReceiver(_playbackModeHandler, new MessageIdFilter("SmartClient.PlaybackIndication"));
		new TrackPageViewData(typeof(PlaybackWpfUserControlMini)).Send();
	}

	public override void Close()
	{
		EnvironmentManager.Instance.UnRegisterReceiver(_msg1);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg2);
		CloseTimeline();
	}

	public override void SetCameras(List<FQID> sources)
	{
		_sources = sources;
		if (sources != null && sources.Count > 0)
		{
			FQID fQID = sources[0];
			if (fQID.ServerId.UserContext != null)
			{
				Item = fQID.ServerId.UserContext.Configuration.GetItem(fQID);
			}
			else
			{
				Item = Configuration.Instance.GetItem(fQID);
			}
		}
		else
		{
			Item = null;
		}
	}

	public override void SetEnabled(bool enable)
	{
		if (enable)
		{
			if (_sources != null && _sources.Count > 0)
			{
				FQID fQID = _sources[0];
				if (fQID.ServerId.UserContext != null)
				{
					Item = fQID.ServerId.UserContext.Configuration.GetItem(fQID);
				}
				else
				{
					Item = Configuration.Instance.GetItem(fQID);
				}
			}
			Refresh();
		}
		else
		{
			Item = null;
			Refresh();
		}
	}

	public override void SetSequence(DateTime startTime, DateTime endTime)
	{
		if (_playbackController != null)
		{
			_playbackController.SetSequence(startTime, endTime);
		}
	}

	public void Refresh()
	{
		UpdateUiTimeLine(_currentTime);
	}

	private object TimeChangedHandler(Message message, FQID destination, FQID sender)
	{
		DateTime time = (DateTime)message.Data;
		if (IsForMe(sender))
		{
			if (time == DateTime.MaxValue)
			{
				EnvironmentManager.Instance.Log(error: true, "PlaybackWpfUserControlMini.TimeChangedHandler", "The datetime is set as max value");
				return null;
			}
			if (base.Dispatcher.CheckAccess())
			{
				UpdateUiTime(time.ToLocalTime());
			}
			else
			{
				base.Dispatcher.BeginInvoke((Action)delegate
				{
					UpdateUiTime(time.ToLocalTime());
				});
			}
		}
		return null;
	}

	private object _playbackModeHandler(Message message, FQID destination, FQID sender)
	{
		PlaybackCommandData command = message.Data as PlaybackCommandData;
		if (IsForMe(sender) && command != null)
		{
			if (base.Dispatcher.CheckAccess())
			{
				UpdatePlayUI(command.Command, command.Speed);
			}
			else
			{
				base.Dispatcher.BeginInvoke((Action)delegate
				{
					UpdatePlayUI(command.Command, command.Speed);
				});
			}
		}
		return null;
	}

	private void SendPlayCommandMessage(string command)
	{
		_currentCommand = command;
		PlaybackCommandData playbackCommandData = null;
		playbackCommandData = ((!(_currentCommand == "PlayForward") && !(_currentCommand == "PlayReverse") && !(_currentCommand == "Speed")) ? new PlaybackCommandData
		{
			Command = _currentCommand
		} : new PlaybackCommandData
		{
			Command = _currentCommand,
			Speed = _localSpeed
		});
		EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackCommand", playbackCommandData), _playbackControllerFQID);
	}

	private bool IsForMe(FQID sender)
	{
		if (sender == null)
		{
			return true;
		}
		if (_playbackControllerFQID == null)
		{
			return true;
		}
		return _playbackControllerFQID.EqualGuids(sender);
	}

	private void UpdatePlayUI(string playback, double speedFromServer)
	{
		switch (playback)
		{
		case "PlayForward":
		case "PlayReverse":
		case "PlayStop":
			_playbackMode = playback;
			if (playback == "PlayForward")
			{
				_playForwardUp.Content = _playForwardUp.FindResource("forwardPause");
				_playForwardDown.Content = _playForwardDown.FindResource("forwardPauseDown");
			}
			else
			{
				_playForwardUp.Content = _playForwardUp.FindResource("forwardPlay");
				_playForwardDown.Content = _playForwardDown.FindResource("forwardPlayDown");
			}
			if (playback == "PlayReverse")
			{
				_playBackwardUp.Content = _playBackwardUp.FindResource("backwardPause");
				_playBackwardDown.Content = _playBackwardDown.FindResource("backwardPauseDown");
			}
			else
			{
				_playBackwardUp.Content = _playBackwardUp.FindResource("backwardPlay");
				_playBackwardDown.Content = _playBackwardDown.FindResource("backwardPlayDown");
			}
			UpdateSpeedSlider(speedFromServer);
			break;
		}
		if (playback == "Speed")
		{
			UpdateSpeedSlider(speedFromServer);
		}
	}

	private void UpdateSpeedSlider(double speedFromServer)
	{
		if (speedFromServer == 0.0)
		{
			return;
		}
		if (SpeedSliderValueConvertor.SpeedToPosition.ContainsKey(speedFromServer))
		{
			if (speedFromServer != _localSpeed)
			{
				_isSpeedFromServer = true;
				_localSpeed = speedFromServer;
				_speedControlSlider.Value = SpeedSliderValueConvertor.SpeedToPosition[speedFromServer];
			}
		}
		else
		{
			EnvironmentManager.Instance.Log(error: true, "PlaybackWpfUserControlMini.UpdateSpeedSlider", "Speed is not supported");
		}
	}

	private void MakeLowerUserControlVisiable(bool value)
	{
		_previousFrameUp.Visibility = (value ? Visibility.Hidden : Visibility.Visible);
		_playBackwardUp.Visibility = (value ? Visibility.Hidden : Visibility.Visible);
		_playForwardUp.Visibility = (value ? Visibility.Hidden : Visibility.Visible);
		_nextFrameUp.Visibility = (value ? Visibility.Hidden : Visibility.Visible);
		_lowerControlGrid.Visibility = ((!value) ? Visibility.Collapsed : Visibility.Visible);
		base.Height = (value ? 100 : 50);
	}

	private void InitializeTimeline()
	{
		string timeSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator;
		string longTimePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;
		string shortDatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
		int num = longTimePattern.IndexOf(timeSeparator + "s", StringComparison.CurrentCulture);
		if (num >= 0)
		{
			for (int i = num + timeSeparator.Length + 1; i < longTimePattern.Length && longTimePattern[i] == 's'; i++)
			{
			}
		}
		_hourPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;
		_timePattern = shortDatePattern ?? "yyyy-MM-dd";
		_timePattern = _timePattern + " " + _hourPattern;
		_hourOfDayPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern + ".fff";
		_currentTime = DateTime.Now;
		_startTime = _currentTime - TimeSpan.FromMinutes(_model.TimeSpanInMinutes / 2);
		_endTime = _currentTime + TimeSpan.FromMinutes(_model.TimeSpanInMinutes / 2);
		ClearCache();
		ClearCacheRecordings();
	}

	private void ClearCache()
	{
		_cache.Clear();
		_minRequested = DateTime.MaxValue;
		_maxRequested = DateTime.MinValue;
	}

	private void ClearCacheRecordings()
	{
		_cacheRecordings.Clear();
		_minRequestedRecordings = DateTime.MaxValue;
		_maxRequestedRecordings = DateTime.MinValue;
	}

	private void CloseTimeline()
	{
		if (_dataSource != null)
		{
			_dataSource.Close();
			_dataSource = null;
		}
	}

	public void UpdateUiTime(DateTime _newCenter)
	{
		if (!_isBeingDragged)
		{
			UpdateUiDateTime(_newCenter);
		}
	}

	public void UpdateUiTimeLine(DateTime _newCenter)
	{
		UpdateUiDateTime(_newCenter);
		UpdateRuler();
	}

	private void UpdateUiDateTime(DateTime _newTime)
	{
		_currentTime = _newTime;
		_startTime = _currentTime - TimeSpan.FromMinutes((double)_model.TimeSpanInMinutes / 2.0);
		_endTime = _currentTime + TimeSpan.FromMinutes((double)_model.TimeSpanInMinutes / 2.0);
		labelDate.Text = _currentTime.ToShortDateString();
		labelTime.Text = _currentTime.ToString(_hourOfDayPattern, CultureInfo.CurrentCulture);
		labelStartTime.Text = _startTime.ToShortTimeString();
		labelEndTime.Text = _endTime.ToShortTimeString();
		DrawingGroup drawingGroup = new DrawingGroup();
		using (DrawingContext drawingContext = drawingGroup.Open())
		{
			drawingContext.DrawRectangle(Brushes.Gray, null, new Rect(base.ActualWidth / 2.0 - 1.0, 0.0, 3.0, base.ActualHeight / 2.0));
			DrawingImage source = new DrawingImage(drawingGroup);
			_centeralBar.Source = source;
		}
		DrawingGroup drawingGroup2 = new DrawingGroup();
		using (DrawingContext drawingContext2 = drawingGroup2.Open())
		{
			drawingContext2.DrawRectangle(Brushes.Black, null, new Rect(0.0, 38.0, base.ActualWidth, _recordingSequenceLine.Height));
			DrawingImage source2 = new DrawingImage(drawingGroup2);
			_recordingSequenceLine.Source = source2;
		}
		_ticksPerPixel = (_endTime.Ticks - _startTime.Ticks) / (int)base.ActualWidth;
		if (_ticksPerPixel == 0L)
		{
			_ticksPerPixel = 1L;
		}
		_ticksPerMouseX = (_endTime.Ticks - _startTime.Ticks) / (int)base.ActualWidth;
		if (Item != null && _dataSource != null && !_isRequestInProgress && !_isBeingDragged)
		{
			if (_isMotionSequencesSupported)
			{
				if (_startTime < _minRequested)
				{
					TimeSpan maxTimeAfter = TimeSpan.FromHours(1.0);
					if (_endTime > _minRequested)
					{
						maxTimeAfter = _cache.StartCache - _startTime - TimeSpan.FromSeconds(5.0);
					}
					else
					{
						ClearCache();
					}
					_isRequestInProgress = true;
					_dataSource.GetDataAsync(_syncState, _startTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter, _numberOfSequencesToRetreive, DataType.SequenceTypeGuids.MotionSequence, SequenceHandler);
				}
				else if (_maxRequested < _endTime)
				{
					if (_startTime > _cache.EndCache)
					{
						ClearCache();
					}
					DateTime dateTime = _startTime;
					if (_startTime < _cache.EndCache)
					{
						dateTime = _cache.EndCache;
					}
					TimeSpan maxTimeAfter2 = _endTime - dateTime;
					_isRequestInProgress = true;
					_dataSource.GetDataAsync(_syncState, dateTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter2, _numberOfSequencesToRetreive, DataType.SequenceTypeGuids.MotionSequence, SequenceHandler);
				}
				if (_startTime < _minRequested)
				{
					_minRequested = _startTime;
				}
				if (_endTime > _maxRequested)
				{
					if (_endTime < DateTime.Now)
					{
						_maxRequested = _endTime;
					}
					else
					{
						_maxRequested = DateTime.Now;
					}
				}
			}
			if (_isRecordingSequencesSupported)
			{
				if (_startTime < _minRequestedRecordings)
				{
					TimeSpan maxTimeAfter3 = TimeSpan.FromHours(1.0);
					if (_endTime > _minRequestedRecordings)
					{
						maxTimeAfter3 = _cacheRecordings.StartCache - _startTime - TimeSpan.FromSeconds(5.0);
					}
					else
					{
						ClearCacheRecordings();
					}
					_isRequestInProgress = true;
					_dataSource.GetDataAsync(_syncState, _startTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter3, _numberOfSequencesToRetreive, DataType.SequenceTypeGuids.RecordingSequence, SequenceRecordingsHandler);
				}
				else if (_maxRequestedRecordings < _endTime)
				{
					if (_startTime > _cacheRecordings.EndCache)
					{
						ClearCacheRecordings();
					}
					DateTime dateTime2 = _startTime;
					if (_startTime < _cacheRecordings.EndCache)
					{
						dateTime2 = _cacheRecordings.EndCache;
					}
					TimeSpan maxTimeAfter4 = _endTime - dateTime2;
					_isRequestInProgress = true;
					_dataSource.GetDataAsync(_syncState, dateTime2.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter4, _numberOfSequencesToRetreive, DataType.SequenceTypeGuids.RecordingSequence, SequenceRecordingsHandler);
				}
				if (_startTime < _minRequestedRecordings)
				{
					_minRequestedRecordings = _startTime;
				}
				if (_endTime > _maxRequestedRecordings)
				{
					if (_endTime < DateTime.Now)
					{
						_maxRequestedRecordings = _endTime;
					}
					else
					{
						_maxRequestedRecordings = DateTime.Now;
					}
				}
			}
		}
		DrawSequences();
	}

	private void UpdateRuler()
	{
		DrawingGroup drawingGroup = new DrawingGroup();
		using (DrawingContext drawingContext = drawingGroup.Open())
		{
			drawingContext.DrawLine(GrayPen, new Point(0.0, 0.0), new Point(base.ActualWidth, 0.0));
			TimeSpan timeSpan = _endTime - _startTime;
			string text = "";
			TimeSpan timeSpan2;
			if (timeSpan.TotalDays >= 1.0)
			{
				timeSpan2 = TimeSpan.FromDays((timeSpan.Days + 4) / 4);
				text = _timePattern;
			}
			else if (timeSpan.Hours > 6)
			{
				timeSpan2 = TimeSpan.FromHours((timeSpan.Hours + 2) / 4);
				text = _hourPattern;
			}
			else if (timeSpan.Hours >= 2)
			{
				timeSpan2 = TimeSpan.FromMinutes(30.0);
				text = _hourPattern;
			}
			else if (timeSpan.TotalMinutes == 60.0)
			{
				timeSpan2 = TimeSpan.FromMinutes(10.0);
				text = _hourPattern;
			}
			else if (timeSpan.TotalMinutes == 20.0)
			{
				timeSpan2 = TimeSpan.FromMinutes(5.0);
				text = _hourPattern;
			}
			else
			{
				timeSpan2 = TimeSpan.FromMinutes(1.0);
				text = _hourPattern;
			}
			long num = _startTime.Ticks - (_startTime.Ticks + timeSpan2.Ticks) % timeSpan2.Ticks;
			bool flag = false;
			while (!flag)
			{
				long num2 = (num - _startTime.Ticks) / _ticksPerPixel;
				if (num2 > 0)
				{
					drawingContext.DrawLine(GrayPen, new Point(num2, 2.0), new Point(num2, 5.0));
					FormattedText formattedText = new FormattedText(new DateTime(num).ToString(text, CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), _rulerTimeFontSize, Brushes.Gray, VisualTreeHelper.GetDpi(this).PixelsPerDip);
					drawingContext.DrawText(formattedText, new Point((int)num2 - 14, 6.0));
				}
				num += timeSpan2.Ticks;
				flag = num > _endTime.Ticks;
			}
		}
		DrawingImage source = new DrawingImage(drawingGroup);
		ruler.Source = source;
	}

	private void TimelineControl_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
		{
			int timeSpanInMinutes = _model.TimeSpanInMinutes;
			int num = TimeSpanSliderValueConvertor.TimeSpanTotalMinutesToPosition[timeSpanInMinutes];
			if (e.Delta > 0 && num < TimeSpanSliderValueConvertor.PositionToTimeSpanTotalMinutes.Length - 1)
			{
				_model.TimeSpanInMinutes = TimeSpanSliderValueConvertor.PositionToTimeSpanTotalMinutes[num + 1];
			}
			else if (e.Delta < 0 && num > 0)
			{
				_model.TimeSpanInMinutes = TimeSpanSliderValueConvertor.PositionToTimeSpanTotalMinutes[num - 1];
			}
			UpdateUiTimeLine(_currentTime);
		}
		else
		{
			UpdateUiTimeLine(new DateTime(_currentTime.Ticks + _ticksPerPixel * Math.Sign(e.Delta) * 20));
			OnGoToCommand(_currentTime.ToUniversalTime());
		}
	}

	private void SequenceHandler(object objects, object syncState)
	{
		if (!base.Dispatcher.CheckAccess())
		{
			base.Dispatcher.BeginInvoke(new AsyncCallbackHandler(SequenceHandler), objects, syncState);
			return;
		}
		SequenceData[] array = objects as SequenceData[];
		SequenceData[] array2 = array;
		foreach (SequenceData data in array2)
		{
			_cache.Add(data);
		}
		_isRequestInProgress = false;
		if (array.Length != 0)
		{
			DrawSequences();
		}
	}

	private void SequenceRecordingsHandler(object objects, object syncState)
	{
		if (!base.Dispatcher.CheckAccess())
		{
			base.Dispatcher.BeginInvoke(new AsyncCallbackHandler(SequenceRecordingsHandler), objects, syncState);
			return;
		}
		SequenceData[] array = objects as SequenceData[];
		SequenceData[] array2 = array;
		foreach (SequenceData data in array2)
		{
			_cacheRecordings.Add(data);
		}
		_isRequestInProgress = false;
		if (array.Length != 0)
		{
			DrawSequences();
		}
	}

	private void DrawSequences()
	{
		DrawingGroup drawingGroup = new DrawingGroup();
		using DrawingContext drawingContext = drawingGroup.Open();
		drawingContext.DrawRectangle(Brushes.Black, null, new Rect(0.0, 38.0, base.ActualWidth, _recordingSequenceLine.Height));
		foreach (SequenceData item in _cacheRecordings.Items)
		{
			DateTime dateTime = item.EventSequence.StartDateTime.ToLocalTime();
			DateTime dateTime2 = item.EventSequence.EndDateTime.ToLocalTime();
			if (dateTime < _endTime && dateTime2 > _startTime)
			{
				if (dateTime < _startTime)
				{
					dateTime = _startTime;
				}
				if (dateTime2 > _endTime)
				{
					dateTime2 = _endTime;
				}
				long num = (dateTime.Ticks - _startTime.Ticks) / _ticksPerPixel;
				long num2 = (dateTime2.Ticks - _startTime.Ticks) / _ticksPerPixel;
				drawingContext.DrawRectangle(Brushes.LightPink, null, new Rect(num + 2, 38.0, num2 - num + 2, 8.0));
			}
		}
		foreach (SequenceData item2 in _cache.Items)
		{
			DateTime dateTime3 = item2.EventSequence.StartDateTime.ToLocalTime();
			DateTime dateTime4 = item2.EventSequence.EndDateTime.ToLocalTime();
			if (dateTime3 < _endTime && dateTime4 > _startTime)
			{
				if (dateTime3 < _startTime)
				{
					dateTime3 = _startTime;
				}
				if (dateTime4 > _endTime)
				{
					dateTime4 = _endTime;
				}
				long num3 = (dateTime3.Ticks - _startTime.Ticks) / _ticksPerPixel;
				long num4 = (dateTime4.Ticks - _startTime.Ticks) / _ticksPerPixel;
				drawingContext.DrawRectangle(Brushes.Red, null, new Rect(num3 + 2, 38.0, num4 - num3 + 2, 8.0));
			}
		}
		DrawingImage source = new DrawingImage(drawingGroup);
		_recordingSequenceLine.Source = source;
	}

	private void OnGoToCommand(DateTime dateTime)
	{
		_currentCommand = "Goto";
		EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackCommand", new PlaybackCommandData
		{
			Command = _currentCommand,
			DateTime = dateTime
		}), _playbackControllerFQID);
	}

	private void _previousFrameUp_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("Previous");
	}

	private void _playBackwardUp_Click(object sender, RoutedEventArgs e)
	{
		if (_playbackMode == "PlayReverse")
		{
			SendPlayCommandMessage("PlayStop");
		}
		else
		{
			SendPlayCommandMessage("PlayReverse");
		}
	}

	private void _playForwardUp_Click(object sender, RoutedEventArgs e)
	{
		if (_playbackMode == "PlayForward")
		{
			SendPlayCommandMessage("PlayStop");
		}
		else
		{
			SendPlayCommandMessage("PlayForward");
		}
	}

	private void _nextFrameUp_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("Next");
	}

	private void _firstSequenceDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("Begin");
	}

	private void _previousSequenceDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("PreviousSequence");
	}

	private void _previousFrameDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("Previous");
	}

	private void _playBackwardDown_Click(object sender, RoutedEventArgs e)
	{
		if (_playbackMode == "PlayReverse")
		{
			SendPlayCommandMessage("PlayStop");
		}
		else
		{
			SendPlayCommandMessage("PlayReverse");
		}
	}

	private void _playForwardDown_Click(object sender, RoutedEventArgs e)
	{
		if (_playbackMode == "PlayForward")
		{
			SendPlayCommandMessage("PlayStop");
		}
		else
		{
			SendPlayCommandMessage("PlayForward");
		}
	}

	private void _nextFrameDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("Next");
	}

	private void _nextSequenceDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("NextSequence");
	}

	private void _lastSequenceDown_Click(object sender, RoutedEventArgs e)
	{
		SendPlayCommandMessage("End");
	}

	private void UpdateShowSpeedControl()
	{
		Slider speedControlSlider = _speedControlSlider;
		Visibility visibility = (_speedControlTextBlock.Visibility = ((!ShowSpeedControl || !ShowTallUserControl) ? Visibility.Hidden : Visibility.Visible));
		speedControlSlider.Visibility = visibility;
	}

	private void _speedControlSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		Slider obj = sender as Slider;
		bool flag = false;
		double value = obj.Value;
		_localSpeed = SpeedSliderValueConvertor.PositionToSpeed[value];
		if (_speedLastPosition != value)
		{
			flag = true;
			_speedLastPosition = value;
		}
		if (flag && !_isSpeedFromServer)
		{
			if (_currentCommand == "PlayForward" || _currentCommand == "PlayReverse")
			{
				SendPlayCommandMessage(_currentCommand);
			}
			else
			{
				SendPlayCommandMessage("Speed");
			}
		}
		_isSpeedFromServer = false;
	}

	private void UpdateTimeSpanControl()
	{
		Slider timespanControlSlider = _timespanControlSlider;
		Visibility visibility = (_timespanControlTextBlock.Visibility = ((!ShowTimeSpanControl || !ShowTallUserControl) ? Visibility.Hidden : Visibility.Visible));
		timespanControlSlider.Visibility = visibility;
	}
}

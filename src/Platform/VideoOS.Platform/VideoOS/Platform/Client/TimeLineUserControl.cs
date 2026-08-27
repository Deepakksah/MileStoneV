using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Platform.Data;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use PlaybackWpfUserControl instead.", false)]
public class TimeLineUserControl : UserControl
{
	public delegate void TimeEventHandler(object sender, TimeEventArgs e);

	private Bitmap _timeLine = new Bitmap(2000, 8);

	private Graphics _graphics;

	private DateTime _startTime;

	private DateTime _endTime;

	private string _timePattern;

	private string _hourPattern;

	private string _hourOfDayPattern;

	private DateTime _minRequested = DateTime.MaxValue;

	private DateTime _maxRequested = DateTime.MinValue;

	private DateTime _minRequestedRecordings = DateTime.MaxValue;

	private DateTime _maxRequestedRecordings = DateTime.MinValue;

	private SequenceCache _cache = new SequenceCache();

	private SequenceCache _cacheRecordings = new SequenceCache();

	private bool _isMotionSequencesSupported;

	private bool _isRecordingSequencesSupported;

	private long _ticksPerPixel;

	private long _ticksPerMouseX;

	private bool _isRequestInProgress;

	private Item _item;

	private SequenceDataSource _dataSource;

	private int _dateOffset = -70;

	private int _timeOffset = 10;

	private Font _smallFont;

	private DateTime _timeBeforeMouseMove;

	private int _timeSpanInMinutes = 60;

	private readonly object _syncState = new object();

	private int _initialMouseX;

	private bool _isMouseDown;

	private IContainer components;

	private PictureBox horizontalDragLine;

	private Label labelTime;

	private Label labelDate;

	private Label labelStartTime;

	private Label labelEndTime;

	private UserControl userControlRuler;

	private PictureBox pictureBox1;

	public DateTime CurrentTime { get; set; }

	public Item Item
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

	public TimeSpan TimeSpan
	{
		get
		{
			return TimeSpan.FromMinutes(_timeSpanInMinutes);
		}
		set
		{
			if ((double)_timeSpanInMinutes != value.TotalMinutes)
			{
				_timeSpanInMinutes = Math.Max((int)value.TotalMinutes, 1);
				SetShowTimePrivate(CurrentTime);
				userControlRuler.Invalidate();
			}
		}
	}

	public event TimeEventHandler MouseSetTimeEvent;

	public event EventHandler TimeSpanInMinutesChanged;

	public TimeLineUserControl()
	{
		InitializeComponent();
		string timeSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator;
		string longTimePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;
		string shortDatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
		int num = longTimePattern.IndexOf(timeSeparator + "s");
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
		CurrentTime = DateTime.Now;
	}

	public void Init()
	{
		labelDate.Text = DateTime.MinValue.ToShortDateString();
		_smallFont = new Font(Font.FontFamily, 7f);
		horizontalDragLine.MouseWheel += OnMouseWheel;
		base.MouseWheel += OnMouseWheel;
		userControlRuler.Paint += UserControlRulerPaint;
		CurrentTime = DateTime.Now;
		_startTime = CurrentTime - TimeSpan.FromMinutes(_timeSpanInMinutes / 2);
		_endTime = CurrentTime + TimeSpan.FromMinutes(_timeSpanInMinutes / 2);
		ClearCache();
		ClearCacheRecordings();
		new TrackPageViewData(typeof(TimeLineUserControl), deprecated: true).Send();
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

	public void Close()
	{
		if (_graphics != null)
		{
			_graphics.Dispose();
		}
		_graphics = null;
		if (_timeLine != null)
		{
			_timeLine.Dispose();
		}
		if (_dataSource != null)
		{
			_dataSource.Close();
			_dataSource = null;
		}
		if (_smallFont != null)
		{
			_smallFont.Dispose();
			_smallFont = null;
		}
	}

	public void RefreshAll()
	{
		SetShowTimePrivate(CurrentTime);
		ShowRuler();
	}

	public void SetShowTime(DateTime newCenter)
	{
		if (!_isMouseDown)
		{
			SetShowTimePrivate(newCenter);
		}
	}

	private void SetShowTimePrivate(DateTime newCenter)
	{
		if (horizontalDragLine.Width == 0 || horizontalDragLine.Height == 0)
		{
			return;
		}
		if (CurrentTime != newCenter)
		{
			userControlRuler.Invalidate();
		}
		CurrentTime = newCenter;
		_startTime = CurrentTime - TimeSpan.FromMinutes((double)_timeSpanInMinutes / 2.0);
		_endTime = CurrentTime + TimeSpan.FromMinutes((double)_timeSpanInMinutes / 2.0);
		if (_graphics != null)
		{
			_graphics.Dispose();
		}
		_timeLine = new Bitmap(horizontalDragLine.Width, horizontalDragLine.Height);
		_graphics = Graphics.FromImage(_timeLine);
		_ticksPerPixel = (_endTime.Ticks - _startTime.Ticks) / _timeLine.Width;
		if (_ticksPerPixel == 0L)
		{
			_ticksPerPixel = 1L;
		}
		_ticksPerMouseX = (_endTime.Ticks - _startTime.Ticks) / horizontalDragLine.Width;
		_graphics.FillRectangle(Brushes.Black, 0, 0, _timeLine.Width, _timeLine.Height);
		horizontalDragLine.Image = new Bitmap(_timeLine, horizontalDragLine.Size);
		labelDate.Text = CurrentTime.ToShortDateString();
		labelTime.Text = CurrentTime.ToString(_hourOfDayPattern, CultureInfo.InvariantCulture);
		labelStartTime.Text = _startTime.ToShortTimeString();
		labelEndTime.Text = _endTime.ToShortTimeString();
		if (Item != null && _dataSource != null && !_isRequestInProgress && !_isMouseDown)
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
					_dataSource.GetDataAsync(_syncState, _startTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter, 10000, DataType.SequenceTypeGuids.MotionSequence, SequenceHandler);
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
					_dataSource.GetDataAsync(_syncState, dateTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter2, 10000, DataType.SequenceTypeGuids.MotionSequence, SequenceHandler);
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
					_dataSource.GetDataAsync(_syncState, _startTime.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter3, 10000, DataType.SequenceTypeGuids.RecordingSequence, SequenceRecordingsHandler);
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
					_dataSource.GetDataAsync(_syncState, dateTime2.ToUniversalTime(), TimeSpan.FromSeconds(0.0), 0, maxTimeAfter4, 10000, DataType.SequenceTypeGuids.RecordingSequence, SequenceRecordingsHandler);
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
		DisplayLine();
	}

	private void SequenceHandler(object objects, object syncState)
	{
		if (base.InvokeRequired)
		{
			BeginInvoke(new AsyncCallbackHandler(SequenceHandler), objects, syncState);
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
			DisplayLine();
		}
	}

	private void SequenceRecordingsHandler(object objects, object syncState)
	{
		if (base.InvokeRequired)
		{
			BeginInvoke(new AsyncCallbackHandler(SequenceRecordingsHandler), objects, syncState);
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
			DisplayLine();
		}
	}

	private void DisplayLine()
	{
		_graphics.FillRectangle(Brushes.Black, 0, 0, _timeLine.Width, _timeLine.Height);
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
				_graphics.FillRectangle(Brushes.LightPink, num + 2, 0f, num2 - num + 2, 8f);
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
				_graphics.FillRectangle(Brushes.Red, num3 + 2, 0f, num4 - num3 + 2, 8f);
			}
		}
		horizontalDragLine.Image = new Bitmap(_timeLine, horizontalDragLine.Size);
	}

	private void UserControlRulerPaint(object sender, PaintEventArgs e)
	{
		ShowRuler();
	}

	private void ShowRuler()
	{
		Brush brush = new SolidBrush(userControlRuler.BackColor);
		if (userControlRuler.Width == 0)
		{
			return;
		}
		Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
		Graphics graphics = Graphics.FromImage(image);
		graphics.FillRectangle(brush, 0, 0, userControlRuler.Width, userControlRuler.Height);
		graphics.DrawLine(Pens.Gray, 0, 0, userControlRuler.Width - 2, 0);
		TimeSpan timeSpan = _endTime - _startTime;
		string text = "";
		TimeSpan timeSpan2;
		if (timeSpan.Days > 0)
		{
			timeSpan2 = TimeSpan.FromDays((timeSpan.Days + 4) / 4);
			text = _timePattern;
		}
		else if (timeSpan.Hours > 6)
		{
			timeSpan2 = TimeSpan.FromHours((timeSpan.Hours + 2) / 4);
			text = _hourPattern;
		}
		else if (timeSpan.Hours > 1)
		{
			timeSpan2 = TimeSpan.FromMinutes(30.0);
			text = _hourPattern;
		}
		else
		{
			timeSpan2 = TimeSpan.FromMinutes(10.0);
			text = _hourPattern;
		}
		long num = _startTime.Ticks - (_startTime.Ticks + timeSpan2.Ticks) % timeSpan2.Ticks;
		bool flag = false;
		while (!flag)
		{
			long num2 = (num - _startTime.Ticks) / _ticksPerPixel;
			if (num2 > 0)
			{
				graphics.DrawLine(Pens.Gray, num2, 2f, num2, 5f);
				graphics.DrawString(new DateTime(num).ToString(text, CultureInfo.CurrentCulture), _smallFont, Brushes.Black, new Rectangle((int)num2 - 14, 6, (int)num2 + 20, 14));
			}
			num += timeSpan2.Ticks;
			flag = num > _endTime.Ticks;
		}
		graphics.Dispose();
		pictureBox1.Image = image;
	}

	private void OnMouseDown(object sender, MouseEventArgs e)
	{
		_initialMouseX = e.X;
		_timeBeforeMouseMove = CurrentTime;
		_isMouseDown = true;
	}

	private void OnMouseMove(object sender, MouseEventArgs e)
	{
		if (_isMouseDown && _timeBeforeMouseMove.Ticks != 0L)
		{
			long num = -(e.X - _initialMouseX) * _ticksPerMouseX;
			DateTime showTimePrivate = new DateTime(_timeBeforeMouseMove.Ticks + num, DateTimeKind.Local);
			SetShowTimePrivate(showTimePrivate);
			if (this.MouseSetTimeEvent != null)
			{
				this.MouseSetTimeEvent(this, new TimeEventArgs
				{
					Time = CurrentTime.ToUniversalTime()
				});
			}
		}
	}

	private void OnResize(object sender, EventArgs e)
	{
		if (base.Width > 0)
		{
			int num = base.Width / 2;
			labelDate.Location = new Point(num + _dateOffset, labelDate.Top);
			labelTime.Location = new Point(num + _timeOffset, labelTime.Top);
			userControlRuler.Invalidate();
			if (!base.DesignMode && CurrentTime != DateTime.MinValue)
			{
				SetShowTimePrivate(CurrentTime);
			}
		}
	}

	private void OnMouseUp(object sender, MouseEventArgs e)
	{
		if (_isMouseDown)
		{
			_isMouseDown = false;
			long value = -(e.X - _initialMouseX) * _ticksPerMouseX;
			_timeBeforeMouseMove += TimeSpan.FromTicks(value);
			if ((!(_startTime >= _minRequested) || !(_startTime <= _maxRequested)) && (!(_endTime >= _minRequested) || !(_endTime <= _maxRequested)))
			{
				ClearCache();
			}
			if ((!(_startTime >= _minRequestedRecordings) || !(_startTime <= _maxRequestedRecordings)) && (!(_endTime >= _minRequestedRecordings) || !(_endTime <= _maxRequestedRecordings)))
			{
				ClearCacheRecordings();
			}
			SetShowTimePrivate(_timeBeforeMouseMove);
		}
	}

	private void OnMouseLeave(object sender, EventArgs e)
	{
		_isMouseDown = false;
		_timeBeforeMouseMove = CurrentTime;
	}

	private void OnClick(object sender, EventArgs e)
	{
		Focus();
	}

	private void OnMouseWheel(object sender, MouseEventArgs e)
	{
		if ((Control.ModifierKeys & Keys.Control) != Keys.None)
		{
			int num = PlaybackUserControlMini.TimeSpanTotalMinutesToPosition[_timeSpanInMinutes];
			if (e.Delta > 0 && num < PlaybackUserControlMini.PositionToTimeSpanTotalMinutes.Length - 1)
			{
				_timeSpanInMinutes = PlaybackUserControlMini.PositionToTimeSpanTotalMinutes[num + 1];
			}
			else if (e.Delta < 0 && num > 0)
			{
				_timeSpanInMinutes = PlaybackUserControlMini.PositionToTimeSpanTotalMinutes[num - 1];
			}
			this.TimeSpanInMinutesChanged?.Invoke(null, EventArgs.Empty);
			SetShowTimePrivate(CurrentTime);
			userControlRuler.Invalidate();
		}
		else
		{
			SetShowTimePrivate(new DateTime(CurrentTime.Ticks + _ticksPerPixel * Math.Sign(e.Delta) * 20));
			if (this.MouseSetTimeEvent != null)
			{
				this.MouseSetTimeEvent(this, new TimeEventArgs
				{
					Time = CurrentTime.ToUniversalTime()
				});
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.horizontalDragLine = new System.Windows.Forms.PictureBox();
		this.labelTime = new System.Windows.Forms.Label();
		this.labelDate = new System.Windows.Forms.Label();
		this.labelStartTime = new System.Windows.Forms.Label();
		this.labelEndTime = new System.Windows.Forms.Label();
		this.userControlRuler = new System.Windows.Forms.UserControl();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.horizontalDragLine).BeginInit();
		this.userControlRuler.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.horizontalDragLine.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.horizontalDragLine.BackColor = System.Drawing.Color.Black;
		this.horizontalDragLine.Location = new System.Drawing.Point(0, 38);
		this.horizontalDragLine.Name = "horizontalDragLine";
		this.horizontalDragLine.Size = new System.Drawing.Size(460, 8);
		this.horizontalDragLine.TabIndex = 1;
		this.horizontalDragLine.TabStop = false;
		this.horizontalDragLine.Click += new System.EventHandler(OnClick);
		this.horizontalDragLine.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
		this.horizontalDragLine.MouseLeave += new System.EventHandler(OnMouseLeave);
		this.horizontalDragLine.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
		this.horizontalDragLine.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
		this.labelTime.AutoSize = true;
		this.labelTime.Location = new System.Drawing.Point(264, 23);
		this.labelTime.Name = "labelTime";
		this.labelTime.Size = new System.Drawing.Size(70, 13);
		this.labelTime.TabIndex = 11;
		this.labelTime.Text = "12:00:00.000";
		this.labelDate.AutoSize = true;
		this.labelDate.Location = new System.Drawing.Point(162, 23);
		this.labelDate.Name = "labelDate";
		this.labelDate.Size = new System.Drawing.Size(61, 13);
		this.labelDate.TabIndex = 10;
		this.labelDate.Text = "2014-01-01";
		this.labelStartTime.AutoSize = true;
		this.labelStartTime.Location = new System.Drawing.Point(3, 23);
		this.labelStartTime.Name = "labelStartTime";
		this.labelStartTime.Size = new System.Drawing.Size(34, 13);
		this.labelStartTime.TabIndex = 13;
		this.labelStartTime.Text = "00:00";
		this.labelEndTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.labelEndTime.AutoSize = true;
		this.labelEndTime.Location = new System.Drawing.Point(410, 23);
		this.labelEndTime.Name = "labelEndTime";
		this.labelEndTime.Size = new System.Drawing.Size(34, 13);
		this.labelEndTime.TabIndex = 12;
		this.labelEndTime.Text = "99:99";
		this.userControlRuler.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.userControlRuler.Controls.Add(this.labelStartTime);
		this.userControlRuler.Controls.Add(this.labelDate);
		this.userControlRuler.Controls.Add(this.labelEndTime);
		this.userControlRuler.Controls.Add(this.labelTime);
		this.userControlRuler.Controls.Add(this.pictureBox1);
		this.userControlRuler.Location = new System.Drawing.Point(0, 0);
		this.userControlRuler.Name = "userControlRuler";
		this.userControlRuler.Size = new System.Drawing.Size(460, 38);
		this.userControlRuler.TabIndex = 14;
		this.userControlRuler.Click += new System.EventHandler(OnClick);
		this.userControlRuler.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
		this.userControlRuler.MouseLeave += new System.EventHandler(OnMouseLeave);
		this.userControlRuler.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
		this.userControlRuler.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
		this.userControlRuler.Resize += new System.EventHandler(OnResize);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(460, 38);
		this.pictureBox1.TabIndex = 15;
		this.pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.userControlRuler);
		base.Controls.Add(this.horizontalDragLine);
		this.DoubleBuffered = true;
		base.Name = "TimeLineUserControl";
		base.Size = new System.Drawing.Size(460, 46);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
		base.MouseLeave += new System.EventHandler(OnMouseLeave);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
		base.Resize += new System.EventHandler(OnResize);
		((System.ComponentModel.ISupportInitialize)this.horizontalDragLine).EndInit();
		this.userControlRuler.ResumeLayout(false);
		this.userControlRuler.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

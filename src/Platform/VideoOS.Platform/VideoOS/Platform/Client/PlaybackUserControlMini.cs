using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Properties;
using VideoOS.Platform.Resources;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use PlaybackWpfUserControl instead and use ShowTallUserControl to control the size.", false)]
public class PlaybackUserControlMini : PlaybackUserControl
{
	private static Dictionary<int, string> PositionToSpeed = new Dictionary<int, string>
	{
		{ 0, "0.1" },
		{ 1, "0.25" },
		{ 2, "0.5" },
		{ 3, "1" },
		{ 4, "2" },
		{ 5, "4" },
		{ 6, "8" },
		{ 7, "16" },
		{ 8, "32" }
	};

	private static Dictionary<double, int> SpeedToPosition = new Dictionary<double, int>
	{
		{ 0.1, 0 },
		{ 0.25, 1 },
		{ 0.5, 2 },
		{ 1.0, 3 },
		{ 2.0, 4 },
		{ 4.0, 5 },
		{ 8.0, 6 },
		{ 16.0, 7 },
		{ 32.0, 8 }
	};

	private static Dictionary<int, string> TimeSpanTotalMinutesToText = new Dictionary<int, string>
	{
		{ 1, "1 minute" },
		{ 2, "2 minutes" },
		{ 5, "5 minutes" },
		{ 10, "10 minutes" },
		{ 20, "20 minutes" },
		{ 60, "1 hour" },
		{ 120, "2 hours" },
		{ 240, "4 hours" },
		{ 480, "8 hours" },
		{ 720, "12 hours" },
		{ 960, "16 hours" },
		{ 1200, "20 hours" },
		{ 1440, "1 day" },
		{ 2880, "2 days" },
		{ 5760, "4 days" },
		{ 10080, "1 week" },
		{ 20160, "2 weeks" },
		{ 40320, "4 weeks" }
	};

	public static int[] PositionToTimeSpanTotalMinutes = new int[18]
	{
		1, 2, 5, 10, 20, 60, 120, 240, 480, 720,
		960, 1200, 1440, 2880, 5760, 10080, 20160, 40320
	};

	public static Dictionary<int, int> TimeSpanTotalMinutesToPosition = new Dictionary<int, int>
	{
		{ 1, 0 },
		{ 2, 1 },
		{ 5, 2 },
		{ 10, 3 },
		{ 20, 4 },
		{ 60, 5 },
		{ 120, 6 },
		{ 240, 7 },
		{ 480, 8 },
		{ 720, 9 },
		{ 960, 10 },
		{ 1200, 11 },
		{ 1440, 12 },
		{ 2880, 13 },
		{ 5760, 14 },
		{ 10080, 15 },
		{ 20160, 16 },
		{ 40320, 17 }
	};

	private IFormatProvider format = CultureInfo.CurrentCulture;

	private FQID _playbackControllerFQID;

	private PlaybackController _playbackController;

	private object _msg1;

	private object _msg2;

	private string _playmode = "PlayStop";

	private List<FQID> _sources;

	private bool _showTallUserControl;

	private const int DefaultDistanceBetweenButton = 3;

	private const int DefaultButtonWidth = 26;

	private const int DefaultSpaceOfTimeLabelFromCenter = 105;

	private const int DefaultButtonTop = 17;

	private const int DefaultLabelTop = 34;

	private const int DefaultSpeedLabelOffset = 66;

	private const int DefaultTimeSpaneLableOffset = 37;

	private double _speed = 1.0;

	private int _speedLastPosition;

	private bool _showSpeedControl;

	private bool _showTimeSpanControl;

	private string _currentCommand;

	private IContainer components;

	private Button buttonPrevSeq;

	private Button buttonPlayBackward;

	private Button buttonPlayForward;

	private Button buttonNextSeq;

	private Button buttonDbStart;

	private Button buttonDbEnd;

	private PictureBox pictureBoxCenter;

	private Panel panelCenter;

	private Button buttonFrameNext;

	private Button buttonFramePrev;

	private TimeLineUserControl timeLineUserControl1;

	private Button buttonFrameNext2;

	private Button buttonFramePrev2;

	private Button buttonPlayBackward2;

	private Button buttonPlayForward2;

	private Label labelSpeed;

	private Label labelTimeSpan;

	private TrackBar trackBarSpeed;

	private TrackBar trackBarTimeSpan;

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
			return timeLineUserControl1.TimeSpan;
		}
		set
		{
			timeLineUserControl1.TimeSpan = value;
			UpdateTrackBarTimeSpan(value);
		}
	}

	public PlaybackUserControlMini()
	{
		InitializeComponent();
		_speedLastPosition = 3;
		pictureBoxCenter.BringToFront();
		MakeLowerUserControlVisiable(_showTallUserControl);
		SelectionFromTime = DateTime.MinValue;
		SelectionToTime = DateTime.MaxValue;
	}

	public override void Init(FQID playbackControllerFQID)
	{
		_playbackControllerFQID = playbackControllerFQID;
		_playbackController = ClientControl.Instance.GetPlaybackController(_playbackControllerFQID);
		timeLineUserControl1.Init();
		timeLineUserControl1.MouseSetTimeEvent += timeLineUserControl1_MouseSetTimeEvent;
		timeLineUserControl1.TimeSpanInMinutesChanged += TimeLineUserControl1_TimeSpanInMinutesChanged;
		UpdateTime(DateTime.UtcNow);
		_msg1 = EnvironmentManager.Instance.RegisterReceiver(TimeChangedHandler, new MessageIdFilter("SmartClient.PlaybackCurrentTimeIndication"));
		_msg2 = EnvironmentManager.Instance.RegisterReceiver(PlaybackModeHandler, new MessageIdFilter("SmartClient.PlaybackIndication"));
		new TrackPageViewData(typeof(PlaybackUserControlMini), deprecated: true).Send();
	}

	public override void Close()
	{
		EnvironmentManager.Instance.UnRegisterReceiver(_msg1);
		EnvironmentManager.Instance.UnRegisterReceiver(_msg2);
		timeLineUserControl1.MouseSetTimeEvent -= timeLineUserControl1_MouseSetTimeEvent;
		timeLineUserControl1.TimeSpanInMinutesChanged -= TimeLineUserControl1_TimeSpanInMinutesChanged;
		timeLineUserControl1.Close();
	}

	private void MakeLowerUserControlVisiable(bool value)
	{
		buttonFrameNext2.Visible = !value;
		buttonFramePrev2.Visible = !value;
		buttonPlayBackward2.Visible = !value;
		buttonPlayForward2.Visible = !value;
		panelCenter.Visible = value;
		buttonFrameNext2.BringToFront();
		buttonFramePrev2.BringToFront();
		buttonPlayBackward2.BringToFront();
		buttonPlayForward2.BringToFront();
		base.Height = (value ? 100 : 50);
	}

	private void UpdateShowSpeedControl()
	{
		TrackBar trackBar = trackBarSpeed;
		bool visible = (labelSpeed.Visible = ShowSpeedControl && ShowTallUserControl);
		trackBar.Visible = visible;
	}

	private void UpdateTimeSpanControl()
	{
		TrackBar trackBar = trackBarTimeSpan;
		bool visible = (labelTimeSpan.Visible = ShowTimeSpanControl && ShowTallUserControl);
		trackBar.Visible = visible;
	}

	public override void SetSequence(DateTime startTime, DateTime endTime)
	{
		if (_playbackController != null)
		{
			_playbackController.SetSequence(startTime, endTime);
		}
	}

	public override void SetCameras(List<FQID> sources)
	{
		_sources = sources;
		if (sources != null && sources.Count > 0)
		{
			FQID fQID = sources[0];
			if (fQID.ServerId.UserContext != null)
			{
				timeLineUserControl1.Item = fQID.ServerId.UserContext.Configuration.GetItem(fQID);
			}
			else
			{
				timeLineUserControl1.Item = Configuration.Instance.GetItem(fQID);
			}
		}
		else
		{
			timeLineUserControl1.Item = null;
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
					timeLineUserControl1.Item = fQID.ServerId.UserContext.Configuration.GetItem(fQID);
				}
				else
				{
					timeLineUserControl1.Item = Configuration.Instance.GetItem(fQID);
				}
			}
			timeLineUserControl1.RefreshAll();
			base.Enabled = true;
		}
		else
		{
			timeLineUserControl1.Item = null;
			timeLineUserControl1.RefreshAll();
			base.Enabled = false;
		}
	}

	public void RefreshAll()
	{
		timeLineUserControl1.RefreshAll();
	}

	private void timeLineUserControl1_MouseSetTimeEvent(object sender, TimeEventArgs e)
	{
		OnGoToCommand(e.Time);
	}

	private void TimeLineUserControl1_TimeSpanInMinutesChanged(object sender, EventArgs e)
	{
		UpdateTrackBarTimeSpan(TimeSpan);
	}

	private object TimeChangedHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
	{
		DateTime time = (DateTime)message.Data;
		if (IsForMe(sender))
		{
			if (time == DateTime.MaxValue)
			{
				EnvironmentManager.Instance.Log(error: true, "PlaybackUserControlMini.TimeChangedHandler", "The datetime is set as max value");
				return null;
			}
			if (base.InvokeRequired)
			{
				BeginInvoke((MethodInvoker)delegate
				{
					UpdateTime(time);
				});
			}
			else
			{
				UpdateTime(time);
			}
		}
		return null;
	}

	private object PlaybackModeHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
	{
		PlaybackCommandData command = message.Data as PlaybackCommandData;
		if (IsForMe(sender) && command != null)
		{
			if (base.InvokeRequired)
			{
				BeginInvoke((MethodInvoker)delegate
				{
					UpdatePlayUI(command.Command, command.Speed);
				});
			}
			else
			{
				UpdatePlayUI(command.Command, command.Speed);
			}
		}
		return null;
	}

	private void UpdatePlayUI(string playback, double speed)
	{
		switch (playback)
		{
		case "PlayForward":
		case "PlayReverse":
		case "PlayStop":
			_playmode = playback;
			buttonPlayForward.Image = ((playback == "PlayForward") ? VideoOS.Platform.Properties.Resources.icn16x16_light_pause_default : VideoOS.Platform.Properties.Resources.icn16x16_light_play_default);
			buttonPlayBackward.Image = ((playback == "PlayReverse") ? VideoOS.Platform.Properties.Resources.icn16x16_light_pause_default : VideoOS.Platform.Properties.Resources.icn16x16_light_playpausebackward_default);
			buttonPlayBackward2.Image = buttonPlayBackward.Image;
			buttonPlayForward2.Image = buttonPlayForward.Image;
			break;
		}
		if (!(playback == "PlayForward") && !(playback == "PlayReverse"))
		{
			return;
		}
		string text = null;
		if (speed != 0.0)
		{
			text = ((speed == 0.25) ? "1/4x" : ((speed != 0.5) ? (speed.ToString(CultureInfo.InvariantCulture) + "x") : "1/2x"));
			labelSpeed.Text = text;
			if (SpeedToPosition.ContainsKey(speed))
			{
				trackBarSpeed.Value = SpeedToPosition[speed];
			}
			else
			{
				EnvironmentManager.Instance.Log(error: true, "PlaybackUserControlMini.UpdatePlayUI", "The speed: " + speed.ToString(CultureInfo.InvariantCulture) + " is not supported");
			}
		}
	}

	private void UpdateTime(DateTime dateTime)
	{
		timeLineUserControl1.SetShowTime(dateTime.ToLocalTime());
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

	private void OnResize(object sender, EventArgs e)
	{
		if (base.Width > 0)
		{
			pictureBoxCenter.Location = new Point(base.Width / 2 - 1, 1);
			int num = base.Width / 2;
			labelSpeed.Location = new Point(num - 15 - 104 - 66, 34);
			labelTimeSpan.Location = new Point(num + 15 + 104 + 37, 34);
			buttonPlayBackward2.Location = new Point(num - 105 - 3 - 26, 17);
			buttonPlayForward2.Location = new Point(num + 105 + 3, 17);
			buttonFramePrev2.Location = new Point(num - 105 - 6 - 52, 17);
			buttonFrameNext2.Location = new Point(num + 105 + 6 + 26, 17);
		}
	}

	private void OnStartDbClick(object sender, EventArgs e)
	{
		SendPlayCommand("Begin");
	}

	private void OnEndDbClick(object sender, EventArgs e)
	{
		SendPlayCommand("End");
	}

	private void OnPrevSeqClick(object sender, EventArgs e)
	{
		SendPlayCommand("PreviousSequence");
	}

	private void OnNextSeqClick(object sender, EventArgs e)
	{
		SendPlayCommand("NextSequence");
	}

	private void OnPlayBackwardClick(object sender, EventArgs e)
	{
		if (_playmode != "PlayReverse")
		{
			SendPlayCommand("PlayReverse");
		}
		else
		{
			SendPlayCommand("PlayStop");
		}
	}

	private void OnPlayForwardClick(object sender, EventArgs e)
	{
		if (_playmode != "PlayForward")
		{
			SendPlayCommand("PlayForward");
		}
		else
		{
			SendPlayCommand("PlayStop");
		}
	}

	private void OnPrevFrameClick(object sender, EventArgs e)
	{
		SendPlayCommand("Previous");
	}

	private void OnNextFrameClick(object sender, EventArgs e)
	{
		SendPlayCommand("Next");
	}

	private void OnGoToCommand(DateTime dateTime)
	{
		_currentCommand = "Goto";
		EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message("SmartClient.PlaybackCommand", new PlaybackCommandData
		{
			Command = _currentCommand,
			DateTime = dateTime
		}), _playbackControllerFQID);
	}

	private void trackBarSpeed_Scroll(object sender, EventArgs e)
	{
		bool flag = false;
		int value = trackBarSpeed.Value;
		if (_speedLastPosition != value)
		{
			flag = true;
			_speedLastPosition = value;
		}
		_speed = double.Parse(PositionToSpeed[value], CultureInfo.InvariantCulture);
		if (_speed == 0.25)
		{
			labelSpeed.Text = "1/4x";
		}
		else if (_speed == 0.5)
		{
			labelSpeed.Text = "1/2x";
		}
		else
		{
			labelSpeed.Text = PositionToSpeed[value] + "x";
		}
		if (flag)
		{
			if (_currentCommand == "PlayForward" || _currentCommand == "PlayReverse")
			{
				SendPlayCommand(_currentCommand);
			}
			else
			{
				SendPlayCommand("Speed");
			}
		}
	}

	private void UpdateTrackBarTimeSpan(TimeSpan timeSpan)
	{
		if (timeSpan.TotalDays >= 7.0)
		{
			double num = timeSpan.TotalDays / 7.0;
			if (num >= 3.0)
			{
				trackBarTimeSpan.Value = 17;
				num = 4.0;
			}
			else if (num >= 1.5)
			{
				trackBarTimeSpan.Value = 16;
				num = 2.0;
			}
			else
			{
				trackBarTimeSpan.Value = 15;
				num = 1.0;
			}
			if (num == 1.0)
			{
				labelTimeSpan.Text = VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_SingleWeek;
			}
			else
			{
				labelTimeSpan.Text = string.Format(format, VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_MultiWeeks, num);
			}
		}
		else if (timeSpan.TotalDays >= 1.0)
		{
			double totalDays = timeSpan.TotalDays;
			if (totalDays >= 3.5)
			{
				trackBarTimeSpan.Value = 14;
				totalDays = 4.0;
			}
			else if (totalDays >= 1.5)
			{
				trackBarTimeSpan.Value = 13;
				totalDays = 2.0;
			}
			else
			{
				trackBarTimeSpan.Value = 12;
				totalDays = 1.0;
			}
			if (totalDays == 1.0)
			{
				labelTimeSpan.Text = VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_SingleDay;
			}
			else
			{
				labelTimeSpan.Text = string.Format(format, VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_MultiDays, totalDays);
			}
		}
		else if (timeSpan.TotalHours >= 1.0)
		{
			double totalHours = timeSpan.TotalHours;
			if (totalHours >= 18.0)
			{
				trackBarTimeSpan.Value = 11;
				totalHours = 20.0;
			}
			else if (totalHours >= 14.0)
			{
				trackBarTimeSpan.Value = 10;
				totalHours = 16.0;
			}
			else if (totalHours >= 10.0)
			{
				trackBarTimeSpan.Value = 9;
				totalHours = 12.0;
			}
			else if (totalHours >= 6.0)
			{
				trackBarTimeSpan.Value = 8;
				totalHours = 8.0;
			}
			else if (totalHours >= 3.0)
			{
				trackBarTimeSpan.Value = 7;
				totalHours = 4.0;
			}
			else if (totalHours >= 1.5)
			{
				trackBarTimeSpan.Value = 6;
				totalHours = 2.0;
			}
			else
			{
				trackBarTimeSpan.Value = 5;
				totalHours = 1.0;
			}
			if (totalHours == 1.0)
			{
				labelTimeSpan.Text = VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_SingleHour;
			}
			else
			{
				labelTimeSpan.Text = string.Format(format, VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_MultiHours, totalHours);
			}
		}
		else if (timeSpan.TotalMinutes > 0.0)
		{
			double totalMinutes = timeSpan.TotalMinutes;
			if (totalMinutes >= 15.0)
			{
				trackBarTimeSpan.Value = 4;
				totalMinutes = 20.0;
			}
			else if (totalMinutes >= 7.5)
			{
				trackBarTimeSpan.Value = 3;
				totalMinutes = 10.0;
			}
			else if (totalMinutes >= 2.5)
			{
				trackBarTimeSpan.Value = 2;
				totalMinutes = 5.0;
			}
			else if (totalMinutes >= 1.5)
			{
				trackBarTimeSpan.Value = 1;
				totalMinutes = 2.0;
			}
			else
			{
				trackBarTimeSpan.Value = 0;
				totalMinutes = 1.0;
			}
			if (totalMinutes == 1.0)
			{
				labelTimeSpan.Text = VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_SingleMinute;
			}
			else
			{
				labelTimeSpan.Text = string.Format(format, VideoOS.Platform.Resources.Strings.PlaybackUserControlMini_MultiMinutes, totalMinutes);
			}
		}
	}

	private void trackBarTimeSpan_Scroll(object sender, EventArgs e)
	{
		int value = trackBarTimeSpan.Value;
		if (value < PositionToTimeSpanTotalMinutes.Length)
		{
			int num = PositionToTimeSpanTotalMinutes[value];
			TimeSpan = new TimeSpan(0, num, 0);
			labelTimeSpan.Text = TimeSpanTotalMinutesToText[num];
		}
		else
		{
			EnvironmentManager.Instance.Log(error: true, "PlaybackUserControlMini.trackBarTimeSpan_Scroll", "The trackbar value: " + value.ToString(CultureInfo.InvariantCulture) + " is not supported");
		}
		if (timeLineUserControl1.CurrentTime != DateTime.MinValue)
		{
			RefreshAll();
		}
	}

	private void SendPlayCommand(string command)
	{
		_currentCommand = command;
		PlaybackCommandData playbackCommandData = null;
		switch (command)
		{
		case "PlayForward":
		case "PlayReverse":
		case "Speed":
			playbackCommandData = new PlaybackCommandData
			{
				Command = command,
				Speed = _speed
			};
			break;
		default:
			playbackCommandData = new PlaybackCommandData
			{
				Command = command
			};
			break;
		}
		EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message("SmartClient.PlaybackCommand", playbackCommandData), _playbackControllerFQID);
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
		this.pictureBoxCenter = new System.Windows.Forms.PictureBox();
		this.buttonDbEnd = new System.Windows.Forms.Button();
		this.buttonDbStart = new System.Windows.Forms.Button();
		this.buttonNextSeq = new System.Windows.Forms.Button();
		this.buttonPlayForward = new System.Windows.Forms.Button();
		this.buttonPlayBackward = new System.Windows.Forms.Button();
		this.buttonPrevSeq = new System.Windows.Forms.Button();
		this.panelCenter = new System.Windows.Forms.Panel();
		this.trackBarTimeSpan = new System.Windows.Forms.TrackBar();
		this.trackBarSpeed = new System.Windows.Forms.TrackBar();
		this.labelTimeSpan = new System.Windows.Forms.Label();
		this.labelSpeed = new System.Windows.Forms.Label();
		this.buttonFrameNext = new System.Windows.Forms.Button();
		this.buttonFramePrev = new System.Windows.Forms.Button();
		this.timeLineUserControl1 = new VideoOS.Platform.Client.TimeLineUserControl();
		this.buttonFrameNext2 = new System.Windows.Forms.Button();
		this.buttonFramePrev2 = new System.Windows.Forms.Button();
		this.buttonPlayBackward2 = new System.Windows.Forms.Button();
		this.buttonPlayForward2 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxCenter).BeginInit();
		this.panelCenter.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.trackBarTimeSpan).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.trackBarSpeed).BeginInit();
		base.SuspendLayout();
		this.pictureBoxCenter.BackColor = System.Drawing.Color.Gray;
		this.pictureBoxCenter.Location = new System.Drawing.Point(229, 1);
		this.pictureBoxCenter.Name = "pictureBoxCenter";
		this.pictureBoxCenter.Size = new System.Drawing.Size(2, 102);
		this.pictureBoxCenter.TabIndex = 7;
		this.pictureBoxCenter.TabStop = false;
		this.buttonDbEnd.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonDbEnd.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_lastsequence_default;
		this.buttonDbEnd.Location = new System.Drawing.Point(320, 14);
		this.buttonDbEnd.Margin = new System.Windows.Forms.Padding(0);
		this.buttonDbEnd.Name = "buttonDbEnd";
		this.buttonDbEnd.Size = new System.Drawing.Size(26, 23);
		this.buttonDbEnd.TabIndex = 6;
		this.buttonDbEnd.UseVisualStyleBackColor = true;
		this.buttonDbEnd.Click += new System.EventHandler(OnEndDbClick);
		this.buttonDbStart.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonDbStart.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_firstsequence_default;
		this.buttonDbStart.Location = new System.Drawing.Point(114, 14);
		this.buttonDbStart.Margin = new System.Windows.Forms.Padding(0);
		this.buttonDbStart.Name = "buttonDbStart";
		this.buttonDbStart.Size = new System.Drawing.Size(26, 23);
		this.buttonDbStart.TabIndex = 5;
		this.buttonDbStart.TabStop = false;
		this.buttonDbStart.UseVisualStyleBackColor = true;
		this.buttonDbStart.Click += new System.EventHandler(OnStartDbClick);
		this.buttonNextSeq.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonNextSeq.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_nextsequence_default;
		this.buttonNextSeq.Location = new System.Drawing.Point(291, 14);
		this.buttonNextSeq.Margin = new System.Windows.Forms.Padding(0);
		this.buttonNextSeq.Name = "buttonNextSeq";
		this.buttonNextSeq.Size = new System.Drawing.Size(26, 23);
		this.buttonNextSeq.TabIndex = 4;
		this.buttonNextSeq.UseVisualStyleBackColor = true;
		this.buttonNextSeq.Click += new System.EventHandler(OnNextSeqClick);
		this.buttonPlayForward.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonPlayForward.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_play_default;
		this.buttonPlayForward.Location = new System.Drawing.Point(262, 14);
		this.buttonPlayForward.Margin = new System.Windows.Forms.Padding(0);
		this.buttonPlayForward.Name = "buttonPlayForward";
		this.buttonPlayForward.Size = new System.Drawing.Size(26, 23);
		this.buttonPlayForward.TabIndex = 3;
		this.buttonPlayForward.UseVisualStyleBackColor = true;
		this.buttonPlayForward.Click += new System.EventHandler(OnPlayForwardClick);
		this.buttonPlayBackward.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonPlayBackward.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_playpausebackward_default;
		this.buttonPlayBackward.Location = new System.Drawing.Point(172, 14);
		this.buttonPlayBackward.Margin = new System.Windows.Forms.Padding(0);
		this.buttonPlayBackward.Name = "buttonPlayBackward";
		this.buttonPlayBackward.Size = new System.Drawing.Size(26, 23);
		this.buttonPlayBackward.TabIndex = 1;
		this.buttonPlayBackward.UseVisualStyleBackColor = true;
		this.buttonPlayBackward.Click += new System.EventHandler(OnPlayBackwardClick);
		this.buttonPrevSeq.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonPrevSeq.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_previoussequence_default;
		this.buttonPrevSeq.Location = new System.Drawing.Point(143, 14);
		this.buttonPrevSeq.Margin = new System.Windows.Forms.Padding(0);
		this.buttonPrevSeq.Name = "buttonPrevSeq";
		this.buttonPrevSeq.Size = new System.Drawing.Size(26, 23);
		this.buttonPrevSeq.TabIndex = 0;
		this.buttonPrevSeq.UseVisualStyleBackColor = true;
		this.buttonPrevSeq.Click += new System.EventHandler(OnPrevSeqClick);
		this.panelCenter.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelCenter.Controls.Add(this.trackBarTimeSpan);
		this.panelCenter.Controls.Add(this.trackBarSpeed);
		this.panelCenter.Controls.Add(this.labelTimeSpan);
		this.panelCenter.Controls.Add(this.labelSpeed);
		this.panelCenter.Controls.Add(this.buttonFrameNext);
		this.panelCenter.Controls.Add(this.buttonFramePrev);
		this.panelCenter.Controls.Add(this.buttonPrevSeq);
		this.panelCenter.Controls.Add(this.buttonPlayBackward);
		this.panelCenter.Controls.Add(this.buttonPlayForward);
		this.panelCenter.Controls.Add(this.buttonDbEnd);
		this.panelCenter.Controls.Add(this.buttonNextSeq);
		this.panelCenter.Controls.Add(this.buttonDbStart);
		this.panelCenter.Location = new System.Drawing.Point(0, 50);
		this.panelCenter.Name = "panelCenter";
		this.panelCenter.Size = new System.Drawing.Size(460, 50);
		this.panelCenter.TabIndex = 10;
		this.panelCenter.Visible = false;
		this.trackBarTimeSpan.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.trackBarTimeSpan.AutoSize = false;
		this.trackBarTimeSpan.LargeChange = 1;
		this.trackBarTimeSpan.Location = new System.Drawing.Point(349, 14);
		this.trackBarTimeSpan.Maximum = 17;
		this.trackBarTimeSpan.Name = "trackBarTimeSpan";
		this.trackBarTimeSpan.Size = new System.Drawing.Size(108, 20);
		this.trackBarTimeSpan.TabIndex = 19;
		this.trackBarTimeSpan.Value = 5;
		this.trackBarTimeSpan.Scroll += new System.EventHandler(trackBarTimeSpan_Scroll);
		this.trackBarSpeed.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.trackBarSpeed.AutoSize = false;
		this.trackBarSpeed.LargeChange = 1;
		this.trackBarSpeed.Location = new System.Drawing.Point(3, 14);
		this.trackBarSpeed.Maximum = 8;
		this.trackBarSpeed.Name = "trackBarSpeed";
		this.trackBarSpeed.Size = new System.Drawing.Size(108, 20);
		this.trackBarSpeed.TabIndex = 18;
		this.trackBarSpeed.Value = 3;
		this.trackBarSpeed.Scroll += new System.EventHandler(trackBarSpeed_Scroll);
		this.labelTimeSpan.AutoSize = true;
		this.labelTimeSpan.Location = new System.Drawing.Point(386, 34);
		this.labelTimeSpan.Name = "labelTimeSpan";
		this.labelTimeSpan.Size = new System.Drawing.Size(37, 13);
		this.labelTimeSpan.TabIndex = 15;
		this.labelTimeSpan.Text = "1 hour";
		this.labelSpeed.Location = new System.Drawing.Point(37, 34);
		this.labelSpeed.Name = "labelSpeed";
		this.labelSpeed.Size = new System.Drawing.Size(37, 13);
		this.labelSpeed.TabIndex = 13;
		this.labelSpeed.Text = "1x";
		this.buttonFrameNext.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonFrameNext.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_stepforward_default;
		this.buttonFrameNext.Location = new System.Drawing.Point(233, 14);
		this.buttonFrameNext.Margin = new System.Windows.Forms.Padding(0);
		this.buttonFrameNext.Name = "buttonFrameNext";
		this.buttonFrameNext.Size = new System.Drawing.Size(26, 23);
		this.buttonFrameNext.TabIndex = 11;
		this.buttonFrameNext.UseVisualStyleBackColor = true;
		this.buttonFrameNext.Click += new System.EventHandler(OnNextFrameClick);
		this.buttonFramePrev.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.buttonFramePrev.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_stepbackward_default;
		this.buttonFramePrev.Location = new System.Drawing.Point(201, 14);
		this.buttonFramePrev.Margin = new System.Windows.Forms.Padding(0);
		this.buttonFramePrev.Name = "buttonFramePrev";
		this.buttonFramePrev.Size = new System.Drawing.Size(26, 23);
		this.buttonFramePrev.TabIndex = 10;
		this.buttonFramePrev.UseVisualStyleBackColor = true;
		this.buttonFramePrev.Click += new System.EventHandler(OnPrevFrameClick);
		this.timeLineUserControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.timeLineUserControl1.Item = null;
		this.timeLineUserControl1.Location = new System.Drawing.Point(0, 0);
		this.timeLineUserControl1.Margin = new System.Windows.Forms.Padding(0);
		this.timeLineUserControl1.Name = "timeLineUserControl1";
		this.timeLineUserControl1.Size = new System.Drawing.Size(460, 50);
		this.timeLineUserControl1.TabIndex = 11;
		this.timeLineUserControl1.TimeSpan = System.TimeSpan.Parse("01:00:00");
		this.buttonFrameNext2.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_stepforward_default;
		this.buttonFrameNext2.Location = new System.Drawing.Point(366, 17);
		this.buttonFrameNext2.Name = "buttonFrameNext2";
		this.buttonFrameNext2.Size = new System.Drawing.Size(26, 23);
		this.buttonFrameNext2.TabIndex = 22;
		this.buttonFrameNext2.UseVisualStyleBackColor = true;
		this.buttonFrameNext2.Visible = false;
		this.buttonFrameNext2.Click += new System.EventHandler(OnNextFrameClick);
		this.buttonFramePrev2.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_stepbackward_default;
		this.buttonFramePrev2.Location = new System.Drawing.Point(94, 17);
		this.buttonFramePrev2.Name = "buttonFramePrev2";
		this.buttonFramePrev2.Size = new System.Drawing.Size(26, 23);
		this.buttonFramePrev2.TabIndex = 21;
		this.buttonFramePrev2.UseVisualStyleBackColor = true;
		this.buttonFramePrev2.Visible = false;
		this.buttonFramePrev2.Click += new System.EventHandler(OnPrevFrameClick);
		this.buttonPlayBackward2.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_playpausebackward_default;
		this.buttonPlayBackward2.Location = new System.Drawing.Point(123, 17);
		this.buttonPlayBackward2.Name = "buttonPlayBackward2";
		this.buttonPlayBackward2.Size = new System.Drawing.Size(26, 23);
		this.buttonPlayBackward2.TabIndex = 19;
		this.buttonPlayBackward2.UseVisualStyleBackColor = true;
		this.buttonPlayBackward2.Visible = false;
		this.buttonPlayBackward2.Click += new System.EventHandler(OnPlayBackwardClick);
		this.buttonPlayForward2.Image = VideoOS.Platform.Properties.Resources.icn16x16_light_play_default;
		this.buttonPlayForward2.Location = new System.Drawing.Point(337, 17);
		this.buttonPlayForward2.Name = "buttonPlayForward2";
		this.buttonPlayForward2.Size = new System.Drawing.Size(26, 23);
		this.buttonPlayForward2.TabIndex = 20;
		this.buttonPlayForward2.UseVisualStyleBackColor = true;
		this.buttonPlayForward2.Visible = false;
		this.buttonPlayForward2.Click += new System.EventHandler(OnPlayForwardClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.timeLineUserControl1);
		base.Controls.Add(this.buttonFrameNext2);
		base.Controls.Add(this.buttonFramePrev2);
		base.Controls.Add(this.buttonPlayBackward2);
		base.Controls.Add(this.buttonPlayForward2);
		base.Controls.Add(this.panelCenter);
		base.Controls.Add(this.pictureBoxCenter);
		this.DoubleBuffered = true;
		base.Name = "PlaybackUserControlMini";
		this.SelectionFromTime = new System.DateTime(2017, 4, 12, 9, 11, 0, 517);
		this.SelectionToTime = new System.DateTime(2017, 4, 12, 9, 11, 0, 517);
		base.Size = new System.Drawing.Size(460, 100);
		base.Resize += new System.EventHandler(OnResize);
		((System.ComponentModel.ISupportInitialize)this.pictureBoxCenter).EndInit();
		this.panelCenter.ResumeLayout(false);
		this.panelCenter.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.trackBarTimeSpan).EndInit();
		((System.ComponentModel.ISupportInitialize)this.trackBarSpeed).EndInit();
		base.ResumeLayout(false);
	}
}

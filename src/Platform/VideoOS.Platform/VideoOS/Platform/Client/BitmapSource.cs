using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Live;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class BitmapSource
{
	private enum MyPlayCommand
	{
		None,
		Start,
		End,
		NextSequence,
		PrevSequence,
		NextFrame,
		PrevFrame
	}

	public delegate void NewBitmapEventHandler(Bitmap bitmap);

	private Item _selectedItem;

	private Guid _streamId;

	private BitmapVideoSource _bitmapVideoSource;

	private BitmapVideoSource _bitmapVideoSourceNext;

	private readonly object _bitmapVideoSourceNextLock = new object();

	private BitmapLiveSource _bitmapLiveSource;

	private string _mode = "Stop";

	private DateTime _currentShownTime = DateTime.MinValue;

	private bool _stopPlayback;

	private bool _stopLive;

	private object _objRef1;

	private object _objRef2;

	private readonly Bitmap _blackImage = new Bitmap(1, 1);

	private readonly FQID _fqid = new FQID(new ServerId("UI", "local", 0, Guid.Empty), Guid.Empty, default(Guid), FolderType.No, Kind.ViewItemInstance);

	private bool _selected = true;

	private int _newWidth;

	private int _newHeight;

	private bool _setResolution;

	private Size _size = new Size(0, 0);

	private MyPlayCommand _nextCommand;

	private bool _initialized;

	private bool _stop;

	private Thread _fetchThread;

	private DateTime _nextToFetchTime;

	private PlaybackTimeInformationData _currentTimeInformation;

	private bool _requestInProgress;

	public bool IsInitialized => _initialized;

	public FQID InstanceFQID => _fqid;

	public Item Item
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			_selectedItem = value;
		}
	}

	public Guid StreamId
	{
		get
		{
			return _streamId;
		}
		set
		{
			if (_streamId != value)
			{
				_streamId = value;
				if (_bitmapLiveSource != null)
				{
					_bitmapLiveSource.StreamId = value;
				}
			}
		}
	}

	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
		}
	}

	public Size Size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
			if (_bitmapVideoSource != null)
			{
				_newWidth = _size.Width;
				_newHeight = _size.Height;
				_setResolution = true;
			}
			if (_bitmapLiveSource != null)
			{
				_bitmapLiveSource.Width = _size.Width;
				_bitmapLiveSource.Height = _size.Height;
				_bitmapLiveSource.SetWidthHeight();
			}
		}
	}

	public FQID PlaybackFQID { get; set; }

	public event NewBitmapEventHandler NewBitmapEvent;

	public BitmapSource()
	{
		Graphics graphics = Graphics.FromImage(_blackImage);
		graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
		graphics.Dispose();
	}

	public void Init()
	{
		if (_initialized)
		{
			throw new MIPException("Already initialized");
		}
		SetUpApplicationEventListeners();
		_stop = false;
		_stopLive = false;
		_stopPlayback = false;
		_bitmapVideoSourceNext = null;
		_fetchThread = new Thread(BitmapFetchThread);
		_fetchThread.IsBackground = true;
		_fetchThread.Name = "BitmapSource.BitmapFetchThread";
		_fetchThread.Start();
		_initialized = true;
		ModeHandler(Mode.ClientPlayback);
		new TrackPageViewData(typeof(BitmapSource)).Send();
	}

	public void Close(bool wait = false)
	{
		_stop = true;
		_stopLive = true;
		_stopPlayback = true;
		if (!_initialized)
		{
			return;
		}
		CloseLiveSession();
		ClosePlaybackSession();
		RemoveApplicationEventListeners();
		if (wait)
		{
			for (int num = 100; num > 0; num--)
			{
				if (_fetchThread == null)
				{
					return;
				}
				Thread.Sleep(50);
			}
		}
		_initialized = false;
	}

	public void LiveStart()
	{
		ModeHandler(Mode.ClientLive);
	}

	public void LiveStop()
	{
		ModeHandler(Mode.ClientPlayback);
	}

	private void SetUpApplicationEventListeners()
	{
		_objRef1 = EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeChangedHandler, new PlaybackControllerFilter("SmartClient.PlaybackCurrentTimeIndication", PlaybackFQID));
		_objRef2 = EnvironmentManager.Instance.RegisterReceiver(PlaybackCommandHandler, new PlaybackControllerFilter("SmartClient.PlaybackIndication", PlaybackFQID));
	}

	private void RemoveApplicationEventListeners()
	{
		if (_objRef1 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef1);
		}
		_objRef1 = null;
		if (_objRef2 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef2);
		}
		_objRef2 = null;
	}

	private object PlaybackTimeChangedHandler(Message message, FQID dest, FQID sender)
	{
		DateTime nextToFetchTime = (DateTime)message.Data;
		_nextToFetchTime = nextToFetchTime;
		return null;
	}

	private object PlaybackCommandHandler(Message message, FQID dest, FQID sender)
	{
		if (message.Data is PlaybackCommandData { Command: var command })
		{
			switch (command)
			{
			case "Begin":
				_nextCommand = MyPlayCommand.Start;
				break;
			case "End":
				_nextCommand = MyPlayCommand.End;
				break;
			case "Previous":
				_nextCommand = MyPlayCommand.PrevFrame;
				break;
			case "Next":
				_nextCommand = MyPlayCommand.NextFrame;
				break;
			case "PreviousSequence":
				_nextCommand = MyPlayCommand.PrevSequence;
				break;
			case "NextSequence":
				_nextCommand = MyPlayCommand.NextSequence;
				break;
			case "Play":
			case "PlayForward":
				_mode = "Forward";
				break;
			case "PlayReverse":
				_mode = "Reverse";
				break;
			case "PlayStop":
				_mode = "Stop";
				break;
			}
		}
		new TrackEventData("SmartClient.PlaybackCommand", deprecated: false, (message.Data is PlaybackCommandData) ? ((PlaybackCommandData)message.Data).Command : null).Send();
		return null;
	}

	private void ModeHandler(Mode newMode)
	{
		if (_bitmapVideoSource != null)
		{
			ClosePlaybackSession();
		}
		if (_bitmapLiveSource != null)
		{
			CloseLiveSession();
		}
		_mode = "Stop";
		switch (newMode)
		{
		case Mode.ClientLive:
			OpenLiveSession();
			break;
		case Mode.ClientPlayback:
			_mode = "Forward";
			OpenPlaybackSession();
			break;
		case Mode.ClientSetup:
			break;
		}
	}

	private void OpenPlaybackSession()
	{
		if (Item == null)
		{
			return;
		}
		if (Item.FQID.ServerId.UserContext != null)
		{
			_selectedItem = Item.FQID.ServerId.UserContext.Configuration.GetItem(Item.FQID);
		}
		else
		{
			_selectedItem = Configuration.Instance.GetItem(Item.FQID);
		}
		if (_selectedItem != null)
		{
			lock (_bitmapVideoSourceNextLock)
			{
				_bitmapVideoSourceNext = new BitmapVideoSource(_selectedItem);
				_bitmapVideoSourceNext.Width = _size.Width;
				_bitmapVideoSourceNext.Height = _size.Height;
				_bitmapVideoSourceNext.SetKeepAspectRatio(keepAspectRatio: true, fillSpace: true);
			}
			_nextToFetchTime = DateTime.UtcNow;
			_stopPlayback = false;
		}
	}

	private void ClosePlaybackSession()
	{
		_stopPlayback = true;
		FireNewBitmapEvent(new Bitmap(_blackImage, new Size(1, 1)));
	}

	private void OpenLiveSession()
	{
		if (Item != null)
		{
			if (Item.FQID.ServerId.UserContext != null)
			{
				_selectedItem = Item.FQID.ServerId.UserContext.Configuration.GetItem(Item.FQID);
			}
			else
			{
				_selectedItem = Configuration.Instance.GetItem(Item.FQID);
			}
			if (_selectedItem != null)
			{
				_bitmapLiveSource = new BitmapLiveSource(_selectedItem, BitmapFormat.BGR24);
				_bitmapLiveSource.LiveContentEvent += BitmapLiveSourceLiveContentEvent;
				_bitmapLiveSource.LiveStatusEvent += BitmapLiveSourceLiveStatusEvent;
				_bitmapLiveSource.Width = _size.Width;
				_bitmapLiveSource.Height = _size.Height;
				_bitmapLiveSource.SetKeepAspectRatio(keepAspectRatio: true, fillSpace: true);
				_bitmapLiveSource.StreamId = _streamId;
				_bitmapLiveSource.Init();
				_bitmapLiveSource.LiveModeStart = true;
				_bitmapLiveSource.SingleFrameQueue = true;
				_stopLive = false;
			}
		}
	}

	private void CloseLiveSession()
	{
		_stopLive = true;
		if (_bitmapLiveSource != null)
		{
			_bitmapLiveSource.LiveContentEvent -= BitmapLiveSourceLiveContentEvent;
			_bitmapLiveSource.LiveStatusEvent -= BitmapLiveSourceLiveStatusEvent;
			_bitmapLiveSource.LiveModeStart = false;
			_bitmapLiveSource.Close();
			_bitmapLiveSource = null;
			FireNewBitmapEvent(new Bitmap(_blackImage, new Size(1, 1)));
		}
	}

	private void BitmapFetchThread()
	{
		bool errorRecovery = false;
		DateTime utcNow = DateTime.UtcNow;
		try
		{
			while (!_stop && EnvironmentManager.Instance.ApplicationLoggedOn)
			{
				try
				{
					if (_stopPlayback)
					{
						StopPlayback();
					}
					if (_bitmapVideoSourceNext == null)
					{
						goto IL_004f;
					}
					SwitchToNextBitmapVideoSource();
					if (_stop)
					{
						continue;
					}
					_bitmapVideoSource.Init();
					ShowError("");
					errorRecovery = false;
					goto IL_004f;
					IL_004f:
					if (_bitmapVideoSource != null && _setResolution)
					{
						SetResolution();
					}
					goto IL_00b2;
				}
				catch (Exception ex)
				{
					if (ex is CommunicationMIPException)
					{
						ShowError("Connection lost to server ...");
					}
					else
					{
						ShowError(ex.Message);
					}
					_bitmapVideoSourceNext = _bitmapVideoSource;
					_bitmapVideoSource = null;
					_nextCommand = MyPlayCommand.None;
					_nextToFetchTime = DateTime.MinValue;
					errorRecovery = true;
					goto IL_00b2;
				}
				IL_00b2:
				if (errorRecovery && !_stop)
				{
					Thread.Sleep(3000);
					continue;
				}
				if (Selected && _nextCommand != MyPlayCommand.None && _bitmapVideoSource != null && !_requestInProgress)
				{
					HandleNextCommand(ref errorRecovery);
					if (errorRecovery)
					{
						continue;
					}
				}
				if (_nextToFetchTime != DateTime.MinValue && _nextToFetchTime != _currentShownTime && _bitmapVideoSource != null && !_requestInProgress)
				{
					DateTime nextToFetchTime = _nextToFetchTime;
					DateTime dateTime = ((nextToFetchTime.Kind == DateTimeKind.Local) ? nextToFetchTime.ToUniversalTime() : nextToFetchTime);
					bool flag = WillResultInSameFrame(dateTime);
					_nextToFetchTime = DateTime.MinValue;
					bool flag2 = false;
					if (ShouldUpdateTime(utcNow, dateTime, flag))
					{
						flag2 = true;
						flag = false;
					}
					if (!flag)
					{
						try
						{
							if (_mode == "Stop")
							{
								_bitmapVideoSource.GoTo(dateTime, "Reverse");
							}
							VideoOS.Platform.Data.BitmapData bitmapData = _bitmapVideoSource.GetAtOrBefore(dateTime) as VideoOS.Platform.Data.BitmapData;
							UpdateBitmapDataTimeStamp(dateTime, bitmapData);
							if (flag2)
							{
								utcNow = DateTime.UtcNow;
								_currentTimeInformation = CreatePlaybackTimeInformation(bitmapData);
								if (_currentTimeInformation != null && _currentTimeInformation.PreviousTime < dateTime)
								{
									bitmapData.Dispose();
									bitmapData = null;
								}
							}
							if (bitmapData != null)
							{
								utcNow = DateTime.UtcNow;
								ShowBitmap(bitmapData);
								bitmapData.Dispose();
							}
						}
						catch (Exception ex2)
						{
							if (ex2 is CommunicationMIPException)
							{
								ShowError("Connection lost to server ...");
							}
							else
							{
								ShowError(ex2.Message);
							}
							errorRecovery = true;
							_bitmapVideoSourceNext = _bitmapVideoSource;
							_bitmapVideoSource = null;
							_nextCommand = MyPlayCommand.None;
						}
					}
				}
				Thread.Sleep(10);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("RGBVideoEnhancement.PlaybackThread", exception);
		}
		if (_bitmapVideoSource != null)
		{
			_bitmapVideoSource.Close();
			_bitmapVideoSource = null;
		}
		_fetchThread = null;
	}

	private bool WillResultInSameFrame(DateTime utcTime)
	{
		if (_currentTimeInformation != null && _currentTimeInformation.NextTime > utcTime)
		{
			return _currentTimeInformation.PreviousTime < utcTime;
		}
		return false;
	}

	private void HandleNextCommand(ref bool errorRecovery)
	{
		try
		{
			VideoOS.Platform.Data.BitmapData bitmapDataByNextCommand = GetBitmapDataByNextCommand();
			if (bitmapDataByNextCommand != null)
			{
				ShowBitmap(bitmapDataByNextCommand);
				EnvironmentManager.Instance.PostMessage(new Message("SmartClient.PlaybackCommand", new PlaybackCommandData
				{
					Command = "Goto",
					DateTime = bitmapDataByNextCommand.DateTime
				}), PlaybackFQID, InstanceFQID);
				bitmapDataByNextCommand.Dispose();
			}
		}
		catch (Exception ex)
		{
			if (ex is CommunicationMIPException)
			{
				ShowError("Connection lost to server ...");
			}
			else
			{
				ShowError(ex.Message);
			}
			errorRecovery = true;
			_bitmapVideoSourceNext = _bitmapVideoSource;
			_bitmapVideoSource = null;
			_nextCommand = MyPlayCommand.None;
			Thread.Sleep(3000);
		}
		_nextCommand = MyPlayCommand.None;
	}

	private VideoOS.Platform.Data.BitmapData GetBitmapDataByNextCommand()
	{
		VideoOS.Platform.Data.BitmapData result = null;
		switch (_nextCommand)
		{
		case MyPlayCommand.Start:
			result = _bitmapVideoSource.GetBegin();
			break;
		case MyPlayCommand.End:
			result = _bitmapVideoSource.GetEnd();
			break;
		case MyPlayCommand.PrevSequence:
			result = _bitmapVideoSource.GetPreviousSequence();
			break;
		case MyPlayCommand.NextSequence:
			result = _bitmapVideoSource.GetNextSequence();
			break;
		case MyPlayCommand.PrevFrame:
			result = _bitmapVideoSource.GetPrevious();
			break;
		case MyPlayCommand.NextFrame:
			result = _bitmapVideoSource.GetNext() as VideoOS.Platform.Data.BitmapData;
			break;
		}
		return result;
	}

	private void SetResolution()
	{
		_bitmapVideoSource.Width = _newWidth;
		_bitmapVideoSource.Height = _newHeight;
		_bitmapVideoSource.SetWidthHeight();
		_setResolution = false;
	}

	private void SwitchToNextBitmapVideoSource()
	{
		lock (_bitmapVideoSourceNextLock)
		{
			if (_bitmapVideoSource != null)
			{
				_bitmapVideoSource.Close();
			}
			_bitmapVideoSource = _bitmapVideoSourceNext;
			_bitmapVideoSourceNext = null;
		}
	}

	private bool ShouldUpdateTime(DateTime lastCheckForTimeUpdate, DateTime utcTime, bool willResultInSameFrame)
	{
		if (_currentTimeInformation != null && _currentTimeInformation.PreviousTime < utcTime && _mode == "Reverse")
		{
			if (!(DateTime.UtcNow - lastCheckForTimeUpdate > TimeSpan.FromSeconds(1.0)))
			{
				return !willResultInSameFrame;
			}
			return true;
		}
		return false;
	}

	private static void UpdateBitmapDataTimeStamp(DateTime utcTime, VideoOS.Platform.Data.BitmapData bitmapData)
	{
		if (bitmapData != null && !bitmapData.IsPreviousAvailable)
		{
			if (utcTime - TimeSpan.FromMilliseconds(10.0) < bitmapData.DateTime)
			{
				bitmapData.PreviousDateTime = utcTime - TimeSpan.FromMilliseconds(10.0);
			}
			else
			{
				bitmapData.PreviousDateTime = bitmapData.DateTime;
			}
		}
	}

	private void StopPlayback()
	{
		if (_bitmapVideoSource != null)
		{
			_bitmapVideoSource.Close();
		}
		_bitmapVideoSource = null;
		_nextCommand = MyPlayCommand.None;
		_nextToFetchTime = DateTime.MinValue;
	}

	private void ShowError(string errorText)
	{
		Bitmap bitmap = ((_size.Width != 0 && _size.Height != 0) ? new Bitmap(_size.Width, _size.Height) : new Bitmap(640, 420));
		Graphics graphics = Graphics.FromImage(bitmap);
		graphics.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
		graphics.DrawString(errorText, new Font(FontFamily.GenericMonospace, 12f), Brushes.White, new PointF(20f, 100f));
		graphics.Dispose();
		FireNewBitmapEvent(bitmap);
	}

	private void FireNewBitmapEvent(Bitmap bitmap)
	{
		if (this.NewBitmapEvent != null)
		{
			this.NewBitmapEvent(bitmap);
		}
	}

	private void ShowBitmap(VideoOS.Platform.Data.BitmapData bitmapData)
	{
		if (_currentTimeInformation != null && _currentTimeInformation.PreviousTime < bitmapData.DateTime && _currentTimeInformation.NextTime > bitmapData.DateTime)
		{
			if (Selected)
			{
				EnvironmentManager.Instance.SendMessage(new Message("SmartClient.PlaybackDeviceInformation", _currentTimeInformation), PlaybackFQID, InstanceFQID);
			}
			return;
		}
		_nextToFetchTime = bitmapData.DateTime;
		_currentTimeInformation = CreatePlaybackTimeInformation(bitmapData);
		if (Selected)
		{
			EnvironmentManager.Instance.SendMessage(new Message("SmartClient.PlaybackDeviceInformation", _currentTimeInformation), PlaybackFQID, InstanceFQID);
		}
		_requestInProgress = true;
		ShowBitmap2(bitmapData);
	}

	private PlaybackTimeInformationData CreatePlaybackTimeInformation(VideoOS.Platform.Data.BitmapData bitmapData)
	{
		return new PlaybackTimeInformationData
		{
			Item = _selectedItem.FQID,
			CurrentTime = bitmapData.DateTime,
			NextTime = bitmapData.NextDateTime,
			PreviousTime = bitmapData.PreviousDateTime
		};
	}

	private void ShowBitmap2(VideoOS.Platform.Data.BitmapData bitmapData)
	{
		if (bitmapData.DateTime != _currentShownTime)
		{
			int planeWidth = bitmapData.GetPlaneWidth(0);
			int planeHeight = bitmapData.GetPlaneHeight(0);
			int planeStride = bitmapData.GetPlaneStride(0);
			IntPtr planePointer = bitmapData.GetPlanePointer(0);
			Bitmap bitmap = new Bitmap(planeWidth, planeHeight, planeStride, PixelFormat.Format24bppRgb, planePointer);
			FireNewBitmapEvent(bitmap);
		}
		_currentShownTime = bitmapData.DateTime;
		_requestInProgress = false;
	}

	private void BitmapLiveSourceLiveContentEvent(object sender, EventArgs e)
	{
		try
		{
			if (!(e is LiveContentEventArgs e2))
			{
				return;
			}
			if (e2.LiveContent != null)
			{
				if (e2.LiveContent is LiveSourceBitmapContent liveSourceBitmapContent)
				{
					if (_stopLive)
					{
						liveSourceBitmapContent.Dispose();
						return;
					}
					int planeWidth = liveSourceBitmapContent.GetPlaneWidth(0);
					int planeHeight = liveSourceBitmapContent.GetPlaneHeight(0);
					int planeStride = liveSourceBitmapContent.GetPlaneStride(0);
					IntPtr planePointer = liveSourceBitmapContent.GetPlanePointer(0);
					Bitmap bitmap = new Bitmap(planeWidth, planeHeight, planeStride, PixelFormat.Format24bppRgb, planePointer);
					FireNewBitmapEvent(bitmap);
					liveSourceBitmapContent.Dispose();
				}
			}
			else if (e2.Exception != null)
			{
				Bitmap bitmap2 = new Bitmap(320, 240);
				Graphics graphics = Graphics.FromImage(bitmap2);
				graphics.FillRectangle(Brushes.Black, 0, 0, bitmap2.Width, bitmap2.Height);
				if (e2.Exception is CommunicationMIPException)
				{
					graphics.DrawString("Connection lost to server ...", new Font(FontFamily.GenericMonospace, 12f), Brushes.White, new PointF(20f, 100f));
				}
				else
				{
					graphics.DrawString(e2.Exception.Message, new Font(FontFamily.GenericMonospace, 12f), Brushes.White, new PointF(20f, 100f));
				}
				graphics.Dispose();
				FireNewBitmapEvent(bitmap2);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("BitmapLiveSourceLiveContentEvent", exception);
		}
	}

	private void BitmapLiveSourceLiveStatusEvent(object sender, EventArgs e)
	{
		if (e is LiveStatusEventArgs e2 && e2.CurrentStatusFlags.HasFlag(StatusFlags.CameraConnectionLost))
		{
			Bitmap bitmap = new Bitmap(320, 240);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
			graphics.DrawString("Connection lost to camera ...", new Font(FontFamily.GenericMonospace, 12f), Brushes.White, new PointF(20f, 100f));
			graphics.Dispose();
			FireNewBitmapEvent(bitmap);
		}
	}
}

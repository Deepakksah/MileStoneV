using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Data.Speaker;
using VideoOS.Platform.Live;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class RawSource
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

	public delegate void RawVideoSampleEventHandler(DateTime timeStamp, byte[] content, string privacyMask);

	public delegate void RawVideoErrorHandler(string error);

	public delegate void RawLiveStatusEventHandler(object sender, EventArgs eventArgs);

	private Item _selectedItem;

	private Guid _streamId;

	private RawVideoSource _rawVideoSource;

	private RawLiveSource _rawLiveSource;

	private readonly object _rawVideoSourceLock = new object();

	private readonly object _rawLiveSourceLock = new object();

	private readonly object _fetchTimeLock = new object();

	private Queue<RawVideoSourceData> _groupOfPictures = new Queue<RawVideoSourceData>();

	private string _mode = "Stop";

	private DateTime _currentShownTime = DateTime.MinValue;

	private bool _stopPlayback;

	private object _playbackTimeHandler;

	private object _playbackCommandHandler;

	private bool _selected = true;

	private MyPlayCommand _nextCommand;

	private bool _initialized;

	private PlaybackController _playbackController;

	private readonly FQID _fqid = new FQID(new ServerId("UI", "local", 0, Guid.Empty), Guid.Empty, default(Guid), FolderType.No, Kind.ViewItemInstance);

	private bool _wasStopped = true;

	public RawLiveStatusEventHandler LiveStatusEvent;

	private bool _stop;

	private Thread _fetchThread;

	private DateTime _nextToFetchTime;

	private PlaybackTimeInformationData _currentTimeInformation;

	private bool _requestInProgress;

	public bool IsInitialized => _initialized;

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

	public FQID PlaybackFQID
	{
		get
		{
			if (_playbackController != null)
			{
				return _playbackController.FQID;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				_playbackController = null;
			}
			else
			{
				_playbackController = ClientControl.Instance.GetPlaybackController(value);
			}
		}
	}

	public event RawVideoSampleEventHandler NewRawVideoSampleEvent;

	public event RawVideoSampleEventHandler NewRawVideoHeaderSampleEvent;

	public event RawVideoErrorHandler ErrorOccuredEvent;

	public RawSource()
	{
		new TrackPageViewData(typeof(RawSource)).Send();
	}

	public void Init()
	{
		if (_initialized)
		{
			throw new MIPException("Already initialized");
		}
		_stop = false;
		_stopPlayback = false;
		_fetchThread = new Thread(VideoSourceFetchThread);
		_fetchThread.IsBackground = true;
		_fetchThread.Name = "RawSource.VideoSourceFetchThread";
		_fetchThread.Start();
		_initialized = true;
	}

	public void Close(bool wait = false)
	{
		_stop = true;
		_stopPlayback = true;
		if (!_initialized)
		{
			return;
		}
		CloseLiveSession();
		ClosePlaybackSession();
		if (_playbackController != null)
		{
			_playbackController.Close();
		}
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

	private void FireNewRawVideoSampleEvent(DateTime timeStamp, byte[] content, string privacyMask)
	{
		if (this.NewRawVideoSampleEvent != null)
		{
			this.NewRawVideoSampleEvent(timeStamp, content, privacyMask);
		}
	}

	private void FireNewRawVideoHeaderSampleEvent(DateTime timeStamp, byte[] content, string privacyMask)
	{
		if (this.NewRawVideoHeaderSampleEvent != null)
		{
			this.NewRawVideoHeaderSampleEvent(timeStamp, content, privacyMask);
		}
	}

	private void FireNewErrorMessage(string error)
	{
		if (this.ErrorOccuredEvent != null)
		{
			this.ErrorOccuredEvent(error);
		}
	}

	public void PlaybackStart()
	{
		ModeHandler(Mode.ClientPlayback);
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
		_playbackTimeHandler = EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeChangedHandler, new PlaybackControllerFilter("SmartClient.PlaybackCurrentTimeIndication", PlaybackFQID));
		_playbackCommandHandler = EnvironmentManager.Instance.RegisterReceiver(PlaybackCommandHandler, new PlaybackControllerFilter("SmartClient.PlaybackIndication", PlaybackFQID));
	}

	private void RemoveApplicationEventListeners()
	{
		if (_playbackTimeHandler != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_playbackTimeHandler);
		}
		_playbackTimeHandler = null;
		if (_playbackCommandHandler != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_playbackCommandHandler);
		}
		_playbackCommandHandler = null;
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
				_rawLiveSource = new RawLiveSource(_selectedItem);
				_rawLiveSource.LiveContentEvent += RawLiveSourceContentEvent;
				_rawLiveSource.LiveStatusEvent += RawLiveStatusEvent;
				_rawLiveSource.StreamId = _streamId;
				_rawLiveSource.Init();
				_rawLiveSource.LiveModeStart = true;
			}
		}
	}

	private void CloseLiveSession()
	{
		if (_rawLiveSource != null)
		{
			_rawLiveSource.LiveContentEvent -= RawLiveSourceContentEvent;
			_rawLiveSource.LiveStatusEvent -= RawLiveStatusEvent;
			_rawLiveSource.LiveModeStart = false;
			_rawLiveSource.Close();
			_rawLiveSource = null;
		}
	}

	private void RawLiveStatusEvent(object sender, EventArgs e)
	{
		LiveStatusEventArgs e2 = e as LiveStatusEventArgs;
		if (e2.CurrentStatusFlags.HasFlag(StatusFlags.CameraConnectionLost) || e2.CurrentStatusFlags.HasFlag(StatusFlags.ClientLiveStopped))
		{
			FireNewErrorMessage("Camera connection lost");
			CloseLiveSession();
		}
	}

	private void RawLiveSourceContentEvent(object sender, EventArgs e)
	{
		try
		{
			if (e is LiveContentRawEventArgs { LiveContent: not null } e2)
			{
				byte[] content = e2.LiveContent.Content;
				if (DoesDataContainVideo(content))
				{
					byte[] array = new byte[32];
					Array.Copy(content, array, 32);
					content = RemovePrefix(content, out var _, out var timestamp);
					FireNewRawVideoSampleEvent(timestamp, content, e2.LiveContent.PrivacyMask);
					FireNewRawVideoHeaderSampleEvent(timestamp, array, e2.LiveContent.PrivacyMask);
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("RawLiveSourceContentEvent", exception);
		}
	}

	private byte[] RemovePrefix(byte[] bytesReceived, out short codec, out DateTime timestamp)
	{
		int num = 32;
		int num2 = BitConverter.ToInt32(GetReversedSubarray(bytesReceived, 2, 4), 0);
		codec = BitConverter.ToInt16(GetReversedSubarray(bytesReceived, 6, 2), 0);
		long num3 = BitConverter.ToInt64(GetReversedSubarray(bytesReceived, 20, 8), 0);
		timestamp = TimeConverter.FromDouble(num3);
		int num4 = num2 - num;
		byte[] array = new byte[num4];
		Buffer.BlockCopy(bytesReceived, num, array, 0, num4);
		return array;
	}

	private byte[] GetReversedSubarray(byte[] array, int start, int length)
	{
		return array.Skip(start).Take(length).Reverse()
			.ToArray();
	}

	private bool DoesDataContainVideo(byte[] data)
	{
		if (data[0] == 60)
		{
			return false;
		}
		return true;
	}

	private object PlaybackTimeChangedHandler(Message message, FQID dest, FQID sender)
	{
		lock (_fetchTimeLock)
		{
			DateTime nextToFetchTime = (DateTime)message.Data;
			_nextToFetchTime = nextToFetchTime;
			return null;
		}
	}

	private object PlaybackCommandHandler(Message message, FQID dest, FQID sender)
	{
		lock (_rawVideoSourceLock)
		{
			if (message.Data is PlaybackCommandData { Command: { Length: var length } command })
			{
				switch (length)
				{
				case 8:
					switch (command[1])
					{
					case 'r':
						if (command == "Previous")
						{
							_nextCommand = MyPlayCommand.PrevFrame;
							FireNewErrorMessage("Previous not supported");
						}
						break;
					case 'l':
						if (command == "PlayStop")
						{
							_mode = "Stop";
							_wasStopped = true;
						}
						break;
					}
					goto end_IL_0009;
				case 4:
					switch (command[0])
					{
					default:
						goto end_IL_0009;
					case 'N':
						if (command == "Next")
						{
							_nextCommand = MyPlayCommand.NextFrame;
						}
						goto end_IL_0009;
					case 'P':
						if (command == "Play")
						{
							break;
						}
						goto end_IL_0009;
					}
					break;
				case 11:
					switch (command[4])
					{
					default:
						goto end_IL_0009;
					case 'F':
						if (command == "PlayForward")
						{
							break;
						}
						goto end_IL_0009;
					case 'R':
						if (command == "PlayReverse")
						{
							_mode = "Stop";
							_wasStopped = true;
							FireNewErrorMessage("Reverse playback not supported");
						}
						goto end_IL_0009;
					}
					break;
				case 5:
					if (command == "Begin")
					{
						_nextCommand = MyPlayCommand.Start;
					}
					goto end_IL_0009;
				case 3:
					if (command == "End")
					{
						_nextCommand = MyPlayCommand.End;
					}
					goto end_IL_0009;
				case 16:
					if (command == "PreviousSequence")
					{
						_nextCommand = MyPlayCommand.PrevSequence;
					}
					goto end_IL_0009;
				case 12:
					if (command == "NextSequence")
					{
						_nextCommand = MyPlayCommand.NextSequence;
					}
					goto end_IL_0009;
				default:
					goto end_IL_0009;
				}
				_mode = "Forward";
			}
			end_IL_0009:;
		}
		new TrackEventData("SmartClient.PlaybackCommand", deprecated: false, (message.Data is PlaybackCommandData) ? ((PlaybackCommandData)message.Data).Command : null).Send();
		return null;
	}

	private void ModeHandler(Mode newMode)
	{
		if (_rawVideoSource != null)
		{
			ClosePlaybackSession();
		}
		if (_rawLiveSource != null)
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
			throw new ArgumentMIPException("Missing camera");
		}
		if (Item.FQID.ServerId.UserContext != null)
		{
			_selectedItem = Item.FQID.ServerId.UserContext.Configuration.GetItem(Item.FQID);
		}
		else
		{
			_selectedItem = Configuration.Instance.GetItem(Item.FQID);
		}
		if (_selectedItem == null)
		{
			throw new MIPException("Cannot find camera");
		}
		SetUpApplicationEventListeners();
		lock (_rawVideoSourceLock)
		{
			_rawVideoSource = new RawVideoSource(_selectedItem);
			_rawVideoSource.Init();
		}
		_stopPlayback = false;
	}

	private void ClosePlaybackSession()
	{
		RemoveApplicationEventListeners();
		_stopPlayback = true;
	}

	private void VideoSourceFetchThread()
	{
		try
		{
			while (!_stop && EnvironmentManager.Instance.ApplicationLoggedOn)
			{
				bool errorRecovery = false;
				HandleStopRequest(out errorRecovery);
				if (errorRecovery && !_stop)
				{
					Thread.Sleep(3000);
					continue;
				}
				HandleNextCommand(out errorRecovery);
				if (!errorRecovery)
				{
					int num = 10;
					if (ReadyToPerformVideoRequest())
					{
						num = FetchAndSendVideo(out errorRecovery);
					}
					if (num > 0)
					{
						Thread.Sleep(num);
					}
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "RawSource", ex.Message);
		}
		if (_rawVideoSource != null)
		{
			StopPlayback();
		}
		_fetchThread = null;
	}

	private int FetchAndSendVideo(out bool errorRecovery)
	{
		errorRecovery = false;
		int val = 10;
		DateTime nextToFetchTime = _nextToFetchTime;
		DateTime dateTime = ((nextToFetchTime.Kind == DateTimeKind.Local) ? nextToFetchTime.ToUniversalTime() : nextToFetchTime);
		try
		{
			lock (_rawVideoSourceLock)
			{
				if (_wasStopped)
				{
					_rawVideoSource.GoTo(dateTime, "Forward");
					_groupOfPictures.Clear();
				}
				RawVideoSourceData nextRawVideoFrame = GetNextRawVideoFrame(dateTime);
				if (nextRawVideoFrame != null)
				{
					long num = nextRawVideoFrame.DateTime.Ticks - dateTime.Ticks;
					long num2 = -5000L;
					if (_wasStopped && num < num2)
					{
						throw new MIPException("No data available after given timestamp");
					}
					val = (nextRawVideoFrame.IsNextAvailable ? ((int)((nextRawVideoFrame.NextDateTime - dateTime).TotalMilliseconds / _playbackController.PlaybackSpeed)) : 10);
					RawVideoSourceDataTimeStamp(dateTime, nextRawVideoFrame);
					SendRawVideoSource(nextRawVideoFrame);
				}
				else if (_groupOfPictures.Any())
				{
					RawVideoSourceData rawVideoSourceData = _groupOfPictures.First();
					PlaybackTimeInformationData data = new PlaybackTimeInformationData
					{
						Item = _selectedItem.FQID,
						PreviousTime = dateTime.AddMilliseconds(-1.0),
						CurrentTime = dateTime,
						NextTime = rawVideoSourceData.DateTime
					};
					EnvironmentManager.Instance.SendMessage(new Message("SmartClient.PlaybackDeviceInformation", data), PlaybackFQID, _fqid);
				}
				_wasStopped = false;
			}
		}
		catch (Exception ex)
		{
			FireNewErrorMessage(ex.ToString());
			errorRecovery = true;
			StopPlayback();
		}
		return Math.Min(val, 10);
	}

	private RawVideoSourceData GetNextRawVideoFrame(DateTime time)
	{
		RawVideoSourceData result = null;
		if (_groupOfPictures.Any())
		{
			if (time >= _groupOfPictures.First().DateTime)
			{
				result = _groupOfPictures.Dequeue();
				if (!_groupOfPictures.Any())
				{
					GetNextGroupOfPicturesAndBufferThem();
				}
			}
		}
		else
		{
			GetNextGroupOfPicturesAndBufferThem();
			if (_groupOfPictures.Any() && time >= _groupOfPictures.First().DateTime)
			{
				result = _groupOfPictures.Dequeue();
			}
		}
		return result;
	}

	private bool ReadyToPerformVideoRequest()
	{
		if (_nextToFetchTime != DateTime.MinValue && _nextToFetchTime != _currentShownTime && _rawVideoSource != null)
		{
			return !_requestInProgress;
		}
		return false;
	}

	private void HandleStopRequest(out bool errorRecovery)
	{
		errorRecovery = false;
		try
		{
			if (_stopPlayback)
			{
				StopPlayback();
			}
		}
		catch
		{
			StopPlayback();
			errorRecovery = true;
		}
	}

	private void GetNextGroupOfPicturesAndBufferThem()
	{
		lock (_rawVideoSourceLock)
		{
			if (_rawVideoSource == null || !(_rawVideoSource?.GetNext() is RawVideoSourceDataList rawVideoSourceDataList))
			{
				return;
			}
			foreach (RawVideoSourceData item in rawVideoSourceDataList.List)
			{
				_groupOfPictures.Enqueue(item);
			}
		}
	}

	private bool WillResultInSameFrame(DateTime utcTime)
	{
		if (_currentTimeInformation != null && _currentTimeInformation.NextTime > utcTime)
		{
			return _currentTimeInformation.PreviousTime < utcTime;
		}
		return false;
	}

	private PlaybackTimeInformationData CreatePlaybackTimeInformation(RawVideoSourceData rawVideoSourceData)
	{
		return new PlaybackTimeInformationData
		{
			Item = _selectedItem.FQID,
			CurrentTime = rawVideoSourceData.DateTime,
			NextTime = rawVideoSourceData.NextDateTime,
			PreviousTime = rawVideoSourceData.PreviousDateTime
		};
	}

	private void HandleNextCommand(out bool errorRecovery)
	{
		errorRecovery = false;
		if (!Selected || _nextCommand == MyPlayCommand.None || _rawVideoSource == null || _requestInProgress)
		{
			return;
		}
		try
		{
			RawVideoSourceDataList rawVideoSourceDataByNextCommand = GetRawVideoSourceDataByNextCommand();
			if (rawVideoSourceDataByNextCommand != null)
			{
				rawVideoSourceDataByNextCommand.List.ForEach(delegate(RawVideoSourceData r)
				{
					_groupOfPictures.Enqueue(r);
				});
				DateTime dateTime = rawVideoSourceDataByNextCommand.List[0].DateTime;
				lock (_fetchTimeLock)
				{
					_nextToFetchTime = dateTime;
				}
			}
		}
		catch
		{
			errorRecovery = true;
			StopPlayback();
			Thread.Sleep(3000);
		}
		_nextCommand = MyPlayCommand.None;
	}

	private RawVideoSourceDataList GetRawVideoSourceDataByNextCommand()
	{
		RawVideoSourceDataList result = null;
		lock (_rawVideoSourceLock)
		{
			if (_rawVideoSource != null)
			{
				switch (_nextCommand)
				{
				case MyPlayCommand.Start:
					result = _rawVideoSource.GetBegin();
					break;
				case MyPlayCommand.End:
					result = _rawVideoSource.GetEnd();
					break;
				case MyPlayCommand.PrevSequence:
					result = _rawVideoSource.GetPreviousSequence();
					break;
				case MyPlayCommand.NextSequence:
					result = _rawVideoSource.GetNextSequence();
					break;
				case MyPlayCommand.PrevFrame:
					result = _rawVideoSource.GetPrevious();
					break;
				case MyPlayCommand.NextFrame:
					result = _rawVideoSource.GetNext() as RawVideoSourceDataList;
					break;
				}
			}
		}
		return result;
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

	private static void RawVideoSourceDataTimeStamp(DateTime utcTime, RawVideoSourceData videoSource)
	{
		if (videoSource != null && !videoSource.IsPreviousAvailable)
		{
			if (utcTime - TimeSpan.FromMilliseconds(10.0) < videoSource.DateTime)
			{
				videoSource.PreviousDateTime = utcTime - TimeSpan.FromMilliseconds(10.0);
			}
			else
			{
				videoSource.PreviousDateTime = videoSource.DateTime;
			}
		}
	}

	private void StopPlayback()
	{
		lock (_rawVideoSourceLock)
		{
			if (_rawVideoSource != null)
			{
				_rawVideoSource.Close();
			}
			_groupOfPictures.Clear();
			_rawVideoSource = null;
		}
		_nextCommand = MyPlayCommand.None;
		lock (_fetchTimeLock)
		{
			_nextToFetchTime = DateTime.MinValue;
		}
		_stopPlayback = false;
	}

	private void SendRawVideoSource(RawVideoSourceData rawVideoSourceData)
	{
		if (WillResultInSameFrame(rawVideoSourceData.DateTime))
		{
			if (Selected)
			{
				EnvironmentManager.Instance.SendMessage(new Message("SmartClient.PlaybackDeviceInformation", _currentTimeInformation), PlaybackFQID, _fqid);
			}
			return;
		}
		_currentShownTime = rawVideoSourceData.DateTime;
		_currentTimeInformation = CreatePlaybackTimeInformation(rawVideoSourceData);
		if (Selected)
		{
			EnvironmentManager.Instance.SendMessage(new Message("SmartClient.PlaybackDeviceInformation", _currentTimeInformation), PlaybackFQID, _fqid);
			FireNewRawVideoSampleEvent(rawVideoSourceData.DateTime, rawVideoSourceData.Content, rawVideoSourceData.PrivacyMask);
			byte[] array = new byte[32];
			Array.Copy(rawVideoSourceData.Content, array, 32);
			FireNewRawVideoHeaderSampleEvent(rawVideoSourceData.DateTime, array, rawVideoSourceData.PrivacyMask);
		}
	}
}

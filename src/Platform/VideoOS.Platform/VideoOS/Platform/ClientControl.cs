using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.AlarmClient;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform;

public class ClientControl
{
	public delegate void NewImageViewerControlHandler(ImageViewerAddOn imageViewerAddOn);

	public static readonly string ExportTimeIntervals = "ExportTimeIntervals";

	public static string EmbeddedCameraFQIDProperty = "EmbeddedCameraFQID";

	public static Guid LiveBuildInWorkSpaceId = new Guid("B8C936BF-815D-4D5D-BAAF-1CEAFC7428C0");

	public static Guid PlaybackBuildInWorkSpaceId = new Guid("DA486915-863C-4ECE-B301-5FC7FD80A2B0");

	public static Guid SequenceExplorerBuildInWorkSpaceId = new Guid("955323C5-2ECC-48CE-93A9-3CF1835A4A23");

	public static Guid SystemMonitorBuildInWorkSpaceId = new Guid("FFFA9495-2D44-4183-97CB-550837441294");

	public static Guid AlarmManagementBuildInWorkSpaceId = new Guid("5A77E227-1A5E-4D7B-B345-46A7BB993037");

	public static Guid AccessControlBuildInWorkSpaceId = new Guid("CE411EB1-3593-4681-A3E1-1BA2D1A761B8");

	public static Guid TransactBuildInWorkSpaceId = new Guid("952F54A2-08EE-44F7-B010-86DCA20BA2B2");

	public static Guid ExportBuildInWorkSpaceId = new Guid("7DB87263-F9AB-426D-B641-91D9E4BD7B86");

	private static ClientControl _instance = new ClientControl();

	private AlarmClientManager _alarmClientManager = new AlarmClientManager();

	private object _msg1;

	private object _msg2;

	private object _msg3;

	private object _msg4;

	protected PlaybackControllerBase PlaybackControllerBase;

	private Dictionary<Guid, WeakReference> _extraPlaybackControllers = new Dictionary<Guid, WeakReference>();

	private Theme _theme = new Theme();

	public static ClientControl Instance
	{
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual TimeZoneInfo SelectedTimeZoneInfo { get; set; }

	public virtual System.Windows.FlowDirection FlowDirection { get; } = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? System.Windows.FlowDirection.RightToLeft : System.Windows.FlowDirection.LeftToRight;

	public virtual Item ShownWorkSpace
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual WorkSpaceState WorkSpaceState
	{
		get
		{
			return WorkSpaceState.Normal;
		}
		set
		{
		}
	}

	public virtual Theme Theme => _theme;

	public virtual TelemetryClient TelemetryClient { get; } = new TelemetryClient();

	public virtual bool Initialized => false;

	public event EventHandler FlowDirectionChanged;

	public virtual event NewImageViewerControlHandler NewImageViewerControlEvent;

	public virtual event EventHandler ApplicationCloseEvent;

	protected ClientControl()
	{
	}

	public virtual void Init()
	{
		if (PlaybackControllerBase == null)
		{
			PlaybackControllerBase = new PlaybackControllerBase();
		}
		PlaybackControllerBase.FQID = null;
		PlaybackControllerBase.Init();
		RegisterReceivers();
	}

	protected void RegisterReceivers()
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
		{
			_msg1 = EnvironmentManager.Instance.RegisterReceiver(NewAlarmHandler, new MessageIdListFilter(new string[2] { "Server.NewAlarmCommand", "Server.NewAlarmAsEventCommand" }));
			_msg2 = EnvironmentManager.Instance.RegisterReceiver(NewEventHandler, new MessageIdFilter("Server.NewEventCommand"));
			_msg3 = EnvironmentManager.Instance.RegisterReceiver(NewEventAsAlarmHandler, new MessageIdFilter("Server.NewEventAsAlarmHandlerCommand"));
		}
		_msg4 = EnvironmentManager.Instance.RegisterReceiver(ApplicationLoggedOnHandler, new MessageIdFilter(MessageId.System.ApplicationLoggedOnIndication));
	}

	public virtual void Close()
	{
		if (PlaybackControllerBase != null)
		{
			PlaybackControllerBase.Close();
		}
		if (_msg1 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msg1);
		}
		if (_msg2 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msg2);
		}
		if (_msg3 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msg3);
		}
		if (_msg4 != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msg4);
		}
		_msg1 = (_msg2 = (_msg3 = (_msg4 = null)));
		_alarmClientManager.Close();
		if (Configuration.Instance != null)
		{
			Configuration.Instance.Logoff();
		}
	}

	public virtual void UnInitialize()
	{
		if (Initialized)
		{
			Close();
			Instance = new ClientControl();
		}
	}

	protected void RaiseFlowDirectionChanged()
	{
		this.FlowDirectionChanged?.Invoke(this, EventArgs.Empty);
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use ImageViewerWpfControl instead.", false)]
	public virtual ImageViewerControl GenerateImageViewerControl()
	{
		HandleNotImplemented();
		return null;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use ImageViewerWpfControl instead.", false)]
	public virtual ImageViewerControl GenerateImageViewerControl(WindowInformation windowInformation)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual VideoControl GenerateVideoControl()
	{
		throw new MIPException("Method not supported for this Environment");
	}

	public virtual ViewControl GenerateViewControl(List<ViewControlItem> items, List<Rectangle> viewLayout, WindowInformation windowInformation)
	{
		throw new MIPException("Method not supported for this Environment");
	}

	public virtual ViewControl GenerateViewControl(List<ViewControlItem> items, List<Rectangle> viewLayout)
	{
		throw new MIPException("Method not supported for this Environment");
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use AudioPlayer instead.", false)]
	public virtual AudioPlayerControl GenerateAudioPlayerControl()
	{
		HandleNotImplemented();
		return null;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use AudioPlayer instead.", false)]
	public virtual AudioPlayerControl GenerateAudioPlayerControl(WindowInformation windowInformation)
	{
		return Instance.GenerateAudioPlayerControl();
	}

	public virtual IAudioRecorder GeneratePcAudioRecorder()
	{
		throw new NotImplementedException("GeneratePcAudioRecorder not available in this MIP Environment");
	}

	public virtual AdminImageViewerControl GenerateAdminImageViewerControl()
	{
		HandleNotImplemented();
		return null;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use PlaybackWpfUserControl instead and use ShowTallUserControl to control the size.", false)]
	public virtual PlaybackUserControl GeneratePlaybackUserControl()
	{
		return new PlaybackUserControlMini();
	}

	public virtual FQID GetPlaybackController(WindowInformation windowInformation)
	{
		return null;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use PlaybackWpfUserControl instead and use ShowTallUserControl to control the size.", false)]
	public virtual PlaybackUserControl GeneratePlaybackUserControl(WindowInformation windowInformation)
	{
		return new PlaybackUserControlMini();
	}

	public virtual List<Item> GetViewGroupItems()
	{
		HandleNotImplemented();
		return null;
	}

	public virtual List<Item> GetSmartWallItems()
	{
		return null;
	}

	public virtual List<Item> GetWorkSpaceItems()
	{
		return null;
	}

	public virtual Item CreateTemporaryGroupItem(string name)
	{
		return null;
	}

	public virtual void DeleteTemporaryGroupItem(Item groupItem)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use RegisterUIControlForAutoTheming(FrameworkElement) instead.", false)]
	public virtual void RegisterUIControlForAutoTheming(object uiControl)
	{
	}

	public virtual void RegisterUIControlForAutoTheming(FrameworkElement uiControl)
	{
	}

	public virtual void CallOnUiThread(Action actionToBeInvoked)
	{
		actionToBeInvoked();
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use ShowHelp(int) instead.", false)]
	public virtual void ShowHelp(Control control, int id)
	{
	}

	public virtual void ShowHelp(int topicId)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. For pure WPF plugins a method like this is not needed.", false)]
	protected virtual void FireSignalKeyDownEvent(Keys keys)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. For pure WPF plugins a method like this is not needed.", false)]
	public virtual void RaiseKeyDownEvent(KeyEventArgs e)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use Print(ImageSource, String, String) instead.", false)]
	public virtual void Print(Bitmap bitmap, string name, string extraInformation)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use Print(ImageSource, String, String, string) instead.", false)]
	public virtual void Print(Bitmap bitmap, string name, string extraInformation, string cameraName)
	{
	}

	public virtual void Print(ImageSource imageSource, string name, string extraInformation)
	{
	}

	public virtual void Print(ImageSource imageSource, string name, string extraInformation, string cameraName)
	{
	}

	public virtual FQID GeneratePlaybackController()
	{
		PlaybackControllerBase playbackControllerBase = new PlaybackControllerBase();
		playbackControllerBase.Init();
		_extraPlaybackControllers[playbackControllerBase.FQID.ObjectId] = new WeakReference(playbackControllerBase);
		new TrackPageViewData(typeof(PlaybackController), deprecated: false, "GeneratePlaybackController").Send();
		return playbackControllerBase.FQID;
	}

	public virtual PlaybackController GetPlaybackController(FQID fqid)
	{
		if (fqid == null || fqid.ObjectId == Guid.Empty)
		{
			if (PlaybackControllerBase == null)
			{
				PlaybackControllerBase = new PlaybackControllerBase();
				PlaybackControllerBase.FQID = null;
				PlaybackControllerBase.Init();
				new TrackPageViewData(typeof(PlaybackController), deprecated: false, "GetPlaybackController").Send();
			}
			return PlaybackControllerBase;
		}
		if (_extraPlaybackControllers.ContainsKey(fqid.ObjectId))
		{
			WeakReference weakReference = _extraPlaybackControllers[fqid.ObjectId];
			if (weakReference.IsAlive)
			{
				return weakReference.Target as PlaybackController;
			}
			_extraPlaybackControllers.Remove(fqid.ObjectId);
		}
		return null;
	}

	public virtual void ReleasePlaybackController(FQID fqid)
	{
		if (fqid != null && _extraPlaybackControllers.ContainsKey(fqid.ObjectId))
		{
			WeakReference weakReference = _extraPlaybackControllers[fqid.ObjectId];
			if (weakReference.IsAlive && weakReference.Target is PlaybackControllerBase playbackControllerBase)
			{
				playbackControllerBase.Close();
			}
			_extraPlaybackControllers.Remove(fqid.ObjectId);
		}
	}

	private static void HandleNotImplemented()
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Standalone)
		{
			EnvironmentManager.Instance.Log(error: true, "TryLoad", "VideoOS.Platform.SDK.UI.Environment.Initialize() not called.", null);
			throw new MIPException("VideoOS.Platform.SDK.UI.Environment.Initialize() not called.");
		}
		throw new MIPException("Method not supported for this Environment");
	}

	public virtual SmartClientProfileSetting GetSmartClientProfileSetting(int smartClientProfileSettingKey)
	{
		return null;
	}

	public virtual WindowState GetWindowState(FQID window)
	{
		throw new MIPException("Method not supported for this Environment");
	}

	private object NewEventHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
	{
		try
		{
			if (message.Data is BaseEvent baseEvent)
			{
				MoveImageToSnapshot(baseEvent);
				ServerId serverId = Configuration.Instance.ServerFQID.ServerId;
				if (baseEvent.EventHeader != null && baseEvent.EventHeader.Source != null && baseEvent.EventHeader.Source.FQID != null)
				{
					serverId = baseEvent.EventHeader.Source.FQID.ServerId;
				}
				IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(serverId);
				if (alarmClient == null)
				{
					alarmClient = _alarmClientManager.GetAlarmClient(EnvironmentManager.Instance.MasterSite.ServerId);
				}
				if (alarmClient != null)
				{
					alarmClient.AddEvent(baseEvent);
				}
				else
				{
					EnvironmentManager.Instance.Log(error: false, "NewEventHandler:", "Event not issued - the EventServer could not be found based on the ServerId:" + serverId.ToString());
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("NewEventHandler: Event not issued", exception);
		}
		new TrackEventData("Server.NewEventCommand").Send();
		return null;
	}

	private static object ApplicationLoggedOnHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
	{
		PluginManager.RebuildFromPlugins();
		return null;
	}

	private object NewEventAsAlarmHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
	{
		try
		{
			IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(Configuration.Instance.ServerFQID.ServerId);
			if (alarmClient != null && message.Data is BaseEvent baseEvent)
			{
				Alarm alarm = AlarmBuilder.CreateAlarmFromEvent(baseEvent);
				if (alarm != null)
				{
					alarmClient.Add(alarm);
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("NewEventAsAlarmHandler:Event not issued", exception);
		}
		new TrackEventData("Server.NewEventAsAlarmHandlerCommand", deprecated: true).Send();
		return null;
	}

	private object NewAlarmHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
	{
		try
		{
			if (!(message.Data is Alarm alarm))
			{
				EnvironmentManager.Instance.Log(error: true, "NewAlarmHandler", "Message.Data is not an Alarm", null);
			}
			else
			{
				ServerId serverId = EnvironmentManager.Instance.MasterSite.ServerId;
				if (alarm.EventHeader != null && alarm.EventHeader.Source != null && alarm.EventHeader.Source.FQID != null)
				{
					serverId = alarm.EventHeader.Source.FQID.ServerId;
				}
				IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(serverId);
				if (alarmClient != null)
				{
					if (message.MessageId == "Server.NewAlarmCommand")
					{
						alarmClient.Add(alarm);
					}
					else if (message.MessageId == "Server.NewAlarmAsEventCommand")
					{
						EnvironmentManager.Instance.Log(error: true, "NewAlarmAsEventCommand", "NewAlarmAsEventCommand is no longer supported. Please, use NewEventCommand instead.", null);
					}
					return null;
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("NewAlarmHandler:Alarm not created", exception);
		}
		return null;
	}

	private void MoveImageToSnapshot(BaseEvent baseEvent)
	{
		if (baseEvent is EventData { EventSequence: not null } eventData && eventData.EventSequence.SnapshotImages != null)
		{
			List<byte[]> list = new List<byte[]>(eventData.EventSequence.SnapshotJPEGs ?? Array.Empty<byte[]>());
			Image[] snapshotImages = eventData.EventSequence.SnapshotImages;
			foreach (Image image in snapshotImages)
			{
				list.Add(ImageToBytes(image));
			}
			eventData.EventSequence.SnapshotJPEGs = list.ToArray();
			eventData.EventSequence.SnapshotImages = Array.Empty<Image>();
		}
	}

	private byte[] ImageToBytes(Image image)
	{
		if (image == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Jpeg);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return memoryStream.ToArray();
	}

	public virtual ImageViewerWpfControlInternal CreateImageViewerWpfControlInternal(WindowInformation windowInformation)
	{
		return new DummyImageViewerWpfControlInternal(windowInformation);
	}

	public virtual PlaybackWpfUserControlInternal CreatePlaybackWpfUserControlInternal(WindowInformation windowInformation)
	{
		return new PlaybackWpfUserControlMini();
	}

	public virtual IAudioPlayerInternal CreateAudioPlayerInternal(FQID playbackControllerFQID)
	{
		return null;
	}
}

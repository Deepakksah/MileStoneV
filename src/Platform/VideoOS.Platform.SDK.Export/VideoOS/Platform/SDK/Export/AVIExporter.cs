using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using VideoOS.AVIExporterClient;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Export.Resources;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Export;

public class AVIExporter : IAVIExporter
{
	private AVIExportClient _avi;

	private bool _aviInUse;

	private List<Item> _cameraList = new List<Item>();

	private List<Item> _audioList = new List<Item>();

	private string _startupError = string.Empty;

	private bool _initializing;

	private bool _initLateUpdate;

	private bool _sequenceExport;

	private object _lock = new object();

	private Dictionary<Guid, LoginSettings> _tokenEventForRecorders = new Dictionary<Guid, LoginSettings>();

	private Bitmap _overlayBitmap;

	private VerticalOverlayPosition _verticalOverlayPosition;

	private HorizontalOverlayPosition _horizontalOverlayPosition;

	private double _scaleFactor = 1.0;

	private bool _ignoreAspect;

	public override List<Item> CameraList
	{
		get
		{
			return _cameraList;
		}
		set
		{
			_cameraList = ((value == null) ? new List<Item>() : value);
		}
	}

	public override List<Item> AudioList
	{
		get
		{
			return _audioList;
		}
		set
		{
			_audioList = ((value == null) ? new List<Item>() : value);
		}
	}

	public override string Path { get; set; }

	public override string Filename { get; set; }

	public override bool Timestamp { get; set; }

	public override string Codec { get; set; }

	public override string[] CodecList
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			try
			{
				AVIExportClient val = new AVIExportClient();
				AVICodec[] possibleVideoCodecs = val.GetPossibleVideoCodecs((uint)Width, (uint)Height);
				string[] array = new string[possibleVideoCodecs.Length];
				for (int i = 0; i < possibleVideoCodecs.Length; i++)
				{
					array[i] = possibleVideoCodecs[i].Description;
				}
				val.Dispose();
				return array;
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "AVIExporter.CodecList", "Exception from Codecs: " + ex.ToString());
				throw new MIPException("CodecList issue, does all codec run in current execution mode (64 bit)?", ex);
			}
		}
	}

	public override int FrameRate { get; set; }

	public override int Width { get; set; }

	public override int Height { get; set; }

	public override int Channels { get; set; }

	public override int AudioSampleRate { get; set; }

	public override int AudioSampleDepth { get; set; }

	public override Rectangle Window { get; set; }

	public override int Progress
	{
		get
		{
			if (_avi != null)
			{
				return (int)_avi.QueryProgress();
			}
			return -2;
		}
	}

	public override int LastError
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			if (_avi == null)
			{
				return -1;
			}
			return (int)_avi.GetLastErrorCode();
		}
	}

	public override int LastDetailedError
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			if (_avi == null)
			{
				return -1;
			}
			return (int)_avi.GetLastDetailedError();
		}
	}

	public override string LastErrorString
	{
		get
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(_startupError))
			{
				return _startupError;
			}
			if (_avi == null)
			{
				return "";
			}
			return _avi.GetLastErrorDescription() + ": " + ((object)_avi.GetLastDetailedError()/*cast due to constrained. prefix*/).ToString();
		}
	}

	public AVIExporter()
	{
		Codec = "Cinepak Codec by Radius";
		FrameRate = 8;
		Channels = 1;
		AudioSampleRate = 8000;
		AudioSampleDepth = 16;
		AutoSplitExportFile = true;
		MaxAVIFileSize = 536870912;
	}

	public override void Init()
	{
		ManagedWrapper.minitialize();
	}

	public override void Close()
	{
		ManagedWrapper.mterminate();
	}

	public override bool SetOverlayImage(Bitmap overlayBitmap, int verticalOverlayPosition, int horizontalOverlayPosition, double scaleFactor, bool ignoreAspect)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (overlayBitmap == null)
		{
			SetError(Strings.AVIExporterError_OverlayImageNotSet);
			return false;
		}
		_overlayBitmap = overlayBitmap;
		if (!Enum.IsDefined(typeof(VerticalOverlayPosition), verticalOverlayPosition) || !Enum.IsDefined(typeof(HorizontalOverlayPosition), horizontalOverlayPosition))
		{
			SetError(Strings.AVIExporterError_OverlayImageInvalidPosition);
			_overlayBitmap = null;
			return false;
		}
		_verticalOverlayPosition = (VerticalOverlayPosition)verticalOverlayPosition;
		_horizontalOverlayPosition = (HorizontalOverlayPosition)horizontalOverlayPosition;
		_scaleFactor = scaleFactor;
		_ignoreAspect = ignoreAspect;
		return true;
	}

	private void SetError(string errorText)
	{
		_startupError = errorText;
	}

	private bool StartExport(Func<Tuple<DateTime, DateTime>> openStreamsAction)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		if (_avi != null)
		{
			SetError(Strings.AVIExporterError_ExportAlreadyRunning);
			return false;
		}
		_startupError = string.Empty;
		if (string.IsNullOrEmpty(Filename))
		{
			SetError(Strings.AVIExporterError_FilenameNotSet);
			return false;
		}
		_initializing = true;
		_avi = new AVIExportClient();
		uint num = ((Width == 0) ? 320u : ((uint)Width));
		uint num2 = ((Height == 0) ? 240u : ((uint)Height));
		AVICodec[] possibleVideoCodecs = _avi.GetPossibleVideoCodecs(num, num2);
		if (possibleVideoCodecs.GetLength(0) == 0)
		{
			return false;
		}
		AVICodec val = null;
		if (Codec != null && !Codec.Equals(""))
		{
			AVICodec[] array = possibleVideoCodecs;
			foreach (AVICodec val2 in array)
			{
				if (val2.Description.ToLower().Equals(Codec.ToLower()))
				{
					val = val2;
				}
			}
		}
		if (val == null)
		{
			AVICodec[] array = possibleVideoCodecs;
			foreach (AVICodec val3 in array)
			{
				string description = val3.Description;
				if (description.Equals("Cinepak Codec by Radius"))
				{
					val = val3;
					break;
				}
				if (description.Equals("Microsoft Video 1"))
				{
					val = val3;
				}
			}
		}
		if (val == null)
		{
			val = possibleVideoCodecs[0];
		}
		Codec = val.Description;
		Tuple<DateTime, DateTime> tuple = openStreamsAction();
		if (tuple == null)
		{
			return false;
		}
		_avi.SetIncludeTimeStamp(Timestamp);
		_avi.SetAutoSplitExportFile(AutoSplitExportFile);
		_avi.SetMaxAVIFileSize(MaxAVIFileSize);
		_ = Window;
		if (Window.Height > 0 && Window.Width > 0)
		{
			uint width = (uint)Width;
			uint height = (uint)Height;
			_avi.SetDigitalZoomRectangle(width, height, (uint)Window.Left, (uint)Window.Top, (uint)Window.Right, (uint)Window.Bottom);
		}
		if (_overlayBitmap != null && !_avi.SetOverlayImage(_overlayBitmap, _verticalOverlayPosition, _horizontalOverlayPosition, _scaleFactor, _ignoreAspect))
		{
			return false;
		}
		if (!Filename.ToLower().EndsWith(".avi"))
		{
			Filename += ".avi";
		}
		_aviInUse = true;
		bool flag;
		try
		{
			flag = _avi.StartExport(val, tuple.Item1, tuple.Item2, System.IO.Path.Combine(Path, Filename));
		}
		finally
		{
			_aviInUse = false;
		}
		ErrorCode lastErrorCode = _avi.GetLastErrorCode();
		if (!flag || (int)lastErrorCode != 0)
		{
			return false;
		}
		_initializing = false;
		if (_initLateUpdate)
		{
			_initLateUpdate = false;
			TokenChangedEventHandler(null, null);
		}
		return true;
	}

	public override bool StartExport(DateTime start, DateTime end)
	{
		_startupError = string.Empty;
		if (_cameraList.Count == 0 && _audioList.Count == 0)
		{
			SetError(Strings.AVIExporterError_NoExportSourcesSet);
			return false;
		}
		if (!DBExporterXpco.CheckExportAuthorization(_cameraList, aviExport: true) || !DBExporterXpco.CheckExportAuthorization(_audioList, aviExport: true))
		{
			SetError(Strings.AVIExporterError_NotAuthorized);
			return false;
		}
		_sequenceExport = false;
		return StartExport(delegate
		{
			DateTime dateTime = start;
			DateTime dateTime2 = end;
			if (_cameraList != null && _cameraList.Count > 0)
			{
				MediaDataInspector.DataInformation dataInformation = MediaDataInspector.TrimStartAndEndTime(_cameraList[0].FQID, start, end);
				if (!dataInformation.RecorderOnline)
				{
					SetError(Strings.AVIExporterError_RecorderOffline);
					return (Tuple<DateTime, DateTime>)null;
				}
				if (!dataInformation.ContainsData)
				{
					SetError(Strings.AVIExporterError_NoVideoInSelectedTimePeriod);
					return (Tuple<DateTime, DateTime>)null;
				}
				dateTime = dataInformation.StartTime;
				dateTime2 = dataInformation.EndTime;
			}
			if (_cameraList != null && _cameraList.Count > 0)
			{
				if (_cameraList[0].FQID.ServerId.UserContext != null)
				{
					_avi.RemovableMasksLifted = _cameraList[0].FQID.ServerId.UserContext.PrivacyMaskLifted;
				}
				CameraConfiguration val = BuildCameraConfiguration(_cameraList[0], dateTime, dateTime2);
				if (val == null)
				{
					SetError(Strings.AVIExporterError_FailedToCreateCameraConfigurationXML);
					return (Tuple<DateTime, DateTime>)null;
				}
				_avi.OpenVideoStream((uint)FrameRate, (uint)Width, (uint)Height, val);
			}
			if (_audioList != null && _audioList.Count > 0)
			{
				List<AudioDeviceConfiguration> list = BuildAudioConfig();
				double[] array = new double[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 1.0;
				}
				_avi.OpenAudioStream(Channels, AudioSampleRate, AudioSampleDepth, (IEnumerable<AudioDeviceConfiguration>)list.ToArray(), (IEnumerable<double>)array);
			}
			return new Tuple<DateTime, DateTime>(dateTime, dateTime2);
		});
	}

	public override bool StartExport(IEnumerable<SequenceAviExportElement> sequences)
	{
		_startupError = string.Empty;
		if (!sequences.Any())
		{
			SetError(Strings.AVIExporterError_NoSequencesProvided);
			return false;
		}
		_sequenceExport = true;
		_audioList.Clear();
		_cameraList = sequences.Select((SequenceAviExportElement s) => s.Camera).ToList();
		if (!DBExporterXpco.CheckExportAuthorization(_cameraList, aviExport: true))
		{
			SetError(Strings.AVIExporterError_NotAuthorized);
			return false;
		}
		return StartExport(delegate
		{
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			List<VideoExportConfiguration> list = new List<VideoExportConfiguration>();
			foreach (SequenceAviExportElement sequence in sequences)
			{
				MediaDataInspector.DataInformation dataInformation = MediaDataInspector.TrimStartAndEndTime(sequence.Camera.FQID, sequence.StartTime, sequence.EndTime);
				if (!dataInformation.RecorderOnline)
				{
					SetError(string.Format(CultureInfo.CurrentUICulture, Strings.AVIExporterError_RecorderOfflineForCamera, sequence.Camera.Name));
					return (Tuple<DateTime, DateTime>)null;
				}
				if (!dataInformation.ContainsData)
				{
					SetError(string.Format(CultureInfo.CurrentUICulture, Strings.AVIExporterError_NoVideoInSelectedTimePeriodForCamera, sequence.Camera.Name));
					return (Tuple<DateTime, DateTime>)null;
				}
				CameraConfiguration val = BuildCameraConfiguration(sequence.Camera, dataInformation.StartTime, dataInformation.EndTime);
				list.Add(new VideoExportConfiguration(val, dataInformation.StartTime, dataInformation.EndTime, (sequence.OverlayText == null) ? string.Empty : sequence.OverlayText));
			}
			if (_cameraList.Count > 0 && _cameraList[0].FQID.ServerId.UserContext != null)
			{
				_avi.RemovableMasksLifted = _cameraList[0].FQID.ServerId.UserContext.PrivacyMaskLifted;
			}
			_avi.OpenVideoStreams((uint)FrameRate, (uint)Width, (uint)Height, (IEnumerable<VideoExportConfiguration>)list.ToArray());
			return new Tuple<DateTime, DateTime>(list.First().BeginTime, list.Last().EndTime);
		});
	}

	public override void Cancel()
	{
		if (_avi != null)
		{
			_avi.Cancel();
		}
	}

	public override void EndExport()
	{
		lock (_lock)
		{
			ReleaseEventHandlers();
			if (_avi != null)
			{
				_avi.Cancel();
				int num = 1000;
				while (num-- > 0 && _aviInUse)
				{
					Thread.Sleep(25);
				}
				_avi.CloseAudioStream();
				_avi.CloseVideoStream();
				_avi.Dispose();
			}
			_avi = null;
			_cameraList = new List<Item>();
			_audioList = new List<Item>();
		}
	}

	private void TokenChangedEventHandler(object sender, EventArgs e)
	{
		lock (_lock)
		{
			if (_avi == null)
			{
				return;
			}
			if (!_initializing)
			{
				if (_sequenceExport)
				{
					List<CameraConfiguration> list = new List<CameraConfiguration>();
					foreach (Item camera in _cameraList)
					{
						CameraConfiguration item = BuildCameraConfiguration(camera, DateTime.MinValue, DateTime.MinValue);
						list.Add(item);
					}
					_avi.UpdateVideoConnectionParameters((IEnumerable<CameraConfiguration>)list.ToArray());
				}
				else
				{
					CameraConfiguration val = BuildCameraConfiguration(_cameraList[0], DateTime.MinValue, DateTime.MinValue);
					_avi.UpdateVideoConnectionParameters(val);
					List<AudioDeviceConfiguration> list2 = BuildAudioConfig();
					if (list2.Count > 0)
					{
						_avi.UpdateAudioConnectionParameters((IEnumerable<AudioDeviceConfiguration>)list2.ToArray());
					}
				}
			}
			else
			{
				_initLateUpdate = true;
			}
		}
	}

	private void ReleaseEventHandlers()
	{
		foreach (LoginSettings value in _tokenEventForRecorders.Values)
		{
			value.TokenChangedEvent -= TokenChangedEventHandler;
		}
		_tokenEventForRecorders.Clear();
	}

	private CameraConfiguration BuildCameraConfiguration(Item cameraItem, DateTime startTrimmed, DateTime endTrimmed)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		Item parent = cameraItem.GetParent();
		if (parent != null)
		{
			LoginSettings loginSettings = cameraItem.FQID.ServerId.UserContext.GetLoginSettings(parent.FQID);
			if (loginSettings != null)
			{
				if (_initializing && !_tokenEventForRecorders.ContainsKey(parent.FQID.ObjectId))
				{
					_tokenEventForRecorders.Add(parent.FQID.ObjectId, loginSettings);
					loginSettings.TokenChangedEvent += TokenChangedEventHandler;
				}
				return new CameraConfiguration(cameraItem.FQID.ServerId.Uri, cameraItem.FQID.ObjectId, (Guid?)null, loginSettings.Token, BuildDBXml(cameraItem.FQID), startTrimmed, endTrimmed, cameraItem.Name, 0.0, new CameraUserRights(true, true, false, true, false, true, false), (IpixConfiguration)null, (PanoramicLensConfiguration)null, (PtzConfiguration)null);
			}
		}
		return null;
	}

	private List<AudioDeviceConfiguration> BuildAudioConfig()
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		List<AudioDeviceConfiguration> list = new List<AudioDeviceConfiguration>();
		foreach (Item audio in _audioList)
		{
			Item parent = audio.GetParent();
			if (parent == null)
			{
				continue;
			}
			LoginSettings loginSettings = audio.FQID.ServerId.UserContext.GetLoginSettings(parent.FQID);
			if (loginSettings != null)
			{
				if (_initializing && !_tokenEventForRecorders.ContainsKey(parent.FQID.ObjectId))
				{
					_tokenEventForRecorders.Add(parent.FQID.ObjectId, loginSettings);
					loginSettings.TokenChangedEvent += TokenChangedEventHandler;
				}
				list.Add(new AudioDeviceConfiguration(parent.FQID.ServerId.Uri, audio.FQID.ObjectId, loginSettings.Token, BuildDBXml(audio.FQID), DateTime.MinValue, DateTime.MinValue, audio.Name, new UserRights(true, true)));
			}
		}
		return list;
	}

	private static string BuildDBXml(FQID fqid)
	{
		if (fqid.ServerId.IsExportType)
		{
			return ((SDKInternalCommandService)InternalCommandService.Instance).BuildToolkitXml(fqid, 0, 0, "RAW", keepAspectRatio: true, fillSpace: false, allowUpscaling: true);
		}
		return "";
	}
}

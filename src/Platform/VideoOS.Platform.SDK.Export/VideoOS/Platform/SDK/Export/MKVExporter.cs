using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Export;

public class MKVExporter : IMKVExporter
{
	public enum MKVExportError
	{
		NoError,
		Exception,
		Cancelled,
		InvalidState
	}

	private object _sessionId;

	private int _lastError;

	private string _lastMessage = "";

	private List<Item> _cameraList = new List<Item>();

	private List<Item> _audioList = new List<Item>();

	private bool _initializing;

	private bool _initLateUpdate;

	private object _lock = new object();

	private Dictionary<Guid, LoginSettings> _tokenEventForRecorders = new Dictionary<Guid, LoginSettings>();

	private bool _exportInProgress;

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

	public override int Progress
	{
		get
		{
			if (_sessionId != null)
			{
				return SDKInternalCommandService.ToolkitExportPercentCompleted(_sessionId);
			}
			return 100;
		}
	}

	public override int LastError => _lastError;

	public override string LastErrorString => _lastMessage;

	public override void Init()
	{
	}

	public override void Close()
	{
		EndExport();
	}

	public override bool StartExport(DateTime start, DateTime end)
	{
		if (_cameraList.Count == 0 && _audioList.Count == 0)
		{
			return false;
		}
		if (Path == null || Filename == null)
		{
			throw new MIPException("Path ad Filename must be filled");
		}
		if (!DBExporterXpco.CheckExportAuthorization(_cameraList, aviExport: true) || !DBExporterXpco.CheckExportAuthorization(_audioList, aviExport: true))
		{
			throw new NotAuthorizedMIPException("You are not authorized to do export for one or more of the provided devices");
		}
		try
		{
			Item item = ((_cameraList.Count == 0) ? null : _cameraList[0]);
			bool flag = false;
			bool flag2 = false;
			MediaDataInspector.DataInformation dataInformation = null;
			if (item != null)
			{
				dataInformation = MediaDataInspector.TrimStartAndEndTime(item.FQID, start, end);
				flag |= dataInformation.ContainsData;
				if (!dataInformation.RecorderOnline)
				{
					flag2 = true;
				}
			}
			foreach (Item audio in _audioList)
			{
				MediaDataInspector.DataInformation dataInformation2 = MediaDataInspector.TrimStartAndEndTime(audio.FQID, start, end);
				flag |= dataInformation2.ContainsData;
				if (!dataInformation.RecorderOnline)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				throw new CommunicationMIPException("Recorder offline");
			}
			if (!flag)
			{
				throw new NoVideoInTimeSpanMIPException("No video or audio in selected time period");
			}
			if (dataInformation != null && dataInformation.ContainsData)
			{
				start = dataInformation.StartTime;
				end = dataInformation.EndTime;
			}
			RegisterEventHandler(item);
			foreach (Item audio2 in AudioList)
			{
				RegisterEventHandler(audio2);
			}
			_initializing = true;
			if (!Directory.Exists(Path))
			{
				Directory.CreateDirectory(Path);
			}
			if (!Filename.ToLower().EndsWith(".mkv"))
			{
				Filename += ".mkv";
			}
			_sessionId = ((SDKInternalCommandService)InternalCommandService.Instance).ToolkitExportCreate(item, AudioList, start, end, System.IO.Path.Combine(Path, Filename).Replace('\\', '/'));
			if (_sessionId == null)
			{
				return false;
			}
			_initializing = false;
			if (_initLateUpdate)
			{
				_initLateUpdate = false;
				TokenChangedEventHandler(null, null);
			}
			_exportInProgress = true;
			Thread thread = new Thread(Run);
			thread.Name = "MKVExport Thread";
			thread.Start();
			return true;
		}
		catch (NoVideoInTimeSpanMIPException)
		{
			throw;
		}
		catch (CommunicationMIPException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			EnvironmentManager.Instance.ExceptionHandler("MKVExport:StartExport", ex3);
			_lastError = 1;
			_lastMessage = ex3.Message;
			return false;
		}
	}

	private void Run()
	{
		try
		{
			switch (SDKInternalCommandService.ToolkitExportStart(_sessionId))
			{
			case 2:
				_lastError = 0;
				break;
			case 0:
				_lastError = 2;
				break;
			default:
				_lastError = 0;
				break;
			}
		}
		catch (Exception ex)
		{
			_lastError = 1;
			_lastMessage = ex.Message;
		}
		_exportInProgress = false;
	}

	public override void Cancel()
	{
		if (_sessionId != null)
		{
			SDKInternalCommandService.ToolkitExportCancel(_sessionId);
		}
	}

	public override void EndExport()
	{
		while (_exportInProgress)
		{
			Thread.Sleep(500);
		}
		lock (_lock)
		{
			ReleaseEventHandlers();
			if (_sessionId != null)
			{
				((SDKInternalCommandService)InternalCommandService.Instance).ToolkitExportClose(_sessionId);
			}
			_sessionId = null;
			_cameraList = new List<Item>();
			_audioList = new List<Item>();
		}
	}

	private void TokenChangedEventHandler(object sender, EventArgs e)
	{
		lock (_lock)
		{
			if (_sessionId == null)
			{
				return;
			}
			if (!_initializing)
			{
				Item item = ((_cameraList.Count <= 0) ? _audioList[0] : _cameraList[0]);
				Item parent = item.GetParent();
				if (parent != null)
				{
					LoginSettings loginSettings = item.FQID.ServerId.UserContext.GetLoginSettings(parent.FQID);
					if (loginSettings != null)
					{
						SDKInternalCommandService.ToolkitExportUpdateToken(_sessionId, loginSettings.Token);
					}
				}
			}
			else
			{
				_initLateUpdate = true;
			}
		}
	}

	private void RegisterEventHandler(Item item)
	{
		if (item == null)
		{
			return;
		}
		try
		{
			Item parent = item.GetParent();
			if (parent != null)
			{
				LoginSettings loginSettings = item.FQID.ServerId.UserContext.GetLoginSettings(parent.FQID);
				if (loginSettings != null && !_tokenEventForRecorders.ContainsKey(parent.FQID.ObjectId))
				{
					_tokenEventForRecorders.Add(parent.FQID.ObjectId, loginSettings);
					loginSettings.TokenChangedEvent += TokenChangedEventHandler;
				}
			}
		}
		catch (Exception)
		{
			_lastError = 3;
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
}

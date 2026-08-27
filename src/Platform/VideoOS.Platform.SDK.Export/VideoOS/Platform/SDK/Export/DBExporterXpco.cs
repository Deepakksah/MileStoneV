using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Export.Resources;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Export;

public class DBExporterXpco : IDBExporter
{
	public class ExportJob
	{
		public Thread Thread;

		public bool ExportInProgress;
	}

	public class DbJob : ExportJob
	{
		public object SessionIdsLock = new object();

		public IEnumerable<object> SessionIds;

		public IEnumerable<object> SessionIdsConfirmed;

		public Item Item;

		public string PathForIniFile;
	}

	public class BookmarkJob : ExportJob
	{
		public int Progress;

		public string LastErrorMessage;

		public bool IsCancelled;

		public BookmarkExporter Exporter;
	}

	public class ItemComparer : IEqualityComparer<Item>
	{
		public bool Equals(Item x, Item y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			return x.FQID == y.FQID;
		}

		public int GetHashCode(Item item)
		{
			if (item == null)
			{
				return 0;
			}
			if (!(item.FQID == null))
			{
				return item.FQID.GetHashCode();
			}
			return 0;
		}
	}

	private int _lastError;

	private string _lastMessage = "";

	private List<Item> _cameraList = new List<Item>();

	private List<Item> _audioList = new List<Item>();

	private List<Item> _metadataList = new List<Item>();

	private bool _initializing;

	private bool _initLateUpdate;

	private object _lock = new object();

	private Dictionary<Guid, LoginSettings> _tokenEventForRecorders = new Dictionary<Guid, LoginSettings>();

	private List<ExportJob> _jobs = new List<ExportJob>();

	private const int Exception = 1;

	private const int Cancelled = 2;

	private const int ExceptionDuringTokenReg = 3;

	private int _itemDataInfoPairs;

	private Dictionary<object, Guid> _sessionIdToTrack = new Dictionary<object, Guid>();

	private ScpFileHandler _scpFileHandler;

	private bool _hasFinished;

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

	public override List<Item> MetadataList
	{
		get
		{
			return _metadataList;
		}
		set
		{
			_metadataList = ((value == null) ? new List<Item>() : value);
		}
	}

	public override bool Compression { get; set; }

	public override bool Encryption { get; set; }

	public override string Password { get; set; }

	public override string Path { get; set; }

	public override string[] ViewerPaths { get; set; }

	public override bool PreventReExport { get; set; }

	public override bool FailOnInvalidSignature { get; set; } = true;

	public override bool SignErrorsDetected { get; set; }

	public override int LastError => _lastError;

	public override string LastErrorString => _lastMessage;

	public override int Progress
	{
		get
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			lock (_lock)
			{
				foreach (ExportJob job in _jobs)
				{
					int num3 = 100;
					if (job is DbJob && job.ExportInProgress)
					{
						DbJob dbJob = (DbJob)job;
						int num4 = 0;
						lock (dbJob.SessionIdsLock)
						{
							int num5 = dbJob.SessionIds.Count();
							foreach (object sessionId in dbJob.SessionIds)
							{
								num4 += SDKInternalCommandService.ToolkitExportPercentCompleted(sessionId);
							}
							num3 = num4 / num5;
						}
					}
					else if (job is BookmarkJob)
					{
						num3 = ((BookmarkJob)job).Progress;
					}
					num += num3;
					flag |= job.ExportInProgress;
				}
				num2 = _jobs.Count;
			}
			int num6 = ((num2 != 0) ? (num / num2) : 0);
			if (num6 == 100 && flag)
			{
				num6 = 99;
			}
			return num6;
		}
	}

	public override void Init()
	{
		_jobs = new List<ExportJob>();
		_tokenEventForRecorders = new Dictionary<Guid, LoginSettings>();
		_hasFinished = false;
	}

	public override void Close()
	{
		EndExport();
	}

	public override void Cancel()
	{
		lock (_lock)
		{
			foreach (ExportJob job in _jobs)
			{
				if (job is DbJob)
				{
					List<object> list;
					lock (((DbJob)job).SessionIdsLock)
					{
						list = ((DbJob)job).SessionIds.ToList();
					}
					foreach (object item in list)
					{
						SDKInternalCommandService.ToolkitExportCancel(item);
					}
				}
				else if (job is BookmarkJob)
				{
					((BookmarkJob)job).IsCancelled = true;
				}
			}
		}
	}

	public override void EndExport()
	{
		bool flag;
		do
		{
			flag = false;
			lock (_lock)
			{
				foreach (ExportJob job in _jobs)
				{
					if (!job.ExportInProgress)
					{
						if (!(job is DbJob))
						{
							continue;
						}
						List<object> list;
						lock (((DbJob)job).SessionIdsLock)
						{
							list = ((DbJob)job).SessionIds.ToList();
						}
						foreach (object item2 in list)
						{
							((SDKInternalCommandService)InternalCommandService.Instance).ToolkitExportClose(item2);
						}
						list = ((DbJob)job).SessionIdsConfirmed.ToList();
						foreach (object item3 in list)
						{
							Item item = ((DbJob)job).Item;
							IStreamDevice streamDevice = GetStreamDevice(item);
							string text = item.FQID.ObjectId.ToString();
							Guid trackId = _sessionIdToTrack[item3];
							_scpFileHandler.EnsureSourceDefined(item, streamDevice, ".\\" + text, Password, trackId);
						}
					}
					else
					{
						flag = true;
					}
				}
				if (!flag)
				{
					_jobs.Clear();
				}
			}
			Thread.Sleep(500);
		}
		while (flag);
		if (!_hasFinished)
		{
			_scpFileHandler.SetupProjectFilePassword(Password, _itemDataInfoPairs);
			_scpFileHandler.Save(addTamperingKey: true);
			_hasFinished = true;
		}
		ReleaseEventHandlers();
	}

	public override bool StartExport(DateTime start, DateTime end)
	{
		_hasFinished = false;
		_lastError = 0;
		_lastMessage = string.Empty;
		SignErrorsDetected = false;
		if (!_cameraList.Any() && !_audioList.Any() && !_metadataList.Any())
		{
			return false;
		}
		if (Path == null)
		{
			throw new MIPException("Path must be filled");
		}
		if (!CheckExportAuthorization(_cameraList, aviExport: false) || !CheckExportAuthorization(_audioList, aviExport: false) || !CheckExportAuthorization(_metadataList, aviExport: false))
		{
			throw new NotAuthorizedMIPException("You are not authorized to do export for one or more of the provided devices");
		}
		try
		{
			if (!Directory.Exists(Path))
			{
				Directory.CreateDirectory(Path);
			}
			ItemComparer comparer = new ItemComparer();
			List<Item> list = new List<Item>(_cameraList.Distinct(comparer));
			list.AddRange(_audioList.Distinct(comparer));
			list.AddRange(_metadataList.Distinct(comparer));
			bool signExport = SignExport;
			bool reExportEnabled = !PreventReExport;
			string pathForIniFile = null;
			_scpFileHandler = new ScpFileHandler(Path);
			string text = System.IO.Path.Combine(Path, "Data");
			bool flag = text.StartsWith("\\\\");
			if (end > DateTime.UtcNow)
			{
				end = DateTime.UtcNow;
			}
			bool flag2 = false;
			bool flag3 = false;
			List<KeyValuePair<Item, MediaDataInspector.DataInformation>> list2 = new List<KeyValuePair<Item, MediaDataInspector.DataInformation>>();
			foreach (Item item in list)
			{
				MediaDataInspector.DataInformation dataInformation = MediaDataInspector.TrimStartAndEndTime(item.FQID, start, end);
				flag2 |= dataInformation.ContainsData;
				list2.Add(new KeyValuePair<Item, MediaDataInspector.DataInformation>(item, dataInformation));
				if (!dataInformation.RecorderOnline)
				{
					flag3 = true;
				}
			}
			if (!flag2 || flag3)
			{
				if (flag3)
				{
					throw new CommunicationMIPException("Recorder offline");
				}
				throw new NoVideoInTimeSpanMIPException("No video in selected time period");
			}
			List<FQID> list3 = new List<FQID>();
			_initializing = true;
			if (flag)
			{
				text = text.Remove(0, 2);
			}
			_itemDataInfoPairs = list2.Count;
			foreach (KeyValuePair<Item, MediaDataInspector.DataInformation> item2 in list2)
			{
				Item key = item2.Key;
				MediaDataInspector.DataInformation value = item2.Value;
				string path = key.FQID.ObjectId.ToString();
				string text2 = ((!flag) ? System.IO.Path.Combine("\\\\?\\" + text, path) : System.IO.Path.Combine("\\\\?\\UNC\\" + text, path));
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
					new DirectoryInfo(text2).Attributes |= FileAttributes.System;
					pathForIniFile = text2;
				}
				if (IncludeBookmarks && key.Authorization.TryGetValue("BookmarkView", out var value2) && string.Compare(value2, "Yes", ignoreCase: true) == 0)
				{
					list3.Add(key.FQID);
				}
				RegisterEventHandler(key);
				IEnumerable<Guid> tracks = GetTracks(GetStreamDevice(key));
				List<object> list4 = new List<object>();
				foreach (Guid item3 in tracks)
				{
					object obj = ((SDKInternalCommandService)InternalCommandService.Instance).ToolkitExportXpcoCreate(key, value.StartTime, value.EndTime, text2, EncryptionStrength, Password, signExport, PrivacyMask, reExportEnabled, item3, FailOnInvalidSignature);
					if (obj == null)
					{
						return false;
					}
					list4.Add(obj);
					_sessionIdToTrack.Add(obj, item3);
				}
				DbJob dbJob = new DbJob
				{
					SessionIds = list4,
					Item = key,
					ExportInProgress = false,
					PathForIniFile = pathForIniFile,
					SessionIdsConfirmed = new List<object>()
				};
				dbJob.Thread = new Thread(Run);
				dbJob.Thread.Name = "DBExport Thread - " + key.Name;
				dbJob.Thread.Start(dbJob);
				lock (_lock)
				{
					_jobs.Add(dbJob);
				}
			}
			if (IncludeBookmarks && list3.Any())
			{
				BookmarkJob bookmarkJob = new BookmarkJob
				{
					ExportInProgress = false,
					Exporter = new BookmarkExporter(list3, start, end, text)
				};
				bookmarkJob.Thread = new Thread(RunBookmarkExport);
				bookmarkJob.Thread.Name = "DBExport Bookmark Thread";
				bookmarkJob.Thread.Start(bookmarkJob);
				lock (_lock)
				{
					_jobs.Add(bookmarkJob);
				}
			}
			_initializing = false;
			if (_initLateUpdate)
			{
				_initLateUpdate = false;
				TokenChangedEventHandler(null, null);
			}
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
			EnvironmentManager.Instance.ExceptionHandler("DBExporterXpco:StartExport", ex3);
			_lastError = 1;
			_lastMessage = ex3.Message;
			return false;
		}
	}

	private static IStreamDevice GetStreamDevice(Item exportItem)
	{
		return ((((SDKConfiguration)exportItem.FQID.ServerId.UserContext.Configuration).FindSystemInfo(exportItem.FQID.ServerId) ?? throw new CommunicationMIPException("Recorder offline")).GetDevice(exportItem.FQID) as IStreamDevice) ?? throw new MIPException("Invalid device: " + exportItem.Name);
	}

	private static IEnumerable<Guid> GetTracks(IStreamDevice device)
	{
		ITrackInfo[] tracks = device.Tracks;
		if (tracks == null || !tracks.Any())
		{
			return new List<Guid> { device.Guid };
		}
		return tracks.Select((ITrackInfo t) => t.TrackId);
	}

	private void Run(object jobParm)
	{
		DbJob dbJob = (DbJob)jobParm;
		dbJob.ExportInProgress = true;
		List<object> list = new List<object>();
		try
		{
			List<object> list2;
			lock (dbJob.SessionIdsLock)
			{
				list2 = dbJob.SessionIds.ToList();
			}
			foreach (object item in list2)
			{
				switch (SDKInternalCommandService.ToolkitExportStart(item))
				{
				case 0:
					_lastError = 2;
					goto end_IL_0083;
				case 1:
					list.Add(item);
					break;
				case 3:
					SignErrorsDetected = true;
					break;
				}
				continue;
				end_IL_0083:
				break;
			}
			if (dbJob.PathForIniFile != null)
			{
				string path = System.IO.Path.Combine(dbJob.PathForIniFile, "desktop.ini");
				File.WriteAllText(path, $"[.ShellClassInfo]\r\nConfirmFileOp=0\r\nLocalizedResourceName={dbJob.Item.Name}\r\n\r\n", new UnicodeEncoding(bigEndian: false, byteOrderMark: true));
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				directoryInfo.Attributes |= FileAttributes.System;
				directoryInfo.Attributes |= FileAttributes.Hidden;
			}
		}
		catch (Exception ex)
		{
			_lastError = 1;
			AppendErrorString(string.Format(CultureInfo.CurrentUICulture, Strings.DBExporterError_ExportFailed, dbJob.Item.Name, ex.Message));
		}
		lock (dbJob.SessionIdsLock)
		{
			dbJob.SessionIdsConfirmed = dbJob.SessionIds.Except(list);
		}
		dbJob.ExportInProgress = false;
	}

	private void RunBookmarkExport(object jobParm)
	{
		BookmarkJob bookmarkJob = jobParm as BookmarkJob;
		bookmarkJob.Exporter.DoExport(bookmarkJob);
		if (bookmarkJob.Progress == -1)
		{
			_lastError = 1;
			AppendErrorString(bookmarkJob.LastErrorMessage);
		}
	}

	private void AppendErrorString(string errorString)
	{
		if (string.IsNullOrEmpty(_lastMessage))
		{
			_lastMessage = errorString;
		}
		else
		{
			_lastMessage = _lastMessage + System.Environment.NewLine + errorString;
		}
	}

	public static bool CheckExportAuthorization(IEnumerable<Item> itemsToCheck, bool aviExport)
	{
		if (aviExport)
		{
			return !itemsToCheck.Any((Item i) => !IsAuthorized(i.Authorization, "ExportAvi") && (i.Authorization.ContainsKey("ExportAvi") || !IsAuthorized(i.Authorization, "ExportDatabase")));
		}
		return !itemsToCheck.Any((Item i) => !IsAuthorized(i.Authorization, "ExportDatabase"));
	}

	private static bool IsAuthorized(Dictionary<string, string> authorizations, string optionName)
	{
		if (authorizations.TryGetValue(optionName, out var value))
		{
			switch (value.ToLowerInvariant())
			{
			case "yes":
			case "true":
			case "on":
				return true;
			}
		}
		return false;
	}

	private void TokenChangedEventHandler(object sender, EventArgs e)
	{
		lock (_lock)
		{
			if (!_initializing)
			{
				foreach (DbJob job in _jobs)
				{
					List<object> list;
					lock (job.SessionIdsLock)
					{
						list = job.SessionIds.ToList();
					}
					foreach (object item2 in list)
					{
						Item item = job.Item;
						Item parent = item.GetParent();
						if (parent != null)
						{
							LoginSettings loginSettings = item.FQID.ServerId.UserContext.GetLoginSettings(parent.FQID);
							if (loginSettings != null)
							{
								SDKInternalCommandService.ToolkitExportUpdateToken(item2, loginSettings.Token);
							}
						}
					}
				}
				return;
			}
			_initLateUpdate = true;
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
		catch (Exception ex)
		{
			_lastMessage = ex.Message;
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

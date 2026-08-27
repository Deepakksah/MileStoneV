using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VideoOS.AVIExporterClient;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Export.Resources;

namespace VideoOS.Platform.SDK.Export;

public class SDKWAVExporter : IWAVExporter
{
	private AVIExportClient _aviExportClient;

	private List<Item> _audioList = new List<Item>();

	private string _startupError;

	private bool _aviInUse;

	private bool _initializing;

	private bool _initLateUpdate;

	private object _lock = new object();

	private Dictionary<Guid, LoginSettings> _tokenEventForRecorders = new Dictionary<Guid, LoginSettings>();

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

	public override string[] CodecList
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			try
			{
				AVIExportClient val = new AVIExportClient();
				try
				{
					AVICodec[] possibleVideoCodecs = val.GetPossibleVideoCodecs(0u, 0u);
					string[] array = new string[possibleVideoCodecs.Length];
					for (int i = 0; i < possibleVideoCodecs.Length; i++)
					{
						array[i] = possibleVideoCodecs[i].Description;
					}
					return array;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch
			{
				EnvironmentManager.Instance.Log(error: true, "WAVExporter.CodecList", "Exception from Codecs");
				throw new MIPException("CodecList issue, does all codec run in current execution mode (32/64 bit)?");
			}
		}
	}

	public override int Progress
	{
		get
		{
			if (_aviExportClient != null)
			{
				return (int)_aviExportClient.QueryProgress();
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
			if (_aviExportClient == null)
			{
				return -1;
			}
			return (int)_aviExportClient.GetLastErrorCode();
		}
	}

	public override int LastDetailedError
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			if (_aviExportClient == null)
			{
				return -1;
			}
			return (int)_aviExportClient.GetLastDetailedError();
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
			if (_aviExportClient == null)
			{
				return "";
			}
			return _aviExportClient.GetLastErrorDescription() + ": " + ((object)_aviExportClient.GetLastDetailedError()/*cast due to constrained. prefix*/).ToString();
		}
	}

	public SDKWAVExporter()
	{
		Codec = "Intel IYUV codec";
	}

	public override void Init()
	{
		ManagedWrapper.minitialize();
	}

	public override void Close()
	{
		ManagedWrapper.mterminate();
	}

	public override void Cancel()
	{
		if (_aviExportClient != null)
		{
			_aviExportClient.Cancel();
		}
	}

	public override void EndExport()
	{
		lock (_lock)
		{
			ReleaseEventHandlers();
			if (_aviExportClient != null)
			{
				_aviExportClient.Cancel();
				int num = 1000;
				while (num-- > 0 && _aviInUse)
				{
					Thread.Sleep(25);
				}
				_aviExportClient.CloseAudioStream();
				_aviExportClient.Dispose();
			}
			_aviExportClient = null;
			_audioList = new List<Item>();
		}
	}

	public override bool StartExport(DateTime start, DateTime end)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		_startupError = string.Empty;
		if (!_audioList.Any())
		{
			SetError(Strings.AVIExporterError_NoExportSourcesSet);
			return false;
		}
		if (_aviExportClient != null)
		{
			SetError(Strings.AVIExporterError_ExportAlreadyRunning);
			return false;
		}
		if (!DBExporterXpco.CheckExportAuthorization(_audioList, aviExport: true))
		{
			SetError(Strings.AVIExporterError_NotAuthorized);
			return false;
		}
		_startupError = string.Empty;
		if (string.IsNullOrEmpty(FileName))
		{
			SetError(Strings.AVIExporterError_FilenameNotSet);
			return false;
		}
		_initializing = true;
		_aviExportClient = new AVIExportClient();
		AVICodec[] possibleVideoCodecs = _aviExportClient.GetPossibleVideoCodecs(0u, 0u);
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
			val = possibleVideoCodecs[0];
		}
		Codec = val.Description;
		DateTime dateTime = start;
		DateTime dateTime2 = end;
		foreach (Item audio in _audioList)
		{
			MediaDataInspector.DataInformation dataInformation = MediaDataInspector.TrimStartAndEndTime(audio.FQID, start, end);
			if (!dataInformation.RecorderOnline)
			{
				SetError(Strings.AVIExporterError_RecorderOffline);
				return false;
			}
			if (DateTime.Compare(dataInformation.StartTime, dateTime) < 0)
			{
				dateTime = dataInformation.StartTime;
			}
			if (DateTime.Compare(dataInformation.EndTime, dateTime2) > 0)
			{
				dateTime2 = dataInformation.EndTime;
			}
		}
		Tuple<DateTime, DateTime> tuple = new Tuple<DateTime, DateTime>(dateTime, dateTime2);
		List<AudioDeviceConfiguration> list = BuildAudioConfig();
		double[] array2 = new double[list.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = 1.0;
		}
		if (_aviExportClient.SetMediaType("audio/wav"))
		{
			_aviExportClient.OpenAudioStream(Channels, AudioSampleRate, AudioSampleDepth, (IEnumerable<AudioDeviceConfiguration>)list.ToArray(), (IEnumerable<double>)array2);
		}
		if (!FileName.ToLower().EndsWith(".wav"))
		{
			FileName += ".wav";
		}
		_aviInUse = true;
		bool flag;
		try
		{
			flag = _aviExportClient.StartExport(val, tuple.Item1, tuple.Item2, System.IO.Path.Combine(Path, FileName));
		}
		finally
		{
			_aviInUse = false;
		}
		ErrorCode lastErrorCode = _aviExportClient.GetLastErrorCode();
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

	private void TokenChangedEventHandler(object sender, EventArgs e)
	{
		lock (_lock)
		{
			if (_aviExportClient != null)
			{
				if (!_initializing)
				{
					List<AudioDeviceConfiguration> list = BuildAudioConfig();
					if (list.Count > 0)
					{
						_aviExportClient.UpdateAudioConnectionParameters((IEnumerable<AudioDeviceConfiguration>)list.ToArray());
					}
				}
			}
			else
			{
				_initLateUpdate = true;
			}
		}
	}

	private List<AudioDeviceConfiguration> BuildAudioConfig()
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
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
				UriBuilder uriBuilder = new UriBuilder(parent.FQID.ServerId.ServerScheme, parent.FQID.ServerId.ServerHostname, parent.FQID.ServerId.Serverport);
				list.Add(new AudioDeviceConfiguration(uriBuilder.Uri, audio.FQID.ObjectId, loginSettings.Token, (string)null, DateTime.MinValue, DateTime.MinValue, audio.Name, new UserRights(true, true)));
			}
		}
		return list;
	}

	private void ReleaseEventHandlers()
	{
		foreach (LoginSettings value in _tokenEventForRecorders.Values)
		{
			value.TokenChangedEvent -= TokenChangedEventHandler;
		}
		_tokenEventForRecorders.Clear();
	}

	private void SetError(string errorText)
	{
		_startupError = errorText;
	}
}

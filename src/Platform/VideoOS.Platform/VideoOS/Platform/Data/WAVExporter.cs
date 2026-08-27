using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class WAVExporter
{
	private IWAVExporter _wavExporter;

	public virtual List<Item> AudioList
	{
		get
		{
			return _wavExporter.AudioList;
		}
		set
		{
			_wavExporter.AudioList = value;
		}
	}

	public virtual string Path
	{
		get
		{
			return _wavExporter.Path;
		}
		set
		{
			_wavExporter.Path = value;
		}
	}

	public virtual bool ExportToDisk
	{
		get
		{
			return _wavExporter.ExportToDisk;
		}
		set
		{
			_wavExporter.ExportToDisk = value;
		}
	}

	public virtual bool ExportToMediaBurner
	{
		get
		{
			return _wavExporter.ExportToMediaBurner;
		}
		set
		{
			_wavExporter.ExportToMediaBurner = value;
		}
	}

	public virtual char MediaBurnerDriverLetter
	{
		get
		{
			return _wavExporter.MediaBurnerDriverLetter;
		}
		set
		{
			_wavExporter.MediaBurnerDriverLetter = value;
		}
	}

	public virtual string ExportName
	{
		get
		{
			return _wavExporter.ExportName;
		}
		set
		{
			_wavExporter.ExportName = value;
		}
	}

	public virtual string FileName
	{
		get
		{
			return _wavExporter.FileName;
		}
		set
		{
			_wavExporter.FileName = value;
		}
	}

	public virtual string Codec
	{
		get
		{
			return _wavExporter.Codec;
		}
		set
		{
			_wavExporter.Codec = value;
		}
	}

	public virtual string[] CodecList => _wavExporter.CodecList;

	public virtual int Channels
	{
		get
		{
			return _wavExporter.Channels;
		}
		set
		{
			_wavExporter.Channels = value;
		}
	}

	public virtual int AudioSampleRate
	{
		get
		{
			return _wavExporter.AudioSampleRate;
		}
		set
		{
			_wavExporter.AudioSampleRate = value;
		}
	}

	public virtual int AudioSampleDepth
	{
		get
		{
			return _wavExporter.AudioSampleDepth;
		}
		set
		{
			_wavExporter.AudioSampleDepth = value;
		}
	}

	public virtual int Progress => _wavExporter.Progress;

	public virtual int LastError => _wavExporter.LastError;

	public virtual int LastDetailedError => _wavExporter.LastDetailedError;

	public virtual string LastErrorString => _wavExporter.LastErrorString;

	public WAVExporter()
	{
		_wavExporter = InternalCommandService.Instance.CreateWAVExporter();
		_wavExporter.Channels = 1;
		_wavExporter.AudioSampleRate = 8000;
		_wavExporter.AudioSampleDepth = 16;
		new TrackPageViewData(typeof(WAVExporter)).Send();
	}

	public virtual void Init()
	{
		_wavExporter.Init();
	}

	public virtual void Close()
	{
		_wavExporter.Close();
	}

	public virtual bool StartExport(DateTime start, DateTime end)
	{
		return _wavExporter.StartExport(start, end);
	}

	public virtual void Cancel()
	{
		_wavExporter.Cancel();
	}

	public virtual void EndExport()
	{
		_wavExporter.EndExport();
	}
}

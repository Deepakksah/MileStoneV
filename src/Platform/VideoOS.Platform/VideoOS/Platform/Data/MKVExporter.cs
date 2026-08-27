using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class MKVExporter : IExporter
{
	private IMKVExporter _mkvExporter;

	[Obsolete("Temporary property for public use only.")]
	public bool UseOldMethod
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual List<Item> CameraList
	{
		get
		{
			return _mkvExporter.CameraList;
		}
		set
		{
			_mkvExporter.CameraList = value;
		}
	}

	public virtual List<Item> AudioList
	{
		get
		{
			return _mkvExporter.AudioList;
		}
		set
		{
			_mkvExporter.AudioList = value;
		}
	}

	public virtual string Path
	{
		get
		{
			return _mkvExporter.Path;
		}
		set
		{
			_mkvExporter.Path = value;
		}
	}

	public virtual string Filename
	{
		get
		{
			return _mkvExporter.Filename;
		}
		set
		{
			_mkvExporter.Filename = value;
		}
	}

	public virtual int Progress => _mkvExporter.Progress;

	public virtual int LastError => _mkvExporter.LastError;

	public virtual int LastDetailedError => 0;

	public virtual string LastErrorString => _mkvExporter.LastErrorString;

	public MKVExporter()
	{
		_mkvExporter = InternalCommandService.Instance.CreateMKVExporter();
		new TrackPageViewData(typeof(MKVExporter)).Send();
	}

	public virtual void Init()
	{
		_mkvExporter.Init();
	}

	public virtual void Close()
	{
		_mkvExporter.Close();
	}

	public virtual bool StartExport(DateTime start, DateTime end)
	{
		return _mkvExporter.StartExport(start, end);
	}

	public virtual void Cancel()
	{
		_mkvExporter.Cancel();
	}

	public virtual void EndExport()
	{
		_mkvExporter.EndExport();
	}

	public virtual void UseFallback()
	{
	}
}

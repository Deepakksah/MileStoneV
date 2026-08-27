using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class AVIExporter : IExporter
{
	public const int VerticalOverlayPositionTop = 0;

	public const int VerticalOverlayPositionCentered = 1;

	public const int VerticalOverlayPositionBottom = 2;

	public const int HorizontalOverlayPositionLeft = 0;

	public const int HorizontalOverlayPositionCentered = 1;

	public const int HorizontalOverlayPositionRight = 2;

	private IAVIExporter _aviExporter;

	private bool _telemetryDataSent;

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
			return _aviExporter.CameraList;
		}
		set
		{
			_aviExporter.CameraList = value;
		}
	}

	public virtual List<Item> AudioList
	{
		get
		{
			return _aviExporter.AudioList;
		}
		set
		{
			_aviExporter.AudioList = value;
		}
	}

	public virtual string Path
	{
		get
		{
			return _aviExporter.Path;
		}
		set
		{
			_aviExporter.Path = value;
		}
	}

	public virtual bool ExportToDisk
	{
		get
		{
			return _aviExporter.ExportToDisk;
		}
		set
		{
			_aviExporter.ExportToDisk = value;
		}
	}

	public virtual bool ExportToMediaBurner
	{
		get
		{
			return _aviExporter.ExportToMediaBurner;
		}
		set
		{
			_aviExporter.ExportToMediaBurner = value;
		}
	}

	public virtual char MediaBurnerDriverLetter
	{
		get
		{
			return _aviExporter.MediaBurnerDriverLetter;
		}
		set
		{
			_aviExporter.MediaBurnerDriverLetter = value;
		}
	}

	public virtual string ExportName
	{
		get
		{
			return _aviExporter.ExportName;
		}
		set
		{
			_aviExporter.ExportName = value;
		}
	}

	public virtual string Filename
	{
		get
		{
			return _aviExporter.Filename;
		}
		set
		{
			_aviExporter.Filename = value;
		}
	}

	public virtual bool Timestamp
	{
		get
		{
			return _aviExporter.Timestamp;
		}
		set
		{
			_aviExporter.Timestamp = value;
		}
	}

	public virtual string Codec
	{
		get
		{
			return _aviExporter.Codec;
		}
		set
		{
			_aviExporter.Codec = value;
		}
	}

	public virtual string[] CodecList => _aviExporter.CodecList;

	public virtual int FrameRate
	{
		get
		{
			return _aviExporter.FrameRate;
		}
		set
		{
			_aviExporter.FrameRate = value;
		}
	}

	public virtual int Width
	{
		get
		{
			return _aviExporter.Width;
		}
		set
		{
			_aviExporter.Width = value;
		}
	}

	public virtual int Height
	{
		get
		{
			return _aviExporter.Height;
		}
		set
		{
			_aviExporter.Height = value;
		}
	}

	public virtual int Channels
	{
		get
		{
			return _aviExporter.Channels;
		}
		set
		{
			_aviExporter.Channels = value;
		}
	}

	public virtual int AudioSampleRate
	{
		get
		{
			return _aviExporter.AudioSampleRate;
		}
		set
		{
			_aviExporter.AudioSampleRate = value;
		}
	}

	public virtual int AudioSampleDepth
	{
		get
		{
			return _aviExporter.AudioSampleDepth;
		}
		set
		{
			_aviExporter.AudioSampleDepth = value;
		}
	}

	public virtual Rectangle Window
	{
		get
		{
			return _aviExporter.Window;
		}
		set
		{
			_aviExporter.Window = value;
		}
	}

	public virtual bool AutoSplitExportFile
	{
		get
		{
			return _aviExporter.AutoSplitExportFile;
		}
		set
		{
			_aviExporter.AutoSplitExportFile = value;
		}
	}

	public virtual int MaxAVIFileSize
	{
		get
		{
			return _aviExporter.MaxAVIFileSize;
		}
		set
		{
			_aviExporter.MaxAVIFileSize = value;
		}
	}

	public virtual int Progress => _aviExporter.Progress;

	public virtual int LastError => _aviExporter.LastError;

	public virtual int LastDetailedError => _aviExporter.LastDetailedError;

	public virtual string LastErrorString => _aviExporter.LastErrorString;

	public AVIExporter()
	{
		_aviExporter = InternalCommandService.Instance.CreateAVIExporter();
	}

	public virtual void Init()
	{
		_aviExporter.Init();
	}

	public virtual void Close()
	{
		_aviExporter.Close();
	}

	public virtual bool SetOverlayImage(Bitmap overlayBitmap, int verticalOverlayPosition, int horizontalOverlayPosition, double scaleFactor, bool ignoreAspect)
	{
		return _aviExporter.SetOverlayImage(overlayBitmap, verticalOverlayPosition, horizontalOverlayPosition, scaleFactor, ignoreAspect);
	}

	public virtual bool StartExport(DateTime start, DateTime end)
	{
		if (!_telemetryDataSent)
		{
			new TrackPageViewData(typeof(AVIExporter)).Send();
			_telemetryDataSent = true;
		}
		return _aviExporter.StartExport(start, end);
	}

	public virtual bool StartExport(IEnumerable<SequenceAviExportElement> sequences)
	{
		if (!_telemetryDataSent)
		{
			new TrackPageViewData(typeof(AVIExporter)).Send();
			_telemetryDataSent = true;
		}
		return _aviExporter.StartExport(sequences);
	}

	public virtual void Cancel()
	{
		_aviExporter.Cancel();
	}

	public virtual void EndExport()
	{
		_aviExporter.EndExport();
	}
}

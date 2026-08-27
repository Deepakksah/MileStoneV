using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Platform.Data;

public abstract class IAVIExporter
{
	public abstract List<Item> CameraList { get; set; }

	public abstract List<Item> AudioList { get; set; }

	public abstract string Path { get; set; }

	public virtual bool ExportToDisk { get; set; }

	public virtual bool ExportToMediaBurner { get; set; }

	public virtual char MediaBurnerDriverLetter { get; set; }

	public virtual string ExportName { get; set; }

	public abstract string Filename { get; set; }

	public abstract bool Timestamp { get; set; }

	public abstract string Codec { get; set; }

	public virtual string[] CodecList => Array.Empty<string>();

	public abstract int FrameRate { get; set; }

	public abstract int Width { get; set; }

	public abstract int Height { get; set; }

	public abstract int Channels { get; set; }

	public abstract int AudioSampleRate { get; set; }

	public abstract int AudioSampleDepth { get; set; }

	public abstract Rectangle Window { get; set; }

	public virtual bool AutoSplitExportFile { get; set; }

	public virtual int MaxAVIFileSize { get; set; }

	public abstract int Progress { get; }

	public abstract int LastError { get; }

	public virtual int LastDetailedError => 0;

	public abstract string LastErrorString { get; }

	public abstract void Init();

	public abstract void Close();

	public virtual bool SetOverlayImage(Bitmap overlayBitmap, int verticalOverlayPosition, int horizontalOverlayPosition, double scaleFactor, bool ignoreAspect)
	{
		throw new NotImplementedException();
	}

	public abstract bool StartExport(DateTime start, DateTime end);

	public virtual bool StartExport(IEnumerable<SequenceAviExportElement> sequences)
	{
		throw new NotImplementedException();
	}

	public abstract void Cancel();

	public abstract void EndExport();
}

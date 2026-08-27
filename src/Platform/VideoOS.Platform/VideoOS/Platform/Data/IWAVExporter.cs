using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public abstract class IWAVExporter
{
	public virtual List<Item> AudioList { get; set; }

	public virtual string Path { get; set; }

	public virtual bool ExportToDisk { get; set; }

	public virtual bool ExportToMediaBurner { get; set; }

	public virtual char MediaBurnerDriverLetter { get; set; }

	public virtual string ExportName { get; set; }

	public virtual string FileName { get; set; }

	public virtual string Codec { get; set; }

	public virtual string[] CodecList => Array.Empty<string>();

	public virtual int Channels { get; set; }

	public virtual int AudioSampleRate { get; set; }

	public virtual int AudioSampleDepth { get; set; }

	public abstract int Progress { get; }

	public abstract int LastError { get; }

	public virtual int LastDetailedError => 0;

	public abstract string LastErrorString { get; }

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public abstract bool StartExport(DateTime start, DateTime end);

	public abstract void Cancel();

	public abstract void EndExport();
}

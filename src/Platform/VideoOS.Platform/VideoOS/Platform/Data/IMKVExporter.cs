using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public abstract class IMKVExporter
{
	public abstract List<Item> CameraList { get; set; }

	public abstract List<Item> AudioList { get; set; }

	public virtual bool ExportToDisk { get; set; }

	public virtual bool ExportToMediaBurner { get; set; }

	public virtual char MediaBurnerDriverLetter { get; set; }

	public abstract string Path { get; set; }

	public virtual string ExportName { get; set; }

	public abstract string Filename { get; set; }

	public abstract int Progress { get; }

	public abstract int LastError { get; }

	public abstract string LastErrorString { get; }

	public abstract void Init();

	public abstract void Close();

	public abstract bool StartExport(DateTime start, DateTime end);

	public abstract void Cancel();

	public abstract void EndExport();
}

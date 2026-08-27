using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public abstract class IDBExporter
{
	public abstract List<Item> CameraList { get; set; }

	public abstract List<Item> AudioList { get; set; }

	public abstract List<Item> MetadataList { get; set; }

	public abstract bool Compression { get; set; }

	public abstract bool Encryption { get; set; }

	public abstract string Password { get; set; }

	public abstract string[] ViewerPaths { get; set; }

	public abstract string Path { get; set; }

	public virtual bool ExportToDisk { get; set; }

	public virtual bool ExportToMediaBurner { get; set; }

	public virtual char MediaBurnerDriverLetter { get; set; }

	public virtual string ExportName { get; set; }

	public virtual EncryptionStrength EncryptionStrength { get; set; } = EncryptionStrength.AES256;

	public virtual bool IncludePlayer { get; set; }

	public virtual bool SignExport { get; set; }

	public virtual bool IncludeBookmarks { get; set; }

	public virtual string PrivacyMask { get; set; }

	public virtual bool PreventReExport { get; set; }

	public virtual bool FailOnInvalidSignature { get; set; }

	public virtual bool SignErrorsDetected { get; set; }

	public abstract int Progress { get; }

	public abstract int LastError { get; }

	public virtual int LastDetailedError => 0;

	public abstract string LastErrorString { get; }

	public virtual int LastWarning => 0;

	public abstract void Init();

	public abstract void Close();

	public abstract bool StartExport(DateTime start, DateTime end);

	public abstract void Cancel();

	public abstract void EndExport();
}

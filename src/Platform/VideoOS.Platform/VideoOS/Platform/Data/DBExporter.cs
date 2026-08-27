using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class DBExporter : IExporter
{
	private IDBExporter _dbExporter;

	public virtual List<Item> CameraList
	{
		get
		{
			return _dbExporter.CameraList;
		}
		set
		{
			_dbExporter.CameraList = value;
		}
	}

	public virtual List<Item> AudioList
	{
		get
		{
			return _dbExporter.AudioList;
		}
		set
		{
			_dbExporter.AudioList = value;
		}
	}

	public virtual List<Item> MetadataList
	{
		get
		{
			return _dbExporter.MetadataList;
		}
		set
		{
			_dbExporter.MetadataList = value;
		}
	}

	public virtual bool Compression
	{
		get
		{
			return _dbExporter.Compression;
		}
		set
		{
			_dbExporter.Compression = value;
		}
	}

	public virtual bool Encryption
	{
		get
		{
			return _dbExporter.Encryption;
		}
		set
		{
			_dbExporter.Encryption = value;
		}
	}

	public virtual string Password
	{
		get
		{
			return _dbExporter.Password;
		}
		set
		{
			_dbExporter.Password = value;
		}
	}

	public virtual string[] ViewerPaths
	{
		get
		{
			return _dbExporter.ViewerPaths;
		}
		set
		{
			_dbExporter.ViewerPaths = value;
		}
	}

	public virtual string Path
	{
		get
		{
			return _dbExporter.Path;
		}
		set
		{
			_dbExporter.Path = value;
		}
	}

	public virtual bool ExportToDisk
	{
		get
		{
			return _dbExporter.ExportToDisk;
		}
		set
		{
			_dbExporter.ExportToDisk = value;
		}
	}

	public virtual bool ExportToMediaBurner
	{
		get
		{
			return _dbExporter.ExportToMediaBurner;
		}
		set
		{
			_dbExporter.ExportToMediaBurner = value;
		}
	}

	public virtual char MediaBurnerDriverLetter
	{
		get
		{
			return _dbExporter.MediaBurnerDriverLetter;
		}
		set
		{
			_dbExporter.MediaBurnerDriverLetter = value;
		}
	}

	public virtual string ExportName
	{
		get
		{
			return _dbExporter.ExportName;
		}
		set
		{
			_dbExporter.ExportName = value;
		}
	}

	public virtual EncryptionStrength EncryptionStrength
	{
		get
		{
			return _dbExporter.EncryptionStrength;
		}
		set
		{
			_dbExporter.EncryptionStrength = value;
		}
	}

	public virtual bool IncludePlayer
	{
		get
		{
			return _dbExporter.IncludePlayer;
		}
		set
		{
			_dbExporter.IncludePlayer = value;
		}
	}

	public virtual bool SignExport
	{
		get
		{
			return _dbExporter.SignExport;
		}
		set
		{
			_dbExporter.SignExport = value;
		}
	}

	public virtual bool IncludeBookmarks
	{
		get
		{
			return _dbExporter.IncludeBookmarks;
		}
		set
		{
			_dbExporter.IncludeBookmarks = value;
		}
	}

	public virtual string PrivacyMask
	{
		get
		{
			return _dbExporter.PrivacyMask;
		}
		set
		{
			_dbExporter.PrivacyMask = value;
		}
	}

	public virtual bool PreventReExport
	{
		get
		{
			return _dbExporter.PreventReExport;
		}
		set
		{
			_dbExporter.PreventReExport = value;
		}
	}

	public virtual bool FailOnInvalidSignature
	{
		get
		{
			return _dbExporter.FailOnInvalidSignature;
		}
		set
		{
			_dbExporter.FailOnInvalidSignature = value;
		}
	}

	public virtual bool SignErrorsDetected => _dbExporter.SignErrorsDetected;

	public virtual int Progress => _dbExporter.Progress;

	public virtual int LastError => _dbExporter.LastError;

	public virtual int LastDetailedError => _dbExporter.LastDetailedError;

	public virtual string LastErrorString => _dbExporter.LastErrorString;

	public virtual int LastWarning
	{
		get
		{
			if (_dbExporter != null)
			{
				return _dbExporter.LastWarning;
			}
			return 0;
		}
	}

	public DBExporter()
		: this(blkFormat: false)
	{
	}

	public DBExporter(bool blkFormat)
	{
		_dbExporter = InternalCommandService.Instance.CreateDBExporter(blkFormat: true);
		new TrackPageViewData(typeof(DBExporter)).Send();
	}

	public virtual void Init()
	{
		_dbExporter.Init();
	}

	public virtual void Close()
	{
		_dbExporter.Close();
	}

	public virtual bool StartExport(DateTime start, DateTime end)
	{
		return _dbExporter.StartExport(start, end);
	}

	public virtual void Cancel()
	{
		_dbExporter.Cancel();
	}

	public virtual void EndExport()
	{
		_dbExporter.EndExport();
	}
}

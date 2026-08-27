using System;
using System.IO;

namespace VideoOS.Mobile.Http.FileSystem;

public class DriveFile : BasicFile
{
	private readonly string _filePath;

	private readonly bool _download;

	private readonly bool _noDownload;

	public const string DownloadExtension = ".download";

	public const string NoDownloadExtension = ".noDownload";

	private readonly bool _allowXframe;

	public const string AllowXframeExtension = ".allowXframe";

	public readonly DateTime _lastModified;

	private FileStream _stream;

	public override Stream Stream
	{
		get
		{
			if (_stream == null)
			{
				_stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			return _stream;
		}
	}

	public override bool Download => _download;

	public override bool NoDownload => _noDownload;

	public override bool AllowXframe => _allowXframe;

	public override DateTime LastModified => _lastModified;

	public DriveFile(string name, string path)
		: base(name)
	{
		_filePath = path;
		_download = File.Exists(path + ".download");
		_noDownload = File.Exists(path + ".noDownload");
		_allowXframe = File.Exists(path + ".allowXframe");
		_lastModified = File.GetLastWriteTime(path);
		_stream = null;
	}
}

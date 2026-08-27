using System;
using System.IO;
using System.Threading;

namespace VideoOS.Installer.Common.Utilities;

public class DirectoryPropergationWatcher : IDisposable
{
	private Timer _timer;

	private int _watchTime;

	private string _path;

	private long _maxSize;

	public event DirectoryPropergationDelegate OnProgress;

	public DirectoryPropergationWatcher(string path, long maxSize, int watchTime)
	{
		_path = path;
		_watchTime = watchTime;
		_maxSize = maxSize;
	}

	public void Start()
	{
		_timer = new Timer(Directory_Changed, this, 0, _watchTime);
	}

	public void Stop()
	{
		if (_timer != null)
		{
			_timer.Dispose();
			_timer = null;
		}
	}

	public void Dispose()
	{
		Stop();
	}

	public long DirSize(DirectoryInfo d)
	{
		long num = 0L;
		try
		{
			FileInfo[] files = d.GetFiles();
			if (files == null)
			{
				return 0L;
			}
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				try
				{
					num += fileInfo.Length;
				}
				catch (FileNotFoundException)
				{
				}
			}
			DirectoryInfo[] directories = d.GetDirectories();
			foreach (DirectoryInfo d2 in directories)
			{
				num += DirSize(d2);
			}
		}
		catch (DirectoryNotFoundException)
		{
		}
		return num;
	}

	private void Directory_Changed(object sender)
	{
		if (this.OnProgress != null && !string.IsNullOrEmpty(_path))
		{
			DirectoryInfo d = new DirectoryInfo(_path);
			this.OnProgress(DirSize(d), _maxSize);
		}
	}
}

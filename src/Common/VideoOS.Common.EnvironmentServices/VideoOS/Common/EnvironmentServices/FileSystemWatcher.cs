using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
[ExcludeFromCodeCoverage]
public class FileSystemWatcher : System.IO.FileSystemWatcher, IFileSystemWatcher, IDisposable
{
	public FileSystemWatcher()
	{
	}

	public FileSystemWatcher(string path)
		: base(path)
	{
	}

	public FileSystemWatcher(string path, string filter)
		: base(path, filter)
	{
	}

	WaitForChangedResult IFileSystemWatcher.WaitForChanged(WatcherChangeTypes changeType)
	{
		return WaitForChanged(changeType);
	}

	WaitForChangedResult IFileSystemWatcher.WaitForChanged(WatcherChangeTypes changeType, int timeout)
	{
		return WaitForChanged(changeType, timeout);
	}
}

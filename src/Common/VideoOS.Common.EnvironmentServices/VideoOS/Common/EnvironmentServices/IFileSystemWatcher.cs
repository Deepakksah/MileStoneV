using System;
using System.IO;

namespace VideoOS.Common.EnvironmentServices;

public interface IFileSystemWatcher : IDisposable
{
	bool EnableRaisingEvents { get; set; }

	string Filter { get; set; }

	NotifyFilters NotifyFilter { get; set; }

	bool IncludeSubdirectories { get; set; }

	int InternalBufferSize { get; set; }

	string Path { get; set; }

	event FileSystemEventHandler Created;

	event FileSystemEventHandler Changed;

	event FileSystemEventHandler Deleted;

	event RenamedEventHandler Renamed;

	event ErrorEventHandler Error;

	WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);

	WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);
}

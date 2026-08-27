using System;

namespace VideoOS.Management.Common;

public class RecordingServerDiskUsage
{
	public string Path { get; public set; }

	public long TotalSize { get; public set; }

	public long MediaData { get; public set; }

	public long OtherData { get; public set; }

	public long Free { get; public set; }

	public DateTimeOffset LastUpdated { get; public set; }
}

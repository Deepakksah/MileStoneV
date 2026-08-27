using System;

namespace VideoOS.Management.Server;

public class ArchiveStorageDataContract
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Path { get; set; }

	public bool IsDefault { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public int RetainMinutes { get; set; }

	public long MaxSize { get; set; }

	public bool FramerateReductionEnabled { get; set; }

	public double TargetFramerate { get; set; }

	public string ArchiveSchedule { get; set; }

	public bool Mounted { get; set; }

	public string RecordingStorageId { get; set; }

	public string RecordingServerId { get; set; }

	public CustomProperty[] CustomProperties { get; set; }
}

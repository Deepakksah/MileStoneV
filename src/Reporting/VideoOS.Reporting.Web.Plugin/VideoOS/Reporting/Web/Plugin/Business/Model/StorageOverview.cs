using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class StorageOverview
{
	public string StorageId { get; set; }

	public string ArchiveId { get; set; }

	public string Name { get; set; }

	public string Path { get; set; }

	public bool IsDefault { get; set; }

	public float Size { get; set; }

	public Unit SizeUnit { get; set; }

	public long SizeRaw { get; set; }

	public int RetainDays { get; set; }

	public int RecordingRetain { get; set; }

	public float Used { get; set; }

	public Unit UsedUnit { get; set; }

	public int MaxRetain { get; set; }

	public int DiskUsageInstanceId { get; set; }

	public int UsedPercent { get; set; }

	public bool IsLast { get; set; }
}

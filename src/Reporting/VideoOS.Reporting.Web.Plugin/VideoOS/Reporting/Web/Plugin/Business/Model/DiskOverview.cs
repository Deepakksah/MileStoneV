using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DiskOverview
{
	public string DiskName { get; set; }

	public DiskCounter MediaDataUsed { get; set; }

	public DiskCounter OtherDataUsed { get; set; }

	public DiskCounter FreeData { get; set; }

	public float TotalDataSize { get; set; }

	public Unit TotalDataSizeUnit { get; set; }

	public long TotalSizeInBytes { get; set; }

	public int DiskWriteSpeedInstanceId { get; set; }

	public string DiskWriteSpeedInstanceName { get; set; }

	public int DiskReadSpeedInstanceId { get; set; }

	public string DiskReadSpeedInstanceName { get; set; }
}

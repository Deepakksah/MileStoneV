using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DiskCounter
{
	public float Value { get; set; }

	public Unit Unit { get; set; }

	public int InstanceId { get; set; }

	public long Raw { get; set; }
}

using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DeviceStatusResult
{
	public DeviceStatus[] DeviceStatus { get; set; }

	public float TotalUsedSpace { get; set; }

	public Unit TotalUsedSpaceUnit { get; set; }
}

using System;

namespace VideoOS.Management.Server;

public class BookmarkSearchParametersDeviceFilter : SearchFilter
{
	public string[] DeviceIds { get; set; }

	public DateTime CenterTime { get; set; }

	public TimeSpan TimeLimit { get; set; }

	public int CountLimit { get; set; }

	public string FromBookmarkId { get; set; }
}

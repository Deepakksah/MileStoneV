using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class BookmarkSearchParametersNoDeviceFilter : SearchFilter
{
	public DeviceType[] DeviceTypes { get; set; }

	public DateTime CenterTime { get; set; }

	public TimeSpan TimeLimit { get; set; }

	public int CountLimit { get; set; }

	public string FromBookmarkId { get; set; }
}

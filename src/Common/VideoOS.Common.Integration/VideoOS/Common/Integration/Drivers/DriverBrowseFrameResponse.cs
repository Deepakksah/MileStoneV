using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Drivers;

public class DriverBrowseFrameResponse
{
	public ReadOnlySharedBuffer Data { get; private set; }

	public DateTime Time { get; private set; }

	public bool? Motion { get; private set; }

	public DriverBrowseFrameResponse(ReadOnlySharedBuffer data, DateTime time, bool? motion)
	{
		Data = data;
		Time = time;
		Motion = motion;
	}
}

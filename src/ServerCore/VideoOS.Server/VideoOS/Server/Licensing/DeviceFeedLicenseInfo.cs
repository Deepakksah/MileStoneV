using System;

namespace VideoOS.Server.Licensing;

public sealed class DeviceFeedLicenseInfo
{
	public string FeedType { get; private set; }

	public int Licensed { get; set; }

	public int Temporary { get; set; }

	public int Expired { get; set; }

	public int Unlicensed { get; set; }

	public DateTime FirstGraceExpiryUtc { get; set; }

	public DeviceFeedLicenseInfo(string feedType)
	{
		FeedType = feedType;
		FirstGraceExpiryUtc = DateTime.MaxValue;
	}
}

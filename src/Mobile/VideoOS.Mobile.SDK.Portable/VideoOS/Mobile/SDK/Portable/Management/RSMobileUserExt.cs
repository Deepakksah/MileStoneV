using System;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class RSMobileUserExt
{
	public Guid UserId { get; set; }

	public string UserName { get; set; }

	public int LivesCount { get; set; }

	public int PlaybacksCount { get; set; }

	public int UploadsCount { get; set; }

	public long InBandKB { get; set; }

	public long OutBandKB { get; set; }

	public RSMobileUserExt()
	{
		Initialize();
	}

	public RSMobileUserExt(MobileUserExt mobileUser)
	{
		Initialize();
		UserId = mobileUser.UserId;
		UserName = mobileUser.UserName;
	}

	public void Initialize()
	{
		LivesCount = 0;
		PlaybacksCount = 0;
		UploadsCount = 0;
		InBandKB = 0L;
		OutBandKB = 0L;
	}
}

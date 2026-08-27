using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

[Flags]
public enum LiveFlags
{
	Nothing = 0,
	LiveFeed = 1,
	Motion = 2,
	Recording = 4,
	Notification = 8,
	CameraConnectionLost = 0x10,
	DatabaseFail = 0x20,
	DiskFull = 0x40,
	ClientLiveStopped = 0x80
}

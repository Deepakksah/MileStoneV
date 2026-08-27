using System;

namespace VideoOS.Platform.Live;

[Flags]
public enum StatusFlags
{
	None = 0,
	LiveFeed = 1,
	Motion = 2,
	Recording = 4,
	Notification = 8,
	CameraConnectionLost = 0x10,
	DatabaseFail = 0x20,
	DiskFull = 0x40,
	ClientLiveStopped = 0x80,
	All = 0xFF
}

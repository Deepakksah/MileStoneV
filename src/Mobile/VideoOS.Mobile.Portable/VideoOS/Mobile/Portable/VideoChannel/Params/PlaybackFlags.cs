using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

[Flags]
public enum PlaybackFlags
{
	Nothing = 0,
	PlayStopped = 1,
	PlayForward = 2,
	PlayBackward = 4,
	PlayMask = 7,
	DatabaseStart = 0x10,
	DatabaseEnd = 0x20,
	DatabaseError = 0x40,
	DatabaseMask = 0x70,
	RangeNoData = 0x100,
	OutOfRange = 0x200
}

using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class NotificationSourceTypesHelper
{
	[Flags]
	public enum NotificationSourceType : uint
	{
		None = 0u,
		Alarm = 1u,
		Event = 2u,
		AlarmAssignment = 4u,
		CustomEvent = 8u,
		AccessRequest = 0x10u,
		Default = 1u
	}

	public static void SetFlag(ref uint flags, NotificationSourceType notificationSourceType)
	{
		flags |= (uint)notificationSourceType;
	}

	public static bool IsSet(uint flags, NotificationSourceType notificationSourceType)
	{
		return (flags & (uint)notificationSourceType) != 0;
	}
}

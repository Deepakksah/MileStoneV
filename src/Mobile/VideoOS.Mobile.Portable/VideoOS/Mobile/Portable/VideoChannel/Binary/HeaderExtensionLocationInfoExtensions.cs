namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public static class HeaderExtensionLocationInfoExtensions
{
	public static bool CheckForMask(ref HeaderExtensionLocationInfo header, BinaryFrameHeaderHelper.LocationMembersFlags flags)
	{
		return (header.Fields & (uint)flags) == (uint)flags;
	}

	public static void SetMask(ref HeaderExtensionLocationInfo header, BinaryFrameHeaderHelper.LocationMembersFlags flags)
	{
		header.Fields |= (uint)flags;
	}
}

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public static class HeaderExtensionStreamInfoExtensions
{
	public static bool CheckForMask(ref HeaderExtensionStreamInfo header, BinaryFrameHeaderHelper.StreamInfoFieldsFlags flags)
	{
		return (header.Fields & (uint)flags) == (uint)flags;
	}

	public static void SetMask(ref HeaderExtensionStreamInfo header, BinaryFrameHeaderHelper.StreamInfoFieldsFlags flags)
	{
		header.Fields |= (uint)flags;
	}
}

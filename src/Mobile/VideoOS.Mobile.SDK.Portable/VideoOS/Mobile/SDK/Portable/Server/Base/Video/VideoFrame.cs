using VideoOS.Mobile.Portable.VideoChannel.Binary;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class VideoFrame
{
	public MainHeaderExt MainHeader;

	public HeaderExtensionSize HeaderExtensionSize;

	public HeaderExtensionLiveEvents HeaderExtensionLiveEvents;

	public HeaderExtensionPlaybackEvents HeaderExtensionPlaybackEvents;

	public HeaderExtensionPlaybackInfo HeaderExtensionPlaybackInfo;

	public HeaderExtensionNative HeaderExtensionNative;

	public HeaderExtensionNativePlaybackSync HeaderExtensionNativePlaybackSync;

	public HeaderExtensionNativePlaybackMotionSeq HeaderExtensionNativePlaybackMotionSeq;

	public HeaderExtensionNativeString HeaderExtensionNativeSignature;

	public HeaderExtensionNativeString HeaderExtensionNativePrivacyMask;

	public HeaderExtensionStreamInfo HeaderExtensionStreamInfo;

	public HeaderExtensionCarouselInfo HeaderExtensionCarouselInfo;

	public HeaderExtensionLocationInfo HeaderExtensionLocationInfo;

	public byte[] Data;

	public bool ExtensionPresence(BinaryFrameHeaderHelper.HeaderExtensionFlags checkFlag)
	{
		return FrameExtractor.CheckExtensionPresence(MainHeader.HeaderExtensionFlags, checkFlag);
	}

	public bool NativePresence(BinaryFrameHeaderHelper.NativeHeaderExtensionFlags nativeFlag)
	{
		return FrameExtractor.CheckExtensionPresence(MainHeader.HeaderExtensionFlags, BinaryFrameHeaderHelper.HeaderExtensionFlags.NativeData) & FrameExtractor.CheckNativePresence(HeaderExtensionNative.NativeDataExtensionFlags, nativeFlag);
	}
}

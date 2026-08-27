using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public static class BinaryFrameInvertor
{
	public static void Invert(ref MainHeader header)
	{
		header.VideoID = EndiansInvertor.Invert(header.VideoID);
		header.TimeStampUtcMs = EndiansInvertor.Invert(header.TimeStampUtcMs);
		header.FrameCount = EndiansInvertor.Invert(header.FrameCount);
		header.FrameSizeBytes = EndiansInvertor.Invert(header.FrameSizeBytes);
		header.HeaderSizeBytes = EndiansInvertor.Invert(header.HeaderSizeBytes);
		header.HeaderExtensionFlags = EndiansInvertor.Invert(header.HeaderExtensionFlags);
	}

	public static void Invert(ref MainHeaderExt header)
	{
		header.VideoID = EndiansInvertor.Invert(header.VideoID);
		header.TimeStampUtcMs = EndiansInvertor.Invert(header.TimeStampUtcMs);
		header.FrameCount = EndiansInvertor.Invert(header.FrameCount);
		header.FrameSizeBytes = EndiansInvertor.Invert(header.FrameSizeBytes);
		header.HeaderVersion = EndiansInvertor.Invert(header.HeaderVersion);
		header.HeaderExtensionFlags = EndiansInvertor.Invert(header.HeaderExtensionFlags);
		header.HeaderSizeBytes = EndiansInvertor.Invert(header.HeaderSizeBytes);
	}

	public static void Invert(ref HeaderExtensionSize extensionSize)
	{
		extensionSize.SrcStrideX = EndiansInvertor.Invert(extensionSize.SrcStrideX);
		extensionSize.SrcStrideY = EndiansInvertor.Invert(extensionSize.SrcStrideY);
		extensionSize.SrcLeft = EndiansInvertor.Invert(extensionSize.SrcLeft);
		extensionSize.SrcTop = EndiansInvertor.Invert(extensionSize.SrcTop);
		extensionSize.SrcRight = EndiansInvertor.Invert(extensionSize.SrcRight);
		extensionSize.SrcBottom = EndiansInvertor.Invert(extensionSize.SrcBottom);
		extensionSize.DestStrideX = EndiansInvertor.Invert(extensionSize.DestStrideX);
		extensionSize.DestStrideY = EndiansInvertor.Invert(extensionSize.DestStrideY);
		extensionSize.ResamplingTag = EndiansInvertor.Invert(extensionSize.ResamplingTag);
		extensionSize.Reserved0 = EndiansInvertor.Invert(extensionSize.Reserved0);
		extensionSize.Reserved1 = EndiansInvertor.Invert(extensionSize.Reserved1);
		extensionSize.Reserved2 = EndiansInvertor.Invert(extensionSize.Reserved2);
	}

	public static void Invert(ref HeaderExtensionLiveEvents extensionLive)
	{
		extensionLive.CurrentFlags = EndiansInvertor.Invert(extensionLive.CurrentFlags);
		extensionLive.ChangedFlags = EndiansInvertor.Invert(extensionLive.ChangedFlags);
	}

	public static void Invert(ref HeaderExtensionPlaybackEvents extensionPlayback)
	{
		extensionPlayback.CurrentFlags = EndiansInvertor.Invert(extensionPlayback.CurrentFlags);
		extensionPlayback.ChangedFlags = EndiansInvertor.Invert(extensionPlayback.ChangedFlags);
	}

	public static void Invert(ref HeaderExtensionPlaybackInfo extensionPlaybackInfo)
	{
		extensionPlaybackInfo.RequestedTimeStampUtcMs = EndiansInvertor.Invert(extensionPlaybackInfo.RequestedTimeStampUtcMs);
		extensionPlaybackInfo.Reserved0 = EndiansInvertor.Invert(extensionPlaybackInfo.Reserved0);
		extensionPlaybackInfo.Reserved1 = EndiansInvertor.Invert(extensionPlaybackInfo.Reserved1);
	}

	public static void Invert(ref HeaderExtensionCarouselInfo extensionCarouselInfo)
	{
		extensionCarouselInfo.HeaderSize = EndiansInvertor.Invert(extensionCarouselInfo.HeaderSize);
		extensionCarouselInfo.Version = EndiansInvertor.Invert(extensionCarouselInfo.Version);
		extensionCarouselInfo.ItemID = EndiansInvertor.Invert(extensionCarouselInfo.ItemID);
	}

	public static void Invert(ref HeaderExtensionNative extensionNative)
	{
		extensionNative.NativeDataHeaderSize = EndiansInvertor.Invert(extensionNative.NativeDataHeaderSize);
		extensionNative.NativeDataExtensionFlags = EndiansInvertor.Invert(extensionNative.NativeDataExtensionFlags);
	}

	public static void Invert(ref HeaderExtensionNativePlaybackSync extPlaybackSync)
	{
		extPlaybackSync.PrevousSyncTimeStamp = EndiansInvertor.Invert(extPlaybackSync.PrevousSyncTimeStamp);
		extPlaybackSync.NextSyncTimeStamp = EndiansInvertor.Invert(extPlaybackSync.NextSyncTimeStamp);
	}

	public static void Invert(ref HeaderExtensionNativePlaybackMotionSeq extPlaybackMotionSeq)
	{
		extPlaybackMotionSeq.SequenceId = EndiansInvertor.Invert(extPlaybackMotionSeq.SequenceId);
		extPlaybackMotionSeq.HasMotion = EndiansInvertor.Invert(extPlaybackMotionSeq.HasMotion);
	}

	public static void Invert(ref HeaderExtensionNativeString extString)
	{
		extString.State = EndiansInvertor.Invert(extString.State);
		extString.Length = EndiansInvertor.Invert(extString.Length);
	}

	public static void Invert(ref HeaderExtensionLocationInfo headerLocationInfo)
	{
		headerLocationInfo.Length = EndiansInvertor.Invert(headerLocationInfo.Length);
		headerLocationInfo.Version = EndiansInvertor.Invert(headerLocationInfo.Version);
		headerLocationInfo.Source = EndiansInvertor.Invert(headerLocationInfo.Source);
		headerLocationInfo.Reserved = EndiansInvertor.Invert(headerLocationInfo.Reserved);
		headerLocationInfo.Fields = EndiansInvertor.Invert(headerLocationInfo.Fields);
		headerLocationInfo.Longitude = EndiansInvertor.Invert(headerLocationInfo.Longitude);
		headerLocationInfo.Latitude = EndiansInvertor.Invert(headerLocationInfo.Latitude);
		headerLocationInfo.Altitude = EndiansInvertor.Invert(headerLocationInfo.Altitude);
		headerLocationInfo.HorizontalAccuracy = EndiansInvertor.Invert(headerLocationInfo.HorizontalAccuracy);
		headerLocationInfo.VerticalAccuracy = EndiansInvertor.Invert(headerLocationInfo.VerticalAccuracy);
		headerLocationInfo.Speed = EndiansInvertor.Invert(headerLocationInfo.Speed);
		headerLocationInfo.Azimuth = EndiansInvertor.Invert(headerLocationInfo.Azimuth);
	}

	public static void Invert(ref HeaderExtensionStreamInfo extensionStreamInfo)
	{
		extensionStreamInfo.HeaderSize = EndiansInvertor.Invert(extensionStreamInfo.HeaderSize);
		extensionStreamInfo.Version = EndiansInvertor.Invert(extensionStreamInfo.Version);
		extensionStreamInfo.Reserved = EndiansInvertor.Invert(extensionStreamInfo.Reserved);
		extensionStreamInfo.Fields = EndiansInvertor.Invert(extensionStreamInfo.Fields);
		extensionStreamInfo.FramesCount = EndiansInvertor.Invert(extensionStreamInfo.FramesCount);
		extensionStreamInfo.TimeBetweenFrames = EndiansInvertor.Invert(extensionStreamInfo.TimeBetweenFrames);
		extensionStreamInfo.DataType = EndiansInvertor.Invert(extensionStreamInfo.DataType);
		extensionStreamInfo.Flags = EndiansInvertor.Invert(extensionStreamInfo.Flags);
		extensionStreamInfo.Profile = EndiansInvertor.Invert(extensionStreamInfo.Profile);
		extensionStreamInfo.Level = EndiansInvertor.Invert(extensionStreamInfo.Level);
		extensionStreamInfo.Constrains = EndiansInvertor.Invert(extensionStreamInfo.Constrains);
		extensionStreamInfo.Compatibility = EndiansInvertor.Invert(extensionStreamInfo.Compatibility);
	}

	public static void Invert(ref HeaderExtensionDynamicInfo extensionStreamInfo)
	{
		extensionStreamInfo.DataType = EndiansInvertor.Invert(extensionStreamInfo.DataType);
		extensionStreamInfo.DataCount = EndiansInvertor.Invert(extensionStreamInfo.DataCount);
		extensionStreamInfo.HeaderSize = EndiansInvertor.Invert(extensionStreamInfo.HeaderSize);
		extensionStreamInfo.Version = EndiansInvertor.Invert(extensionStreamInfo.Version);
	}

	public static void Invert(ref HeaderTypeDeviceStateInfo extensionStreamInfo)
	{
		extensionStreamInfo.ErrorCode = EndiansInvertor.Invert(extensionStreamInfo.ErrorCode);
		extensionStreamInfo.ItemID = EndiansInvertor.Invert(extensionStreamInfo.ItemID);
	}
}

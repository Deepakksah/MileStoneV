using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class FrameExtractor
{
	public static MainHeaderExt ExtractMainHeader(ByteBuffer buffer, bool networkByteOrder = false)
	{
		MainHeaderExt obj = default(MainHeaderExt);
		BinaryFrameHeaderHelper.Instance.ByteArrToStructure(buffer.data, buffer.begin, ref obj, 40);
		if (networkByteOrder)
		{
			BinaryFrameInvertor.Invert(ref obj);
		}
		if (obj.HeaderVersion >= 36)
		{
			obj.HeaderSizeBytes = obj.HeaderVersion;
		}
		return obj;
	}

	public static bool ExtractFrame(ByteBuffer buffer, OnVideoFrame newFrame, bool networkByteOrder = false)
	{
		MainHeaderExt tHeader = ExtractMainHeader(buffer, networkByteOrder);
		return ExtractFrame(buffer, newFrame, tHeader, networkByteOrder);
	}

	public static bool ExtractFrame(ByteBuffer buffer, OnVideoFrame newFrame, MainHeaderExt tHeader, bool networkByteOrder = false)
	{
		bool flag = true;
		try
		{
			HeadersExtractor headersExtractor = ExtractHeader(buffer, tHeader, networkByteOrder);
			flag = headersExtractor.CheckSizes();
			VideoFrame videoFrame = new VideoFrame
			{
				MainHeader = tHeader,
				HeaderExtensionSize = headersExtractor.HeaderExtensionSize,
				HeaderExtensionLiveEvents = headersExtractor.HeaderExtensionLives,
				HeaderExtensionPlaybackEvents = headersExtractor.HeaderExtensionPlaybacks,
				HeaderExtensionPlaybackInfo = headersExtractor.HeaderExtensionPlaybackInfo,
				HeaderExtensionNative = headersExtractor.HeaderExtensionNative,
				HeaderExtensionStreamInfo = headersExtractor.StreamInfo,
				HeaderExtensionCarouselInfo = headersExtractor.CarouselInfo,
				HeaderExtensionLocationInfo = headersExtractor.LocationInfoInfo,
				Data = new byte[tHeader.FrameSizeBytes]
			};
			Array.Copy(buffer.data, buffer.begin + (int)tHeader.HeaderSizeBytes, videoFrame.Data, 0, (int)tHeader.FrameSizeBytes);
			newFrame?.Invoke(videoFrame);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "FrameExtractor.ExtractFrame", ex.Message);
			flag = false;
		}
		return flag;
	}

	public static HeadersExtractor ExtractHeader(ByteBuffer buffer, MainHeaderExt tHeader, bool networkByteOrder = false)
	{
		HeadersExtractor headersExtractor = new HeadersExtractor(tHeader, buffer, networkByteOrder);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.Sizes, ref headersExtractor.HeaderExtensionSize, 48, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.LiveEvents, ref headersExtractor.HeaderExtensionLives, 8, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.PlaybackEvents, ref headersExtractor.HeaderExtensionPlaybacks, 8, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.PlaybackInfo, ref headersExtractor.HeaderExtensionPlaybackInfo, 16, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.NativeData, ref headersExtractor.HeaderExtensionNative, 8, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractNative(BinaryFrameHeaderHelper.NativeHeaderExtensionFlags.NativePlaybackSync, ref headersExtractor.HeaderExtensionNativePlaybackSync, 16, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractNative(BinaryFrameHeaderHelper.NativeHeaderExtensionFlags.NativePlaybackSeq, ref headersExtractor.HeaderExtensionNativePlaybackMotionSeq, 10, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractNativeVariable(BinaryFrameHeaderHelper.ExtractFromByteArrCount, BinaryFrameHeaderHelper.NativeHeaderExtensionFlags.PrivacyMask, ref headersExtractor.HeaderExtensionNativePrivacyMask);
		headersExtractor.ExtractNativeVariable(BinaryFrameHeaderHelper.ExtractFromByteArrCount, BinaryFrameHeaderHelper.NativeHeaderExtensionFlags.Signature, ref headersExtractor.HeaderExtensionNativeSignature);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.StreamInfo, ref headersExtractor.StreamInfo, 52, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.CarouselInfo, ref headersExtractor.CarouselInfo, 24, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.LocationInfo, ref headersExtractor.LocationInfoInfo, 48, BinaryFrameInvertor.Invert);
		headersExtractor.ExtractHeader(BinaryFrameHeaderHelper.HeaderExtensionFlags.DynamicInfo, ref headersExtractor.DinamicInfo, 16, BinaryFrameInvertor.Invert);
		for (int i = 0; i < headersExtractor.DinamicInfo.DataCount; i++)
		{
			HeaderTypeDeviceStateInfo header = default(HeaderTypeDeviceStateInfo);
			headersExtractor.ExtractDynamicType((BinaryFrameHeaderHelper.DynamicTypes)headersExtractor.DinamicInfo.DataType, ref header, BinaryFrameInvertor.Invert);
			headersExtractor.DeviceStateInfoList.Add(header);
		}
		return headersExtractor;
	}

	public static bool CheckExtensionPresence(ushort extensionFlags, BinaryFrameHeaderHelper.HeaderExtensionFlags checkFlag)
	{
		return (extensionFlags & (ushort)checkFlag) == (ushort)checkFlag;
	}

	public static bool CheckNativePresence(uint nativeFlags, BinaryFrameHeaderHelper.NativeHeaderExtensionFlags checkFlag)
	{
		return (nativeFlags & (uint)checkFlag) == (uint)checkFlag;
	}
}

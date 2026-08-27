using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Binary;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class HeadersExtractor
{
	public delegate void Invert<T>(ref T header) where T : struct;

	public delegate int Extractor<T>(byte[] buffer, int startIndex, ref T obj, int sizeOfT);

	public delegate int ExtractorVariable<T>(byte[] buffer, int startIndex, ref T obj, bool networkByteOrder);

	private MainHeaderExt _header;

	private int _extensionOffset;

	public HeaderExtensionSize HeaderExtensionSize;

	public HeaderExtensionLiveEvents HeaderExtensionLives;

	public HeaderExtensionPlaybackEvents HeaderExtensionPlaybacks;

	public HeaderExtensionPlaybackInfo HeaderExtensionPlaybackInfo;

	public HeaderExtensionNative HeaderExtensionNative;

	public HeaderExtensionNativePlaybackMotionSeq HeaderExtensionNativePlaybackMotionSeq;

	public HeaderExtensionNativePlaybackSync HeaderExtensionNativePlaybackSync;

	public HeaderExtensionNativeString HeaderExtensionNativeSignature;

	public HeaderExtensionNativeString HeaderExtensionNativePrivacyMask;

	public HeaderExtensionStreamInfo StreamInfo;

	public HeaderExtensionCarouselInfo CarouselInfo;

	public HeaderExtensionLocationInfo LocationInfoInfo;

	public HeaderExtensionDynamicInfo DinamicInfo;

	public List<HeaderTypeDeviceStateInfo> DeviceStateInfoList = new List<HeaderTypeDeviceStateInfo>();

	public bool NetworkByteOrder { get; private set; }

	public ByteBuffer Buffer { get; private set; }

	public HeadersExtractor(MainHeaderExt header, ByteBuffer buffer, bool networkByteOrder)
	{
		_header = header;
		Buffer = buffer;
		NetworkByteOrder = networkByteOrder;
		int num = 36;
		if (header.HeaderVersion == 0)
		{
			num = 40;
		}
		_extensionOffset = Buffer.begin + num;
	}

	public void ExtractHeader<T>(BinaryFrameHeaderHelper.HeaderExtensionFlags checkFlag, ref T header, int sizeOfT, Invert<T> invertor) where T : struct
	{
		if (FrameExtractor.CheckExtensionPresence(_header.HeaderExtensionFlags, checkFlag))
		{
			_extensionOffset += BinaryFrameHeaderHelper.Instance.ByteArrToStructure(Buffer.data, _extensionOffset, ref header, sizeOfT);
			if (NetworkByteOrder)
			{
				invertor(ref header);
			}
		}
	}

	public void ExtractDynamicType<T>(BinaryFrameHeaderHelper.DynamicTypes type, ref T header, Invert<T> invertor) where T : struct
	{
		int sizeOfT = 0;
		if (type == BinaryFrameHeaderHelper.DynamicTypes.HeaderTypeDeviceStateInfo)
		{
			sizeOfT = 24;
		}
		_extensionOffset += BinaryFrameHeaderHelper.Instance.ByteArrToStructure(Buffer.data, _extensionOffset, ref header, sizeOfT);
		if (NetworkByteOrder)
		{
			invertor(ref header);
		}
	}

	public void ExtractNative<T>(Extractor<T> extractor, BinaryFrameHeaderHelper.NativeHeaderExtensionFlags checkFlag, ref T header, int sizeOfT, Invert<T> invertor) where T : struct
	{
		if (FrameExtractor.CheckExtensionPresence(_header.HeaderExtensionFlags, BinaryFrameHeaderHelper.HeaderExtensionFlags.NativeData) && FrameExtractor.CheckNativePresence(HeaderExtensionNative.NativeDataExtensionFlags, checkFlag))
		{
			_extensionOffset += extractor(Buffer.data, _extensionOffset, ref header, sizeOfT);
			if (NetworkByteOrder)
			{
				invertor(ref header);
			}
		}
	}

	public void ExtractNative<T>(BinaryFrameHeaderHelper.NativeHeaderExtensionFlags checkFlag, ref T header, int sizeOfT, Invert<T> invertor) where T : struct
	{
		ExtractNative(BinaryFrameHeaderHelper.Instance.ByteArrToStructure, checkFlag, ref header, sizeOfT, invertor);
	}

	public void ExtractNativeVariable<T>(ExtractorVariable<T> extractor, BinaryFrameHeaderHelper.NativeHeaderExtensionFlags checkFlag, ref T header) where T : struct
	{
		if (FrameExtractor.CheckExtensionPresence(_header.HeaderExtensionFlags, BinaryFrameHeaderHelper.HeaderExtensionFlags.NativeData) && FrameExtractor.CheckNativePresence(HeaderExtensionNative.NativeDataExtensionFlags, checkFlag))
		{
			_extensionOffset += extractor(Buffer.data, _extensionOffset, ref header, NetworkByteOrder);
		}
	}

	public bool CheckSizes()
	{
		bool result = _extensionOffset == _header.HeaderSizeBytes;
		if (_header.FrameSizeBytes > Buffer.lenWritten - _extensionOffset)
		{
			_header.FrameSizeBytes = (uint)(Buffer.lenWritten - _extensionOffset);
			result = false;
		}
		return result;
	}
}

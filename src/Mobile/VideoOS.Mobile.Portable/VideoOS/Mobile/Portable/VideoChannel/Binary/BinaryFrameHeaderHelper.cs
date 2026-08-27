using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public class BinaryFrameHeaderHelper : IBinaryStructHelper
{
	public enum HeaderExtensionFlags
	{
		OnlyHeader = 0,
		Sizes = 1,
		LiveEvents = 2,
		PlaybackEvents = 4,
		NativeData = 8,
		MotionData = 16,
		LocationInfo = 32,
		StreamInfo = 64,
		CarouselInfo = 128,
		DynamicInfo = 256,
		PlaybackInfo = 512,
		All = 15
	}

	public enum NativeHeaderExtensionFlags
	{
		OnlyHeader = 0,
		NativePlaybackSync = 1,
		NativePlaybackSeq = 2,
		PrivacyMask = 4,
		Signature = 8,
		All = 15
	}

	public enum MainHeaderBytesOffset
	{
		VideoID = 0,
		TimeStamp = 16,
		FrameCount = 24,
		FrameSize = 28,
		HeaderSize = 32,
		HdrExtFlags = 34,
		TotalSize = 36
	}

	public enum MainHeaderExtBytesOffset
	{
		VideoID = 0,
		TimeStamp = 16,
		FrameCount = 24,
		FrameSize = 28,
		HeaderVersion = 32,
		HdrExtFlags = 34,
		HeaderSize = 36,
		TotalSize = 40
	}

	public enum HeaderExtensionSizeBytesOffset
	{
		SrcStrideX = 0,
		SrcStrideY = 4,
		SrcLeft = 8,
		SrcTop = 12,
		SrcRight = 16,
		SrcBottom = 20,
		DestStrideX = 24,
		DestStrideY = 28,
		ResamplingTag = 32,
		Reserved0 = 36,
		Reserved1 = 40,
		Reserved2 = 44,
		TotalSize = 48
	}

	public enum HeaderExtensionLiveEventsBytesOffset
	{
		CurrentFlags = 0,
		ChangedFlags = 4,
		TotalSize = 8
	}

	public enum HeaderExtensionPlaybackInfoBytesOffset
	{
		RequestedTimeStampUtcMs = 0,
		Reserved0 = 8,
		Reserved1 = 12,
		TotalSize = 16
	}

	public enum HeaderExtensionPlaybackEventsBytesOffset
	{
		CurrentFlags = 0,
		ChangedFlags = 4,
		TotalSize = 8
	}

	public enum HeaderExtensionNativeBytesOffset
	{
		HeaderSize = 0,
		ExtensionFlags = 4,
		TotalSize = 8
	}

	public enum HeaderExtensionNativePlaybackSyncBytesOffset
	{
		PreviousSyncTimeStamp = 0,
		NextSyncTimeStamp = 8,
		TotalSize = 0x10
	}

	public enum HeaderExtensionNativePlaybackMotionSeqBytesOffset
	{
		SequenceId = 0,
		NasMotion = 8,
		TotalSize = 10
	}

	public enum HeaderExtensionNativeStringBytesOffset
	{
		State = 0,
		Length = 4,
		Data = 8
	}

	[Flags]
	public enum HeaderExtensionNativeStringStateFlags
	{
		None = 0,
		Changed = 1,
		Empty = 2
	}

	public enum HeaderExtMotionOffset
	{
		HeaderSize = 0,
		MotionAmmount = 4,
		TotalSize = 8
	}

	[Flags]
	public enum LocationSourceFlags
	{
		Unknown = 0,
		Satellite = 1,
		WiFi = 2,
		Cellular = 4,
		IpAddress = 8,
		Radio = 6
	}

	[Flags]
	public enum LocationMembersFlags
	{
		Longitude = 1,
		Latitude = 2,
		Altitude = 4,
		HorizontalAccuracy = 8,
		VerticalAccuracy = 0x10,
		Speed = 0x20,
		Azimuth = 0x40,
		Horizontal = 3,
		All = 0x7F
	}

	public enum HeaderExtensionLocationInfoOffset
	{
		Length = 0,
		Version = 4,
		Source = 8,
		Reserved = 12,
		Fields = 16,
		Longitude = 20,
		Latitude = 24,
		Altitude = 28,
		HorizontalAccuracy = 32,
		VerticalAccuracy = 36,
		Speed = 40,
		Azimuth = 44,
		TotalSize = 48
	}

	public enum HeaderExtensionStreamInfoOffset
	{
		HeaderSize = 0,
		Version = 4,
		Reserved = 8,
		Fields = 12,
		TimeBetweenFrames = 16,
		DataType = 20,
		Flags = 24,
		Profile = 28,
		Level = 32,
		Compatibility = 40,
		Constraints = 44,
		FramesCount = 48,
		TotalSize = 52
	}

	[Flags]
	public enum StreamInfoFieldsFlags
	{
		TimeBetweenFrames = 1,
		SourceEncodingType = 2,
		Flags = 4,
		Profile = 8,
		Level = 0x10,
		Compatibility = 0x20,
		Constraints = 0x40,
		FramesCount = 0x80,
		All = 0xFF
	}

	public enum HeaderExtensionCarouselInfoOffset
	{
		HeaderSize = 0,
		Version = 4,
		ItemId = 8,
		TotalSize = 24
	}

	public enum HeaderExtensionDynamicInfoOffset
	{
		HeaderSize = 0,
		Version = 4,
		HeaderCount = 8,
		HeaderType = 12,
		TotalSize = 16
	}

	public enum DynamicTypes
	{
		HeaderTypeDeviceStateInfo
	}

	public enum HeaderTypeDeviceStateInfoOffset
	{
		ItemID = 0,
		ErrorCode = 16,
		Reserved = 20,
		TotalSize = 24
	}

	public delegate void Inverter<T>(ref T item);

	public delegate byte[] Writer<T>(T tObject, int sizeOfT, Inverter<T> inverter, bool invert);

	public static BinaryFrameHeaderHelper Instance = new BinaryFrameHeaderHelper();

	public IBinaryStructHelper Implementer { get; }

	private BinaryFrameHeaderHelper()
	{
		Implementer = BinaryFrameHeaderImplementer.Instance;
	}

	public T ReadStruct<T>(byte[] buffer)
	{
		return Implementer.ReadStruct<T>(buffer);
	}

	public byte[] WriteStruct<T>(T tObject, int sizeOfT)
	{
		return Implementer.WriteStruct(tObject, sizeOfT);
	}

	public byte[] WriteStruct<T>(T tObject, int sizeOfT, Inverter<T> inverter, bool invert = false)
	{
		if (invert)
		{
			inverter(ref tObject);
		}
		return WriteStruct(tObject, sizeOfT);
	}

	public void StructureToByteArray<T>(T obj, int sizeOfT, ref byte[] buffer, ref int startIndex)
	{
		byte[] array = WriteStruct(obj, sizeOfT);
		int num = array.Length;
		if (buffer.Length - startIndex < num)
		{
			num = buffer.Length - startIndex;
			Logger.Instance.Log(LogContent.Error, "BinaryFrameHeaderHelper.StructureToByteArray", "smaller size of the input array");
		}
		Array.Copy(array, 0, buffer, startIndex, num);
		startIndex += num;
	}

	public int ByteArrToStructure<T>(byte[] buffer, int startIndex, ref T obj, int sizeOfT)
	{
		if (buffer.Length - startIndex < sizeOfT)
		{
			sizeOfT = buffer.Length - startIndex;
			Logger.Instance.Log(LogContent.Error, "BinaryFrameHeaderHelper.ByteArrToStructure", "smaller size of the input array");
		}
		byte[] array = new byte[sizeOfT];
		Array.Copy(buffer, startIndex, array, 0, sizeOfT);
		obj = ReadStruct<T>(array);
		return sizeOfT;
	}

	public static byte[] ReadBytes(byte[] src, ref int startIndex, int count)
	{
		byte[] array = new byte[count];
		Array.Copy(src, startIndex, array, 0, count);
		startIndex += count;
		return array;
	}

	public static int ExtractFromByteArr(byte[] buffer, int startIndex, ref HeaderExtensionNativeString extString, bool networkByteOrder)
	{
		extString.State = BitConverter.ToUInt32(ReadBytes(buffer, ref startIndex, 4), 0);
		extString.Length = BitConverter.ToUInt32(ReadBytes(buffer, ref startIndex, 4), 0);
		if (networkByteOrder)
		{
			BinaryFrameInvertor.Invert(ref extString);
		}
		extString.Data = new byte[extString.Length];
		ReadBytes(buffer, ref startIndex, (int)extString.Length);
		return startIndex;
	}

	public static int ExtractFromByteArrCount(byte[] buffer, int startIndex, ref HeaderExtensionNativeString extString, bool networkByteOrder)
	{
		return ExtractFromByteArr(buffer, startIndex, ref extString, networkByteOrder) - startIndex;
	}
}

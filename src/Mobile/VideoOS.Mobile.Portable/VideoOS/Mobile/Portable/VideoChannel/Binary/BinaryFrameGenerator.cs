using System;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public class BinaryFrameGenerator
{
	public static HeaderExtensionSize GenerateExtFromVideoParams(VideoParams videoParams)
	{
		HeaderExtensionSize result = default(HeaderExtensionSize);
		if (videoParams.CheckForMask(StreamParamsHelper.VideoParamMask.SrcStrides))
		{
			result.SrcStrideX = videoParams.SrcStrideX;
			result.SrcStrideY = videoParams.SrcStrideY;
			result.SrcLeft = 0;
			result.SrcTop = 0;
			result.SrcRight = videoParams.SrcStrideX;
			result.SrcBottom = videoParams.SrcStrideY;
		}
		if (videoParams.CheckForMask(StreamParamsHelper.VideoParamMask.SrcRect))
		{
			result.SrcLeft = videoParams.SrcRect.Left;
			result.SrcTop = videoParams.SrcRect.Top;
			result.SrcRight = videoParams.SrcRect.Right;
			result.SrcBottom = videoParams.SrcRect.Bottom;
		}
		if (videoParams.CheckForMask(StreamParamsHelper.VideoParamMask.DstStrides))
		{
			result.DestStrideX = videoParams.DestWidth;
			result.DestStrideY = videoParams.DestHeight;
			result.ResamplingTag = 0u;
			result.Reserved0 = 0;
			result.Reserved1 = videoParams.DestWidth;
			result.Reserved2 = videoParams.DestHeight;
		}
		if (videoParams.CheckForMask(StreamParamsHelper.VideoParamMask.ResamplingTag))
		{
			result.ResamplingTag = videoParams.ResamplingTag;
		}
		return result;
	}

	public static bool GenerateFromEvents(ref HeaderExtensionPlaybackEvents playbackEvents, object events)
	{
		bool result = false;
		if (events != null && events is PlaybackStatusEventArgs e)
		{
			playbackEvents.ChangedFlags = (uint)e.ChangedStatusFlags;
			playbackEvents.CurrentFlags = (uint)e.CurrentStatusFlags;
			result = true;
		}
		return result;
	}

	public static bool GenerateFromVideoData(ref HeaderExtensionNativePlaybackSync playbackSync, VideoData videoData, bool generate)
	{
		int num;
		if (generate && videoData.RawSyncData != null)
		{
			num = (videoData.RawSyncData.IsSyncFrame ? 1 : 0);
			if (num != 0)
			{
				playbackSync.PrevousSyncTimeStamp = (ulong)videoData.RawSyncData.PreviousSyncTime.ToLong();
				playbackSync.NextSyncTimeStamp = (ulong)videoData.RawSyncData.NextSyncTime.ToLong();
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public static bool GenerateFromVideoData(ref HeaderExtensionPlaybackInfo playbackInfo, VideoData videoData)
	{
		int num;
		if (videoData != null)
		{
			num = ((videoData.RequestedTimeUTC != default(DateTime)) ? 1 : 0);
			if (num != 0)
			{
				playbackInfo.RequestedTimeStampUtcMs = (ulong)videoData.RequestedTimeUTC.ToLong();
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public static bool GenerateFromVideoData(ref HeaderExtensionNativePlaybackMotionSeq playbackMotionSeq, VideoData videoData, bool generate)
	{
		int num;
		if (generate)
		{
			num = ((videoData.RawPlayback != null) ? 1 : 0);
			if (num != 0)
			{
				playbackMotionSeq.HasMotion = (videoData.RawPlayback.HasMotion ? ((ushort)1) : ((ushort)0));
				playbackMotionSeq.SequenceId = (ulong)videoData.RawPlayback.SequenceNumber;
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public static bool GenerateFromVideoData(ref HeaderExtensionStreamInfo streamInfoHeader, VideoData videoData, bool generate)
	{
		int num;
		if (generate && videoData != null)
		{
			num = ((videoData.RawVideo != null) ? 1 : 0);
			if (num != 0)
			{
				StreamInfo streamInfo = new StreamInfo
				{
					DataType = videoData.RawVideo.RawDataType,
					TimeBetweenFrames = (uint)(videoData.EndTimeUtc - videoData.BeginTimeUtc).TotalMilliseconds,
					Profile = videoData.RawVideo.Profile,
					Level = videoData.RawVideo.Level,
					Flags = (videoData.ContainsKeyFrame ? 1u : 0u),
					FramesCount = videoData.RawVideo.FramesCount
				};
				streamInfoHeader = streamInfo.ConvertTo();
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public static bool GenerateFromString(ref HeaderExtensionNativeString extString, string value, bool generate)
	{
		if (generate)
		{
			extString.State = 1u;
			if (string.IsNullOrWhiteSpace(value))
			{
				extString.State |= 2u;
				extString.Length = 0u;
				extString.Data = new byte[0];
				return generate;
			}
			extString.Data = AsciiEncoding.Instance.GetBytes(value);
			extString.Length = (uint)extString.Data.Length;
		}
		return generate;
	}

	public static bool GenerateFromByteArr(ref HeaderExtensionNativeString extString, byte[] value, bool generate)
	{
		int num;
		if (generate && value != null)
		{
			num = (value.Any() ? 1 : 0);
			if (num != 0)
			{
				extString.State = 1u;
				extString.Data = value;
				extString.Length = (uint)extString.Data.Length;
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public static bool GenerateFromLocationInfo(ref HeaderExtensionLocationInfo headerLocationInfo, LocationInfo locationInfo)
	{
		bool num = locationInfo != null;
		if (num)
		{
			headerLocationInfo = locationInfo.ConvertTo();
		}
		return num;
	}

	public static void AddExtension(ref MainHeaderExt header, bool bAdd, ushort flag, uint size)
	{
		if (bAdd)
		{
			header.HeaderExtensionFlags |= flag;
			header.HeaderSizeBytes += size;
		}
	}

	public static void AddExtension(ref HeaderExtensionNative header, bool bAdd, uint flag, uint size)
	{
		if (bAdd)
		{
			header.NativeDataExtensionFlags |= flag;
			header.NativeDataHeaderSize += size;
		}
	}

	public static string GenerateFromHeaderExt(HeaderExtensionNativeString ext)
	{
		return AsciiEncoding.Instance.GetString(ext.Data);
	}

	public static int GetSize(HeaderExtensionNativeString ext)
	{
		return 8 + ((ext.Data != null) ? ext.Data.Length : 0);
	}

	public static byte[] ToByteArr(HeaderExtensionNativeString ext, int sizeOfT)
	{
		int startIndex = 0;
		byte[] array = new byte[GetSize(ext)];
		WriteBytes(ref startIndex, array, BitConverter.GetBytes(ext.State));
		WriteBytes(ref startIndex, array, BitConverter.GetBytes(ext.Length));
		WriteBytes(ref startIndex, array, ext.Data);
		return array;
	}

	public static byte[] ToByteArr(HeaderExtensionNativeString ext, int sizeOfT, BinaryFrameHeaderHelper.Inverter<HeaderExtensionNativeString> inverter, bool invert = false)
	{
		if (invert)
		{
			inverter(ref ext);
		}
		return ToByteArr(ext, sizeOfT);
	}

	public static void WriteBytes(ref int startIndex, byte[] dest, byte[] src)
	{
		Array.Copy(src, 0, dest, startIndex, src.Length);
		startIndex += src.Length;
	}
}

using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel;

public static class GbdExtension
{
	public static RawDataType RawDataType(this GbdHeader header)
	{
		return (int)header.CodecType switch
		{
			10 => VideoOS.Mobile.Portable.VideoChannel.RawDataType.H264, 
			1 => VideoOS.Mobile.Portable.VideoChannel.RawDataType.Jpeg, 
			14 => VideoOS.Mobile.Portable.VideoChannel.RawDataType.H265, 
			_ => VideoOS.Mobile.Portable.VideoChannel.RawDataType.Unsupported, 
		};
	}

	public static VideoData ToVideoData(this GbdVideoData gbdVideoData, StreamParamsHelper.StreamType streamType)
	{
		VideoData videoData = new VideoData
		{
			RawVideo = gbdVideoData.RawVideo,
			StreamType = streamType,
			BeginTimeUtc = gbdVideoData.PicTimeUtc,
			EndTimeUtc = gbdVideoData.NextFrameTimeUtc,
			PrevTimeUtc = gbdVideoData.PrevTimeUtc
		};
		if (gbdVideoData.ByteBuffer == null)
		{
			videoData.Content = new byte[0];
		}
		else
		{
			videoData.Content = new byte[gbdVideoData.ByteBuffer.lenWritten];
			Array.Copy(gbdVideoData.ByteBuffer.data, gbdVideoData.ByteBuffer.begin, videoData.Content, 0, gbdVideoData.ByteBuffer.lenWritten);
		}
		videoData.RawVideo.RawDataType = gbdVideoData.GbdHeader.RawDataType();
		return videoData;
	}
}

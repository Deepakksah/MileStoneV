using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Server.Service.VideoSegmentation;

public class SegmentInformation
{
	[JsonIgnore]
	public DateTime FirstFrameUTC;

	public int FrameCount;

	public double Duration;

	public double Offset;

	public double Speed;

	public double[] FrameOffsets;

	public bool ContainsKeyFrame;

	[JsonIgnore]
	public uint DurationMs;

	[JsonIgnore]
	public string FileName;

	[JsonIgnore]
	public PlaybackParamsHelper.SeekType Command;

	[JsonIgnore]
	public DateTime? TargetDateTime;

	public RawDataType RawStreamType { get; set; }

	public static SegmentInformation FromVideoFrames(IReadOnlyList<GbdVideoData> frames)
	{
		SegmentInformation result = null;
		try
		{
			uint gopDurationMs = GetGopDurationMs(frames);
			result = new SegmentInformation
			{
				Duration = (float)gopDurationMs / 1000f,
				DurationMs = gopDurationMs,
				FrameCount = frames.Count,
				FrameOffsets = frames.Select((GbdVideoData x) => (x.PicTimeUtc - x.SyncTimeUtc).TotalSeconds).ToArray(),
				Offset = 0.0,
				Speed = 0.0,
				FirstFrameUTC = frames[0].PicTimeUtc,
				FileName = frames[0].PicTimeUtc.ToMilisecString() + ".mp4",
				RawStreamType = frames[0].GbdHeader.RawDataType(),
				ContainsKeyFrame = frames.Any((GbdVideoData f) => f.IsSyncFrame)
			};
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}

	public static uint GetGopDurationMs(IReadOnlyList<GbdVideoData> frames)
	{
		if (frames == null || frames.Count < 1)
		{
			return 0u;
		}
		GbdVideoData gbdVideoData = frames.Last();
		GbdVideoData gbdVideoData2 = frames.First();
		if (gbdVideoData2 == null || gbdVideoData == null)
		{
			return 0u;
		}
		try
		{
			return (uint)(gbdVideoData.PicTimeUtc - gbdVideoData2.PicTimeUtc).TotalMilliseconds + gbdVideoData.Duration;
		}
		catch (Exception ex)
		{
			Log.Error("MfSegmenter.GetGopDuration()", ex.Message);
			return 0u;
		}
	}
}

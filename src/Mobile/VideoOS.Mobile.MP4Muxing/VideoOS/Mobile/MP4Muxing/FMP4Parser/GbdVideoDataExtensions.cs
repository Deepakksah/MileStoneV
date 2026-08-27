using System;
using System.Runtime.CompilerServices;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class GbdVideoDataExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint FrameTimeInScale(this IGbdVideoData videoData, uint timeScale)
	{
		return videoData.FrameTimeInScale(timeScale, videoData.SyncTimeUtc);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint FrameTimeInScale(this IGbdVideoData videoData, uint timeScale, DateTime firstFrameTimeUtc)
	{
		return (uint)((videoData.PicTimeUtc - firstFrameTimeUtc).TotalSeconds * (double)timeScale);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint RelativeFrameTimeInScale(this IGbdVideoData videoData, uint timeScale, uint fragmentStartTime)
	{
		return videoData.RelativeFrameTimeInScale(timeScale, fragmentStartTime, videoData.SyncTimeUtc);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint RelativeFrameTimeInScale(this IGbdVideoData videoData, uint timeScale, uint fragmentStartTime, DateTime firstFrameTimeUtc)
	{
		return (uint)((videoData.NextFrameTimeUtc - firstFrameTimeUtc).TotalSeconds * (double)timeScale) - fragmentStartTime;
	}
}

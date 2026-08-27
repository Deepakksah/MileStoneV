using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Mp4FrameGroup : IMp4FrameGroup
{
	private readonly List<IMp4Frame> _frames = new List<IMp4Frame>();

	public ulong BaseMediaDecodeTime { get; private set; }

	public IReadOnlyList<IMp4Frame> Frames => _frames;

	public Mp4FrameGroup(IReadOnlyList<IGbdVideoData> videoFrames, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer)
		: this(videoFrames, fmp4FragmentInfo, mp4MuxerCodecImplementer, Mp4Frame.Factory)
	{
	}

	public Mp4FrameGroup(IReadOnlyList<IGbdVideoData> videoFrames, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer, Func<IGbdVideoData, IFmp4FragmentInfo, IMp4MuxerCodecImpl, uint, IMp4Frame> frameFactory)
	{
		BaseMediaDecodeTime = videoFrames.First().FrameTimeInScale(fmp4FragmentInfo.TimeScaleInFmp4, fmp4FragmentInfo.FirstFrameTimeUtc);
		uint num = (uint)BaseMediaDecodeTime;
		foreach (IGbdVideoData videoFrame in videoFrames)
		{
			IMp4Frame mp4Frame = frameFactory(videoFrame, fmp4FragmentInfo, mp4MuxerCodecImplementer, num);
			if (mp4Frame.TrunSample.SampleDuration.HasValue)
			{
				num += mp4Frame.TrunSample.SampleDuration.Value;
			}
			_frames.Add(mp4Frame);
		}
	}
}

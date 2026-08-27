using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.DataTypes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Mp4Frame : IMp4Frame
{
	private readonly TrackFragmentRunBoxSample _trunSample;

	private readonly List<GenericMultiDataBoxData> _datas = new List<GenericMultiDataBoxData>();

	public IEnumerable<GenericMultiDataBoxData> Datas => _datas;

	public TrackFragmentRunBoxSample TrunSample => _trunSample;

	public uint FrameSize => (uint)_datas.Sum((GenericMultiDataBoxData data) => data.DataCount);

	public static Mp4Frame Factory(IGbdVideoData frame, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer, uint prevSampleStartTime)
	{
		return new Mp4Frame(frame, fmp4FragmentInfo, mp4MuxerCodecImplementer, prevSampleStartTime);
	}

	public Mp4Frame(IGbdVideoData frame, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer, uint prevSampleStartTime)
	{
		IList<Tuple<uint, uint>> startOffsets = mp4MuxerCodecImplementer.GetStartOffsets(frame);
		startOffsets.Add(new Tuple<uint, uint>((uint)frame.ByteBuffer.lenWritten, 0u));
		for (int i = 1; i < startOffsets.Count; i++)
		{
			Tuple<uint, uint> tuple = startOffsets[i - 1];
			Tuple<uint, uint> tuple2 = startOffsets[i];
			uint num = tuple.Item1 + tuple.Item2;
			GenericMultiDataBoxData genericMultiDataBoxData = new GenericMultiDataBoxData
			{
				Buffer = frame.ByteBuffer.data,
				StartIndex = (uint)frame.ByteBuffer.begin + num,
				DataCount = tuple2.Item1 - num
			};
			GenericMultiDataBoxData item = new GenericMultiDataBoxData
			{
				Buffer = BoxHelper.Int32ToBuffer(genericMultiDataBoxData.DataCount),
				StartIndex = 0u,
				DataCount = 4u
			};
			_datas.Add(item);
			_datas.Add(genericMultiDataBoxData);
		}
		uint value = frame.RelativeFrameTimeInScale(fmp4FragmentInfo.TimeScaleInFmp4, prevSampleStartTime, fmp4FragmentInfo.FirstFrameTimeUtc);
		_trunSample = new TrackFragmentRunBoxSample
		{
			SampleDuration = value,
			SampleSize = FrameSize,
			SampleFlags = new SampleFlags
			{
				IsNotSyncSample = !frame.IsSyncFrame
			},
			ComposTimeOffset = 0u
		};
	}
}

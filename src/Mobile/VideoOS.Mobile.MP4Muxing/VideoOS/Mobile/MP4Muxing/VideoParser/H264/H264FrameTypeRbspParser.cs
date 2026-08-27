using System.Collections.Generic;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264FrameTypeRbspParser
{
	private enum SliceType
	{
		SingleP,
		SingleB,
		SingleI,
		SingleSp,
		SingleSi,
		RepeatP,
		RepeatB,
		RepeatI,
		RepeatSp,
		RepeatSi
	}

	private static readonly Dictionary<SliceType, FrameType> SliceTypeToFrameType = new Dictionary<SliceType, FrameType>
	{
		{
			SliceType.SingleP,
			FrameType.P
		},
		{
			SliceType.SingleB,
			FrameType.B
		},
		{
			SliceType.SingleI,
			FrameType.I
		},
		{
			SliceType.SingleSp,
			FrameType.P
		},
		{
			SliceType.SingleSi,
			FrameType.I
		},
		{
			SliceType.RepeatP,
			FrameType.P
		},
		{
			SliceType.RepeatB,
			FrameType.B
		},
		{
			SliceType.RepeatI,
			FrameType.I
		},
		{
			SliceType.RepeatSp,
			FrameType.P
		},
		{
			SliceType.RepeatSi,
			FrameType.I
		}
	};

	public static FrameType Parse(byte[] data, int startIndex)
	{
		H264RbspParser h264RbspParser = new H264RbspParser(data, startIndex, null);
		H264NalUnit h264NalUnit = h264RbspParser.ReadNalUnit();
		if (h264NalUnit.NalUnitType == 5)
		{
			return FrameType.I;
		}
		if (h264NalUnit.NalUnitType == 1 || h264NalUnit.NalUnitType == 2)
		{
			h264RbspParser.ReadUe();
			SliceType key = (SliceType)h264RbspParser.ReadUe();
			if (SliceTypeToFrameType.ContainsKey(key))
			{
				return SliceTypeToFrameType[key];
			}
		}
		return FrameType.Unknown;
	}
}

using System;
using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PlaybackParamsHelper
{
	[Flags]
	public enum ParamMask : uint
	{
		Nothing = 0u,
		Speed = 1u,
		SeekType = 2u,
		Time = 4u,
		Blocking = 8u,
		ControllerId = 0x10u,
		InvestigationId = 0x20u,
		TimeSpan = 0x40u,
		ForcePlaybackFromExport = 0x80u,
		TimeRangeBegin = 0x100u,
		TimeRangeEnd = 0x200u,
		AutoLoop = 0x400u,
		RequestMandatory = 4u,
		ChangePosible = 0x40Fu,
		SeekTypeAndTime = 6u,
		GetThumnailByTimePossible = 0x46u,
		GetThumbnailFromExportPossible = 0x66u,
		TimeRangeMandatory = 0x300u,
		All = 0x7FFu
	}

	public enum SeekType
	{
		Nothing,
		[Description("DbStart")]
		DatabaseStart,
		[Description("DbEnd")]
		DatabaseEnd,
		[Description("PrevSeq")]
		SequencePrevious,
		[Description("NextSeq")]
		SequenceNext,
		[Description("PrevFrame")]
		FramePrevious,
		[Description("NextFrame")]
		FrameNext,
		[Description("Time")]
		Time,
		[Description("TimeOrBefore")]
		TimeOrBefore,
		[Description("TimeOrAfter")]
		TimeOrAfter,
		[Description("TimeBefore")]
		TimeBefore,
		[Description("TimeAfter")]
		TimeAfter
	}
}

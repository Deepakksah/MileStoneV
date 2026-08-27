using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class SequenceParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		SeqType = 1u,
		Time = 2u,
		BeforeTime = 4u,
		BeforeCount = 8u,
		AfterTime = 16u,
		AfterCount = 32u,
		ItemId = 64u,
		ForceSequencesFromExport = 128u,
		InvestigationId = 256u,
		ItemKind = 512u,
		MinTimeBetweenSequences = 1024u,
		RequestMandatory = 67u,
		BeforeAll = 12u,
		AfterAll = 48u,
		RequestPosible = 127u,
		All = 2047u
	}

	public enum SequenceType
	{
		Unknown,
		[Description("Motion")]
		Motion,
		[Description("Recording")]
		Recording,
		[Description("RecordingWithTrigger")]
		RecordingWithTrigger
	}
}

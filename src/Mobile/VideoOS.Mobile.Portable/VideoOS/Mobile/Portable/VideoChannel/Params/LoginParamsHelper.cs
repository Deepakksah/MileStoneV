using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class LoginParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("ProcessingMessage")]
		ProcessingMessage = 1u,
		[Description("SupportsResampling")]
		SupportsResampling = 2u,
		[Description("SupportsExtendedResamplingFactor")]
		ExtendedResampling = 4u,
		[Description("SupportsCarousel")]
		SupportsCarousel = 8u,
		[Description("Footages")]
		SupportsFootages = 32u,
		[Description("SupportsAudioIn")]
		SupportsAudioIn = 64u,
		[Description("SupportsAudioOut")]
		SupportsAudioOut = 128u,
		[Description("SupportsSpeakerAudioIn")]
		SupportsSpeakerAudioIn = 256u,
		[Description("SupportsAdaptiveStreaming")]
		SupportsAdaptiveStreaming = 512u,
		[Description("SupportsSmartMap")]
		SupportsSmartMap = 2048u,
		LoginPossible = 65535u,
		All = 65535u
	}
}

using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CarouselParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing,
		VideoId,
		CarouselCommand,
		All
	}

	public enum CarouselCommands
	{
		Unknown,
		[Description("NextCamera")]
		NextCamera,
		[Description("PreviousCamera")]
		PreviousCamera,
		[Description("PauseCarousel")]
		PauseCarousel,
		[Description("ResumeCarousel")]
		ResumeCarousel
	}
}

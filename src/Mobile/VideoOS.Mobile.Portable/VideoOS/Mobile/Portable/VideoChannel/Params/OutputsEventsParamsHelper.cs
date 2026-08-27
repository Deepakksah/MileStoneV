using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class OutputsEventsParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("CameraId")]
		CameraId = 1u,
		All = 1u
	}
}

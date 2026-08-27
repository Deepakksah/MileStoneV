using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class AcknowledgeAlarmParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("Id")]
		Id = 1u,
		Mandatory = 1u,
		All = 1u
	}
}

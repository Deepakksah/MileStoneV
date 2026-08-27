using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class GetServerStatusParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing,
		[Description("ServerAccessibility")]
		ServerAccessibility,
		[Description("UserId")]
		UserId,
		All
	}
}

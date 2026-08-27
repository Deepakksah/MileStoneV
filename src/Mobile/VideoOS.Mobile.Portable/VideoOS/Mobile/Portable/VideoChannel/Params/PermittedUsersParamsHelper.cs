using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PermittedUsersParamsHelper
{
	public enum ParamMask
	{
		Nothing = 0,
		[Description("SourceId")]
		SourceId = 1,
		None = 0,
		All = 1
	}
}

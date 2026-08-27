using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public enum UserType
{
	[Description("Unknown")]
	Unknown,
	[Description("Basic")]
	Basic,
	[Description("ActiveDirectory")]
	ActiveDirectory,
	[Description("External")]
	External
}

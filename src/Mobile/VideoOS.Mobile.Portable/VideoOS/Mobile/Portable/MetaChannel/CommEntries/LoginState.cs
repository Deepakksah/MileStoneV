using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public enum LoginState
{
	[Description("Unknown")]
	Initial,
	[Description("Connected")]
	Connected,
	[Description("LoggedInXProtect")]
	LoggedInXProtect,
	[Description("LoggedInSystemInfo")]
	LoggedInSystemInfo
}

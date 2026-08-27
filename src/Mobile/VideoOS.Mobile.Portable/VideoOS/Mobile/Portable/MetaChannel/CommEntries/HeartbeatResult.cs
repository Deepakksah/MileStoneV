using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public enum HeartbeatResult
{
	[Description("HeartbeatReceived")]
	HeartbeatReceived,
	[Description("InvalidSLC")]
	InvalidSLC,
	[Description("OnlineServiceCommFailure")]
	OnlineServiceCommFailure,
	[Description("InternalServiceError")]
	InternalServiceError,
	[Description("InternalResponseProcessingError")]
	InternalResponseProcessingError
}

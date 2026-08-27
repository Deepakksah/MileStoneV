using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public enum ExecuteAccessControlCommandParamMask
{
	Nothing = 0,
	[Description("AccessControlSystemId")]
	AccessControlSystemId = 1,
	[Description("AccessControlOperationalInstanceId")]
	AccessControlOperationalInstanceId = 2,
	[Description("AccessControlCommandTypeId")]
	AccessControlCommandTypeId = 4,
	[Description("EventId")]
	EventId = 8,
	[Description("TriggerSourceObjectId")]
	TriggerSourceObjectId = 16,
	[Description("TriggerSourceKindId")]
	TriggerSourceKindId = 32,
	None = 0,
	RuleCommand = 7,
	SystemCommand = 56,
	All = 63
}

using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public enum GetAccessControlEventsParamMask
{
	Nothing = 0,
	[Description("PageSize")]
	PageSize = 1,
	[Description("Page")]
	Page = 2,
	[Description("StartTimestamp")]
	StartTimestamp = 4,
	[Description("EndTimestamp")]
	EndTimestamp = 8,
	[Description("IncludeEventsCount")]
	IncludeEventsCount = 16,
	[Description("AccessControlEventId")]
	AccessControlEventId = 32,
	[Description("AccessControlRuleId")]
	AccessControlRuleId = 64,
	[Description("ExcludedAccessControlSystemIds")]
	ExcludedAccessControlSystemIds = 128,
	[Description("ExcludedAccessControlDoorIds")]
	ExcludedAccessControlDoorIds = 256,
	[Description("ExcludedAccessControlEventCategoryIds")]
	ExcludedAccessControlEventCategoryIds = 512,
	None = 0,
	ByEventId = 32,
	ByTimestamp = 11,
	All = 1023
}

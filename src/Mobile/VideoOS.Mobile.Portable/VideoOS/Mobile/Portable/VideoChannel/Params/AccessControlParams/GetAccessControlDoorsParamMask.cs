using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public enum GetAccessControlDoorsParamMask
{
	Nothing = 0,
	[Description("AccessControlDoorId")]
	AccessControlDoorId = 1,
	[Description("ExcludedAccessControlSystemIds")]
	ExcludedAccessControlSystemIds = 2,
	[Description("ExcludedAccessControlDoorIds")]
	ExcludedAccessControlDoorIds = 4,
	[Description("ExcludedAccessControlDoorCategoryIds")]
	ExcludedAccessControlDoorCategoryIds = 8,
	[Description("ExcludedAccessControlDoorTypeIds")]
	ExcludedAccessControlDoorTypeIds = 16,
	None = 0,
	All = 31
}

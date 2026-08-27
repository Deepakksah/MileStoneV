using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public enum InvestigationState
{
	[Description("InProgress")]
	InProgress,
	[Description("Completed")]
	Completed,
	[Description("Old")]
	Old,
	[Description("Imported")]
	Imported,
	[Description("Error")]
	Error
}

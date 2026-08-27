using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Management;

public enum ExportActionNoDiskSpace
{
	Unknown,
	[Description("InformUser")]
	InformUser,
	[Description("DeleteOldest")]
	DeleteOldest
}

using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public enum ExportType
{
	[Description("Unknown")]
	Unknown,
	[Description("Avi")]
	Avi,
	[Description("OriginDb")]
	OriginDb,
	[Description("Db")]
	Db,
	[Description("Mkv")]
	Mkv,
	[Description("Binary")]
	Binary,
	[Description("Aggregated")]
	Aggregated
}

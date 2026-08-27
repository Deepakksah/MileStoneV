using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel;

public enum RawDataType : uint
{
	[Description("NULL")]
	Unsupported = 0u,
	[Description("JPEG")]
	Jpeg = 1246774599u,
	[Description("H264")]
	H264 = 1211250228u,
	[Description("H265")]
	H265 = 1211250229u
}

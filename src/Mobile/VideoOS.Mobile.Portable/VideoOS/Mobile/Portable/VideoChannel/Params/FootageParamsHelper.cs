using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class FootageParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("FootageName")]
		FootageName = 1u,
		[Description("FileName")]
		FileName = 2u,
		[Description("FileSize")]
		FileSize = 4u,
		[Description("StreamId")]
		StreamId = 8u,
		[Description("ByteOrder")]
		ByteOrder = 16u,
		[Description("ExportId")]
		ExportId = 32u,
		RequestStreamMandatory = 6u,
		All = 63u
	}
}

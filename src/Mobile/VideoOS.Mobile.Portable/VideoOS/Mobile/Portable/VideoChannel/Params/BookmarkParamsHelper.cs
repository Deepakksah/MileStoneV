using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public static class BookmarkParamsHelper
{
	[Flags]
	public enum ParamMask : uint
	{
		Nothing = 0u,
		BookmarkId = 1u,
		VideoId = 2u,
		CameraId = 4u,
		Name = 8u,
		Description = 0x10u,
		Time = 0x20u,
		StartTime = 0x40u,
		EndTime = 0x80u,
		Count = 0x100u,
		MyBookmarks = 0x200u,
		SearchCameraIds = 0x400u,
		Keyword = 0x800u,
		Reference = 0x1000u,
		All = 0x1FFFu
	}
}

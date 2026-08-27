using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public static class ViewParamsHelper
{
	[Flags]
	public enum ParamMask : uint
	{
		Nothing = 0u,
		ViewId = 1u,
		IncludeEmptyItems = 2u,
		All = 3u
	}
}

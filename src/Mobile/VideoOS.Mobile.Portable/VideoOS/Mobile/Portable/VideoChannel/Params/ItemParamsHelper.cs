using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public static class ItemParamsHelper
{
	[Flags]
	public enum ParamMask : uint
	{
		Nothing = 0u,
		ItemKind = 1u,
		Hierarchy = 2u,
		IncludeRelatedDevices = 4u,
		Mandatory = 1u,
		All = 7u
	}
}

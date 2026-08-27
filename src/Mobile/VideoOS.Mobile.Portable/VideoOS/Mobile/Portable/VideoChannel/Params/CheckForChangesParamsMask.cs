using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

[Flags]
public enum CheckForChangesParamsMask : uint
{
	None = 0u,
	Investigations = 1u,
	All = 1u
}

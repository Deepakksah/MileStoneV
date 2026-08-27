using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct DSOP_UPLEVEL_FILTER_FLAGS
{
	public uint flBothModes;

	public uint flMixedModeOnly;

	public uint flNativeModeOnly;
}

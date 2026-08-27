using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct DSOP_FILTER_FLAGS
{
	public DSOP_UPLEVEL_FILTER_FLAGS Uplevel;

	public uint flDownlevel;
}

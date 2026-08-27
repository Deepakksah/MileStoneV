using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct DSOP_INIT_INFO
{
	public uint cbSize;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzTargetComputer;

	public uint cDsScopeInfos;

	public IntPtr aDsScopeInfos;

	public uint flOptions;

	public uint cAttributesToFetch;

	public IntPtr apwzAttributeNames;
}

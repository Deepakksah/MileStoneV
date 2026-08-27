using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct DSOP_SCOPE_INIT_INFO
{
	public uint cbSize;

	public uint flType;

	public uint flScope;

	[MarshalAs(UnmanagedType.Struct)]
	public DSOP_FILTER_FLAGS FilterFlags;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzDcName;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzADsPath;

	public uint hr;
}

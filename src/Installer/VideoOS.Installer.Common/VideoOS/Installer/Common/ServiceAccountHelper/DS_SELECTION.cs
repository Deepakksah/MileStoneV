using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct DS_SELECTION
{
	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzName;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzADsPath;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzClass;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwzUPN;

	public IntPtr pvarFetchedAttributes;

	public uint flScopeType;
}

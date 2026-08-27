using System;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public struct STGMEDIUM
{
	public uint tymed;

	public IntPtr hGlobal;

	public IntPtr pUnkForRelease;
}

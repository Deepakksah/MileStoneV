using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public class PInvoke
{
	[DllImport("Kernel32.dll")]
	public static extern IntPtr GlobalLock(IntPtr hMem);

	[DllImport("Kernel32.dll")]
	public static extern bool GlobalUnlock(IntPtr hMem);
}

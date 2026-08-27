using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class NativeMethods
{
	[DllImport("Shell32.dll")]
	public static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);
}

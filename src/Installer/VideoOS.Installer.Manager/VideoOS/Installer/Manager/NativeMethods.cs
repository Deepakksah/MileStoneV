using System.Runtime.InteropServices;

namespace VideoOS.Installer.Manager;

public static class NativeMethods
{
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern uint GetLastError();
}

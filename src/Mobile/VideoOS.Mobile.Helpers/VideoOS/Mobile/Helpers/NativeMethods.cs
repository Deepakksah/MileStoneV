using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Helpers;

public static class NativeMethods
{
	[DllImport("advapi32.dll", SetLastError = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

	[DllImport("advapi32.dll", SetLastError = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool RevertToSelf();

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool DuplicateToken(IntPtr token, int impersonationLevel, ref IntPtr duplication);

	[DllImport("advapi32.dll", SetLastError = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool ImpersonateLoggedOnUser(IntPtr userToken);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
	public static extern bool CloseHandle(IntPtr handle);
}

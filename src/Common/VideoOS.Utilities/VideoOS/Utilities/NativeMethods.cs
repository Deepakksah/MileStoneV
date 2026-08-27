using System;
using System.Runtime.InteropServices;

namespace VideoOS.Utilities;

public static class NativeMethods
{
	private static readonly uint KF_FLAG_FORCE_APP_DATA_REDIRECTION = 524288u;

	private static bool? _isAppContainer = null;

	public static bool IsAppContainer
	{
		get
		{
			if (!_isAppContainer.HasValue)
			{
				uint desiredAccess = 8u;
				if (!OpenProcessToken(GetCurrentProcess(), desiredAccess, out var TokenHandle))
				{
					_isAppContainer = false;
					return false;
				}
				uint tokenInformationClass = 29u;
				uint ReturnLength = 4u;
				if (!GetTokenInformation(TokenHandle, tokenInformationClass, out var TokenInformation, ReturnLength, out ReturnLength))
				{
					_isAppContainer = false;
					return false;
				}
				_isAppContainer = TokenInformation != 0;
			}
			return _isAppContainer.Value;
		}
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetCurrentProcess();

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetTokenInformation(IntPtr TokenHandle, uint TokenInformationClass, out uint TokenInformation, uint TokenInformationLength, out uint ReturnLength);

	[DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr tokenHandle = default(IntPtr));

	public static string GetCommonApplicationData()
	{
		return SHGetKnownFolderPath(new Guid("{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}"), KF_FLAG_FORCE_APP_DATA_REDIRECTION, (IntPtr)0);
	}
}

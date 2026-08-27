using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.IIS7;

public static class NativeMethods
{
	public struct OSVERSIONINFOEX
	{
		public int dwOSVersionInfoSize;

		public int dwMajorVersion;

		public int dwMinorVersion;

		public int dwBuildNumber;

		public int dwPlatformId;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szCSDVersion;

		public short wServicePackMajor;

		public short wServicePackMinor;

		public SuiteMask wSuiteMask;

		public byte wProductType;

		public byte wReserved;
	}

	[Flags]
	public enum SuiteMask : ushort
	{
		VER_SUITE_BACKOFFICE = 4,
		VER_SUITE_BLADE = 0x400,
		VER_SUITE_COMPUTE_SERVER = 0x4000,
		VER_SUITE_DATACENTER = 0x80,
		VER_SUITE_ENTERPRISE = 2,
		VER_SUITE_EMBEDDEDNT = 0x40,
		VER_SUITE_PERSONAL = 0x200,
		VER_SUITE_SINGLEUSERTS = 0x100,
		VER_SUITE_SMALLBUSINESS = 1,
		VER_SUITE_SMALLBUSINESS_RESTRICTED = 0x20,
		VER_SUITE_STORAGE_SERVER = 0x2000,
		VER_SUITE_TERMINAL = 0x10,
		VER_SUITE_WH_SERVER = 0x8000
	}

	public const int VER_NT_WORKSTATION = 1;

	public const int VER_NT_DOMAIN_CONTROLLER = 2;

	public const int VER_NT_SERVER = 3;

	[DllImport("kernel32.dll")]
	public static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

	public static bool IsWorkstation()
	{
		OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX
		{
			dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))
		};
		if (!GetVersionEx(ref osVersionInfo))
		{
			return false;
		}
		return osVersionInfo.wProductType == 1;
	}

	public static bool IsWindows7or8Embedded()
	{
		if (IsWorkstation())
		{
			OSVERSIONINFOEX osVersionInfo = default(OSVERSIONINFOEX);
			OperatingSystem oSVersion = Environment.OSVersion;
			osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			if (!GetVersionEx(ref osVersionInfo))
			{
				return false;
			}
			bool num = oSVersion.Version.Major == 6 && oSVersion.Version.Minor == 1;
			bool flag = oSVersion.Version.Major == 6 && oSVersion.Version.Minor == 2;
			bool flag2 = osVersionInfo.wSuiteMask.HasFlag(SuiteMask.VER_SUITE_EMBEDDEDNT);
			return (num || flag) && flag2;
		}
		return false;
	}
}

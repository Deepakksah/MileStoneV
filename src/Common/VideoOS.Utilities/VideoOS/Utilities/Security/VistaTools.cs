using System;
using System.Runtime.InteropServices;

namespace VideoOS.Utilities.Security;

public class VistaTools
{
	private struct OSVERSIONINFOEX
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

		public short wSuiteMask;

		public byte wProductType;

		public byte wReserved;
	}

	public enum ProductType
	{
		Server,
		Workstation,
		Unknown
	}

	public const uint STANDARD_RIGHTS_REQUIRED = 983040u;

	public const uint TOKEN_ASSIGN_PRIMARY = 1u;

	public const uint TOKEN_DUPLICATE = 2u;

	public const uint TOKEN_IMPERSONATE = 4u;

	public const uint TOKEN_QUERY = 8u;

	public const uint TOKEN_QUERY_SOURCE = 16u;

	public const uint TOKEN_ADJUST_PRIVILEGES = 32u;

	public const uint TOKEN_ADJUST_GROUPS = 64u;

	public const uint TOKEN_ADJUST_DEFAULT = 128u;

	public const uint TOKEN_ADJUST_SESSIONID = 256u;

	public const uint TOKEN_ALL_ACCESS_P = 983295u;

	private const int VER_NT_WORKSTATION = 1;

	private const int VER_NT_DOMAIN_CONTROLLER = 2;

	private const int VER_NT_SERVER = 3;

	private const int VER_SUITE_SMALLBUSINESS = 1;

	private const int VER_SUITE_ENTERPRISE = 2;

	private const int VER_SUITE_TERMINAL = 16;

	private const int VER_SUITE_DATACENTER = 128;

	private const int VER_SUITE_SINGLEUSERTS = 256;

	private const int VER_SUITE_PERSONAL = 512;

	private const int VER_SUITE_BLADE = 1024;

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetCurrentProcess();

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
	public static extern IntPtr LoadLibrary(string lpFileName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
	public static extern IntPtr GetProcAddress(IntPtr hmodule, string procName);

	public static bool IsVista()
	{
		return Environment.OSVersion.Version.Major >= 6;
	}

	public static bool IsReallyVista()
	{
		IntPtr hmodule = LoadLibrary("kernel32");
		if (hmodule.ToInt32() != 0 && GetProcAddress(hmodule, "CreateThreadpoolWait").ToInt32() != 0)
		{
			return true;
		}
		return false;
	}

	public static bool IsElevated()
	{
		if (!IsReallyVista())
		{
			throw new VistaToolsException("Function requires Vista or higher");
		}
		IntPtr TokenHandle = IntPtr.Zero;
		IntPtr currentProcess = GetCurrentProcess();
		if (currentProcess == IntPtr.Zero)
		{
			throw new VistaToolsException("Error getting current process handle");
		}
		if (!OpenProcessToken(currentProcess, 8u, out TokenHandle))
		{
			throw new VistaToolsException("Error opening process token");
		}
		try
		{
			TOKEN_ELEVATION structure = default(TOKEN_ELEVATION);
			structure.TokenIsElevated = 0u;
			uint ReturnLength = 0u;
			int num = Marshal.SizeOf(structure);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			try
			{
				Marshal.StructureToPtr(structure, intPtr, fDeleteOld: true);
				if (!GetTokenInformation(TokenHandle, TOKEN_INFORMATION_CLASS.TokenElevation, intPtr, (uint)num, out ReturnLength) || num != ReturnLength)
				{
					throw new VistaToolsException("Error getting token information");
				}
				structure = (TOKEN_ELEVATION)Marshal.PtrToStructure(intPtr, typeof(TOKEN_ELEVATION));
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return structure.TokenIsElevated != 0;
		}
		finally
		{
			CloseHandle(TokenHandle);
		}
	}

	public static TOKEN_ELEVATION_TYPE GetElevationType()
	{
		if (!IsReallyVista())
		{
			throw new VistaToolsException("Function requires Vista or higher");
		}
		IntPtr TokenHandle = IntPtr.Zero;
		IntPtr currentProcess = GetCurrentProcess();
		if (currentProcess == IntPtr.Zero)
		{
			throw new VistaToolsException("Error getting current process handle");
		}
		if (!OpenProcessToken(currentProcess, 8u, out TokenHandle))
		{
			throw new VistaToolsException("Error opening process token");
		}
		try
		{
			TOKEN_ELEVATION_TYPE tOKEN_ELEVATION_TYPE = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;
			uint ReturnLength = 0u;
			uint num = (uint)Marshal.SizeOf((int)tOKEN_ELEVATION_TYPE);
			IntPtr intPtr = Marshal.AllocHGlobal((int)num);
			try
			{
				if (!GetTokenInformation(TokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, intPtr, num, out ReturnLength) || num != ReturnLength)
				{
					throw new VistaToolsException("Error getting token information");
				}
				tOKEN_ELEVATION_TYPE = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return tOKEN_ELEVATION_TYPE;
		}
		finally
		{
			CloseHandle(TokenHandle);
		}
	}

	[DllImport("kernel32.dll")]
	private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

	public static ProductType GetOSProductType()
	{
		OSVERSIONINFOEX osVersionInfo = default(OSVERSIONINFOEX);
		_ = Environment.OSVersion;
		osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
		if (!GetVersionEx(ref osVersionInfo))
		{
			return ProductType.Unknown;
		}
		if (osVersionInfo.wProductType == 1)
		{
			return ProductType.Workstation;
		}
		return ProductType.Server;
	}
}

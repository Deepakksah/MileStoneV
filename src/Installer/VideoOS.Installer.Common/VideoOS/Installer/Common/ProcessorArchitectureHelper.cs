using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Common;

public static class ProcessorArchitectureHelper
{
	public struct SystemInfo
	{
		public ushort wProcessorArchitecture;

		public ushort wReserved;

		public uint dwPageSize;

		public IntPtr lpMinimumApplicationAddress;

		public IntPtr lpMaximumApplicationAddress;

		public UIntPtr dwActiveProcessorMask;

		public uint dwNumberOfProcessors;

		public uint dwProcessorType;

		public uint dwAllocationGranularity;

		public ushort wProcessorLevel;

		public ushort wProcessorRevision;
	}

	public const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;

	public const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;

	public const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;

	public const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = ushort.MaxValue;

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern void GetNativeSystemInfo(ref SystemInfo sysInfo);

	public static PlatformType GetPlatformType()
	{
		SystemInfo sysInfo = default(SystemInfo);
		GetNativeSystemInfo(ref sysInfo);
		return sysInfo.wProcessorArchitecture switch
		{
			9 => PlatformType.X64, 
			0 => PlatformType.X86, 
			6 => PlatformType.IA64, 
			_ => PlatformType.Unknown, 
		};
	}

	public static PlatformType ConvertToPlatform(string platform)
	{
		if (platform.ToUpper() == PlatformType.X64.ToString())
		{
			return PlatformType.X64;
		}
		if (platform.ToUpper() == PlatformType.X86.ToString())
		{
			return PlatformType.X86;
		}
		if (platform.ToUpper() == PlatformType.IA64.ToString())
		{
			return PlatformType.IA64;
		}
		if (platform.ToUpper() == PlatformType.ANY.ToString())
		{
			return PlatformType.ANY;
		}
		return PlatformType.Unknown;
	}
}

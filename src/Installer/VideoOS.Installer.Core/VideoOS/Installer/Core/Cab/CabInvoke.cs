using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

public class CabInvoke
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class FILE_IN_CABINET_INFO
	{
		public string NameInCabinet;

		public uint FileSize;

		public uint Win32Error;

		public ushort DosDate;

		public ushort DosTime;

		public ushort DosAttribs;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string FullTargetName;
	}

	public enum FDINotificationType
	{
		CabinetInfo,
		PartialFile,
		CopyFile,
		CloseFileInfo,
		NextCabinet,
		Enumerate
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class FILEPATHS
	{
		public string Target;

		public string Source;

		public uint Win32Error;

		public uint Flags;
	}

	public enum FILEOP : uint
	{
		FILEOP_ABORT,
		FILEOP_DOIT,
		FILEOP_SKIP
	}

	public delegate uint PSP_FILE_CALLBACK(uint context, uint notification, IntPtr param1, IntPtr param2);

	public const uint SPFILENOTIFY_FILEINCABINET = 17u;

	public const uint SPFILENOTIFY_NEEDNEWCABINET = 18u;

	public const uint SPFILENOTIFY_FILEEXTRACTED = 19u;

	public const uint NO_ERROR = 0u;

	public const uint A_NORMAL = 0u;

	public const uint A_RDONLY = 1u;

	public const uint A_HIDDEN = 2u;

	public const uint A_SYSTEM = 4u;

	public const uint A_SUBDIR = 16u;

	public const uint A_ARCH = 32u;

	public const int O_RDONLY = 0;

	public const int O_WRONLY = 1;

	public const int O_RDWR = 2;

	public const int O_APPEND = 8;

	public const int O_CREAT = 256;

	public const int O_TRUNC = 512;

	public const int O_EXCL = 1024;

	public const int O_TEXT = 16384;

	public const int O_BINARY = 32768;

	public const int S_IREAD = 256;

	public const int S_IWRITE = 128;

	[DllImport("SetupApi.dll", CharSet = CharSet.Auto)]
	public static extern bool SetupIterateCabinet(string cabinetFile, uint reserved, PSP_FILE_CALLBACK callBack, uint context);

	[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
	public static extern uint GetLastError();

	[DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr FDICreate(FdiMemAllocDelegate fnMemAlloc, FdiMemFreeDelegate fnMemFree, FdiFileOpenDelegate fnFileOpen, FdiFileReadDelegate fnFileRead, FdiFileWriteDelegate fnFileWrite, FdiFileCloseDelegate fnFileClose, FdiFileSeekDelegate fnFileSeek, int cpuType, [In][Out][MarshalAs(UnmanagedType.LPStruct)] CabError erf);

	[DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	public static extern bool FDICopy(IntPtr hfdi, string cabinetName, string cabinetPath, int flags, FdiNotifyDelegate fnNotify, FdiNotifyDelegate fnDecrypt, string userData);

	[DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool FDIDestroy(IntPtr hfdi);
}

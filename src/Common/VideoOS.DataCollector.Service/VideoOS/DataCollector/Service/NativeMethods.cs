using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.DataCollector.Service;

public static class NativeMethods
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class MEMORYSTATUSEX
	{
		public uint dwLength;

		public uint dwMemoryLoad;

		public ulong ullTotalPhys;

		public ulong ullAvailPhys;

		public ulong ullTotalPageFile;

		public ulong ullAvailPageFile;

		public ulong ullTotalVirtual;

		public ulong ullAvailVirtual;

		public ulong ullAvailExtendedVirtual;

		public MEMORYSTATUSEX()
		{
			dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
		}
	}

	[DllImport("msi.dll", CharSet = CharSet.Unicode)]
	private static extern uint MsiEnumRelatedProducts(string lpUpgradeCode, int dwReserved, int iProductIndex, StringBuilder lpProductBuf);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GlobalMemoryStatusEx([In][Out] MEMORYSTATUSEX lpBuffer);

	public static string? EnumRelatedProducts(string upgradeCode)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		if (MsiEnumRelatedProducts(upgradeCode, 0, 0, stringBuilder) != 0)
		{
			return null;
		}
		return stringBuilder.ToString();
	}
}

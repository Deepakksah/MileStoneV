using System.ComponentModel;
using System.Runtime.InteropServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public static class SubstitutePath
{
	private static class NativeMethods
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DefineDosDevice(int flags, string devname, [MarshalAs(UnmanagedType.LPWStr)] string path);
	}

	public static void MapDrive(char letter, string path)
	{
		if (!NativeMethods.DefineDosDevice(0, devName(letter), path))
		{
			throw new Win32Exception();
		}
	}

	public static void UnmapDrive(char letter)
	{
		if (!NativeMethods.DefineDosDevice(2, devName(letter), null))
		{
			throw new Win32Exception();
		}
	}

	private static string devName(char letter)
	{
		return new string(char.ToUpperInvariant(letter), 1) + ":";
	}
}

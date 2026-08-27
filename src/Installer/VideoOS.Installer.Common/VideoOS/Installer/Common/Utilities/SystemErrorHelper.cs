using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Installer.Common.Utilities;

public static class SystemErrorHelper
{
	[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
	public static extern uint GetLastError();

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadLibraryExW", SetLastError = true)]
	public static extern IntPtr LoadLibraryEx(string fileName, IntPtr hFile, uint flags);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr FindResourceEx(IntPtr hModule, IntPtr type, IntPtr name, ushort langId);

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool FreeLibrary(IntPtr hModule);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr LockResource(IntPtr resourceData);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "FormatMessageW", SetLastError = true)]
	public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, StringBuilder lpBuffer, uint nSize, IntPtr Arguments);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr LoadResource(IntPtr hModule, IntPtr lpResourceInfo);

	public static string GetErrorMessage(int errorNumber, CultureInfo culture)
	{
		if (culture == null)
		{
			culture = CultureInfo.CurrentCulture;
		}
		string text = null;
		if (text == null)
		{
			text = GetMessageFromModule(Path.Combine(Environment.SystemDirectory, "msimsg.dll"), errorNumber, culture);
		}
		return text;
	}

	private static string GetMessageFromModule(string modulePath, int errorNumber, CultureInfo culture)
	{
		IntPtr intPtr = LoadLibraryEx(modulePath, IntPtr.Zero, 2u);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		string result;
		try
		{
			int num = ((culture != CultureInfo.InvariantCulture) ? culture.LCID : 0);
			IntPtr intPtr2 = FindResourceEx(intPtr, new IntPtr(10), new IntPtr(errorNumber), (ushort)num);
			if (intPtr2 != IntPtr.Zero)
			{
				IntPtr intPtr3 = LockResource(LoadResource(intPtr, intPtr2));
				if (num == 0)
				{
					result = Marshal.PtrToStringAnsi(intPtr3);
				}
				else
				{
					int i;
					for (i = 0; Marshal.ReadByte(intPtr3, i) != 0; i++)
					{
					}
					byte[] array = new byte[i + 1];
					Marshal.Copy(intPtr3, array, 0, array.Length);
					result = Encoding.GetEncoding(culture.TextInfo.ANSICodePage).GetString(array);
				}
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				result = ((FormatMessage(4608u, intPtr, (uint)errorNumber, (ushort)num, stringBuilder, (uint)stringBuilder.Capacity, IntPtr.Zero) != 0) ? stringBuilder.ToString().Trim() : null);
			}
		}
		finally
		{
			FreeLibrary(intPtr);
		}
		GetLastError();
		return result;
	}
}

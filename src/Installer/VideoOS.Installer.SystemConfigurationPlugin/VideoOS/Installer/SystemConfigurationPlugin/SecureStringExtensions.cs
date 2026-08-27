using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class SecureStringExtensions
{
	public static string ConvertToUnsecureString(this SecureString securePassword)
	{
		if (securePassword == null)
		{
			return string.Empty;
		}
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
			return Marshal.PtrToStringUni(intPtr);
		}
		finally
		{
			Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
		}
	}

	public static bool IsEqualTo(this SecureString secureString1, SecureString secureString2)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		try
		{
			intPtr = Marshal.SecureStringToBSTR(secureString1);
			intPtr2 = Marshal.SecureStringToBSTR(secureString2);
			int num = Marshal.ReadInt32(intPtr, -4);
			int num2 = Marshal.ReadInt32(intPtr2, -4);
			if (num == num2)
			{
				for (int i = 0; i < num; i++)
				{
					byte num3 = Marshal.ReadByte(intPtr, i);
					byte b = Marshal.ReadByte(intPtr2, i);
					if (num3 != b)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		finally
		{
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(intPtr2);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(intPtr);
			}
		}
	}
}

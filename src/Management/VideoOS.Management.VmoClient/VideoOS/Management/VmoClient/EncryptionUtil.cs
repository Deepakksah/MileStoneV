using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace VideoOS.Management.VmoClient;

public static class EncryptionUtil
{
	public static SecureString EncryptString(string str)
	{
		SecureString secureString = new SecureString();
		if (str != null)
		{
			char[] array = str.ToCharArray();
			foreach (char c in array)
			{
				secureString.AppendChar(c);
			}
		}
		return secureString;
	}

	public static string DecryptSecureString(SecureString secureStr)
	{
		string result = string.Empty;
		if (secureStr != null)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			IntPtr intPtr = Marshal.SecureStringToBSTR(secureStr);
			result = Marshal.PtrToStringBSTR(intPtr);
			Marshal.ZeroFreeBSTR(intPtr);
		}
		return result;
	}
}

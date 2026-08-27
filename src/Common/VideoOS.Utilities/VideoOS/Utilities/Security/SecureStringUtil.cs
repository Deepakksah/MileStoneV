using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Utilities.Security;

public static class SecureStringUtil
{
	public static SecureString EncryptString(this string value)
	{
		SecureString secureString = new SecureString();
		try
		{
			if (value != null)
			{
				char[] array = value.ToCharArray();
				foreach (char c in array)
				{
					secureString.AppendChar(c);
				}
			}
		}
		catch (Exception)
		{
			secureString.Dispose();
			throw;
		}
		return secureString;
	}

	public static string DecryptSecureString(this SecureString secureString)
	{
		string result = string.Empty;
		if (secureString != null)
		{
			IntPtr intPtr = Marshal.SecureStringToBSTR(secureString);
			result = Marshal.PtrToStringBSTR(intPtr);
			Marshal.ZeroFreeBSTR(intPtr);
		}
		return result;
	}
}

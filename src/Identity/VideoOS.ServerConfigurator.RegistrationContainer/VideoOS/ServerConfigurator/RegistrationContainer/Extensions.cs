using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public static class Extensions
{
	public static byte[] EncryptCredentialPassword(this SecureString password)
	{
		return ProtectedData.Protect(password.DecryptSecureString().Encode(), null, DataProtectionScope.LocalMachine);
	}

	public static SecureString DecryptCredentialPassword(this byte[] encryptedPassword)
	{
		return ProtectedData.Unprotect(encryptedPassword, null, DataProtectionScope.LocalMachine).Decode().EncryptString();
	}

	private static SecureString EncryptString(this string value)
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

	private static string DecryptSecureString(this SecureString secureString)
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

	private static byte[] Encode(this string encodedText)
	{
		return Encoding.UTF8.GetBytes(encodedText);
	}

	private static string Decode(this byte[] decoded)
	{
		return Encoding.UTF8.GetString(decoded);
	}
}

using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Login;

public static class DataProtection
{
	private static byte[] _entropy = new byte[10] { 45, 78, 154, 194, 230, 5, 2, 134, 205, 156 };

	public static string Protect(string data)
	{
		return Convert.ToBase64String(ProtectedData.Protect(ProtectedData.Protect(Encoding.Unicode.GetBytes(data), _entropy, DataProtectionScope.CurrentUser), _entropy, DataProtectionScope.LocalMachine));
	}

	public static string Unprotect(string data)
	{
		byte[] encryptedData = ProtectedData.Unprotect(Convert.FromBase64String(data), _entropy, DataProtectionScope.LocalMachine);
		encryptedData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
		return Encoding.Unicode.GetString(encryptedData);
	}

	public static string ProtectFromLegacy(string data)
	{
		try
		{
			return Protect(UnprotectLegacy(data));
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	private static string UnprotectLegacy(string data)
	{
		byte[] encryptedData = Convert.FromBase64String(data);
		byte[] optionalEntropy = new byte[1];
		byte[] bytes = ProtectedData.Unprotect(encryptedData, optionalEntropy, DataProtectionScope.CurrentUser);
		return Encoding.Unicode.GetString(bytes);
	}
}

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class EncryptHelper
{
	public static string EncryptPassword(string password, bool unicodePassword)
	{
		if (!string.IsNullOrEmpty(password))
		{
			byte[] optionalEntropy = new byte[8] { 20, 70, 139, 161, 137, 60, 220, 152 };
			byte[] userData = ((!unicodePassword) ? Encoding.ASCII.GetBytes(password) : Encoding.Unicode.GetBytes(password));
			return Convert.ToBase64String(ProtectedData.Protect(userData, optionalEntropy, DataProtectionScope.CurrentUser));
		}
		return string.Empty;
	}

	public static string DecryptPassword(string encryptedPass, bool unicodePassword)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(encryptedPass))
		{
			try
			{
				byte[] optionalEntropy = new byte[8] { 20, 70, 139, 161, 137, 60, 220, 152 };
				byte[] bytes = ProtectedData.Unprotect(Convert.FromBase64String(encryptedPass), optionalEntropy, DataProtectionScope.CurrentUser);
				result = ((!unicodePassword) ? Encoding.ASCII.GetString(bytes) : Encoding.Unicode.GetString(bytes));
			}
			catch (FormatException exception)
			{
				LogManager.Write(exception);
				LogManager.Write(encryptedPass, TraceEventType.Information);
				throw;
			}
		}
		return result;
	}
}

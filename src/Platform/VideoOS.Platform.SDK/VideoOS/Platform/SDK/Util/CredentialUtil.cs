using System;
using System.Net;

namespace VideoOS.Platform.SDK.Util;

public class CredentialUtil
{
	public static void UpdateCredentialCacheUri(Uri oldUri, Uri newUri, CredentialCache credentialCache)
	{
		bool isBasic;
		NetworkCredential cred = ConvertCredentialCacheToNetworkCredential(oldUri, credentialCache, out isBasic);
		string authType = (isBasic ? "Basic" : "Negotiate");
		credentialCache.Remove(oldUri, authType);
		credentialCache.Add(newUri, authType, cred);
	}

	public static NetworkCredential ConvertCredentialCacheToNetworkCredential(Uri uri, CredentialCache cc, out bool isBasic)
	{
		isBasic = false;
		NetworkCredential credential = cc.GetCredential(uri, "Basic");
		if (credential != null)
		{
			isBasic = true;
		}
		else
		{
			credential = cc.GetCredential(uri, "Negotiate");
		}
		return credential;
	}

	public static bool AreNetworkCredentialsEqual(NetworkCredential nc1, NetworkCredential nc2)
	{
		if (string.Equals(nc1.Domain, nc2.Domain, StringComparison.OrdinalIgnoreCase) && string.Equals(nc1.UserName, nc2.UserName))
		{
			return ArePasswordsEqual(nc1, nc2);
		}
		return false;
	}

	private static bool ArePasswordsEqual(NetworkCredential nc1, NetworkCredential nc2)
	{
		if (!string.Equals(nc1.Password, nc2.Password))
		{
			return false;
		}
		if (nc1.SecurePassword == null && nc2.SecurePassword == null)
		{
			return true;
		}
		if (nc1.SecurePassword != null && nc2.SecurePassword != null)
		{
			if (nc1.SecurePassword.Length != nc2.SecurePassword.Length)
			{
				return false;
			}
			string a = EncryptionUtil.DecryptSecureString(nc1.SecurePassword);
			string b = EncryptionUtil.DecryptSecureString(nc2.SecurePassword);
			return string.Equals(a, b);
		}
		return false;
	}
}

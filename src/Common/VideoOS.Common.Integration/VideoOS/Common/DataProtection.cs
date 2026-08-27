using System.Security.Cryptography;
using System.Text;
using VideoOS.Common.Integration;

namespace VideoOS.Common;

public static class DataProtection
{
	public static string Protect(string data)
	{
		byte[] entropy = new byte[1];
		return new DPAPIProtectionProvider(DataProtectionScope.CurrentUser, entropy, Encoding.Unicode).Encode(data);
	}

	public static string Unprotect(string data)
	{
		byte[] entropy = new byte[1];
		return new DPAPIProtectionProvider(DataProtectionScope.CurrentUser, entropy, Encoding.Unicode).Decode(data);
	}
}

using System.Security.Cryptography;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Users;

public static class Protector
{
	private static byte[] _entropy = new byte[7] { 71, 113, 39, 17, 203, 111, 191 };

	private static byte[] Protect(byte[] data, DataProtectionScope scope)
	{
		return ProtectedData.Protect(data, _entropy, scope);
	}

	private static byte[] Unprotect(byte[] data, DataProtectionScope scope)
	{
		return ProtectedData.Unprotect(data, _entropy, scope);
	}

	public static byte[] Protect(object data, DataProtectionScope scope)
	{
		return Protect(data.ByteSerialize(), scope);
	}

	public static T Unprotect<T>(byte[] data, DataProtectionScope scope)
	{
		return Unprotect(data, scope).ByteDeserialize<T>();
	}
}

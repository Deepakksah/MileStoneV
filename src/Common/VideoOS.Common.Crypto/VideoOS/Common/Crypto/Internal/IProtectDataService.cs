using System.Security.Cryptography;

namespace VideoOS.Common.Crypto.Internal;

public interface IProtectDataService
{
	byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope);

	byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope);
}

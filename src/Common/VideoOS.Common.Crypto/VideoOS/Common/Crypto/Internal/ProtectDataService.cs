using System.Security.Cryptography;

namespace VideoOS.Common.Crypto.Internal;

public class ProtectDataService : IProtectDataService
{
	byte[] IProtectDataService.Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
	{
		return ProtectedData.Protect(userData, optionalEntropy, scope);
	}

	byte[] IProtectDataService.Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope)
	{
		return ProtectedData.Unprotect(encryptedData, optionalEntropy, scope);
	}
}

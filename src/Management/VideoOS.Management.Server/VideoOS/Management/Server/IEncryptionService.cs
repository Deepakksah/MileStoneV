using VideoOS.Common.Crypto;

namespace VideoOS.Management.Server;

public interface IEncryptionService
{
	ISymmetricEncryption SymmetricEncryption { get; }

	string Encrypt(string value);

	byte[] Encrypt(byte[] value);

	string Decrypt(string encryptedValue);

	byte[] Decrypt(byte[] encryptedValue);

	bool TryDecrypt(string value, out string encryptedValue);

	bool TryDecrypt(string value);
}

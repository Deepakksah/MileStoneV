namespace VideoOS.Common.Crypto;

public interface ISymmetricEncryption
{
	byte[] Key { get; }

	byte[] Encrypt(string value);

	byte[] EncryptSequence(byte[] value);

	string EncryptToBase64String(string value);

	string Decrypt(byte[] value);

	byte[] DecryptSequence(byte[] value);

	string DecryptFromBase64String(string value);
}

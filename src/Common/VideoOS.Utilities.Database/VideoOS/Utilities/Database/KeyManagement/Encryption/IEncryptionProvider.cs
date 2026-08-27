namespace VideoOS.Utilities.Database.KeyManagement.Encryption;

public interface IEncryptionProvider
{
	byte[] GenerateKey();

	byte[] EncryptKey(string password, byte[] key);

	byte[] DecryptKey(string password, byte[] encryptedKey);
}

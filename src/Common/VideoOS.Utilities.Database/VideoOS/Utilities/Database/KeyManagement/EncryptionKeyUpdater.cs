using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database.KeyManagement.Encryption;
using VideoOS.Utilities.Database.KeyManagement.KeyProvider;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Utilities.Database.KeyManagement;

public class EncryptionKeyUpdater : EncryptionKeyReader, IEncryptionKeyUpdater, IEncryptionKeyReader
{
	private readonly IDatabaseKeyProvider _databaseKeyProvider;

	public EncryptionKeyUpdater(IPasswordProvider passwordProvider, IEncryptionProvider encryptionProvider, IDatabaseKeyProvider databaseKeyProvider, ILogger logger)
		: base(passwordProvider, encryptionProvider, databaseKeyProvider, logger)
	{
		_databaseKeyProvider = databaseKeyProvider ?? throw new ArgumentNullException("databaseKeyProvider");
	}

	public virtual void ChangePassword(string newPassword, string accountName)
	{
		LoggerExtensions.LogDebug(Logger, "Key management: Read old password.", Array.Empty<object>());
		string password = PasswordProvider.ReadPassword();
		LoggerExtensions.LogDebug(Logger, "Key management: Read existing encryption key.", Array.Empty<object>());
		byte[] encryptedKey = _databaseKeyProvider.ReadKey();
		LoggerExtensions.LogDebug(Logger, "Key management: Decrypt key.", Array.Empty<object>());
		byte[] key = EncryptionProvider.DecryptKey(password, encryptedKey);
		LoggerExtensions.LogDebug(Logger, "Key management: Encrypt key with new password.", Array.Empty<object>());
		byte[] key2 = EncryptionProvider.EncryptKey(newPassword, key);
		LoggerExtensions.LogDebug(Logger, "Key management: Save new password and key.", Array.Empty<object>());
		using IWritingSession writingSession = PasswordProvider.StartWritingSession(accountName);
		_databaseKeyProvider.UpdateKey(key2);
		LoggerExtensions.LogInformation(Logger, "Key management: Master key successfully saved in the DB.", Array.Empty<object>());
		writingSession.Write(newPassword);
		LoggerExtensions.LogInformation(Logger, "Key management: Password successfully saved to file", Array.Empty<object>());
	}
}

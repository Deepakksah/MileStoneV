using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database.KeyManagement.Encryption;
using VideoOS.Utilities.Database.KeyManagement.KeyProvider;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Utilities.Database.KeyManagement;

public class EncryptionKeyCreator : EncryptionKeyReader, IEncryptionKeyCreator, IEncryptionKeyReader
{
	private readonly IDatabaseKeyProvider _databaseKeyProvider;

	public EncryptionKeyCreator(IPasswordProvider passwordProvider, IEncryptionProvider encryptionProvider, IDatabaseKeyProvider databaseKeyProvider, ILogger logger)
		: base(passwordProvider, encryptionProvider, databaseKeyProvider, logger)
	{
		_databaseKeyProvider = databaseKeyProvider ?? throw new ArgumentNullException("databaseKeyProvider");
	}

	private byte[] GenerateKey()
	{
		return EncryptionProvider.GenerateKey();
	}

	public virtual void InsertPasswordAndKey(string newPassword, string accountName)
	{
		LoggerExtensions.LogInformation(Logger, "Key management: Persisting password and key.", Array.Empty<object>());
		byte[] key = EncryptionProvider.EncryptKey(newPassword, GenerateKey());
		using IWritingSession writingSession = PasswordProvider.StartWritingSession(accountName);
		_databaseKeyProvider.InsertKey(key);
		LoggerExtensions.LogInformation(Logger, "Key management: Master key successfully saved in the DB.", Array.Empty<object>());
		writingSession.Write(newPassword);
		LoggerExtensions.LogInformation(Logger, "Key management: Password successfully saved to file", Array.Empty<object>());
	}
}

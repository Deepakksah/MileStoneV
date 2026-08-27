using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database.KeyManagement.Encryption;
using VideoOS.Utilities.Database.KeyManagement.KeyProvider;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Utilities.Database.KeyManagement;

public class RestrictedEncryptionKeyCreator : EncryptionKeyCreator, IEncryptionKeyCreator, IEncryptionKeyReader
{
	public RestrictedEncryptionKeyCreator(IPasswordProvider passwordProvider, IEncryptionProvider encryptionProvider, IDatabaseKeyProvider keyProvider, ILogger logger)
		: base(passwordProvider, encryptionProvider, keyProvider, logger)
	{
	}

	public override void InsertPasswordAndKey(string newPassword, string accountName)
	{
		throw new InvalidOperationException("InsertPasswordAndKey is not allowed by this implementation of IEncryptionKeyCreator");
	}
}

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database.KeyManagement.Encryption;
using VideoOS.Utilities.Database.KeyManagement.KeyProvider;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Utilities.Database.KeyManagement;

public static class KeyManagerFactory
{
	private const string Filename = "436030EF-CD5D-4722-B75C-FFD3B1FEAEF0.txt";

	private const string FolderName = "KeyManagement";

	public static IEncryptionKeyReader GetEncryptionKeyReader(string manufacturerName, ISqlConnectionManager sqlConnectionManager, ILogger logger)
	{
		if (string.IsNullOrEmpty(manufacturerName))
		{
			throw new ArgumentNullException("manufacturerName");
		}
		FilePasswordProvider passwordProvider = new FilePasswordProvider(Path.Combine(manufacturerName, "KeyManagement"), "436030EF-CD5D-4722-B75C-FFD3B1FEAEF0.txt");
		EncryptionProvider encryptionProvider = new EncryptionProvider();
		DatabaseKeyReader databaseKeyReader = new DatabaseKeyReader(sqlConnectionManager);
		return new EncryptionKeyReader(passwordProvider, encryptionProvider, databaseKeyReader, logger);
	}

	public static IEncryptionKeyUpdater GetEncryptionKeyUpdater(string manufacturerName, ISqlConnectionManager sqlConnectionManager, ILogger logger)
	{
		if (string.IsNullOrEmpty(manufacturerName))
		{
			throw new ArgumentNullException("manufacturerName");
		}
		FilePasswordProvider passwordProvider = new FilePasswordProvider(Path.Combine(manufacturerName, "KeyManagement"), "436030EF-CD5D-4722-B75C-FFD3B1FEAEF0.txt");
		EncryptionProvider encryptionProvider = new EncryptionProvider();
		DatabaseKeyProvider databaseKeyProvider = new DatabaseKeyProvider(sqlConnectionManager);
		return new EncryptionKeyUpdater(passwordProvider, encryptionProvider, databaseKeyProvider, logger);
	}

	public static IEncryptionKeyCreator GetEncryptionKeyCreator(string manufacturerName, ISqlConnectionManager sqlConnectionManager, ILogger logger)
	{
		if (string.IsNullOrEmpty(manufacturerName))
		{
			throw new ArgumentNullException("manufacturerName");
		}
		FilePasswordProvider passwordProvider = new FilePasswordProvider(Path.Combine(manufacturerName, "KeyManagement"), "436030EF-CD5D-4722-B75C-FFD3B1FEAEF0.txt");
		return GetEncryptionKeyCreator(sqlConnectionManager, logger, passwordProvider);
	}

	public static IEncryptionKeyCreator GetRestrictedEncryptionKeyCreator(string manufacturerName, ISqlConnectionManager sqlConnectionManager, ILogger logger)
	{
		if (string.IsNullOrEmpty(manufacturerName))
		{
			throw new ArgumentNullException("manufacturerName");
		}
		FilePasswordProvider passwordProvider = new FilePasswordProvider(Path.Combine(manufacturerName, "KeyManagement"), "436030EF-CD5D-4722-B75C-FFD3B1FEAEF0.txt");
		EncryptionProvider encryptionProvider = new EncryptionProvider();
		DatabaseKeyProvider keyProvider = new DatabaseKeyProvider(sqlConnectionManager);
		return new RestrictedEncryptionKeyCreator(passwordProvider, encryptionProvider, keyProvider, logger);
	}

	public static IEncryptionKeyCreator GetEncryptionKeyCreator(ISqlConnectionManager sqlConnectionManager, ILogger logger, IPasswordProvider passwordProvider)
	{
		EncryptionProvider encryptionProvider = new EncryptionProvider();
		DatabaseKeyProvider databaseKeyProvider = new DatabaseKeyProvider(sqlConnectionManager);
		return new EncryptionKeyCreator(passwordProvider, encryptionProvider, databaseKeyProvider, logger);
	}
}

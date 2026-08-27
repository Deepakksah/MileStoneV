using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database.KeyManagement.Encryption;
using VideoOS.Utilities.Database.KeyManagement.KeyProvider;
using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Utilities.Database.KeyManagement;

public class EncryptionKeyReader : IEncryptionKeyReader
{
	protected readonly IPasswordProvider PasswordProvider;

	protected readonly IEncryptionProvider EncryptionProvider;

	private readonly IDatabaseKeyReader _databaseKeyReader;

	protected readonly ILogger Logger;

	public EncryptionKeyReader(IPasswordProvider passwordProvider, IEncryptionProvider encryptionProvider, IDatabaseKeyReader databaseKeyReader, ILogger logger)
	{
		PasswordProvider = passwordProvider ?? throw new ArgumentNullException("passwordProvider");
		EncryptionProvider = encryptionProvider ?? throw new ArgumentNullException("encryptionProvider");
		_databaseKeyReader = databaseKeyReader ?? throw new ArgumentNullException("databaseKeyReader");
		Logger = logger ?? throw new ArgumentNullException("logger");
	}

	public byte[] GetUnencryptedKey(string password)
	{
		byte[] encryptedKey = _databaseKeyReader.ReadKey();
		return EncryptionProvider.DecryptKey(password, encryptedKey);
	}

	public byte[] GetUnencryptedKey()
	{
		return GetUnencryptedKey(PasswordProvider.ReadPassword());
	}

	public KeyManagementState GetKeyManagementState()
	{
		LoggerExtensions.LogInformation(Logger, "Key management: Getting state.", Array.Empty<object>());
		KeyManagementState keyManagementState = DoGetKeyManagementState();
		LoggerExtensions.LogInformation(Logger, "Key management: state {KeyManagementState}", new object[1] { keyManagementState });
		return keyManagementState;
	}

	public bool VerifyPassword(string password)
	{
		if (!HasKey())
		{
			throw new KeyStorageException(KeyStorageErrorCode.KeyIsMissingFromDatabaseOnRead, "Failed to fetch key.");
		}
		return CanDecrypt(password, GetKey());
	}

	public void VerifyAndSavePassword(string password, string accountName)
	{
		if (HasKey() && !CanDecrypt(password, GetKey()))
		{
			LoggerExtensions.LogError(Logger, "Key management: Failed to set password. Password is invalid.", Array.Empty<object>());
			throw new KeyStorageException(KeyStorageErrorCode.KeyEncryptionOperationFailed, "Failed to set password. Password is invalid.");
		}
		using IWritingSession writingSession = PasswordProvider.StartWritingSession(accountName);
		writingSession.Write(password);
		LoggerExtensions.LogInformation(Logger, "Key management: Successfully entered system configuration password.", Array.Empty<object>());
	}

	public byte[] GetKey()
	{
		return _databaseKeyReader.ReadKey();
	}

	protected bool HasKey()
	{
		return _databaseKeyReader.HasKey();
	}

	protected bool CanDecrypt(string password, byte[] key)
	{
		return CanDecryptBytePassword(password, key);
	}

	private bool HasPassword()
	{
		return PasswordProvider.HasPassword();
	}

	private string LoadPassword()
	{
		return PasswordProvider.ReadPassword();
	}

	private bool CanDecryptBytePassword(string password, byte[] encryptedKey)
	{
		try
		{
			EncryptionProvider.DecryptKey(password, encryptedKey);
			return true;
		}
		catch (KeyStorageException)
		{
			return false;
		}
	}

	private KeyManagementState DoGetKeyManagementState()
	{
		try
		{
			if (!HasKey())
			{
				return KeyManagementState.KeyMissing;
			}
			if (!HasPassword())
			{
				return KeyManagementState.PasswordInconsistent;
			}
			byte[] key = GetKey();
			string password = LoadPassword();
			if (!CanDecrypt(password, key))
			{
				return KeyManagementState.PasswordInconsistent;
			}
		}
		catch (KeyStorageException ex) when (ex.InnerException is UnauthorizedAccessException)
		{
			LoggerExtensions.LogError(Logger, "{Method} failed. Access to the file holding the system configuration password is denied.", new object[1] { "GetKeyManagementState" });
			throw;
		}
		catch (KeyStorageException ex2)
		{
			LoggerExtensions.LogError(Logger, "{Method} failed. KeyStorage error code = {Code}, exception was {Exception}", new object[3] { "GetKeyManagementState", ex2.ErrorType, ex2 });
			throw;
		}
		return KeyManagementState.PasswordConsistent;
	}
}

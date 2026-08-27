using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.Server.DataComponents;

public sealed class KeyManager
{
	private readonly ILogger _log;

	private readonly ISqlConnectionManager _sqlConnectionManager;

	public KeyManager(ISqlConnectionManager connectionManager, ILogger log)
	{
		_sqlConnectionManager = connectionManager;
		_log = log;
	}

	public byte[] LoadMasterKey()
	{
		try
		{
			IEncryptionKeyReader encryptionKeyReader = KeyManagerFactory.GetEncryptionKeyReader(OemResources.Instance.App.CompanyName, _sqlConnectionManager, _log);
			return encryptionKeyReader.GetUnencryptedKey();
		}
		catch (KeyStorageException ex)
		{
			_log.LogError("{MasterKeyFailureError}", CreateMasterKeyFailureErrorMessage(ex));
			_log.LogError("{Exception}", ex);
			throw new ServerServiceException(CreateMasterKeyFailureErrorMessage(ex), ex);
		}
	}

	private static string CreateMasterKeyFailureErrorMessage(KeyStorageException e)
	{
		switch (e.ErrorType)
		{
		case KeyStorageErrorCode.PasswordFileLocationError:
		case KeyStorageErrorCode.AccessDeniedToPasswordFile:
		case KeyStorageErrorCode.PasswordFileCannotBeUnprotected:
			return Messages.LoadPasswordDiskError();
		case KeyStorageErrorCode.KeyIsMissingFromDatabaseOnRead:
		case KeyStorageErrorCode.KeyIsMissingFromDatabaseOnUpdating:
			return Messages.EncryptionKeyIsMissing();
		case KeyStorageErrorCode.KeyEncryptionOperationFailed:
			return Messages.EncryptionKeyReadError();
		default:
			return Messages.DefaultEncryptionKeyLoadError();
		}
	}
}

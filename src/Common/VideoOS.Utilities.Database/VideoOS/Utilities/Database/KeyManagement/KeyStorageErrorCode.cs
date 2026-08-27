namespace VideoOS.Utilities.Database.KeyManagement;

public enum KeyStorageErrorCode
{
	KeyIsMissingFromDatabaseOnRead,
	KeyIsMissingFromDatabaseOnUpdating,
	PasswordFileLocationError,
	AccessDeniedToPasswordFile,
	PasswordFileCannotBeUnprotected,
	PasswordFileCannotBeProtected,
	KeyEncryptionOperationFailed,
	UnknownDatabaseError,
	RequestedDatabaseCannotBeFound,
	StoredProcedureNotFound,
	PasswordFileInUse,
	PasswordFileSecurity,
	UnknownError
}

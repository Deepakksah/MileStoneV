namespace VideoOS.DatabaseUtility.MediaStorage;

public class MediaStorageUtilityException(string errorMessage, MediaStorageUtilityException.ErrorCode errorCode) : UtilityException(errorMessage)
{
	public enum ErrorCode
	{
		MissingTable,
		DatabaseNotOpen,
		ReadConfigurationError,
		FailedOpeningDatabase,
		RepairFailed,
		DatabaseStateException,
		SystemException,
		UnsupportedEncryption
	}

	private ErrorCode m_ErrorCode = errorCode;

	public ErrorCode Error => m_ErrorCode;
}

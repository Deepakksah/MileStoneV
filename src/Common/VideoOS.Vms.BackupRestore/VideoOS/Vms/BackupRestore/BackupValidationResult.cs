namespace VideoOS.Vms.BackupRestore;

public class BackupValidationResult : ValidationResult
{
	public BackupConfigManager.BackupConfigError ValidationError { get; set; }

	private BackupValidationResult(bool success, BackupConfigManager.BackupConfigError validationError, string englishErrorDescription, string[] errorParameters)
		: base(success, englishErrorDescription, errorParameters)
	{
		ValidationError = validationError;
	}

	public static BackupValidationResult CreateSuccessResult()
	{
		return new BackupValidationResult(success: true, BackupConfigManager.BackupConfigError.Undefined, null, null);
	}

	public static BackupValidationResult CreateErrorResult(BackupConfigManager.BackupConfigError validationError, string englishErrorDescription, string[] errorParameters)
	{
		return new BackupValidationResult(success: false, validationError, englishErrorDescription, errorParameters);
	}
}

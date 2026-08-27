namespace VideoOS.Vms.BackupRestore;

public class RestoreValidationResult : ValidationResult
{
	public RestoreConfigError ValidationError { get; set; }

	private RestoreValidationResult(bool success, RestoreConfigError validationError, string englishErrorDescription, string[] errorParameters)
		: base(success, englishErrorDescription, errorParameters)
	{
		ValidationError = validationError;
	}

	public static RestoreValidationResult CreateSuccessResult()
	{
		return new RestoreValidationResult(success: true, RestoreConfigError.Undefined, null, null);
	}

	public static RestoreValidationResult CreateErrorResult(RestoreConfigError validationError, string englishErrorDescription, string[] errorParameters)
	{
		return new RestoreValidationResult(success: false, validationError, englishErrorDescription, errorParameters);
	}
}

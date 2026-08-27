namespace VideoOS.SqlUpdate.Core.Common;

public class DatabaseValidationResult
{
	public DatabaseValidationStatus Status { get; }

	public string ErrorMessage { get; }

	public DatabaseValidationResult(DatabaseValidationStatus status, string errorMessage)
	{
		Status = status;
		ErrorMessage = errorMessage;
	}
}

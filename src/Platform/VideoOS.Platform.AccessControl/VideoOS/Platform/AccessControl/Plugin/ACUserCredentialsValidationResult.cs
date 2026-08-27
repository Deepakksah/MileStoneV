namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACUserCredentialsValidationResult
{
	public bool CredentialsValid { get; private set; }

	public string ErrorMessage { get; private set; }

	public int ErrorCode { get; private set; }

	public ACUserCredentialsValidationResult(bool valid)
		: this(valid, string.Empty, -1)
	{
	}

	public ACUserCredentialsValidationResult(bool valid, string errorMessage, int errorCode)
	{
		CredentialsValid = valid;
		ErrorMessage = errorMessage;
		ErrorCode = errorCode;
	}
}

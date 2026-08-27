namespace VideoOS.Utilities.Security;

public enum PasswordValidationResult
{
	Ok,
	TooShort,
	TooLong,
	ComplexityPolicyNotMet,
	LocalPasswordFilterNotMet,
	OtherError,
	PasswordsDiffers,
	PasswordIsEmpty
}

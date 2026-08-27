namespace VideoOS.Common.Integration;

public enum PasswordValidationResult
{
	Success,
	NeedsDigits,
	NeedsLowercase,
	NeedsUppercase,
	MissingRequiredCharacter,
	UsesForbiddenCharacter
}

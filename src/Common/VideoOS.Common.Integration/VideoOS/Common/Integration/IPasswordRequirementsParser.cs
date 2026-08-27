namespace VideoOS.Common.Integration;

public interface IPasswordRequirementsParser
{
	PasswordRequirements GetRequirements(string passwordRequirements);

	PasswordRequirements GetRequirements(bool needsUppercaseLetters, bool needsLowercaseLetters, bool needsDigits);
}

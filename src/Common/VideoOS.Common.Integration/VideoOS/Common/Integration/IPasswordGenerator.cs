namespace VideoOS.Common.Integration;

public interface IPasswordGenerator
{
	string GeneratePassword(int? minPasswordLength, int? maxPasswordLength, PasswordRequirements passwordRequirements);
}

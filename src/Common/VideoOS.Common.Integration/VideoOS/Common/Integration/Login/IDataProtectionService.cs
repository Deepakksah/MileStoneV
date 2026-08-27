namespace VideoOS.Common.Integration.Login;

public interface IDataProtectionService
{
	string Unprotect(string encrypted);
}

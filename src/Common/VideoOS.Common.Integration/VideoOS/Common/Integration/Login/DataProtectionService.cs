namespace VideoOS.Common.Integration.Login;

public class DataProtectionService : IDataProtectionService
{
	public string Unprotect(string encrypted)
	{
		return DataProtection.Unprotect(encrypted);
	}
}

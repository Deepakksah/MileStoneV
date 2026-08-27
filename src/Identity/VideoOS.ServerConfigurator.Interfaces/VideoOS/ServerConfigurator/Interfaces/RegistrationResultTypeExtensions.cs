namespace VideoOS.ServerConfigurator.Interfaces;

public static class RegistrationResultTypeExtensions
{
	public static bool SuccessOrWarning(this RegistrationResultType registrationResultType)
	{
		return registrationResultType switch
		{
			RegistrationResultType.Success => true, 
			RegistrationResultType.Warning => true, 
			_ => false, 
		};
	}
}

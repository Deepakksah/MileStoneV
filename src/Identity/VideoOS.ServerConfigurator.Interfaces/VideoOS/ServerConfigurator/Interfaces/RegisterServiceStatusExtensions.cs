namespace VideoOS.ServerConfigurator.Interfaces;

public static class RegisterServiceStatusExtensions
{
	public static RegistrationResultType ResultType(this RegisterServiceStatus registerServiceStatus)
	{
		return registerServiceStatus switch
		{
			RegisterServiceStatus.Success => RegistrationResultType.Success, 
			RegisterServiceStatus.ServiceRegistrationNotFound => RegistrationResultType.Warning, 
			_ => RegistrationResultType.Error, 
		};
	}
}

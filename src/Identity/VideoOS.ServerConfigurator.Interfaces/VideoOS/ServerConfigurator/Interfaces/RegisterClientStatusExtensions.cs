namespace VideoOS.ServerConfigurator.Interfaces;

public static class RegisterClientStatusExtensions
{
	public static RegistrationResultType ResultType(this RegisterClientStatus registerClientStatus)
	{
		if (registerClientStatus == RegisterClientStatus.Success)
		{
			return RegistrationResultType.Success;
		}
		return RegistrationResultType.Error;
	}
}

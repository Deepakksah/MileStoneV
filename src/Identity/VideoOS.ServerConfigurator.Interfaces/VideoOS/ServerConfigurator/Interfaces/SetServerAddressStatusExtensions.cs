namespace VideoOS.ServerConfigurator.Interfaces;

public static class SetServerAddressStatusExtensions
{
	public static RegistrationResultType ResultType(this SetServerAddressStatus setServerAddressStatus)
	{
		if (setServerAddressStatus == SetServerAddressStatus.Success)
		{
			return RegistrationResultType.Success;
		}
		return RegistrationResultType.Error;
	}
}

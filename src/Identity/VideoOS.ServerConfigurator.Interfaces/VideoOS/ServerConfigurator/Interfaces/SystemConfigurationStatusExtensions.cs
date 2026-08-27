namespace VideoOS.ServerConfigurator.Interfaces;

public static class SystemConfigurationStatusExtensions
{
	public static RegistrationResultType ResultType(this SystemConfigurationStatus setServerAddressStatus)
	{
		if (setServerAddressStatus == SystemConfigurationStatus.Success)
		{
			return RegistrationResultType.Success;
		}
		return RegistrationResultType.Error;
	}
}

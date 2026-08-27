namespace VideoOS.ServerConfigurator.Interfaces;

public static class ConsistencyStatusExtensions
{
	public static RegistrationResultType ResultType(this ConsistencyStatus registerServiceStatus)
	{
		return registerServiceStatus switch
		{
			ConsistencyStatus.Success => RegistrationResultType.Success, 
			ConsistencyStatus.ClientEndpointSecuritySchemeDoesNotMatchServerEndpoint => RegistrationResultType.Error, 
			_ => RegistrationResultType.Error, 
		};
	}
}

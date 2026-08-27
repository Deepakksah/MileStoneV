namespace VideoOS.ServerConfigurator.Interfaces;

public static class SetSqlServerStatusExtensions
{
	public static RegistrationResultType ResultType(this SetSqlServerStatus setSqlServerStatus)
	{
		if (setSqlServerStatus == SetSqlServerStatus.Success)
		{
			return RegistrationResultType.Success;
		}
		return RegistrationResultType.Error;
	}
}

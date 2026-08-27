namespace VideoOS.ServerConfigurator.Interfaces;

public static class ApplyStatusExtensions
{
	public static RegistrationResultType ResultType(this ApplyStatus applyStatus)
	{
		if (applyStatus == ApplyStatus.Success)
		{
			return RegistrationResultType.Success;
		}
		return RegistrationResultType.Error;
	}
}

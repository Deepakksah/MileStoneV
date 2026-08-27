namespace VideoOS.ServerConfigurator.Interfaces;

public enum RegisterServiceStatus
{
	Success,
	ManagementServerNotFound,
	UnspecifiedError,
	CouldNotStopServer,
	CouldNotStartServer,
	InsufficientCredentials,
	ServiceRegistrationNotFound
}

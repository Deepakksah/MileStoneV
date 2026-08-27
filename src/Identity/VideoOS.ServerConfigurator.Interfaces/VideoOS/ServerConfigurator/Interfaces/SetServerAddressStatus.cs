namespace VideoOS.ServerConfigurator.Interfaces;

public enum SetServerAddressStatus
{
	Success,
	UnspecifiedError,
	CouldNotStopServer,
	CouldNotStartServer,
	InsufficientCredentials,
	ManagementServerNotFound,
	SetServerAddressNotAllowedWhenManagementServerIsLocal
}

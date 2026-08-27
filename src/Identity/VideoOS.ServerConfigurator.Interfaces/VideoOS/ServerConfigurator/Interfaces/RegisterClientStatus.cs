namespace VideoOS.ServerConfigurator.Interfaces;

public enum RegisterClientStatus
{
	Success,
	InsufficientCredentials,
	InvalidCredentials,
	AuthorizationServerNotFound,
	UnspecifiedError,
	CouldNotStopServer,
	CouldNotStartServer
}

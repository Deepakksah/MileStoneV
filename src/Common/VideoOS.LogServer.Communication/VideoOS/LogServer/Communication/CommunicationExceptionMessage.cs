namespace VideoOS.LogServer.Communication;

public enum CommunicationExceptionMessage
{
	CommunicationError,
	FaultError,
	TimeoutError,
	InvalidLogServerAddress,
	LogServerNotRegistered,
	FailedToObtainRegisteredServices,
	UserValidationError,
	MethodNotSupported,
	InvalidResponse,
	UnknownSessionError,
	QueryTimeoutError
}

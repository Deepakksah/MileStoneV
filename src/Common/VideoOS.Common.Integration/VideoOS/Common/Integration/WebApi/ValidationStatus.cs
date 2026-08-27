namespace VideoOS.Common.Integration.WebApi;

public enum ValidationStatus
{
	Undefined,
	Successful,
	CommunicationException,
	Timeout,
	ServiceIdMismatch,
	ProtocolMismatch,
	AccessTokenException
}

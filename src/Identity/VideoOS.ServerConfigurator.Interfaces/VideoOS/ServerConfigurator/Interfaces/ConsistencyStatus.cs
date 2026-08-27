namespace VideoOS.ServerConfigurator.Interfaces;

public enum ConsistencyStatus
{
	Success,
	ClientEndpointSecuritySchemeDoesNotMatchServerEndpoint,
	ServerCertificateIsNotTrusted
}

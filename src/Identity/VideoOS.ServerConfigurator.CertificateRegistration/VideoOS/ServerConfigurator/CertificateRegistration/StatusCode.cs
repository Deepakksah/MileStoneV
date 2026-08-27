namespace VideoOS.ServerConfigurator.CertificateRegistration;

public enum StatusCode
{
	Success = 0,
	VmsAdminCredentialsNeeded = 1,
	AuthorizationServerNotFound = 2,
	AuthorizationServerAddressInvalid = 3,
	UnspecifiedErrorEncryption = 4,
	UnspecifiedErrorRegistration = 5,
	CouldNotStartServer = 6,
	CouldNotStopServer = 7,
	ManagementServerNotFound = 8,
	ServiceRegistrationNotFound = 9,
	UnhandledException = int.MaxValue
}

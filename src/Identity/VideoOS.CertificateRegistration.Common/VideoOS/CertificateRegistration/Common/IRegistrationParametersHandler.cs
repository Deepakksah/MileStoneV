using System;

namespace VideoOS.CertificateRegistration.Common;

public interface IRegistrationParametersHandler
{
	RegistrationParameters ReadRegistrationParameters(Uri authorizationServerAddress);

	bool WriteRegistrationParameters(RegistrationParameters parameters);
}

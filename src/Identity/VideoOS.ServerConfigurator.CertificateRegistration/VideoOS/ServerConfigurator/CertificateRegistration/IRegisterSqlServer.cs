using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IRegisterSqlServer : IServerCleanup
{
	Status PreRegisterSqlServer();

	[Obsolete("Do not use, for future use", false)]
	Status SetSqlServerParameters(ConnectionStringParameters connectionStringParameters);

	Status PostRegisterSqlServer();
}

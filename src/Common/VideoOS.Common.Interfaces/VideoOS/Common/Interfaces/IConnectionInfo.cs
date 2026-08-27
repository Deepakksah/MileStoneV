using System;
using System.Net;
using System.Security.Principal;

namespace VideoOS.Common.Interfaces;

public interface IConnectionInfo
{
	ConnectionAuthenticationType AuthenticationType { get; }

	string AuthorizationId { get; set; }

	string ClientDisplayVersion { get; set; }

	ICredentials Credentials { get; }

	string DomainName { get; }

	string EncryptedPassword { get; }

	string LoginProviderId { get; }

	string Password { get; }

	Uri ServerAddress { get; }

	string ServerDisplayVersion { get; set; }

	Guid ServerTypeId { get; }

	string UserDomainName { get; }

	IIdentity UserIdentity { get; }

	string UserName { get; }

	bool UseSecureConnection { get; }

	string[] SupportedScopes { get; }

	string ExternalProvider { get; }
}

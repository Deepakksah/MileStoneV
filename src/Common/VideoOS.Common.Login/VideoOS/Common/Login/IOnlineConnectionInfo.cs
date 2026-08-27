using System;
using System.Net;

namespace VideoOS.Common.Login;

public interface IOnlineConnectionInfo : IConnectionInfo
{
	Guid ServerTypeId { get; }

	string ServerTypeName { get; }

	Uri ServerAddress { get; }

	AuthenticationType AuthenticationType { get; }

	string LoginProviderId { get; }

	string DomainName { get; }

	string UserName { get; }

	string EncryptedPassword { get; }

	string Password { get; }

	CredentialCache Credentials { get; }
}

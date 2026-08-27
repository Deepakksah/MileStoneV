using System;
using System.Collections.Generic;

namespace VideoOS.CertificateRegistration.Common;

public class RegistrationParameters
{
	public Uri AuthorizationServerAddress { get; }

	public Guid ClientId { get; }

	public IReadOnlyCollection<string> Scopes { get; }

	public string ClientName { get; }

	public RegistrationParameters(Guid clientId, string clientName, IReadOnlyCollection<string> scopes, Uri authorizationServerAddress)
	{
		ClientId = clientId;
		ClientName = clientName;
		Scopes = scopes;
		AuthorizationServerAddress = authorizationServerAddress;
	}
}

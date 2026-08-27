using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class CertificateAffectedClientRegistration : ClientRegistration
{
	public CertificateAffectedClientRegistration(IEnumerable<ServerEndpointConfiguration> serverEndpoints, IEnumerable<ClientEndpointConfiguration> clientEndpoints, NetworkCredential credentials, Uri clientRegistrationServer, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration)
		: base(GetAffectedClients(serverEndpoints, clientEndpoints), credentials, clientRegistrationServer, registrationLogger, logger, innerRegistration)
	{
	}

	private static IEnumerable<ClientEndpointConfiguration> GetAffectedClients(IEnumerable<ServerEndpointConfiguration> serverEndpoints, IEnumerable<ClientEndpointConfiguration> clientEndpoints)
	{
		IEnumerable<Guid> enumerable = from y in serverEndpoints.Select((ServerEndpointConfiguration x) => x.CertificateGroupId).Distinct()
			where y != Guid.Empty
			select y;
		List<ClientEndpointConfiguration> list = new List<ClientEndpointConfiguration>();
		foreach (Guid groupId in enumerable)
		{
			list.AddRange(clientEndpoints.Where((ClientEndpointConfiguration x) => x.ServerGroupDependency == groupId));
		}
		return list;
	}
}

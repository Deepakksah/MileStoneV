using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface INonvolatileServerCertificateQueryModel
{
	IEnumerable<ServerEndpointConfiguration> ServerEndpointsInstalled { get; }

	IEnumerable<CertificateGroup> CertificateGroups { get; }

	bool IsValidThumbprint(string thumbprint, Guid certificateGroupId);
}

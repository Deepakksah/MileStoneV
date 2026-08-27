using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface IServerCertificateQueryModel : INonvolatileServerCertificateQueryModel
{
	IEnumerable<ServerEndpointConfiguration> ServerEndpointsToBeInstalled { get; }

	event EventHandler<EventArgs> CertificateGroupsModified;

	event EventHandler<EventArgs> ServerEndpointsModified;
}

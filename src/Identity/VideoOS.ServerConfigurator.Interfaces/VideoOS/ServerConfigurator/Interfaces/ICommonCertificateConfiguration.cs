using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface ICommonCertificateConfiguration
{
	IEnumerable<ServerEndpointConfiguration> ServerCertificateConfigCollection { get; }

	IEnumerable<CertificateGroup> CertificateGroupCollection { get; }

	IEnumerable<CertificateGroup> GetCertificateGroup(Guid id);

	ServerEndpointConfiguration GetServerConfiguration(string serverId);

	void SetServerConfiguration(ServerEndpointConfiguration serverConfigurator);

	void SetCertificateGroup(CertificateGroup certificateGroup);
}

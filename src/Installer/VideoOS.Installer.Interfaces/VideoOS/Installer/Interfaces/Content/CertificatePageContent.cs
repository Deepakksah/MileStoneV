using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.Content;

public class CertificatePageContent
{
	public IList<ClientEndpoint> ClientEndpoints { get; } = new List<ClientEndpoint>();

	public IList<ServerEndpoint> ServerEndpoints { get; } = new List<ServerEndpoint>();

	public IList<SystemConfigurationElement> SystemConfigurationElements { get; } = new List<SystemConfigurationElement>();

	public IList<ServerCertificateGroup> ServerCertificateGroups { get; } = new List<ServerCertificateGroup>();
}

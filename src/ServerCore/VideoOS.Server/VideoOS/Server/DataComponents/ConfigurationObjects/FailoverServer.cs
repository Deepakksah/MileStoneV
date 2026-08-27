using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class FailoverServer
{
	public Guid Id { get; set; }

	public Guid ParentId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string HostName { get; set; }

	public ushort UdpPort { get; set; }

	public short GroupSequence { get; set; }

	public bool Enabled { get; set; }

	public string DatabaseLocation { get; set; }

	public DateTime LastModified { get; set; }

	public bool CertificateConfigured { get; set; }

	public string WebServerHostName { get; set; }

	public int WebServerPort { get; set; }

	public bool PublicAccessEnabled { get; set; }

	public string PublicWebServerHostName { get; set; }

	public int PublicWebServerPort { get; set; }

	public static FailoverServer[] QueryFailoverServers()
	{
		using FailoverServerComponent failoverServerComponent = new FailoverServerComponent();
		return failoverServerComponent.QueryFailoverServers();
	}
}

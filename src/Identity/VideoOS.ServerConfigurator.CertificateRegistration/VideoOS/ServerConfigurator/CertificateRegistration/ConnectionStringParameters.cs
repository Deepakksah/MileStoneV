using System;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

[Serializable]
public class ConnectionStringParameters
{
	public string Host { get; set; }

	public int? Port { get; set; }

	public string InstanceName { get; set; }

	public string InitialCatalog { get; set; }
}

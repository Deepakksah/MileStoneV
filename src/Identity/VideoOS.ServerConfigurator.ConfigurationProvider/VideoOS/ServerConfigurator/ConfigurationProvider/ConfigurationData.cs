using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[XmlRoot("ConfigurationData")]
public class ConfigurationData : ConfigurationDataBase
{
	public ApplicationSettings ApplicationSettings { get; set; }

	public ClientRegistrationServer ClientRegistrationServer { get; set; }

	public Collection<CertificateGroupData> CertificateGroupsData { get; }

	public Collection<ServerCertificateConfigData> ServersCertificateConfigData { get; }

	public Collection<ClientRegistrationConfigData> ClientsRegistrationConfigData { get; }

	public Collection<ServerCertificateConfigData> VolatileServersCertificateConfigData { get; }

	public Collection<ClientRegistrationConfigData> VolatileClientsRegistrationConfigData { get; }

	public ConfigurationData()
	{
		ApplicationSettings = new ApplicationSettings();
		ClientRegistrationServer = new ClientRegistrationServer();
		CertificateGroupsData = new Collection<CertificateGroupData>();
		ServersCertificateConfigData = new Collection<ServerCertificateConfigData>();
		ClientsRegistrationConfigData = new Collection<ClientRegistrationConfigData>();
		VolatileServersCertificateConfigData = new Collection<ServerCertificateConfigData>();
		VolatileClientsRegistrationConfigData = new Collection<ClientRegistrationConfigData>();
	}
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Name = "ConfigurationData", Namespace = "")]
[XmlRoot("ConfigurationData")]
public class ConfigurationData2 : ConfigurationDataBase
{
	[DataMember(Order = 0)]
	public ApplicationSettings ApplicationSettings { get; set; }

	[DataMember(Order = 1)]
	public ManagementServerAddress ManagementServerAddress { get; set; }

	[DataMember(Order = 2)]
	public AuthorizationServerAddress AuthorizationServerAddress { get; set; }

	[DataMember(Order = 3)]
	public string HostName { get; set; }

	[DataMember(Order = 4)]
	public Collection<CertificateGroupData> CertificateGroupsData { get; private set; }

	[DataMember(Order = 5)]
	public Collection<ServerEndpointConfiguration> ServerEndpointConfigurations { get; private set; }

	[DataMember(Order = 6)]
	public Collection<ClientEndpointConfiguration> ClientEndpointConfigurations { get; private set; }

	[DataMember(Order = 7)]
	public Collection<SystemConfigurationElement> ConfigurationElements { get; private set; }

	[DataMember(Order = 8)]
	public Collection<ServerEndpointConfiguration> VolatileServersCertificateConfigData { get; private set; }

	[DataMember(Order = 9)]
	public Collection<ClientEndpointConfiguration> VolatileClientsRegistrationConfigData { get; private set; }

	[DataMember(Order = 10)]
	public Collection<SystemConfigurationElement> VolatileSystemConfigurationElements { get; private set; }

	[DataMember(Order = 11)]
	public bool OverrideLocalManagementServer { get; set; }

	[DataMember(Order = 12)]
	public bool PreferredSecureState { get; set; }

	public ConfigurationData2(ConfigurationData configurationData)
		: this()
	{
		if (configurationData == null)
		{
			throw new ArgumentNullException("configurationData");
		}
		ApplicationSettings = configurationData.ApplicationSettings;
		CertificateGroupsData = configurationData.CertificateGroupsData;
		ServerEndpointConfigurations = new Collection<ServerEndpointConfiguration>(configurationData.ServersCertificateConfigData?.Select((ServerCertificateConfigData x) => new ServerEndpointConfiguration(x)).ToList());
		ClientEndpointConfigurations = new Collection<ClientEndpointConfiguration>(configurationData.ClientsRegistrationConfigData.Select((ClientRegistrationConfigData x) => new ClientEndpointConfiguration(x)).ToList());
		SetManagementServerAddress(configurationData);
		SetAuthorizationServerAddress(configurationData);
	}

	public ConfigurationData2()
	{
		Initialize();
	}

	[OnDeserializing]
	private void OnDeserializingMethod(StreamingContext context)
	{
		Initialize();
	}

	private void Initialize()
	{
		ApplicationSettings = new ApplicationSettings();
		ManagementServerAddress = new ManagementServerAddress();
		AuthorizationServerAddress = new AuthorizationServerAddress();
		CertificateGroupsData = new Collection<CertificateGroupData>();
		ServerEndpointConfigurations = new Collection<ServerEndpointConfiguration>();
		ClientEndpointConfigurations = new Collection<ClientEndpointConfiguration>();
		ConfigurationElements = new Collection<SystemConfigurationElement>();
		VolatileServersCertificateConfigData = new Collection<ServerEndpointConfiguration>();
		VolatileClientsRegistrationConfigData = new Collection<ClientEndpointConfiguration>();
		VolatileSystemConfigurationElements = new Collection<SystemConfigurationElement>();
	}

	private void SetManagementServerAddress(ConfigurationData configurationData)
	{
		ManagementServerAddress = new ManagementServerAddress();
		Uri uri = new Uri(configurationData.ClientRegistrationServer.SecureIdpAddress);
		Uri uri2 = new Uri(configurationData.ClientRegistrationServer.UnsecureIdpAddress);
		ManagementServerAddress.FullyQualifiedDomainName = uri.Host;
		ManagementServerAddress.SecurePort = uri.Port;
		ManagementServerAddress.UnsecurePort = uri2.Port;
	}

	private void SetAuthorizationServerAddress(ConfigurationData configurationData)
	{
		AuthorizationServerAddress = new AuthorizationServerAddress();
		Uri uri = new Uri(configurationData.ClientRegistrationServer.SecureIdpAddress);
		Uri uri2 = new Uri(configurationData.ClientRegistrationServer.UnsecureIdpAddress);
		AuthorizationServerAddress.FullyQualifiedDomainName = uri.Host;
		AuthorizationServerAddress.Path = uri.AbsolutePath;
		AuthorizationServerAddress.SecurePort = uri.Port;
		AuthorizationServerAddress.UnsecurePort = uri2.Port;
	}
}

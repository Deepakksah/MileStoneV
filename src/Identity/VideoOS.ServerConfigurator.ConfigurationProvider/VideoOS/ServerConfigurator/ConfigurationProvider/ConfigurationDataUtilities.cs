using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

public static class ConfigurationDataUtilities
{
	public static void PopulateToConfigurationApplicationSettings(ApplicationSettings applicationSettings, IApplicationSettingsQueryModel queryModel)
	{
		applicationSettings.UICulture = queryModel.ApplicationUICulture?.Name;
	}

	public static void PopulateToConfigurationCertificateGroups(Collection<CertificateGroupData> certificateGroupsData, IEnumerable<CertificateGroup> certificateGroups)
	{
		UpdateCollection(certificateGroupsData, certificateGroups, (CertificateGroupData dest, CertificateGroup source) => dest.Id == source.Id, () => new CertificateGroupData(), delegate(CertificateGroupData dest, CertificateGroup source)
		{
			dest.Id = source.Id;
			dest.UseEncryption = source.UseEncryption;
			dest.PresentationImplementationType = source.Presenter.AssemblyQualifiedName;
			dest.PresentationImplementationAssemblyFile = source.Presenter.FileLocation;
			dest.TitleKey = source.TitleKey;
			dest.SubtitleKey = source.SubtitleKey;
			dest.FilterStrings = new FilterStringCollection(source.FilterStrings);
			if (source.SelectedCertificate == null)
			{
				dest.SelectedCertificate = null;
			}
			else
			{
				if (dest.SelectedCertificate == null || dest.SelectedCertificate.Thumbprint != source.SelectedCertificate.Thumbprint)
				{
					dest.SelectedCertificate = new SelectedCertificate();
				}
				dest.SelectedCertificate.FriendlyName = source.SelectedCertificate.FriendlyName;
				dest.SelectedCertificate.IssuerName = source.SelectedCertificate.IssuerName;
				dest.SelectedCertificate.Subject = source.SelectedCertificate.Subject;
				dest.SelectedCertificate.HasPrivateKey = source.SelectedCertificate.HasPrivateKey;
				dest.SelectedCertificate.SubjectName = source.SelectedCertificate.SubjectName;
				dest.SelectedCertificate.ValidFrom = source.SelectedCertificate.ValidFrom;
				dest.SelectedCertificate.ValidUntil = source.SelectedCertificate.ValidUntil;
				dest.SelectedCertificate.Thumbprint = source.SelectedCertificate.Thumbprint;
				dest.SelectedCertificate.CertificateAuthority = source.SelectedCertificate.CertificateAuthority;
				dest.SelectedCertificate.ServerAuthentication = source.SelectedCertificate.ServerAuthentication;
				dest.SelectedCertificate.ClientAuthentication = source.SelectedCertificate.ClientAuthentication;
			}
		});
	}

	public static void PopulateToDataModelServerEndpoints(Collection<ServerEndpointConfiguration> serversCertificateConfigData, ICertificateCmdModel certificateCmdModel)
	{
		foreach (ServerEndpointConfiguration serversCertificateConfigDatum in serversCertificateConfigData)
		{
			certificateCmdModel.AddServerEndpointInstalled(ConfigurationToDataModel(serversCertificateConfigDatum));
		}
	}

	private static VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration ConfigurationToDataModel(ServerEndpointConfiguration serverCertificateConfigData)
	{
		VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration serverEndpointConfiguration = new VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration(serverCertificateConfigData.ServerId, serverCertificateConfigData.DisplayName, serverCertificateConfigData.CertificateGroupId, serverCertificateConfigData.ApplyCertificateRegistrarType, serverCertificateConfigData.RegisterServiceRegistrarType, serverCertificateConfigData.SqlServerRegistrarType);
		serverEndpointConfiguration.SetRegistrarFileLocation(serverCertificateConfigData.RegistrarImplementationLocation);
		return serverEndpointConfiguration;
	}

	public static void PopulateToDataApplicationSettings(ApplicationSettings applicationSettings, ICertificateCmdModel certificateCmdModel)
	{
		CultureInfo applicationUICulture = new CultureInfo("en-US");
		try
		{
			if (applicationSettings != null && applicationSettings.UICulture != null)
			{
				applicationUICulture = new CultureInfo(applicationSettings.UICulture);
			}
		}
		catch (CultureNotFoundException)
		{
		}
		certificateCmdModel.SetApplicationUICulture(applicationUICulture);
	}

	public static void PopulateToDataModelCertificateGroups(Collection<CertificateGroupData> certificateGroupsData, ICertificateCmdModel certificateCmdModel)
	{
		foreach (CertificateGroupData certificateGroupsDatum in certificateGroupsData)
		{
			CertificateGroup certificateGroup = new CertificateGroup(certificateGroupsDatum.Id, certificateGroupsDatum.TitleKey, certificateGroupsDatum.SubtitleKey, certificateGroupsDatum.UseEncryption, certificateGroupsDatum.SelectedCertificate, certificateGroupsDatum.PresentationImplementationType, certificateGroupsDatum.FilterStrings);
			certificateGroup.SetPresenterFileLocation(certificateGroupsDatum.PresentationImplementationAssemblyFile);
			certificateCmdModel.AddCertificateGroup(certificateGroup);
		}
	}

	public static void PopulateToConfigurationServerEndpoints(Collection<ServerEndpointConfiguration> serversCertificateConfigData, IEnumerable<VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration> serverCertificateConfigs)
	{
		UpdateCollection(serversCertificateConfigData, serverCertificateConfigs, (ServerEndpointConfiguration dest, VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration source) => dest.ServerId == source.Id, () => new ServerEndpointConfiguration(), delegate(ServerEndpointConfiguration dest, VideoOS.ServerConfigurator.Interfaces.ServerEndpointConfiguration source)
		{
			dest.ServerId = source.Id;
			dest.DisplayName = source.DisplayName;
			dest.CertificateGroupId = source.CertificateGroupId;
			dest.ApplyCertificateRegistrarType = source.CertificateRegistrar?.AssemblyQualifiedName;
			dest.RegisterServiceRegistrarType = source.ThisServerRegistrar?.AssemblyQualifiedName;
			dest.RegistrarImplementationLocation = source.CertificateRegistrar?.FileLocation ?? source.ThisServerRegistrar?.FileLocation;
			dest.SqlServerRegistrarType = source.SqlServerRegistrar?.AssemblyQualifiedName;
		});
	}

	public static void PopulateToDataModelClientEndpoints(Collection<ClientEndpointConfiguration> clientsRegistrationConfigData, ICertificateCmdModel certificateCmdModel)
	{
		foreach (ClientEndpointConfiguration clientsRegistrationConfigDatum in clientsRegistrationConfigData)
		{
			certificateCmdModel.AddClientEndpointInstalled(ConfigurationToDataModel(clientsRegistrationConfigDatum));
		}
	}

	public static void PopulateToDataModelVolatileItems(ConfigurationData2 configurationData, ICertificateCmdModel certificateCmdModel)
	{
		certificateCmdModel.SetClientEndpointsToBeInstalled(configurationData.VolatileClientsRegistrationConfigData.Select((ClientEndpointConfiguration x) => ConfigurationToDataModel(x)));
		certificateCmdModel.SetServerEndpointsToBeInstalled(configurationData.VolatileServersCertificateConfigData.Select((ServerEndpointConfiguration x) => ConfigurationToDataModel(x)));
		certificateCmdModel.SetConfigurationElementsToBeInstalled(configurationData.VolatileSystemConfigurationElements.Select((SystemConfigurationElement x) => ConfigurationToDataModel(x)));
	}

	private static VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration ConfigurationToDataModel(ClientEndpointConfiguration clientRegistrationConfigData)
	{
		VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration clientEndpointConfiguration = new VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration(clientRegistrationConfigData.ClientId, clientRegistrationConfigData.DisplayName, clientRegistrationConfigData.CertificateGroupDependencyId, clientRegistrationConfigData.RegisterClientRegistrarType, clientRegistrationConfigData.RegisterSetServerAddressImplementationType);
		clientEndpointConfiguration.SetRegistrarFileLocation(clientRegistrationConfigData.RegistrarImplementationLocation);
		return clientEndpointConfiguration;
	}

	public static void PopulateToConfigurationClientEndpoints(Collection<ClientEndpointConfiguration> clientsRegistrationConfigData, IEnumerable<VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration> clientRegistrationConfigs)
	{
		UpdateCollection(clientsRegistrationConfigData, clientRegistrationConfigs, (ClientEndpointConfiguration dest, VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration source) => dest.ClientId == source.Id, () => new ClientEndpointConfiguration(), delegate(ClientEndpointConfiguration dest, VideoOS.ServerConfigurator.Interfaces.ClientEndpointConfiguration source)
		{
			dest.ClientId = source.Id;
			dest.DisplayName = source.DisplayName;
			dest.CertificateGroupDependencyId = source.ServerGroupDependency;
			dest.RegisterClientRegistrarType = source.ClientRegistrar?.AssemblyQualifiedName;
			dest.RegisterSetServerAddressImplementationType = source.ServerAddressRegistrar?.AssemblyQualifiedName;
			dest.RegistrarImplementationLocation = source.ClientRegistrar?.FileLocation ?? source.ServerAddressRegistrar?.FileLocation;
		});
	}

	public static void PopulateToDataModelPreferredSecureState(bool preferredSecureState, ICertificateCmdModel certificateCmdModel)
	{
		certificateCmdModel.SetPreferredSecureState(preferredSecureState);
	}

	public static void PopulateToDataModelSystemConfigurationElements(Collection<SystemConfigurationElement> configurationElements, ICertificateCmdModel certificateCmdModel)
	{
		foreach (SystemConfigurationElement configurationElement2 in configurationElements)
		{
			VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement configurationElement = ConfigurationToDataModel(configurationElement2);
			certificateCmdModel.AddConfigurationElementInstalled(configurationElement);
		}
	}

	private static VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement ConfigurationToDataModel(SystemConfigurationElement sourceElement)
	{
		VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement systemConfigurationElement = new VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement(sourceElement.ConfigurationElementId, sourceElement.DisplayName, sourceElement.SystemFeature.ToDataModel(), sourceElement.Purpose.ToDataModel(), sourceElement.ConfigurationRegistrarType);
		systemConfigurationElement.SetRegistrarFileLocation(sourceElement.RegistrarImplementationLocation);
		return systemConfigurationElement;
	}

	public static void PopulateToConfigurationConfigurationElements(Collection<SystemConfigurationElement> configurationElementsConfigData, IEnumerable<VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement> configurationElementsModel)
	{
		UpdateCollection(configurationElementsConfigData, configurationElementsModel, (SystemConfigurationElement dest, VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement source) => dest.ConfigurationElementId == source.Id, () => new SystemConfigurationElement(), delegate(SystemConfigurationElement dest, VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement source)
		{
			dest.ConfigurationElementId = source.Id;
			dest.DisplayName = source.DisplayName;
			dest.ConfigurationRegistrarType = source.ConfigurationImplementation?.AssemblyQualifiedName;
			dest.RegistrarImplementationLocation = source.ConfigurationImplementation?.FileLocation;
			dest.SystemFeature = source.SystemFeature.FromDataModel();
			dest.Purpose = source.Purpose.FromDataModel();
		});
	}

	public static void PopulateToDataModelManagementServer(ManagementServerAddress managementServerAddress, ICertificateCmdModel certificateCmdModel)
	{
		if (managementServerAddress.FullyQualifiedDomainName != null)
		{
			certificateCmdModel.SetManagementServer(new UriBuilder(Uri.UriSchemeHttp, managementServerAddress.FullyQualifiedDomainName, managementServerAddress.UnsecurePort).Uri, secureState: false);
			certificateCmdModel.SetManagementServer(new UriBuilder(Uri.UriSchemeHttps, managementServerAddress.FullyQualifiedDomainName, managementServerAddress.SecurePort).Uri, secureState: true);
		}
	}

	public static void PopulateToDataModelAuthorizationServer(AuthorizationServerAddress authorizationServerAddress, ICertificateCmdModel certificateCmdModel)
	{
		if (authorizationServerAddress.FullyQualifiedDomainName != null)
		{
			certificateCmdModel.SetAuthorizationServer(new UriBuilder(Uri.UriSchemeHttp, authorizationServerAddress.FullyQualifiedDomainName, authorizationServerAddress.UnsecurePort, authorizationServerAddress.Path).Uri);
			certificateCmdModel.SetAuthorizationServer(new UriBuilder(Uri.UriSchemeHttps, authorizationServerAddress.FullyQualifiedDomainName, authorizationServerAddress.SecurePort, authorizationServerAddress.Path).Uri);
		}
	}

	public static void PopulateToDataModelPersistedHostName(string persistedHostName, ICertificateCmdModel certificateCmdModel)
	{
		certificateCmdModel.SetPersistedHostName(persistedHostName);
	}

	public static void PopulateToConfigurationManagementServer(ConfigurationData2 configurationData, INonvolatileClientRegistrationQueryModel certificateQueryModel)
	{
		configurationData.ManagementServerAddress.FullyQualifiedDomainName = certificateQueryModel.GetManagementServer(secureState: false)?.Host ?? certificateQueryModel.GetManagementServer(secureState: true)?.Host;
		configurationData.ManagementServerAddress.UnsecurePort = certificateQueryModel.GetManagementServer(secureState: false)?.Port ?? 80;
		configurationData.ManagementServerAddress.SecurePort = certificateQueryModel.GetManagementServer(secureState: true)?.Port ?? 443;
	}

	public static void PopulateToConfigurationHostName(ConfigurationData2 configurationData, INonvolatileClientRegistrationQueryModel certificateQueryModel)
	{
		configurationData.HostName = certificateQueryModel.HostName;
	}

	public static void PopulateToConfigurationAuthorizationServer(ConfigurationData2 configurationData, INonvolatileClientRegistrationQueryModel certificateQueryModel)
	{
		configurationData.AuthorizationServerAddress.FullyQualifiedDomainName = certificateQueryModel.GetAuthorizationServer(secureState: false)?.Host ?? certificateQueryModel.GetAuthorizationServer(secureState: true)?.Host;
		configurationData.AuthorizationServerAddress.Path = certificateQueryModel.GetAuthorizationServer(secureState: false)?.AbsolutePath ?? certificateQueryModel.GetAuthorizationServer(secureState: true)?.AbsolutePath;
		configurationData.AuthorizationServerAddress.UnsecurePort = certificateQueryModel.GetAuthorizationServer(secureState: false)?.Port ?? 80;
		configurationData.AuthorizationServerAddress.SecurePort = certificateQueryModel.GetAuthorizationServer(secureState: true)?.Port ?? 443;
	}

	public static void PopulateToDataModelOverrideLocalManagementServer(bool overrideLocalManagementServer, ICertificateCmdModel certificateCmdModel)
	{
		certificateCmdModel.SetOverrideLocalManagementServer(overrideLocalManagementServer);
	}

	public static void PopulateToConfigurationOverrideLocalManagementServer(ConfigurationData2 configurationData, INonvolatileClientRegistrationQueryModel clientRegistrationQueryModel)
	{
		configurationData.OverrideLocalManagementServer = clientRegistrationQueryModel.OverrideLocalManagementServer;
	}

	private static void UpdateCollection<T1, T2>(ICollection<T1> dest, IEnumerable<T2> source, Func<T1, T2, bool> isMatch, Func<T1> create, Action<T1, T2> update)
	{
		foreach (T1 item in dest.Where((T1 destElement) => !source.Any((T2 arg) => isMatch(destElement, arg))).ToList())
		{
			dest.Remove(item);
		}
		foreach (T2 sourceElement in source)
		{
			T1 val = dest.FirstOrDefault((T1 d) => isMatch(d, sourceElement));
			if (object.Equals(val, default(T1)))
			{
				val = create();
				dest.Add(val);
			}
			update(val, sourceElement);
		}
	}

	public static void PopulateToConfigurationPreferredSecureState(ConfigurationData2 configurationData, INonvolatileClientRegistrationQueryModel clientRegistrationQueryModel)
	{
		configurationData.PreferredSecureState = clientRegistrationQueryModel.GetManagementServer()?.Scheme == Uri.UriSchemeHttps;
	}
}

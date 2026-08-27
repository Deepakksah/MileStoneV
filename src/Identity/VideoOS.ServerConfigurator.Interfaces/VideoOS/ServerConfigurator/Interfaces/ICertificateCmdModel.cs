using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface ICertificateCmdModel
{
	void AddServerEndpointInstalled(ServerEndpointConfiguration serverEndpoint);

	void AddClientEndpointInstalled(ClientEndpointConfiguration clientEndpoint);

	void AddConfigurationElementInstalled(SystemConfigurationElement configurationElement);

	void SetServerEndpointsToBeInstalled(IEnumerable<ServerEndpointConfiguration> serverEndpoints);

	void SetClientEndpointsToBeInstalled(IEnumerable<ClientEndpointConfiguration> clientEndpoints);

	void SetConfigurationElementsToBeInstalled(IEnumerable<SystemConfigurationElement> configurationElements);

	void RemoveServerEndpointInstalled(Guid id);

	void RemoveClientEndpointInstalled(Guid id);

	void RemoveConfigurationElementInstalled(Guid id);

	void SetLocationOnEndpointToBeInstalled(Guid endpointId, string location);

	void SetGroupPresenterLocation(Guid certificateGroupId, string location);

	RegistrationResult ApplyCertificatesAndRegisterAllEndpointsInstalled(NetworkCredential credentials);

	RegistrationResult ApplyCertificatesAndRegisterAllEndpointsRelatedToGroup(Guid groupId, NetworkCredential credentials);

	RegistrationResult RegisterAllEndpointsInstalled(NetworkCredential credentials);

	ApplyStatus ApplyCertificatesForEndpointsToBeInstalled(IEnumerable<Guid> serverEndpointIds);

	RegistrationResult ConfigureClientEndpointsToBeInstalled(IEnumerable<Guid> clientEndpointIds, bool offlineRegistration);

	SystemConfigurationStatus CommitConfigurationElementsToBeInstalled(IEnumerable<Guid> configurationElementIds);

	void SetManagementServer(Uri managementServer);

	void SetManagementServer(Uri managementServer, bool secureState);

	void SetOverrideLocalManagementServer(bool value);

	void SetPreferredSecureState(bool secure);

	void SetAuthorizationServer(Uri managementServer);

	void SetAuthorizationServer(DnsEndPoint managementServer, bool secureState);

	void AddCertificateGroup(CertificateGroup certificateGroup);

	void SetUseEncryption(bool newValue, Guid certificateGroupId);

	void SetLockEncryption(bool encryptionValue, bool lockValue, Guid certificateGroupId);

	void RemoveCertificateGroup(Guid id);

	void SetCertificate(string thumbprint, Guid certificateGroupId);

	void SetApplicationUICulture(CultureInfo cultureInfo);

	void SetPersistedHostName(string persistedHostName);
}

using System.Xml;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface ICommonConfigurationProvider
{
	void LoadCertificateConfiguration(ICertificateCmdModel certificateCmdModel);

	void LoadEntireModel(ICertificateCmdModel certificateCmdModel, XmlReader xmlReader);

	void PersistCertificateConfiguration(ICertificateQueryModel certificateQueryModel);

	void PersistEntireModel(ICertificateQueryModel certificateQueryModel, XmlWriter xmlWriter);

	void PersistCertificateConfiguration(IApplicationSettingsQueryModel applicationSettingsQueryModel);

	void PersistCertificateConfiguration(IServerCertificateQueryModel serverCertificateQueryModel);

	void PersistCertificateConfiguration(IClientRegistrationQueryModel clientRegistrationQueryModel);
}

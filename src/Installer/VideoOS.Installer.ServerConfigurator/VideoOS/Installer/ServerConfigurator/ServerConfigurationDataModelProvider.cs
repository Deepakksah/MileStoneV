using System;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Manager.CertificateCommon;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.ServerConfigurator;

public class ServerConfigurationDataModelProvider
{
	private static readonly Lazy<ServerConfigurationDataModelProvider> lazy = new Lazy<ServerConfigurationDataModelProvider>(() => new ServerConfigurationDataModelProvider());

	public static ServerConfigurationDataModelProvider Instance => lazy.Value;

	public ICertificateCmdModel CertificateCmdModel => InstallerManager.Instance.ModelRepository.GetModel<ICertificateCmdModel>();

	public ICertificateQueryModel CertificateQueryModel => InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>();

	private ServerConfigurationDataModelProvider()
	{
		if (!InstallerManager.Instance.ModelRepository.ContainsModelContainer(typeof(CertificateModelContainer)))
		{
			CertificateModelContainer certificateModelContainer = new CertificateModelContainer();
			certificateModelContainer.Load();
			InstallerManager.Instance.ModelRepository.RegisterModel(certificateModelContainer);
		}
	}
}

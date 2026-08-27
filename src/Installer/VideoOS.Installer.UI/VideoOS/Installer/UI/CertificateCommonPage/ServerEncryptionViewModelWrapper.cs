using System;
using System.ComponentModel;
using System.Net;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Manager.CertificateCommon;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.UI.CertificateCommonPage;

public class ServerEncryptionViewModelWrapper
{
	private class CredentialStore : ICredentialsStore
	{
		public NetworkCredential Credentials => null;

		public NetworkCredential PromptForCredentials()
		{
			throw new NotSupportedException();
		}
	}

	public INotifyPropertyChanged ViewModel { get; }

	public ServerEncryptionViewModelWrapper()
	{
		ICertificateQueryModel model = InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>();
		IClientRegistrationQueryModel model2 = InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>();
		ICertificateCmdModel model3 = InstallerManager.Instance.ModelRepository.GetModel<ICertificateCmdModel>();
		ViewModel = new ServerEncryptionViewModel(model, model2, model3, null, new SynchronizationService(), new ServerConfiguratorApplicationResources(), new VideoOS.ServerConfigurator.Common.Activator(), new ServerConfiguratorLogWrapper(), new CredentialStore());
	}

	public ServerEncryptionViewModelWrapper(INotifyPropertyChanged viewModel)
	{
		ViewModel = viewModel;
	}

	public bool Validate()
	{
		return ((ServerEncryptionViewModel)ViewModel).IsServerGroupsToBeInstalledValid;
	}
}

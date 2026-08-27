using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Globalization;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ServerConfiguratorViewModel : INotifyPropertyChanged
{
	private List<TabPageViewModel> _subViewModels = new List<TabPageViewModel>();

	private int _selectedTabIndex;

	public SettingsViewModel SettingsViewModel { get; }

	public ServerEncryptionViewModel ServerEncryptionViewModel { get; }

	public ServerRegistrationViewModel ServerRegistrationViewModel { get; }

	public string ApplicationTitle { get; }

	public ImageSource ApplicationIcon { get; }

	public int SelectedTabIndex
	{
		get
		{
			return _selectedTabIndex;
		}
		set
		{
			if (value >= _subViewModels.Count)
			{
				value = _subViewModels.Count - 1;
			}
			if (_selectedTabIndex != value)
			{
				_selectedTabIndex = value;
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTabIndex"));
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTabViewModel"));
			}
		}
	}

	public IEnumerable<TabPageViewModel> SubViewModels => _subViewModels;

	public TabPageViewModel SelectedTabViewModel => _subViewModels[SelectedTabIndex];

	public event PropertyChangedEventHandler PropertyChanged;

	public ServerConfiguratorViewModel(IApplicationSettingsQueryModel applicationSettingsQueryModel, IServerCertificateQueryModel certificateQueryModel, IClientRegistrationQueryModel registrationQueryModel, ICertificateCmdModel commandModel, ICommonConfigurationProvider configurationProvider, ILogger logger, IApplicationResources applicationResources, ICultureProvider cultureProvider)
	{
		if (applicationResources == null)
		{
			throw new ArgumentNullException("applicationResources");
		}
		ApplicationTitle = applicationResources.ServerConfiguratorApplicationName;
		ApplicationIcon = applicationResources.ServerConfiguratorIcon.GetImageSource(16.0, () => (ImageSource)null);
		SynchronizationService synchronizationService = new SynchronizationService();
		CredentialsStore credentialsStore = new CredentialsStore(applicationResources);
		ServerEncryptionViewModel = new ServerEncryptionViewModel(certificateQueryModel, registrationQueryModel, commandModel, configurationProvider, synchronizationService, applicationResources, new VideoOS.ServerConfigurator.Common.Activator(), logger, credentialsStore);
		_subViewModels.Add(ServerEncryptionViewModel);
		ServerRegistrationViewModel = new ServerRegistrationViewModel(registrationQueryModel, commandModel, configurationProvider, synchronizationService, logger, credentialsStore, applicationResources);
		_subViewModels.Add(ServerRegistrationViewModel);
		SettingsViewModel = new SettingsViewModel(applicationSettingsQueryModel, commandModel, configurationProvider, synchronizationService, cultureProvider);
		_subViewModels.Add(SettingsViewModel);
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ServerEncryptionViewModel : TabPageViewModel
{
	private readonly IServerCertificateQueryModel _queryModel;

	private readonly IClientRegistrationQueryModel _registrationQueryModel;

	private readonly ICertificateCmdModel _commandModel;

	private readonly ICommonConfigurationProvider _configurationProvider;

	private readonly IApplicationResources _applicationResources;

	private readonly IActivator _activator;

	private readonly VideoOS.ServerConfigurator.Interfaces.ILogger _logger;

	private readonly ICredentialsStore _credentialsStore;

	private Dictionary<Guid, IPresenter> _presenterCache = new Dictionary<Guid, IPresenter>();

	private bool _isServerGroupsToBeInstalledValid;

	private CommandHandler _applyCommand;

	public override Visibility Visibility
	{
		get
		{
			if (!_queryModel.ServerEndpointsInstalled.Any())
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
	}

	public ObservableCollection<CertificateGroupViewModel> CertificateGroups { get; } = new ObservableCollection<CertificateGroupViewModel>();

	public ObservableCollection<CertificateGroupViewModel> CertificateGroupsToBeInstalled { get; } = new ObservableCollection<CertificateGroupViewModel>();

	private bool CanApply
	{
		get
		{
			return LocalApplyCommand.CanExecute;
		}
		set
		{
			if (LocalApplyCommand.CanExecute != value)
			{
				LocalApplyCommand.CanExecute = value;
				OnPropertyChanged("CanApply");
			}
		}
	}

	public bool IsServerGroupsToBeInstalledValid
	{
		get
		{
			return _isServerGroupsToBeInstalledValid;
		}
		set
		{
			if (_isServerGroupsToBeInstalledValid != value)
			{
				_isServerGroupsToBeInstalledValid = value;
				OnPropertyChanged("IsServerGroupsToBeInstalledValid");
			}
		}
	}

	public bool LearnMoreAvailable => !string.IsNullOrWhiteSpace(_applicationResources.CertificateGuideUri);

	private CommandHandler LocalApplyCommand => _applyCommand ?? (_applyCommand = new CommandHandler(delegate
	{
		ApplyAction();
	}));

	public ICommand ApplyCommand => LocalApplyCommand;

	public ICommand LearnMoreCommand { get; }

	private IPresenter GetPresenter(Guid id, ImplementationDefinition presenterDefinition)
	{
		if (!_presenterCache.ContainsKey(id))
		{
			IPresenter presenter;
			try
			{
				presenter = _activator.CreateInstance<IPresenter>(presenterDefinition.AssemblyQualifiedName, presenterDefinition.FileLocation, _logger, out var newInstance, DefaultCompatibilityProvider.Instance);
				if (newInstance)
				{
					(presenter as IRequireApplicationResources)?.SetApplicationResources(_applicationResources);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(FormattableString.Invariant($"Could not create presenter Fake presenter is used. Exception: {ex}"));
				presenter = new FakePresenter();
			}
			_presenterCache[id] = presenter;
		}
		return _presenterCache[id];
	}

	public ServerEncryptionViewModel(IServerCertificateQueryModel queryModel, IClientRegistrationQueryModel registrationQueryModel, ICertificateCmdModel commandModel, ICommonConfigurationProvider configurationProvider, ISynchronizationService synchronizationService, IApplicationResources applicationResources, IActivator activator, VideoOS.ServerConfigurator.Interfaces.ILogger logger, ICredentialsStore credentialsStore)
		: base(synchronizationService)
	{
		_queryModel = queryModel ?? throw new ArgumentNullException("queryModel");
		_registrationQueryModel = registrationQueryModel ?? throw new ArgumentNullException("registrationQueryModel");
		_commandModel = commandModel ?? throw new ArgumentNullException("commandModel");
		_applicationResources = applicationResources ?? throw new ArgumentNullException("applicationResources");
		_configurationProvider = configurationProvider;
		_activator = activator ?? throw new ArgumentNullException("activator");
		_logger = logger ?? throw new ArgumentNullException("logger");
		_credentialsStore = credentialsStore ?? throw new ArgumentNullException("credentialsStore");
		_queryModel.CertificateGroupsModified += QueryModel_CertificateGroupsModified;
		_queryModel.ServerEndpointsModified += QueryModel_ServerEndpointsModified;
		UpdateViewModel();
		LearnMoreCommand = new CommandHandler(delegate
		{
			LearnMoreAction();
		});
	}

	public override void ShowErrorOrWarning(int errorMessageCode)
	{
		RegistrationResult registrationResult = RegistrationResult.CreateRegistrationResult(errorMessageCode);
		if (registrationResult.ResultType != RegistrationResultType.Success)
		{
			ShowStatus(registrationResult);
		}
	}

	public void ShowStatus(RegistrationResult registrationResult)
	{
		if (registrationResult == null)
		{
			throw new ArgumentNullException("registrationResult");
		}
		if (registrationResult.ResultType.SuccessOrWarning())
		{
			base.SuccessMessage = StatusMessageProvider.GetMessage(registrationResult, ApplyStatus.Success, RegistrationResultType.Success, _logger, MessageSource.Encryption);
			base.WarningMessage = StatusMessageProvider.GetMessage(registrationResult, null, RegistrationResultType.Warning, _logger, MessageSource.Encryption);
		}
		else
		{
			base.ErrorMessage = StatusMessageProvider.GetMessage(registrationResult, null, RegistrationResultType.Error, _logger, MessageSource.Encryption);
		}
	}

	private void QueryModel_ServerEndpointsModified(object sender, EventArgs e)
	{
		SafeCall(UpdateViewModel);
	}

	private void QueryModel_CertificateGroupsModified(object sender, EventArgs e)
	{
		SafeCall(UpdateViewModel);
	}

	private void UpdateViewModel()
	{
		List<CertificateGroupViewModel> list = new List<CertificateGroupViewModel>();
		List<CertificateGroupViewModel> list2 = new List<CertificateGroupViewModel>();
		foreach (CertificateGroup certificateGroup in _queryModel.CertificateGroups ?? new List<CertificateGroup>())
		{
			IEnumerable<ServerEndpointConfiguration> servers = _queryModel.ServerEndpointsInstalled?.Where((ServerEndpointConfiguration x) => x.CertificateGroupId == certificateGroup.Id) ?? new List<ServerEndpointConfiguration>();
			IEnumerable<ServerEndpointConfiguration> servers2 = _queryModel.ServerEndpointsToBeInstalled?.Where((ServerEndpointConfiguration x) => x.CertificateGroupId == certificateGroup.Id) ?? new List<ServerEndpointConfiguration>();
			AddCertificateGroup(list, certificateGroup, servers);
			AddCertificateGroup(list2, certificateGroup, servers2);
		}
		CertificateGroups.UpdateKeyedItems(list, (CertificateGroupViewModel vm) => vm.GroupId, delegate(CertificateGroupViewModel current, CertificateGroupViewModel updated)
		{
			current.Update(updated);
		});
		CertificateGroupsToBeInstalled.UpdateKeyedItems(list2, (CertificateGroupViewModel vm) => vm.GroupId, delegate(CertificateGroupViewModel current, CertificateGroupViewModel updated)
		{
			current.Update(updated);
		});
		CanApply = CertificateGroups.All((CertificateGroupViewModel x) => x.CertificateIsValid);
		IsServerGroupsToBeInstalledValid = CertificateGroupsToBeInstalled.All((CertificateGroupViewModel x) => x.CertificateIsValid);
	}

	private void AddCertificateGroup(List<CertificateGroupViewModel> certificateGroups, CertificateGroup certificateGroup, IEnumerable<ServerEndpointConfiguration> servers)
	{
		if (servers.Any())
		{
			CertificateViewModel selectedCertificate = new CertificateViewModel(certificateGroup.SelectedCertificate);
			IEnumerable<CertificateViewModel> availableCertificates = GetAvailableCertificates(certificateGroup, selectedCertificate);
			IPresenter presenter = GetPresenter(certificateGroup.Id, certificateGroup.Presenter);
			string title = presenter.LocalizedString(certificateGroup.TitleKey, CultureInfo.CurrentUICulture);
			string subtitle = presenter.LocalizedString(certificateGroup.SubtitleKey, CultureInfo.CurrentUICulture);
			CertificateGroupViewModel item = new CertificateGroupViewModel(title, subtitle, _commandModel, certificateGroup.Id, certificateGroup.UseEncryption, servers.Select((ServerEndpointConfiguration x) => new ServerViewModel(x.DisplayName, x.Id)), availableCertificates, selectedCertificate, certificateGroup.SelectedCertificateIsValid || !certificateGroup.UseEncryption, certificateGroup.Locked, certificateGroup.LockEncryption);
			certificateGroups.Add(item);
		}
	}

	private static IEnumerable<CertificateViewModel> GetAvailableCertificates(CertificateGroup certificateGroup, CertificateViewModel selectedCertificate)
	{
		IEnumerable<CertificateViewModel> enumerable = certificateGroup.ValidCertificates?.Select((ICertificate x) => new CertificateViewModel(x));
		CertificateViewModel certificateViewModel = new CertificateViewModel(null);
		IEnumerable<CertificateViewModel> enumerable3;
		if (enumerable == null)
		{
			IEnumerable<CertificateViewModel> enumerable2 = new CertificateViewModel[1] { certificateViewModel };
			enumerable3 = enumerable2;
		}
		else
		{
			enumerable3 = new CertificateViewModel[1] { certificateViewModel }.Concat(enumerable);
		}
		enumerable = enumerable3;
		if (enumerable == null)
		{
			enumerable = new List<CertificateViewModel> { selectedCertificate };
		}
		else if (!enumerable.Any((CertificateViewModel x) => x == selectedCertificate))
		{
			enumerable = enumerable.Concat(new List<CertificateViewModel> { selectedCertificate });
		}
		return enumerable;
	}

	private async void ApplyAction()
	{
		if (!CanApply)
		{
			return;
		}
		base.ErrorMessage = string.Empty;
		base.SuccessMessage = string.Empty;
		base.WarningMessage = string.Empty;
		string message = string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_ApplyCertificate_Text, _applicationResources.ProductNameWithoutCompany);
		MessageBoxViewModel viewModel = new MessageBoxViewModel(Resources.MessageBox_ApplyCertificates_Title, message, Resources.MessageBox_Apply, Resources.MessageBox_Cancel, isWarning: false);
		if (await ShowMessage(viewModel) != MessageBoxResult.OK)
		{
			return;
		}
		try
		{
			_logger.LogInfo("Applying certificates started");
			base.IsBusy = true;
			RegistrationResult applyResult = null;
			NetworkCredential credentials = _credentialsStore.Credentials;
			await Task.Run(delegate
			{
				applyResult = _commandModel.ApplyCertificatesAndRegisterAllEndpointsInstalled(credentials);
				if (applyResult.ResultType.SuccessOrWarning())
				{
					_configurationProvider.PersistCertificateConfiguration(_queryModel);
					_configurationProvider.PersistCertificateConfiguration(_registrationQueryModel);
				}
			});
			_logger.LogInfo(FormattableString.Invariant($"Applying certificates done with result = {applyResult}"));
			ShowStatus(applyResult);
		}
		finally
		{
			base.IsBusy = false;
		}
	}

	private void LearnMoreAction()
	{
		Process.Start(_applicationResources.CertificateGuideUri);
	}
}

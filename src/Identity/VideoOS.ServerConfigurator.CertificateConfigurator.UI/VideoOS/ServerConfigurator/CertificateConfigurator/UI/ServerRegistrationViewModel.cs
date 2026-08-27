using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ServerRegistrationViewModel : TabPageViewModel
{
	private readonly IClientRegistrationQueryModel _queryModel;

	private readonly ICertificateCmdModel _commandModel;

	private readonly ICommonConfigurationProvider _configurationProvider;

	private readonly ILogger _logger;

	private readonly ICredentialsStore _credentialsStore;

	private string _managementServerAddress;

	private readonly IApplicationResources _applicationResources;

	public string ManagementServerAddress
	{
		get
		{
			return _managementServerAddress;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetProperty(ref _managementServerAddress, value, "ManagementServerAddress");
			Uri uri = new Uri(value);
			bool isDefaultPort = uri.IsDefaultPort;
			bool? serverGroupSecureState = _queryModel.GetServerGroupSecureState();
			string text = ((!serverGroupSecureState.HasValue) ? uri.Scheme : (serverGroupSecureState.Value ? Uri.UriSchemeHttps : Uri.UriSchemeHttp));
			if (!serverGroupSecureState.HasValue)
			{
				_commandModel.SetPreferredSecureState(uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
			}
			int portNumber = (isDefaultPort ? new UriBuilder(text, "localhost").Uri.Port : uri.Port);
			Uri managementServer = _queryModel.GetManagementServer();
			Uri uri2 = new UriBuilder(text, uri.Host, portNumber).Uri;
			if (uri2 != managementServer)
			{
				_commandModel.SetManagementServer(uri2);
				_commandModel.SetPreferredSecureState(uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
			}
		}
	}

	public bool UseRemoteManagementServer
	{
		get
		{
			return !_queryModel.UseLocalManagementServer();
		}
		set
		{
			_commandModel.SetOverrideLocalManagementServer(value);
			OnPropertyChanged("UseRemoteManagementServer");
		}
	}

	public bool IsLocalManagementServer => _queryModel.IsLocalManagementServerPresent();

	public static string RegisterTabText => Resources.RegisterTabText;

	public ICommand RegisterCommand { get; }

	public ICommand UseRemoteManagementServerCommand { get; }

	public bool LearnMoreAvailable => !string.IsNullOrWhiteSpace(_applicationResources.RegisterTabLearnMoreLink);

	public ICommand LearnMoreCommand { get; }

	public ServerRegistrationViewModel(IClientRegistrationQueryModel queryModel, ICertificateCmdModel commandModel, ICommonConfigurationProvider configurationProvider, ISynchronizationService synchronizationService, ILogger logger, ICredentialsStore credentialsStore, IApplicationResources applicationResources)
		: base(synchronizationService)
	{
		_queryModel = queryModel ?? throw new ArgumentNullException("queryModel");
		_commandModel = commandModel ?? throw new ArgumentNullException("commandModel");
		_configurationProvider = configurationProvider ?? throw new ArgumentNullException("configurationProvider");
		_logger = logger ?? throw new ArgumentNullException("logger");
		_credentialsStore = credentialsStore ?? throw new ArgumentNullException("credentialsStore");
		_applicationResources = applicationResources ?? throw new ArgumentNullException("applicationResources");
		RegisterCommand = new CommandHandler(delegate
		{
			RegisterAction();
		});
		UseRemoteManagementServerCommand = new CommandHandler(delegate
		{
			UseRemoteManagementServerAction();
		});
		_queryModel.ClientRegistrationServerModified += delegate
		{
			SafeCall(UpdateManagementServerAddress);
		};
		UpdateManagementServerAddress();
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
			base.SuccessMessage = StatusMessageProvider.GetMessage(registrationResult, RegisterClientStatus.Success, RegistrationResultType.Success, _logger, MessageSource.Registration);
			base.WarningMessage = StatusMessageProvider.GetMessage(registrationResult, null, RegistrationResultType.Warning, _logger, MessageSource.Registration);
		}
		else
		{
			base.ErrorMessage = StatusMessageProvider.GetMessage(registrationResult, null, RegistrationResultType.Error, _logger, MessageSource.Registration);
		}
	}

	private void UpdateManagementServerAddress()
	{
		ManagementServerAddress = _queryModel.GetManagementServer().ToString();
		OnPropertyChanged("ManagementServerAddress");
	}

	private async void UseRemoteManagementServerAction()
	{
		if (UseRemoteManagementServer)
		{
			UseRemoteManagementServer = false;
			string message = string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_ForceEditHostname_Text, _applicationResources.ProductNameWithoutCompany);
			MessageBoxViewModel viewModel = new MessageBoxViewModel(Resources.MessageBox_ForceEditHostname_Title, message, Resources.MessageBox_Change, Resources.MessageBox_Cancel, isWarning: true);
			if (await ShowMessage(viewModel) == MessageBoxResult.OK)
			{
				_logger.LogInfo("Edit local Management Server address confirmed");
				UseRemoteManagementServer = true;
			}
		}
		else
		{
			_logger.LogInfo($"Revert local Management Server address to original value: {_queryModel.GetManagementServer().ToString()}");
		}
		UpdateManagementServerAddress();
	}

	private async void RegisterAction()
	{
		base.ErrorMessage = string.Empty;
		base.SuccessMessage = string.Empty;
		base.WarningMessage = string.Empty;
		string message = string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_RegisterServers_Text, _applicationResources.ProductNameWithoutCompany);
		MessageBoxViewModel viewModel = new MessageBoxViewModel(Resources.MessageBox_RegisterServers_Title, message, Resources.MessageBox_Register, Resources.MessageBox_Cancel, isWarning: false);
		if (await ShowMessage(viewModel) != MessageBoxResult.OK)
		{
			return;
		}
		RegistrationResult registerResult = null;
		base.IsBusy = true;
		try
		{
			_logger.LogInfo("Registration started");
			NetworkCredential credentials = _credentialsStore.Credentials;
			await Task.Run(() => registerResult = _commandModel.RegisterAllEndpointsInstalled(credentials));
			registerResult.GetResult<RegisterClientStatus>();
			registerResult.GetResult<SetServerAddressStatus>();
			registerResult.GetResult<SystemConfigurationStatus>();
			if (registerResult.GetResult<RegisterClientStatus>() == RegisterClientStatus.InsufficientCredentials || registerResult.GetResult<SetServerAddressStatus>() == SetServerAddressStatus.InsufficientCredentials || registerResult.GetResult<RegisterServiceStatus>() == RegisterServiceStatus.InsufficientCredentials || registerResult.GetResult<SystemConfigurationStatus>() == SystemConfigurationStatus.InsufficientCredentials)
			{
				credentials = _credentialsStore.PromptForCredentials();
				if (credentials != null)
				{
					await Task.Run(() => registerResult = _commandModel.RegisterAllEndpointsInstalled(credentials));
				}
			}
			if (registerResult.ResultType.SuccessOrWarning())
			{
				await Task.Run(delegate
				{
					_configurationProvider.PersistCertificateConfiguration(_queryModel);
				});
				UpdateManagementServerAddress();
			}
			_logger.LogInfo(FormattableString.Invariant($"Registration done with result = {registerResult}"));
			ShowStatus(registerResult);
		}
		finally
		{
			base.IsBusy = false;
		}
	}

	private void LearnMoreAction()
	{
		Process.Start(_applicationResources.RegisterTabLearnMoreLink);
	}
}

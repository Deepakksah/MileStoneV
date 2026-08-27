using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Manager.CertificateCommon;
using VideoOS.Installer.UI.CertificateCommonPage;
using VideoOS.Installer.UI.Framework;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.UI;

public class WizardPageCertificate : WizardPageSubscription<CertificatePageContent, INotifyPropertyChanged>
{
	private CertificateSetup _certificateSetup;

	private Dictionary<string, string> _parameters;

	private Container<bool?> _initialDefinedSecureState;

	private IContainer components;

	protected override IEnumerable<IWizardCommonPageSubscription<CertificatePageContent>> Subscriptions => InstallerManager.Instance.SubscriptionPlugins.CertificateSubscriptions;

	public WizardPageCertificate()
		: this(new CertificateSetup())
	{
	}

	private WizardPageCertificate(CertificateSetup certificateSetup)
		: base("CertificateCommonPage", (System.Windows.Controls.UserControl)certificateSetup)
	{
		_certificateSetup = certificateSetup;
		InitializeComponent();
	}

	public override bool OnBeforeActivate(Dictionary<string, string> parameters)
	{
		_parameters = parameters;
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.ContainsKey("HARDWARE-MANUFACTURER-MODE") && bool.Parse(parameters["HARDWARE-MANUFACTURER-MODE"]))
		{
			return false;
		}
		if (base.OnBeforeActivate(parameters))
		{
			List<CertificatePageContent> list = (from y in Subscriptions
				where y.BeforeActivate(parameters)
				select y.GetContent()).ToList();
			_initialDefinedSecureState = _initialDefinedSecureState ?? new Container<bool?>(LockEncryptionValue(parameters));
			CertificateModelInitializer.Initialize(definedSecureAddress: (parameters.GetValueAsNullableBoolean("ISCORPORATEINSTALLER") == true) ? _initialDefinedSecureState.Value : LockEncryptionValue(parameters), content: list, isQuietMode: false, isUpgrade: GlobalData.GetValueAsBoolean("IS_UPGRADE"), parameters: parameters);
			return !list.All((CertificatePageContent c) => c.ServerEndpoints.All((ServerEndpoint se) => se.CertificateGroupId == Guid.Empty));
		}
		return false;
	}

	private static bool? LockEncryptionValue(Dictionary<string, string> parameters)
	{
		if (parameters.TryGetValue("MANAGEMENTSERVERURI", out var value) && Uri.TryCreate(value, UriKind.Absolute, out var result))
		{
			return result.Scheme == Uri.UriSchemeHttps;
		}
		return null;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = base.CollectParameters();
		if (GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER"))
		{
			return dictionary;
		}
		UriBuilder uriBuilder = ((!_parameters.ContainsKey("MANAGEMENTSERVERURI") || string.IsNullOrWhiteSpace(_parameters["MANAGEMENTSERVERURI"])) ? new UriBuilder(_parameters["MANAGEMENTAPPNAMESECURE"]) : new UriBuilder(_parameters["MANAGEMENTSERVERURI"]));
		uriBuilder.Scheme = ((InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>().GetSecureState() == true) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
		uriBuilder.Port = ((InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>().GetSecureState() == true) ? new UriBuilder(_parameters["MANAGEMENTAPPNAMESECURE"]).Uri.Port : new UriBuilder(_parameters["MANAGEMENTAPPNAME"]).Uri.Port);
		dictionary["MANAGEMENTSERVERURI"] = uriBuilder.Uri.AbsoluteUri;
		if (!_parameters.TryGetValue("IDPADDRESS", out var value) || string.IsNullOrEmpty(value))
		{
			dictionary["IDPADDRESS"] = new UriBuilder(new Uri(dictionary["MANAGEMENTSERVERURI"]))
			{
				Path = "IDP"
			}.Uri.ToString();
		}
		return dictionary;
	}

	protected override INotifyPropertyChanged CreateViewModel(IEnumerable<CertificatePageContent> content)
	{
		INotifyPropertyChanged viewModel = new ServerEncryptionViewModelWrapper().ViewModel;
		viewModel.PropertyChanged += delegate
		{
			TriggerValidation();
		};
		System.Windows.Controls.UserControl userControl = new ServerEncryptionControl();
		_certificateSetup.Content = userControl;
		userControl.DataContext = viewModel;
		return viewModel;
	}

	protected override bool ValidateViewModel(INotifyPropertyChanged viewModel)
	{
		return new ServerEncryptionViewModelWrapper(viewModel).Validate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}
}

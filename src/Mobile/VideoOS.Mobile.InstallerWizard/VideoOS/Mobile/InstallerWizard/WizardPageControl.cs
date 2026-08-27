using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Mobile.Configuration;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.InstallerWizard.Utilities;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.InstallerWizard;

public class WizardPageControl : WizardPageBase
{
	private delegate void BeginUriCheckDelegate(string hostname, string username, string password, bool isUserAccount, bool isBasic);

	private static readonly string[] ValidSchemes = new string[2]
	{
		Uri.UriSchemeHttp,
		Uri.UriSchemeHttps
	};

	private static readonly Dictionary<LoginCheck.LoginCheckResult, string> LoginCheckStrings = new Dictionary<LoginCheck.LoginCheckResult, string>
	{
		[LoginCheck.LoginCheckResult.InvalidServerUriFormat] = Strings.INVALIDURI,
		[LoginCheck.LoginCheckResult.InvalidusernameFormat] = Strings.INVALIDUSERNAME,
		[LoginCheck.LoginCheckResult.LoginFailedServerNotFound] = Strings.SERVERNOTFOUND,
		[LoginCheck.LoginCheckResult.LoginFailedInvalidUser] = Strings.COULDNOTLOGIN,
		[LoginCheck.LoginCheckResult.LoginSuccess] = string.Empty,
		[LoginCheck.LoginCheckResult.RequireAdministrativeRights] = Strings.REQUIREADMINISTRATIVERIGHTS
	};

	private Dictionary<string, string> _parameters = new Dictionary<string, string>();

	private InstallerConfiguration _installerConfig = new InstallerConfiguration();

	private ManagementServerInfo _managementServerInfo;

	private bool _serverUrlLocked;

	private const string HttpPortDefaultValue = "8081";

	private const string HttpSecurePortDefaultValue = "8082";

	private const string InstallationModeCustom = "CustomMode";

	private string result = "";

	private bool completed;

	private bool readyToContinue;

	private InternalConfigBase _internalConfig;

	private string _installationPath;

	private BeginUriCheckDelegate _uriCheckDelegate;

	private IRegistryHelper registryInstance;

	private IContainer components;

	private Label labelTitle;

	private Label label2;

	private TextBox textBoxURI;

	private Label label5;

	private Label label6;

	private Label labelError;

	private Label labelPorts;

	private Label labelPort1;

	private Label labelPort2;

	private TextBox textBox1;

	private TextBox textBox2;

	private TextBox textBoxHTTPPort;

	private TextBox textBoxHttpSecurePort;

	private Panel panelPorts;

	private PictureBox errorIcon;

	public IWizardPages WizardPagesParent { get; set; }

	public WizardPageControl()
	{
		InitializeComponent();
		labelError.Text = "";
		errorIcon.Visible = false;
		IRegistryProvider registryProvider = new VideoOS.Mobile.InstallerWizard.Utilities.RegistryHelper();
		registryInstance = registryProvider.GetRegistryInstance();
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
		PageName = "ServerPage";
		WizardPage.PageActivated += WizardPage_PageActivated;
		try
		{
			ReadInitialValuesFromConfig();
		}
		catch (Exception)
		{
		}
		InitializeDefaultPortValues();
	}

	private void InitializeDefaultPortValues()
	{
		textBoxHTTPPort.Text = "8081";
		textBoxHttpSecurePort.Text = "8082";
		if (registryInstance.HasValue("InstallationFolder", (string data) => File.Exists(data)))
		{
			_installationPath = registryInstance.Read("InstallationFolder");
		}
		ReadInitialPortValuesFromRegistry();
		ReadInitialPortValuesFromConfig();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		readyToContinue = false;
		labelError.Text = string.Empty;
		errorIcon.Visible = false;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Strings.Culture = e.Culture;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters.ContainsKey("INSTALLATION_MODE") && parameters["INSTALLATION_MODE"].Equals("CustomMode"))
		{
			panelPorts.Visible = true;
			labelTitle.Text = Strings.WizardPageControl_LabelTitle_CustomMode;
		}
		else
		{
			panelPorts.Visible = false;
			labelTitle.Text = Strings.WizardPageControl_LabelTitle_TypicalMode;
		}
		string text = null;
		if (parameters.ContainsKey("MANAGEMENTSERVERURI") && HostHelper.ValidateAddress(parameters["MANAGEMENTSERVERURI"], out var _))
		{
			text = parameters["MANAGEMENTSERVERURI"];
		}
		if (text != null)
		{
			textBoxURI.Text = text;
		}
		_serverUrlLocked = parameters.TryGetValue("LOCKMANAGEMENTSERVER", out var value) && value == "true";
		textBoxURI.Enabled = !_serverUrlLocked;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Uri uri = new Uri(textBoxURI.Text);
		_parameters["CALLER"] = ParametersKeys.CallerWizardId.ToString();
		_parameters["MANAGEMENTSERVERURI"] = uri.AbsoluteUri;
		_parameters["MANAGEMENTAPPNAME"] = _managementServerInfo?.HttpAddress.AbsoluteUri;
		_parameters["MANAGEMENTAPPNAMESECURE"] = _managementServerInfo?.HttpsAddress.AbsoluteUri;
		_parameters["IDPADDRESS"] = _managementServerInfo?.IDPAddress.AbsoluteUri;
		_parameters["INSTALLERCONFIGURATION"] = HttpUtility.HtmlEncode(_installerConfig.ToString());
		_parameters["HTTPPORT"] = textBoxHTTPPort.Text.Trim();
		_parameters["HTTPSECUREPORT"] = textBoxHttpSecurePort.Text.Trim();
		return _parameters;
	}

	public override bool OnBeforeContinue()
	{
		if (readyToContinue)
		{
			return true;
		}
		if (!ValidatePorts(out var errorMessage))
		{
			labelError.Text = errorMessage;
			errorIcon.Visible = true;
			return false;
		}
		labelError.Text = Strings.VALIDATING;
		errorIcon.Visible = false;
		Wizard.NextButton.Enabled = false;
		Wizard.BackButton.Enabled = false;
		completed = false;
		result = "";
		bool valueAsBoolean = GlobalData.GetValueAsBoolean("USERACCOUNT");
		GlobalData.TryGetValue("SERVICEACCOUNT", out var value);
		GlobalData.TryGetValue("ENCRYPTEDPASSWORD", out var value2);
		GlobalData.TryGetValue("MOSVMSUSEBASICUSER", out var value3);
		bool flag = string.Equals(value3, bool.TrueString, StringComparison.OrdinalIgnoreCase);
		if (flag)
		{
			GlobalData.TryGetValue("MOSVMSUSER", out value);
			GlobalData.TryGetValue("MOSVMSPASSWORD", out value2);
			value2 = EncryptHelper.DecryptPassword(value2, unicodePassword: true);
		}
		SynchronizationContext current = SynchronizationContext.Current;
		_uriCheckDelegate = CheckUriThread;
		_uriCheckDelegate.BeginInvoke(textBoxURI.Text, value, value2, valueAsBoolean, flag, LanguageUncabEnded, current);
		return false;
	}

	private bool ValidatePorts(out string errorMessage)
	{
		errorMessage = string.Empty;
		if (!VideoOS.Mobile.Helpers.PortHelper.IsPortValid(textBoxHTTPPort.Text))
		{
			errorMessage = Strings.INVALIDHTTPPORT;
			return false;
		}
		if (!VideoOS.Mobile.Helpers.PortHelper.IsPortValid(textBoxHttpSecurePort.Text))
		{
			errorMessage = Strings.INVALIDHTTPSECUREPORT;
			return false;
		}
		if (textBoxHTTPPort.Text == textBoxHttpSecurePort.Text)
		{
			errorMessage = Strings.CANNOTBESAME;
			return false;
		}
		return true;
	}

	private void CheckUriThread(string uriString, string username, string password, bool isUserAccount, bool isBasic)
	{
		Uri uri = null;
		Uri identityServerAddress = null;
		result = "";
		if (!Uri.TryCreate(uriString, UriKind.Absolute, out uri) || !ValidSchemes.Contains(uri.Scheme))
		{
			result = Strings.INVALIDURI;
			completed = true;
		}
		if (!completed)
		{
			try
			{
				if (new ServerHelper().TryGetManagementServerInfo(uri, out _managementServerInfo))
				{
					if (uri.Scheme != _managementServerInfo.IDPAddress.Scheme)
					{
						if (_managementServerInfo.IDPAddress.Scheme == Uri.UriSchemeHttp)
						{
							result = Resources.ErrorMessage_ServerIsRunningUnsecure;
						}
						else
						{
							result = Resources.ErrorMessage_ServerIsRunningSecure;
						}
						completed = true;
					}
					identityServerAddress = new Uri(_managementServerInfo.IDPAddress.AbsoluteUri);
				}
				else
				{
					result = Strings.Temp_SERVERNOTFOUND;
					completed = true;
				}
			}
			catch (ServerHelper.ServerException ex)
			{
				result = ex.Message;
				completed = true;
			}
		}
		if (!completed && isBasic)
		{
			LoginCheck.LoginCheckResult key = LoginCheck.CheckBasicLogin(uriString, username, password);
			result = LoginCheckStrings[key];
			completed = true;
		}
		if (!completed)
		{
			string text = ((!isUserAccount) ? BuiltinUserLoginCheck(uri, identityServerAddress, username, null) : Validate(uri, identityServerAddress, username, EncryptHelper.DecryptPassword(password, unicodePassword: true)));
			if (text != "")
			{
				result = text;
				completed = true;
			}
		}
		completed = true;
	}

	private string BuiltinUserLoginCheck(Uri managementServerAddress, Uri identityServerAddress, string username, SecureString password)
	{
		string text = string.Empty;
		try
		{
			SystemImpersonsation.ImpersonateSystem();
			using Impersonation impersonation = new Impersonation(username, null, password, LogonType.Service);
			impersonation.Impersonate();
			text = Validate(managementServerAddress, identityServerAddress, "", "");
		}
		catch (Exception ex)
		{
			text = "Error checking built in user credentials! DETAILS: " + ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
		}
		finally
		{
			SystemImpersonsation.StopImpersonation();
		}
		return text;
	}

	private void ShowResultMainThread(object state)
	{
		if (completed)
		{
			labelError.Text = result;
			if (!string.IsNullOrWhiteSpace(result))
			{
				errorIcon.Visible = true;
			}
		}
		Wizard.NextButton.Enabled = true;
		Wizard.BackButton.Enabled = true;
		if (result == "")
		{
			readyToContinue = true;
			Wizard.MoveToNextPage();
		}
	}

	private void LanguageUncabEnded(IAsyncResult ar)
	{
		try
		{
			if (_uriCheckDelegate != null)
			{
				_uriCheckDelegate.EndInvoke(ar);
			}
			((SynchronizationContext)ar.AsyncState).Send(ShowResultMainThread, null);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void ReadInitialValuesFromConfig()
	{
		ReadInitialValuesFromRegistry();
		if (registryInstance.HasValue("InstallationFolder", (string data) => File.Exists(data)))
		{
			string path = registryInstance.Read("InstallationFolder");
			string text = (from x in Directory.GetFiles(searchPattern: Path.GetFileNameWithoutExtension(path) + "*.config", path: Path.GetDirectoryName(path))
				orderby x
				select x).First();
			InternalConfigBase internalConfigBase = new InternalConfigBase(text);
			internalConfigBase.InitFromConfigurationFile();
			string scheme = internalConfigBase.ManagementServer.Address.Scheme;
			string iP = internalConfigBase.ManagementServer.Address.IP;
			int port = internalConfigBase.ManagementServer.Address.Port;
			string text2 = "";
			if (!string.IsNullOrWhiteSpace(iP) && port != 0)
			{
				text2 = new UriBuilder(scheme, iP, port).Uri.AbsoluteUri;
			}
			textBoxURI.Text = text2;
			CustomActionsCommon.LoadInstallerConfigFromExeConfig(_installerConfig, text);
		}
		_installerConfig.ServerId = ((_installerConfig.ServerId == null || _installerConfig.ServerId == Guid.Empty.ToString()) ? Guid.NewGuid().ToString() : _installerConfig.ServerId);
		_installerConfig.ServerIdS = ((_installerConfig.ServerIdS == null || _installerConfig.ServerIdS == Guid.Empty.ToString()) ? Guid.NewGuid().ToString() : _installerConfig.ServerIdS);
		_installerConfig.ServerIdT = ((_installerConfig.ServerIdT == null || _installerConfig.ServerIdT == Guid.Empty.ToString()) ? Guid.NewGuid().ToString() : _installerConfig.ServerIdT);
	}

	private void ReadInitialValuesFromRegistry()
	{
		registryInstance.InitTextBox(textBoxURI, "SurveillanceName");
		registryInstance.InitTextBox(textBoxURI, "SurveillancePort");
		_installerConfig.LoggingFilePath = SetLoggingPathFromRegistryIfNotDefaultValue(_installerConfig.LoggingFilePath);
		string text = registryInstance.InitValue(string.Empty, "OSMADDRESSES");
		_installerConfig.OsmAddresses = (from el in text.Split('|')
			where !string.IsNullOrWhiteSpace(el)
			select el).ToList();
		_installerConfig.ServerId = registryInstance.InitValue(_installerConfig.ServerId, "SERVERID");
		_installerConfig.ServerIdS = registryInstance.InitValue(_installerConfig.ServerIdS, "SERVERIDS");
		_installerConfig.ServerIdT = registryInstance.InitValue(_installerConfig.ServerIdT, "SERVERIDT");
		_installerConfig.ExportsFolder = registryInstance.InitValue(_installerConfig.ExportsFolder, "ExportFolder");
		_installerConfig.CertificationPassword = registryInstance.InitValue(_installerConfig.CertificationPassword, "Certificate");
	}

	private string SetLoggingPathFromRegistryIfNotDefaultValue(string initValue)
	{
		string text = registryInstance.InitValue(initValue, "LogFolder");
		if (!string.IsNullOrEmpty(text) && Regex.IsMatch(text, ".*\\\\Milestone Mobile Server\\\\Logs"))
		{
			return null;
		}
		return text;
	}

	private void ReadInitialPortValuesFromConfig()
	{
		if (!string.IsNullOrWhiteSpace(_installationPath))
		{
			_internalConfig = new InternalConfigBase(_installationPath);
			_internalConfig.InitFromConfigurationFile();
			if (VideoOS.Mobile.Helpers.PortHelper.IsPortValid(_internalConfig.HttpMetaChannel.Address.Port))
			{
				textBoxHTTPPort.Text = _internalConfig.HttpMetaChannel.Address.Port.ToString();
			}
			if (VideoOS.Mobile.Helpers.PortHelper.IsPortValid(_internalConfig.HttpSecureMetaChannel.Address.Port))
			{
				textBoxHttpSecurePort.Text = _internalConfig.HttpSecureMetaChannel.Address.Port.ToString();
			}
		}
	}

	private void ReadInitialPortValuesFromRegistry()
	{
		registryInstance.InitTextBox(textBoxHTTPPort, "PortHttp");
		if (!VideoOS.Mobile.Helpers.PortHelper.IsPortValid(textBoxHTTPPort.Text))
		{
			textBoxHTTPPort.Text = "8081";
		}
		registryInstance.InitTextBox(textBoxHttpSecurePort, "PortHttpSecured");
		if (!VideoOS.Mobile.Helpers.PortHelper.IsPortValid(textBoxHttpSecurePort.Text))
		{
			textBoxHttpSecurePort.Text = "8082";
		}
	}

	private string Validate(Uri managementServerAddress, Uri identityServerAddress, string username, string password)
	{
		try
		{
			ICredentials credentials = ServerHelper.ResolveCredentials(username, password);
			if (ServerHelper.TryLoginToCorporate(managementServerAddress, identityServerAddress, credentials))
			{
				return "";
			}
			return Strings.Temp_SERVERNOTFOUND;
		}
		catch (ServerHelper.ServerException ex)
		{
			return ex.Message;
		}
		catch
		{
			return Strings.Temp_SERVERNOTFOUND;
		}
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Mobile.InstallerWizard.WizardPageControl));
		this.labelTitle = new System.Windows.Forms.Label();
		this.textBoxURI = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.labelError = new System.Windows.Forms.Label();
		this.labelPorts = new System.Windows.Forms.Label();
		this.labelPort1 = new System.Windows.Forms.Label();
		this.labelPort2 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.textBoxHTTPPort = new System.Windows.Forms.TextBox();
		this.textBoxHttpSecurePort = new System.Windows.Forms.TextBox();
		this.panelPorts = new System.Windows.Forms.Panel();
		this.errorIcon = new System.Windows.Forms.PictureBox();
		base.WizardPage.SuspendLayout();
		this.panelPorts.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorIcon).BeginInit();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.errorIcon);
		base.WizardPage.Controls.Add(this.panelPorts);
		base.WizardPage.Controls.Add(this.label5);
		base.WizardPage.Controls.Add(this.label6);
		base.WizardPage.Controls.Add(this.label2);
		base.WizardPage.Controls.Add(this.textBoxURI);
		base.WizardPage.Controls.Add(this.labelTitle);
		base.WizardPage.Controls.Add(this.labelError);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Name = "WizardPage";
		componentResourceManager.ApplyResources(this.labelTitle, "labelTitle");
		this.labelTitle.Name = "labelTitle";
		componentResourceManager.ApplyResources(this.textBoxURI, "textBoxURI");
		this.textBoxURI.Name = "textBoxURI";
		this.textBoxURI.Tag = "";
		componentResourceManager.ApplyResources(this.label2, "label2");
		this.label2.Name = "label2";
		this.label2.Tag = "SERVERURI";
		componentResourceManager.ApplyResources(this.label5, "label5");
		this.label5.Name = "label5";
		componentResourceManager.ApplyResources(this.label6, "label6");
		this.label6.Name = "label6";
		componentResourceManager.ApplyResources(this.labelError, "labelError");
		this.labelError.ForeColor = System.Drawing.Color.Black;
		this.labelError.Name = "labelError";
		componentResourceManager.ApplyResources(this.labelPorts, "labelPorts");
		this.labelPorts.Name = "labelPorts";
		componentResourceManager.ApplyResources(this.labelPort1, "labelPort1");
		this.labelPort1.Name = "labelPort1";
		componentResourceManager.ApplyResources(this.labelPort2, "labelPort2");
		this.labelPort2.Name = "labelPort2";
		componentResourceManager.ApplyResources(this.textBox1, "textBox1");
		this.textBox1.Name = "textBox1";
		this.textBox1.Tag = "";
		componentResourceManager.ApplyResources(this.textBox2, "textBox2");
		this.textBox2.Name = "textBox2";
		this.textBox2.Tag = "";
		componentResourceManager.ApplyResources(this.textBoxHTTPPort, "textBoxHTTPPort");
		this.textBoxHTTPPort.Name = "textBoxHTTPPort";
		componentResourceManager.ApplyResources(this.textBoxHttpSecurePort, "textBoxHttpSecurePort");
		this.textBoxHttpSecurePort.Name = "textBoxHttpSecurePort";
		this.panelPorts.Controls.Add(this.labelPorts);
		this.panelPorts.Controls.Add(this.textBoxHttpSecurePort);
		this.panelPorts.Controls.Add(this.labelPort1);
		this.panelPorts.Controls.Add(this.textBoxHTTPPort);
		this.panelPorts.Controls.Add(this.labelPort2);
		componentResourceManager.ApplyResources(this.panelPorts, "panelPorts");
		this.panelPorts.Name = "panelPorts";
		this.errorIcon.Image = VideoOS.Mobile.InstallerWizard.Strings.error_16x16;
		componentResourceManager.ApplyResources(this.errorIcon, "pictureBox1");
		this.errorIcon.Name = "errorIcon";
		this.errorIcon.TabStop = false;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageControl";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		this.panelPorts.ResumeLayout(false);
		this.panelPorts.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.errorIcon).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

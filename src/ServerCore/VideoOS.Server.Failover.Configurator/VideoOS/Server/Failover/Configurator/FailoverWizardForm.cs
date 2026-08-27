using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.FormattedLinkLabel;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinTabs;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Controls;
using VideoOS.Common.Integration;
using VideoOS.Diagnostics.Logging;
using VideoOS.Server.Failover.Configurator.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Failover.Configurator;

public class FailoverWizardForm : HelpForm
{
	private enum FailoverWizardTabPage
	{
		NodeSelectionPage,
		MaintenanceModePage,
		WebServerConfigurationPage,
		ConnectionTypePage,
		SecondaryPrepareSuccessPage,
		PrimarySqlReplicationFolders,
		PrimaryEnterConfigurationPage,
		PrimaryEnterCertificateDetailsPage,
		PrimaryPrepareCompleteSuccessPage,
		SecondarySelectLicensePage,
		SecondaryConfigurationCompletePage,
		ProcessingPage,
		SummaryPage,
		DeconfigurationCompletePage
	}

	private bool _isPrimaryNode;

	private bool _isHttpsMode;

	private bool _isMaintenanceMode;

	private bool _needsConfirmationBeforeClosing;

	private bool _shouldLaunchWebConsoleOnClose;

	private bool _isExternalSqlSetup;

	private string _webConsolePassword;

	private string _sqlDataFolder;

	private string _sqlLogFolder;

	private bool _areCustomSqlFoldersUsed;

	public static ILogger Logger;

	private FailoverConfiguratorRunner _failoverConfiguratorRunner;

	private IContainer components;

	private UltraTabControl Wizard;

	private UltraTabSharedControlsPage TabSharedControlsPage;

	private UltraTabPageControl TagPageSelectNodeRole;

	private UltraButton btnCancel;

	private UltraButton btnNext;

	private Panel pnlSelectNodeRole;

	private Label lblSeparator;

	private UltraLabel lblSelectNodeHeader;

	private RadioButton rbtSecondaryNode;

	private RadioButton rbtPrimaryNode;

	private Label lblPrimaryMachineDescription;

	private Label lblSecondaryMachineDescription;

	private Button btnHelp;

	private LogoHorizontalResizable logoHorizontalResizableFailover;

	private UltraTabPageControl ultraTabPageControlConnectionType;

	private Panel panelSelectConnectionType;

	private UltraLabel lblConectionMethodHeader;

	private RadioButton rbHttpsSelection;

	private RadioButton rbHttpSelection;

	private UltraTabPageControl ultraTabPageControl1;

	private UltraTabPageControl ultraTabPageControl4;

	private UltraTabPageControl ultraTabPageControl5;

	private UltraTabPageControl ultraTabPageControl6;

	private UltraTabPageControl ultraTabPageControl7;

	private Panel panelSecondaryPrepareHttp;

	private UltraLabel lblSecondaryPreparationSuccessHeader;

	private Panel panelPrimaryEnterConfiguration;

	private UltraLabel lblPrimaryConfigurationHeader;

	private Panel panelPrimaryCertificateFolder;

	private UltraLabel lblChooseCertificateFolderHeader;

	private Panel panelPrimaryPrepareSuccess;

	private UltraLabel lblPrimaryConfiguredHeader;

	private Panel panelSecondaryConfigurationComplete;

	private UltraLabel lblSecondaryConfigurationEndHeader;

	private Label label1;

	private Panel panelSecurityCode;

	private Label label2;

	private Label label6;

	private Label label5;

	private Button btnBrowseLicense;

	private TextBox txtVirtualIpAddress;

	private TextBox txtFailoverLicenseLocation;

	private TextBox txtSecondaryMachineAddress;

	private Label label4;

	private Label lblCertificateDestination;

	private TextBox txtCertificateFolderLocation;

	private Button btnSelectCertificateFolder;

	private Label label8;

	private Label lblSecondaryConfigurationEndSubHeader;

	private UltraTabPageControl ultraTabPageControl2;

	private Panel panel1;

	private UltraLabel lblSummaryPageHeader;

	private UltraTabPageControl ultraTabPageControl8;

	private Panel panel2;

	private UltraLabel lblProcessingPageHeader;

	private ProgressBar progressBarProcessing;

	private UltraTabPageControl ultraTabPageControl9;

	private UltraTabPageControl ultraTabPageControl10;

	private Panel panel3;

	private UltraLabel lblSelectSecondaryLicenseHeader;

	private Button btnFailoverLicenseBrowse;

	private TextBox txtSecondaryFailoverLicense;

	private Label label10;

	private Label lblChooseCertificateFolderDescription;

	private PictureBox pictureBoxCopyCode;

	private Panel pnlMaintenanceMode;

	private Label lblMaintenanceModeApplyLicenseDescription;

	private RadioButton cbMaintenanceOptionApplyLicense;

	private Label lblMaintenanceModeDeconfigureDescription;

	private Label lblMaintenanceModeGenerateCertificatesDescription;

	private RadioButton cbMaintenanceOptionGenerateCertificates;

	private UltraLabel lblMaintenanceModeHeader;

	private RadioButton cbMaintenanceOptionDeconfigure;

	private UltraTabPageControl ultraTabPageControl11;

	private Panel panel4;

	private UltraLabel lblDeconfigurationSuccessfulHeader;

	private Label lblVirtualIPError;

	private Label lblSecondaryMachineAddressError;

	private Label lblEnterSecurityCode;

	private TextBox txtPrimaryEnterSecurityCode;

	private Label lblHttpsCodeErrorLabel;

	private Label lblDeconfigurationDescription;

	private Label lblInitialPageDescription;

	private Label lblHttpsButtonDescription;

	private TextBox txtSecurityCode;

	private PictureBox pbHelpIconSecurityCode;

	private ToolTip toolTipFailoverConfiguration;

	private CheckBox cbConfirmSecondaryContinue;

	private Label lblContinueSecondaryWarning;

	private Label lblConnectionDescription;

	private PictureBox pbEnterSecurityCode;

	private PictureBox pbVirtualIp;

	private PictureBox pbLicenseInfo;

	private PictureBox pbFqdn;

	private Label lblLicenseDescription;

	private PictureBox pbLicenseInformationOnPage;

	private Label label3;

	private LinkLabel linkLabelLearnMore;

	private Label lblConnectionTypeDescription;

	private UltraFormattedLinkLabel lblSummaryDescription;

	private FlowLayoutPanel flowLayoutPanelSecondarySuccess;

	private UltraTabPageControl ultraTabPageControl3;

	private Panel pnlWebServerPassword;

	private UltraLabel lblWebServerPasswordHeader;

	private PictureBox pbWebServerPassword;

	private Label lblWebServerConfirmPasswordError;

	private Label lblWebServerConfirmPassword;

	private TextBox txtWebServerConfirmPassword;

	private Label lblWebServerPasswordError;

	private Label lblWebServerPassword;

	private TextBox txtWebServerPassword;

	private PictureBox pbWebServerUsername;

	private TextBox txtWebServerUsername;

	private Label lblWebServerUsername;

	private Label lblMaintenanceModeCreateWebServerPasswordDescription;

	private RadioButton cbMaintenanceOptionCreateWebServerPassword;

	private Label lblWebServerPasswordDescription;

	private Panel pnlExternalSql;

	private CheckBox cbUseExternalSql;

	private Panel pnlExternalSqlWarning;

	private PictureBox pictureBoxWarning;

	private Label lblExternalSqlWarning;

	private UltraTabPageControl ultraTabPageControl12;

	private Panel panel5;

	private UltraLabel lblSqlReplicationFoldersHeader;

	private Label lblSqlFoldersPageDescription;

	private TextBox txtSelectSecondSqlFolder;

	private TextBox txtSelectFirstSqlFolder;

	private Label lblSqlDataFolderSelectDescription;

	private Label lblSqlLogsFolderSelectDescription;

	private Button btnSelectSqlLogFolder;

	private Button btnSelectSqlDataFolder;

	private Panel pnlSqlFolders;

	private CheckBox checkBoxUseCustomSqlFolders;

	private Panel panelLicenseInfoPrimary;

	private Panel panelLicenseInfoSecondary;

	private Panel panelLicenseInfoSummary;

	private Panel panelLicenseInfoMaintanance;

	public FailoverWizardForm(IServerAccess serverAccess, IProtectionProvider passwordProtection)
	{
		LogManager.ModifyLogParameter += FailoverConfigurationHelper.LogManagerOnModifyLogParameter;
		Logger = Telemetry.GetILogger("ManagementServerFailover");
		CorporateHelp.Instance.Initialize(CultureInfo.CurrentCulture.Name, base.Handle);
		InitializeComponent();
		_failoverConfiguratorRunner = new FailoverConfiguratorRunner(serverAccess, passwordProtection, Logger);
		base.FormClosing += FailoverWizardForm_FormClosing;
	}

	private async void FailoverWizardForm_Load(object sender, EventArgs e)
	{
		PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_CheckingSystemHeader);
		InitializeComponentsOemResources();
		_isMaintenanceMode = _failoverConfiguratorRunner.Settings.MaintenanceMode;
		_isPrimaryNode = _failoverConfiguratorRunner.Settings.IsPrimary;
		_isHttpsMode = _failoverConfiguratorRunner.Settings.IsHttps;
		_isExternalSqlSetup = _failoverConfiguratorRunner.Settings.IsExternalSqlSetup;
		txtCertificateFolderLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		if (!(await FailoverConfigurationHelper.CheckIfFailoverIsInstalledAsync()))
		{
			string messageText = string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_NotInstalledErrorDescription, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerConfigurationHelpUrl, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion);
			string headerText = string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_NotInstalledErrorHeader, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion);
			NavigateToSummaryPage(messageText, headerText);
		}
		else if (_isMaintenanceMode)
		{
			PrepareAndSelectTab(FailoverWizardTabPage.MaintenanceModePage);
			AddLicenseInfoControlToPanel(panelLicenseInfoMaintanance);
		}
		else if (!(await FailoverConfigurationHelper.CheckIfFeatureIsLicensedAsync(_failoverConfiguratorRunner.ServerAccess)))
		{
			Logger.LogWarning("Feature not licensed. Request a license from your administrator.");
			string messageText2 = string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_NotSupportedErrorDescription, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion);
			string headerText2 = string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_NotSupportedErrorHeader, OemResources.Instance.ManagementFailoverServer.ProductNameWithoutCompany, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion);
			NavigateToSummaryPage(messageText2, headerText2);
		}
		else
		{
			PrepareAndSelectTab(FailoverWizardTabPage.NodeSelectionPage);
		}
	}

	private void FailoverWizardForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (_needsConfirmationBeforeClosing)
		{
			string[] buttons = new string[2]
			{
				VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonNo,
				VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonYes
			};
			int num = Utils.ShowMessage(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxCloseWizardTitle, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxCloseWizardText, buttons, ExceptionDialogDefaultButton.Button1, MessageBoxIcon.Exclamation, this);
			e.Cancel = num == 0;
		}
	}

	private void InitializeComponentsOemResources()
	{
		base.Icon = OemResources.Instance.Server.ChangeSqlAddressIcon;
		logoHorizontalResizableFailover.Logo = OemResources.Instance.Manager.WizardSmallLogo;
		logoHorizontalResizableFailover.BackgroundImage = OemResources.Instance.Manager.WizardSmallTopBar;
		logoHorizontalResizableFailover.ForeColor = OemResources.Instance.Manager.WizardSmallTopBarTextColor;
		Text = OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimple;
		lblHttpsButtonDescription.Text = string.Format(CultureInfo.CurrentCulture, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_HttpsDescriptionText, OemResources.Instance.App.CompanyName);
		lblChooseCertificateFolderDescription.Text = string.Format(CultureInfo.CurrentCulture, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_CertificatePage_Description, OemResources.Instance.App.CompanyName);
		lblSecondaryConfigurationEndHeader.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_FinalizeSecondaryPageHeader;
		lblSecondaryConfigurationEndSubHeader.Text = string.Format(CultureInfo.CurrentCulture, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ToCompleteMessage, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion);
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		NavigateNext((FailoverWizardTabPage)Wizard.ActiveTab.Index);
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		if (_shouldLaunchWebConsoleOnClose)
		{
			Process.Start(WebConsoleHelper.GetWebConsoleAddress(FailoverConfigurationHelper.LocalFqdn, _isHttpsMode));
		}
		Close();
	}

	protected override string GetHelpTopic()
	{
		return "fms_managementserverfailoverexplained";
	}

	private void btnHelp_Click(object sender, EventArgs e)
	{
		ShowHelp();
	}

	private bool ShowShouldContinueDialog(string dialogTitle, string dialogText, string continueButtonText)
	{
		string[] buttons = new string[2]
		{
			continueButtonText,
			VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonCancel
		};
		return Utils.ShowMessage(dialogTitle, dialogText, buttons, ExceptionDialogDefaultButton.Button1, MessageBoxIcon.Exclamation, this) == 0;
	}

	private async void NavigateNext(FailoverWizardTabPage currentTabKey)
	{
		switch (currentTabKey)
		{
		case FailoverWizardTabPage.NodeSelectionPage:
			_isPrimaryNode = rbtPrimaryNode.Checked;
			_isExternalSqlSetup = cbUseExternalSql.Checked;
			if ((!_isPrimaryNode || ShowShouldContinueDialog(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinuePrimaryTitle, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinuePrimaryText, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonConfirm)) && (_isPrimaryNode || ShowShouldContinueDialog(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinueSecondaryTitle, string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinueSecondaryText, OemResources.Instance.ManagementFailoverServer.ManagementServerNameSimple, OemResources.Instance.ManagementFailoverServer.LogServiceNameSimple, OemResources.Instance.ManagementFailoverServer.EventServerApplicationNameShort, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion, OemResources.Instance.ManagementFailoverServer.ProductNameWithoutCompany), VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonConfirm)))
			{
				string arg = (_isPrimaryNode ? VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_Title_Primary : VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_Title_Secondary);
				Text = string.Format(CultureInfo.CurrentCulture, "{0} - {1}", OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimple, arg);
				PrepareAndSelectTab(FailoverWizardTabPage.ConnectionTypePage);
			}
			break;
		case FailoverWizardTabPage.ConnectionTypePage:
			_isHttpsMode = rbHttpsSelection.Checked;
			if (_isPrimaryNode)
			{
				PrepareAndSelectTab(_isExternalSqlSetup ? FailoverWizardTabPage.PrimaryEnterConfigurationPage : FailoverWizardTabPage.PrimarySqlReplicationFolders);
			}
			else
			{
				PrepareAndSelectTab(FailoverWizardTabPage.WebServerConfigurationPage);
			}
			break;
		case FailoverWizardTabPage.PrimarySqlReplicationFolders:
			_sqlDataFolder = txtSelectFirstSqlFolder.Text;
			_sqlLogFolder = txtSelectSecondSqlFolder.Text;
			_areCustomSqlFoldersUsed = checkBoxUseCustomSqlFolders.Checked;
			if (_isPrimaryNode)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.PrimaryEnterConfigurationPage);
			}
			break;
		case FailoverWizardTabPage.WebServerConfigurationPage:
			if (!ValidateWebServerPassword())
			{
				break;
			}
			_webConsolePassword = txtWebServerPassword.Text;
			if (_isMaintenanceMode)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_SetWebServerPasswordHeader);
				if (!(await _failoverConfiguratorRunner.CreateWebServerPasswordAsync(_webConsolePassword)))
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
				}
				else
				{
					NavigateToSummaryPage(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_SetWebServerPasswordCompleteDescription, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_SetWebServerPasswordCompleteHeader);
				}
			}
			else if (_isPrimaryNode)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_PreparePrimaryHeader);
				string sqlReplicationFolderPath = GetSqlReplicationFolderPath("DATA");
				string sqlReplicationFolderPath2 = GetSqlReplicationFolderPath("Log");
				if (!(await _failoverConfiguratorRunner.ConfigurePrimaryNodeAsync(_isHttpsMode, txtPrimaryEnterSecurityCode.Text, _webConsolePassword, txtVirtualIpAddress.Text, txtSecondaryMachineAddress.Text, sqlReplicationFolderPath, sqlReplicationFolderPath2, _isExternalSqlSetup, txtFailoverLicenseLocation.Text)))
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
					break;
				}
				if (_isHttpsMode)
				{
					PrepareAndSelectTab(FailoverWizardTabPage.PrimaryEnterCertificateDetailsPage);
					break;
				}
				AddLicenseInfoControlToPanel(panelLicenseInfoPrimary);
				PrepareAndSelectTab(FailoverWizardTabPage.PrimaryPrepareCompleteSuccessPage);
			}
			else
			{
				PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_PrepareSecondHeader);
				var (flag, text) = await _failoverConfiguratorRunner.ConfigureSecondaryNodeAsync(_isHttpsMode, _webConsolePassword);
				if (!flag)
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
					break;
				}
				txtSecurityCode.Text = text;
				PrepareAndSelectTab(FailoverWizardTabPage.SecondaryPrepareSuccessPage);
			}
			break;
		case FailoverWizardTabPage.PrimaryEnterConfigurationPage:
			if (ShowShouldContinueDialog(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxLicenseTitle, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxLicenseText, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonContinue))
			{
				PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_ValidateHeader);
				if (!(await ValidatePrimaryNodeAsync()))
				{
					PrepareAndSelectTab(FailoverWizardTabPage.PrimaryEnterConfigurationPage);
				}
				else
				{
					PrepareAndSelectTab(FailoverWizardTabPage.WebServerConfigurationPage);
				}
			}
			break;
		case FailoverWizardTabPage.PrimaryEnterCertificateDetailsPage:
			PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_GenerateHeader);
			if (!(await _failoverConfiguratorRunner.DownloadServerCertificateAsync(txtCertificateFolderLocation.Text)))
			{
				NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
			}
			else if (!_isMaintenanceMode)
			{
				AddLicenseInfoControlToPanel(panelLicenseInfoPrimary);
				PrepareAndSelectTab(FailoverWizardTabPage.PrimaryPrepareCompleteSuccessPage);
			}
			else
			{
				string messageText = string.Format(CultureInfo.CurrentCulture, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_CertificateGenerationComplete, txtCertificateFolderLocation.Text);
				NavigateToSummaryPage(messageText, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_CertificateGenerationCompleteHeader);
			}
			break;
		case FailoverWizardTabPage.SecondaryPrepareSuccessPage:
			PrepareAndSelectTab(FailoverWizardTabPage.SecondarySelectLicensePage);
			break;
		case FailoverWizardTabPage.SecondarySelectLicensePage:
			if (_isMaintenanceMode)
			{
				if (string.IsNullOrWhiteSpace(txtSecondaryFailoverLicense.Text))
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
				}
				else if (await _failoverConfiguratorRunner.ChangeFailoverLicenseAsync(txtSecondaryFailoverLicense.Text))
				{
					NavigateToSummaryPage(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_LicenseChangedCompleteDescription, string.Format(CultureInfo.CurrentCulture, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_FinalizeSecondaryPageHeader, OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion), shouldDisplayLicenseInfo: true);
				}
				else
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
				}
			}
			else
			{
				PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_FinalizeHeader);
				if (!(await _failoverConfiguratorRunner.FinalizeSecondaryNodeAsync(_isHttpsMode, _isExternalSqlSetup, txtSecondaryFailoverLicense.Text)))
				{
					NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
					break;
				}
				AddLicenseInfoControlToPanel(panelLicenseInfoSecondary);
				PrepareAndSelectTab(FailoverWizardTabPage.SecondaryConfigurationCompletePage);
			}
			break;
		case FailoverWizardTabPage.MaintenanceModePage:
			if (cbMaintenanceOptionDeconfigure.Checked)
			{
				if (ShowShouldContinueDialog(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinueDeconfigurationTitle, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxContinueDeconfigurationText, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_MessageBoxButtonConfirm))
				{
					PrepareAndSelectTab(FailoverWizardTabPage.ProcessingPage, VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ProcessingPage_DeconfigureHeader);
					if (!(await _failoverConfiguratorRunner.DeconfigureAsync(_isExternalSqlSetup)))
					{
						NavigateToSummaryPage(string.Format(VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_Deconfiguration_GeneralErrorMessage, OemResources.Instance.ManagementFailoverServer.SupportPortalUrl, OemResources.Instance.ManagementFailoverServer.CompanyNameShort));
					}
					else
					{
						PrepareAndSelectTab(FailoverWizardTabPage.DeconfigurationCompletePage);
					}
				}
			}
			else if (cbMaintenanceOptionGenerateCertificates.Checked)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.PrimaryEnterCertificateDetailsPage);
			}
			else if (cbMaintenanceOptionApplyLicense.Checked)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.SecondarySelectLicensePage);
			}
			else if (cbMaintenanceOptionCreateWebServerPassword.Checked)
			{
				PrepareAndSelectTab(FailoverWizardTabPage.WebServerConfigurationPage);
			}
			break;
		case FailoverWizardTabPage.PrimaryPrepareCompleteSuccessPage:
			break;
		}
	}

	private void PrepareAndSelectTab(FailoverWizardTabPage newTabKey, string processingPageHeader = "")
	{
		switch (newTabKey)
		{
		default:
			return;
		case FailoverWizardTabPage.NodeSelectionPage:
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			break;
		case FailoverWizardTabPage.MaintenanceModePage:
		{
			RadioButton radioButton = cbMaintenanceOptionGenerateCertificates;
			bool enabled = (lblMaintenanceModeGenerateCertificatesDescription.Enabled = CanGenerateCertificates());
			radioButton.Enabled = enabled;
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			break;
		}
		case FailoverWizardTabPage.ConnectionTypePage:
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			_needsConfirmationBeforeClosing = true;
			break;
		case FailoverWizardTabPage.WebServerConfigurationPage:
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			_needsConfirmationBeforeClosing = true;
			break;
		case FailoverWizardTabPage.SecondaryPrepareSuccessPage:
			if (!_isHttpsMode)
			{
				panelSecurityCode.Visible = false;
			}
			btnNext.Visible = true;
			btnNext.Enabled = false;
			btnCancel.Enabled = true;
			break;
		case FailoverWizardTabPage.PrimaryEnterConfigurationPage:
			btnNext.Enabled = true;
			btnNext.Visible = true;
			btnCancel.Enabled = true;
			if (!_isHttpsMode)
			{
				lblEnterSecurityCode.Visible = false;
				txtPrimaryEnterSecurityCode.Visible = false;
				lblHttpsCodeErrorLabel.Visible = false;
				pbEnterSecurityCode.Visible = false;
			}
			break;
		case FailoverWizardTabPage.PrimaryEnterCertificateDetailsPage:
			btnNext.Enabled = true;
			btnNext.Visible = true;
			btnCancel.Enabled = true;
			break;
		case FailoverWizardTabPage.PrimaryPrepareCompleteSuccessPage:
			btnNext.Visible = false;
			btnCancel.Enabled = true;
			btnCancel.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ButtonClose;
			_needsConfirmationBeforeClosing = false;
			break;
		case FailoverWizardTabPage.SecondarySelectLicensePage:
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			break;
		case FailoverWizardTabPage.SecondaryConfigurationCompletePage:
			btnNext.Visible = false;
			btnCancel.Enabled = true;
			btnCancel.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ButtonClose;
			_needsConfirmationBeforeClosing = false;
			_shouldLaunchWebConsoleOnClose = true;
			break;
		case FailoverWizardTabPage.SummaryPage:
			btnNext.Visible = false;
			btnCancel.Enabled = true;
			btnCancel.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ButtonClose;
			_needsConfirmationBeforeClosing = false;
			break;
		case FailoverWizardTabPage.ProcessingPage:
			lblProcessingPageHeader.Text = processingPageHeader;
			btnNext.Enabled = false;
			btnCancel.Enabled = false;
			break;
		case FailoverWizardTabPage.DeconfigurationCompletePage:
			btnNext.Enabled = false;
			btnCancel.Enabled = true;
			btnCancel.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_ButtonClose;
			break;
		case FailoverWizardTabPage.PrimarySqlReplicationFolders:
			btnNext.Enabled = true;
			btnCancel.Enabled = true;
			_needsConfirmationBeforeClosing = true;
			break;
		}
		Wizard.SelectedTab = Wizard.Tabs[(int)newTabKey];
	}

	private void NavigateToSummaryPage(string messageText, string headerText = null, bool shouldDisplayLicenseInfo = false)
	{
		lblSummaryDescription.Value = messageText;
		lblSummaryPageHeader.Text = ((!string.IsNullOrEmpty(headerText)) ? headerText : VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_GeneralErrorHeader);
		if (shouldDisplayLicenseInfo)
		{
			panelLicenseInfoSummary.Visible = true;
			AddLicenseInfoControlToPanel(panelLicenseInfoSummary);
		}
		PrepareAndSelectTab(FailoverWizardTabPage.SummaryPage);
	}

	private static void AddLicenseInfoControlToPanel(Panel licenseInfoPanel)
	{
		UserControlLicenseInfo value = new UserControlLicenseInfo();
		licenseInfoPanel.Controls.Add(value);
	}

	private async Task<bool> ValidatePrimaryNodeAsync()
	{
		if (!(await ValidatePrimaryEnterConfigurationPageTabAsync()))
		{
			return false;
		}
		bool flag = _isHttpsMode;
		if (flag)
		{
			flag = !(await ValidateHttpsCodeTabAsync());
		}
		if (flag)
		{
			return false;
		}
		return true;
	}

	private async Task<bool> ValidatePrimaryEnterConfigurationPageTabAsync()
	{
		lblSecondaryMachineAddressError.Visible = false;
		lblVirtualIPError.Visible = false;
		if (string.IsNullOrWhiteSpace(txtSecondaryMachineAddress.Text))
		{
			lblSecondaryMachineAddressError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_EmptyField_Error;
			lblSecondaryMachineAddressError.Visible = true;
			return false;
		}
		if (IsPrimaryAddress(txtSecondaryMachineAddress.Text))
		{
			lblSecondaryMachineAddressError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_SecondaryMachineAddressSameAsFirst_Error;
			lblSecondaryMachineAddressError.Visible = true;
			return false;
		}
		if (!(await FailoverConfigurationHelper.PingHostAsync(txtSecondaryMachineAddress.Text, TimeSpan.FromMilliseconds(5000.0))))
		{
			lblSecondaryMachineAddressError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_SecondaryMachineAddressValidation_Error;
			lblSecondaryMachineAddressError.Visible = true;
			return false;
		}
		if (string.IsNullOrWhiteSpace(txtVirtualIpAddress.Text))
		{
			lblVirtualIPError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_EmptyField_Error;
			lblVirtualIPError.Visible = true;
			return false;
		}
		if (!ValidateIP(txtVirtualIpAddress.Text))
		{
			lblVirtualIPError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_VirtualIPValidation_Error;
			lblVirtualIPError.Visible = true;
			return false;
		}
		if (await FailoverConfigurationHelper.PingHostAsync(txtVirtualIpAddress.Text, TimeSpan.FromMilliseconds(500.0)))
		{
			lblVirtualIPError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_VirtualIPInUse_Error;
			lblVirtualIPError.Visible = true;
			return false;
		}
		return true;
	}

	private async Task<bool> ValidateHttpsCodeTabAsync()
	{
		lblHttpsCodeErrorLabel.Visible = false;
		if (string.IsNullOrWhiteSpace(txtPrimaryEnterSecurityCode.Text))
		{
			lblHttpsCodeErrorLabel.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_EmptyField_Error;
			lblHttpsCodeErrorLabel.Visible = true;
			return false;
		}
		string value = await FailoverConfigurationHelper.ValidateCertificateAuthorityConnection(Logger, txtSecondaryMachineAddress.Text, txtPrimaryEnterSecurityCode.Text);
		if (!string.IsNullOrEmpty(value))
		{
			lblHttpsCodeErrorLabel.Text = value;
			lblHttpsCodeErrorLabel.Visible = true;
			return false;
		}
		return true;
	}

	private bool ValidateWebServerPassword()
	{
		lblWebServerPasswordError.Visible = false;
		lblWebServerConfirmPasswordError.Visible = false;
		string text = txtWebServerPassword.Text;
		if (string.IsNullOrWhiteSpace(text))
		{
			lblWebServerPasswordError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_EmptyField_Error;
			lblWebServerPasswordError.Visible = true;
			return false;
		}
		if (text.Length < 6 || text.Length > 32)
		{
			lblWebServerPasswordError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_WebServerPasswordLengthError;
			lblWebServerPasswordError.Visible = true;
			return false;
		}
		if (new PasswordRequirements(new List<CharacterSet>(), new CharacterSet(), needsUppercaseLetters: false, needsLowercaseLetters: false, needsDigits: false).ValidatePassword(text) != PasswordValidationResult.Success)
		{
			lblWebServerPasswordError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_WebServerPasswordUnsupportedCharError;
			lblWebServerPasswordError.Visible = true;
			return false;
		}
		if (text != txtWebServerConfirmPassword.Text)
		{
			lblWebServerConfirmPasswordError.Text = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_WebServerConfirmPasswordDoesNotMatchError;
			lblWebServerConfirmPasswordError.Visible = true;
			return false;
		}
		return true;
	}

	private static bool ValidateIP(string ipAddress)
	{
		return new Regex("^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$").IsMatch(ipAddress, 0);
	}

	private static bool IsPrimaryAddress(string deviceAddress)
	{
		string hostName = Dns.GetHostName();
		if (!deviceAddress.Equals(hostName, StringComparison.OrdinalIgnoreCase) && !deviceAddress.Equals(FailoverConfigurationHelper.LocalFqdn, StringComparison.OrdinalIgnoreCase) && !deviceAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
		{
			return (from ip in Dns.GetHostAddresses(hostName)
				select ip.ToString()).Contains(deviceAddress);
		}
		return true;
	}

	private static string GetSelectedFilePath()
	{
		string result = string.Empty;
		using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			openFileDialog.Filter = VideoOS.Server.Failover.Configurator.Properties.Resources.FailoverWizardForm_LicenseFileSelect_Filter;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				result = openFileDialog.FileName;
			}
		}
		return result;
	}

	private bool CanGenerateCertificates()
	{
		if (_isMaintenanceMode && _isPrimaryNode)
		{
			return _isHttpsMode;
		}
		return false;
	}

	private string GetSqlReplicationFolderPath(string sqlFolderName)
	{
		string text = ((sqlFolderName == "DATA") ? _sqlDataFolder : _sqlLogFolder);
		if (!_areCustomSqlFoldersUsed || string.IsNullOrWhiteSpace(text))
		{
			return string.Empty;
		}
		return text;
	}

	private void pictureBoxCopyCode_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(txtSecurityCode.Text);
	}

	private void btnFailoverLicenseBrowse_Click(object sender, EventArgs e)
	{
		txtSecondaryFailoverLicense.Text = GetSelectedFilePath();
	}

	private void btnBrowseLicense_Click(object sender, EventArgs e)
	{
		txtFailoverLicenseLocation.Text = GetSelectedFilePath();
	}

	private void btnSelectCertificateFolder_Click(object sender, EventArgs e)
	{
		using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			txtCertificateFolderLocation.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void cbConfirmSecondaryContinue_CheckedChanged(object sender, EventArgs e)
	{
		btnNext.Enabled = cbConfirmSecondaryContinue.Checked;
	}

	private void linkLabelLearnMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("https://doc.milestonesys.com/latest/en-US/default.htm#cshid=9877");
	}

	private void rbtPrimaryNode_CheckedChanged(object sender, EventArgs e)
	{
		cbUseExternalSql.Enabled = true;
	}

	private void cbUseExternalSql_CheckedChanged(object sender, EventArgs e)
	{
		pnlExternalSqlWarning.Visible = cbUseExternalSql.Checked;
	}

	private void rbtSecondaryNode_CheckedChanged(object sender, EventArgs e)
	{
		cbUseExternalSql.Enabled = false;
		cbUseExternalSql.Checked = false;
		pnlExternalSqlWarning.Visible = false;
	}

	private void btnSelectSqlDataFolder_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			txtSelectFirstSqlFolder.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void btnSelectSqlLogFolder_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			txtSelectSecondSqlFolder.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void checkBoxUseCustomSqlFolders_CheckedChanged(object sender, EventArgs e)
	{
		pnlSqlFolders.Enabled = checkBoxUseCustomSqlFolders.Checked;
		if (!checkBoxUseCustomSqlFolders.Checked)
		{
			txtSelectFirstSqlFolder.Text = string.Empty;
			txtSelectSecondSqlFolder.Text = string.Empty;
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

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Server.Failover.Configurator.FailoverWizardForm));
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		Infragistics.Win.UltraWinTabControl.UltraTab ultraTab14 = new Infragistics.Win.UltraWinTabControl.UltraTab();
		this.TagPageSelectNodeRole = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.pnlSelectNodeRole = new System.Windows.Forms.Panel();
		this.pnlExternalSql = new System.Windows.Forms.Panel();
		this.pnlExternalSqlWarning = new System.Windows.Forms.Panel();
		this.lblExternalSqlWarning = new System.Windows.Forms.Label();
		this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
		this.cbUseExternalSql = new System.Windows.Forms.CheckBox();
		this.lblInitialPageDescription = new System.Windows.Forms.Label();
		this.lblPrimaryMachineDescription = new System.Windows.Forms.Label();
		this.lblSecondaryMachineDescription = new System.Windows.Forms.Label();
		this.rbtPrimaryNode = new System.Windows.Forms.RadioButton();
		this.lblSelectNodeHeader = new Infragistics.Win.Misc.UltraLabel();
		this.rbtSecondaryNode = new System.Windows.Forms.RadioButton();
		this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.pnlMaintenanceMode = new System.Windows.Forms.Panel();
		this.panelLicenseInfoMaintanance = new System.Windows.Forms.Panel();
		this.lblMaintenanceModeCreateWebServerPasswordDescription = new System.Windows.Forms.Label();
		this.cbMaintenanceOptionCreateWebServerPassword = new System.Windows.Forms.RadioButton();
		this.lblMaintenanceModeApplyLicenseDescription = new System.Windows.Forms.Label();
		this.cbMaintenanceOptionApplyLicense = new System.Windows.Forms.RadioButton();
		this.lblMaintenanceModeDeconfigureDescription = new System.Windows.Forms.Label();
		this.lblMaintenanceModeGenerateCertificatesDescription = new System.Windows.Forms.Label();
		this.cbMaintenanceOptionGenerateCertificates = new System.Windows.Forms.RadioButton();
		this.lblMaintenanceModeHeader = new Infragistics.Win.Misc.UltraLabel();
		this.cbMaintenanceOptionDeconfigure = new System.Windows.Forms.RadioButton();
		this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.pnlWebServerPassword = new System.Windows.Forms.Panel();
		this.lblWebServerPasswordDescription = new System.Windows.Forms.Label();
		this.pbWebServerUsername = new System.Windows.Forms.PictureBox();
		this.txtWebServerUsername = new System.Windows.Forms.TextBox();
		this.lblWebServerUsername = new System.Windows.Forms.Label();
		this.pbWebServerPassword = new System.Windows.Forms.PictureBox();
		this.lblWebServerConfirmPasswordError = new System.Windows.Forms.Label();
		this.lblWebServerConfirmPassword = new System.Windows.Forms.Label();
		this.txtWebServerConfirmPassword = new System.Windows.Forms.TextBox();
		this.lblWebServerPasswordError = new System.Windows.Forms.Label();
		this.lblWebServerPassword = new System.Windows.Forms.Label();
		this.txtWebServerPassword = new System.Windows.Forms.TextBox();
		this.lblWebServerPasswordHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControlConnectionType = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelSelectConnectionType = new System.Windows.Forms.Panel();
		this.lblConnectionTypeDescription = new System.Windows.Forms.Label();
		this.lblHttpsButtonDescription = new System.Windows.Forms.Label();
		this.rbHttpSelection = new System.Windows.Forms.RadioButton();
		this.rbHttpsSelection = new System.Windows.Forms.RadioButton();
		this.lblConectionMethodHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelSecondaryPrepareHttp = new System.Windows.Forms.Panel();
		this.flowLayoutPanelSecondarySuccess = new System.Windows.Forms.FlowLayoutPanel();
		this.panelSecurityCode = new System.Windows.Forms.Panel();
		this.pbHelpIconSecurityCode = new System.Windows.Forms.PictureBox();
		this.txtSecurityCode = new System.Windows.Forms.TextBox();
		this.pictureBoxCopyCode = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cbConfirmSecondaryContinue = new System.Windows.Forms.CheckBox();
		this.lblContinueSecondaryWarning = new System.Windows.Forms.Label();
		this.lblSecondaryPreparationSuccessHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panel5 = new System.Windows.Forms.Panel();
		this.checkBoxUseCustomSqlFolders = new System.Windows.Forms.CheckBox();
		this.pnlSqlFolders = new System.Windows.Forms.Panel();
		this.txtSelectSecondSqlFolder = new System.Windows.Forms.TextBox();
		this.lblSqlDataFolderSelectDescription = new System.Windows.Forms.Label();
		this.btnSelectSqlDataFolder = new System.Windows.Forms.Button();
		this.btnSelectSqlLogFolder = new System.Windows.Forms.Button();
		this.lblSqlLogsFolderSelectDescription = new System.Windows.Forms.Label();
		this.txtSelectFirstSqlFolder = new System.Windows.Forms.TextBox();
		this.lblSqlFoldersPageDescription = new System.Windows.Forms.Label();
		this.lblSqlReplicationFoldersHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelPrimaryEnterConfiguration = new System.Windows.Forms.Panel();
		this.pbEnterSecurityCode = new System.Windows.Forms.PictureBox();
		this.pbVirtualIp = new System.Windows.Forms.PictureBox();
		this.pbLicenseInfo = new System.Windows.Forms.PictureBox();
		this.pbFqdn = new System.Windows.Forms.PictureBox();
		this.lblConnectionDescription = new System.Windows.Forms.Label();
		this.lblHttpsCodeErrorLabel = new System.Windows.Forms.Label();
		this.lblEnterSecurityCode = new System.Windows.Forms.Label();
		this.txtPrimaryEnterSecurityCode = new System.Windows.Forms.TextBox();
		this.lblVirtualIPError = new System.Windows.Forms.Label();
		this.lblSecondaryMachineAddressError = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.btnBrowseLicense = new System.Windows.Forms.Button();
		this.txtVirtualIpAddress = new System.Windows.Forms.TextBox();
		this.txtFailoverLicenseLocation = new System.Windows.Forms.TextBox();
		this.txtSecondaryMachineAddress = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.lblPrimaryConfigurationHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelPrimaryCertificateFolder = new System.Windows.Forms.Panel();
		this.lblChooseCertificateFolderDescription = new System.Windows.Forms.Label();
		this.btnSelectCertificateFolder = new System.Windows.Forms.Button();
		this.txtCertificateFolderLocation = new System.Windows.Forms.TextBox();
		this.lblCertificateDestination = new System.Windows.Forms.Label();
		this.lblChooseCertificateFolderHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelPrimaryPrepareSuccess = new System.Windows.Forms.Panel();
		this.panelLicenseInfoPrimary = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.lblPrimaryConfiguredHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panel3 = new System.Windows.Forms.Panel();
		this.pbLicenseInformationOnPage = new System.Windows.Forms.PictureBox();
		this.lblLicenseDescription = new System.Windows.Forms.Label();
		this.btnFailoverLicenseBrowse = new System.Windows.Forms.Button();
		this.txtSecondaryFailoverLicense = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.lblSelectSecondaryLicenseHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panelSecondaryConfigurationComplete = new System.Windows.Forms.Panel();
		this.panelLicenseInfoSecondary = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.linkLabelLearnMore = new System.Windows.Forms.LinkLabel();
		this.lblSecondaryConfigurationEndSubHeader = new System.Windows.Forms.Label();
		this.lblSecondaryConfigurationEndHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panel2 = new System.Windows.Forms.Panel();
		this.progressBarProcessing = new System.Windows.Forms.ProgressBar();
		this.lblProcessingPageHeader = new Infragistics.Win.Misc.UltraLabel();
		this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelLicenseInfoSummary = new System.Windows.Forms.Panel();
		this.lblSummaryDescription = new Infragistics.Win.FormattedLinkLabel.UltraFormattedLinkLabel();
		this.lblSummaryPageHeader = new Infragistics.Win.Misc.UltraLabel();
		this.logoHorizontalResizableFailover = new VideoOS.Common.Controls.LogoHorizontalResizable();
		this.btnHelp = new System.Windows.Forms.Button();
		this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
		this.panel4 = new System.Windows.Forms.Panel();
		this.lblDeconfigurationDescription = new System.Windows.Forms.Label();
		this.lblDeconfigurationSuccessfulHeader = new Infragistics.Win.Misc.UltraLabel();
		this.Wizard = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
		this.TabSharedControlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
		this.lblSeparator = new System.Windows.Forms.Label();
		this.btnNext = new Infragistics.Win.Misc.UltraButton();
		this.btnCancel = new Infragistics.Win.Misc.UltraButton();
		this.toolTipFailoverConfiguration = new System.Windows.Forms.ToolTip(this.components);
		this.TagPageSelectNodeRole.SuspendLayout();
		this.pnlSelectNodeRole.SuspendLayout();
		this.pnlExternalSql.SuspendLayout();
		this.pnlExternalSqlWarning.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxWarning).BeginInit();
		this.ultraTabPageControl9.SuspendLayout();
		this.pnlMaintenanceMode.SuspendLayout();
		this.ultraTabPageControl3.SuspendLayout();
		this.pnlWebServerPassword.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbWebServerUsername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbWebServerPassword).BeginInit();
		this.ultraTabPageControlConnectionType.SuspendLayout();
		this.panelSelectConnectionType.SuspendLayout();
		this.ultraTabPageControl1.SuspendLayout();
		this.panelSecondaryPrepareHttp.SuspendLayout();
		this.flowLayoutPanelSecondarySuccess.SuspendLayout();
		this.panelSecurityCode.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbHelpIconSecurityCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxCopyCode).BeginInit();
		this.ultraTabPageControl12.SuspendLayout();
		this.panel5.SuspendLayout();
		this.pnlSqlFolders.SuspendLayout();
		this.ultraTabPageControl4.SuspendLayout();
		this.panelPrimaryEnterConfiguration.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbEnterSecurityCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbVirtualIp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbLicenseInfo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbFqdn).BeginInit();
		this.ultraTabPageControl5.SuspendLayout();
		this.panelPrimaryCertificateFolder.SuspendLayout();
		this.ultraTabPageControl6.SuspendLayout();
		this.panelPrimaryPrepareSuccess.SuspendLayout();
		this.ultraTabPageControl10.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbLicenseInformationOnPage).BeginInit();
		this.ultraTabPageControl7.SuspendLayout();
		this.panelSecondaryConfigurationComplete.SuspendLayout();
		this.ultraTabPageControl8.SuspendLayout();
		this.panel2.SuspendLayout();
		this.ultraTabPageControl2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.ultraTabPageControl11.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Wizard).BeginInit();
		this.Wizard.SuspendLayout();
		this.TabSharedControlsPage.SuspendLayout();
		base.SuspendLayout();
		this.TagPageSelectNodeRole.Controls.Add(this.pnlSelectNodeRole);
		resources.ApplyResources(this.TagPageSelectNodeRole, "TagPageSelectNodeRole");
		this.TagPageSelectNodeRole.Name = "TagPageSelectNodeRole";
		resources.ApplyResources(this.pnlSelectNodeRole, "pnlSelectNodeRole");
		this.pnlSelectNodeRole.BackColor = System.Drawing.SystemColors.Control;
		this.pnlSelectNodeRole.Controls.Add(this.pnlExternalSql);
		this.pnlSelectNodeRole.Controls.Add(this.lblInitialPageDescription);
		this.pnlSelectNodeRole.Controls.Add(this.lblPrimaryMachineDescription);
		this.pnlSelectNodeRole.Controls.Add(this.lblSecondaryMachineDescription);
		this.pnlSelectNodeRole.Controls.Add(this.rbtPrimaryNode);
		this.pnlSelectNodeRole.Controls.Add(this.lblSelectNodeHeader);
		this.pnlSelectNodeRole.Controls.Add(this.rbtSecondaryNode);
		this.pnlSelectNodeRole.Name = "pnlSelectNodeRole";
		this.pnlSelectNodeRole.TabStop = true;
		this.pnlExternalSql.Controls.Add(this.pnlExternalSqlWarning);
		this.pnlExternalSql.Controls.Add(this.cbUseExternalSql);
		resources.ApplyResources(this.pnlExternalSql, "pnlExternalSql");
		this.pnlExternalSql.Name = "pnlExternalSql";
		this.pnlExternalSqlWarning.Controls.Add(this.lblExternalSqlWarning);
		this.pnlExternalSqlWarning.Controls.Add(this.pictureBoxWarning);
		resources.ApplyResources(this.pnlExternalSqlWarning, "pnlExternalSqlWarning");
		this.pnlExternalSqlWarning.Name = "pnlExternalSqlWarning";
		resources.ApplyResources(this.lblExternalSqlWarning, "lblExternalSqlWarning");
		this.lblExternalSqlWarning.Name = "lblExternalSqlWarning";
		resources.ApplyResources(this.pictureBoxWarning, "pictureBoxWarning");
		this.pictureBoxWarning.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.warning_status;
		this.pictureBoxWarning.Name = "pictureBoxWarning";
		this.pictureBoxWarning.TabStop = false;
		resources.ApplyResources(this.cbUseExternalSql, "cbUseExternalSql");
		this.cbUseExternalSql.Name = "cbUseExternalSql";
		this.cbUseExternalSql.UseVisualStyleBackColor = true;
		this.cbUseExternalSql.CheckedChanged += new System.EventHandler(cbUseExternalSql_CheckedChanged);
		this.lblInitialPageDescription.AutoEllipsis = true;
		resources.ApplyResources(this.lblInitialPageDescription, "lblInitialPageDescription");
		this.lblInitialPageDescription.Name = "lblInitialPageDescription";
		resources.ApplyResources(this.lblPrimaryMachineDescription, "lblPrimaryMachineDescription");
		this.lblPrimaryMachineDescription.Name = "lblPrimaryMachineDescription";
		resources.ApplyResources(this.lblSecondaryMachineDescription, "lblSecondaryMachineDescription");
		this.lblSecondaryMachineDescription.Name = "lblSecondaryMachineDescription";
		resources.ApplyResources(this.rbtPrimaryNode, "rbtPrimaryNode");
		this.rbtPrimaryNode.Name = "rbtPrimaryNode";
		this.rbtPrimaryNode.UseVisualStyleBackColor = true;
		this.rbtPrimaryNode.CheckedChanged += new System.EventHandler(rbtPrimaryNode_CheckedChanged);
		resources.ApplyResources(this.lblSelectNodeHeader, "lblSelectNodeHeader");
		this.lblSelectNodeHeader.Name = "lblSelectNodeHeader";
		resources.ApplyResources(this.rbtSecondaryNode, "rbtSecondaryNode");
		this.rbtSecondaryNode.Checked = true;
		this.rbtSecondaryNode.Name = "rbtSecondaryNode";
		this.rbtSecondaryNode.TabStop = true;
		this.rbtSecondaryNode.UseVisualStyleBackColor = true;
		this.rbtSecondaryNode.CheckedChanged += new System.EventHandler(rbtSecondaryNode_CheckedChanged);
		this.ultraTabPageControl9.Controls.Add(this.pnlMaintenanceMode);
		resources.ApplyResources(this.ultraTabPageControl9, "ultraTabPageControl9");
		this.ultraTabPageControl9.Name = "ultraTabPageControl9";
		resources.ApplyResources(this.pnlMaintenanceMode, "pnlMaintenanceMode");
		this.pnlMaintenanceMode.BackColor = System.Drawing.SystemColors.Control;
		this.pnlMaintenanceMode.Controls.Add(this.panelLicenseInfoMaintanance);
		this.pnlMaintenanceMode.Controls.Add(this.lblMaintenanceModeCreateWebServerPasswordDescription);
		this.pnlMaintenanceMode.Controls.Add(this.cbMaintenanceOptionCreateWebServerPassword);
		this.pnlMaintenanceMode.Controls.Add(this.lblMaintenanceModeApplyLicenseDescription);
		this.pnlMaintenanceMode.Controls.Add(this.cbMaintenanceOptionApplyLicense);
		this.pnlMaintenanceMode.Controls.Add(this.lblMaintenanceModeDeconfigureDescription);
		this.pnlMaintenanceMode.Controls.Add(this.lblMaintenanceModeGenerateCertificatesDescription);
		this.pnlMaintenanceMode.Controls.Add(this.cbMaintenanceOptionGenerateCertificates);
		this.pnlMaintenanceMode.Controls.Add(this.lblMaintenanceModeHeader);
		this.pnlMaintenanceMode.Controls.Add(this.cbMaintenanceOptionDeconfigure);
		this.pnlMaintenanceMode.Name = "pnlMaintenanceMode";
		this.pnlMaintenanceMode.TabStop = true;
		resources.ApplyResources(this.panelLicenseInfoMaintanance, "panelLicenseInfoMaintanance");
		this.panelLicenseInfoMaintanance.Name = "panelLicenseInfoMaintanance";
		resources.ApplyResources(this.lblMaintenanceModeCreateWebServerPasswordDescription, "lblMaintenanceModeCreateWebServerPasswordDescription");
		this.lblMaintenanceModeCreateWebServerPasswordDescription.Name = "lblMaintenanceModeCreateWebServerPasswordDescription";
		resources.ApplyResources(this.cbMaintenanceOptionCreateWebServerPassword, "cbMaintenanceOptionCreateWebServerPassword");
		this.cbMaintenanceOptionCreateWebServerPassword.Name = "cbMaintenanceOptionCreateWebServerPassword";
		this.cbMaintenanceOptionCreateWebServerPassword.TabStop = true;
		this.cbMaintenanceOptionCreateWebServerPassword.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.lblMaintenanceModeApplyLicenseDescription, "lblMaintenanceModeApplyLicenseDescription");
		this.lblMaintenanceModeApplyLicenseDescription.Name = "lblMaintenanceModeApplyLicenseDescription";
		resources.ApplyResources(this.cbMaintenanceOptionApplyLicense, "cbMaintenanceOptionApplyLicense");
		this.cbMaintenanceOptionApplyLicense.Checked = true;
		this.cbMaintenanceOptionApplyLicense.Name = "cbMaintenanceOptionApplyLicense";
		this.cbMaintenanceOptionApplyLicense.TabStop = true;
		this.cbMaintenanceOptionApplyLicense.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.lblMaintenanceModeDeconfigureDescription, "lblMaintenanceModeDeconfigureDescription");
		this.lblMaintenanceModeDeconfigureDescription.Name = "lblMaintenanceModeDeconfigureDescription";
		resources.ApplyResources(this.lblMaintenanceModeGenerateCertificatesDescription, "lblMaintenanceModeGenerateCertificatesDescription");
		this.lblMaintenanceModeGenerateCertificatesDescription.Name = "lblMaintenanceModeGenerateCertificatesDescription";
		resources.ApplyResources(this.cbMaintenanceOptionGenerateCertificates, "cbMaintenanceOptionGenerateCertificates");
		this.cbMaintenanceOptionGenerateCertificates.Name = "cbMaintenanceOptionGenerateCertificates";
		this.cbMaintenanceOptionGenerateCertificates.TabStop = true;
		this.cbMaintenanceOptionGenerateCertificates.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.lblMaintenanceModeHeader, "lblMaintenanceModeHeader");
		this.lblMaintenanceModeHeader.Name = "lblMaintenanceModeHeader";
		resources.ApplyResources(this.cbMaintenanceOptionDeconfigure, "cbMaintenanceOptionDeconfigure");
		this.cbMaintenanceOptionDeconfigure.Name = "cbMaintenanceOptionDeconfigure";
		this.cbMaintenanceOptionDeconfigure.TabStop = true;
		this.cbMaintenanceOptionDeconfigure.UseVisualStyleBackColor = true;
		this.ultraTabPageControl3.Controls.Add(this.pnlWebServerPassword);
		resources.ApplyResources(this.ultraTabPageControl3, "ultraTabPageControl3");
		this.ultraTabPageControl3.Name = "ultraTabPageControl3";
		resources.ApplyResources(this.pnlWebServerPassword, "pnlWebServerPassword");
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerPasswordDescription);
		this.pnlWebServerPassword.Controls.Add(this.pbWebServerUsername);
		this.pnlWebServerPassword.Controls.Add(this.txtWebServerUsername);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerUsername);
		this.pnlWebServerPassword.Controls.Add(this.pbWebServerPassword);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerConfirmPasswordError);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerConfirmPassword);
		this.pnlWebServerPassword.Controls.Add(this.txtWebServerConfirmPassword);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerPasswordError);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerPassword);
		this.pnlWebServerPassword.Controls.Add(this.txtWebServerPassword);
		this.pnlWebServerPassword.Controls.Add(this.lblWebServerPasswordHeader);
		this.pnlWebServerPassword.Name = "pnlWebServerPassword";
		this.pnlWebServerPassword.TabStop = true;
		resources.ApplyResources(this.lblWebServerPasswordDescription, "lblWebServerPasswordDescription");
		this.lblWebServerPasswordDescription.Name = "lblWebServerPasswordDescription";
		this.pbWebServerUsername.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbWebServerUsername, "pbWebServerUsername");
		this.pbWebServerUsername.Name = "pbWebServerUsername";
		this.pbWebServerUsername.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbWebServerUsername, resources.GetString("pbWebServerUsername.ToolTip"));
		resources.ApplyResources(this.txtWebServerUsername, "txtWebServerUsername");
		this.txtWebServerUsername.Name = "txtWebServerUsername";
		this.txtWebServerUsername.ReadOnly = true;
		resources.ApplyResources(this.lblWebServerUsername, "lblWebServerUsername");
		this.lblWebServerUsername.Name = "lblWebServerUsername";
		this.pbWebServerPassword.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbWebServerPassword, "pbWebServerPassword");
		this.pbWebServerPassword.Name = "pbWebServerPassword";
		this.pbWebServerPassword.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbWebServerPassword, resources.GetString("pbWebServerPassword.ToolTip"));
		this.lblWebServerConfirmPasswordError.ForeColor = System.Drawing.Color.Red;
		resources.ApplyResources(this.lblWebServerConfirmPasswordError, "lblWebServerConfirmPasswordError");
		this.lblWebServerConfirmPasswordError.Name = "lblWebServerConfirmPasswordError";
		resources.ApplyResources(this.lblWebServerConfirmPassword, "lblWebServerConfirmPassword");
		this.lblWebServerConfirmPassword.Name = "lblWebServerConfirmPassword";
		resources.ApplyResources(this.txtWebServerConfirmPassword, "txtWebServerConfirmPassword");
		this.txtWebServerConfirmPassword.Name = "txtWebServerConfirmPassword";
		resources.ApplyResources(this.lblWebServerPasswordError, "lblWebServerPasswordError");
		this.lblWebServerPasswordError.ForeColor = System.Drawing.Color.Red;
		this.lblWebServerPasswordError.Name = "lblWebServerPasswordError";
		resources.ApplyResources(this.lblWebServerPassword, "lblWebServerPassword");
		this.lblWebServerPassword.Name = "lblWebServerPassword";
		resources.ApplyResources(this.txtWebServerPassword, "txtWebServerPassword");
		this.txtWebServerPassword.Name = "txtWebServerPassword";
		resources.ApplyResources(this.lblWebServerPasswordHeader, "lblWebServerPasswordHeader");
		this.lblWebServerPasswordHeader.Name = "lblWebServerPasswordHeader";
		this.ultraTabPageControlConnectionType.Controls.Add(this.panelSelectConnectionType);
		resources.ApplyResources(this.ultraTabPageControlConnectionType, "ultraTabPageControlConnectionType");
		this.ultraTabPageControlConnectionType.Name = "ultraTabPageControlConnectionType";
		this.panelSelectConnectionType.Controls.Add(this.lblConnectionTypeDescription);
		this.panelSelectConnectionType.Controls.Add(this.lblHttpsButtonDescription);
		this.panelSelectConnectionType.Controls.Add(this.rbHttpSelection);
		this.panelSelectConnectionType.Controls.Add(this.rbHttpsSelection);
		this.panelSelectConnectionType.Controls.Add(this.lblConectionMethodHeader);
		resources.ApplyResources(this.panelSelectConnectionType, "panelSelectConnectionType");
		this.panelSelectConnectionType.Name = "panelSelectConnectionType";
		resources.ApplyResources(this.lblConnectionTypeDescription, "lblConnectionTypeDescription");
		this.lblConnectionTypeDescription.Name = "lblConnectionTypeDescription";
		resources.ApplyResources(this.lblHttpsButtonDescription, "lblHttpsButtonDescription");
		this.lblHttpsButtonDescription.Name = "lblHttpsButtonDescription";
		resources.ApplyResources(this.rbHttpSelection, "rbHttpSelection");
		this.rbHttpSelection.Name = "rbHttpSelection";
		this.rbHttpSelection.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.rbHttpsSelection, "rbHttpsSelection");
		this.rbHttpsSelection.Checked = true;
		this.rbHttpsSelection.Name = "rbHttpsSelection";
		this.rbHttpsSelection.TabStop = true;
		this.rbHttpsSelection.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.lblConectionMethodHeader, "lblConectionMethodHeader");
		this.lblConectionMethodHeader.Name = "lblConectionMethodHeader";
		this.ultraTabPageControl1.Controls.Add(this.panelSecondaryPrepareHttp);
		resources.ApplyResources(this.ultraTabPageControl1, "ultraTabPageControl1");
		this.ultraTabPageControl1.Name = "ultraTabPageControl1";
		this.panelSecondaryPrepareHttp.Controls.Add(this.flowLayoutPanelSecondarySuccess);
		this.panelSecondaryPrepareHttp.Controls.Add(this.lblSecondaryPreparationSuccessHeader);
		resources.ApplyResources(this.panelSecondaryPrepareHttp, "panelSecondaryPrepareHttp");
		this.panelSecondaryPrepareHttp.Name = "panelSecondaryPrepareHttp";
		this.flowLayoutPanelSecondarySuccess.Controls.Add(this.panelSecurityCode);
		this.flowLayoutPanelSecondarySuccess.Controls.Add(this.label1);
		this.flowLayoutPanelSecondarySuccess.Controls.Add(this.cbConfirmSecondaryContinue);
		this.flowLayoutPanelSecondarySuccess.Controls.Add(this.lblContinueSecondaryWarning);
		resources.ApplyResources(this.flowLayoutPanelSecondarySuccess, "flowLayoutPanelSecondarySuccess");
		this.flowLayoutPanelSecondarySuccess.Name = "flowLayoutPanelSecondarySuccess";
		this.panelSecurityCode.Controls.Add(this.pbHelpIconSecurityCode);
		this.panelSecurityCode.Controls.Add(this.txtSecurityCode);
		this.panelSecurityCode.Controls.Add(this.pictureBoxCopyCode);
		this.panelSecurityCode.Controls.Add(this.label2);
		resources.ApplyResources(this.panelSecurityCode, "panelSecurityCode");
		this.panelSecurityCode.Name = "panelSecurityCode";
		this.pbHelpIconSecurityCode.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbHelpIconSecurityCode, "pbHelpIconSecurityCode");
		this.pbHelpIconSecurityCode.Name = "pbHelpIconSecurityCode";
		this.pbHelpIconSecurityCode.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbHelpIconSecurityCode, resources.GetString("pbHelpIconSecurityCode.ToolTip"));
		resources.ApplyResources(this.txtSecurityCode, "txtSecurityCode");
		this.txtSecurityCode.Name = "txtSecurityCode";
		this.txtSecurityCode.ReadOnly = true;
		this.pictureBoxCopyCode.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pictureBoxCopyCode.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.copy_security_code;
		resources.ApplyResources(this.pictureBoxCopyCode, "pictureBoxCopyCode");
		this.pictureBoxCopyCode.Name = "pictureBoxCopyCode";
		this.pictureBoxCopyCode.TabStop = false;
		this.pictureBoxCopyCode.Click += new System.EventHandler(pictureBoxCopyCode_Click);
		resources.ApplyResources(this.label2, "label2");
		this.label2.Name = "label2";
		resources.ApplyResources(this.label1, "label1");
		this.label1.Name = "label1";
		resources.ApplyResources(this.cbConfirmSecondaryContinue, "cbConfirmSecondaryContinue");
		this.cbConfirmSecondaryContinue.Name = "cbConfirmSecondaryContinue";
		this.cbConfirmSecondaryContinue.UseVisualStyleBackColor = true;
		this.cbConfirmSecondaryContinue.CheckedChanged += new System.EventHandler(cbConfirmSecondaryContinue_CheckedChanged);
		resources.ApplyResources(this.lblContinueSecondaryWarning, "lblContinueSecondaryWarning");
		this.lblContinueSecondaryWarning.Name = "lblContinueSecondaryWarning";
		resources.ApplyResources(this.lblSecondaryPreparationSuccessHeader, "lblSecondaryPreparationSuccessHeader");
		this.lblSecondaryPreparationSuccessHeader.Name = "lblSecondaryPreparationSuccessHeader";
		this.ultraTabPageControl12.Controls.Add(this.panel5);
		resources.ApplyResources(this.ultraTabPageControl12, "ultraTabPageControl12");
		this.ultraTabPageControl12.Name = "ultraTabPageControl12";
		this.panel5.Controls.Add(this.checkBoxUseCustomSqlFolders);
		this.panel5.Controls.Add(this.pnlSqlFolders);
		this.panel5.Controls.Add(this.lblSqlFoldersPageDescription);
		this.panel5.Controls.Add(this.lblSqlReplicationFoldersHeader);
		resources.ApplyResources(this.panel5, "panel5");
		this.panel5.Name = "panel5";
		resources.ApplyResources(this.checkBoxUseCustomSqlFolders, "checkBoxUseCustomSqlFolders");
		this.checkBoxUseCustomSqlFolders.Name = "checkBoxUseCustomSqlFolders";
		this.checkBoxUseCustomSqlFolders.UseVisualStyleBackColor = true;
		this.checkBoxUseCustomSqlFolders.CheckedChanged += new System.EventHandler(checkBoxUseCustomSqlFolders_CheckedChanged);
		this.pnlSqlFolders.Controls.Add(this.txtSelectSecondSqlFolder);
		this.pnlSqlFolders.Controls.Add(this.lblSqlDataFolderSelectDescription);
		this.pnlSqlFolders.Controls.Add(this.btnSelectSqlDataFolder);
		this.pnlSqlFolders.Controls.Add(this.btnSelectSqlLogFolder);
		this.pnlSqlFolders.Controls.Add(this.lblSqlLogsFolderSelectDescription);
		this.pnlSqlFolders.Controls.Add(this.txtSelectFirstSqlFolder);
		resources.ApplyResources(this.pnlSqlFolders, "pnlSqlFolders");
		this.pnlSqlFolders.Name = "pnlSqlFolders";
		resources.ApplyResources(this.txtSelectSecondSqlFolder, "txtSelectSecondSqlFolder");
		this.txtSelectSecondSqlFolder.Name = "txtSelectSecondSqlFolder";
		resources.ApplyResources(this.lblSqlDataFolderSelectDescription, "lblSqlDataFolderSelectDescription");
		this.lblSqlDataFolderSelectDescription.Name = "lblSqlDataFolderSelectDescription";
		resources.ApplyResources(this.btnSelectSqlDataFolder, "btnSelectSqlDataFolder");
		this.btnSelectSqlDataFolder.Name = "btnSelectSqlDataFolder";
		this.btnSelectSqlDataFolder.UseVisualStyleBackColor = true;
		this.btnSelectSqlDataFolder.Click += new System.EventHandler(btnSelectSqlDataFolder_Click);
		resources.ApplyResources(this.btnSelectSqlLogFolder, "btnSelectSqlLogFolder");
		this.btnSelectSqlLogFolder.Name = "btnSelectSqlLogFolder";
		this.btnSelectSqlLogFolder.UseVisualStyleBackColor = true;
		this.btnSelectSqlLogFolder.Click += new System.EventHandler(btnSelectSqlLogFolder_Click);
		resources.ApplyResources(this.lblSqlLogsFolderSelectDescription, "lblSqlLogsFolderSelectDescription");
		this.lblSqlLogsFolderSelectDescription.Name = "lblSqlLogsFolderSelectDescription";
		resources.ApplyResources(this.txtSelectFirstSqlFolder, "txtSelectFirstSqlFolder");
		this.txtSelectFirstSqlFolder.Name = "txtSelectFirstSqlFolder";
		resources.ApplyResources(this.lblSqlFoldersPageDescription, "lblSqlFoldersPageDescription");
		this.lblSqlFoldersPageDescription.Name = "lblSqlFoldersPageDescription";
		resources.ApplyResources(this.lblSqlReplicationFoldersHeader, "lblSqlReplicationFoldersHeader");
		this.lblSqlReplicationFoldersHeader.Name = "lblSqlReplicationFoldersHeader";
		this.ultraTabPageControl4.Controls.Add(this.panelPrimaryEnterConfiguration);
		resources.ApplyResources(this.ultraTabPageControl4, "ultraTabPageControl4");
		this.ultraTabPageControl4.Name = "ultraTabPageControl4";
		this.panelPrimaryEnterConfiguration.Controls.Add(this.pbEnterSecurityCode);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.pbVirtualIp);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.pbLicenseInfo);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.pbFqdn);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblConnectionDescription);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblHttpsCodeErrorLabel);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblEnterSecurityCode);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.txtPrimaryEnterSecurityCode);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblVirtualIPError);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblSecondaryMachineAddressError);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.label6);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.label5);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.btnBrowseLicense);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.txtVirtualIpAddress);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.txtFailoverLicenseLocation);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.txtSecondaryMachineAddress);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.label4);
		this.panelPrimaryEnterConfiguration.Controls.Add(this.lblPrimaryConfigurationHeader);
		resources.ApplyResources(this.panelPrimaryEnterConfiguration, "panelPrimaryEnterConfiguration");
		this.panelPrimaryEnterConfiguration.Name = "panelPrimaryEnterConfiguration";
		this.pbEnterSecurityCode.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbEnterSecurityCode, "pbEnterSecurityCode");
		this.pbEnterSecurityCode.Name = "pbEnterSecurityCode";
		this.pbEnterSecurityCode.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbEnterSecurityCode, resources.GetString("pbEnterSecurityCode.ToolTip"));
		this.pbVirtualIp.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbVirtualIp, "pbVirtualIp");
		this.pbVirtualIp.Name = "pbVirtualIp";
		this.pbVirtualIp.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbVirtualIp, resources.GetString("pbVirtualIp.ToolTip"));
		this.pbLicenseInfo.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbLicenseInfo, "pbLicenseInfo");
		this.pbLicenseInfo.Name = "pbLicenseInfo";
		this.pbLicenseInfo.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbLicenseInfo, resources.GetString("pbLicenseInfo.ToolTip"));
		this.pbFqdn.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbFqdn, "pbFqdn");
		this.pbFqdn.Name = "pbFqdn";
		this.pbFqdn.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbFqdn, resources.GetString("pbFqdn.ToolTip"));
		resources.ApplyResources(this.lblConnectionDescription, "lblConnectionDescription");
		this.lblConnectionDescription.Name = "lblConnectionDescription";
		this.lblHttpsCodeErrorLabel.ForeColor = System.Drawing.Color.Red;
		resources.ApplyResources(this.lblHttpsCodeErrorLabel, "lblHttpsCodeErrorLabel");
		this.lblHttpsCodeErrorLabel.Name = "lblHttpsCodeErrorLabel";
		resources.ApplyResources(this.lblEnterSecurityCode, "lblEnterSecurityCode");
		this.lblEnterSecurityCode.Name = "lblEnterSecurityCode";
		resources.ApplyResources(this.txtPrimaryEnterSecurityCode, "txtPrimaryEnterSecurityCode");
		this.txtPrimaryEnterSecurityCode.Name = "txtPrimaryEnterSecurityCode";
		resources.ApplyResources(this.lblVirtualIPError, "lblVirtualIPError");
		this.lblVirtualIPError.ForeColor = System.Drawing.Color.Red;
		this.lblVirtualIPError.Name = "lblVirtualIPError";
		resources.ApplyResources(this.lblSecondaryMachineAddressError, "lblSecondaryMachineAddressError");
		this.lblSecondaryMachineAddressError.ForeColor = System.Drawing.Color.Red;
		this.lblSecondaryMachineAddressError.Name = "lblSecondaryMachineAddressError";
		resources.ApplyResources(this.label6, "label6");
		this.label6.Name = "label6";
		resources.ApplyResources(this.label5, "label5");
		this.label5.Name = "label5";
		resources.ApplyResources(this.btnBrowseLicense, "btnBrowseLicense");
		this.btnBrowseLicense.Name = "btnBrowseLicense";
		this.btnBrowseLicense.UseVisualStyleBackColor = true;
		this.btnBrowseLicense.Click += new System.EventHandler(btnBrowseLicense_Click);
		resources.ApplyResources(this.txtVirtualIpAddress, "txtVirtualIpAddress");
		this.txtVirtualIpAddress.Name = "txtVirtualIpAddress";
		resources.ApplyResources(this.txtFailoverLicenseLocation, "txtFailoverLicenseLocation");
		this.txtFailoverLicenseLocation.Name = "txtFailoverLicenseLocation";
		this.txtFailoverLicenseLocation.ReadOnly = true;
		resources.ApplyResources(this.txtSecondaryMachineAddress, "txtSecondaryMachineAddress");
		this.txtSecondaryMachineAddress.Name = "txtSecondaryMachineAddress";
		resources.ApplyResources(this.label4, "label4");
		this.label4.Name = "label4";
		resources.ApplyResources(this.lblPrimaryConfigurationHeader, "lblPrimaryConfigurationHeader");
		this.lblPrimaryConfigurationHeader.Name = "lblPrimaryConfigurationHeader";
		this.ultraTabPageControl5.Controls.Add(this.panelPrimaryCertificateFolder);
		resources.ApplyResources(this.ultraTabPageControl5, "ultraTabPageControl5");
		this.ultraTabPageControl5.Name = "ultraTabPageControl5";
		this.panelPrimaryCertificateFolder.Controls.Add(this.lblChooseCertificateFolderDescription);
		this.panelPrimaryCertificateFolder.Controls.Add(this.btnSelectCertificateFolder);
		this.panelPrimaryCertificateFolder.Controls.Add(this.txtCertificateFolderLocation);
		this.panelPrimaryCertificateFolder.Controls.Add(this.lblCertificateDestination);
		this.panelPrimaryCertificateFolder.Controls.Add(this.lblChooseCertificateFolderHeader);
		resources.ApplyResources(this.panelPrimaryCertificateFolder, "panelPrimaryCertificateFolder");
		this.panelPrimaryCertificateFolder.Name = "panelPrimaryCertificateFolder";
		resources.ApplyResources(this.lblChooseCertificateFolderDescription, "lblChooseCertificateFolderDescription");
		this.lblChooseCertificateFolderDescription.Name = "lblChooseCertificateFolderDescription";
		resources.ApplyResources(this.btnSelectCertificateFolder, "btnSelectCertificateFolder");
		this.btnSelectCertificateFolder.Name = "btnSelectCertificateFolder";
		this.btnSelectCertificateFolder.UseVisualStyleBackColor = true;
		this.btnSelectCertificateFolder.Click += new System.EventHandler(btnSelectCertificateFolder_Click);
		resources.ApplyResources(this.txtCertificateFolderLocation, "txtCertificateFolderLocation");
		this.txtCertificateFolderLocation.Name = "txtCertificateFolderLocation";
		this.txtCertificateFolderLocation.ReadOnly = true;
		resources.ApplyResources(this.lblCertificateDestination, "lblCertificateDestination");
		this.lblCertificateDestination.Name = "lblCertificateDestination";
		resources.ApplyResources(this.lblChooseCertificateFolderHeader, "lblChooseCertificateFolderHeader");
		this.lblChooseCertificateFolderHeader.Name = "lblChooseCertificateFolderHeader";
		this.ultraTabPageControl6.Controls.Add(this.panelPrimaryPrepareSuccess);
		this.ultraTabPageControl6.Controls.Add(this.logoHorizontalResizableFailover);
		this.ultraTabPageControl6.Controls.Add(this.btnHelp);
		resources.ApplyResources(this.ultraTabPageControl6, "ultraTabPageControl6");
		this.ultraTabPageControl6.Name = "ultraTabPageControl6";
		this.panelPrimaryPrepareSuccess.Controls.Add(this.panelLicenseInfoPrimary);
		this.panelPrimaryPrepareSuccess.Controls.Add(this.label8);
		this.panelPrimaryPrepareSuccess.Controls.Add(this.lblPrimaryConfiguredHeader);
		resources.ApplyResources(this.panelPrimaryPrepareSuccess, "panelPrimaryPrepareSuccess");
		this.panelPrimaryPrepareSuccess.Name = "panelPrimaryPrepareSuccess";
		resources.ApplyResources(this.panelLicenseInfoPrimary, "panelLicenseInfoPrimary");
		this.panelLicenseInfoPrimary.Name = "panelLicenseInfoPrimary";
		resources.ApplyResources(this.label8, "label8");
		this.label8.Name = "label8";
		resources.ApplyResources(this.lblPrimaryConfiguredHeader, "lblPrimaryConfiguredHeader");
		this.lblPrimaryConfiguredHeader.Name = "lblPrimaryConfiguredHeader";
		this.ultraTabPageControl10.Controls.Add(this.panel3);
		resources.ApplyResources(this.ultraTabPageControl10, "ultraTabPageControl10");
		this.ultraTabPageControl10.Name = "ultraTabPageControl10";
		this.panel3.Controls.Add(this.pbLicenseInformationOnPage);
		this.panel3.Controls.Add(this.lblLicenseDescription);
		this.panel3.Controls.Add(this.btnFailoverLicenseBrowse);
		this.panel3.Controls.Add(this.txtSecondaryFailoverLicense);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.lblSelectSecondaryLicenseHeader);
		resources.ApplyResources(this.panel3, "panel3");
		this.panel3.Name = "panel3";
		this.pbLicenseInformationOnPage.Image = VideoOS.Server.Failover.Configurator.Properties.Resources.atom_icon_help_off;
		resources.ApplyResources(this.pbLicenseInformationOnPage, "pbLicenseInformationOnPage");
		this.pbLicenseInformationOnPage.Name = "pbLicenseInformationOnPage";
		this.pbLicenseInformationOnPage.TabStop = false;
		this.toolTipFailoverConfiguration.SetToolTip(this.pbLicenseInformationOnPage, resources.GetString("pbLicenseInformationOnPage.ToolTip"));
		resources.ApplyResources(this.lblLicenseDescription, "lblLicenseDescription");
		this.lblLicenseDescription.Name = "lblLicenseDescription";
		resources.ApplyResources(this.btnFailoverLicenseBrowse, "btnFailoverLicenseBrowse");
		this.btnFailoverLicenseBrowse.Name = "btnFailoverLicenseBrowse";
		this.btnFailoverLicenseBrowse.UseVisualStyleBackColor = true;
		this.btnFailoverLicenseBrowse.Click += new System.EventHandler(btnFailoverLicenseBrowse_Click);
		resources.ApplyResources(this.txtSecondaryFailoverLicense, "txtSecondaryFailoverLicense");
		this.txtSecondaryFailoverLicense.Name = "txtSecondaryFailoverLicense";
		this.txtSecondaryFailoverLicense.ReadOnly = true;
		resources.ApplyResources(this.label10, "label10");
		this.label10.Name = "label10";
		resources.ApplyResources(this.lblSelectSecondaryLicenseHeader, "lblSelectSecondaryLicenseHeader");
		this.lblSelectSecondaryLicenseHeader.Name = "lblSelectSecondaryLicenseHeader";
		this.ultraTabPageControl7.Controls.Add(this.panelSecondaryConfigurationComplete);
		resources.ApplyResources(this.ultraTabPageControl7, "ultraTabPageControl7");
		this.ultraTabPageControl7.Name = "ultraTabPageControl7";
		this.panelSecondaryConfigurationComplete.Controls.Add(this.panelLicenseInfoSecondary);
		this.panelSecondaryConfigurationComplete.Controls.Add(this.label3);
		this.panelSecondaryConfigurationComplete.Controls.Add(this.linkLabelLearnMore);
		this.panelSecondaryConfigurationComplete.Controls.Add(this.lblSecondaryConfigurationEndSubHeader);
		this.panelSecondaryConfigurationComplete.Controls.Add(this.lblSecondaryConfigurationEndHeader);
		resources.ApplyResources(this.panelSecondaryConfigurationComplete, "panelSecondaryConfigurationComplete");
		this.panelSecondaryConfigurationComplete.Name = "panelSecondaryConfigurationComplete";
		resources.ApplyResources(this.panelLicenseInfoSecondary, "panelLicenseInfoSecondary");
		this.panelLicenseInfoSecondary.Name = "panelLicenseInfoSecondary";
		resources.ApplyResources(this.label3, "label3");
		this.label3.Name = "label3";
		resources.ApplyResources(this.linkLabelLearnMore, "linkLabelLearnMore");
		this.linkLabelLearnMore.LinkColor = System.Drawing.Color.DodgerBlue;
		this.linkLabelLearnMore.Name = "linkLabelLearnMore";
		this.linkLabelLearnMore.TabStop = true;
		this.linkLabelLearnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabelLearnMore_LinkClicked);
		resources.ApplyResources(this.lblSecondaryConfigurationEndSubHeader, "lblSecondaryConfigurationEndSubHeader");
		this.lblSecondaryConfigurationEndSubHeader.Name = "lblSecondaryConfigurationEndSubHeader";
		resources.ApplyResources(this.lblSecondaryConfigurationEndHeader, "lblSecondaryConfigurationEndHeader");
		this.lblSecondaryConfigurationEndHeader.Name = "lblSecondaryConfigurationEndHeader";
		this.ultraTabPageControl8.Controls.Add(this.panel2);
		resources.ApplyResources(this.ultraTabPageControl8, "ultraTabPageControl8");
		this.ultraTabPageControl8.Name = "ultraTabPageControl8";
		this.panel2.Controls.Add(this.progressBarProcessing);
		this.panel2.Controls.Add(this.lblProcessingPageHeader);
		resources.ApplyResources(this.panel2, "panel2");
		this.panel2.Name = "panel2";
		resources.ApplyResources(this.progressBarProcessing, "progressBarProcessing");
		this.progressBarProcessing.Name = "progressBarProcessing";
		this.progressBarProcessing.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		resources.ApplyResources(this.lblProcessingPageHeader, "lblProcessingPageHeader");
		this.lblProcessingPageHeader.Name = "lblProcessingPageHeader";
		this.ultraTabPageControl2.Controls.Add(this.panel1);
		resources.ApplyResources(this.ultraTabPageControl2, "ultraTabPageControl2");
		this.ultraTabPageControl2.Name = "ultraTabPageControl2";
		this.panel1.Controls.Add(this.panelLicenseInfoSummary);
		this.panel1.Controls.Add(this.lblSummaryDescription);
		this.panel1.Controls.Add(this.lblSummaryPageHeader);
		resources.ApplyResources(this.panel1, "panel1");
		this.panel1.Name = "panel1";
		resources.ApplyResources(this.panelLicenseInfoSummary, "panelLicenseInfoSummary");
		this.panelLicenseInfoSummary.Name = "panelLicenseInfoSummary";
		resources.ApplyResources(this.lblSummaryDescription, "lblSummaryDescription");
		this.lblSummaryDescription.Name = "lblSummaryDescription";
		this.lblSummaryDescription.TabStop = true;
		resources.ApplyResources(this.lblSummaryPageHeader, "lblSummaryPageHeader");
		this.lblSummaryPageHeader.Name = "lblSummaryPageHeader";
		resources.ApplyResources(this.logoHorizontalResizableFailover, "logoHorizontalResizableFailover");
		this.logoHorizontalResizableFailover.Name = "logoHorizontalResizableFailover";
		resources.ApplyResources(this.btnHelp, "btnHelp");
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.UseVisualStyleBackColor = true;
		this.btnHelp.Click += new System.EventHandler(btnHelp_Click);
		this.ultraTabPageControl11.Controls.Add(this.panel4);
		resources.ApplyResources(this.ultraTabPageControl11, "ultraTabPageControl11");
		this.ultraTabPageControl11.Name = "ultraTabPageControl11";
		this.panel4.Controls.Add(this.lblDeconfigurationDescription);
		this.panel4.Controls.Add(this.lblDeconfigurationSuccessfulHeader);
		resources.ApplyResources(this.panel4, "panel4");
		this.panel4.Name = "panel4";
		resources.ApplyResources(this.lblDeconfigurationDescription, "lblDeconfigurationDescription");
		this.lblDeconfigurationDescription.Name = "lblDeconfigurationDescription";
		resources.ApplyResources(this.lblDeconfigurationSuccessfulHeader, "lblDeconfigurationSuccessfulHeader");
		this.lblDeconfigurationSuccessfulHeader.Name = "lblDeconfigurationSuccessfulHeader";
		this.Wizard.Controls.Add(this.TabSharedControlsPage);
		this.Wizard.Controls.Add(this.TagPageSelectNodeRole);
		this.Wizard.Controls.Add(this.ultraTabPageControlConnectionType);
		this.Wizard.Controls.Add(this.ultraTabPageControl1);
		this.Wizard.Controls.Add(this.ultraTabPageControl4);
		this.Wizard.Controls.Add(this.ultraTabPageControl5);
		this.Wizard.Controls.Add(this.ultraTabPageControl6);
		this.Wizard.Controls.Add(this.ultraTabPageControl7);
		this.Wizard.Controls.Add(this.ultraTabPageControl2);
		this.Wizard.Controls.Add(this.ultraTabPageControl8);
		this.Wizard.Controls.Add(this.ultraTabPageControl9);
		this.Wizard.Controls.Add(this.ultraTabPageControl10);
		this.Wizard.Controls.Add(this.ultraTabPageControl11);
		this.Wizard.Controls.Add(this.ultraTabPageControl3);
		this.Wizard.Controls.Add(this.ultraTabPageControl12);
		resources.ApplyResources(this.Wizard, "Wizard");
		this.Wizard.Name = "Wizard";
		this.Wizard.ScrollButtons = Infragistics.Win.UltraWinTabs.TabScrollButtons.None;
		this.Wizard.SharedControls.AddRange(new System.Windows.Forms.Control[2] { this.logoHorizontalResizableFailover, this.btnHelp });
		this.Wizard.SharedControlsPage = this.TabSharedControlsPage;
		this.Wizard.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard;
		ultraTab.Key = "NodeSelectionPage";
		ultraTab.TabPage = this.TagPageSelectNodeRole;
		resources.ApplyResources(ultraTab, "ultraTab1");
		ultraTab.ForceApplyResources = "";
		ultraTab2.Key = "MaintenanceModePage";
		ultraTab2.TabPage = this.ultraTabPageControl9;
		resources.ApplyResources(ultraTab2, "ultraTab11");
		ultraTab2.ForceApplyResources = "";
		ultraTab3.Key = "WebServerConfigurationPage";
		ultraTab3.TabPage = this.ultraTabPageControl3;
		resources.ApplyResources(ultraTab3, "ultraTab4");
		ultraTab3.ForceApplyResources = "";
		ultraTab4.Key = "ConnectionMethodPage";
		ultraTab4.TabPage = this.ultraTabPageControlConnectionType;
		resources.ApplyResources(ultraTab4, "ultraTab6");
		ultraTab4.ForceApplyResources = "";
		ultraTab5.Key = "SecondaryPrepareSuccess";
		ultraTab5.TabPage = this.ultraTabPageControl1;
		resources.ApplyResources(ultraTab5, "ultraTab2");
		ultraTab5.ForceApplyResources = "";
		ultraTab6.Key = "PrimarySqlReplicationFolders";
		ultraTab6.TabPage = this.ultraTabPageControl12;
		resources.ApplyResources(ultraTab6, "ultraTab14");
		ultraTab6.ForceApplyResources = "";
		ultraTab7.Key = "PrimaryEnterConfiguration";
		ultraTab7.TabPage = this.ultraTabPageControl4;
		resources.ApplyResources(ultraTab7, "ultraTab5");
		ultraTab7.ForceApplyResources = "";
		ultraTab8.Key = "PrimaryEnterCertificateFolder";
		ultraTab8.TabPage = this.ultraTabPageControl5;
		resources.ApplyResources(ultraTab8, "ultraTab7");
		ultraTab8.ForceApplyResources = "";
		ultraTab9.Key = "PrimaryPrepareSuccess";
		ultraTab9.TabPage = this.ultraTabPageControl6;
		resources.ApplyResources(ultraTab9, "ultraTab8");
		ultraTab9.ForceApplyResources = "";
		ultraTab10.Key = "SecondarySelectLicensePage";
		ultraTab10.TabPage = this.ultraTabPageControl10;
		resources.ApplyResources(ultraTab10, "ultraTab12");
		ultraTab10.ForceApplyResources = "";
		ultraTab11.Key = "SecondaryConfigurationComplete";
		ultraTab11.TabPage = this.ultraTabPageControl7;
		resources.ApplyResources(ultraTab11, "ultraTab9");
		ultraTab11.ForceApplyResources = "";
		ultraTab12.Key = "ProcessingPage";
		ultraTab12.TabPage = this.ultraTabPageControl8;
		resources.ApplyResources(ultraTab12, "ultraTab10");
		ultraTab12.ForceApplyResources = "";
		ultraTab13.Key = "ErrorPage";
		ultraTab13.TabPage = this.ultraTabPageControl2;
		resources.ApplyResources(ultraTab13, "ultraTab3");
		ultraTab13.ForceApplyResources = "";
		ultraTab14.Key = "DeconfigurationCompletePage";
		ultraTab14.TabPage = this.ultraTabPageControl11;
		resources.ApplyResources(ultraTab14, "ultraTab13");
		ultraTab14.ForceApplyResources = "";
		this.Wizard.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[14]
		{
			ultraTab, ultraTab2, ultraTab3, ultraTab4, ultraTab5, ultraTab6, ultraTab7, ultraTab8, ultraTab9, ultraTab10,
			ultraTab11, ultraTab12, ultraTab13, ultraTab14
		});
		this.Wizard.TabStop = false;
		this.TabSharedControlsPage.Controls.Add(this.logoHorizontalResizableFailover);
		this.TabSharedControlsPage.Controls.Add(this.btnHelp);
		this.TabSharedControlsPage.Controls.Add(this.lblSeparator);
		this.TabSharedControlsPage.Controls.Add(this.btnNext);
		this.TabSharedControlsPage.Controls.Add(this.btnCancel);
		resources.ApplyResources(this.TabSharedControlsPage, "TabSharedControlsPage");
		this.TabSharedControlsPage.Name = "TabSharedControlsPage";
		resources.ApplyResources(this.lblSeparator, "lblSeparator");
		this.lblSeparator.BackColor = System.Drawing.SystemColors.GrayText;
		this.lblSeparator.Name = "lblSeparator";
		resources.ApplyResources(this.btnNext, "btnNext");
		this.btnNext.Name = "btnNext";
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		resources.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.Wizard);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "FailoverWizardForm";
		base.Load += new System.EventHandler(FailoverWizardForm_Load);
		this.TagPageSelectNodeRole.ResumeLayout(false);
		this.pnlSelectNodeRole.ResumeLayout(false);
		this.pnlSelectNodeRole.PerformLayout();
		this.pnlExternalSql.ResumeLayout(false);
		this.pnlExternalSql.PerformLayout();
		this.pnlExternalSqlWarning.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBoxWarning).EndInit();
		this.ultraTabPageControl9.ResumeLayout(false);
		this.pnlMaintenanceMode.ResumeLayout(false);
		this.pnlMaintenanceMode.PerformLayout();
		this.ultraTabPageControl3.ResumeLayout(false);
		this.pnlWebServerPassword.ResumeLayout(false);
		this.pnlWebServerPassword.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pbWebServerUsername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbWebServerPassword).EndInit();
		this.ultraTabPageControlConnectionType.ResumeLayout(false);
		this.panelSelectConnectionType.ResumeLayout(false);
		this.panelSelectConnectionType.PerformLayout();
		this.ultraTabPageControl1.ResumeLayout(false);
		this.panelSecondaryPrepareHttp.ResumeLayout(false);
		this.flowLayoutPanelSecondarySuccess.ResumeLayout(false);
		this.flowLayoutPanelSecondarySuccess.PerformLayout();
		this.panelSecurityCode.ResumeLayout(false);
		this.panelSecurityCode.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pbHelpIconSecurityCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxCopyCode).EndInit();
		this.ultraTabPageControl12.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.pnlSqlFolders.ResumeLayout(false);
		this.pnlSqlFolders.PerformLayout();
		this.ultraTabPageControl4.ResumeLayout(false);
		this.panelPrimaryEnterConfiguration.ResumeLayout(false);
		this.panelPrimaryEnterConfiguration.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pbEnterSecurityCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbVirtualIp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbLicenseInfo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbFqdn).EndInit();
		this.ultraTabPageControl5.ResumeLayout(false);
		this.panelPrimaryCertificateFolder.ResumeLayout(false);
		this.panelPrimaryCertificateFolder.PerformLayout();
		this.ultraTabPageControl6.ResumeLayout(false);
		this.panelPrimaryPrepareSuccess.ResumeLayout(false);
		this.ultraTabPageControl10.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pbLicenseInformationOnPage).EndInit();
		this.ultraTabPageControl7.ResumeLayout(false);
		this.panelSecondaryConfigurationComplete.ResumeLayout(false);
		this.panelSecondaryConfigurationComplete.PerformLayout();
		this.ultraTabPageControl8.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.ultraTabPageControl2.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.ultraTabPageControl11.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.Wizard).EndInit();
		this.Wizard.ResumeLayout(false);
		this.TabSharedControlsPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

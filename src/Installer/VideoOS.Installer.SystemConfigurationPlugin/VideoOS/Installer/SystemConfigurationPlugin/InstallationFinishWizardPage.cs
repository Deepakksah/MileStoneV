using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class InstallationFinishWizardPage : WizardPageBase
{
	private bool _runApp;

	private bool _showDownloadManager;

	private string _dependentPackId;

	private ElementHost _elementHost;

	private FinishPageControl _wpfControl;

	private InstallationFinishPageViewModel _viewModel;

	private IInstallerManager _installerManager = new InternalInstallerManager();

	private static Guid FinishedInstallationPageFlowId = new Guid("7C959F47-B26D-4278-B63E-8E51614488D8");

	private IContainer components;

	public InstallationFinishWizardPage()
	{
		InitializeComponent();
		PageName = "FinishInstallationPage";
		base.Name = "FinishInstallationPage";
		WizardPage.FlowId = FinishedInstallationPageFlowId;
		if (InstallerManager.Instance.IsModify || InstallerManager.Instance.IsUninstall)
		{
			WizardPage.FinishPage = false;
		}
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new FinishPageControl();
		_elementHost.Child = _wpfControl;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
	}

	public override void OnBeforeCancel()
	{
		WizardPage.FinishPage = true;
		PostCloseAction();
	}

	private void PostCloseAction()
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			GlobalData.SetValue("PREPAREFINALIZE", _viewModel.PrepareFinalize.ToString(), "PostCloseAction()");
			GlobalData.SetValue("REMOVEINSTALLLOG", _viewModel.RemoveInstallLog.ToString(), "PostCloseAction()");
			return;
		}
		if (SupportsConfiguration())
		{
			StartManagmentClient();
			return;
		}
		if (_runApp && _viewModel.OpenClient)
		{
			ApplicationLaunchHelper.OpenClientAction(_dependentPackId, InstallerManager.Instance);
		}
		if (_showDownloadManager && _viewModel.OpenDownloadManager)
		{
			ApplicationLaunchHelper.OpenGettingStartedAction(InstallerManager.Instance);
		}
	}

	private static void StartManagmentClient()
	{
		if (InstallerManager.Instance.InstallerConfig.InstallerSettings.FlowSettings20 == null || InstallerManager.Instance.InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings == null)
		{
			return;
		}
		TreckType treckType = InstallerManager.Instance.InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks.FirstOrDefault((TreckType p) => p.Id == "Custom");
		if (treckType == null)
		{
			return;
		}
		string appExec = treckType.FinishSetup.RunApp.AppExec;
		string dependentPackId = string.Empty;
		if (treckType.FinishSetup.RunApp.DependentPackage != null && !string.IsNullOrEmpty(treckType.FinishSetup.RunApp.DependentPackage.ID))
		{
			if (InstallerManager.Instance.SequenceManager.InstalledPackages == null)
			{
				return;
			}
			dependentPackId = treckType.FinishSetup.RunApp.DependentPackage.ID;
		}
		ApplicationLaunchHelper.OpenClientAction(dependentPackId, appExec, InstallerManager.Instance);
	}

	private void Wizard_CloseFromCancel(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		WizardPage.FinishPage = true;
		FlowManager.TriggerEvent(null);
	}

	private void wizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.NavigationPanel.Show();
		Wizard.CloseFromCancel -= Wizard_CloseFromCancel;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		GlobalData.SetValue("INSTALLFINISHED", true.ToString(), "InstallationFinishWizardPage.WizardPage_PageActivated()");
		Wizard.CloseFromCancel += Wizard_CloseFromCancel;
		_dependentPackId = InstallerManager.Instance.SetupFinishParams();
		_runApp = InstallerManager.Instance.Parameters.ContainsKey("Run Application");
		_showDownloadManager = InstallerManager.Instance.Parameters.ContainsKey("Getting Started");
		UpdateWizardButtons();
		UpdateFinishPageViewModel();
		SetInstalledComponents();
	}

	private void UpdateWizardButtons()
	{
		Wizard.NextButton.Visible = SupportsConfiguration();
		Wizard.CancelButton.Visible = true;
		Wizard.CancelButton.Text = Resources.SkipConfigurationButtonText;
		Wizard.BackButton.Visible = false;
	}

	private void SetInstalledComponents()
	{
		IEnumerable<string> enumerable = new List<string>();
		enumerable = ((!InstallerManager.Instance.IsModify) ? InstallationComponentHelper.GetInstalledComponentsOnInstall() : InstallationComponentHelper.GetInstalledComponentsOnModify());
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			_viewModel.SummaryVisibility = false;
			_viewModel.HeaderInfo = string.Format(CultureInfo.InvariantCulture, Resources.FinishInstallationPage_HardwareManufacturerMode, VersionFormat.GenerateProductNameWithVersion());
		}
		else if (enumerable.Count() == 0)
		{
			_viewModel.SummaryVisibility = false;
			_viewModel.HeaderInfo = string.Format(CultureInfo.InvariantCulture, Resources.FinishInstallationPageAllInstalled, VersionFormat.GenerateProductNameWithVersion());
		}
		else
		{
			_viewModel.ContentViewModel = new InstallationSummaryControlViewModel(enumerable);
		}
	}

	private void UpdateFinishPageViewModel()
	{
		if (SupportsConfiguration())
		{
			_viewModel = new InstallationFinishPageViewModel(singleComputerInstall: true, _installerManager);
			_wpfControl.DataContext = _viewModel;
			return;
		}
		if (InstallerManager.Instance.IsModify)
		{
			_viewModel = new ModifiedInstallationFinishPageViewModel(_installerManager);
			_wpfControl.DataContext = _viewModel;
		}
		else
		{
			_viewModel = new InstallationFinishPageViewModel(singleComputerInstall: false, _installerManager);
			_wpfControl.DataContext = _viewModel;
		}
		bool flag = InstallerManager.Instance.Parameters.ContainsKey("Run Application Title") && !GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		_viewModel.OpenClientVisibility = flag;
		if (flag)
		{
			_viewModel.OpenClient = _runApp;
			_viewModel.ClientLabel = string.Format(CultureInfo.InvariantCulture, Resources.FinishPageOpenClientLabel, InstallerManager.Instance.Parameters["Run Application Title"]);
		}
		_viewModel.OpenDownloadManagerVisibility = _showDownloadManager;
		if (_showDownloadManager)
		{
			string downloadManagerLabel = ((!InstallerManager.Instance.Parameters.ContainsKey("Getting Started Title")) ? "" : ((InstallerManager.Instance.Parameters["Getting Started Title"] == LinkTitleType.DownloadComponent.ToString()) ? Resources.FinishInstallationPageOpenDownloadManager : ""));
			_viewModel.DownloadManagerLabel = downloadManagerLabel;
		}
	}

	private static bool SupportsConfiguration()
	{
		string value = null;
		if (GlobalData.TryGetValue("SUPPORTSCONFIGURATION", out value))
		{
			return bool.Parse(value);
		}
		return false;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.SystemConfigurationPlugin.InstallationFinishWizardPage));
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("f3045f08-8bc0-44cc-86ef-7c9f2569ad4f");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.ShowFromNext += new System.EventHandler(wizardPage_ShowFromNext);
		base.Name = "WizardPageFinish";
		this.PageName = "FinishPage";
		base.ResumeLayout(false);
	}
}

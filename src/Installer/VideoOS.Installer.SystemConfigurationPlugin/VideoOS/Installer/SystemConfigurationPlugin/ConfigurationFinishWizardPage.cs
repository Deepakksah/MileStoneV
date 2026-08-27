using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Manager;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ConfigurationFinishWizardPage : WizardPageBase
{
	private string _dependentPackId;

	private ElementHost _elementHost;

	private FinishPageControl _wpfControl;

	private ConfigurationFinishPageViewModel _viewModel;

	private IInstallerManager _installerManager = new InternalInstallerManager();

	private static Guid FinishedConfigurationPageFlowId = new Guid("3101CF9D-75AA-4856-BCB6-70BC46D30A25");

	private IContainer components;

	public ConfigurationFinishWizardPage()
	{
		InitializeComponent();
		PageName = "FinishConfigurationPage";
		base.Name = "FinishConfigurationPage";
		WizardPage.FlowId = FinishedConfigurationPageFlowId;
		if (InstallerManager.Instance.IsModify || InstallerManager.Instance.IsUninstall)
		{
			WizardPage.FinishPage = false;
		}
		else
		{
			WizardPage.FinishPage = true;
		}
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new FinishPageControl();
		_elementHost.Child = _wpfControl;
		LoadResources();
	}

	private void LoadResources()
	{
		_viewModel = new ConfigurationFinishPageViewModel(_installerManager);
		_wpfControl.DataContext = _viewModel;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override void OnBeforeCancel()
	{
		PostCloseAction();
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		List<SingleSummaryControlViewModel> list = new List<SingleSummaryControlViewModel>();
		list.Add(GetHardwareSummary(parameters));
		SingleSummaryControlViewModel userSummarary = GetUserSummarary(parameters);
		if (userSummarary != null)
		{
			list.Add(userSummarary);
		}
		SingleSummaryControlViewModel failedUserSummary = GetFailedUserSummary(parameters);
		if (failedUserSummary != null)
		{
			list.Add(failedUserSummary);
		}
		SetConfiguredContent(_viewModel, list);
	}

	private static SingleSummaryControlViewModel GetHardwareSummary(Dictionary<string, string> parameters)
	{
		IEnumerable<string> items = new List<string>();
		string value = "";
		if (parameters.TryGetValue(ServerParameters.ConfiguredHardware.ToString(), out value))
		{
			items = from c in HardwareConfigurations.GetFromXml(value)
				where c.Added
				select c.Name into n
				orderby n
				select n;
		}
		return new SingleSummaryControlViewModel(items, Resources.ConfigurationSummaryCameraHeader)
		{
			EmptySummaryDescription = Resources.FinishConfigurationPageNoHardwareDescription
		};
	}

	private static SingleSummaryControlViewModel GetUserSummarary(Dictionary<string, string> parameters)
	{
		IEnumerable<string> enumerable = new List<string>();
		string value = string.Empty;
		if (parameters.TryGetValue(ServerParameters.AddedUsers.ToString(), out value))
		{
			enumerable = from u in SystemConfigurationHelper.ConvertCsvStringToCollection(value)
				orderby u
				select u;
		}
		if (enumerable.Count() > 0)
		{
			return new SingleSummaryControlViewModel(enumerable, Resources.ConfigurationSummaryUserHeader);
		}
		return null;
	}

	private static SingleSummaryControlViewModel GetFailedUserSummary(Dictionary<string, string> parameters)
	{
		new List<string>();
		string value = string.Empty;
		if (parameters.TryGetValue(ServerParameters.FailedUsers.ToString(), out value))
		{
			return new SingleSummaryControlViewModel(from f in SystemConfigurationHelper.ConvertCsvStringToCollection(value)
				orderby f
				select f, Resources.ConfigurationSummaryUserFailedHeader);
		}
		return null;
	}

	private void PostCloseAction()
	{
		if (_viewModel.OpenClient)
		{
			ApplicationLaunchHelper.OpenClientAction(_dependentPackId, InstallerManager.Instance);
		}
	}

	private void Wizard_CloseFromCancel(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		FlowManager.TriggerEvent(null);
	}

	private void wizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.NavigationPanel.Show();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CloseFromCancel += Wizard_CloseFromCancel;
		_dependentPackId = InstallerManager.Instance.SetupFinishParams();
		UpdateWizardButtons();
		UpdateFinishPageProperties();
	}

	private void UpdateWizardButtons()
	{
		Wizard.CancelButton.Visible = true;
		Wizard.CancelButton.Text = Resources.CloseAfterConfigurationButtonText;
		Wizard.NextButton.Visible = false;
		Wizard.BackButton.Visible = false;
	}

	private static void SetConfiguredContent(ConfigurationFinishPageViewModel viewModel, List<SingleSummaryControlViewModel> summaries)
	{
		viewModel.SetSummary(new ConfigurationSummaryControlViewModel(summaries));
	}

	private void UpdateFinishPageProperties()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("Run Application") && InstallerManager.Instance.Parameters.ContainsKey("Run Application Title"))
		{
			_viewModel.OpenClientVisibility = true;
			_viewModel.OpenClient = true;
			_viewModel.ClientLabel = string.Format(CultureInfo.InvariantCulture, Resources.FinishPageOpenClientLabel, InstallerManager.Instance.Parameters["Run Application Title"]);
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.SystemConfigurationPlugin.InstallationFinishWizardPage));
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FlowId = new System.Guid("f3045f08-8bc0-44cc-86ef-7c9f2569ad4f");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.ShowFromNext += new System.EventHandler(wizardPage_ShowFromNext);
		base.Name = "WizardPageFinish";
		this.PageName = "FinishPage";
		base.ResumeLayout(false);
	}
}

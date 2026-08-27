using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageFinish : WizardPageBase
{
	private bool _runApp;

	private bool _gettingStarted;

	private string _dependentPackId;

	private IContainer components;

	private Label labelHeadline;

	private Label labelSubHeading;

	private ListView listViewInstalledComponents;

	private Button buttonDone;

	private CheckBox checkBoxGettingStarted;

	private CheckBox checkBoxOpenApp;

	public WizardPageFinish()
	{
		InitializeComponent();
		labelSubHeading.UseMnemonic = false;
		LoadResources();
	}

	private void LoadResources()
	{
		labelHeadline.Text = (InstallerManager.Instance.IsModify ? Resources.Finish_ModifyLabelHeadLine : string.Format(Resources.Finish_LabelHeadLine));
		labelSubHeading.Text = (InstallerManager.Instance.IsModify ? Resources.Finish_LabelComponnetsPresent : string.Format(Resources.Finish_LabelfinishComponentInstall));
		buttonDone.Text = string.Format(Resources.Finish_ButtonClose);
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

	private void wizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.NavigationPanel.Hide();
	}

	private void buttonDone_Click(object sender, EventArgs e)
	{
		FlowManager.TriggerEvent(null);
	}

	private void PostCloseAction()
	{
		if (_runApp && checkBoxOpenApp.Checked)
		{
			ApplicationLaunchHelper.OpenClientAction(_dependentPackId, InstallerManager.Instance);
		}
		if (_gettingStarted && checkBoxGettingStarted.Checked)
		{
			ApplicationLaunchHelper.OpenGettingStartedAction(InstallerManager.Instance);
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		_dependentPackId = InstallerManager.Instance.SetupFinishParams();
		_runApp = InstallerManager.Instance.Parameters.ContainsKey("Run Application");
		_gettingStarted = InstallerManager.Instance.Parameters.ContainsKey("Getting Started");
		checkBoxOpenApp.Visible = InstallerManager.Instance.Parameters.ContainsKey("Run Application Title");
		checkBoxOpenApp.Checked = _runApp;
		checkBoxOpenApp.Enabled = _runApp;
		checkBoxGettingStarted.Visible = _gettingStarted;
		if (checkBoxOpenApp.Visible)
		{
			string text = string.Format(checkBoxOpenApp.Text, InstallerManager.Instance.Parameters["Run Application Title"]);
			checkBoxOpenApp.Text = text;
		}
		if (_gettingStarted)
		{
			string text2 = ((!InstallerManager.Instance.Parameters.ContainsKey("Getting Started Title")) ? Resources.Finish_GettingStartedLabel : ((InstallerManager.Instance.Parameters["Getting Started Title"] == LinkTitleType.DownloadComponent.ToString()) ? Resources.Finish_OpenDownloadLink : Resources.Finish_GettingStartedLabel));
			checkBoxGettingStarted.Text = text2;
		}
		IEnumerable<string> enumerable = ((!InstallerManager.Instance.IsModify) ? InstallationComponentHelper.GetInstalledComponentsOnInstall() : InstallationComponentHelper.GetInstalledComponentsOnModify());
		foreach (string item in enumerable)
		{
			listViewInstalledComponents.Items.Add(item);
		}
		if (listViewInstalledComponents.Items.Count == 0)
		{
			listViewInstalledComponents.Hide();
			labelSubHeading.Text = string.Format(Resources.Finish_AllInstalled, VersionFormat.GenerateProductNameWithVersion());
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageFinish));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.listViewInstalledComponents = new System.Windows.Forms.ListView();
		this.buttonDone = new System.Windows.Forms.Button();
		this.checkBoxOpenApp = new System.Windows.Forms.CheckBox();
		this.checkBoxGettingStarted = new System.Windows.Forms.CheckBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.checkBoxGettingStarted);
		base.WizardPage.Controls.Add(this.checkBoxOpenApp);
		base.WizardPage.Controls.Add(this.buttonDone);
		base.WizardPage.Controls.Add(this.listViewInstalledComponents);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelHeadline);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = true;
		base.WizardPage.FlowId = new System.Guid("f3045f08-8bc0-44cc-86ef-7c9f2569ad4f");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.ShowFromNext += new System.EventHandler(wizardPage_ShowFromNext);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.labelSubHeading, "labelSubHeading");
		this.labelSubHeading.Name = "labelSubHeading";
		componentResourceManager.ApplyResources(this.listViewInstalledComponents, "listViewInstalledComponents");
		this.listViewInstalledComponents.AllowColumnReorder = true;
		this.listViewInstalledComponents.FullRowSelect = true;
		this.listViewInstalledComponents.GridLines = true;
		this.listViewInstalledComponents.Name = "listViewInstalledComponents";
		this.listViewInstalledComponents.UseCompatibleStateImageBehavior = false;
		this.listViewInstalledComponents.View = System.Windows.Forms.View.List;
		componentResourceManager.ApplyResources(this.buttonDone, "buttonDone");
		this.buttonDone.Name = "buttonDone";
		this.buttonDone.Click += new System.EventHandler(buttonDone_Click);
		componentResourceManager.ApplyResources(this.checkBoxOpenApp, "checkBoxOpenApp");
		this.checkBoxOpenApp.Checked = true;
		this.checkBoxOpenApp.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxOpenApp.Name = "checkBoxOpenApp";
		this.checkBoxOpenApp.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.checkBoxGettingStarted, "checkBoxGettingStarted");
		this.checkBoxGettingStarted.Checked = true;
		this.checkBoxGettingStarted.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxGettingStarted.Name = "checkBoxGettingStarted";
		this.checkBoxGettingStarted.UseVisualStyleBackColor = true;
		base.Name = "WizardPageFinish";
		this.PageName = "FinishPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

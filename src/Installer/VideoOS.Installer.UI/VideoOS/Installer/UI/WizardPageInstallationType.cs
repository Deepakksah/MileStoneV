using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageInstallationType : WizardPageBase
{
	private string _trackKeyTypical;

	private string _trackKeyCustom;

	private bool _customMode;

	private IContainer components;

	private Label labelTypicalInstallDescription;

	private Button buttonInstallCustom;

	private Button buttonInstallTypical;

	private Label labelHeadline;

	private Label labelCustomInstallDescription;

	public WizardPageInstallationType(string trackKeyTypical, string trackKeyCustom)
		: this()
	{
		_trackKeyTypical = trackKeyTypical;
		_trackKeyCustom = trackKeyCustom;
	}

	public WizardPageInstallationType()
	{
		InitializeComponent();
		labelTypicalInstallDescription.UseMnemonic = false;
		LoadResources();
	}

	private void buttonInstallTypical_Click(object sender, EventArgs e)
	{
		try
		{
			InstallerManager.Instance.GetFreeSpace(null);
		}
		catch (Win32Exception exception)
		{
			ErrorHandler.ShowError(exception);
			return;
		}
		if (InstallerManager.Instance.IsEnoughOfSpaceOnDrive() != 0.0)
		{
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_NotEnoughSpaceHeadLine, string.Format(Resources.ErrorMessage_NotEnoughSpaceSubLine, VersionFormat.GenerateProductNameWithVersion(), InstallerManager.Instance.GetRequiredSpace(), InstallerManager.Instance.SpaceAvailableOnDrive), null, null, enablePreviousButton: true);
			ErrorHandler.ShowError();
			return;
		}
		_customMode = false;
		GlobalData.SetValue("INSTALLATION_MODE", "TypicalMode", "buttonInstallTypical_Click()");
		Wizard.NavigationPanel.Show();
		if (Wizard.OnButtonNext != null)
		{
			Wizard.OnButtonNext(Wizard);
		}
	}

	private void buttonInstallCustom_Click(object sender, EventArgs e)
	{
		_customMode = true;
		GlobalData.SetValue("INSTALLATION_MODE", "CustomMode", "buttonInstallCustom_Click()");
		Wizard.NavigationPanel.Show();
		if (Wizard.OnButtonNext != null)
		{
			Wizard.OnButtonNext(Wizard);
		}
	}

	public override string OnTriggerEvent()
	{
		if (!_customMode)
		{
			return "TypicalInstall";
		}
		return null;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (_customMode && !string.IsNullOrEmpty(_trackKeyCustom))
		{
			dictionary.Add("TRECK_ID", _trackKeyCustom);
		}
		else if (!string.IsNullOrEmpty(_trackKeyTypical))
		{
			dictionary.Add("TRECK_ID", _trackKeyTypical);
		}
		return dictionary;
	}

	private void WizardPage_ShowFromBack(object sender, EventArgs e)
	{
		Wizard.NavigationPanel.Hide();
	}

	private void WizardPage_ShowFromNext(object sender, EventArgs e)
	{
		Wizard.NavigationPanel.Hide();
	}

	private void WizardPage_CloseFromBack(object sender, PageEventArgs e)
	{
		Wizard.NavigationPanel.Show();
	}

	private void WizardPage_CloseFromNext(object sender, PageEventArgs e)
	{
		Wizard.NavigationPanel.Show();
	}

	private void LoadResources()
	{
		string arg = VersionFormat.GenerateProductNameWithVersion();
		labelCustomInstallDescription.Text = (InstallerManager.Instance.InstallerConfig.InstallerInfo.GroupInstaller ? Resources.InstallationType_LabelInstallCustomGroupDescription : Resources.InstallationType_LabelInstallCustomCompDescription);
		labelHeadline.Text = Resources.InstallationType_LabelHeadLine;
		labelTypicalInstallDescription.Text = (InstallerManager.Instance.InstallerConfig.InstallerInfo.GroupInstaller ? string.Format(Resources.InstallationType_LabelInstallTypicalGroupDescription, arg) : string.Format(Resources.InstallationType_LabelInstallTypicalCompDescription, arg));
		buttonInstallCustom.Text = Resources.InstallationType_ButtonInstallCustom;
		buttonInstallTypical.Text = Resources.InstallationType_ButtonInstallTypical;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageInstallationType));
		this.buttonInstallTypical = new System.Windows.Forms.Button();
		this.buttonInstallCustom = new System.Windows.Forms.Button();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelTypicalInstallDescription = new System.Windows.Forms.Label();
		this.labelCustomInstallDescription = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelCustomInstallDescription);
		base.WizardPage.Controls.Add(this.labelTypicalInstallDescription);
		base.WizardPage.Controls.Add(this.buttonInstallCustom);
		base.WizardPage.Controls.Add(this.buttonInstallTypical);
		base.WizardPage.Controls.Add(this.labelHeadline);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.CloseFromBack += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_CloseFromBack);
		base.WizardPage.CloseFromNext += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_CloseFromNext);
		base.WizardPage.ShowFromBack += new System.EventHandler(WizardPage_ShowFromBack);
		base.WizardPage.ShowFromNext += new System.EventHandler(WizardPage_ShowFromNext);
		componentResourceManager.ApplyResources(this.buttonInstallTypical, "buttonInstallTypical");
		this.buttonInstallTypical.Name = "buttonInstallTypical";
		this.buttonInstallTypical.UseVisualStyleBackColor = true;
		this.buttonInstallTypical.Click += new System.EventHandler(buttonInstallTypical_Click);
		componentResourceManager.ApplyResources(this.buttonInstallCustom, "buttonInstallCustom");
		this.buttonInstallCustom.Name = "buttonInstallCustom";
		this.buttonInstallCustom.UseVisualStyleBackColor = true;
		this.buttonInstallCustom.Click += new System.EventHandler(buttonInstallCustom_Click);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.labelTypicalInstallDescription, "labelTypicalInstallDescription");
		this.labelTypicalInstallDescription.Name = "labelTypicalInstallDescription";
		componentResourceManager.ApplyResources(this.labelCustomInstallDescription, "labelCustomInstallDescription");
		this.labelCustomInstallDescription.Name = "labelCustomInstallDescription";
		base.Name = "WizardPageInstallationType";
		this.PageName = "InstallationTypePage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

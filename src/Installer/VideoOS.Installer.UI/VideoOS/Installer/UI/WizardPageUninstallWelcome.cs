using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageUninstallWelcome : WizardPageBase
{
	private delegate void SetContinueDelegate();

	private bool _isInit = true;

	private IContainer components;

	private Label labelHeadline;

	private RadioButton radioButtonUninstall;

	private RadioButton radioButtonModify;

	public WizardPageUninstallWelcome()
	{
		InitializeComponent();
		labelHeadline.UseMnemonic = false;
		radioButtonModify.UseMnemonic = false;
		radioButtonUninstall.UseMnemonic = false;
	}

	private void LoadResources()
	{
		labelHeadline.Text = string.Format(Resources.UninstallWelcome_PrepareModify, VersionFormat.GenerateProductNameWithVersionUninstallMode());
		new LabelTextAdaptor(labelHeadline).AdaptingFontSize();
		radioButtonModify.Text = string.Format(Resources.UninstallWelcome_Modify, VersionFormat.GenerateProductNameWithVersionUninstallMode());
		radioButtonUninstall.Text = string.Format(Resources.UninstallWelcome_Uninstall, VersionFormat.GenerateProductNameWithVersionUninstallMode());
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		LoadResources();
		Wizard.NavigationPanel.Show();
		int num = 0;
		PackageRefType[] installExecuteSequence = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (packageRefType.VisibilitySpecified && packageRefType.Visibility == VisibilityType.Shown)
			{
				num++;
			}
		}
		int noModifyKey = GetNoModifyKey();
		if (num <= 1 || noModifyKey == 1)
		{
			radioButtonModify.Enabled = false;
			radioButtonUninstall.Checked = true;
		}
		else
		{
			radioButtonUninstall.Checked = !InstallerManager.Instance.IsModify;
		}
		_isInit = false;
	}

	private static int GetNoModifyKey()
	{
		try
		{
			Guid guid = new Guid(InstallerManager.Instance.InstallerConfig.InstallerInfo.ID);
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine(VideoOS.Installer.Common.Properties.Settings.Default.SoftwareKey, guid.ToString("B", CultureInfo.InvariantCulture)));
			if (registryKey != null)
			{
				return Convert.ToInt32(registryKey.GetValue("NoModify"), CultureInfo.InvariantCulture);
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "GetNoModifyKey(): failed to get NoModify Key, Exception: {0}", arg), TraceEventType.Error);
		}
		return 0;
	}

	private void WizardPage_CloseFromNext(object sender, PageEventArgs e)
	{
		Wizard.BackButton.Enabled = false;
	}

	private void radioButtonModify_CheckedChanged(object sender, EventArgs e)
	{
		if (!_isInit)
		{
			InstallerManager.Instance.ModeChanged(radioButtonModify.Checked);
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string> { 
		{
			"TRECK_ID",
			radioButtonModify.Checked ? "modify" : "uninstall"
		} };
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageUninstallWelcome));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.radioButtonModify = new System.Windows.Forms.RadioButton();
		this.radioButtonUninstall = new System.Windows.Forms.RadioButton();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.radioButtonUninstall);
		base.WizardPage.Controls.Add(this.radioButtonModify);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("2106aa47-cb8d-45ae-a9d5-da1ff1dafabf");
		base.WizardPage.CloseFromNext += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_CloseFromNext);
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		this.radioButtonModify.Checked = true;
		componentResourceManager.ApplyResources(this.radioButtonModify, "radioButtonModify");
		this.radioButtonModify.Name = "radioButtonModify";
		this.radioButtonModify.TabStop = true;
		this.radioButtonModify.UseVisualStyleBackColor = true;
		this.radioButtonModify.CheckedChanged += new System.EventHandler(radioButtonModify_CheckedChanged);
		componentResourceManager.ApplyResources(this.radioButtonUninstall, "radioButtonUninstall");
		this.radioButtonUninstall.Name = "radioButtonUninstall";
		this.radioButtonUninstall.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageUninstallWelcome";
		this.PageName = "UninstallWelcomePage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

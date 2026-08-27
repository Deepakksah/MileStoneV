using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using VideoOS.Installer.RecorderPlugins.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.RecorderPlugins;

public class ServerUninstallControl : WizardPageBase
{
	private IContainer components;

	private Label labelHeadLine;

	private CheckBox checkBoxKeepArchives;

	private CheckBox checkBoxKeepDp;

	private static string CurrentDevicePackVersion
	{
		get
		{
			string result = "";
			try
			{
				using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
				using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\VideoOS\\DeviceDrivers");
				result = registryKey2.GetValue("VersionC").ToString();
			}
			catch
			{
			}
			return result;
		}
	}

	public ServerUninstallControl()
	{
		InitializeComponent();
		PageName = "ServerUninstallPage";
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		ReloadResourcesLocal();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		ReloadResourcesLocal();
	}

	private void ReloadResourcesLocal()
	{
		labelHeadLine.Text = Resources.ServerUninstall_HeadLine;
		string currentDevicePackVersion = CurrentDevicePackVersion;
		checkBoxKeepDp.Text = string.Format(CultureInfo.CurrentCulture, Resources.ServerUninstall_KeepDp, CurrentDevicePackVersion);
		checkBoxKeepDp.Checked = !string.IsNullOrEmpty(currentDevicePackVersion);
		checkBoxKeepDp.Enabled = !string.IsNullOrEmpty(currentDevicePackVersion);
		checkBoxKeepArchives.Text = Resources.ServerUninstall_KeepMediaArch;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string>
		{
			["REMOVEARCHIVES"] = (checkBoxKeepArchives.Checked ? "no" : "yes"),
			["REMOVEDEVICEPACK"] = (checkBoxKeepDp.Checked ? "false" : "true")
		};
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.ContainsKey("REMOVEARCHIVES"))
		{
			string text = parameters["REMOVEARCHIVES"];
			checkBoxKeepArchives.Checked = text == "no";
		}
		if (parameters.ContainsKey("REMOVEDEVICEPACK") && checkBoxKeepDp.Enabled)
		{
			string text2 = parameters["REMOVEDEVICEPACK"];
			checkBoxKeepDp.Checked = text2 == "false";
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
		this.labelHeadLine = new System.Windows.Forms.Label();
		this.checkBoxKeepArchives = new System.Windows.Forms.CheckBox();
		this.checkBoxKeepDp = new System.Windows.Forms.CheckBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.checkBoxKeepArchives);
		base.WizardPage.Controls.Add(this.labelHeadLine);
		base.WizardPage.Controls.Add(this.checkBoxKeepDp);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("45984772-2c05-466c-adb8-fbca0067e62e");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelHeadLine.AutoSize = true;
		this.labelHeadLine.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadLine.Location = new System.Drawing.Point(44, 41);
		this.labelHeadLine.Name = "labelHeadLine";
		this.labelHeadLine.Size = new System.Drawing.Size(262, 25);
		this.labelHeadLine.TabIndex = 0;
		this.labelHeadLine.Text = "Recording Server uninstall";
		this.checkBoxKeepArchives.AutoSize = true;
		this.checkBoxKeepArchives.Checked = true;
		this.checkBoxKeepArchives.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxKeepArchives.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBoxKeepArchives.Location = new System.Drawing.Point(50, 83);
		this.checkBoxKeepArchives.Name = "checkBoxKeepArchives";
		this.checkBoxKeepArchives.Size = new System.Drawing.Size(142, 18);
		this.checkBoxKeepArchives.TabIndex = 1;
		this.checkBoxKeepArchives.Text = "Keep Media database";
		this.checkBoxKeepArchives.UseVisualStyleBackColor = true;
		this.checkBoxKeepDp.AutoSize = true;
		this.checkBoxKeepDp.Checked = true;
		this.checkBoxKeepDp.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxKeepDp.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBoxKeepDp.Location = new System.Drawing.Point(50, 113);
		this.checkBoxKeepDp.Name = "checkBoxKeepDp";
		this.checkBoxKeepDp.Size = new System.Drawing.Size(260, 18);
		this.checkBoxKeepDp.TabIndex = 1;
		this.checkBoxKeepDp.Text = "Keep &Device Pack installed on the machine";
		this.checkBoxKeepDp.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ServerUninstallControl";
		this.PageName = "ServerUninstallPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

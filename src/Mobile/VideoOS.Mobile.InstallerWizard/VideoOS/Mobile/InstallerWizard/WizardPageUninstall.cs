using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Mobile.InstallerWizard;

public class WizardPageUninstall : WizardPageBase
{
	private Dictionary<string, string> _parameters;

	private IContainer components;

	private Label labelTitle;

	private GroupBox groupBoxLogs;

	private RadioButton radioButtonLogsDelete;

	private RadioButton radioButtonLogsKeep;

	private GroupBox groupBoxExports;

	private RadioButton radioButtonExportsDelete;

	private RadioButton radioButtonExportsKeep;

	private Label labelDescription;

	public WizardPageUninstall()
	{
		InitializeComponent();
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
		PageName = "UninstallPage";
		_parameters = new Dictionary<string, string>();
		WizardPage.PageActivated += WizardPage_PageActivated;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.BackButton.Enabled = true;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
	}

	public override Dictionary<string, string> CollectParameters()
	{
		_parameters["DELETEEXPORTS"] = radioButtonExportsDelete.Checked.ToString();
		_parameters["DELETELOGS"] = radioButtonLogsDelete.Checked.ToString();
		return _parameters;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Mobile.InstallerWizard.WizardPageUninstall));
		this.labelTitle = new System.Windows.Forms.Label();
		this.groupBoxLogs = new System.Windows.Forms.GroupBox();
		this.radioButtonLogsDelete = new System.Windows.Forms.RadioButton();
		this.radioButtonLogsKeep = new System.Windows.Forms.RadioButton();
		this.groupBoxExports = new System.Windows.Forms.GroupBox();
		this.radioButtonExportsDelete = new System.Windows.Forms.RadioButton();
		this.radioButtonExportsKeep = new System.Windows.Forms.RadioButton();
		this.labelDescription = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.groupBoxExports);
		base.WizardPage.Controls.Add(this.groupBoxLogs);
		base.WizardPage.Controls.Add(this.labelTitle);
		base.WizardPage.Controls.Add(this.labelDescription);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Name = "WizardPage";
		componentResourceManager.ApplyResources(this.labelTitle, "labelTitle");
		this.labelTitle.Name = "labelTitle";
		this.groupBoxLogs.Controls.Add(this.radioButtonLogsDelete);
		this.groupBoxLogs.Controls.Add(this.radioButtonLogsKeep);
		componentResourceManager.ApplyResources(this.groupBoxLogs, "groupBoxLogs");
		this.groupBoxLogs.Name = "groupBoxLogs";
		this.groupBoxLogs.TabStop = false;
		componentResourceManager.ApplyResources(this.radioButtonLogsDelete, "radioButtonLogsDelete");
		this.radioButtonLogsDelete.Name = "radioButtonLogsDelete";
		this.radioButtonLogsDelete.TabStop = true;
		this.radioButtonLogsDelete.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.radioButtonLogsKeep, "radioButtonLogsKeep");
		this.radioButtonLogsKeep.Checked = true;
		this.radioButtonLogsKeep.Name = "radioButtonLogsKeep";
		this.radioButtonLogsKeep.TabStop = true;
		this.radioButtonLogsKeep.UseVisualStyleBackColor = true;
		this.groupBoxExports.Controls.Add(this.radioButtonExportsDelete);
		this.groupBoxExports.Controls.Add(this.radioButtonExportsKeep);
		componentResourceManager.ApplyResources(this.groupBoxExports, "groupBoxExports");
		this.groupBoxExports.Name = "groupBoxExports";
		this.groupBoxExports.TabStop = false;
		componentResourceManager.ApplyResources(this.radioButtonExportsDelete, "radioButtonExportsDelete");
		this.radioButtonExportsDelete.Name = "radioButtonExportsDelete";
		this.radioButtonExportsDelete.TabStop = true;
		this.radioButtonExportsDelete.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.radioButtonExportsKeep, "radioButtonExportsKeep");
		this.radioButtonExportsKeep.Checked = true;
		this.radioButtonExportsKeep.Name = "radioButtonExportsKeep";
		this.radioButtonExportsKeep.TabStop = true;
		this.radioButtonExportsKeep.UseVisualStyleBackColor = true;
		componentResourceManager.ApplyResources(this.labelDescription, "labelDescription");
		this.labelDescription.Name = "labelDescription";
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.Name = "WizardPageUninstall";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

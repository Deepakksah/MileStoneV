using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.RecorderWarningPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.RecorderWarningPlugin;

public class RecorderWarningPageControl : WizardPageBase
{
	private Dictionary<string, string> _parameters;

	private IContainer components;

	private Label labelTitle;

	private Label labelReason;

	private Label labelKnowledgebaseDescription;

	private LinkLabel labelRecorderKBLink;

	private CheckBox checkBoxAccept;

	public RecorderWarningPageControl()
	{
		InitializeComponent();
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
		PageName = "RecorderWarningPage";
		_parameters = new Dictionary<string, string>();
		LoadResources();
	}

	private void LoadResources()
	{
		labelTitle.Text = Resources.StringHeading;
		labelKnowledgebaseDescription.Text = Resources.StringRecorderKBLinkDescription;
		labelReason.Text = Resources.StringReason;
		checkBoxAccept.Text = Resources.StringCheckBoxAccept;
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		try
		{
			labelRecorderKBLink.Text = OemResources.Instance.App.RecorderSecurityKnowledgebaseUrl;
			labelRecorderKBLink.Tag = OemResources.Instance.App.RecorderSecurityKnowledgebaseUrl;
		}
		catch (Exception ex)
		{
			throw new UIFrameworkErrorException(ex, enablePreviousButtong: false);
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return _parameters;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.NextEnabled = checkBoxAccept.Checked;
	}

	private void Wizard_checkBoxAccept_CheckedChanged(object sender, EventArgs e)
	{
		Wizard.NextEnabled = checkBoxAccept.Checked;
	}

	private void Wizard_DriverListLink_Clicked(object sender, EventArgs e)
	{
		labelRecorderKBLink.LinkVisited = true;
		string text = string.Empty;
		try
		{
			text = OemResources.Instance.App.RecorderSecurityKnowledgebaseUrl;
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(text))
		{
			Process.Start(text);
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
		this.labelTitle = new System.Windows.Forms.Label();
		this.labelReason = new System.Windows.Forms.Label();
		this.labelKnowledgebaseDescription = new System.Windows.Forms.Label();
		this.labelRecorderKBLink = new System.Windows.Forms.LinkLabel();
		this.checkBoxAccept = new System.Windows.Forms.CheckBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelTitle);
		base.WizardPage.Controls.Add(this.labelReason);
		base.WizardPage.Controls.Add(this.labelKnowledgebaseDescription);
		base.WizardPage.Controls.Add(this.labelRecorderKBLink);
		base.WizardPage.Controls.Add(this.checkBoxAccept);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("2c5d8c41-4999-442f-a486-d128f8f103cb");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelTitle.AutoSize = true;
		this.labelTitle.Font = new System.Drawing.Font("Arial", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelTitle.Location = new System.Drawing.Point(44, 40);
		this.labelTitle.Name = "labelTitle";
		this.labelTitle.Size = new System.Drawing.Size(87, 24);
		this.labelTitle.TabIndex = 0;
		this.labelTitle.Text = "Warning";
		this.labelReason.AutoSize = true;
		this.labelReason.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelReason.Location = new System.Drawing.Point(47, 100);
		this.labelReason.MaximumSize = new System.Drawing.Size(550, 0);
		this.labelReason.Name = "labelReason";
		this.labelReason.Size = new System.Drawing.Size(546, 60);
		this.labelReason.TabIndex = 1;
		this.labelReason.Tag = "MyCustomParameterMain";
		this.labelReason.Text = "An important security patch is available.\r\n\rMake sure all recording servers are updated with the security patch before you continue this upgrade. \r\n\rContinuing without updating the recording servers, will cause the recording servers to fail.";
		this.labelKnowledgebaseDescription.AutoSize = true;
		this.labelKnowledgebaseDescription.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelKnowledgebaseDescription.Location = new System.Drawing.Point(47, 225);
		this.labelKnowledgebaseDescription.MaximumSize = new System.Drawing.Size(550, 0);
		this.labelKnowledgebaseDescription.Name = "labelKnowledgebaseDescription";
		this.labelKnowledgebaseDescription.Size = new System.Drawing.Size(328, 15);
		this.labelKnowledgebaseDescription.TabIndex = 2;
		this.labelKnowledgebaseDescription.Tag = "MyCustomParameterMain";
		this.labelKnowledgebaseDescription.Text = "To update your recording servers, follow these instructions:";
		this.labelRecorderKBLink.AutoSize = true;
		this.labelRecorderKBLink.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelRecorderKBLink.Location = new System.Drawing.Point(47, 240);
		this.labelRecorderKBLink.MaximumSize = new System.Drawing.Size(550, 0);
		this.labelRecorderKBLink.Name = "labelRecorderKBLink";
		this.labelRecorderKBLink.Size = new System.Drawing.Size(101, 15);
		this.labelRecorderKBLink.TabIndex = 3;
		this.labelRecorderKBLink.TabStop = true;
		this.labelRecorderKBLink.Text = "";
		this.labelRecorderKBLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(Wizard_DriverListLink_Clicked);
		this.checkBoxAccept.Location = new System.Drawing.Point(50, 380);
		this.checkBoxAccept.Name = "checkBoxAccept";
		this.checkBoxAccept.Size = new System.Drawing.Size(500, 24);
		this.checkBoxAccept.TabIndex = 0;
		this.checkBoxAccept.Text = "The security patch is installed on all recording servers";
		this.checkBoxAccept.UseVisualStyleBackColor = true;
		this.checkBoxAccept.CheckedChanged += new System.EventHandler(Wizard_checkBoxAccept_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "RecorderWarningPageControl";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

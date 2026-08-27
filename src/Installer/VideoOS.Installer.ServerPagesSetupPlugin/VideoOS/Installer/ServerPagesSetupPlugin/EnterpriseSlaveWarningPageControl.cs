using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class EnterpriseSlaveWarningPageControl : WizardPageBase
{
	private IContainer components;

	private TableLayoutPanel panel;

	private CheckBox checkBoxAccept;

	private Label labelReason;

	private Label labelTitle;

	public EnterpriseSlaveWarningPageControl()
	{
		InitializeComponent();
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
		PageName = "EnterpriseSlaveWarningPage";
		LoadResources();
	}

	private void LoadResources()
	{
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EnterpriseSlaveWarningPageControl));
		labelTitle.Text = componentResourceManager.GetString("labelTitle.Text");
		labelReason.Text = string.Format(CultureInfo.CurrentCulture, componentResourceManager.GetString("labelReason.Text"), OemResources.Instance.App.LookupResourceString("LegacyApplicationName"));
		checkBoxAccept.Text = componentResourceManager.GetString("checkBoxAccept.Text");
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

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.NextEnabled = checkBoxAccept.Checked;
	}

	private void Wizard_checkBoxAccept_CheckedChanged(object sender, EventArgs e)
	{
		Wizard.NextEnabled = checkBoxAccept.Checked;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Resources are loaded at runtime", MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.ServerPagesSetupPlugin.EnterpriseSlaveWarningPageControl));
		this.panel = new System.Windows.Forms.TableLayoutPanel();
		this.checkBoxAccept = new System.Windows.Forms.CheckBox();
		this.labelReason = new System.Windows.Forms.Label();
		this.labelTitle = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		this.panel.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.panel);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("eb0cbd21-0ffa-4d7f-842b-c7336cfa968f");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this.panel, "panel");
		this.panel.Controls.Add(this.checkBoxAccept, 0, 4);
		this.panel.Controls.Add(this.labelReason, 0, 2);
		this.panel.Controls.Add(this.labelTitle, 0, 0);
		this.panel.Name = "panel";
		componentResourceManager.ApplyResources(this.checkBoxAccept, "checkBoxAccept");
		this.checkBoxAccept.Name = "checkBoxAccept";
		this.checkBoxAccept.UseVisualStyleBackColor = true;
		this.checkBoxAccept.CheckedChanged += new System.EventHandler(Wizard_checkBoxAccept_CheckedChanged);
		componentResourceManager.ApplyResources(this.labelReason, "labelReason");
		this.labelReason.Name = "labelReason";
		this.labelReason.Tag = "";
		componentResourceManager.ApplyResources(this.labelTitle, "labelTitle");
		this.labelTitle.Name = "labelTitle";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "EnterpriseSlaveWarningPageControl";
		base.WizardPage.ResumeLayout(false);
		this.panel.ResumeLayout(false);
		this.panel.PerformLayout();
		base.ResumeLayout(false);
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Server.Failover.Configurator.Properties;

namespace VideoOS.Server.Failover.Configurator;

public class UserControlLicenseInfo : UserControl
{
	private IContainer components;

	private Label lblLicenseInfoSummary;

	private Label lblLicenseInfoStatic;

	private Panel panelBlueBorder;

	public UserControlLicenseInfo()
	{
		InitializeComponent();
	}

	private async void UserControlLicenseInfo_Load(object sender, EventArgs e)
	{
		Label label = lblLicenseInfoSummary;
		label.Text = await GetLicenseStatusAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task<string> GetLicenseStatusAsync()
	{
		string text = await LicenseChecker.GetNodeLicenseStatusAsync(Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"));
		return string.IsNullOrEmpty(text) ? Resources.FailoverWizardForm_MaintenanceGetLicenseInfoFail : text;
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
		this.lblLicenseInfoSummary = new System.Windows.Forms.Label();
		this.lblLicenseInfoStatic = new System.Windows.Forms.Label();
		this.panelBlueBorder = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.lblLicenseInfoSummary.AutoSize = true;
		this.lblLicenseInfoSummary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblLicenseInfoSummary.Location = new System.Drawing.Point(11, 29);
		this.lblLicenseInfoSummary.Name = "lblLicenseInfoSummary";
		this.lblLicenseInfoSummary.Size = new System.Drawing.Size(54, 13);
		this.lblLicenseInfoSummary.TabIndex = 107;
		this.lblLicenseInfoSummary.Text = "Loading...";
		this.lblLicenseInfoStatic.AutoSize = true;
		this.lblLicenseInfoStatic.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold);
		this.lblLicenseInfoStatic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblLicenseInfoStatic.Location = new System.Drawing.Point(11, 14);
		this.lblLicenseInfoStatic.Name = "lblLicenseInfoStatic";
		this.lblLicenseInfoStatic.Size = new System.Drawing.Size(113, 13);
		this.lblLicenseInfoStatic.TabIndex = 106;
		this.lblLicenseInfoStatic.Text = "License information:";
		this.panelBlueBorder.BackColor = System.Drawing.Color.FromArgb(0, 152, 223);
		this.panelBlueBorder.Location = new System.Drawing.Point(6, 13);
		this.panelBlueBorder.Name = "panelBlueBorder";
		this.panelBlueBorder.Size = new System.Drawing.Size(2, 32);
		this.panelBlueBorder.TabIndex = 105;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblLicenseInfoSummary);
		base.Controls.Add(this.lblLicenseInfoStatic);
		base.Controls.Add(this.panelBlueBorder);
		base.Name = "UserControlLicenseInfo";
		base.Size = new System.Drawing.Size(600, 61);
		base.Load += new System.EventHandler(UserControlLicenseInfo_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

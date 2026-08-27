using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Installer.UI.Framework;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class WizardPageConfirmBackup : WizardPageBase
{
	private ILogger _logger;

	private IContainer components;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Label labelBackupInstructionsHeader;

	private Label labelBackupInstructions;

	private Label labelBackupMoreInformation;

	private CheckBox checkBoxConfirmBackupCreated;

	public WizardPageConfirmBackup()
	{
		InitializeComponent();
		_logger = LogManager.InnerLog;
	}

	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.ShowNavigationButtons();
		Wizard.NextButton.Show();
		UpdateControlsState();
		LoadResources();
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
	}

	private void LoadResources()
	{
		labelHeadline.Text = Resources.ConfirmBackup_Headline;
		labelBackupInstructions.Text = Resources.ConfirmBackup_Instructions;
		labelBackupMoreInformation.Text = Resources.ConfirmBackup_LabelMoreInformation;
		checkBoxConfirmBackupCreated.Text = Resources.ConfirmBackup_CheckBoxConfirm;
		string applicationNameShort = OemResources.Instance.Manager.ApplicationNameShort;
		labelSubHeading.Text = string.Format(CultureInfo.CurrentCulture, Resources.ConfirmBackup_Description, applicationNameShort);
		labelBackupInstructionsHeader.Text = string.Format(CultureInfo.CurrentCulture, Resources.ConfirmBackup_InstructionsHeader, applicationNameShort);
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
	public override bool OnBeforeActivate()
	{
		try
		{
			return ShouldLoadPage();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, ex.ToString(), Array.Empty<object>());
			throw;
		}
	}

	public override bool OnBeforeContinue()
	{
		return true;
	}

	private bool ShouldLoadPage()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_EXTERNALLY_MANAGED") && InstallerManager.Instance.Parameters["IS_EXTERNALLY_MANAGED"] == "true")
		{
			return false;
		}
		if (!InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") || !(InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true"))
		{
			return false;
		}
		if (!CorporateDatabaseValidationActionPage.RunManagmentServerValidateDatabase(_logger).Status.HasFlag(DatabaseValidationStatus.Upgrade))
		{
			return false;
		}
		return true;
	}

	private void CheckBoxcheckBoxConfirmBackupCreatedChanged(object sender, EventArgs e)
	{
		UpdateControlsState();
	}

	private void UpdateControlsState()
	{
		if (checkBoxConfirmBackupCreated.Checked)
		{
			Wizard.NextButton.Enabled = true;
		}
		else
		{
			Wizard.NextButton.Enabled = false;
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

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "XProtect")]
	private void InitializeComponent()
	{
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.labelBackupInstructionsHeader = new System.Windows.Forms.Label();
		this.labelBackupInstructions = new System.Windows.Forms.Label();
		this.labelBackupMoreInformation = new System.Windows.Forms.Label();
		this.checkBoxConfirmBackupCreated = new System.Windows.Forms.CheckBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelBackupInstructionsHeader);
		base.WizardPage.Controls.Add(this.labelBackupInstructions);
		base.WizardPage.Controls.Add(this.labelBackupMoreInformation);
		base.WizardPage.Controls.Add(this.checkBoxConfirmBackupCreated);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.Text = "Confirm that the system configuration is backed up";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 46);
		this.labelSubHeading.Text = "It is recommended that you back up your system configuration before performing an upgrade. You can create a backup in the Management Client.";
		this.labelBackupInstructionsHeader.Font = new System.Drawing.Font("Arial", 9f);
		this.labelBackupInstructionsHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelBackupInstructionsHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.labelBackupInstructionsHeader.Location = new System.Drawing.Point(47, 150);
		this.labelBackupInstructionsHeader.Name = "labelBackupInstructionsHeader";
		this.labelBackupInstructionsHeader.Size = new System.Drawing.Size(524, 20);
		this.labelBackupInstructionsHeader.Text = "To create a backup in the Management Client:";
		this.labelBackupInstructions.Font = new System.Drawing.Font("Arial", 9f);
		this.labelBackupInstructions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelBackupInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.labelBackupInstructions.Location = new System.Drawing.Point(70, 180);
		this.labelBackupInstructions.Name = "labelBackupInstructions";
		this.labelBackupInstructions.Size = new System.Drawing.Size(501, 100);
		this.labelBackupInstructions.Text = "1. From the menu bar, select File > Backup Configuration.\r\n2. Read the note in the dialog box, and click Backup.\r\n3. Enter a file name for the .cnf file.\r\n4. Enter a folder destination, and click Save.\r\n5. Wait until the backup is finished, and click Close.";
		this.labelBackupMoreInformation.Font = new System.Drawing.Font("Arial", 9f);
		this.labelBackupMoreInformation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelBackupMoreInformation.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.labelBackupMoreInformation.Location = new System.Drawing.Point(47, 300);
		this.labelBackupMoreInformation.Name = "labelBackupMoreInformation";
		this.labelBackupMoreInformation.Size = new System.Drawing.Size(501, 20);
		this.labelBackupMoreInformation.Text = "For more information, please refer to the administrator manual.";
		this.checkBoxConfirmBackupCreated.AutoSize = true;
		this.checkBoxConfirmBackupCreated.Checked = false;
		this.checkBoxConfirmBackupCreated.Font = new System.Drawing.Font("Arial", 9f);
		this.checkBoxConfirmBackupCreated.Location = new System.Drawing.Point(47, 370);
		this.checkBoxConfirmBackupCreated.Name = "checkBoxConfirmBackupCreated";
		this.checkBoxConfirmBackupCreated.Size = new System.Drawing.Size(500, 20);
		this.checkBoxConfirmBackupCreated.TabIndex = 2;
		this.checkBoxConfirmBackupCreated.TabStop = true;
		this.checkBoxConfirmBackupCreated.Text = "I confirm that a backup of the system configuration has been created.";
		this.checkBoxConfirmBackupCreated.UseVisualStyleBackColor = true;
		this.checkBoxConfirmBackupCreated.CheckedChanged += new System.EventHandler(CheckBoxcheckBoxConfirmBackupCreatedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageConfirmBackup";
		this.PageName = "ConfirmBackupPage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

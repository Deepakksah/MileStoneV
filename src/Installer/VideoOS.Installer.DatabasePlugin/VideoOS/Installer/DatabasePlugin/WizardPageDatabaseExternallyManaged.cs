using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.DatabasePlugin;

public class WizardPageDatabaseExternallyManaged : WizardPageBase
{
	private IContainer components;

	private RadioButton radioButtonManagedDatabase;

	private RadioButton radioButtonExternallyManagedDatabase;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Label labelManagedDatabase;

	private Label labelExternallyManagedDatabase;

	public WizardPageDatabaseExternallyManaged()
	{
		InitializeComponent();
		SetControlsState();
		LoadResources();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.ShowNavigationButtons();
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = VideoOS.Installer.UI.Properties.Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
	}

	private void LoadResources()
	{
		labelHeadline.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_Headline;
		labelSubHeading.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_Description;
		radioButtonExternallyManagedDatabase.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_PreCreated;
		radioButtonManagedDatabase.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_XProtect;
		labelExternallyManagedDatabase.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_PreCreated_Description;
		labelManagedDatabase.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExternallyManaged_XProtect_Description;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		bool flag = radioButtonExternallyManagedDatabase.Checked;
		Dictionary<string, string> dictionary = new Dictionary<string, string> { 
		{
			"IS_EXTERNALLY_MANAGED",
			flag ? "true" : "false"
		} };
		LogManager.Write("InstallationMode: " + (InstallerManager.Instance.Parameters.ContainsKey("INSTALLATION_MODE") ? InstallerManager.Instance.Parameters["INSTALLATION_MODE"] : "Mode Unknown"));
		if (InstallerManager.Instance.Parameters.ContainsKey("INSTALLATION_MODE") && InstallerManager.Instance.Parameters["INSTALLATION_MODE"].Equals("NormalMode"))
		{
			dictionary.Add("TRECK_ID", flag ? "StandaloneExternallyManaged" : "StandaloneManaged");
		}
		if (flag)
		{
			dictionary.Add("SQL-KEEP-DATA", "yes");
			dictionary.Add("InstallNewServerChecked", "0");
		}
		return dictionary;
	}

	private void SetControlsState()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_EXTERNALLY_MANAGED"))
		{
			radioButtonExternallyManagedDatabase.Checked = InstallerManager.Instance.Parameters["IS_EXTERNALLY_MANAGED"].Equals("true");
		}
	}

	public override bool OnBeforeContinue()
	{
		GlobalData.SetValue("IS_EXTERNALLY_MANAGED", radioButtonExternallyManagedDatabase.Checked ? "true" : "false", "dbExternallyManagedPage.OnBeforeContinue()");
		return true;
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
		this.radioButtonManagedDatabase = new System.Windows.Forms.RadioButton();
		this.radioButtonExternallyManagedDatabase = new System.Windows.Forms.RadioButton();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.labelManagedDatabase = new System.Windows.Forms.Label();
		this.labelExternallyManagedDatabase = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.radioButtonManagedDatabase);
		base.WizardPage.Controls.Add(this.radioButtonExternallyManagedDatabase);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelExternallyManagedDatabase);
		base.WizardPage.Controls.Add(this.labelManagedDatabase);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.radioButtonManagedDatabase.AutoSize = true;
		this.radioButtonManagedDatabase.Checked = true;
		this.radioButtonManagedDatabase.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonManagedDatabase.Location = new System.Drawing.Point(50, 179);
		this.radioButtonManagedDatabase.Name = "radioButtonManagedDatabase";
		this.radioButtonManagedDatabase.Size = new System.Drawing.Size(329, 18);
		this.radioButtonManagedDatabase.TabIndex = 2;
		this.radioButtonManagedDatabase.TabStop = true;
		this.radioButtonManagedDatabase.Text = "&Let the XProtect installer create or recreate a database";
		this.radioButtonManagedDatabase.UseVisualStyleBackColor = true;
		this.radioButtonExternallyManagedDatabase.AutoSize = true;
		this.radioButtonExternallyManagedDatabase.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonExternallyManagedDatabase.Location = new System.Drawing.Point(50, 274);
		this.radioButtonExternallyManagedDatabase.Name = "radioButtonExternallyManagedDatabase";
		this.radioButtonExternallyManagedDatabase.Size = new System.Drawing.Size(176, 18);
		this.radioButtonExternallyManagedDatabase.TabIndex = 3;
		this.radioButtonExternallyManagedDatabase.TabStop = true;
		this.radioButtonExternallyManagedDatabase.Text = "&Use a pre-created database";
		this.radioButtonExternallyManagedDatabase.UseVisualStyleBackColor = true;
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "&Database setup";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 46);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "&Select the type of database you wish to use";
		this.labelManagedDatabase.Font = new System.Drawing.Font("Arial", 9f);
		this.labelManagedDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelManagedDatabase.Location = new System.Drawing.Point(65, 206);
		this.labelManagedDatabase.Name = "labelManagedDatabase";
		this.labelManagedDatabase.Size = new System.Drawing.Size(504, 42);
		this.labelManagedDatabase.TabIndex = 10;
		this.labelManagedDatabase.Text = "&Use this option to create or upgrade the entire database including the option to install SQL Express.";
		this.labelExternallyManagedDatabase.Font = new System.Drawing.Font("Arial", 9f);
		this.labelExternallyManagedDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelExternallyManagedDatabase.Location = new System.Drawing.Point(65, 302);
		this.labelExternallyManagedDatabase.Name = "labelExternallyManagedDatabase";
		this.labelExternallyManagedDatabase.Size = new System.Drawing.Size(504, 74);
		this.labelExternallyManagedDatabase.TabIndex = 11;
		this.labelExternallyManagedDatabase.Text = "&Use this option when databases are pre-created by someone with administrator rights on the database server. Content is only created or upgraded within existing databases.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageDatabaseExternallyManaged";
		this.PageName = "DatabaseExternallyManagedPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

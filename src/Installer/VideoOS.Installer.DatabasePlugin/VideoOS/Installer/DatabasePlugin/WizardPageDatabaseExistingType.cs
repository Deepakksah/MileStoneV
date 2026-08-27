using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class WizardPageDatabaseExistingType : WizardPageBase
{
	private Wizard _wizard;

	private readonly bool _isInitializing = true;

	private IContainer components;

	private Label labelKeepExistingDB;

	private RadioButton radioButtonUseExistingBD;

	private RadioButton radioButtonCreateNewDB;

	private TextBox textBoxNewDBName;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Label labeNewDBName;

	private Label labelExiatingDBName;

	private RadioButton radioButtonOverwriteExistData;

	private RadioButton radioButtonKeepExistdata;

	private Panel panel1;

	private TextBox textBoxExisnDbName;

	public WizardPageDatabaseExistingType()
	{
		InitializeComponent();
		SetControlsState();
		LoadResources();
		_isInitializing = false;
	}

	private void LoadResources()
	{
		labelHeadline.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_HeadLine;
		labelSubHeading.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_SubLine;
		radioButtonCreateNewDB.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_CreateNewdb;
		labeNewDBName.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_DatabaseName;
		labelExiatingDBName.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_DatabaseName;
		radioButtonUseExistingBD.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_UseExistDb;
		labelKeepExistingDB.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_UseExistOptions;
		radioButtonKeepExistdata.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_ExistDataKeep;
		radioButtonOverwriteExistData.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_ExistDataOverwrite;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void radioButtonUseExistingBD_CheckedChanged(object sender, EventArgs e)
	{
		SetControlsState();
	}

	private void SetControlsState()
	{
		radioButtonKeepExistdata.Enabled = radioButtonUseExistingBD.Checked;
		radioButtonOverwriteExistData.Enabled = radioButtonUseExistingBD.Checked;
		textBoxExisnDbName.Enabled = radioButtonUseExistingBD.Checked;
		labelExiatingDBName.Enabled = radioButtonUseExistingBD.Checked;
		labelKeepExistingDB.Enabled = radioButtonUseExistingBD.Checked;
		labeNewDBName.Enabled = !radioButtonUseExistingBD.Checked;
		textBoxNewDBName.Enabled = !radioButtonUseExistingBD.Checked;
		if (!_isInitializing)
		{
			if ((radioButtonUseExistingBD.Checked && !CommonHelper.IsValidSqlServerPath(textBoxExisnDbName.Text)) || (radioButtonCreateNewDB.Checked && !CommonHelper.IsValidSqlServerPath(textBoxNewDBName.Text)))
			{
				_wizard.NextEnabled = false;
			}
			else
			{
				_wizard.NextEnabled = true;
			}
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		_wizard = Page.Parent as Wizard;
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = VideoOS.Installer.UI.Properties.Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
	}

	private void CheckEnteredText(string text)
	{
		if (!_isInitializing)
		{
			_wizard.NextEnabled = CommonHelper.IsValidSqlServerPath(text);
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		bool flag = InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") && InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
		if (flag)
		{
			string serverName = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"]).ServerName;
			string databaseName = (radioButtonUseExistingBD.Checked ? textBoxExisnDbName.Text : textBoxNewDBName.Text);
			string logServerDatabaseName = "SurveillanceLogServerV2";
			foreach (var item in TemporaryGenerateConnectionStringsByConvention(serverName, databaseName, logServerDatabaseName))
			{
				dictionary.Add(item.Key, item.ConnectionString);
			}
		}
		else if (!flag && InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			string serverName2 = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_LOG"]).ServerName;
			string databaseName2 = (radioButtonUseExistingBD.Checked ? textBoxExisnDbName.Text : textBoxNewDBName.Text);
			string connectionString = new DatabaseConnectionInfo(serverName2, databaseName2, AuthenticationType.IntegratedSecurityTrusted).ConnectionString;
			dictionary.Add("SQL_CONNECTION_STRING_LOG", connectionString);
		}
		if (radioButtonUseExistingBD.Checked)
		{
			dictionary.Add("SQL-CREATE-DATABASE", "no");
			dictionary.Add("SQL-KEEP-DATA", radioButtonKeepExistdata.Checked ? "yes" : "no");
		}
		else
		{
			dictionary.Add("SQL-CREATE-DATABASE", "yes");
		}
		return dictionary;
	}

	private void textBoxNewDBName_TextChanged(object sender, EventArgs e)
	{
		CheckEnteredText(radioButtonCreateNewDB.Checked ? textBoxNewDBName.Text : textBoxExisnDbName.Text);
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.ContainsKey("SQL_CONNECTION_STRING_MS") || parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			bool flag = InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") && InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
			string text = string.Empty;
			if (flag)
			{
				text = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"]).DatabaseName;
			}
			else if (!flag && InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
			{
				text = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_LOG"]).DatabaseName;
			}
			textBoxExisnDbName.Text = text;
			textBoxNewDBName.Text = text;
			radioButtonUseExistingBD.Checked = (parameters.ContainsKey("InstallDatabaseExists") && parameters["InstallDatabaseExists"] == bool.TrueString) || (parameters.ContainsKey("IS_UPGRADE") && parameters["IS_UPGRADE"] == "true");
			radioButtonCreateNewDB.Checked = !radioButtonUseExistingBD.Checked;
		}
	}

	public override bool OnBeforeContinue()
	{
		if (radioButtonUseExistingBD.Checked)
		{
			GlobalData.SetValue("SQL-KEEP-DATA", radioButtonKeepExistdata.Checked ? "yes" : "no", "dbExist. OnBeforeContinue()");
		}
		else
		{
			GlobalData.SetValue("SQL-KEEP-DATA", "no", "dbExist. OnBeforeContinue()");
		}
		GlobalData.SetValue("SQL-CREATE-DATABASE", radioButtonCreateNewDB.Checked ? "yes" : "no", "dbExist. OnBeforeContinue()");
		bool flag = InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") && InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
		if (flag)
		{
			string serverName = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"]).ServerName;
			string databaseName = (radioButtonUseExistingBD.Checked ? textBoxExisnDbName.Text : textBoxNewDBName.Text);
			string logServerDatabaseName = "SurveillanceLogServerV2";
			foreach (var item in TemporaryGenerateConnectionStringsByConvention(serverName, databaseName, logServerDatabaseName))
			{
				GlobalData.SetValue(item.Key, item.ConnectionString, "dbExist. OnBeforeContinue()");
			}
		}
		else if (!flag && InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			string serverName2 = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_LOG"]).ServerName;
			string databaseName2 = (radioButtonUseExistingBD.Checked ? textBoxExisnDbName.Text : textBoxNewDBName.Text);
			string connectionString = new DatabaseConnectionInfo(serverName2, databaseName2, AuthenticationType.IntegratedSecurityTrusted).ConnectionString;
			GlobalData.SetValue("SQL_CONNECTION_STRING_LOG", connectionString, "dbExist. OnBeforeContinue()");
		}
		bool result = true;
		if (radioButtonUseExistingBD.Checked && radioButtonOverwriteExistData.Checked)
		{
			using (new ShowWaitCursor())
			{
				if (MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.DatabasePlugin.Properties.Resources.Database_ConfirmMessageOverwriteCaption, VideoOS.Installer.DatabasePlugin.Properties.Resources.Database_ConfirmMessageOverwriteText, InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					result = false;
				}
			}
		}
		GlobalData.SetValue("SQL_CONFIRM_OVERWRITE", result.ToString(), "dbExist. OnBeforeContinue()");
		return result;
	}

	private static IList<(string Key, string ConnectionString)> TemporaryGenerateConnectionStringsByConvention(string dataSource, string databaseName, string logServerDatabaseName)
	{
		return new(string, string)[5]
		{
			("SQL_CONNECTION_STRING_MS", new DatabaseConnectionInfo(dataSource, databaseName, AuthenticationType.IntegratedSecurityTrusted).ConnectionString),
			("SQL_CONNECTION_STRING_IDP", new DatabaseConnectionInfo(dataSource, databaseName + "_IDP", AuthenticationType.IntegratedSecurityTrusted).ConnectionString),
			("SQL_CONNECTION_STRING_IM", new DatabaseConnectionInfo(dataSource, databaseName + "_IM", AuthenticationType.IntegratedSecurityTrusted).ConnectionString),
			("SQL_CONNECTION_STRING_ES", new DatabaseConnectionInfo(dataSource, databaseName, AuthenticationType.IntegratedSecurityTrusted).ConnectionString),
			("SQL_CONNECTION_STRING_LOG", new DatabaseConnectionInfo(dataSource, logServerDatabaseName, AuthenticationType.IntegratedSecurityTrusted).ConnectionString)
		};
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
		this.labelKeepExistingDB = new System.Windows.Forms.Label();
		this.radioButtonUseExistingBD = new System.Windows.Forms.RadioButton();
		this.radioButtonCreateNewDB = new System.Windows.Forms.RadioButton();
		this.textBoxNewDBName = new System.Windows.Forms.TextBox();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.labeNewDBName = new System.Windows.Forms.Label();
		this.labelExiatingDBName = new System.Windows.Forms.Label();
		this.radioButtonKeepExistdata = new System.Windows.Forms.RadioButton();
		this.radioButtonOverwriteExistData = new System.Windows.Forms.RadioButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.textBoxExisnDbName = new System.Windows.Forms.TextBox();
		base.WizardPage.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelExiatingDBName);
		base.WizardPage.Controls.Add(this.labeNewDBName);
		base.WizardPage.Controls.Add(this.radioButtonUseExistingBD);
		base.WizardPage.Controls.Add(this.radioButtonCreateNewDB);
		base.WizardPage.Controls.Add(this.textBoxNewDBName);
		base.WizardPage.Controls.Add(this.textBoxExisnDbName);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.panel1);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelKeepExistingDB.AutoSize = true;
		this.labelKeepExistingDB.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelKeepExistingDB.Location = new System.Drawing.Point(13, 15);
		this.labelKeepExistingDB.Name = "labelKeepExistingDB";
		this.labelKeepExistingDB.Size = new System.Drawing.Size(316, 14);
		this.labelKeepExistingDB.TabIndex = 8;
		this.labelKeepExistingDB.Text = "Do you want to keep the existing data in the database?";
		this.radioButtonUseExistingBD.AutoSize = true;
		this.radioButtonUseExistingBD.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonUseExistingBD.Location = new System.Drawing.Point(50, 240);
		this.radioButtonUseExistingBD.Name = "radioButtonUseExistingBD";
		this.radioButtonUseExistingBD.Size = new System.Drawing.Size(152, 18);
		this.radioButtonUseExistingBD.TabIndex = 5;
		this.radioButtonUseExistingBD.Text = "&Using existing database";
		this.radioButtonUseExistingBD.UseVisualStyleBackColor = true;
		this.radioButtonUseExistingBD.CheckedChanged += new System.EventHandler(radioButtonUseExistingBD_CheckedChanged);
		this.radioButtonCreateNewDB.AutoSize = true;
		this.radioButtonCreateNewDB.Checked = true;
		this.radioButtonCreateNewDB.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonCreateNewDB.Location = new System.Drawing.Point(50, 150);
		this.radioButtonCreateNewDB.Name = "radioButtonCreateNewDB";
		this.radioButtonCreateNewDB.Size = new System.Drawing.Size(142, 18);
		this.radioButtonCreateNewDB.TabIndex = 2;
		this.radioButtonCreateNewDB.TabStop = true;
		this.radioButtonCreateNewDB.Text = "&Create new database";
		this.radioButtonCreateNewDB.UseVisualStyleBackColor = true;
		this.textBoxNewDBName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxNewDBName.Location = new System.Drawing.Point(289, 174);
		this.textBoxNewDBName.Name = "textBoxNewDBName";
		this.textBoxNewDBName.Size = new System.Drawing.Size(262, 22);
		this.textBoxNewDBName.TabIndex = 4;
		this.textBoxNewDBName.Text = "Surveillance";
		this.textBoxNewDBName.TextChanged += new System.EventHandler(textBoxNewDBName_TextChanged);
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "Configuration Database Setup";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(536, 46);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "Please specify whether you want to create a new database or use an existing one. In the last case, select whether to keep or overwrite existing data.  ";
		this.labeNewDBName.AutoSize = true;
		this.labeNewDBName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labeNewDBName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labeNewDBName.Location = new System.Drawing.Point(66, 178);
		this.labeNewDBName.Name = "labeNewDBName";
		this.labeNewDBName.Size = new System.Drawing.Size(95, 14);
		this.labeNewDBName.TabIndex = 3;
		this.labeNewDBName.Text = "&Database name:";
		this.labelExiatingDBName.AutoSize = true;
		this.labelExiatingDBName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelExiatingDBName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelExiatingDBName.Location = new System.Drawing.Point(66, 268);
		this.labelExiatingDBName.Name = "labelExiatingDBName";
		this.labelExiatingDBName.Size = new System.Drawing.Size(95, 14);
		this.labelExiatingDBName.TabIndex = 6;
		this.labelExiatingDBName.Text = "D&atabase name:";
		this.radioButtonKeepExistdata.AutoSize = true;
		this.radioButtonKeepExistdata.Checked = true;
		this.radioButtonKeepExistdata.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonKeepExistdata.Location = new System.Drawing.Point(16, 35);
		this.radioButtonKeepExistdata.Name = "radioButtonKeepExistdata";
		this.radioButtonKeepExistdata.Size = new System.Drawing.Size(126, 18);
		this.radioButtonKeepExistdata.TabIndex = 9;
		this.radioButtonKeepExistdata.TabStop = true;
		this.radioButtonKeepExistdata.Text = "&Keep existing data";
		this.radioButtonKeepExistdata.UseVisualStyleBackColor = true;
		this.radioButtonOverwriteExistData.AutoSize = true;
		this.radioButtonOverwriteExistData.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonOverwriteExistData.Location = new System.Drawing.Point(16, 59);
		this.radioButtonOverwriteExistData.Name = "radioButtonOverwriteExistData";
		this.radioButtonOverwriteExistData.Size = new System.Drawing.Size(152, 18);
		this.radioButtonOverwriteExistData.TabIndex = 10;
		this.radioButtonOverwriteExistData.Text = "&Overwrite existing data";
		this.radioButtonOverwriteExistData.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.radioButtonOverwriteExistData);
		this.panel1.Controls.Add(this.radioButtonKeepExistdata);
		this.panel1.Controls.Add(this.labelKeepExistingDB);
		this.panel1.Location = new System.Drawing.Point(50, 309);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(533, 98);
		this.panel1.TabIndex = 11;
		this.textBoxExisnDbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxExisnDbName.Location = new System.Drawing.Point(289, 264);
		this.textBoxExisnDbName.Name = "textBoxExisnDbName";
		this.textBoxExisnDbName.Size = new System.Drawing.Size(262, 22);
		this.textBoxExisnDbName.TabIndex = 5;
		this.textBoxExisnDbName.Text = "Surveillance";
		this.textBoxExisnDbName.TextChanged += new System.EventHandler(textBoxNewDBName_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageDatabaseExistingType";
		this.PageName = "DatabaseExistingType";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}

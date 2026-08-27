using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class WizardPageDatabaseAdvancedSetupStandalone : WizardPageBase
{
	private bool _isValid;

	private bool _autoTryConnectInProgress;

	private IContainer components;

	private Label labelHeadline;

	private Label labelSubHeading;

	private TextBox textBoxServer;

	private TextBox textBoxDatabaseName;

	private Label labelConnectionString;

	private Label labelAuthenticationType;

	private Label labelServer;

	private Label labelDatabaseName;

	private Button tryConnectButton;

	private UserControlProgressSpinner tryConnectSpinner;

	private Label labelConnectionStatus;

	private ComboBox comboBoxAuthenticationType;

	public WizardPageDatabaseAdvancedSetupStandalone()
	{
		InitializeComponent();
		tryConnectButton.Click += delegate
		{
			_autoTryConnectInProgress = false;
			TryConnectButton_Click();
		};
		textBoxServer.TextChanged += delegate
		{
			OnConnectionStringChange();
		};
		textBoxDatabaseName.TextChanged += delegate
		{
			OnConnectionStringChange();
		};
		comboBoxAuthenticationType.SelectedIndexChanged += delegate
		{
			OnConnectionStringChange();
		};
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		try
		{
			Wizard.ShowNavigationButtons();
			Wizard.NextButton.Show();
			Wizard.CancelButton.Text = VideoOS.Installer.UI.Properties.Resources.btn_Cancel;
			LoadResources();
			SetControlsState();
			Wizard.NextButton.Enabled = true;
			if (Wizard.BackButton.Location == Wizard.NextButton.Location)
			{
				Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	private void LoadResources()
	{
		labelHeadline.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_Headline;
		labelSubHeading.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_Description;
		labelServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_ServerHeader;
		labelDatabaseName.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_DatabaseHeader;
		labelConnectionString.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetupStandalone_ConnectionStringLabel;
		labelAuthenticationType.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_AuthenticationTypeHeader;
		comboBoxAuthenticationType.Items.Clear();
		foreach (AuthenticationType value in Enum.GetValues(typeof(AuthenticationType)))
		{
			string authenticationTypeResource = AuthenticationTypeHelper.GetAuthenticationTypeResource(value);
			if (authenticationTypeResource != null)
			{
				comboBoxAuthenticationType.Items.Add(authenticationTypeResource);
			}
		}
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		List<(string, string)> list = new List<(string, string)>();
		if (InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			list.Add(("SQL_CONNECTION_STRING_LOG", GenerateConnectionString()));
		}
		if (InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_ES"))
		{
			list.Add(("SQL_CONNECTION_STRING_ES", GenerateConnectionString()));
		}
		return list.ToDictionary<(string, string), string, string>(((string Name, string ConnectionString) x) => x.Name, ((string Name, string ConnectionString) y) => y.ConnectionString);
	}

	private void SetControlsState()
	{
		labelConnectionStatus.Visible = false;
		_isValid = false;
		_autoTryConnectInProgress = false;
		comboBoxAuthenticationType.Enabled = true;
		comboBoxAuthenticationType.DropDownStyle = ComboBoxStyle.DropDownList;
		if (InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			TryPopulateConnectionString("SQL_CONNECTION_STRING_LOG");
		}
		else if (InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_ES"))
		{
			TryPopulateConnectionString("SQL_CONNECTION_STRING_ES");
		}
	}

	public override bool OnBeforeContinue()
	{
		if (!_isValid)
		{
			AutoTryConnect();
			return false;
		}
		return true;
	}

	private void OnConnectionStringChange()
	{
		_isValid = false;
		HideValidationResult();
	}

	private void AutoTryConnect()
	{
		_autoTryConnectInProgress = true;
		TryConnectButton_Click();
	}

	private string TryGenerateConnectionString()
	{
		try
		{
			HideValidationResult();
			return GenerateConnectionString();
		}
		catch (Exception ex)
		{
			LogManager.Write(ex);
			ShowValidationResult(ex.Message, isSuccess: false);
			return null;
		}
	}

	public async void TryConnectButton_Click()
	{
		try
		{
			string text = TryGenerateConnectionString();
			if (text == null)
			{
				_autoTryConnectInProgress = false;
				return;
			}
			SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(new DatabaseConnectionInfo(text), null);
			LogManager.Write("Try connect to Data Server:" + sqlConnectionManager.ServerName + " Database Name:" + sqlConnectionManager.DatabaseName);
			HandleTryConnectSpinner(isConnecting: true);
			bool flag = await TryConnectAsync(sqlConnectionManager);
			LogManager.Write($"Try connect result: {flag}");
			HandleTryConnectSpinner(isConnecting: false);
			ShowValidationResult(flag ? VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Success : VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Fail, flag);
			if (_autoTryConnectInProgress)
			{
				if (flag)
				{
					Wizard.MoveToNextPage();
				}
				else
				{
					_autoTryConnectInProgress = false;
				}
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			_autoTryConnectInProgress = false;
			HandleTryConnectSpinner(isConnecting: false);
			ShowValidationResult(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Unexpected, isSuccess: false);
		}
	}

	private void HandleTryConnectSpinner(bool isConnecting)
	{
		tryConnectSpinner.Visible = isConnecting;
		tryConnectButton.Visible = !isConnecting;
		textBoxServer.Enabled = !isConnecting;
		textBoxDatabaseName.Enabled = !isConnecting;
		comboBoxAuthenticationType.Enabled = !isConnecting;
		Wizard.NextButton.Enabled = !isConnecting;
		if (isConnecting)
		{
			tryConnectSpinner.Start();
		}
		else
		{
			tryConnectSpinner.Stop();
		}
	}

	private Task<bool> TryConnectAsync(SqlConnectionManager sqlConnectionManager)
	{
		return Task.Run(delegate
		{
			try
			{
				return DatabaseErrorCheckHelper.CheckConnectionToExistingDatabase(sqlConnectionManager);
			}
			catch
			{
				return false;
			}
		});
	}

	private void HideValidationResult()
	{
		labelConnectionStatus.Visible = false;
	}

	private void ShowValidationResult(string message, bool isSuccess)
	{
		labelConnectionStatus.Text = message;
		labelConnectionStatus.ForeColor = (isSuccess ? Color.Green : Color.Red);
		labelConnectionStatus.Visible = true;
		_isValid = isSuccess;
	}

	private string GenerateConnectionString()
	{
		AuthenticationType selectedIndex = (AuthenticationType)comboBoxAuthenticationType.SelectedIndex;
		if (string.IsNullOrEmpty(textBoxServer.Text))
		{
			throw new Exception(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_MissingServerName);
		}
		if (string.IsNullOrEmpty(textBoxDatabaseName.Text))
		{
			throw new Exception(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_MissingDatabaseName);
		}
		return new DatabaseConnectionInfo(textBoxServer.Text, textBoxDatabaseName.Text, selectedIndex).ConnectionString;
	}

	private void TryPopulateConnectionString(string connectionStringName)
	{
		try
		{
			DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(GetConnectionString(connectionStringName).ConnectionString);
			textBoxServer.Text = databaseConnectionInfo.ServerName;
			textBoxDatabaseName.Text = databaseConnectionInfo.DatabaseName;
			comboBoxAuthenticationType.SelectedIndex = (int)databaseConnectionInfo.AuthenticationType;
		}
		catch
		{
			LogManager.Write("Can't read and populate connection string for " + connectionStringName);
		}
	}

	private (string Name, string ConnectionString) GetConnectionString(string connectionStringName)
	{
		string text = string.Empty;
		if (!(connectionStringName == "SQL_CONNECTION_STRING_ES"))
		{
			if (connectionStringName == "SQL_CONNECTION_STRING_LOG")
			{
				text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.LogServer, null);
			}
		}
		else
		{
			text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.EventServer, new EventServerSqlConnectionStorage());
		}
		return (Name: connectionStringName, ConnectionString: string.IsNullOrEmpty(text) ? InstallerManager.Instance.Parameters[connectionStringName] : text);
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
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.textBoxServer = new System.Windows.Forms.TextBox();
		this.textBoxDatabaseName = new System.Windows.Forms.TextBox();
		this.labelConnectionString = new System.Windows.Forms.Label();
		this.labelAuthenticationType = new System.Windows.Forms.Label();
		this.labelServer = new System.Windows.Forms.Label();
		this.labelDatabaseName = new System.Windows.Forms.Label();
		this.tryConnectButton = new System.Windows.Forms.Button();
		this.tryConnectSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		this.labelConnectionStatus = new System.Windows.Forms.Label();
		this.comboBoxAuthenticationType = new System.Windows.Forms.ComboBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelAuthenticationType);
		base.WizardPage.Controls.Add(this.textBoxDatabaseName);
		base.WizardPage.Controls.Add(this.labelDatabaseName);
		base.WizardPage.Controls.Add(this.labelServer);
		base.WizardPage.Controls.Add(this.textBoxServer);
		base.WizardPage.Controls.Add(this.labelConnectionString);
		base.WizardPage.Controls.Add(this.tryConnectButton);
		base.WizardPage.Controls.Add(this.tryConnectSpinner);
		base.WizardPage.Controls.Add(this.labelConnectionStatus);
		base.WizardPage.Controls.Add(this.comboBoxAuthenticationType);
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
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "&Advanced database setup";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 46);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "&Provide server and database names for selected XProtect components and select an authentication type.";
		this.textBoxServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxServer.Location = new System.Drawing.Point(191, 307);
		this.textBoxServer.Name = "textBoxServer";
		this.textBoxServer.Size = new System.Drawing.Size(175, 22);
		this.textBoxServer.TabIndex = 4;
		this.textBoxDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxDatabaseName.Location = new System.Drawing.Point(383, 307);
		this.textBoxDatabaseName.Name = "textBoxDatabaseName";
		this.textBoxDatabaseName.Size = new System.Drawing.Size(175, 22);
		this.textBoxDatabaseName.TabIndex = 6;
		this.labelConnectionString.AutoSize = true;
		this.labelConnectionString.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelConnectionString.Location = new System.Drawing.Point(47, 310);
		this.labelConnectionString.Name = "labelConnectionString";
		this.labelConnectionString.Size = new System.Drawing.Size(72, 14);
		this.labelConnectionString.TabIndex = 2;
		this.labelConnectionString.Text = "&Parameters:";
		this.labelAuthenticationType.AutoSize = true;
		this.labelAuthenticationType.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelAuthenticationType.Location = new System.Drawing.Point(47, 165);
		this.labelAuthenticationType.Name = "labelAuthenticationType";
		this.labelAuthenticationType.Size = new System.Drawing.Size(121, 14);
		this.labelAuthenticationType.TabIndex = 2;
		this.labelAuthenticationType.Text = "&Authentication type:";
		this.labelServer.AutoSize = true;
		this.labelServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelServer.Location = new System.Drawing.Point(188, 283);
		this.labelServer.Name = "labelServer";
		this.labelServer.Size = new System.Drawing.Size(46, 14);
		this.labelServer.TabIndex = 3;
		this.labelServer.Text = "&Server:";
		this.labelDatabaseName.AutoSize = true;
		this.labelDatabaseName.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelDatabaseName.Location = new System.Drawing.Point(380, 283);
		this.labelDatabaseName.Name = "labelDatabaseName";
		this.labelDatabaseName.Size = new System.Drawing.Size(95, 14);
		this.labelDatabaseName.TabIndex = 5;
		this.labelDatabaseName.Text = "&Database name:";
		this.tryConnectButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.tryConnectButton.Location = new System.Drawing.Point(567, 305);
		this.tryConnectButton.Name = "tryConnectButton";
		this.tryConnectButton.Size = new System.Drawing.Size(26, 26);
		this.tryConnectButton.TabIndex = 7;
		this.tryConnectButton.UseVisualStyleBackColor = true;
		this.tryConnectSpinner.BackColor = System.Drawing.Color.Transparent;
		this.tryConnectSpinner.Location = new System.Drawing.Point(563, 301);
		this.tryConnectSpinner.Name = "tryConnectSpinner";
		this.tryConnectSpinner.Size = new System.Drawing.Size(34, 34);
		this.tryConnectSpinner.TabIndex = 10;
		this.tryConnectSpinner.TabStop = false;
		this.labelConnectionStatus.AutoSize = true;
		this.labelConnectionStatus.BackColor = System.Drawing.Color.Transparent;
		this.labelConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelConnectionStatus.ForeColor = System.Drawing.Color.Red;
		this.labelConnectionStatus.Location = new System.Drawing.Point(186, 332);
		this.labelConnectionStatus.Name = "labelConnectionStatus";
		this.labelConnectionStatus.Size = new System.Drawing.Size(263, 15);
		this.labelConnectionStatus.TabIndex = 2;
		this.labelConnectionStatus.Text = "Can't connect to Management Server database";
		this.labelConnectionStatus.Visible = false;
		this.comboBoxAuthenticationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBoxAuthenticationType.FormattingEnabled = true;
		this.comboBoxAuthenticationType.Location = new System.Drawing.Point(50, 189);
		this.comboBoxAuthenticationType.Name = "comboBoxAuthenticationType";
		this.comboBoxAuthenticationType.Size = new System.Drawing.Size(508, 24);
		this.comboBoxAuthenticationType.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageDatabaseAdvancedSetupStandalone";
		this.PageName = "DatabaseAdvancedSetupPageStandalone";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class WizardPageDatabaseAdvancedSetup : WizardPageBase
{
	private bool _isValidMs;

	private bool _isValidIdp;

	private bool _isValidIm;

	private bool _isValidLogServer;

	private bool _autoTryConnectMsInProgress;

	private bool _autoTryConnectIdpInProgress;

	private bool _autoTryConnectImInProgress;

	private bool _autoTryConnectLogServerInProgress;

	private bool _shouldSkipLogServerInstallation;

	private IContainer components;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Label labelMs;

	private TextBox textBoxMsServer;

	private TextBox textBoxMsDatabaseName;

	private TextBox textBoxIdpServer;

	private TextBox textBoxIdpDatabaseName;

	private TextBox textBoxImServer;

	private TextBox textBoxImDatabaseName;

	private TextBox textBoxLogServer;

	private TextBox textBoxLogServerDatabaseName;

	private Label labelIdp;

	private Label labelIm;

	private Label labelLogServer;

	private Label labelAuthenticationType;

	private Label labelServer;

	private Label labelDatabaseName;

	private Button tryConnectManagementServerButton;

	private Button tryConnectIdpButton;

	private Button tryConnectIncidentManagerButton;

	private Button tryConnectLogServerButton;

	private Label labelManagementServerConnectionStatus;

	private Label labelIdpConnectionStatus;

	private Label labelImConnectionStatus;

	private Label labelLogServerConnectionStatus;

	private UserControlProgressSpinner tryConnectManagementServerSpinner;

	private UserControlProgressSpinner tryConnectIdpSpinner;

	private UserControlProgressSpinner tryConnectIncidentManagerSpinner;

	private UserControlProgressSpinner tryConnectLogServerSpinner;

	private ComboBox comboBoxAuthenticationType;

	public WizardPageDatabaseAdvancedSetup()
	{
		InitializeComponent();
		tryConnectManagementServerButton.Click += delegate
		{
			_autoTryConnectMsInProgress = false;
			TryConnectButton_Click("SQL_CONNECTION_STRING_MS");
		};
		tryConnectIdpButton.Click += delegate
		{
			_autoTryConnectIdpInProgress = false;
			TryConnectButton_Click("SQL_CONNECTION_STRING_IDP");
		};
		tryConnectIncidentManagerButton.Click += delegate
		{
			_autoTryConnectImInProgress = false;
			TryConnectButton_Click("SQL_CONNECTION_STRING_IM");
		};
		tryConnectLogServerButton.Click += delegate
		{
			_autoTryConnectLogServerInProgress = false;
			TryConnectButton_Click("SQL_CONNECTION_STRING_LOG");
		};
		textBoxMsServer.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_MS");
		};
		textBoxMsDatabaseName.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_MS");
		};
		textBoxIdpServer.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_IDP");
		};
		textBoxIdpDatabaseName.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_IDP");
		};
		textBoxImServer.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_IM");
		};
		textBoxImDatabaseName.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_IM");
		};
		textBoxLogServer.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_LOG");
		};
		textBoxLogServerDatabaseName.TextChanged += delegate
		{
			OnTextChange("SQL_CONNECTION_STRING_LOG");
		};
		comboBoxAuthenticationType.SelectedIndexChanged += delegate
		{
			OnAuthenticationTypeChange();
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
		labelMs.Text = "Management Server:";
		labelIdp.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_IdentityServer;
		labelIm.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_IncidentManager;
		labelLogServer.Text = "Log Server:";
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
		return TryGenerateConnectionStrings().ToDictionary(((string Name, string ConnectionString) x) => x.Name, ((string Name, string ConnectionString) y) => y.ConnectionString);
	}

	private void SetControlsState()
	{
		_isValidMs = false;
		_isValidIdp = false;
		_isValidIm = false;
		_isValidLogServer = false;
		_autoTryConnectMsInProgress = false;
		_autoTryConnectIdpInProgress = false;
		_autoTryConnectImInProgress = false;
		_autoTryConnectLogServerInProgress = false;
		InstallerManager.Instance.InstallerConfig.InstallExecuteSequence.ToList().ForEach(delegate(PackageRefType p)
		{
			if (p.ID.Equals("L8451C4D-72B8-4D83-885D-A74B82F6C997", StringComparison.OrdinalIgnoreCase))
			{
				_shouldSkipLogServerInstallation = p.Condition == ConditionType.Skip;
			}
		});
		labelManagementServerConnectionStatus.Visible = false;
		labelIdpConnectionStatus.Visible = false;
		labelImConnectionStatus.Visible = false;
		labelLogServerConnectionStatus.Visible = false;
		TryPopulateConnectionString("SQL_CONNECTION_STRING_MS");
		TryPopulateConnectionString("SQL_CONNECTION_STRING_IDP");
		TryPopulateConnectionString("SQL_CONNECTION_STRING_IM");
		textBoxLogServer.Enabled = !_shouldSkipLogServerInstallation;
		textBoxLogServerDatabaseName.Enabled = !_shouldSkipLogServerInstallation;
		tryConnectLogServerButton.Enabled = !_shouldSkipLogServerInstallation;
		if (!_shouldSkipLogServerInstallation)
		{
			TryPopulateConnectionString("SQL_CONNECTION_STRING_LOG");
		}
		comboBoxAuthenticationType.Enabled = true;
		comboBoxAuthenticationType.DropDownStyle = ComboBoxStyle.DropDownList;
		Wizard.NextButton.Enabled = true;
	}

	public override bool OnBeforeContinue()
	{
		if (!IsValid())
		{
			AutoTryConnectAll();
			return false;
		}
		IEnumerable<(string, string)> enumerable = TryGenerateConnectionStrings();
		if (enumerable.Any<(string, string)>(((string Name, string ConnectionString) x) => string.IsNullOrEmpty(x.ConnectionString)))
		{
			return false;
		}
		foreach (var item in enumerable)
		{
			GlobalData.SetValue(item.Item1, item.Item2, "dbAdvancedSetup.OnBeforeContinue()");
		}
		return true;
	}

	private void OnTextChange(string connectionStringName)
	{
		SaveTryConnectResult(connectionStringName, isValid: false);
		HideValidationResult(connectionStringName);
	}

	private void OnAuthenticationTypeChange()
	{
		SaveTryConnectResult("SQL_CONNECTION_STRING_MS", isValid: false);
		SaveTryConnectResult("SQL_CONNECTION_STRING_IDP", isValid: false);
		SaveTryConnectResult("SQL_CONNECTION_STRING_IM", isValid: false);
		if (!_shouldSkipLogServerInstallation)
		{
			SaveTryConnectResult("SQL_CONNECTION_STRING_LOG", isValid: false);
		}
		HideValidationResult("SQL_CONNECTION_STRING_MS");
		HideValidationResult("SQL_CONNECTION_STRING_IDP");
		HideValidationResult("SQL_CONNECTION_STRING_IM");
		HideValidationResult("SQL_CONNECTION_STRING_LOG");
	}

	private bool IsValid()
	{
		if (_isValidMs && _isValidIdp && _isValidIm)
		{
			if (!_shouldSkipLogServerInstallation)
			{
				return _isValidLogServer;
			}
			return true;
		}
		return false;
	}

	private bool IsAnyTryConnectionInProgress()
	{
		if (!_autoTryConnectMsInProgress && !_autoTryConnectIdpInProgress && !_autoTryConnectImInProgress)
		{
			if (!_shouldSkipLogServerInstallation)
			{
				return _autoTryConnectLogServerInProgress;
			}
			return false;
		}
		return true;
	}

	private bool IsTryConnectInProgress(string connectionStringName)
	{
		return connectionStringName switch
		{
			"SQL_CONNECTION_STRING_MS" => _autoTryConnectMsInProgress, 
			"SQL_CONNECTION_STRING_IDP" => _autoTryConnectIdpInProgress, 
			"SQL_CONNECTION_STRING_IM" => _autoTryConnectImInProgress, 
			"SQL_CONNECTION_STRING_LOG" => _autoTryConnectLogServerInProgress, 
			_ => false, 
		};
	}

	private void SaveTryConnectInProgress(string connectionStringName, bool inProgress)
	{
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			_autoTryConnectMsInProgress = inProgress;
			break;
		case "SQL_CONNECTION_STRING_IDP":
			_autoTryConnectIdpInProgress = inProgress;
			break;
		case "SQL_CONNECTION_STRING_IM":
			_autoTryConnectImInProgress = inProgress;
			break;
		case "SQL_CONNECTION_STRING_LOG":
			_autoTryConnectLogServerInProgress = inProgress;
			break;
		}
	}

	private void SaveTryConnectResult(string connectionStringName, bool isValid)
	{
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			_isValidMs = isValid;
			break;
		case "SQL_CONNECTION_STRING_IDP":
			_isValidIdp = isValid;
			break;
		case "SQL_CONNECTION_STRING_IM":
			_isValidIm = isValid;
			break;
		case "SQL_CONNECTION_STRING_LOG":
			_isValidLogServer = isValid;
			break;
		}
	}

	private void AutoTryConnectAll()
	{
		if (!_isValidMs)
		{
			_autoTryConnectMsInProgress = true;
			TryConnectButton_Click("SQL_CONNECTION_STRING_MS");
		}
		if (!_isValidIdp)
		{
			_autoTryConnectIdpInProgress = true;
			TryConnectButton_Click("SQL_CONNECTION_STRING_IDP");
		}
		if (!_isValidIm)
		{
			_autoTryConnectImInProgress = true;
			TryConnectButton_Click("SQL_CONNECTION_STRING_IM");
		}
		if (!_shouldSkipLogServerInstallation && !_isValidLogServer)
		{
			_autoTryConnectLogServerInProgress = true;
			TryConnectButton_Click("SQL_CONNECTION_STRING_LOG");
		}
	}

	private IEnumerable<(string Name, string ConnectionString)> TryGenerateConnectionStrings()
	{
		List<(string, string)> list = new List<(string, string)>
		{
			("SQL_CONNECTION_STRING_MS", TryGenerateConnectionString("SQL_CONNECTION_STRING_MS")),
			("SQL_CONNECTION_STRING_ES", TryGenerateConnectionString("SQL_CONNECTION_STRING_ES")),
			("SQL_CONNECTION_STRING_IDP", TryGenerateConnectionString("SQL_CONNECTION_STRING_IDP")),
			("SQL_CONNECTION_STRING_IM", TryGenerateConnectionString("SQL_CONNECTION_STRING_IM"))
		};
		if (!_shouldSkipLogServerInstallation)
		{
			list.Add(("SQL_CONNECTION_STRING_LOG", TryGenerateConnectionString("SQL_CONNECTION_STRING_LOG")));
		}
		return list;
	}

	private string TryGenerateConnectionString(string connectionStringName)
	{
		try
		{
			HideValidationResult(connectionStringName);
			return GenerateConnectionString(connectionStringName);
		}
		catch (Exception ex)
		{
			LogManager.Write(ex);
			SaveTryConnectResult(connectionStringName, isValid: false);
			ShowValidationResult(connectionStringName, ex.Message, isSuccess: false);
			return null;
		}
	}

	public async void TryConnectButton_Click(string connectionStringName)
	{
		try
		{
			string text = TryGenerateConnectionString(connectionStringName);
			if (text == null)
			{
				SaveTryConnectResult(connectionStringName, isValid: false);
				SaveTryConnectInProgress(connectionStringName, inProgress: false);
				return;
			}
			SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(new DatabaseConnectionInfo(text), null);
			LogManager.Write("Try connect to Data Server:" + sqlConnectionManager.ServerName + " Database Name:" + sqlConnectionManager.DatabaseName);
			HandleTryConnectSpinner(connectionStringName, isConnecting: true);
			bool flag = await TryConnectAsync(sqlConnectionManager);
			LogManager.Write($"Try connect result: {flag} for {connectionStringName}");
			SaveTryConnectResult(connectionStringName, flag);
			if (IsTryConnectInProgress(connectionStringName))
			{
				SaveTryConnectInProgress(connectionStringName, inProgress: false);
				if (IsValid())
				{
					Wizard.MoveToNextPage();
				}
			}
			HandleTryConnectSpinner(connectionStringName, isConnecting: false);
			ShowValidationResult(connectionStringName, flag ? VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Success : VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Fail, flag);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			SaveTryConnectResult(connectionStringName, isValid: false);
			SaveTryConnectInProgress(connectionStringName, inProgress: false);
			HandleTryConnectSpinner(connectionStringName, isConnecting: false);
			ShowValidationResult(connectionStringName, VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_Unexpected, isSuccess: false);
		}
	}

	private void HandleTryConnectSpinner(string connectionStringName, bool isConnecting)
	{
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			tryConnectManagementServerSpinner.Visible = isConnecting;
			tryConnectManagementServerButton.Visible = !isConnecting;
			textBoxMsServer.Enabled = !isConnecting;
			textBoxMsDatabaseName.Enabled = !isConnecting;
			if (isConnecting)
			{
				tryConnectManagementServerSpinner.Start();
			}
			else
			{
				tryConnectManagementServerSpinner.Stop();
			}
			break;
		case "SQL_CONNECTION_STRING_IDP":
			tryConnectIdpSpinner.Visible = isConnecting;
			tryConnectIdpButton.Visible = !isConnecting;
			textBoxIdpServer.Enabled = !isConnecting;
			textBoxIdpDatabaseName.Enabled = !isConnecting;
			if (isConnecting)
			{
				tryConnectIdpSpinner.Start();
			}
			else
			{
				tryConnectIdpSpinner.Stop();
			}
			break;
		case "SQL_CONNECTION_STRING_IM":
			tryConnectIncidentManagerSpinner.Visible = isConnecting;
			tryConnectIncidentManagerButton.Visible = !isConnecting;
			textBoxImServer.Enabled = !isConnecting;
			textBoxImDatabaseName.Enabled = !isConnecting;
			if (isConnecting)
			{
				tryConnectIncidentManagerSpinner.Start();
			}
			else
			{
				tryConnectIncidentManagerSpinner.Stop();
			}
			break;
		case "SQL_CONNECTION_STRING_LOG":
			tryConnectLogServerSpinner.Visible = isConnecting;
			tryConnectLogServerButton.Visible = !isConnecting;
			textBoxLogServer.Enabled = !isConnecting;
			textBoxLogServerDatabaseName.Enabled = !isConnecting;
			if (isConnecting)
			{
				tryConnectLogServerSpinner.Start();
			}
			else
			{
				tryConnectLogServerSpinner.Stop();
			}
			break;
		}
		comboBoxAuthenticationType.Enabled = !IsAnyTryConnectionInProgress();
		Wizard.NextButton.Enabled = !IsAnyTryConnectionInProgress();
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

	private void HideValidationResult(string connectionStringName)
	{
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			labelManagementServerConnectionStatus.Visible = false;
			break;
		case "SQL_CONNECTION_STRING_IDP":
			labelIdpConnectionStatus.Visible = false;
			break;
		case "SQL_CONNECTION_STRING_IM":
			labelImConnectionStatus.Visible = false;
			break;
		case "SQL_CONNECTION_STRING_LOG":
			labelLogServerConnectionStatus.Visible = false;
			break;
		}
	}

	private void ShowValidationResult(string connectionStringName, string message, bool isSuccess)
	{
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			labelManagementServerConnectionStatus.Text = message;
			labelManagementServerConnectionStatus.ForeColor = (isSuccess ? Color.Green : Color.Red);
			labelManagementServerConnectionStatus.Visible = true;
			break;
		case "SQL_CONNECTION_STRING_IDP":
			labelIdpConnectionStatus.Text = message;
			labelIdpConnectionStatus.ForeColor = (isSuccess ? Color.Green : Color.Red);
			labelIdpConnectionStatus.Visible = true;
			break;
		case "SQL_CONNECTION_STRING_IM":
			labelImConnectionStatus.Text = message;
			labelImConnectionStatus.ForeColor = (isSuccess ? Color.Green : Color.Red);
			labelImConnectionStatus.Visible = true;
			break;
		case "SQL_CONNECTION_STRING_LOG":
			labelLogServerConnectionStatus.Text = message;
			labelLogServerConnectionStatus.ForeColor = (isSuccess ? Color.Green : Color.Red);
			labelLogServerConnectionStatus.Visible = true;
			break;
		}
	}

	private string GenerateConnectionString(string connectionStringName)
	{
		AuthenticationType selectedIndex = (AuthenticationType)comboBoxAuthenticationType.SelectedIndex;
		string text;
		string text2;
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
		case "SQL_CONNECTION_STRING_ES":
			text = textBoxMsServer.Text;
			text2 = textBoxMsDatabaseName.Text;
			break;
		case "SQL_CONNECTION_STRING_IDP":
			text = textBoxIdpServer.Text;
			text2 = textBoxIdpDatabaseName.Text;
			break;
		case "SQL_CONNECTION_STRING_IM":
			text = textBoxImServer.Text;
			text2 = textBoxImDatabaseName.Text;
			break;
		case "SQL_CONNECTION_STRING_LOG":
			text = textBoxLogServer.Text;
			text2 = textBoxLogServerDatabaseName.Text;
			break;
		default:
			throw new Exception("Unknown XProtect component. Name: " + connectionStringName);
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new Exception(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_MissingServerName);
		}
		if (string.IsNullOrEmpty(text2))
		{
			throw new Exception(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseAdvancedSetup_TryConnect_MissingDatabaseName);
		}
		return new DatabaseConnectionInfo(text, text2, selectedIndex).ConnectionString;
	}

	private void TryPopulateConnectionString(string connectionStringName)
	{
		try
		{
			DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(GetConnectionString(connectionStringName).ConnectionString);
			switch (connectionStringName)
			{
			case "SQL_CONNECTION_STRING_MS":
				textBoxMsServer.Text = databaseConnectionInfo.ServerName;
				textBoxMsDatabaseName.Text = databaseConnectionInfo.DatabaseName;
				break;
			case "SQL_CONNECTION_STRING_IDP":
				textBoxIdpServer.Text = databaseConnectionInfo.ServerName;
				textBoxIdpDatabaseName.Text = databaseConnectionInfo.DatabaseName;
				break;
			case "SQL_CONNECTION_STRING_IM":
				textBoxImServer.Text = databaseConnectionInfo.ServerName;
				textBoxImDatabaseName.Text = databaseConnectionInfo.DatabaseName;
				break;
			case "SQL_CONNECTION_STRING_LOG":
				textBoxLogServer.Text = databaseConnectionInfo.ServerName;
				textBoxLogServerDatabaseName.Text = databaseConnectionInfo.DatabaseName;
				break;
			}
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
		switch (connectionStringName)
		{
		case "SQL_CONNECTION_STRING_MS":
			text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.ManagementServer, new RegistrySqlConnectionStorage());
			break;
		case "SQL_CONNECTION_STRING_IDP":
			text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.IDP, new IdpRegistrySqlConnectionStorage());
			break;
		case "SQL_CONNECTION_STRING_IM":
			text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.IncidentManager, null);
			break;
		case "SQL_CONNECTION_STRING_LOG":
			text = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.LogServer, null);
			break;
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
		this.labelMs = new System.Windows.Forms.Label();
		this.textBoxMsServer = new System.Windows.Forms.TextBox();
		this.textBoxMsDatabaseName = new System.Windows.Forms.TextBox();
		this.textBoxIdpServer = new System.Windows.Forms.TextBox();
		this.textBoxIdpDatabaseName = new System.Windows.Forms.TextBox();
		this.textBoxImServer = new System.Windows.Forms.TextBox();
		this.textBoxImDatabaseName = new System.Windows.Forms.TextBox();
		this.textBoxLogServer = new System.Windows.Forms.TextBox();
		this.textBoxLogServerDatabaseName = new System.Windows.Forms.TextBox();
		this.labelIdp = new System.Windows.Forms.Label();
		this.labelIm = new System.Windows.Forms.Label();
		this.labelLogServer = new System.Windows.Forms.Label();
		this.labelAuthenticationType = new System.Windows.Forms.Label();
		this.labelServer = new System.Windows.Forms.Label();
		this.labelDatabaseName = new System.Windows.Forms.Label();
		this.tryConnectManagementServerButton = new System.Windows.Forms.Button();
		this.tryConnectIdpButton = new System.Windows.Forms.Button();
		this.tryConnectIncidentManagerButton = new System.Windows.Forms.Button();
		this.tryConnectLogServerButton = new System.Windows.Forms.Button();
		this.labelManagementServerConnectionStatus = new System.Windows.Forms.Label();
		this.labelIdpConnectionStatus = new System.Windows.Forms.Label();
		this.labelImConnectionStatus = new System.Windows.Forms.Label();
		this.labelLogServerConnectionStatus = new System.Windows.Forms.Label();
		this.tryConnectManagementServerSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		this.tryConnectIdpSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		this.tryConnectIncidentManagerSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		this.tryConnectLogServerSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		this.comboBoxAuthenticationType = new System.Windows.Forms.ComboBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.labelAuthenticationType);
		base.WizardPage.Controls.Add(this.textBoxLogServerDatabaseName);
		base.WizardPage.Controls.Add(this.textBoxImDatabaseName);
		base.WizardPage.Controls.Add(this.textBoxIdpDatabaseName);
		base.WizardPage.Controls.Add(this.textBoxIdpServer);
		base.WizardPage.Controls.Add(this.textBoxMsDatabaseName);
		base.WizardPage.Controls.Add(this.labelDatabaseName);
		base.WizardPage.Controls.Add(this.textBoxMsServer);
		base.WizardPage.Controls.Add(this.textBoxImServer);
		base.WizardPage.Controls.Add(this.labelServer);
		base.WizardPage.Controls.Add(this.textBoxLogServer);
		base.WizardPage.Controls.Add(this.labelIdp);
		base.WizardPage.Controls.Add(this.labelIm);
		base.WizardPage.Controls.Add(this.labelLogServer);
		base.WizardPage.Controls.Add(this.labelMs);
		base.WizardPage.Controls.Add(this.tryConnectLogServerButton);
		base.WizardPage.Controls.Add(this.tryConnectIncidentManagerButton);
		base.WizardPage.Controls.Add(this.tryConnectIdpButton);
		base.WizardPage.Controls.Add(this.tryConnectManagementServerButton);
		base.WizardPage.Controls.Add(this.labelLogServerConnectionStatus);
		base.WizardPage.Controls.Add(this.labelImConnectionStatus);
		base.WizardPage.Controls.Add(this.labelIdpConnectionStatus);
		base.WizardPage.Controls.Add(this.labelManagementServerConnectionStatus);
		base.WizardPage.Controls.Add(this.tryConnectManagementServerSpinner);
		base.WizardPage.Controls.Add(this.tryConnectLogServerSpinner);
		base.WizardPage.Controls.Add(this.tryConnectIncidentManagerSpinner);
		base.WizardPage.Controls.Add(this.tryConnectIdpSpinner);
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
		this.labelHeadline.Size = new System.Drawing.Size(539, 25);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "&Advanced database setup";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 73);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 41);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "&Provide server and database names for selected XProtect components and select an authentication type.";
		this.labelMs.AutoSize = true;
		this.labelMs.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelMs.Location = new System.Drawing.Point(47, 220);
		this.labelMs.Name = "labelMs";
		this.labelMs.Size = new System.Drawing.Size(121, 14);
		this.labelMs.TabIndex = 2;
		this.labelMs.Text = "&Management Server:";
		this.textBoxMsServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxMsServer.Location = new System.Drawing.Point(191, 217);
		this.textBoxMsServer.Name = "textBoxMsServer";
		this.textBoxMsServer.Size = new System.Drawing.Size(175, 22);
		this.textBoxMsServer.TabIndex = 4;
		this.textBoxMsDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxMsDatabaseName.Location = new System.Drawing.Point(383, 217);
		this.textBoxMsDatabaseName.Name = "textBoxMsDatabaseName";
		this.textBoxMsDatabaseName.Size = new System.Drawing.Size(175, 22);
		this.textBoxMsDatabaseName.TabIndex = 6;
		this.textBoxIdpServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxIdpServer.Location = new System.Drawing.Point(191, 267);
		this.textBoxIdpServer.Name = "textBoxIdpServer";
		this.textBoxIdpServer.Size = new System.Drawing.Size(175, 22);
		this.textBoxIdpServer.TabIndex = 9;
		this.textBoxIdpDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxIdpDatabaseName.Location = new System.Drawing.Point(383, 267);
		this.textBoxIdpDatabaseName.Name = "textBoxIdpDatabaseName";
		this.textBoxIdpDatabaseName.Size = new System.Drawing.Size(175, 22);
		this.textBoxIdpDatabaseName.TabIndex = 10;
		this.textBoxImServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxImServer.Location = new System.Drawing.Point(191, 317);
		this.textBoxImServer.Name = "textBoxImServer";
		this.textBoxImServer.Size = new System.Drawing.Size(175, 22);
		this.textBoxImServer.TabIndex = 13;
		this.textBoxImDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxImDatabaseName.Location = new System.Drawing.Point(383, 317);
		this.textBoxImDatabaseName.Name = "textBoxImDatabaseName";
		this.textBoxImDatabaseName.Size = new System.Drawing.Size(175, 22);
		this.textBoxImDatabaseName.TabIndex = 14;
		this.textBoxLogServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxLogServer.Location = new System.Drawing.Point(191, 367);
		this.textBoxLogServer.Name = "textBoxLogServer";
		this.textBoxLogServer.Size = new System.Drawing.Size(175, 22);
		this.textBoxLogServer.TabIndex = 17;
		this.textBoxLogServerDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxLogServerDatabaseName.Location = new System.Drawing.Point(383, 367);
		this.textBoxLogServerDatabaseName.Name = "textBoxLogServerDatabaseName";
		this.textBoxLogServerDatabaseName.Size = new System.Drawing.Size(175, 22);
		this.textBoxLogServerDatabaseName.TabIndex = 18;
		this.labelIdp.AutoSize = true;
		this.labelIdp.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelIdp.Location = new System.Drawing.Point(47, 270);
		this.labelIdp.Name = "labelIdp";
		this.labelIdp.Size = new System.Drawing.Size(93, 14);
		this.labelIdp.TabIndex = 8;
		this.labelIdp.Text = "&Identity Server:";
		this.labelIm.AutoSize = true;
		this.labelIm.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelIm.Location = new System.Drawing.Point(47, 320);
		this.labelIm.Name = "labelIm";
		this.labelIm.Size = new System.Drawing.Size(106, 14);
		this.labelIm.TabIndex = 12;
		this.labelIm.Text = "&Incident Manager:";
		this.labelLogServer.AutoSize = true;
		this.labelLogServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLogServer.Location = new System.Drawing.Point(47, 370);
		this.labelLogServer.Name = "labelLogServer";
		this.labelLogServer.Size = new System.Drawing.Size(70, 14);
		this.labelLogServer.TabIndex = 16;
		this.labelLogServer.Text = "&Log Server:";
		this.labelAuthenticationType.AutoSize = true;
		this.labelAuthenticationType.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelAuthenticationType.Location = new System.Drawing.Point(47, 120);
		this.labelAuthenticationType.Name = "labelAuthenticationType";
		this.labelAuthenticationType.Size = new System.Drawing.Size(121, 14);
		this.labelAuthenticationType.TabIndex = 2;
		this.labelAuthenticationType.Text = "&Authentication type:";
		this.labelServer.AutoSize = true;
		this.labelServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelServer.Location = new System.Drawing.Point(188, 193);
		this.labelServer.Name = "labelServer";
		this.labelServer.Size = new System.Drawing.Size(46, 14);
		this.labelServer.TabIndex = 3;
		this.labelServer.Text = "&Server:";
		this.labelDatabaseName.AutoSize = true;
		this.labelDatabaseName.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelDatabaseName.Location = new System.Drawing.Point(380, 193);
		this.labelDatabaseName.Name = "labelDatabaseName";
		this.labelDatabaseName.Size = new System.Drawing.Size(95, 14);
		this.labelDatabaseName.TabIndex = 5;
		this.labelDatabaseName.Text = "&Database name:";
		this.tryConnectManagementServerButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.tryConnectManagementServerButton.Location = new System.Drawing.Point(567, 215);
		this.tryConnectManagementServerButton.Name = "tryConnectManagementServerButton";
		this.tryConnectManagementServerButton.Size = new System.Drawing.Size(26, 26);
		this.tryConnectManagementServerButton.TabIndex = 7;
		this.tryConnectManagementServerButton.UseVisualStyleBackColor = true;
		this.tryConnectIdpButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.tryConnectIdpButton.Location = new System.Drawing.Point(567, 265);
		this.tryConnectIdpButton.Name = "tryConnectIdpButton";
		this.tryConnectIdpButton.Size = new System.Drawing.Size(26, 26);
		this.tryConnectIdpButton.TabIndex = 11;
		this.tryConnectIdpButton.UseVisualStyleBackColor = true;
		this.tryConnectIncidentManagerButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.tryConnectIncidentManagerButton.Location = new System.Drawing.Point(567, 315);
		this.tryConnectIncidentManagerButton.Name = "tryConnectIncidentManagerButton";
		this.tryConnectIncidentManagerButton.Size = new System.Drawing.Size(26, 26);
		this.tryConnectIncidentManagerButton.TabIndex = 15;
		this.tryConnectIncidentManagerButton.UseVisualStyleBackColor = true;
		this.tryConnectLogServerButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.tryConnectLogServerButton.Location = new System.Drawing.Point(567, 365);
		this.tryConnectLogServerButton.Name = "tryConnectLogServerButton";
		this.tryConnectLogServerButton.Size = new System.Drawing.Size(26, 26);
		this.tryConnectLogServerButton.TabIndex = 19;
		this.tryConnectLogServerButton.UseVisualStyleBackColor = true;
		this.labelManagementServerConnectionStatus.AutoSize = true;
		this.labelManagementServerConnectionStatus.BackColor = System.Drawing.Color.Transparent;
		this.labelManagementServerConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelManagementServerConnectionStatus.ForeColor = System.Drawing.Color.Red;
		this.labelManagementServerConnectionStatus.Location = new System.Drawing.Point(188, 242);
		this.labelManagementServerConnectionStatus.Name = "labelManagementServerConnectionStatus";
		this.labelManagementServerConnectionStatus.Size = new System.Drawing.Size(263, 15);
		this.labelManagementServerConnectionStatus.TabIndex = 1;
		this.labelManagementServerConnectionStatus.Text = "Can't connect to Management Server database";
		this.labelManagementServerConnectionStatus.Visible = false;
		this.labelIdpConnectionStatus.AutoSize = true;
		this.labelIdpConnectionStatus.BackColor = System.Drawing.Color.Transparent;
		this.labelIdpConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelIdpConnectionStatus.ForeColor = System.Drawing.Color.Red;
		this.labelIdpConnectionStatus.Location = new System.Drawing.Point(188, 292);
		this.labelIdpConnectionStatus.Name = "labelIdpConnectionStatus";
		this.labelIdpConnectionStatus.Size = new System.Drawing.Size(263, 15);
		this.labelIdpConnectionStatus.TabIndex = 2;
		this.labelIdpConnectionStatus.Text = "Can't connect to Management Server database";
		this.labelIdpConnectionStatus.Visible = false;
		this.labelImConnectionStatus.AutoSize = true;
		this.labelImConnectionStatus.BackColor = System.Drawing.Color.Transparent;
		this.labelImConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelImConnectionStatus.ForeColor = System.Drawing.Color.Red;
		this.labelImConnectionStatus.Location = new System.Drawing.Point(188, 342);
		this.labelImConnectionStatus.Name = "labelImConnectionStatus";
		this.labelImConnectionStatus.Size = new System.Drawing.Size(263, 15);
		this.labelImConnectionStatus.TabIndex = 3;
		this.labelImConnectionStatus.Text = "Can't connect to Management Server database";
		this.labelImConnectionStatus.Visible = false;
		this.labelLogServerConnectionStatus.AutoSize = true;
		this.labelLogServerConnectionStatus.BackColor = System.Drawing.Color.Transparent;
		this.labelLogServerConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLogServerConnectionStatus.ForeColor = System.Drawing.Color.Red;
		this.labelLogServerConnectionStatus.Location = new System.Drawing.Point(188, 392);
		this.labelLogServerConnectionStatus.Name = "labelLogServerConnectionStatus";
		this.labelLogServerConnectionStatus.Size = new System.Drawing.Size(263, 15);
		this.labelLogServerConnectionStatus.TabIndex = 4;
		this.labelLogServerConnectionStatus.Text = "Can't connect to Management Server database";
		this.labelLogServerConnectionStatus.Visible = false;
		this.tryConnectManagementServerSpinner.BackColor = System.Drawing.Color.Transparent;
		this.tryConnectManagementServerSpinner.Location = new System.Drawing.Point(563, 211);
		this.tryConnectManagementServerSpinner.Name = "tryConnectManagementServerSpinner";
		this.tryConnectManagementServerSpinner.Size = new System.Drawing.Size(34, 34);
		this.tryConnectManagementServerSpinner.TabIndex = 2;
		this.tryConnectManagementServerSpinner.TabStop = false;
		this.tryConnectIdpSpinner.BackColor = System.Drawing.Color.Transparent;
		this.tryConnectIdpSpinner.Location = new System.Drawing.Point(563, 261);
		this.tryConnectIdpSpinner.Name = "tryConnectIdpSpinner";
		this.tryConnectIdpSpinner.Size = new System.Drawing.Size(34, 34);
		this.tryConnectIdpSpinner.TabIndex = 3;
		this.tryConnectIdpSpinner.TabStop = false;
		this.tryConnectIncidentManagerSpinner.BackColor = System.Drawing.Color.Transparent;
		this.tryConnectIncidentManagerSpinner.Location = new System.Drawing.Point(563, 311);
		this.tryConnectIncidentManagerSpinner.Name = "tryConnectIncidentManagerSpinner";
		this.tryConnectIncidentManagerSpinner.Size = new System.Drawing.Size(34, 34);
		this.tryConnectIncidentManagerSpinner.TabIndex = 4;
		this.tryConnectIncidentManagerSpinner.TabStop = false;
		this.tryConnectLogServerSpinner.BackColor = System.Drawing.Color.Transparent;
		this.tryConnectLogServerSpinner.Location = new System.Drawing.Point(563, 361);
		this.tryConnectLogServerSpinner.Name = "tryConnectLogServerSpinner";
		this.tryConnectLogServerSpinner.Size = new System.Drawing.Size(34, 34);
		this.tryConnectLogServerSpinner.TabIndex = 5;
		this.tryConnectLogServerSpinner.TabStop = false;
		this.comboBoxAuthenticationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBoxAuthenticationType.FormattingEnabled = true;
		this.comboBoxAuthenticationType.Location = new System.Drawing.Point(50, 144);
		this.comboBoxAuthenticationType.Name = "comboBoxAuthenticationType";
		this.comboBoxAuthenticationType.Size = new System.Drawing.Size(508, 24);
		this.comboBoxAuthenticationType.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageDatabaseAdvancedSetup";
		this.PageName = "DatabaseAdvancedSetupPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

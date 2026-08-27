using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class WizardPageDatabaseInstallType : WizardPageBase
{
	public class Native
	{
		public const short SQL_HANDLE_ENV = 1;

		public const short SQL_HANDLE_DBC = 2;

		public const int SQL_ATTR_ODBC_VERSION = 200;

		public const int SQL_OV_ODBC3 = 3;

		public const short SQL_SUCCESS = 0;

		public const short SQL_NEED_DATA = 99;

		public const short DEFAULT_RESULT_SIZE = 1024;

		public const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";

		[DllImport("odbc32.dll")]
		public static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);

		[DllImport("odbc32.dll")]
		public static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);

		[DllImport("odbc32.dll")]
		public static extern short SQLFreeHandle(short hType, IntPtr handle);

		[DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
		public static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, short inStringLength, StringBuilder outString, short outStringLength, out short outLengthNeeded);
	}

	private delegate void BeginGetSQLServersDelegate();

	private delegate void GetAvailableSQLServerEndedDelegate();

	private string[] _availableServers;

	private Wizard _wizard;

	private bool _isInitializing = true;

	private string _instanceName;

	private bool _retrievingServersList;

	private IContainer components;

	private ComboBox comboBoxExistServerName;

	private RadioButton radioButtonUseExistingServer;

	private RadioButton radioButtonInstallNewServer;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Button refreshServersListButton;

	private RadioButton radioButtonManuallyAddExistingServer;

	private TextBox textBoxExistingServerName;

	private Label labelExistingServerConnectError;

	private Label labelManualServerConnectError;

	private UserControlProgressSpinner userControlProgressSpinner;

	public WizardPageDatabaseInstallType()
	{
		InitializeComponent();
		SetComboboxStatusMessage(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_PrepareSQLServers);
		SetControlsState();
		LoadResources();
		textBoxExistingServerName.TextChanged += textBoxExistingServerName_TextChanged;
		_isInitializing = false;
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
		_wizard = Page.Parent as Wizard;
		if (!radioButtonInstallNewServer.Enabled && comboBoxExistServerName.SelectedIndex == -1)
		{
			_wizard.NextEnabled = false;
		}
		if (_availableServers == null || _availableServers.Length == 0)
		{
			GetServersList();
		}
		SetInitButtonsState();
	}

	private void LoadResources()
	{
		labelHeadline.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_HeadLine;
		labelSubHeading.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_SubLine;
		radioButtonInstallNewServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_InstallNew;
		radioButtonUseExistingServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_UseExisting;
		radioButtonManuallyAddExistingServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_ManualSelectServer;
		labelExistingServerConnectError.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_UserRightsError;
		labelManualServerConnectError.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_ServerNotFoundError;
		SetTextBoxExistingServerNameDefaultText();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void SetControlsState()
	{
		comboBoxExistServerName.Enabled = radioButtonUseExistingServer.Checked && !_retrievingServersList;
		refreshServersListButton.Enabled = radioButtonUseExistingServer.Checked && !_retrievingServersList;
		textBoxExistingServerName.Enabled = radioButtonManuallyAddExistingServer.Checked;
		labelExistingServerConnectError.Visible = radioButtonUseExistingServer.Checked && labelExistingServerConnectError.Visible;
		labelManualServerConnectError.Visible = radioButtonManuallyAddExistingServer.Checked && labelManualServerConnectError.Visible;
		if (!_isInitializing && _wizard != null && _wizard.Page == Page)
		{
			if (radioButtonInstallNewServer.Checked || (radioButtonUseExistingServer.Checked && comboBoxExistServerName.SelectedIndex != -1 && comboBoxExistServerName.DropDownStyle == ComboBoxStyle.DropDownList && !labelExistingServerConnectError.Visible) || (radioButtonManuallyAddExistingServer.Checked && CommonHelper.IsValidSqlServerPath(textBoxExistingServerName.Text) && !textBoxExistingServerName.Text.Equals(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_InsertServerAddress) && !labelManualServerConnectError.Visible))
			{
				_wizard.NextEnabled = true;
			}
			else
			{
				_wizard.NextEnabled = false;
			}
		}
	}

	private void SetInitButtonsState()
	{
		bool flag = InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") && InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
		if (flag)
		{
			_instanceName = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"]).ServerName;
		}
		else if (!flag && InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			_instanceName = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_LOG"]).ServerName;
		}
		else
		{
			LogManager.Write("Specify the default SQL Server instance.", TraceEventType.Error);
		}
		labelExistingServerConnectError.Visible = false;
		labelManualServerConnectError.Visible = false;
		if (InstallerManager.Instance.InstallerConfig.InstallerSettings.DisableInstallSqlServer)
		{
			LogManager.Write("Installation of SQL disabled from the InstallerConfig.xml");
			radioButtonUseExistingServer.Checked = true;
			radioButtonInstallNewServer.Enabled = false;
		}
		else if (DatabaseCommon.IsDefaultInstancePresent())
		{
			radioButtonInstallNewServer.Enabled = true;
			radioButtonInstallNewServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_UseInstalled;
			radioButtonInstallNewServer.Checked = true;
			SetTextBoxExistingServerNameDefaultText();
		}
		else if (IsUpgrade())
		{
			textBoxExistingServerName.Text = _instanceName;
			radioButtonManuallyAddExistingServer.Checked = true;
		}
		else
		{
			radioButtonInstallNewServer.Enabled = true;
			radioButtonInstallNewServer.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_InstallNew;
			radioButtonInstallNewServer.Checked = true;
		}
	}

	private void GetServersList()
	{
		SetComboboxStatusMessage(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_PrepareSQLServers);
		ShowSpinner();
		BeginGetSQLServer(GetSQLServerEnded, null);
	}

	public IAsyncResult BeginGetSQLServer(AsyncCallback callback, object asyncState)
	{
		_retrievingServersList = true;
		SetControlsState();
		BeginGetSQLServersDelegate beginGetSQLServersDelegate = GetSQLServers;
		return beginGetSQLServersDelegate.BeginInvoke(callback, beginGetSQLServersDelegate);
	}

	public void GetSQLServers()
	{
		int i = 0;
		List<string> list = new List<string>();
		for (; i < 2; i++)
		{
			string[] array = null;
			string text = string.Empty;
			IntPtr outputHandle = IntPtr.Zero;
			IntPtr outputHandle2 = IntPtr.Zero;
			StringBuilder stringBuilder = new StringBuilder("DRIVER=SQL SERVER");
			StringBuilder stringBuilder2 = new StringBuilder(1024);
			short inStringLength = (short)stringBuilder.Length;
			short outLengthNeeded = 0;
			try
			{
				if (Native.SQLAllocHandle(1, outputHandle, out outputHandle) == 0 && Native.SQLSetEnvAttr(outputHandle, 200, (IntPtr)3, 0) == 0 && Native.SQLAllocHandle(2, outputHandle, out outputHandle2) == 0 && 99 == Native.SQLBrowseConnect(outputHandle2, stringBuilder, inStringLength, stringBuilder2, 1024, out outLengthNeeded))
				{
					if (1024 < outLengthNeeded)
					{
						stringBuilder2.Capacity = outLengthNeeded;
						if (99 != Native.SQLBrowseConnect(outputHandle2, stringBuilder, inStringLength, stringBuilder2, outLengthNeeded, out outLengthNeeded))
						{
							throw new UIFrameworkErrorException("Unabled to aquire SQL Servers from ODBC driver.");
						}
					}
					text = stringBuilder2.ToString();
					int num = text.IndexOf("{") + 1;
					int num2 = text.IndexOf("}") - num;
					text = ((num <= 0 || num2 <= 0) ? string.Empty : text.Substring(num, num2));
				}
			}
			catch (Exception ex)
			{
				ex.GetType();
				text = string.Empty;
			}
			finally
			{
				if (outputHandle2 != IntPtr.Zero)
				{
					Native.SQLFreeHandle(2, outputHandle2);
				}
				if (outputHandle != IntPtr.Zero)
				{
					Native.SQLFreeHandle(1, outputHandle2);
				}
			}
			if (text.Length > 0)
			{
				array = text.Split(',');
			}
			if (array != null)
			{
				list.AddRange(array);
			}
		}
		_availableServers = list.Distinct().ToArray();
	}

	private void GetSQLServerEnded(IAsyncResult ar)
	{
		((BeginGetSQLServersDelegate)ar.AsyncState).EndInvoke(ar);
		WizardPage.BeginInvoke(new GetAvailableSQLServerEndedDelegate(EndGetSQLServers));
	}

	public void EndGetSQLServers()
	{
		_retrievingServersList = false;
		FillAvailableSQLServers();
		SetControlsState();
		HideSpinner();
	}

	public void FillAvailableSQLServers()
	{
		if (_availableServers != null && _availableServers.Length != 0)
		{
			comboBoxExistServerName.Items.Clear();
			ComboBox.ObjectCollection items = comboBoxExistServerName.Items;
			object[] availableServers = _availableServers;
			items.AddRange(availableServers);
			if (comboBoxExistServerName.SelectedIndex == -1 && comboBoxExistServerName.Items.Count > 0)
			{
				comboBoxExistServerName.SelectedIndex = 0;
			}
			LogManager.Write("Instance name: " + _instanceName);
			foreach (string item in comboBoxExistServerName.Items)
			{
				if (!string.IsNullOrEmpty(_instanceName) && item.EndsWith(_instanceName, StringComparison.InvariantCultureIgnoreCase))
				{
					comboBoxExistServerName.SelectedItem = item;
				}
			}
			SetComboboxToDropDownList();
		}
		else if (_wizard.Page == Page)
		{
			SetComboboxStatusMessage(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseExistType_FailedLoadServers);
		}
	}

	private bool IsUpgrade()
	{
		if (!string.IsNullOrEmpty(_instanceName) && InstallerManager.Instance.Parameters.ContainsKey("IS_UPGRADE"))
		{
			return InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true";
		}
		return false;
	}

	private void radioButtons_CheckedChanged(object sender, EventArgs e)
	{
		if (sender is RadioButton { Checked: not false })
		{
			SetControlsState();
		}
	}

	private void comboBoxExistDBName_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboBoxExistServerName.SelectedIndex != -1 && radioButtonUseExistingServer.Checked)
		{
			_wizard.NextEnabled = true;
		}
	}

	private void comboBoxExistDBName_TextChanged(object sender, EventArgs e)
	{
		if (comboBoxExistServerName.SelectedIndex == -1 || _retrievingServersList)
		{
			comboBoxExistServerName.Font = new Font(comboBoxExistServerName.Font, FontStyle.Italic);
		}
		else
		{
			comboBoxExistServerName.Font = new Font(comboBoxExistServerName.Font, FontStyle.Regular);
		}
		labelExistingServerConnectError.Visible = false;
	}

	private void textBoxExistingServerName_TextChanged(object sender, EventArgs e)
	{
		if (_wizard != null)
		{
			_wizard.NextEnabled = radioButtonManuallyAddExistingServer.Checked && CommonHelper.IsValidSqlServerPath(textBoxExistingServerName.Text);
		}
		if (textBoxExistingServerName.Text.Equals(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_InsertServerAddress))
		{
			textBoxExistingServerName.Font = new Font(textBoxExistingServerName.Font, FontStyle.Italic);
		}
		else
		{
			textBoxExistingServerName.Font = new Font(textBoxExistingServerName.Font, FontStyle.Regular);
		}
		labelManualServerConnectError.Visible = false;
	}

	private void textBoxExistingServerName_Enter(object sender, EventArgs e)
	{
		if (textBoxExistingServerName.Text.Equals(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_InsertServerAddress))
		{
			textBoxExistingServerName.Text = "";
		}
	}

	private void textBoxExistingServerName_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(textBoxExistingServerName.Text))
		{
			SetTextBoxExistingServerNameDefaultText();
		}
	}

	private void refreshServersListButton_Clicked(object sender, EventArgs e)
	{
		if (!_retrievingServersList)
		{
			GetServersList();
		}
	}

	private void ShowSpinner()
	{
		refreshServersListButton.Visible = false;
		userControlProgressSpinner.Visible = true;
		userControlProgressSpinner.Start();
	}

	private void HideSpinner()
	{
		userControlProgressSpinner.Stop();
		userControlProgressSpinner.Visible = false;
		refreshServersListButton.Visible = true;
	}

	private void SetComboboxStatusMessage(string message)
	{
		SetComboboxToDropDown();
		comboBoxExistServerName.Text = message;
	}

	private void SetComboboxToDropDown()
	{
		comboBoxExistServerName.DropDownStyle = ComboBoxStyle.DropDown;
		SetControlsState();
	}

	private void SetComboboxToDropDownList()
	{
		comboBoxExistServerName.DropDownStyle = ComboBoxStyle.DropDownList;
		SetControlsState();
	}

	private void SetTextBoxExistingServerNameDefaultText()
	{
		textBoxExistingServerName.Text = VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_InsertServerAddress;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return null;
	}

	public override bool OnBeforeContinue()
	{
		bool flag = radioButtonInstallNewServer.Checked && !DatabaseCommon.IsDefaultInstancePresent();
		bool flag2 = radioButtonInstallNewServer.Checked || IsLocalServerSelected();
		string text = (radioButtonUseExistingServer.Checked ? comboBoxExistServerName.Text.Trim().ToUpperInvariant().Replace("(LOCAL)", "localhost") : (radioButtonManuallyAddExistingServer.Checked ? textBoxExistingServerName.Text.Trim() : "localhost"));
		bool num = InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER") && InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
		GlobalData.SetValue("InstallNewServerChecked", flag ? "1" : "0", "dbInstallPage.OnBeforeContinue()");
		GlobalData.SetValue("SQL-SERVER-LOCAL", flag2 ? "yes" : "no", "dbInstallPage.OnBeforeContinue()");
		string value;
		if (num)
		{
			DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"]);
			string logServerDatabaseName = "SurveillanceLogServerV2";
			AuthenticationType authenticationType = (flag ? AuthenticationType.IntegratedSecurityTrusted : databaseConnectionInfo.AuthenticationType);
			foreach (var item in TemporaryGenerateConnectionStringsByConvention(text, databaseConnectionInfo.DatabaseName, authenticationType, logServerDatabaseName))
			{
				GlobalData.SetValue(item.Key, item.ConnectionString, "dbInstallPage. OnBeforeContinue()");
			}
			GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out value);
		}
		else
		{
			if (!InstallerManager.Instance.Parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
			{
				throw new UIFrameworkErrorException(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstallType_UnsupportedComponent);
			}
			DatabaseConnectionInfo databaseConnectionInfo2 = new DatabaseConnectionInfo(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_LOG"]);
			AuthenticationType authenticationType2 = (flag ? AuthenticationType.IntegratedSecurityTrusted : databaseConnectionInfo2.AuthenticationType);
			string connectionString = new DatabaseConnectionInfo(text, databaseConnectionInfo2.DatabaseName, authenticationType2).ConnectionString;
			GlobalData.SetValue("SQL_CONNECTION_STRING_LOG", connectionString, "dbInstallPage. OnBeforeContinue()");
			value = connectionString;
		}
		IDatabaseConnectionInfo databaseConnectionInfo3 = new DatabaseConnectionInfo(value).CloneWithMasterDatabaseName();
		using (new ShowWaitCursor())
		{
			ISqlConnectionManager sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo3, null);
			LogManager.Write("Check OnBeforeContinue() Data Server:" + sqlConnectionManager.ServerName + " Database Name:" + sqlConnectionManager.DatabaseName);
			DataBaseErrorString dataBaseErrorString = DatabaseErrorCheckHelper.CheckSelectedDB(sqlConnectionManager);
			if (dataBaseErrorString != DataBaseErrorString.Empty && dataBaseErrorString != DataBaseErrorString.ServerNotFound)
			{
				LogManager.Write($"OnBeforeContinue(): Connection failed, database error: {dataBaseErrorString.ToString()}", TraceEventType.Error);
				if (dataBaseErrorString == DataBaseErrorString.SQLServerNotSupported)
				{
					MessageHelper.ShowInstallerMessageBox(string.Format(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_InstalledVersionNotSupportedHeadLine), string.Format(VideoOS.Installer.DatabasePlugin.Properties.Resources.DatabaseInstall_InstalledVersionNotSupported, DatabaseErrorCheckHelper.MinimumSupportedSqlVersionYear), InstallerMessageBoxIcon.Warning);
				}
				else
				{
					labelExistingServerConnectError.Visible = radioButtonUseExistingServer.Checked;
					labelManualServerConnectError.Visible = radioButtonManuallyAddExistingServer.Checked;
				}
				SetControlsState();
				return false;
			}
		}
		return true;
	}

	private bool IsLocalServerSelected()
	{
		string text = comboBoxExistServerName.Text.Trim();
		if (radioButtonUseExistingServer.Checked)
		{
			if (text.Equals("(local)", StringComparison.InvariantCultureIgnoreCase) || text.StartsWith("(local)\\", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (text.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) || text.StartsWith("localhost\\", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		else if (radioButtonManuallyAddExistingServer.Checked)
		{
			text = textBoxExistingServerName.Text.Trim();
			if (text.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) || text.StartsWith("localhost\\", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		if (text.StartsWith(Environment.MachineName, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static IList<(string Key, string ConnectionString)> TemporaryGenerateConnectionStringsByConvention(string dataSource, string databaseName, AuthenticationType authenticationType, string logServerDatabaseName)
	{
		return new(string, string)[5]
		{
			("SQL_CONNECTION_STRING_MS", new DatabaseConnectionInfo(dataSource, databaseName, authenticationType).ConnectionString),
			("SQL_CONNECTION_STRING_IDP", new DatabaseConnectionInfo(dataSource, databaseName + "_IDP", authenticationType).ConnectionString),
			("SQL_CONNECTION_STRING_IM", new DatabaseConnectionInfo(dataSource, databaseName + "_IM", authenticationType).ConnectionString),
			("SQL_CONNECTION_STRING_ES", new DatabaseConnectionInfo(dataSource, databaseName, authenticationType).ConnectionString),
			("SQL_CONNECTION_STRING_LOG", new DatabaseConnectionInfo(dataSource, logServerDatabaseName, authenticationType).ConnectionString)
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
		this.comboBoxExistServerName = new System.Windows.Forms.ComboBox();
		this.radioButtonUseExistingServer = new System.Windows.Forms.RadioButton();
		this.radioButtonInstallNewServer = new System.Windows.Forms.RadioButton();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.refreshServersListButton = new System.Windows.Forms.Button();
		this.radioButtonManuallyAddExistingServer = new System.Windows.Forms.RadioButton();
		this.textBoxExistingServerName = new System.Windows.Forms.TextBox();
		this.labelExistingServerConnectError = new System.Windows.Forms.Label();
		this.labelManualServerConnectError = new System.Windows.Forms.Label();
		this.userControlProgressSpinner = new VideoOS.Installer.UI.UserControlProgressSpinner();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.comboBoxExistServerName);
		base.WizardPage.Controls.Add(this.radioButtonUseExistingServer);
		base.WizardPage.Controls.Add(this.radioButtonInstallNewServer);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.refreshServersListButton);
		base.WizardPage.Controls.Add(this.userControlProgressSpinner);
		base.WizardPage.Controls.Add(this.radioButtonManuallyAddExistingServer);
		base.WizardPage.Controls.Add(this.textBoxExistingServerName);
		base.WizardPage.Controls.Add(this.labelExistingServerConnectError);
		base.WizardPage.Controls.Add(this.labelManualServerConnectError);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.comboBoxExistServerName.BackColor = System.Drawing.SystemColors.Menu;
		this.comboBoxExistServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBoxExistServerName.FormattingEnabled = true;
		this.comboBoxExistServerName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.comboBoxExistServerName.Location = new System.Drawing.Point(70, 235);
		this.comboBoxExistServerName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
		this.comboBoxExistServerName.Name = "comboBoxExistServerName";
		this.comboBoxExistServerName.Size = new System.Drawing.Size(406, 24);
		this.comboBoxExistServerName.TabIndex = 6;
		this.comboBoxExistServerName.SelectedIndexChanged += new System.EventHandler(comboBoxExistDBName_SelectedIndexChanged);
		this.comboBoxExistServerName.TextChanged += new System.EventHandler(comboBoxExistDBName_TextChanged);
		this.radioButtonUseExistingServer.AutoSize = true;
		this.radioButtonUseExistingServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonUseExistingServer.Location = new System.Drawing.Point(50, 210);
		this.radioButtonUseExistingServer.Name = "radioButtonUseExistingServer";
		this.radioButtonUseExistingServer.Size = new System.Drawing.Size(164, 18);
		this.radioButtonUseExistingServer.TabIndex = 5;
		this.radioButtonUseExistingServer.Text = "&Using existing SQL Server";
		this.radioButtonUseExistingServer.UseVisualStyleBackColor = true;
		this.radioButtonUseExistingServer.CheckedChanged += new System.EventHandler(radioButtons_CheckedChanged);
		this.radioButtonInstallNewServer.AutoSize = true;
		this.radioButtonInstallNewServer.Checked = true;
		this.radioButtonInstallNewServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonInstallNewServer.Location = new System.Drawing.Point(50, 171);
		this.radioButtonInstallNewServer.Name = "radioButtonInstallNewServer";
		this.radioButtonInstallNewServer.Size = new System.Drawing.Size(324, 18);
		this.radioButtonInstallNewServer.TabIndex = 4;
		this.radioButtonInstallNewServer.TabStop = true;
		this.radioButtonInstallNewServer.Text = "&Install new SQL Server 2008 Express on this computer";
		this.radioButtonInstallNewServer.UseVisualStyleBackColor = true;
		this.radioButtonInstallNewServer.CheckedChanged += new System.EventHandler(radioButtons_CheckedChanged);
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "Select MS SQL Server";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(47, 93);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(524, 46);
		this.labelSubHeading.TabIndex = 1;
		this.labelSubHeading.Text = "Select the type of database you wish to use. You can either choose to use a new, local MS SQL Server Express database or select existing SQL Server on the network.";
		this.refreshServersListButton.Image = VideoOS.Installer.UI.Properties.Resources.refresharrow_3_16x16;
		this.refreshServersListButton.Location = new System.Drawing.Point(486, 234);
		this.refreshServersListButton.Name = "refreshServersListButton";
		this.refreshServersListButton.Size = new System.Drawing.Size(26, 26);
		this.refreshServersListButton.TabIndex = 7;
		this.refreshServersListButton.UseVisualStyleBackColor = true;
		this.refreshServersListButton.Click += new System.EventHandler(refreshServersListButton_Clicked);
		this.radioButtonManuallyAddExistingServer.AutoSize = true;
		this.radioButtonManuallyAddExistingServer.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButtonManuallyAddExistingServer.Location = new System.Drawing.Point(50, 282);
		this.radioButtonManuallyAddExistingServer.Name = "radioButtonManuallyAddExistingServer";
		this.radioButtonManuallyAddExistingServer.Size = new System.Drawing.Size(157, 18);
		this.radioButtonManuallyAddExistingServer.TabIndex = 8;
		this.radioButtonManuallyAddExistingServer.Text = "&Select via direct address";
		this.radioButtonManuallyAddExistingServer.UseVisualStyleBackColor = true;
		this.radioButtonManuallyAddExistingServer.CheckedChanged += new System.EventHandler(radioButtons_CheckedChanged);
		this.textBoxExistingServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxExistingServerName.Location = new System.Drawing.Point(70, 307);
		this.textBoxExistingServerName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
		this.textBoxExistingServerName.MinimumSize = new System.Drawing.Size(406, 24);
		this.textBoxExistingServerName.Name = "textBoxExistingServerName";
		this.textBoxExistingServerName.Size = new System.Drawing.Size(406, 22);
		this.textBoxExistingServerName.TabIndex = 9;
		this.textBoxExistingServerName.Text = "Type server address";
		this.textBoxExistingServerName.Enter += new System.EventHandler(textBoxExistingServerName_Enter);
		this.textBoxExistingServerName.Leave += new System.EventHandler(textBoxExistingServerName_Leave);
		this.labelExistingServerConnectError.AutoSize = true;
		this.labelExistingServerConnectError.BackColor = System.Drawing.Color.Transparent;
		this.labelExistingServerConnectError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExistingServerConnectError.ForeColor = System.Drawing.Color.Red;
		this.labelExistingServerConnectError.Location = new System.Drawing.Point(70, 262);
		this.labelExistingServerConnectError.Name = "labelExistingServerConnectError";
		this.labelExistingServerConnectError.Size = new System.Drawing.Size(81, 15);
		this.labelExistingServerConnectError.TabIndex = 2;
		this.labelExistingServerConnectError.Visible = false;
		this.labelManualServerConnectError.AutoSize = true;
		this.labelManualServerConnectError.BackColor = System.Drawing.Color.Transparent;
		this.labelManualServerConnectError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.labelManualServerConnectError.ForeColor = System.Drawing.Color.Red;
		this.labelManualServerConnectError.Location = new System.Drawing.Point(70, 334);
		this.labelManualServerConnectError.Name = "labelManualServerConnectError";
		this.labelManualServerConnectError.Size = new System.Drawing.Size(81, 15);
		this.labelManualServerConnectError.TabIndex = 1;
		this.labelManualServerConnectError.Visible = false;
		this.userControlProgressSpinner.BackColor = System.Drawing.Color.Transparent;
		this.userControlProgressSpinner.Location = new System.Drawing.Point(482, 230);
		this.userControlProgressSpinner.Name = "userControlProgressSpinner";
		this.userControlProgressSpinner.Size = new System.Drawing.Size(34, 34);
		this.userControlProgressSpinner.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageDatabaseInstallType";
		this.PageName = "DatabaseInstallTypePage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

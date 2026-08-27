using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.RecorderPlugins.Properties;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.RecorderPlugins;

public class ServerConfigurationControl : WizardPageBase
{
	private enum ManagementServerConnectionStatus
	{
		ServerRunningUnsecure,
		ServerRunningSecure,
		LoginFailed,
		NotConnected
	}

	private Dictionary<string, string> _parameters;

	private bool _isStandAlone = true;

	private bool _isFaiolver;

	private string _enteredMediaDbPath;

	private string _enteredManServer;

	private string _enteredRecServerName;

	private bool _uiParams;

	private bool _serverUrlLocked;

	private ManagementServerInfo _managementServerInfo;

	private Uri _managementServerAddress;

	private string _defaultRecorderName;

	private string _defaultMediaPath;

	private IContainer components;

	private TextBox textBoxRecServerName;

	private Label labelRecServerName;

	private Label labelMServerName;

	private Label labelHeadLine;

	private TextBox textBoxHostName;

	private TextBox textBoxMediaDB;

	private Label labelMediaDB;

	private Label labelMServerExample;

	private Label textBoxMediaDatabaseInfo;

	private Label labelRetentionTime;

	private NumericUpDown retentionTimeUpDown;

	private Label labelDays;

	private Button buttonBrowseDatebaseLocation;

	private Label labelSpaceAvailable;

	private Label labelMServerNameInfo;

	private string DefaultRecorderName
	{
		get
		{
			if (_defaultRecorderName != null)
			{
				return _defaultRecorderName;
			}
			string text;
			if (_isStandAlone)
			{
				try
				{
					text = Dns.GetHostEntry("").HostName;
					LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Default Recorder name defined={0}", text));
				}
				catch (Exception ex)
				{
					LogManager.Write(ex.Message);
					text = Environment.MachineName;
				}
			}
			else
			{
				text = Environment.MachineName;
				LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Main installer detected, setting Recorder name to machine name={0}", text));
			}
			_defaultRecorderName = text;
			return text;
		}
	}

	private string DefaultMediaDbPath
	{
		get
		{
			if (_defaultMediaPath != null)
			{
				return _defaultMediaPath;
			}
			DriveInfo getLocalDriveWithMostAvailableFreeSpace = GetLocalDriveWithMostAvailableFreeSpace;
			string text = string.Format(CultureInfo.CurrentCulture, "{0}{1}", (getLocalDriveWithMostAvailableFreeSpace == null) ? "c:\\" : getLocalDriveWithMostAvailableFreeSpace.Name, "MediaDatabase");
			LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Default MediaDbPath defined = {0}", text));
			_defaultMediaPath = text;
			return text;
		}
	}

	public override bool ExecSetParametersInQuiet => true;

	private static DriveInfo GetLocalDriveWithMostAvailableFreeSpace
	{
		get
		{
			DriveInfo driveInfo = null;
			try
			{
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo2 in drives)
				{
					if (driveInfo2.IsReady && driveInfo2.DriveType == DriveType.Fixed && (driveInfo == null || driveInfo2.AvailableFreeSpace > driveInfo.AvailableFreeSpace))
					{
						driveInfo = driveInfo2;
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return driveInfo;
		}
	}

	public ServerConfigurationControl()
	{
		InitializeComponent();
		PageName = "ServerConfigurationPage";
		ReloadResources();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		ReloadResources();
	}

	private void ReloadResources()
	{
		labelHeadLine.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_HeadLine;
		labelMediaDB.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_MediaDb;
		labelMServerExample.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_ManagementServerExample;
		labelRecServerName.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_ServerName;
		labelMServerName.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_ManagementServerURL;
		labelMServerNameInfo.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_ManagementServerURLInfo;
		labelRetentionTime.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_RetentionTimeLabel;
		textBoxMediaDatabaseInfo.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_MediaDbInfo;
		labelDays.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_RetentionDaysLabel;
		buttonBrowseDatebaseLocation.Text = VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_BrowseButton;
		UpdateSpaceAvailable(DefaultMediaDbPath);
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER"))
		{
			_managementServerAddress = new Uri(_parameters["MANAGEMENTSERVERURI"]);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CONNECTEDTOSERVER set to false, setting server address to {0}", _managementServerAddress.AbsoluteUri), TraceEventType.Information);
		}
		if (_managementServerInfo != null)
		{
			dictionary["MANAGEMENTAPPNAME"] = _managementServerInfo.HttpAddress.AbsoluteUri;
			dictionary["MANAGEMENTAPPNAMESECURE"] = _managementServerInfo.HttpsAddress.AbsoluteUri;
			dictionary["IDPADDRESS"] = _managementServerInfo.IDPAddress.AbsoluteUri;
		}
		if (_managementServerAddress != null)
		{
			dictionary["MANAGEMENTSERVERURI"] = _managementServerAddress.AbsoluteUri;
		}
		dictionary["RECORDERNAME"] = textBoxRecServerName.Text;
		dictionary["MEDIADBPATH"] = textBoxMediaDB.Text;
		dictionary["STORAGERETENTIONTIME"] = retentionTimeUpDown.Value.ToString(CultureInfo.InvariantCulture);
		_uiParams = true;
		_enteredRecServerName = textBoxRecServerName.Text;
		_enteredMediaDbPath = textBoxMediaDB.Text;
		_enteredManServer = _parameters["MANAGEMENTSERVERURI"];
		if (PluginUtilities.Instance.FailoverInstallation)
		{
			dictionary["ISFAILOVER"] = bool.TrueString;
		}
		else
		{
			dictionary["ISFAILOVER"] = bool.FalseString;
		}
		return dictionary;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		_parameters = parameters ?? throw new ArgumentNullException("parameters");
		if (_parameters.ContainsKey("ISCORPORATEINSTALLER") && _parameters["ISCORPORATEINSTALLER"] == "true")
		{
			_isStandAlone = false;
		}
		else
		{
			_isStandAlone = true;
		}
		if (!_uiParams)
		{
			SetServerInfo();
		}
		else
		{
			_parameters["RECORDERNAME"] = _enteredRecServerName;
			_parameters["MANAGEMENTSERVERURI"] = _enteredManServer;
			_parameters["MEDIADBPATH"] = _enteredMediaDbPath;
		}
		if (_parameters.ContainsKey("RECORDERNAME"))
		{
			textBoxRecServerName.Text = _parameters["RECORDERNAME"];
		}
		if (_parameters.ContainsKey("MEDIADBPATH"))
		{
			textBoxMediaDB.Text = _parameters["MEDIADBPATH"];
		}
		if (_parameters.ContainsKey("TRECK_ID"))
		{
			_isFaiolver = _parameters["TRECK_ID"].ToLower(CultureInfo.CurrentUICulture) == "failover";
		}
		if (!_isStandAlone)
		{
			textBoxHostName.Text = Dns.GetHostEntry("").HostName;
			textBoxHostName.Enabled = false;
		}
		else
		{
			textBoxHostName.Text = _enteredManServer;
			_serverUrlLocked = parameters.TryGetValue("LOCKMANAGEMENTSERVER", out var value) && value == "true";
			textBoxHostName.Enabled = !_serverUrlLocked;
		}
		UpdateSpaceAvailable(textBoxMediaDB.Text);
		if (_parameters.ContainsKey("STORAGERETENTIONTIME") && decimal.TryParse(_parameters["STORAGERETENTIONTIME"], NumberStyles.Integer, CultureInfo.CurrentCulture, out var result))
		{
			retentionTimeUpDown.Value = result;
		}
		DisableControlsAndVisibility(parameters);
	}

	private void DisableControlsAndVisibility(Dictionary<string, string> parameters)
	{
		string valueFromRegistry = PluginUtilities.Instance.GetValueFromRegistry("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderName");
		if (IsUpgrade() && !PluginUtilities.Instance.IsRecorderInstalledInHWMM())
		{
			textBoxRecServerName.Enabled = false;
			textBoxMediaDB.Enabled = false;
			buttonBrowseDatebaseLocation.Enabled = false;
			labelRetentionTime.Visible = false;
			retentionTimeUpDown.Visible = false;
			labelDays.Visible = false;
		}
		else if (!string.IsNullOrEmpty(valueFromRegistry) || (parameters.ContainsKey("TRECK_ID") && parameters["TRECK_ID"].ToLower(CultureInfo.CurrentUICulture) == "failover"))
		{
			labelRetentionTime.Visible = false;
			retentionTimeUpDown.Visible = false;
			labelDays.Visible = false;
		}
	}

	public override bool OnBeforeContinue()
	{
		bool result;
		using (new ShowWaitCursor())
		{
			bool flag = ValidateInput();
			if (flag)
			{
				result = SupportsConfiguration();
				GlobalData.SetValue("SUPPORTSCONFIGURATION", result.ToString(), "ServerConfigurationControl.OnBeforeContinue()");
				try
				{
					bool flag2 = TryLoginToServer();
					if (flag2 && _isFaiolver && !IsSupportedByLicense())
					{
						MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ServerConfig_FailoverLicenseNotSupported, InstallerMessageBoxIcon.Exclamation);
						result = false;
					}
					else
					{
						result = flag2;
					}
				}
				catch (WebException ex)
				{
					MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, ex.Message, InstallerMessageBoxIcon.Exclamation);
					result = false;
				}
			}
			else
			{
				result = flag;
			}
		}
		return result;
	}

	private bool IsSupportedByLicense()
	{
		bool useHttps = _managementServerAddress.Scheme == Uri.UriSchemeHttps;
		Uri iDPAddress = _managementServerInfo.IDPAddress;
		using IdentityServerWindowsTokenHandler tokenHandler = new IdentityServerWindowsTokenHandler((NetworkCredential)CredentialCache.DefaultCredentials, new IdentityServerClientSettings(iDPAddress, "GrantValidatorClient", "windows_credentials"));
		using SimpleTokenCache tokenCache = new SimpleTokenCache(tokenHandler);
		using ServerConnection serverConnection = new ServerConnection(_managementServerAddress.Host, useHttps, tokenCache);
		serverConnection.ServerPort = _managementServerAddress.Port;
		using VmoClient vmoClient = new VmoClient(serverConnection);
		return vmoClient.ManagementServer.IsFeatureLicensed("RecordingServerFailover");
	}

	private static bool SupportsConfiguration()
	{
		if (IsTypicalInstallationMode())
		{
			if (!PluginUtilities.Instance.IsRecorderInstalledInHWMM())
			{
				if (!IsUpgrade())
				{
					return RecorderNeverInstalled();
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private static bool IsTypicalInstallationMode()
	{
		string value = null;
		if (GlobalData.TryGetValue("INSTALLATION_MODE", out value))
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value.Equals("Typical", StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
		return false;
	}

	private static bool RecorderNeverInstalled()
	{
		return string.IsNullOrEmpty(RegistryHelper.GetRegistryValue("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderName", RegistryView.Registry64));
	}

	private static bool IsUpgrade()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_UPGRADE"))
		{
			return InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true";
		}
		return false;
	}

	private bool TryLoginToServer()
	{
		if (!_isStandAlone)
		{
			return true;
		}
		Uri uri = new Uri(_parameters["MANAGEMENTSERVERURI"]);
		ManagementServerConnectionStatus managementServerConnectionStatus = ManagementServerConnectionStatus.NotConnected;
		try
		{
			try
			{
				if (new ServerHelper().TryGetManagementServerInfo(uri, out _managementServerInfo))
				{
					if (uri.Scheme != _managementServerInfo.IDPAddress.Scheme)
					{
						if (_managementServerInfo.IDPAddress.Scheme == Uri.UriSchemeHttp)
						{
							LogManager.Write("Change to usecure server address, server is running unsecure", TraceEventType.Warning);
							managementServerConnectionStatus = ManagementServerConnectionStatus.ServerRunningUnsecure;
						}
						else
						{
							LogManager.Write("Change to secure server address, server is running secure", TraceEventType.Warning);
							managementServerConnectionStatus = ManagementServerConnectionStatus.ServerRunningSecure;
						}
					}
					else
					{
						if (ServerHelper.TryLoginToCorporate(uri, _managementServerInfo.IDPAddress))
						{
							LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Login succeeded to server at {0}", uri.AbsoluteUri), TraceEventType.Information);
							_managementServerAddress = uri;
							return true;
						}
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Login failed to server at {0}", uri.AbsoluteUri), TraceEventType.Warning);
						managementServerConnectionStatus = ManagementServerConnectionStatus.LoginFailed;
					}
				}
				else
				{
					managementServerConnectionStatus = ManagementServerConnectionStatus.NotConnected;
				}
			}
			catch (Exception ex) when (ex is WebException || ex is SocketException)
			{
				managementServerConnectionStatus = ManagementServerConnectionStatus.NotConnected;
			}
			if (_isFaiolver)
			{
				MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_ConnectionFailed, InstallerMessageBoxIcon.Warning, InstallerMessageBoxButtons.OK);
				return false;
			}
			if (_isStandAlone)
			{
				switch (managementServerConnectionStatus)
				{
				case ManagementServerConnectionStatus.ServerRunningUnsecure:
					throw new UIFrameworkErrorException(VideoOS.Installer.UI.Properties.Resources.ErrorMessage_ServerIsRunningUnsecure, enablePreviousButtong: true);
				case ManagementServerConnectionStatus.ServerRunningSecure:
					throw new UIFrameworkErrorException(VideoOS.Installer.UI.Properties.Resources.ErrorMessage_ServerIsRunningSecure, enablePreviousButtong: true);
				case ManagementServerConnectionStatus.LoginFailed:
				case ManagementServerConnectionStatus.NotConnected:
					return HandleNotConnected();
				}
			}
		}
		catch (Exception ex2)
		{
			LogManager.Write(ex2.Message, TraceEventType.Warning);
			throw new UIFrameworkErrorException(ex2, enablePreviousButtong: true);
		}
		return false;
	}

	private static bool HandleNotConnected()
	{
		switch (MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.WarningMessage_ConnectionFailed, InstallerMessageBoxIcon.Warning, InstallerMessageBoxButtons.YesNo))
		{
		case DialogResult.No:
			LogManager.Write("HandleNotConnected(): Dialog result = No, do not continue and return to the ServerConfigurationDlg");
			return false;
		case DialogResult.Yes:
			GlobalData.SetValue("CONNECTEDTOSERVER", "false", "ServerConfigurationControl.HandleNotConnected()");
			return true;
		default:
			return false;
		}
	}

	public bool ValidateInput()
	{
		Uri validatedAddress = null;
		string text = null;
		string text2 = null;
		string text3 = null;
		try
		{
			text2 = textBoxHostName.Text;
			text = textBoxRecServerName.Text;
			text3 = textBoxMediaDB.Text;
			if (_isStandAlone)
			{
				if (string.IsNullOrEmpty(text2))
				{
					MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_ServerAddressEmpty, InstallerMessageBoxIcon.Exclamation);
					return false;
				}
				if (!HostHelper.ValidateAddress(text2, out validatedAddress))
				{
					LogManager.Write(string.Format(CultureInfo.CurrentCulture, "The Address: {0} to the Management Server is invalid", text2));
					MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, string.Format(CultureInfo.CurrentCulture, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_InvalidServerURL, OemResources.Instance.Server.ApplicationName), InstallerMessageBoxIcon.Exclamation);
					return false;
				}
				_parameters["MANAGEMENTSERVERURI"] = validatedAddress.ToString();
			}
		}
		catch (ArgumentNullException ex)
		{
			MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, ex.Message, InstallerMessageBoxIcon.Exclamation);
			return false;
		}
		if (string.IsNullOrEmpty(text))
		{
			MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_ServerNameEmpty, InstallerMessageBoxIcon.Exclamation);
			return false;
		}
		try
		{
			if (string.IsNullOrEmpty(text3))
			{
				MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_EmptyMediaDBPath, InstallerMessageBoxIcon.Exclamation);
				return false;
			}
			if (!IsValidPath(text3))
			{
				MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_InvalidMediaDBPath, InstallerMessageBoxIcon.Exclamation);
				return false;
			}
			if (Path.GetPathRoot(text3.Trim()).Length < 3 && Path.GetPathRoot(text3.Trim()) != "\\\\")
			{
				MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_InvalidMediaDBPath, InstallerMessageBoxIcon.Exclamation);
				return false;
			}
		}
		catch (ArgumentException ex2)
		{
			MessageHelper.ShowInstallerMessageBox(VideoOS.Installer.RecorderPlugins.Properties.Resources.ErrorMessage_Caption, ex2.Message, InstallerMessageBoxIcon.Exclamation);
			return false;
		}
		return true;
	}

	private static bool IsValidPath(string path)
	{
		if (!string.IsNullOrEmpty(path) && Regex.IsMatch(path, "^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$"))
		{
			return path.Length <= 90;
		}
		return false;
	}

	public void SetServerInfo()
	{
		GetRecorderName();
		GetServerUri();
		GetMediaDBFolder();
	}

	public void GetRecorderName()
	{
		string text = DefaultRecorderName;
		if (_parameters.ContainsKey("RECORDERNAME") && !string.IsNullOrEmpty(_parameters["RECORDERNAME"]))
		{
			text = _parameters["RECORDERNAME"];
		}
		if (string.Compare(text, DefaultRecorderName, StringComparison.OrdinalIgnoreCase) != 0)
		{
			LogManager.Write(string.Format(CultureInfo.CurrentCulture, "RecorderName was owerwritten from commandline/arguments.xml:{0}", text));
			return;
		}
		try
		{
			LogManager.Write("Getting recorder name from Registry");
			string valueFromRegistry = PluginUtilities.Instance.GetValueFromRegistry("SOFTWARE\\VideoOS\\Recorder\\Installation", "RecorderName");
			if (!string.IsNullOrEmpty(valueFromRegistry))
			{
				text = valueFromRegistry;
			}
			LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Setting recorder name from registry:{0}", text));
			_parameters["RECORDERNAME"] = text;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	public void GetMediaDBFolder()
	{
		try
		{
			string text = DefaultMediaDbPath;
			if (_parameters.ContainsKey("MEDIADBPATH") && !string.IsNullOrEmpty(_parameters["MEDIADBPATH"]))
			{
				text = _parameters["MEDIADBPATH"];
			}
			if (string.Compare(text, DefaultMediaDbPath, StringComparison.OrdinalIgnoreCase) != 0)
			{
				LogManager.Write(string.Format(CultureInfo.CurrentCulture, "MediaDbPath was owerwritten from commandline/arguments.xml:{0}", text));
				return;
			}
			try
			{
				LogManager.Write("Getting MediaDBFolder from Registry");
				string valueFromRegistry = PluginUtilities.Instance.GetValueFromRegistry("SOFTWARE\\VideoOS\\Recorder\\Installation", "MediaDBFolder");
				if (!string.IsNullOrEmpty(valueFromRegistry))
				{
					text = valueFromRegistry;
					LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Setting MediaDBFolder from Registry:{0}", text));
				}
			}
			catch (Exception ex)
			{
				LogManager.Write(string.Format(CultureInfo.CurrentCulture, "GetMediaDBFolder failed: {0}", ex.ToString()));
				return;
			}
			_parameters["MEDIADBPATH"] = text;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	public void GetServerUri()
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installing in {0}, setting {1} to 'localhost'", "HARDWARE-MANUFACTURER-MODE", "MANAGEMENTSERVERURI"));
			_parameters["MANAGEMENTSERVERURI"] = "localhost";
			return;
		}
		string text = null;
		if (_parameters.ContainsKey("MANAGEMENTSERVERURI") && HostHelper.ValidateAddress(_parameters["MANAGEMENTSERVERURI"], out var validatedAddress))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Found {0}={1} ", "MANAGEMENTSERVERURI", _parameters["MANAGEMENTSERVERURI"]));
			text = _parameters["MANAGEMENTSERVERURI"];
		}
		if (text == null && _parameters.ContainsKey("MANAGEMENTAPPNAME") && HostHelper.ValidateAddress(_parameters["MANAGEMENTAPPNAME"], out validatedAddress))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Found {0}={1} ", "MANAGEMENTAPPNAME", _parameters["MANAGEMENTAPPNAME"]));
			text = _parameters["MANAGEMENTAPPNAME"];
		}
		_enteredManServer = text;
	}

	private void buttonBrowseDatabaseLocation_Click(object sender, EventArgs e)
	{
		using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.SelectedPath = textBoxMediaDB.Text;
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			UpdateSpaceAvailable(folderBrowserDialog.SelectedPath);
			textBoxMediaDB.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void textBoxMediaDB_Leave(object sender, EventArgs e)
	{
		UpdateSpaceAvailable(textBoxMediaDB.Text);
	}

	private void UpdateSpaceAvailable(string _targetDir)
	{
		int num = 1024;
		if (CommonHelper.IsValidPath(_targetDir))
		{
			try
			{
				double num2 = InstallerManager.Instance.GetFreeSpaceBytes(_targetDir) / (double)(num * num);
				int digits = 1;
				string format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailable;
				double num3 = 2.0;
				if (num2 < (double)num)
				{
					format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailableMb;
					digits = 0;
					num3 = 1.0;
				}
				else if ((double)num <= num2 && num2 <= (double)(10 * num))
				{
					format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailable;
					digits = 2;
					num3 = num;
				}
				else if ((double)(10 * num) <= num2 && num2 <= (double)(num * num))
				{
					format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailable;
					digits = 0;
					num3 = num;
				}
				else if ((double)(num * num) <= num2 && num2 <= (double)(10 * num * num))
				{
					format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailableTb;
					digits = 2;
					num3 = Math.Pow(num, 2.0);
				}
				else if ((double)(10 * num * num) <= num2)
				{
					format = VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailableTb;
					digits = 0;
					num3 = Math.Pow(num, 2.0);
				}
				labelSpaceAvailable.Text = string.Format(CultureInfo.CurrentCulture, format, Math.Round(num2 / num3, digits).ToString(CultureInfo.CurrentCulture));
				return;
			}
			catch (Win32Exception)
			{
				labelSpaceAvailable.Text = string.Format(CultureInfo.CurrentCulture, VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailable, "-");
				return;
			}
		}
		labelSpaceAvailable.Text = string.Format(CultureInfo.CurrentCulture, VideoOS.Installer.RecorderPlugins.Properties.Resources.MediaDatabaseLocation_LabelSpaceAvailable, "-");
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.BackButton.Show();
		Wizard.BackButton.Enabled = true;
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = VideoOS.Installer.UI.Properties.Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
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
		this.textBoxRecServerName = new System.Windows.Forms.TextBox();
		this.labelRecServerName = new System.Windows.Forms.Label();
		this.labelMServerName = new System.Windows.Forms.Label();
		this.labelHeadLine = new System.Windows.Forms.Label();
		this.textBoxHostName = new System.Windows.Forms.TextBox();
		this.textBoxMediaDB = new System.Windows.Forms.TextBox();
		this.labelMediaDB = new System.Windows.Forms.Label();
		this.labelMServerExample = new System.Windows.Forms.Label();
		this.textBoxMediaDatabaseInfo = new System.Windows.Forms.Label();
		this.labelRetentionTime = new System.Windows.Forms.Label();
		this.retentionTimeUpDown = new System.Windows.Forms.NumericUpDown();
		this.labelDays = new System.Windows.Forms.Label();
		this.buttonBrowseDatebaseLocation = new System.Windows.Forms.Button();
		this.labelSpaceAvailable = new System.Windows.Forms.Label();
		this.labelMServerNameInfo = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelDays);
		base.WizardPage.Controls.Add(this.textBoxMediaDatabaseInfo);
		base.WizardPage.Controls.Add(this.textBoxMediaDB);
		base.WizardPage.Controls.Add(this.labelRetentionTime);
		base.WizardPage.Controls.Add(this.retentionTimeUpDown);
		base.WizardPage.Controls.Add(this.textBoxRecServerName);
		base.WizardPage.Controls.Add(this.labelRecServerName);
		base.WizardPage.Controls.Add(this.labelMServerName);
		base.WizardPage.Controls.Add(this.labelMServerNameInfo);
		base.WizardPage.Controls.Add(this.labelHeadLine);
		base.WizardPage.Controls.Add(this.textBoxHostName);
		base.WizardPage.Controls.Add(this.labelMediaDB);
		base.WizardPage.Controls.Add(this.labelMServerExample);
		base.WizardPage.Controls.Add(this.labelSpaceAvailable);
		base.WizardPage.Controls.Add(this.buttonBrowseDatebaseLocation);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("45984772-2c05-466c-adb8-fbca0067e62e");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.textBoxRecServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxRecServerName.Location = new System.Drawing.Point(43, 109);
		this.textBoxRecServerName.Name = "textBoxRecServerName";
		this.textBoxRecServerName.Size = new System.Drawing.Size(550, 22);
		this.textBoxRecServerName.TabIndex = 3;
		this.labelRecServerName.AutoSize = true;
		this.labelRecServerName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelRecServerName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelRecServerName.Location = new System.Drawing.Point(40, 92);
		this.labelRecServerName.Name = "labelRecServerName";
		this.labelRecServerName.Size = new System.Drawing.Size(136, 14);
		this.labelRecServerName.TabIndex = 2;
		this.labelRecServerName.Tag = "";
		this.labelRecServerName.Text = "&Recording server name:";
		this.labelMServerName.AutoSize = true;
		this.labelMServerName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelMServerName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMServerName.Location = new System.Drawing.Point(40, 156);
		this.labelMServerName.Name = "labelMServerName";
		this.labelMServerName.Size = new System.Drawing.Size(164, 14);
		this.labelMServerName.TabIndex = 4;
		this.labelMServerName.Tag = "";
		this.labelMServerName.Text = "&Management server address:";
		this.labelHeadLine.AutoSize = true;
		this.labelHeadLine.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadLine.Location = new System.Drawing.Point(38, 40);
		this.labelHeadLine.MaximumSize = new System.Drawing.Size(550, 60);
		this.labelHeadLine.Name = "labelHeadLine";
		this.labelHeadLine.Size = new System.Drawing.Size(327, 25);
		this.labelHeadLine.TabIndex = 0;
		this.labelHeadLine.Text = "Specify recording server settings";
		this.textBoxHostName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxHostName.Location = new System.Drawing.Point(43, 191);
		this.textBoxHostName.Name = "textBoxHostName";
		this.textBoxHostName.Size = new System.Drawing.Size(550, 22);
		this.textBoxHostName.TabIndex = 5;
		this.textBoxHostName.Text = "localhost";
		this.textBoxMediaDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxMediaDB.Location = new System.Drawing.Point(43, 313);
		this.textBoxMediaDB.Name = "textBoxMediaDB";
		this.textBoxMediaDB.Size = new System.Drawing.Size(414, 22);
		this.textBoxMediaDB.TabIndex = 7;
		this.textBoxMediaDB.Text = "C:\\MediaDatabase";
		this.textBoxMediaDB.Leave += new System.EventHandler(textBoxMediaDB_Leave);
		this.labelMediaDB.AutoSize = true;
		this.labelMediaDB.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelMediaDB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMediaDB.Location = new System.Drawing.Point(40, 261);
		this.labelMediaDB.Name = "labelMediaDB";
		this.labelMediaDB.Size = new System.Drawing.Size(207, 14);
		this.labelMediaDB.TabIndex = 6;
		this.labelMediaDB.Tag = "";
		this.labelMediaDB.Text = "Select your Media &database location:";
		this.labelMServerExample.AutoSize = true;
		this.labelMServerExample.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelMServerExample.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMServerExample.Location = new System.Drawing.Point(40, 217);
		this.labelMServerExample.MaximumSize = new System.Drawing.Size(553, 40);
		this.labelMServerExample.Name = "labelMServerExample";
		this.labelMServerExample.Size = new System.Drawing.Size(536, 14);
		this.labelMServerExample.TabIndex = 8;
		this.labelMServerExample.Tag = "";
		this.labelMServerExample.Text = "Examples: Localhost, Managementserver.domain.com, http://Managementserver.domain.com:81";
		this.textBoxMediaDatabaseInfo.BackColor = System.Drawing.Color.Transparent;
		this.textBoxMediaDatabaseInfo.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBoxMediaDatabaseInfo.ForeColor = System.Drawing.SystemColors.ControlText;
		this.textBoxMediaDatabaseInfo.Location = new System.Drawing.Point(40, 279);
		this.textBoxMediaDatabaseInfo.Margin = new System.Windows.Forms.Padding(0);
		this.textBoxMediaDatabaseInfo.Name = "textBoxMediaDatabaseInfo";
		this.textBoxMediaDatabaseInfo.Size = new System.Drawing.Size(553, 32);
		this.textBoxMediaDatabaseInfo.TabIndex = 10;
		this.textBoxMediaDatabaseInfo.Text = "It is recommended that you save your video recordings in a drive different from where you install the program.";
		this.labelRetentionTime.AutoSize = true;
		this.labelRetentionTime.BackColor = System.Drawing.Color.Transparent;
		this.labelRetentionTime.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelRetentionTime.Location = new System.Drawing.Point(40, 371);
		this.labelRetentionTime.Name = "labelRetentionTime";
		this.labelRetentionTime.Size = new System.Drawing.Size(172, 14);
		this.labelRetentionTime.TabIndex = 11;
		this.labelRetentionTime.Text = "Retention time for recordings:";
		this.retentionTimeUpDown.Location = new System.Drawing.Point(43, 390);
		this.retentionTimeUpDown.Maximum = new decimal(new int[4] { 365000, 0, 0, 0 });
		this.retentionTimeUpDown.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.retentionTimeUpDown.Name = "retentionTimeUpDown";
		this.retentionTimeUpDown.Size = new System.Drawing.Size(64, 20);
		this.retentionTimeUpDown.TabIndex = 12;
		this.retentionTimeUpDown.Value = new decimal(new int[4] { 7, 0, 0, 0 });
		this.labelDays.AutoSize = true;
		this.labelDays.BackColor = System.Drawing.Color.Transparent;
		this.labelDays.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelDays.Location = new System.Drawing.Point(112, 393);
		this.labelDays.Name = "labelDays";
		this.labelDays.Size = new System.Drawing.Size(31, 14);
		this.labelDays.TabIndex = 12;
		this.labelDays.Text = "days";
		this.buttonBrowseDatebaseLocation.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.buttonBrowseDatebaseLocation.Font = new System.Drawing.Font("Tahoma", 9f);
		this.buttonBrowseDatebaseLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.buttonBrowseDatebaseLocation.Location = new System.Drawing.Point(463, 313);
		this.buttonBrowseDatebaseLocation.Name = "buttonBrowseDatebaseLocation";
		this.buttonBrowseDatebaseLocation.Size = new System.Drawing.Size(130, 22);
		this.buttonBrowseDatebaseLocation.TabIndex = 8;
		this.buttonBrowseDatebaseLocation.Text = "&Browse...";
		this.buttonBrowseDatebaseLocation.UseVisualStyleBackColor = true;
		this.buttonBrowseDatebaseLocation.Click += new System.EventHandler(buttonBrowseDatabaseLocation_Click);
		this.labelSpaceAvailable.AutoSize = true;
		this.labelSpaceAvailable.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelSpaceAvailable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSpaceAvailable.Location = new System.Drawing.Point(40, 339);
		this.labelSpaceAvailable.Name = "labelSpaceAvailable";
		this.labelSpaceAvailable.Size = new System.Drawing.Size(182, 14);
		this.labelSpaceAvailable.TabIndex = 10;
		this.labelSpaceAvailable.Text = "Space available on drive: {0} GB";
		this.labelMServerNameInfo.AutoSize = true;
		this.labelMServerNameInfo.Font = new System.Drawing.Font("Tahoma", 9f);
		this.labelMServerNameInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMServerNameInfo.Location = new System.Drawing.Point(40, 174);
		this.labelMServerNameInfo.Name = "labelMServerNameInfo";
		this.labelMServerNameInfo.Size = new System.Drawing.Size(366, 14);
		this.labelMServerNameInfo.TabIndex = 5;
		this.labelMServerNameInfo.Tag = "";
		this.labelMServerNameInfo.Text = "Use HTTPS if the management server is configured to be secure.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ServerConfigurationControl";
		this.PageName = "RecorderConfigurationPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}

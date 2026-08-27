using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Server.Service.TrayController.Resources;
using VideoOS.Utilities.WinFormsExtensions;
using VideoOS.Vms.Types;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Service.TrayController;

public class TrayIconControl
{
	private AppMain _main;

	private NotifyIcon _notifyIcon;

	private ContextMenu _contextMenu;

	private MenuItem _runMenuItem;

	private MenuItem _stopMenuItem;

	private MenuItem _messageMenuItem;

	private MenuItem _aboutMenuItem;

	private MenuItem _changeEncryptionMenuItem;

	private MenuItem _changeLicenseItem;

	private MenuItem _exitMenuItem;

	private MenuItem _restoreMenuItem;

	private MenuItem _selectBackupFolderMenuItem;

	private MenuItem _helpMenuItem;

	private MenuItem _resetSystemRecoveryPassword;

	private MenuItem _enterSystemRecoveryPassword;

	private MenuItem _configureMsFailoverMenuItem;

	private Icon _indeterminateIcon;

	private Icon _runningIcon;

	private Icon _startingIcon;

	private Icon _stoppedIcon;

	private Icon _stoppingIcon;

	private Container _components = new Container();

	private ServiceState _serviceState;

	public event EventHandler StatusMessagesFormRequested;

	public TrayIconControl(AppMain main)
	{
		_main = main;
		_indeterminateIcon = AppMain.LoadIcon("ServerTrayIconIndeterminate");
		_runningIcon = AppMain.LoadIcon("ServerTrayIconRunning");
		_startingIcon = AppMain.LoadIcon("ServerTrayIconStarting");
		_stoppedIcon = AppMain.LoadIcon("ServerTrayIconStopped");
		_stoppingIcon = AppMain.LoadIcon("ServerTrayIconStopping");
		_runMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.StartService, Menu_Run_Click);
		_stopMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.StopService, Menu_Stop_Click);
		_messageMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.ShowStatusMessages, Menu_StatusMessages_Click);
		_resetSystemRecoveryPassword = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.ResetTheSystemRecoveryPassword, Menu_ChangeSystemRecoveryPassword_Click);
		_enterSystemRecoveryPassword = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.EnterTheSystemRecoveryPassword, Menu_EnterSystemRecoveryPassword_Click);
		_changeEncryptionMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.EncryptionConfiguration, Menu_ChangeEncryptionConfiguration_Click);
		_changeLicenseItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.ChangeLicense, Menu_ChangeLicense_Click);
		_restoreMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Restore, Menu_Restore_Click);
		_selectBackupFolderMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.SelectBackupFolder, Menu_SelectBackupFolder_Click);
		_helpMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Help, Menu_Help_Click);
		_aboutMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.About, Menu_About_Click);
		_exitMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Exit, Menu_Exit_Click);
		_configureMsFailoverMenuItem = new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.ConfigureMsFailover, Menu_Configure_Failover_Click);
		_contextMenu = new ContextMenu();
		_contextMenu.MenuItems.AddRange(new MenuItem[16]
		{
			_runMenuItem,
			_stopMenuItem,
			new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Separator),
			_messageMenuItem,
			new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Separator),
			_resetSystemRecoveryPassword,
			_enterSystemRecoveryPassword,
			new MenuItem(VideoOS.Server.Service.TrayController.Resources.Menu.Separator),
			_configureMsFailoverMenuItem,
			_changeEncryptionMenuItem,
			_changeLicenseItem,
			_restoreMenuItem,
			_selectBackupFolderMenuItem,
			_helpMenuItem,
			_aboutMenuItem,
			_exitMenuItem
		});
		_contextMenu.Popup += ContextMenu_Popup;
		_notifyIcon = new NotifyIcon(_components);
		_notifyIcon.Visible = false;
		_notifyIcon.ContextMenu = _contextMenu;
		_notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
		ServiceStopped();
		_notifyIcon.Visible = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifyIcon.Visible = false;
			if (_components != null)
			{
				_components.Dispose();
			}
		}
	}

	protected virtual void OnStatusMessagesFormRequested()
	{
		if (this.StatusMessagesFormRequested != null)
		{
			this.StatusMessagesFormRequested(this, null);
		}
	}

	private void NotifyIcon_DoubleClick(object sender, EventArgs e)
	{
		OnStatusMessagesFormRequested();
	}

	private void ContextMenu_Popup(object sender, EventArgs e)
	{
		switch (_serviceState)
		{
		case ServiceState.Running:
			_runMenuItem.Enabled = false;
			_stopMenuItem.Enabled = true;
			_enterSystemRecoveryPassword.Enabled = false;
			break;
		case ServiceState.Stopped:
			_runMenuItem.Enabled = true;
			_stopMenuItem.Enabled = false;
			_enterSystemRecoveryPassword.Enabled = true;
			break;
		case ServiceState.Indeterminate:
			_runMenuItem.Enabled = true;
			_stopMenuItem.Enabled = true;
			_enterSystemRecoveryPassword.Enabled = true;
			break;
		default:
			_runMenuItem.Enabled = false;
			_stopMenuItem.Enabled = false;
			_enterSystemRecoveryPassword.Enabled = false;
			break;
		}
	}

	private void Menu_StatusMessages_Click(object sender, EventArgs e)
	{
		OnStatusMessagesFormRequested();
	}

	public void ServiceStopped()
	{
		SetState(ServiceState.Stopped);
	}

	public void SetState(ServiceState serviceState)
	{
		string arg = "";
		_serviceState = serviceState;
		switch (_serviceState)
		{
		case ServiceState.Indeterminate:
			_notifyIcon.Icon = _indeterminateIcon;
			arg = Text.TrayIconServiceStatusIndeterminate;
			break;
		case ServiceState.Running:
			arg = Text.TrayIconServiceStatusRunning;
			_notifyIcon.Icon = _runningIcon;
			break;
		case ServiceState.Stopped:
			_notifyIcon.Icon = _stoppedIcon;
			arg = Text.TrayIconServiceStatusStopped;
			break;
		case ServiceState.StartPending:
			_notifyIcon.Icon = _startingIcon;
			arg = Text.TrayIconServiceStatusStartPending;
			break;
		case ServiceState.StopPending:
			_notifyIcon.Icon = _stoppingIcon;
			arg = Text.TrayIconServiceStatusStopPending;
			break;
		default:
			_notifyIcon.Icon = _stoppedIcon;
			break;
		}
		_notifyIcon.SetText(string.Format(CultureInfo.InvariantCulture, Text.TrayIconToolTipStatus, OemResources.Instance.Server.ApplicationName, arg));
	}

	public void UpdateState(ServiceState serviceState)
	{
		if (serviceState != _serviceState)
		{
			SetState(serviceState);
		}
	}

	private void Menu_Run_Click(object sender, EventArgs e)
	{
		AppMain.StartService();
	}

	private void Menu_Stop_Click(object sender, EventArgs e)
	{
		AppMain.StopService();
	}

	private void Menu_Exit_Click(object sender, EventArgs e)
	{
		_main.Exit();
	}

	private void Menu_ChangeEncryptionConfiguration_Click(object sender, EventArgs e)
	{
		AppMain.ChangeEncryptionConfiguration();
	}

	private void Menu_ChangeLicense_Click(object sender, EventArgs e)
	{
		AppMain.ChangeLicense();
	}

	private void Menu_Restore_Click(object sender, EventArgs e)
	{
		AppMain.Restore();
	}

	private void Menu_SelectBackupFolder_Click(object sender, EventArgs e)
	{
		AppMain.CreateSharedBackupFolder();
	}

	private void Menu_ChangeSystemRecoveryPassword_Click(object sender, EventArgs e)
	{
		AppMain.ChangeSystemRecoveryPassword();
	}

	private void Menu_EnterSystemRecoveryPassword_Click(object sender, EventArgs e)
	{
		AppMain.SetSystemRecoveryPassword();
	}

	private void Menu_Configure_Failover_Click(object sender, EventArgs e)
	{
		AppMain.ConfigureManagementServerFailover();
	}

	private void Menu_Help_Click(object sender, EventArgs e)
	{
		AppMain.ShowHelp();
	}

	private void Menu_About_Click(object sender, EventArgs e)
	{
		AppMain.ShowAbout();
	}
}

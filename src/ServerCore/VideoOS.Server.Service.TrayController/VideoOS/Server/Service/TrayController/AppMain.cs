using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Server.UACParts;
using VideoOS.Common.Controls;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Diagnostics.Logging;
using VideoOS.Server.Service.TrayController.Properties;
using VideoOS.Server.Service.TrayController.Resources;
using VideoOS.TrayControllerControls;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Service.TrayController;

public class AppMain : IQuit
{
	private static string _eventHandlerName = "Global\\VideoOS.Server.Service.TrayController";

	private static TrayEventHandlerService _eventHandler;

	private readonly int _port;

	private readonly TimeSpan _pollPeriod = TimeSpan.FromMilliseconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("pollinterval"), CultureInfo.InvariantCulture));

	private TrayIconControl _trayIconControl;

	private ServerObserver _sObserver;

	private volatile StatusMessagesForm _statusMessagesForm;

	private object _statusMessagesFormLock = new object();

	private ServerConfig _serverConfig;

	public string CultureName { get; private set; }

	public static void ShowAbout()
	{
		using AboutForm aboutForm = new AboutForm();
		aboutForm.ShowDialog();
	}

	public static void ShowHelp()
	{
		CorporateHelp.Instance.ShowHelpTopic("ServerTrayHelp");
	}

	public static Icon LoadIcon(Assembly assembly, string name)
	{
		string path = "VideoOS.Server.Service.TrayController";
		return ResourceHelper.LoadIcon(assembly, path, name);
	}

	public static Icon LoadIcon(string name)
	{
		Assembly assembly = typeof(AppMain).Assembly;
		return LoadIcon(assembly, name);
	}

	public static Assembly GetMainAssembly()
	{
		return typeof(AppMain).Assembly;
	}

	[STAThread]
	private static void Main(string[] args)
	{
		_eventHandler = new TrayEventHandlerService(_eventHandlerName);
		if (args.Length != 0 && string.Compare(args[0], "-quit", ignoreCase: true) == 0)
		{
			_eventHandler.TrySignalEvent();
			return;
		}
		if (_eventHandler.IsAlreadyRunning(GetMainAssembly()))
		{
			MessageBox.Show(Text.MsgBoxOnlyOneInstanseAllowedPerUser, Text.MsgBoxOnlyOneInstanceAllowedCaption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			return;
		}
		_eventHandler.EnsureCreateEventCreated();
		Start();
		Application.EnableVisualStyles();
		Application.Run();
	}

	public void QuitSignalled()
	{
		Exit();
	}

	public AppMain()
	{
		_eventHandler.StartSignalMonitoring(this);
		_port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("traycontrollerport"));
		_pollPeriod = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("pollinterval")));
		CultureName = "en-US";
		_serverConfig = new ServerConfig(new FileService(), null);
		LogManager.ModifyLogParameter += LogManager_ModifyLogParameter;
		CorporateHelp.Instance.Initialize(_serverConfig.VmoServerName, _serverConfig.VmoServerPort, CultureName, IntPtr.Zero);
		_statusMessagesForm = new StatusMessagesForm();
		_statusMessagesForm.Icon = LoadIcon("Server");
		_statusMessagesForm.Text = VideoOS.Server.Service.TrayController.Properties.Resources.StatusMessagesFormTitle;
		_statusMessagesForm.Hide();
		_sObserver = new ServerObserver(_port);
		_sObserver.PollPeriod = _pollPeriod;
		_sObserver.ServerStateUpdated += OnServerStateUpdate;
		_sObserver.ConnectionLost += OnConnectionLost;
		_sObserver.NewMessagesReceived += OnNewMessagesReceived;
		_sObserver.Start();
		_trayIconControl = new TrayIconControl(this);
		_trayIconControl.StatusMessagesFormRequested += OnStatusMessagesFormRequested;
	}

	public static void StartService()
	{
		RunUACParts("START");
	}

	public static void StopService()
	{
		RunUACParts("STOP");
	}

	public static void ChangeEncryptionConfiguration()
	{
		RunUACParts("CHANGEENCRYPTIONCONFIGURATION");
	}

	public static void ChangeLicense()
	{
		RunUACParts("CHANGELICENSE");
	}

	public static void CreateSharedBackupFolder()
	{
		RunUACParts("RESTOREBACKUPFOLDER");
	}

	public static void Restore()
	{
		RunUACParts("RESTORE");
	}

	public static void SetSystemRecoveryPassword()
	{
		RunUACParts("SETSYSTEMRECOVERYPASSWORD");
	}

	public static void ChangeSystemRecoveryPassword()
	{
		RunUACParts("CHANGESYSTEMRECOVERYPASSWORD");
	}

	public static void ConfigureManagementServerFailover()
	{
		RunUACParts("CONFIGUREMANAGEMENTSERVERFAILOVER");
	}

	public static void RunUACParts(string commandName)
	{
		Process process = new Process();
		process.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\Server.UACParts.exe";
		process.StartInfo.Arguments = commandName;
		try
		{
			process.Start();
		}
		catch (Win32Exception ex)
		{
			if (ex.NativeErrorCode != 1223)
			{
				string msgBoxUacErrorCaption = Text.MsgBoxUacErrorCaption;
				MessageBox.Show(ex.Message, msgBoxUacErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
	}

	private static AppMain Start()
	{
		AppMain appMain = new AppMain();
		Application.ApplicationExit += appMain.Application_ApplicationExit;
		return appMain;
	}

	public void Exit()
	{
		Application.Exit();
	}

	private void Application_ApplicationExit(object sender, EventArgs e)
	{
		Dispose();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_sObserver.Dispose();
			_trayIconControl.Dispose();
			lock (_statusMessagesFormLock)
			{
				_statusMessagesForm?.CloseForm();
				_statusMessagesForm = null;
			}
		}
	}

	private void LogManager_ModifyLogParameter(object sender, LogManager.LogInitializeParameterEventArgs e)
	{
		if (e.LogType == "File" && (e.Key == "Filename" || e.Key == "Archive"))
		{
			string text = (string)e.Value2;
			text = text.Replace("~", "").Replace("/", "\\");
			if (Path.IsPathRooted(text))
			{
				text = text.TrimStart('\\');
			}
			e.Value = Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, text);
		}
	}

	private void OnServerStateUpdate(object sender, ServerObserver.ServerStateUpdatedEventArgs e)
	{
		_trayIconControl?.UpdateState(e.State);
	}

	private void OnConnectionLost(object sender, EventArgs e)
	{
		_trayIconControl?.ServiceStopped();
	}

	private void OnNewMessagesReceived(object sender, ServerObserver.ServerMessagesReceivedEventArgs e)
	{
		_statusMessagesForm.ReceiveMessages(e.Messages, e.InSession);
	}

	private void OnStatusMessagesFormRequested(object sender, EventArgs e)
	{
		_statusMessagesForm.WindowState = FormWindowState.Normal;
		_statusMessagesForm.Show();
		_statusMessagesForm.BringToFront();
		_statusMessagesForm.Activate();
	}
}

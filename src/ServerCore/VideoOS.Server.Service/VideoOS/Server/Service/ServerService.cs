#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using VideoOS.Diagnostics.Logging;
using VideoOS.Server.Service.Properties;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Service;
using VideoOS.Vms.Types;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Server.Service;

public class ServerService : ServiceBase
{
	private ManualResetEvent _serverThreadStopEvent = new ManualResetEvent(initialState: false);

	private Thread _serverThread;

	private gRPCStateProvider _grpcstateProvider;

	private bool _runAsWindow;

	private MainForm _mainForm;

	private int? _vmoPort;

	private Container components;

	private int _shutdownGate;

	public ServerServiceState State { get; private set; }

	public ServerService()
	{
		InitializeComponent();
	}

	private static void Main(string[] args)
	{
		LogManager.ModifyLogParameter += LogManager_ModifyLogParameter;
		try
		{
			string[] directories = Directory.GetDirectories(OemResources.Instance.Server.CommonApplicationDataFolder, "dump*", SearchOption.TopDirectoryOnly);
			string[] array = directories;
			foreach (string path in array)
			{
				try
				{
					Directory.Delete(path, recursive: true);
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}
		catch (IOException)
		{
		}
		ServerService serverService = new ServerService();
		bool flag = false;
		if (args.Length != 0)
		{
			foreach (string text in args)
			{
				if (text.IndexOf("-window", StringComparison.CurrentCultureIgnoreCase) != -1 || text.IndexOf("-x", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					serverService._runAsWindow = true;
				}
				else if (text.IndexOf("-Y", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					flag = true;
				}
				else if (text.IndexOf("-VMOPort:", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					serverService._vmoPort = Convert.ToInt32(text.Substring("-VMOPort:".Length), CultureInfo.InvariantCulture);
				}
			}
		}
		if (serverService._runAsWindow)
		{
			if (!flag || DialogResult.OK == MessageBox.Show("Continue starting up?", "Startup Options", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
			{
				serverService._mainForm = new MainForm(serverService);
				serverService._mainForm.ShowDialog();
			}
		}
		else
		{
			ServiceBase[] services = new ServiceBase[1] { serverService };
			ServiceBase.Run(services);
		}
	}

	private void SetServiceRecoveryOptions(bool enableRecovery)
	{
		if (!_runAsWindow)
		{
			ServiceControllerPermission serviceControllerPermission = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, ".", OemResources.Instance.Server.ServiceName);
			try
			{
				serviceControllerPermission.Demand();
			}
			catch (SecurityException)
			{
				return;
			}
			ServiceInstallerEx serviceInstallerEx = new ServiceInstallerEx();
			serviceInstallerEx.FailCountResetTime = 600u;
			if (enableRecovery)
			{
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.None, 60000));
			}
			else
			{
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.None, 60000));
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.None, 60000));
				serviceInstallerEx.FailureActions.Add(new FailureAction(RecoverAction.None, 60000));
			}
			serviceInstallerEx.UpdateServiceConfig();
			serviceInstallerEx.Dispose();
		}
	}

	public void StartFromWindow()
	{
		try
		{
			OnStart(null);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
			Process.GetCurrentProcess().Kill();
		}
	}

	public void StopFromWindow()
	{
		try
		{
			OnStop();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
			Process.GetCurrentProcess().Kill();
		}
	}

	private void InitializeComponent()
	{
		base.CanShutdown = true;
		base.ServiceName = OemResources.Instance.Server.ServiceName;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	protected override void OnStart(string[] args)
	{
		try
		{
			SetServiceRecoveryOptions(enableRecovery: true);
			State = new ServerServiceState();
			ChangeStatus(ServiceState.StartPending);
			LogStatusMessage(Resources.StatusServerStarting);
			try
			{
				if (_vmoPort.HasValue)
				{
					ServerAccessor.ServerConfig.Initialize(_vmoPort.Value);
				}
				else
				{
					ServerAccessor.ServerConfig.Initialize();
				}
			}
			catch (ServerConfigInvalidException ex)
			{
				ChangeStatus(ServiceState.Stopped);
				LogStatusMessage(Resources.StatusFailedToReadConfig);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Resources.StatusFailedToReadConfig);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Message:");
				stringBuilder.AppendLine(ex.Message);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Exception:");
				stringBuilder.AppendLine(ex.ToString());
				AddEventLogEntry(stringBuilder.ToString(), EventLogEntryType.Error);
				throw new Exception(Resources.StatusFailedToReadConfig, ex);
			}
			try
			{
				_grpcstateProvider = new gRPCStateProvider(State, ServerAccessor.ServerConfig.TrayControllerPort);
				_grpcstateProvider.Start();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				string message = string.Empty;
				if (ex3 is SocketException)
				{
					SocketException ex4 = ex3 as SocketException;
					if (ex4.SocketErrorCode == SocketError.AddressAlreadyInUse)
					{
						message = string.Format(CultureInfo.InvariantCulture, Resources.StatusInformationProviderPortFailed, ServerAccessor.ServerConfig.TrayControllerPort);
					}
				}
				else
				{
					message = string.Format(CultureInfo.InvariantCulture, Resources.StatusInformationProviderFailed, ex3.Message);
				}
				throw new Exception(message);
			}
			ServerAccessor.Instance.StatusMessage += AddStatusMessage;
			ServerAccessor.Instance.ShutdownRequested += OnShutdownRequested;
			_serverThreadStopEvent.Reset();
			_serverThread = new Thread(ServerThreadFunction);
			_serverThread.Start();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex6)
		{
			ChangeStatus(ServiceState.Stopped);
			string message2 = string.Format(CultureInfo.InvariantCulture, Resources.StatusTerminateFatal, ex6.Message);
			LogStatusMessage(message2);
			Trace.WriteLine(ex6.ToString());
			AddEventLogEntry(ex6.ToString(), EventLogEntryType.Error);
			Thread.Sleep(1000);
			throw;
		}
	}

	private void OnShutdownRequested(object sender, ShutdownRequestedEventArgs e)
	{
		if (Interlocked.Exchange(ref _shutdownGate, 1) == 0)
		{
			LogStatusMessage(e.Message);
			Stop();
		}
	}

	protected override void OnStop()
	{
		ChangeStatus(ServiceState.StopPending);
		try
		{
			if (_serverThread != null && _serverThread.IsAlive)
			{
				_serverThreadStopEvent.Set();
				if (!_serverThread.Join(30000))
				{
					LogStatusMessage(Resources.StatusShutdownKill);
					AddEventLogEntry(Resources.StatusShutdownKill, EventLogEntryType.Error);
					SetServiceRecoveryOptions(enableRecovery: false);
					Thread.Sleep(1000);
					Process.GetCurrentProcess().Kill();
				}
			}
			_serverThreadStopEvent.Reset();
			_serverThread = null;
			_grpcstateProvider?.Stop();
			ServerAccessor.Instance.StatusMessage -= AddStatusMessage;
			ServerAccessor.Instance.ShutdownRequested -= OnShutdownRequested;
			LogManager.CloseAllLoggers();
		}
		finally
		{
			ChangeStatus(ServiceState.Stopped);
		}
	}

	protected override void OnShutdown()
	{
		OnStop();
		base.OnShutdown();
	}

	private void ServerThreadFunction()
	{
		try
		{
			ServerAccessor.Instance.Initialize();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			ServerAccessor.Instance.Logger.LogError("Server initialization failed, ex: {0}", ex2);
			LogStatusMessage(string.Format(Resources.StatusErrorDuringInitialization, ex2.Message));
			LogStatusMessage(Resources.StatusForcingShutdown);
			_serverThread = null;
			try
			{
				Exception ex3 = ex2;
				bool skipDatabaseLog = false;
				while (ex3 != null)
				{
					if (ex3 is KeyStorageException)
					{
						skipDatabaseLog = true;
						break;
					}
					ex3 = ex3.InnerException;
				}
				ServerAccessor.Instance.Close(skipDatabaseLog);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (_runAsWindow)
			{
				_mainForm.Invoke(new MethodInvoker(_mainForm.Close));
			}
			else
			{
				Stop();
			}
			Thread.Sleep(1000);
			return;
		}
		try
		{
			ChangeStatus(ServiceState.Running);
			_serverThreadStopEvent.WaitOne();
			ServerAccessor.Instance.Close(skipDatabaseLog: false);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex6)
		{
			Trace.WriteLine(ex6.ToString());
		}
		finally
		{
			ChangeStatus(ServiceState.Stopped);
		}
	}

	private void AddStatusMessage(object sender, StatusMessageEventArgs e)
	{
		State.LogStatusMessage(e.Timestamp.ToLocalTime(), e.Message);
	}

	private void LogStatusMessage(string message)
	{
		State.LogStatusMessage(DateTime.Now, message);
	}

	public static void AddEventLogEntry(string message, EventLogEntryType logType)
	{
		try
		{
			EventLog.WriteEntry(OemResources.Instance.Server.ServiceName, message, logType);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void ChangeStatus(ServiceState status)
	{
		State.ChangeStatus(status);
	}

	private static void LogManager_ModifyLogParameter(object sender, LogManager.LogInitializeParameterEventArgs e)
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
}

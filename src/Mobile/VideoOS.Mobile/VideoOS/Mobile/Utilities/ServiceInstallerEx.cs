using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace VideoOS.Mobile.Utilities;

public class ServiceInstallerEx : ServiceInstaller
{
	public struct SERVICE_DESCRIPTION
	{
		public string lpDescription;
	}

	public struct SERVICE_FAILURE_ACTIONS
	{
		public int dwResetPeriod;

		public string lpRebootMsg;

		public string lpCommand;

		public int cActions;

		public int lpsaActions;
	}

	public struct LUID_AND_ATTRIBUTES
	{
		public long Luid;

		public int Attributes;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TOKEN_PRIVILEGES
	{
		public int PrivilegeCount;

		public LUID_AND_ATTRIBUTES Privileges;
	}

	private const int SC_MANAGER_ALL_ACCESS = 983103;

	private const int SERVICE_ALL_ACCESS = 983551;

	private const int SERVICE_CONFIG_DESCRIPTION = 1;

	private const int SERVICE_CONFIG_FAILURE_ACTIONS = 2;

	private const int SERVICE_NO_CHANGE = -1;

	private const int ERROR_ACCESS_DENIED = 5;

	private const int TOKEN_ADJUST_PRIVILEGES = 32;

	private const int TOKEN_QUERY = 8;

	private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

	private const int SE_PRIVILEGE_ENABLED = 2;

	private string description = "";

	private int failResetTime = -1;

	private string failRebootMsg = "";

	private string failRunCommand = "";

	private bool setDescription;

	private bool setFailActions;

	private bool startOnInstall;

	private int startTimeout = 15000;

	private string logMsgBase;

	public ArrayList FailureActions;

	public new string Description
	{
		set
		{
			description = value;
			setDescription = true;
		}
	}

	public int FailCountResetTime
	{
		set
		{
			failResetTime = value;
			setFailActions = true;
		}
	}

	public string FailRebootMsg
	{
		set
		{
			failRebootMsg = value;
			setFailActions = true;
		}
	}

	public string FailRunCommand
	{
		set
		{
			failRunCommand = value;
			setFailActions = true;
		}
	}

	public bool StartOnInstall
	{
		set
		{
			startOnInstall = value;
		}
	}

	public int StartTimeout
	{
		set
		{
			startTimeout = value;
		}
	}

	[DllImport("advapi32.dll")]
	public static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, int dwDesiredAccess);

	[DllImport("advapi32.dll")]
	public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, int dwDesiredAccess);

	[DllImport("advapi32.dll")]
	public static extern IntPtr LockServiceDatabase(IntPtr hSCManager);

	[DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
	public static extern bool ChangeServiceFailureActions(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS lpInfo);

	[DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
	public static extern bool ChangeServiceDescription(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_DESCRIPTION lpInfo);

	[DllImport("advapi32.dll")]
	public static extern bool CloseServiceHandle(IntPtr hSCObject);

	[DllImport("advapi32.dll")]
	public static extern bool UnlockServiceDatabase(IntPtr hSCManager);

	[DllImport("kernel32.dll")]
	public static extern int GetLastError();

	[DllImport("advapi32.dll")]
	public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, [MarshalAs(UnmanagedType.Struct)] ref TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, ref int ReturnLength);

	[DllImport("advapi32.dll")]
	public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref long lpLuid);

	[DllImport("advapi32.dll")]
	public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

	[DllImport("kernel32.dll")]
	public static extern IntPtr GetCurrentProcess();

	[DllImport("kernel32.dll")]
	public static extern bool CloseHandle(IntPtr hndl);

	public ServiceInstallerEx()
	{
		FailureActions = new ArrayList();
		base.Committed += UpdateServiceConfig;
		base.Committed += StartIfNeeded;
		logMsgBase = "ServiceInstallerEx : " + base.ServiceName + " : ";
	}

	private void LogInstallMessage(EventLogEntryType logLevel, string msg)
	{
		Console.WriteLine(msg);
		try
		{
			EventLog.WriteEntry(base.ServiceName, msg, logLevel);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	private bool GrandShutdownPrivilege()
	{
		bool result = false;
		IntPtr TokenHandle = IntPtr.Zero;
		_ = IntPtr.Zero;
		TOKEN_PRIVILEGES NewState = default(TOKEN_PRIVILEGES);
		long lpLuid = 0L;
		int ReturnLength = 0;
		try
		{
			if (!OpenProcessToken(GetCurrentProcess(), 40, ref TokenHandle))
			{
				return result;
			}
			LookupPrivilegeValue(null, "SeShutdownPrivilege", ref lpLuid);
			NewState.PrivilegeCount = 1;
			NewState.Privileges.Luid = lpLuid;
			NewState.Privileges.Attributes = 2;
			AdjustTokenPrivileges(TokenHandle, DisableAllPrivileges: false, ref NewState, 0, IntPtr.Zero, ref ReturnLength);
			if (GetLastError() != 0)
			{
				throw new Exception("Failed to grant shutdown privilege");
			}
			result = true;
		}
		catch (Exception ex)
		{
			LogInstallMessage(EventLogEntryType.Error, logMsgBase + ex.Message);
		}
		finally
		{
			if (TokenHandle != IntPtr.Zero)
			{
				CloseHandle(TokenHandle);
			}
		}
		return result;
	}

	private void UpdateServiceConfig(object sender, InstallEventArgs e)
	{
		setFailActions = false;
		int count = FailureActions.Count;
		if (count > 0)
		{
			setFailActions = true;
		}
		if (!setDescription && !setFailActions)
		{
			return;
		}
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		IntPtr intPtr3 = IntPtr.Zero;
		IntPtr intPtr4 = IntPtr.Zero;
		try
		{
			intPtr = OpenSCManager(null, null, 983103);
			if (intPtr.ToInt32() <= 0)
			{
				LogInstallMessage(EventLogEntryType.Error, logMsgBase + "Failed to Open Service Control Manager");
				return;
			}
			intPtr4 = LockServiceDatabase(intPtr);
			if (intPtr4.ToInt32() <= 0)
			{
				LogInstallMessage(EventLogEntryType.Error, logMsgBase + "Failed to Lock Service Database for Write");
				return;
			}
			intPtr2 = OpenService(intPtr, base.ServiceName, 983551);
			if (intPtr2.ToInt32() <= 0)
			{
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Failed to Open Service ");
				return;
			}
			if (setFailActions)
			{
				int[] array = new int[count * 2];
				int num = 0;
				bool flag = false;
				foreach (FailureAction failureAction in FailureActions)
				{
					array[num] = (int)failureAction.Type;
					array[++num] = failureAction.Delay;
					num++;
					if (failureAction.Type == RecoverAction.Reboot)
					{
						flag = true;
					}
				}
				if (flag && !GrandShutdownPrivilege())
				{
					return;
				}
				intPtr3 = Marshal.AllocHGlobal(count * 8);
				Marshal.Copy(array, 0, intPtr3, count * 2);
				SERVICE_FAILURE_ACTIONS lpInfo = new SERVICE_FAILURE_ACTIONS
				{
					cActions = count,
					dwResetPeriod = failResetTime,
					lpCommand = failRunCommand,
					lpRebootMsg = failRebootMsg,
					lpsaActions = intPtr3.ToInt32()
				};
				if (!ChangeServiceFailureActions(intPtr2, 2, ref lpInfo) && GetLastError() == 5)
				{
					throw new Exception(logMsgBase + "Access Denied while setting Failure Actions");
				}
				Marshal.FreeHGlobal(intPtr3);
				intPtr3 = IntPtr.Zero;
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Successfully configured Failure Actions");
			}
			if (setDescription)
			{
				SERVICE_DESCRIPTION lpInfo2 = new SERVICE_DESCRIPTION
				{
					lpDescription = description
				};
				if (!ChangeServiceDescription(intPtr2, 1, ref lpInfo2))
				{
					throw new Exception(logMsgBase + "Failed to set description");
				}
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Successfully set description");
			}
		}
		catch (Exception ex)
		{
			LogInstallMessage(EventLogEntryType.Error, ex.Message);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				if (intPtr4 != IntPtr.Zero)
				{
					UnlockServiceDatabase(intPtr4);
					intPtr4 = IntPtr.Zero;
				}
				CloseServiceHandle(intPtr);
				intPtr = IntPtr.Zero;
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CloseServiceHandle(intPtr2);
				intPtr2 = IntPtr.Zero;
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
				intPtr3 = IntPtr.Zero;
			}
		}
	}

	private void StartIfNeeded(object sender, InstallEventArgs e)
	{
		if (!startOnInstall)
		{
			return;
		}
		try
		{
			TimeSpan timeout = new TimeSpan(0, 0, startTimeout);
			ServiceController serviceController = new ServiceController(base.ServiceName);
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			serviceController.Close();
			LogInstallMessage(EventLogEntryType.Information, logMsgBase + " Service Started");
		}
		catch (Exception ex)
		{
			LogInstallMessage(EventLogEntryType.Error, logMsgBase + ex.Message);
		}
	}
}

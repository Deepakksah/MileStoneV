using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace VideoOS.Utilities.Service;

public class ServiceInstallerEx : ServiceInstaller
{
	public struct SERVICE_DESCRIPTION
	{
		public string lpDescription;
	}

	public struct SERVICE_FAILURE_ACTIONS
	{
		[MarshalAs(UnmanagedType.U4)]
		public uint dwResetPeriod;

		[MarshalAs(UnmanagedType.LPStr)]
		public string lpRebootMsg;

		[MarshalAs(UnmanagedType.LPStr)]
		public string lpCommand;

		[MarshalAs(UnmanagedType.U4)]
		public uint cActions;

		public IntPtr lpsaActions;
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

	private const uint SC_MANAGER_ALL_ACCESS = 983103u;

	private const uint SERVICE_CONFIG_DESCRIPTION = 1u;

	private const uint SERVICE_CONFIG_FAILURE_ACTIONS = 2u;

	private const uint ERROR_ACCESS_DENIED = 5u;

	private const uint SERVICE_NO_CHANGE = uint.MaxValue;

	private const uint SERVICE_QUERY_CONFIG = 1u;

	private const uint SERVICE_CHANGE_CONFIG = 2u;

	private const uint SERVICE_QUERY_STATUS = 4u;

	private const uint SERVICE_ENUMERATE_DEPENDENTS = 8u;

	private const uint SERVICE_START = 16u;

	private const uint SERVICE_STOP = 32u;

	private const uint SERVICE_PAUSE_CONTINUE = 64u;

	private const uint SERVICE_INTERROGATE = 128u;

	private const uint SERVICE_USER_DEFINED_CONTROL = 256u;

	private const uint STANDARD_RIGHTS_REQUIRED = 983040u;

	private const uint SERVICE_ALL_ACCESS = 983551u;

	private const int TOKEN_ADJUST_PRIVILEGES = 32;

	private const int TOKEN_QUERY = 8;

	private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

	private const int SE_PRIVILEGE_ENABLED = 2;

	private string description = "";

	private uint failResetTime = uint.MaxValue;

	private string failRebootMsg = "";

	private string failRunCommand = "";

	private bool setDescription;

	private bool setFailActions;

	private bool startOnInstall;

	private string displayName = "";

	private string path = "";

	private bool setPath;

	private bool setDisplayName;

	private int startTimeout = 15000;

	private string logMsgBase;

	public List<FailureAction> FailureActions;

	public new string Description
	{
		set
		{
			description = value;
			setDescription = true;
		}
	}

	public uint FailCountResetTime
	{
		set
		{
			failResetTime = value;
			setFailActions = true;
		}
	}

	public string Path
	{
		get
		{
			return path;
		}
		set
		{
			path = value;
			setPath = true;
		}
	}

	public new string DisplayName
	{
		set
		{
			displayName = value;
			setDisplayName = true;
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

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool ChangeServiceConfig(IntPtr hService, uint nServiceType, uint nStartType, uint nErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword, string lpDisplayName);

	[DllImport("advapi32.dll")]
	public static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, uint dwDesiredAccess);

	[DllImport("advapi32.dll")]
	public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

	[DllImport("advapi32.dll")]
	public static extern IntPtr LockServiceDatabase(IntPtr hSCManager);

	[DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
	public static extern bool ChangeServiceFailureActions(IntPtr hService, uint dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS lpInfo);

	[DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
	public static extern bool ChangeServiceDescription(IntPtr hService, uint dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_DESCRIPTION lpInfo);

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
		FailureActions = new List<FailureAction>();
		base.Committed += UpdateServiceConfig;
		base.Committed += StartIfNeeded;
		logMsgBase = "ServiceInstallerEx : " + base.ServiceName + " : ";
	}

	private static void LogInstallMessage(EventLogEntryType logLevel, string msg)
	{
		Console.WriteLine(msg);
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
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LogInstallMessage(EventLogEntryType.Error, logMsgBase + ex2.Message);
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

	public void UpdateServiceConfig()
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
			intPtr = OpenSCManager(null, null, 983103u);
			if (intPtr.ToInt64() <= 0)
			{
				LogInstallMessage(EventLogEntryType.Error, logMsgBase + "Failed to Open Service Control Manager");
				return;
			}
			intPtr4 = LockServiceDatabase(intPtr);
			if (intPtr4.ToInt64() <= 0)
			{
				int lastError = GetLastError();
				LogInstallMessage(EventLogEntryType.Error, logMsgBase + "Failed to Lock Service Database for Write:" + lastError.ToString(CultureInfo.InvariantCulture));
				return;
			}
			intPtr2 = OpenService(intPtr, base.ServiceName, 983551u);
			if (intPtr2.ToInt64() <= 0)
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
					cActions = (uint)count,
					dwResetPeriod = failResetTime,
					lpCommand = failRunCommand,
					lpRebootMsg = failRebootMsg,
					lpsaActions = intPtr3
				};
				if (!ChangeServiceFailureActions(intPtr2, 2u, ref lpInfo) && (long)GetLastError() == 5)
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
				if (!ChangeServiceDescription(intPtr2, 1u, ref lpInfo2))
				{
					throw new Exception(logMsgBase + "Failed to set description");
				}
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Successfully set description");
			}
			if (setDisplayName)
			{
				if (!ChangeServiceConfig(intPtr2, uint.MaxValue, uint.MaxValue, uint.MaxValue, null, null, IntPtr.Zero, null, null, null, displayName))
				{
					throw new Exception(logMsgBase + "Failed to set display name");
				}
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Successfully set display name");
			}
			if (setPath)
			{
				if (!ChangeServiceConfig(intPtr2, uint.MaxValue, uint.MaxValue, uint.MaxValue, path, null, IntPtr.Zero, null, null, null, null))
				{
					throw new Exception(logMsgBase + "Failed to set path");
				}
				LogInstallMessage(EventLogEntryType.Information, logMsgBase + "Successfully set path");
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LogInstallMessage(EventLogEntryType.Error, ex2.Message);
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

	private void UpdateServiceConfig(object sender, InstallEventArgs e)
	{
		UpdateServiceConfig();
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
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LogInstallMessage(EventLogEntryType.Error, logMsgBase + ex2.Message);
		}
	}
}

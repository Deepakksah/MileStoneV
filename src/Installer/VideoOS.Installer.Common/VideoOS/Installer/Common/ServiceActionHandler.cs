using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common;

public class ServiceActionHandler
{
	private const uint SERVICE_NO_CHANGE = uint.MaxValue;

	private string _errorMessage;

	private string _serviceName;

	private ServiceActionType _serviceActionType;

	private bool _setErrorMessage;

	private SearchType _searchOn;

	private bool _isAfterAction;

	private StartuptypeType _startupType;

	public string ErrorMessage => _errorMessage;

	public bool IsAfterAction => _isAfterAction;

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool ChangeServiceConfig(IntPtr hService, uint nServiceType, uint nStartType, uint nErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, [In] char[] lpDependencies, string lpServiceStartName, string lpPassword, string lpDisplayName);

	public ServiceActionHandler(string serviceName, ServiceActionType serviceActionType, bool setErrorMessage, SearchType searchOn, bool isAfterAction, StartuptypeType startupType)
	{
		_serviceName = serviceName;
		_serviceActionType = serviceActionType;
		_setErrorMessage = setErrorMessage;
		_searchOn = searchOn;
		_isAfterAction = isAfterAction;
		_startupType = startupType;
	}

	public ServiceActionHandler()
	{
	}

	public void Perform()
	{
		if (string.IsNullOrEmpty(_serviceName))
		{
			throw new ArgumentNullException("serviceName");
		}
		Perform(_serviceName, _serviceActionType, _setErrorMessage, _searchOn, _startupType);
	}

	public void Perform(string serviceName, ServiceActionType serviceActionType, bool setErrorMessage, SearchType searchOn, StartuptypeType startupType)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return;
		}
		LogManager.Write($"Start action: ServiceName='{serviceName}' ActionType='{serviceActionType}' SearchOn='{searchOn}' ExecutedOn='{IsAfterAction}' StartupType='{startupType}'");
		List<string> list = new List<string>();
		switch (searchOn)
		{
		case SearchType.Equals:
			list.Add(serviceName);
			break;
		case SearchType.Contains:
		{
			ServiceController[] services = ServiceController.GetServices();
			foreach (ServiceController serviceController in services)
			{
				if (serviceController.ServiceName.Contains(serviceName))
				{
					list.Add(serviceController.ServiceName);
				}
			}
			break;
		}
		}
		foreach (string item in list)
		{
			if (startupType != StartuptypeType.None)
			{
				ChangeServiceStartupType(item, startupType);
			}
			switch (serviceActionType)
			{
			case ServiceActionType.Restart:
				RestartService(item, setErrorMessage);
				break;
			case ServiceActionType.Start:
				StartService(item, setErrorMessage);
				break;
			case ServiceActionType.Stop:
				StopService(item, setErrorMessage);
				break;
			case ServiceActionType.Kill:
				KillService(item);
				break;
			}
		}
	}

	private void ChangeServiceStartupType(string serviceName, StartuptypeType startupType)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		ServiceStartMode serviceStartMode = ServiceStartMode.Automatic;
		switch (startupType)
		{
		case StartuptypeType.Automatic:
			serviceStartMode = ServiceStartMode.Automatic;
			break;
		case StartuptypeType.Disabled:
			serviceStartMode = ServiceStartMode.Disabled;
			break;
		case StartuptypeType.Manual:
			serviceStartMode = ServiceStartMode.Manual;
			break;
		case StartuptypeType.AutomaticDelayed:
			serviceStartMode = ServiceStartMode.Automatic;
			break;
		}
		using SafeHandle safeHandle = serviceController.ServiceHandle;
		if (!ChangeServiceConfig(safeHandle.DangerousGetHandle(), uint.MaxValue, (uint)serviceStartMode, uint.MaxValue, null, null, IntPtr.Zero, null, null, null, null))
		{
			LogManager.Write($"Could not change service '{serviceName}' start type to '{serviceStartMode}'");
			LogManager.Write(new Win32Exception(Marshal.GetLastWin32Error()));
		}
	}

	private void RestartService(string serviceName, bool setErrorMessage)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!StopService(serviceName, setErrorMessage) && setErrorMessage && _errorMessage != null)
		{
			stringBuilder.AppendLine(_errorMessage);
			_errorMessage = null;
		}
		if (!StartService(serviceName, setErrorMessage) && setErrorMessage && _errorMessage != null)
		{
			stringBuilder.AppendLine(_errorMessage);
		}
		if (stringBuilder != null && stringBuilder.Length != 0)
		{
			_errorMessage = stringBuilder.ToString();
		}
	}

	private void KillService(string serviceName)
	{
		foreach (ManagementObject item in new ManagementObjectSearcher($"SELECT ProcessId FROM Win32_Service WHERE Name='{serviceName}'").Get())
		{
			uint num = (uint)item["ProcessId"];
			Process process = null;
			try
			{
				process = Process.GetProcessById((int)num);
			}
			catch (ArgumentException)
			{
				LogManager.Write($"Process specified by {num} id is no longer running");
			}
			try
			{
				if (process != null)
				{
					process.Kill();
					LogManager.Write($"Process {process.ProcessName} is killed");
				}
			}
			catch (Win32Exception exception)
			{
				LogManager.Write(exception);
			}
			catch (InvalidOperationException exception2)
			{
				LogManager.Write(exception2);
			}
		}
	}

	private bool StopService(string serviceName, bool setErrorMessage)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			if (serviceController.Status == ServiceControllerStatus.Stopped)
			{
				LogManager.Write($"Service {serviceName} is already stopped");
				return true;
			}
			if (serviceController.Status == ServiceControllerStatus.StopPending)
			{
				LogManager.Write($"Service {serviceName} is stopping");
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 0, 250));
				return true;
			}
			if (!serviceController.CanStop)
			{
				serviceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 0, 250));
			}
			serviceController.Stop();
			LogManager.Write($"Service {serviceName} stop called");
			return serviceController.Status == ServiceControllerStatus.Stopped || serviceController.Status == ServiceControllerStatus.StopPending;
		}
		catch (InvalidOperationException ex)
		{
			string errorMessage = string.Format(Resources.Action_ErrorStopService, serviceName, ex.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage;
			}
			LogManager.Write(ex);
			return false;
		}
		catch (System.ServiceProcess.TimeoutException ex2)
		{
			string errorMessage2 = string.Format(Resources.Action_ErrorStopService, serviceName, ex2.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage2;
			}
			LogManager.Write(ex2);
			return false;
		}
		catch (Exception ex3)
		{
			string errorMessage3 = string.Format(Resources.Action_ErrorStopService, serviceName, ex3.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage3;
			}
			LogManager.Write(ex3);
			return true;
		}
	}

	private bool StartService(string serviceName, bool setErrorMessage)
	{
		ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			if (serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending)
			{
				LogManager.Write($"Service {serviceName} is already running");
				return true;
			}
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 0, 250));
			serviceController.Start();
			try
			{
				serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1.0));
			}
			catch (System.ServiceProcess.TimeoutException)
			{
			}
			LogManager.Write($"Starting service {serviceName} called, result: {serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending}");
		}
		catch (InvalidOperationException ex2)
		{
			string errorMessage = string.Format(Resources.Action_ErorrStartService, serviceName, ex2.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage;
			}
			LogManager.Write(ex2);
		}
		catch (System.ServiceProcess.TimeoutException ex3)
		{
			string errorMessage2 = string.Format(Resources.Action_ErorrStartService, serviceName, ex3.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage2;
			}
			LogManager.Write(ex3);
		}
		catch (Exception ex4)
		{
			string errorMessage3 = string.Format(Resources.Action_ErorrStartService, serviceName, ex4.Message);
			if (setErrorMessage)
			{
				_errorMessage = errorMessage3;
			}
			LogManager.Write(ex4);
		}
		if (serviceController != null)
		{
			if (serviceController.Status != ServiceControllerStatus.Running)
			{
				return serviceController.Status == ServiceControllerStatus.StartPending;
			}
			return true;
		}
		return false;
	}
}

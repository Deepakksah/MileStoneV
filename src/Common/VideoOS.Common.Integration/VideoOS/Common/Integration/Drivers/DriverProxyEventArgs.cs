using System;

namespace VideoOS.Common.Integration.Drivers;

public class DriverProxyEventArgs : EventArgs
{
	public string DriverDllName { get; private set; }

	public string ProxyServerName { get; private set; }

	public string ServerName { get; private set; }

	public string NativeErrorMessage { get; private set; }

	public string ManagedExceptionSummary { get; private set; }

	public DriverProxyEventArgs(string driverDllName, string proxyServerName, string serverName, string nativeErrorMessage, string managedExceptionSummary)
	{
		DriverDllName = driverDllName;
		ProxyServerName = proxyServerName;
		ServerName = serverName;
		NativeErrorMessage = nativeErrorMessage;
		ManagedExceptionSummary = managedExceptionSummary;
	}
}

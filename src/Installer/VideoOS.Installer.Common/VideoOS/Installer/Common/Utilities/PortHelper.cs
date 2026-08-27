using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class PortHelper
{
	private enum TCP_TABLE_CLASS
	{
		TCP_TABLE_BASIC_LISTENER,
		TCP_TABLE_BASIC_CONNECTIONS,
		TCP_TABLE_BASIC_ALL,
		TCP_TABLE_OWNER_PID_LISTENER,
		TCP_TABLE_OWNER_PID_CONNECTIONS,
		TCP_TABLE_OWNER_PID_ALL,
		TCP_TABLE_OWNER_MODULE_LISTENER,
		TCP_TABLE_OWNER_MODULE_CONNECTIONS,
		TCP_TABLE_OWNER_MODULE_ALL
	}

	private struct MIB_TCPROW_EX
	{
		public TcpState dwState;

		public uint dwLocalAddr;

		public int dwLocalPort;

		public uint dwRemoteAddr;

		public int dwRemotePort;

		public int dwProcessId;
	}

	[DllImport("iphlpapi.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern int GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool bOrder, int dwFamily, TCP_TABLE_CLASS dwClass, int dwReserved);

	public static bool IsPortAvailable(int port, out string processName)
	{
		bool flag = true;
		processName = null;
		IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
		TcpConnectionInformation[] activeTcpConnections = iPGlobalProperties.GetActiveTcpConnections();
		foreach (TcpConnectionInformation tcpConnectionInformation in activeTcpConnections)
		{
			if (tcpConnectionInformation.LocalEndPoint.Port == port && (tcpConnectionInformation.State == TcpState.Established || tcpConnectionInformation.State == TcpState.Listen))
			{
				processName = WhoIsUsingPort(port);
				flag = false;
				LogManager.Write($"Port {port} is in use");
				LogManager.Write($"LocalEndPoint: {tcpConnectionInformation.LocalEndPoint} RemoteEndPoint: {tcpConnectionInformation.RemoteEndPoint} State: {tcpConnectionInformation.State}");
				break;
			}
		}
		if (flag)
		{
			IPEndPoint[] activeTcpListeners = iPGlobalProperties.GetActiveTcpListeners();
			foreach (IPEndPoint iPEndPoint in activeTcpListeners)
			{
				if (iPEndPoint.Port == port)
				{
					processName = WhoIsUsingPort(port);
					flag = false;
					LogManager.Write($"Port {port} is in use");
					LogManager.Write($"Address: {iPEndPoint.Address}");
					break;
				}
			}
		}
		return flag;
	}

	private static string WhoIsUsingPort(int port)
	{
		int processIdTcp;
		try
		{
			processIdTcp = GetProcessIdTcp(port);
			if (processIdTcp == -1)
			{
				LogManager.Write("Process was NOT found!!!!!! ");
				return null;
			}
		}
		catch (Win32Exception exception)
		{
			LogManager.Write(exception);
			return null;
		}
		return Process.GetProcessById(processIdTcp).ProcessName;
	}

	private static int GetProcessIdTcp(int port)
	{
		int result = -1;
		IntPtr intPtr = IntPtr.Zero;
		int dwOutBufLen = 0;
		int extendedTcpTable = GetExtendedTcpTable(intPtr, ref dwOutBufLen, bOrder: true, 2, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
		try
		{
			intPtr = Marshal.AllocHGlobal(dwOutBufLen);
			extendedTcpTable = GetExtendedTcpTable(intPtr, ref dwOutBufLen, bOrder: true, 2, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
			if (extendedTcpTable == 0)
			{
				int num = Marshal.ReadInt32(intPtr);
				IntPtr ptr = new IntPtr(intPtr.ToInt64() + 4);
				for (int i = 0; i <= num - 1; i++)
				{
					MIB_TCPROW_EX mIB_TCPROW_EX = (MIB_TCPROW_EX)Marshal.PtrToStructure(ptr, typeof(MIB_TCPROW_EX));
					byte[] bytes = BitConverter.GetBytes(mIB_TCPROW_EX.dwLocalPort);
					int num2 = bytes[0] * 256 + bytes[1];
					if (port == num2)
					{
						result = mIB_TCPROW_EX.dwProcessId;
					}
					ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(MIB_TCPROW_EX)));
				}
				return result;
			}
			throw new Win32Exception(extendedTcpTable);
		}
		catch
		{
			throw new Win32Exception(extendedTcpTable);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}
}

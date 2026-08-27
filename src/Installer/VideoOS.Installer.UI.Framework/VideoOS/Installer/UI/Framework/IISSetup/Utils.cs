using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public static class Utils
{
	public const int NO_ERROR = 0;

	public const int MIB_TCP_STATE_CLOSED = 1;

	public const int MIB_TCP_STATE_LISTEN = 2;

	public const int MIB_TCP_STATE_SYN_SENT = 3;

	public const int MIB_TCP_STATE_SYN_RCVD = 4;

	public const int MIB_TCP_STATE_ESTAB = 5;

	public const int MIB_TCP_STATE_FIN_WAIT1 = 6;

	public const int MIB_TCP_STATE_FIN_WAIT2 = 7;

	public const int MIB_TCP_STATE_CLOSE_WAIT = 8;

	public const int MIB_TCP_STATE_CLOSING = 9;

	public const int MIB_TCP_STATE_LAST_ACK = 10;

	public const int MIB_TCP_STATE_TIME_WAIT = 11;

	public const int MIB_TCP_STATE_DELETE_TCB = 12;

	public static ushort ConvertPort(uint dwPort)
	{
		return BitConverter.ToUInt16(new byte[2]
		{
			byte.Parse((dwPort >> 8).ToString()),
			byte.Parse((dwPort & 0xFF).ToString())
		}, 0);
	}

	public static int BufferToInt(byte[] buffer, ref int nOffset)
	{
		int result = buffer[nOffset] + (buffer[nOffset + 1] << 8) + (buffer[nOffset + 2] << 16) + (buffer[nOffset + 3] << 24);
		nOffset += 4;
		return result;
	}

	public static string StateToStr(int state)
	{
		string result = "";
		if (state <= 0)
		{
			return result;
		}
		switch (state)
		{
		case 1:
			result = "CLOSED";
			break;
		case 2:
			result = "LISTEN";
			break;
		case 3:
			result = "SYN_SENT";
			break;
		case 4:
			result = "SYN_RCVD";
			break;
		case 5:
			result = "ESTAB";
			break;
		case 6:
			result = "FIN_WAIT1";
			break;
		case 7:
			result = "FIN_WAIT2";
			break;
		case 8:
			result = "CLOSE_WAIT";
			break;
		case 9:
			result = "CLOSING";
			break;
		case 10:
			result = "LAST_ACK";
			break;
		case 11:
			result = "TIME_WAIT";
			break;
		case 12:
			result = "DELETE_TCB";
			break;
		}
		return result;
	}

	public static IPEndPoint BufferToIPEndPoint(byte[] buffer, ref int nOffset, bool IsRemote)
	{
		long num = ((buffer[nOffset + 3] << 24) | (buffer[nOffset + 2] << 16) | (buffer[nOffset + 1] << 8) | buffer[nOffset]) & 0xFFFFFFFFu;
		nOffset += 4;
		int num2 = 0;
		num2 = ((!IsRemote || num != 0L) ? ((buffer[nOffset] << 8) + buffer[nOffset + 1] + (buffer[nOffset + 2] << 24) + (buffer[nOffset + 3] << 16)) : 0);
		nOffset += 4;
		return new IPEndPoint(num, num2);
	}

	public static string GetHostName(IPEndPoint HostAddress, string LocalHostName)
	{
		try
		{
			if (HostAddress.Address.Equals(new IPAddress(0L)))
			{
				if (HostAddress.Port > 0)
				{
					return LocalHostName + ":" + HostAddress.Port;
				}
				return "Anyone";
			}
			string hostName = IPHelper.DnsGetHostEntry(HostAddress.Address.ToString()).HostName;
			if (IPAddress.TryParse(hostName, out var address) && address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				return "[" + hostName + "]:" + HostAddress.Port;
			}
			return IPHelper.DnsGetHostEntry(HostAddress.Address.ToString()).HostName + ":" + HostAddress.Port;
		}
		catch
		{
			return HostAddress.ToString();
		}
	}

	public static string GetLocalHostName()
	{
		return IPHelper.DnsGetHostEntry("localhost").HostName;
	}

	public static int CompareIPEndPoints(IPEndPoint first, IPEndPoint second)
	{
		int num = 0;
		byte[] addressBytes = first.Address.GetAddressBytes();
		byte[] addressBytes2 = second.Address.GetAddressBytes();
		for (int i = 0; i < addressBytes.Length; i++)
		{
			num = addressBytes[i] - addressBytes2[i];
			if (num != 0)
			{
				return num;
			}
		}
		num = first.Port - second.Port;
		if (num != 0)
		{
			return num;
		}
		return 0;
	}

	public static string GetProcessNameByPID(int processID)
	{
		try
		{
			return Process.GetProcessById(processID).ProcessName;
		}
		catch (Exception)
		{
			return "Unknown";
		}
	}

	public static string GetProcessMainWindowTitleByPID(int processID)
	{
		try
		{
			return Process.GetProcessById(processID).MainWindowTitle;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static string GetProcessMainModuleFileNameByPID(int processID)
	{
		try
		{
			return Process.GetProcessById(processID).MainModule.FileName;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}
}

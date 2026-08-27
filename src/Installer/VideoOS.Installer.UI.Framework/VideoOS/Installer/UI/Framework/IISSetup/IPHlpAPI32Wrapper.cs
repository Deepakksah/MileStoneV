using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public class IPHlpAPI32Wrapper
{
	public const byte NO_ERROR = 0;

	public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

	public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

	public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

	public int dwFlags = 4864;

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetUdpStatistics(ref MIB_UDPSTATS pStats);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetUdpTable(byte[] UcpTable, out int pdwSize, bool bOrder);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetTcpStatistics(ref MIB_TCPSTATS pStats);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int AllocateAndGetTcpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int AllocateAndGetUdpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

	[DllImport("kernel32", SetLastError = true)]
	public static extern IntPtr GetProcessHeap();

	[DllImport("kernel32", SetLastError = true)]
	private static extern int FormatMessage(int flags, IntPtr source, int messageId, int languageId, StringBuilder buffer, int size, IntPtr arguments);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetExtendedTcpTable(byte[] pTcpTable, out int dwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, int reserved);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	public static extern int GetExtendedUdpTable(byte[] pUdpTable, out int dwOutBufLen, bool sort, int ipVersion, UDP_TABLE_CLASS tblClass, int reserved);

	public static string GetAPIErrorMessageDescription(int ApiErrNumber)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		if (FormatMessage(4096, (IntPtr)0, ApiErrNumber, 0, stringBuilder, stringBuilder.Capacity, (IntPtr)0) > 0)
		{
			string text = stringBuilder.ToString();
			text = text.Substring(0, text.Length - 2);
			return text + " (" + ApiErrNumber + ")";
		}
		return "none";
	}
}

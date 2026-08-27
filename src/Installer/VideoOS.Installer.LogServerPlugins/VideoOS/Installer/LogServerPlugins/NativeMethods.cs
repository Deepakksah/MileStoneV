using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Installer.LogServerPlugins;

public static class NativeMethods
{
	public enum ComputerNameFormat
	{
		ComputerNameNetBIOS,
		ComputerNameDnsHostname,
		ComputerNameDnsDomain,
		ComputerNameDnsFullyQualified,
		ComputerNamePhysicalNetBIOS,
		ComputerNamePhysicalDnsHostname,
		ComputerNamePhysicalDnsDomain,
		ComputerNamePhysicalDnsFullyQualified,
		ComputerNameMax
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetComputerNameExW", SetLastError = true)]
	public static extern int GetComputerNameEx(ComputerNameFormat NameType, [Out] StringBuilder lpBuffer, ref uint lpnSize);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "DnsHostnameToComputerNameW", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DnsHostnameToComputerName(string Hostname, StringBuilder ComputerName, ref int nSize);

	public static string GetComputerNameEx(ComputerNameFormat format)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		uint lpnSize = (uint)stringBuilder.Capacity;
		if (GetComputerNameEx(format, stringBuilder, ref lpnSize) != 0)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	public static string DnsHostnameToComputerName(string hostname)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		int nSize = stringBuilder.Capacity;
		if (DnsHostnameToComputerName(hostname, stringBuilder, ref nSize))
		{
			return stringBuilder.ToString();
		}
		return null;
	}
}
